////////////////////////////////////////////////////////////
// ccpuid.cpp: CPUID信息.
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
// 使用getcpuid/getcpuidex函数来获取CPUID信息
// 使用C99标准整数类型uint32_t, 提高可移植性.
//
// [2012-07-11] V1.00
// V1.0发布.
//
////////////////////////////////////////////////////////////

#include "ccpuid.h"

CCPUID CCPUID::_cur;
#include "desc.h"


const char*	CCPUID::CacheDesc[] = {
	"Null descriptor, this byte contains no information"
	,"Instruction TLB: 4 KByte pages, 4-way set associative, 32 entries"
	,"Instruction TLB: 4 MByte pages, fully associative, 2 entries"
	,"Data TLB: 4 KByte pages, 4-way set associative, 64 entries"
	,"Data TLB: 4 MByte pages, 4-way set associative, 8 entries"
	,"Data TLB1: 4 MByte pages, 4-way set associative, 32 entries"
	,"1st-level instruction cache: 8 KBytes, 4-way set associative, 32 byte line size"
	,""
	,"1st-level instruction cache: 16 KBytes, 4-way set associative, 32 byte line size"
	,"1st-level instruction cache: 32KBytes, 4-way set associative, 64 byte line size"
	,"1st-level data cache: 8 KBytes, 2-way set associative, 32 byte line size"
	,"Instruction TLB: 4 MByte pages, 4-way set associative, 4 entries"
	,"1st-level data cache: 16 KBytes, 4-way set associative, 32 byte line size"
	,"1st-level data cache: 16 KBytes, 4-way set associative, 64 byte line size"
	,"1st-level data cache: 24 KBytes, 6-way set associative, 64 byte line size"
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,"2nd-level cache: 256 KBytes, 8-way set associative, 64 byte line size"
	,"3rd-level cache: 512 KBytes, 4-way set associative, 64 byte line size, 2 lines per sector"
	,"3rd-level cache: 1 MBytes, 8-way set associative, 64 byte line size, 2 lines per sector"
	,""
	,"3rd-level cache: 2 MBytes, 8-way set associative, 64 byte line size, 2 lines per sector"
	,""
	,""
	,""
	,"3rd-level cache: 4 MBytes, 8-way set associative, 64 byte line size, 2 lines per sector"
	,""
	,""
	,"1st-level data cache: 32 KBytes, 8-way set associative, 64 byte line size"
	,""
	,""
	,""
	,"1st-level instruction cache: 32 KBytes, 8-way set associative, 64 byte line size"
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,"No 2nd-level cache or, if processor contains a valid 2nd-level cache, no 3rd-level cache"
	,"2nd-level cache: 128 KBytes, 4-way set associative, 32 byte line size"
	,"2nd-level cache: 256 KBytes, 4-way set associative, 32 byte line size"
	,"2nd-level cache: 512 KBytes, 4-way set associative, 32 byte line size"
	,"2nd-level cache: 1 MByte, 4-way set associative, 32 byte line size"
	,"2nd-level cache: 2 MByte, 4-way set associative, 32 byte line size"
	,"3rd-level cache: 4 MByte, 4-way set associative, 64 byte line size"
	,"3rd-level cache: 8 MByte, 8-way set associative, 64 byte line size"
	,"2nd-level cache: 3MByte, 12-way set associative, 64 byte line size"
	,"3rd-level cache: 4MB, 16-way set associative, 64-byte line size (Intel Xeon processor MP, Family 0FH, Model 06H); 2nd-level cache: 4 MByte, 16-way set associative, 64 byte line size"
	,"3rd-level cache: 6MByte, 12-way set associative, 64 byte line size"
	,"3rd-level cache: 8MByte, 16-way set associative, 64 byte line size"
	,"3rd-level cache: 12MByte, 12-way set associative, 64 byte line size"
	,"3rd-level cache: 16MByte, 16-way set associative, 64 byte line size"
	,"2nd-level cache: 6MByte, 24-way set associative, 64 byte line size"
	,"Instruction TLB: 4 KByte pages, 32 entries"
	,"Instruction TLB: 4 KByte and 2-MByte or 4-MByte pages, 64 entries"
	,"Instruction TLB: 4 KByte and 2-MByte or 4-MByte pages, 128 entries"
	,"Instruction TLB: 4 KByte and 2-MByte or 4-MByte pages, 256 entries"
	,""
	,""
	,"Instruction TLB: 2-MByte or 4-MByte pages, fully associative, 7 entries"
	,"Data TLB0: 4 MByte pages, 4-way set associative, 16 entries"
	,"Data TLB0: 4 KByte pages, 4-way associative, 16 entries"
	,"Data TLB0: 4 KByte pages, fully associative, 16 entries"
	,"Data TLB0: 2-MByte or 4 MByte pages, 4-way set associative, 32 entries"
	,"Data TLB: 4 KByte and 4 MByte pages, 64 entries"
	,"Data TLB: 4 KByte and 4 MByte pages,128 entries"
	,"Data TLB: 4 KByte and 4 MByte pages,256 entries"
	,""
	,""
	,""
	,"1st-level data cache: 16 KByte, 8-way set associative, 64 byte line size"
	,""
	,""
	,""
	,""
	,""
	,"1st-level data cache: 8 KByte, 4-way set associative, 64 byte line size"
	,"1st-level data cache: 16 KByte, 4-way set associative, 64 byte line size"
	,"1st-level data cache: 32 KByte, 4-way set associative, 64 byte line size"
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,"Trace cache: 12 K-μop, 8-way set associative"
	,"Trace cache: 16 K-μop, 8-way set associative"
	,"Trace cache: 32 K-μop, 8-way set associative"
	,""
	,""
	,""
	,"Instruction TLB: 2M/4M pages, fully associative, 8 entries"
	,""
	,"2nd-level cache: 1 MByte, 4-way set associative, 64byte line size"
	,"2nd-level cache: 128 KByte, 8-way set associative, 64 byte line size, 2 lines per sector"
	,"2nd-level cache: 256 KByte, 8-way set associative, 64 byte line size, 2 lines per sector"
	,"2nd-level cache: 512 KByte, 8-way set associative, 64 byte line size, 2 lines per sector"
	,"2nd-level cache: 1 MByte, 8-way set associative, 64 byte line size, 2 lines per sector"
	,"2nd-level cache: 2 MByte, 8-way set associative, 64byte line size"
	,""
	,"2nd-level cache: 512 KByte, 2-way set associative, 64-byte line size"
	,"2nd-level cache: 512 KByte, 8-way set associative, 64-byte line size"
	,""
	,"2nd-level cache: 256 KByte, 8-way set associative, 32 byte line size"
	,"2nd-level cache: 512 KByte, 8-way set associative, 32 byte line size"
	,"2nd-level cache: 1 MByte, 8-way set associative, 32 byte line size"
	,"2nd-level cache: 2 MByte, 8-way set associative, 32 byte line size"
	,"2nd-level cache: 512 KByte, 4-way set associative, 64 byte line size"
	,"2nd-level cache: 1 MByte, 8-way set associative, 64 byte line size"
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,"Instruction TLB: 4 KByte pages, 4-way set associative, 128 entries"
	,"Instruction TLB: 2M pages, 4-way, 8 entries or 4M pages, 4-way, 4 entries"
	,"Instruction TLB: 4KByte pages, 4-way set associative, 64 entries"
	,"Data TLB: 4 KByte pages, 4-way set associative, 128 entries"
	,"Data TLB1: 4 KByte pages, 4-way associative, 256 entries"
	,""
	,""
	,""
	,""
	,""
	,"Data TLB1: 4 KByte pages, 4-way associative, 64 entries"
	,""
	,""
	,""
	,""
	,""
	,"Data TLB: 4 KByte and 4 MByte pages, 4-way associative, 8 entries"
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,"Shared 2nd-Level TLB: 4 KByte pages, 4-way associative, 512 entries"
	,""
	,""
	,""
	,""
	,""
	,"3rd-level cache: 512 KByte, 4-way set associative, 64 byte line size"
	,"3rd-level cache: 1 MByte, 4-way set associative, 64 byte line size"
	,"3rd-level cache: 2 MByte, 4-way set associative, 64 byte line size"
	,""
	,""
	,""
	,"3rd-level cache: 1 MByte, 8-way set associative, 64 byte line size"
	,"3rd-level cache: 2 MByte, 8-way set associative, 64 byte line size"
	,"3rd-level cache: 4 MByte, 8-way set associative, 64 byte line size"
	,""
	,""
	,""
	,"3rd-level cache: 1.5 MByte, 12-way set associative, 64 byte line size"
	,"3rd-level cache: 3 MByte, 12-way set associative, 64 byte line size"
	,"3rd-level cache: 6 MByte, 12-way set associative, 64 byte line size"
	,""
	,""
	,""
	,"3rd-level cache: 2 MByte, 16-way set associative, 64 byte line size"
	,"3rd-level cache: 4 MByte, 16-way set associative, 64 byte line size"
	,"3rd-level cache: 8 MByte, 16-way set associative, 64 byte line size"
	,""
	,""
	,""
	,""
	,""
	,"3rd-level cache: 12MByte, 24-way set associative, 64 byte line size"
	,"3rd-level cache: 18MByte, 24-way set associative, 64 byte line size"
	,"3rd-level cache: 24MByte, 24-way set associative, 64 byte line size"
	,""
	,""
	,""
	,"64-Byte prefetching"
	,"128-Byte prefetching"
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,""
	,"CPUID leaf 2 does not report cache descriptor information, use CPUID leaf 4 to query cache parameters"
};

