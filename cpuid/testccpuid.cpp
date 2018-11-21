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
FILE *fp;

struct OutputDefine {
	const char *DisplayName;
	const CPUIDFIELD Field;
};

OutputDefine Define[] = {

{"APM版本",CPUF_APM_Version},
{"100Mhz频率切换",CPUF_100MHzSteps},
{"MMX",CPUF_MMX},
{"MMX+",CPUF_MmxExt},
{"3DNow",CPUF_3DNow},
{"3DNow+",CPUF_3DNowExt},
{"3DNowPrefetch",CPUF_3DNowPrefetch},
{"AES",CPUF_AES},
{"F16C",CPUF_F16C},
{"FMA",CPUF_FMA},
{"FMA4",CPUF_FMA4},
{"PAE",CPUF_PAE},
{"Page1GB",CPUF_Page1GB},
};


#define DEBUG(str)	printf("[%s@%d]%s\n",__FILE__,__LINE__,str)

int main(int argc, char* argv[])
{
	int i;
	DEBUG("Opening File...");
	fp = fopen(argv[1], "w");
	DEBUG("Start Writting...");
	//CCPUID ccid;
	//ccid.RefreshAll();
	CCPUID& ccid = CCPUID::cur();
	// base info
	//fprintf(fp, "CCPUID.InfoCount:\t%d\n", ccid.InfoCount());
	//fprintf(fp, "CCPUID.LFuncStd:\t%.8Xh\n", ccid.LFuncStd());
	//fprintf(fp, "CCPUID.LFuncExt:\t%.8Xh\n", ccid.LFuncExt());
	fprintf(fp, "CPU厂商:\t%s\n", ccid.Vendor());
	//fprintf(fp,"CCPUID.Brand:\t%s\n", ccid.Brand());
	fprintf(fp, "CPU型号:\t%s 步进:%d 家族:%d+%d\n", ccid.BrandTrim(),ccid.GetField(CPUF_Stepping), ccid.GetField(CPUF_BaseFamily),ccid.GetField(CPUF_ExtFamily));
	fprintf(fp, "CPU SIMD功能:\n");
/*	DEBUG("Fetching SIMD Fetaure...");
	for (int n = 0; n < sizeof(Define) / sizeof(OutputDefine); n++)
	{
		uint32_t val = ccid.GetField(Define[n].Field);
		if (val > 0)
			fprintf(fp, "\t%s:\t%ld\t%s\n", Define[n].DisplayName, val,ccid.CPUFDesc[Define[n].Field]);
	}*/
	DEBUG("Fetching SSE Feature...");
	if (ccid.sse() > 1)
		for (i = 1; i < (int)(sizeof(CCPUID::SseNames) / sizeof(CCPUID::SseNames[0])); ++i)
			if (ccid.hwsse() >= i)
				fprintf(fp, "\t%s\n", CCPUID::SseNames[i]);
	for (i = 1; i < (int)(sizeof(CCPUID::AvxNames) / sizeof(CCPUID::AvxNames[0])); ++i)
		if (ccid.hwavx() >= i)
			fprintf(fp, "\t%s\n", CCPUID::AvxNames[i]);
	DEBUG("Fetch Description Feature...");
	fprintf(fp,"所有CPU功能:\n");
	for (int n = 0; n < CPUFDescLen; n++)
	{
		uint32_t result = ccid.GetField(ccid.CPUFDesc[n].cpuf);
		
		if (result != ccid.CPUFDesc[n].reserved)
			fprintf(fp, "%s\t%s\t%ld\n", ccid.CPUFDesc[n].szName, ccid.CPUFDesc[n].szDesc, result);
	}
	fflush(fp);
	DEBUG("Flushing...");
	fclose(fp);

	return 0;
}