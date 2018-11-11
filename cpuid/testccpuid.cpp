////////////////////////////////////////////////////////////
// testccpuid.cpp : 测试ccpuid.h, 显示所有的CPUID信息.
// Author: zyl910
// Blog: http://www.cnblogs.com/zyl910
// URL: http://www.cnblogs.com/zyl910/archive/2012/08/22/ccpuid_v101.html
// Version: V1.01
// Updata: 2012-08-22
//
// Update
// ~~~~~~
//
// [2012-08-22] V1.01
// 兼容GCC.
//
// [2012-07-11] V1.00
// V1.0发布.
//
////////////////////////////////////////////////////////////

#include <stdio.h>

#include "ccpuid.h"

bool bShowDesc = true;	// 显示描述信息
FILE *fp;
// 获取程序位数（被编译为多少位的代码）
inline int GetProgramBits()
{
	return sizeof(int*) * 8;
}

// 打印CPUID字段_某项.
inline void prtCcpuid_Item(uint32_t fid, uint32_t fidsub, const uint32_t CPUInfo[4])
{
	static const char* RegName[4] = { "EAX", "EBX", "ECX", "EDX" };
	uint32_t mask = CPUIDFIELD_MASK_FID | CPUIDFIELD_MASK_FIDSUB;
	uint32_t cur = CPUIDFIELD_MAKE(fid, fidsub, 0, 0, 1) & mask;
	int i;
	for (i = 0; i < CCPUID::CPUFDescLen; ++i)
	{
		const CPUIDFIELDDESC& v = CCPUID::CPUFDesc[i];
		if ((v.cpuf&mask) == cur)
		{
			CPUIDFIELD f = v.cpuf;
			uint32_t bits = CPUIDFIELD_LEN(f);
			uint32_t pos = CPUIDFIELD_POS(f);
			uint32_t reg = CPUIDFIELD_REG(f);
			uint32_t n = getcpuidfield_buf(CPUInfo, f);	//UINT32 n = __GETBITS32(CPUInfo[reg], pos, bits);
			if (bits > 1)
			{
				fprintf(fp, "\t%s[%2d:%2d]", RegName[reg], pos + bits - 1, pos);
			}
			else
			{
				fprintf(fp, "\t%s[   %2d]", RegName[reg], pos);
			}
			fprintf(fp, "=%s:\t0x%X\t(%u)", v.szName, n, n);
			if (bShowDesc)
			{
				fprintf(fp, "\t// %s", v.szDesc);
			}
			fprintf(fp, "\n");
		}
	}
}

// 打印CPUID字段.
void prtCcpuid(const CCPUID& ccid)
{
	int i;
	for (i = 0; i < ccid.InfoCount(); ++i)
	{
		const CPUIDINFO& v = ccid.Info[i];
		fprintf(fp, "0x%.8X[%d]:\t%.8X\t%.8X\t%.8X\t%.8X\n", v.fid, v.fidsub, v.dw[0], v.dw[1], v.dw[2], v.dw[3]);
		// 检查子功能号. 如果是规范的子功能号，便故意设为0，根据子功能号0的字段来解析各个子功能号的信息。
		uint32_t fidsub = v.fidsub;
		switch (v.fid)
		{
		case 0x4: fidsub = 0;
		case 0xB: fidsub = 0;
		case 0x8000001D: fidsub = 0;
		}
		// item
		prtCcpuid_Item(v.fid, fidsub, v.dw);
		// otheritem
		if (0 == v.fid)	// Vendor-ID (Function 02h)
		{
			fprintf(fp, "\tVendor:\t%s\n", ccid.Vendor());
		}
		else if (0x80000004 == v.fid)	// Processor Brand String (Function 80000002h,80000003h,80000004h)
		{
			fprintf(fp, "\tBrand:\t%s\n", ccid.Brand());
		}
		else if (0x2 == v.fid)	// Cache Descriptors (Function 02h)
		{
			for (int j = 0; j <= 3; ++j)
			{
				uint32_t n = v.dw[j];
				if (n > 0)	// 最高位为0，且不是全0
				{
					for (int k = 0; k <= 3; ++k)
					{
						if (j > 0 || k > 0)	// EAX的低8位不是缓存信息
						{
							int by = n & 0x00FF;
							if (by > 0)
							{
								fprintf(fp, "\t0x%.2X:\t%s\n", by, CCPUID::CacheDesc[by]);
							}
						}
						n >>= 8;
					}
				}
			}
		}
	}
}

int main(int argc, char* argv[])
{
	int i;
	fp = fopen(argv[1], "w");
	//CCPUID ccid;
	//ccid.RefreshAll();
	CCPUID& ccid = CCPUID::cur();
	// base info
	fprintf(fp, "CCPUID.InfoCount:\t%d\n", ccid.InfoCount());
	fprintf(fp, "CCPUID.LFuncStd:\t%.8Xh\n", ccid.LFuncStd());
	fprintf(fp, "CCPUID.LFuncExt:\t%.8Xh\n", ccid.LFuncExt());
	fprintf(fp, "CCPUID.Vendor:\t%s\n", ccid.Vendor());
	//fprintf(fp,"CCPUID.Brand:\t%s\n", ccid.Brand());
	fprintf(fp, "CCPUID.BrandTrim:\t%s\n", ccid.BrandTrim());

	// simd info
	fprintf(fp, "CCPUID.MMX:\t%d\t// hw: %d\n", ccid.mmx(), ccid.hwmmx());
	fprintf(fp, "CCPUID.SSE:\t%d\t// hw: %d\n", ccid.sse(), ccid.hwsse());
	for (i = 1; i < (int)(sizeof(CCPUID::SseNames) / sizeof(CCPUID::SseNames[0])); ++i)
	{
		if (ccid.hwsse() >= i)	fprintf(fp, "\t%s\n", CCPUID::SseNames[i]);
	}
	fprintf(fp, "SSE4A:\t%d\n", ccid.GetField(CPUF_SSE4A));
	fprintf(fp, "AES:\t%d\n", ccid.GetField(CPUF_AES));
	fprintf(fp, "PCLMULQDQ:\t%d\n", ccid.GetField(CPUF_PCLMULQDQ));
	fprintf(fp, "CCPUID.AVX:\t%d\t// hw: %d\n", ccid.avx(), ccid.hwavx());
	for (i = 1; i < (int)(sizeof(CCPUID::AvxNames) / sizeof(CCPUID::AvxNames[0])); ++i)
	{
		if (ccid.hwavx() >= i)	fprintf(fp, "\t%s\n", CCPUID::AvxNames[i]);
	}
	fprintf(fp, "F16C:\t%d\n", ccid.GetField(CPUF_F16C));
	fprintf(fp, "FMA:\t%d\n", ccid.GetField(CPUF_FMA));
	fprintf(fp, "FMA4:\t%d\n", ccid.GetField(CPUF_FMA4));
	fprintf(fp, "XOP:\t%d\n", ccid.GetField(CPUF_XOP));

	// field info
	fprintf(fp, "== fields ==\n");
	prtCcpuid(ccid);

	return 0;
}