const char*	CCPUID::SseNames[] = {
	"None",
	"SSE",
	"SSE2",
	"SSE3",
	"SSSE3",
	"SSE4.1",
	"SSE4.2",
};

const char*	CCPUID::AvxNames[] = {
	"None",
	"AVX",
	"AVX2",
	"AVX512"
};

////////////////////////////////////////////////////////////
// functions
////////////////////////////////////////////////////////////

// 取得CPU厂商（Vendor）.
//
// result: 成功时返回字符串的长度（一般为12）。失败时返回0.
// pvendor: 接收厂商信息的字符串缓冲区。至少为13字节.
int cpu_getvendor(char* pvendor)
{
	uint32_t dwBuf[4];
	if (NULL == pvendor)	return 0;
	// Function 0: Vendor-ID and Largest Standard Function
	getcpuid(dwBuf, 0);
	// save. 保存到pvendor
	*(uint32_t *)&pvendor[0] = dwBuf[1];	// ebx: 前四个字符.
	*(uint32_t *)&pvendor[4] = dwBuf[3];	// edx: 中间四个字符.
	*(uint32_t *)&pvendor[8] = dwBuf[2];	// ecx: 最后四个字符.
	pvendor[12] = '\0';
	return 12;
}

// 取得CPU商标（Brand）.
//
// result: 成功时返回字符串的长度（一般为48）。失败时返回0.
// pbrand: 接收商标信息的字符串缓冲区。至少为49字节.
int cpu_getbrand(char* pbrand)
{
	uint32_t dwBuf[4];
	if (NULL == pbrand)	return 0;
	// Function 0x80000000: Largest Extended Function Number
	getcpuid(dwBuf, 0x80000000U);
	if (dwBuf[0] < 0x80000004U)	return 0;
	// Function 80000002h,80000003h,80000004h: Processor Brand String
	getcpuid((uint32_t *)&pbrand[0], 0x80000002U);	// 前16个字符.
	getcpuid((uint32_t *)&pbrand[16], 0x80000003U);	// 中间16个字符.
	getcpuid((uint32_t *)&pbrand[32], 0x80000004U);	// 最后16个字符.
	pbrand[48] = '\0';
	return 48;
}

// 是否支持MMX指令集.
//
// result: 返回操作系统是否支持MMX指令集. 非0表示支持, 0表示不支持.
// phwmmx: 返回硬件是否支持MMX指令集. 非0表示支持, 0表示不支持.
int	simd_mmx(int* phwmmx)
{
	const uint32_t	BIT_D_MMX = 0x00800000;	// bit 23
	int	rt = 0;	// result
	uint32_t dwBuf[4];

	// check processor support
	getcpuid(dwBuf, 1);	// Function 1: Feature Information
	if (dwBuf[3] & BIT_D_MMX)	rt = 1;
	if (NULL != phwmmx)	*phwmmx = rt;

	// check OS support
	if (rt)
	{
#if defined(_WIN64) && defined(_MSC_VER) && !defined(__INTEL_COMPILER)
		// VC编译器不支持64位下的MMX.
		rt = 0;
#else
		try
		{
			_mm_empty();	// MMX instruction: emms
		}
		catch (...)
		{
			rt = 0;
		}
#endif	// #if defined(_WIN64) && defined(_MSC_VER)
	}

	return rt;
}

// 检测SSE系列指令集的支持级别.
//
// result: 返回操作系统的SSE系列指令集支持级别. 详见SIMD_SSE_常数.
// phwmmx: 返回硬件的SSE系列指令集支持级别. 详见SIMD_SSE_常数.
int	simd_sse_level(int* phwsse)
{
	const uint32_t	BIT_D_SSE = 0x02000000;	// bit 25
	const uint32_t	BIT_D_SSE2 = 0x04000000;	// bit 26
	const uint32_t	BIT_C_SSE3 = 0x00000001;	// bit 0
	const uint32_t	BIT_C_SSSE3 = 0x00000100;	// bit 9
	const uint32_t	BIT_C_SSE41 = 0x00080000;	// bit 19
	const uint32_t	BIT_C_SSE42 = 0x00100000;	// bit 20
	int	rt = SIMD_SSE_NONE;	// result
	uint32_t dwBuf[4];

	// check processor support
	getcpuid(dwBuf, 1);	// Function 1: Feature Information
	if (dwBuf[3] & BIT_D_SSE)
	{
		rt = SIMD_SSE_1;
		if (dwBuf[3] & BIT_D_SSE2)
		{
			rt = SIMD_SSE_2;
			if (dwBuf[2] & BIT_C_SSE3)
			{
				rt = SIMD_SSE_3;
				if (dwBuf[2] & BIT_C_SSSE3)
				{
					rt = SIMD_SSE_3S;
					if (dwBuf[2] & BIT_C_SSE41)
					{
						rt = SIMD_SSE_41;
						if (dwBuf[2] & BIT_C_SSE42)
						{
							rt = SIMD_SSE_42;
						}
					}
				}
			}
		}
	}
	if (NULL != phwsse)	*phwsse = rt;

	// check OS support
	try
	{
		__m128 xmm1 = _mm_setzero_ps();	// SSE instruction: xorps
		int* pxmm1 = (int*)&xmm1;	// 避免GCC的 -Wstrict-aliasing 警告.
		if (0 != *pxmm1)	rt = SIMD_SSE_NONE;	// 避免Release模式编译优化时剔除_mm_setzero_ps.
	}
	catch (...)
	{
		rt = SIMD_SSE_NONE;
	}

	return rt;
}

// 检测AVX系列指令集的支持级别.
//
// result: 返回操作系统的AVX系列指令集支持级别. 详见SIMD_AVX_常数.
// phwavx: 返回硬件的AVX系列指令集支持级别. 详见SIMD_AVX_常数.
int	simd_avx_level(int* phwavx)
{
	int	rt = SIMD_AVX_NONE;	// result

	// check processor support
	if (0 != getcpuidfield(CPUF_AVX))
	{
		rt = SIMD_AVX_1;
		if (0 != getcpuidfield(CPUF_AVX2))
		{
			rt = SIMD_AVX_2;
		}
		if (0 != getcpuidfield(CPUF_AVX512))
		{
			rt = SIMD_AVX_512;
		}
	}
	if (NULL != phwavx)	*phwavx = rt;

	// check OS support
	if (0 != getcpuidfield(CPUF_OSXSAVE))	// XGETBV enabled for application use.
	{
		uint32_t n = getcpuidfield(CPUF_XFeatureSupportedMaskLo);	// XCR0: XFeatureSupportedMask register.
		if (6 == (n & 6))	// XCR0[2:1] = ‘11b’ (XMM state and YMM state are enabled by OS).
		{
			return rt;
		}
	}
	return SIMD_AVX_NONE;
}

////////////////////////////////////////////////////////////
// CCPUID
////////////////////////////////////////////////////////////

// 构造函数.
CCPUID::CCPUID()
	:_InfoCount(0), _LFuncStd(0), _LFuncExt(0), _BrandTrim(0)
{
	_Vendor[0] = '\0';
	_Brand[0] = '\0';
}

// 刷新信息.在Info数组中追加一项.
void CCPUID::RefreshInfo_Put(uint32_t fid, uint32_t fidsub, uint32_t CPUInfo[4])
{
	if (_InfoCount >= MAX_CPUIDINFO)	return;
	Info[_InfoCount].fid = fid;
	Info[_InfoCount].fidsub = fidsub;
	Info[_InfoCount].dw[0] = CPUInfo[0];
	Info[_InfoCount].dw[1] = CPUInfo[1];
	Info[_InfoCount].dw[2] = CPUInfo[2];
	Info[_InfoCount].dw[3] = CPUInfo[3];
	++_InfoCount;
}

// 刷新信息.
void CCPUID::RefreshInfo()
{
	uint32_t CPUInfo[4];
	uint32_t nmax;
	uint32_t i, j;

	// == 将CPUID信息保存到Info数组 ==
	_InfoCount = 0;

	// 标准功能.
	getcpuid(CPUInfo, 0);
	RefreshInfo_Put(0, 0, CPUInfo);
	nmax = CPUInfo[0];	// CPUID(0).EAX[31:0]=LFuncStd
	_LFuncStd = nmax;
	for (i = 1; i <= nmax; ++i)
	{
		getcpuidex(CPUInfo, i, 0);
		RefreshInfo_Put(i, 0, CPUInfo);
		// fidsub
		if (0x4 == i)	// Deterministic Cache Parameters (Function 04h)
		{
			j = 1;
			while (true)
			{
				getcpuidex(CPUInfo, i, j);
				if (0 == (CPUInfo[0] & 0x1F))	break;	// EAX[4:0]=Cache_Type
				RefreshInfo_Put(i, j, CPUInfo);
				// next
				++j;
			}
		}
		else if (0xB == i)	// x2APIC Features / Processor Topology (Function 0Bh)
		{
			j = 1;
			while (true)
			{
				getcpuidex(CPUInfo, i, j);
				if (0 == CPUInfo[0] && 0 == CPUInfo[1])	break;	// until EAX=0 and EBX=0
				RefreshInfo_Put(i, j, CPUInfo);
				// next
				++j;
			}
		}
		else if (0xD == i)	// XSAVE Features (Function 0Dh)
		{
			// fidsub = 1
			j = 1;
			getcpuidex(CPUInfo, i, j);
			RefreshInfo_Put(i, j, CPUInfo);
			// fidsub >= 2
			for (j = 2; j <= 63; ++j)
			{
				getcpuidex(CPUInfo, i, j);
				if (0 != CPUInfo[0]
					|| 0 != CPUInfo[1]
					|| 0 != CPUInfo[2]
					|| 0 != CPUInfo[3])
				{
					RefreshInfo_Put(i, j, CPUInfo);
				}
			}
		}
	}

	// 扩展功能.
	getcpuid(CPUInfo, 0x80000000U);
	RefreshInfo_Put(0x80000000U, 0, CPUInfo);
	nmax = CPUInfo[0];	// CPUID(0x80000000).EAX[31:0]=LFuncExt
	_LFuncExt = nmax;
	if (nmax != 0)
	{
		for (i = 0x80000001U; i <= nmax; ++i)
		{
			getcpuidex(CPUInfo, i, 0);
			RefreshInfo_Put(i, 0, CPUInfo);
			// fidsub
			if (0x8000001DU == i)	// Cache Properties (Function 8000001Dh)
			{
				j = 1;
				while (true)
				{
					getcpuidex(CPUInfo, i, j);
					if (0 == (CPUInfo[0] & 0x1F))	break;	// EAX[4:0]=Cache_Type
					RefreshInfo_Put(i, j, CPUInfo);
					// next
					++j;
				}
			}
		}
	}
}

// 刷新属性.
void CCPUID::RefreshProperty()
{
	uint32_t dwBuf[4];

	// Vendor
	GetData(dwBuf, 0);	// Function 0: Vendor-ID and Largest Standard Function
	uint32_t* pVendor = (uint32_t *)_Vendor;	// 避免GCC的 -Wstrict-aliasing 警告.
	pVendor[0] = dwBuf[1];	// ebx: 前四个字符.
	pVendor[1] = dwBuf[3];	// edx: 中间四个字符.
	pVendor[2] = dwBuf[2];	// ecx: 最后四个字符.
	_Vendor[12] = '\0';

	// Brand
	_Brand[0] = '\0';
	if (_LFuncExt >= 0x80000004)
	{
		// Function 80000002h,80000003h,80000004h: Processor Brand String
		GetData((uint32_t *)&_Brand[0], 0x80000002);	// 前16个字符.
		GetData((uint32_t *)&_Brand[16], 0x80000003);	// 中间16个字符.
		GetData((uint32_t *)&_Brand[32], 0x80000004);	// 最后16个字符.
		_Brand[48] = '\0';
	}
	_BrandTrim = &_Brand[0];
	while ('\0' != *_BrandTrim && ' ' == *_BrandTrim) ++_BrandTrim;	// 去除首都空格.

	// SIMD
	_mmx = simd_mmx(&_hwmmx);
	_sse = simd_sse_level(&_hwsse);
	_avx = simd_avx_level(&_hwavx);
}

// 刷新所有.
void CCPUID::RefreshAll()
{
	RefreshInfo();
	RefreshProperty();
}

// 取得信息.
//
// return: 成功时返回LPCPUIDINFO. 如果失败, 便返回NULL.
// InfoType: 功能号. 即CPUID指令的eax参数.
// ECXValue: 子功能号. 即CPUID指令的ecx参数.
LPCCPUIDINFO CCPUID::GetInfo(uint32_t InfoType, uint32_t ECXValue) const
{
	// 顺序搜索.
	for (int i = 0; i < InfoCount(); ++i)
	{
		const CPUIDINFO& v = Info[i];
		if (InfoType == v.fid && ECXValue == v.fidsub)
		{
			return &v;
		}
	}
	return NULL;
}

// 取得数据.
//
// CPUInfo: 成功时返回4个DWORD. 如果失败, 便返回全0.
// InfoType: 功能号. 即CPUID指令的eax参数.
// ECXValue: 子功能号. 即CPUID指令的ecx参数.
void CCPUID::GetData(uint32_t CPUInfo[4], uint32_t InfoType, uint32_t ECXValue) const
{
	LPCCPUIDINFO p = GetInfo(InfoType, ECXValue);
	if (NULL == p)
	{
		CPUInfo[0] = 0;
		CPUInfo[1] = 0;
		CPUInfo[2] = 0;
		CPUInfo[3] = 0;
		return;
	}
	CPUInfo[0] = p->dw[0];
	CPUInfo[1] = p->dw[1];
	CPUInfo[2] = p->dw[2];
	CPUInfo[3] = p->dw[3];
}

// 取得CPUID字段
uint32_t CCPUID::GetField(CPUIDFIELD cpuf) const
{
	LPCCPUIDINFO p = GetInfo(CPUIDFIELD_FID(cpuf), CPUIDFIELD_FIDSUB(cpuf));
	if (NULL == p)	return 0;
	return getcpuidfield_buf(p->dw, cpuf);
}

int	CCPUID::simd_mmx(int* phwmmx) const
{
	const uint32_t	BIT_D_MMX = 0x00800000;	// bit 23
	int	rt = 0;	// result
	uint32_t dwBuf[4];

	// check processor support
	GetData(dwBuf, 1);	// Function 1: Feature Information
	if (dwBuf[3] & BIT_D_MMX)	rt = 1;
	if (NULL != phwmmx)	*phwmmx = rt;

	// check OS support
	if (rt)
	{
#if defined(_WIN64) && defined(_MSC_VER) && !defined(__INTEL_COMPILER)
		// VC编译器不支持64位下的MMX.
		rt = 0;
#else
		try
		{
			_mm_empty();	// MMX instruction: emms
		}
		catch (...)
		{
			rt = 0;
		}
#endif	// #if defined(_WIN64) && defined(_MSC_VER)
	}

	return rt;
}

int	CCPUID::simd_sse_level(int* phwsse) const
{
	const uint32_t	BIT_D_SSE = 0x02000000;	// bit 25
	const uint32_t	BIT_D_SSE2 = 0x04000000;	// bit 26
	const uint32_t	BIT_C_SSE3 = 0x00000001;	// bit 0
	const uint32_t	BIT_C_SSSE3 = 0x00000100;	// bit 9
	const uint32_t	BIT_C_SSE41 = 0x00080000;	// bit 19
	const uint32_t	BIT_C_SSE42 = 0x00100000;	// bit 20
	int	rt = SIMD_SSE_NONE;	// result
	uint32_t dwBuf[4];

	// check processor support
	GetData(dwBuf, 1);	// Function 1: Feature Information
	if (dwBuf[3] & BIT_D_SSE)
	{
		rt = SIMD_SSE_1;
		if (dwBuf[3] & BIT_D_SSE2)
		{
			rt = SIMD_SSE_2;
			if (dwBuf[2] & BIT_C_SSE3)
			{
				rt = SIMD_SSE_3;
				if (dwBuf[2] & BIT_C_SSSE3)
				{
					rt = SIMD_SSE_3S;
					if (dwBuf[2] & BIT_C_SSE41)
					{
						rt = SIMD_SSE_41;
						if (dwBuf[2] & BIT_C_SSE42)
						{
							rt = SIMD_SSE_42;
						}
					}
				}
			}
		}
	}
	if (NULL != phwsse)	*phwsse = rt;

	// check OS support
	try
	{
		__m128 xmm1 = _mm_setzero_ps();	// SSE instruction: xorps
		int* pxmm1 = (int*)&xmm1;	// 避免GCC的 -Wstrict-aliasing 警告.
		if (0 != *pxmm1)	rt = SIMD_SSE_NONE;	// 避免Release模式编译优化时剔除_mm_setzero_ps.
	}
	catch (...)
	{
		rt = SIMD_SSE_NONE;
	}

	return rt;
}

int	CCPUID::simd_avx_level(int* phwavx) const
{
	int	rt = SIMD_AVX_NONE;	// result

	// check processor support
	if (0 != GetField(CPUF_AVX))
	{
		rt = SIMD_AVX_1;
		if (0 != GetField(CPUF_AVX2))
		{
			rt = SIMD_AVX_2;
		}
	}
	if (NULL != phwavx)	*phwavx = rt;

	// check OS support
	if (0 != GetField(CPUF_OSXSAVE))	// XGETBV enabled for application use.
	{
		uint32_t n = GetField(CPUF_XFeatureSupportedMaskLo);	// XCR0: XFeatureSupportedMask register.
		if (6 == (n & 6))	// XCR0[2:1] = ‘11b’ (XMM state and YMM state are enabled by OS).
		{
			return rt;
		}
	}
	return SIMD_AVX_NONE;
}