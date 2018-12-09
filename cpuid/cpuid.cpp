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
#include <inttypes.h>
#include "ccpuid.h"
#include<omp.h>
FILE *fp;

struct OutputDefine {
	const char *DisplayName;
	const CPUIDFIELD Field;
};

OutputDefine Define[] = {
{"MMX",CPUF_MMX},
{"MMX+",CPUF_MmxExt},
{"3DNow",CPUF_3DNow},
{"3DNow+",CPUF_3DNowExt},
{"3DNowPrefetch",CPUF_3DNowPrefetch},
{"AES",CPUF_AES},
{"FMA",CPUF_FMA},
{"FMA4",CPUF_FMA4},
{"PAE",CPUF_PAE},
};


#define DEBUG(str) if (fp != stdout)	fprintf(stderr,"[%s@%d]%s\n",__FILE__,__LINE__,str)

int main(int argc, char* argv[])
{
	int i;
	if (argc > 1)
		fp = fopen(argv[1], "w");
	if (fp == NULL)
		fp = stdout;
	DEBUG("Start Writting...");
	CCPUID& ccid = CCPUID::cur();
	fprintf(fp, "true;CPU厂商;;%s;\n", ccid.Vendor());
	fprintf(fp, "true;CPU型号;;%s 步进:%d 家族:%d+%d;\n", ccid.BrandTrim(),ccid.GetField(CPUF_Stepping), ccid.GetField(CPUF_BaseFamily),ccid.GetField(CPUF_ExtFamily));
	fprintf(fp, "true;CPU线程数;;逻辑处理器数量:%d 最大线程数:%d\n", omp_get_num_procs(), omp_get_max_threads());
	fprintf(fp, "true;CPU 指令集;;");
	DEBUG("Fetching SSE Feature...");
	for (int n = 0; n < sizeof(Define) / sizeof(OutputDefine); n++)
		if (ccid.GetField(Define[n].Field) != 0)
			fprintf(fp, "%s,", Define[n].DisplayName);
	if (ccid.sse() > 1)
		for (i = 1; i < (int)(sizeof(CCPUID::SseNames) / sizeof(CCPUID::SseNames[0])); ++i)
			if (ccid.hwsse() >= i)
				fprintf(fp, "%s,", CCPUID::SseNames[i]);
	for (i = 1; i < (int)(sizeof(CCPUID::AvxNames) / sizeof(CCPUID::AvxNames[0])); ++i)
		if (ccid.hwavx() >= i)
			fprintf(fp, "%s,", CCPUID::AvxNames[i]);
	fprintf(fp, "\n");
	DEBUG("Fetch Description Feature...");
	int n = 0;
	while (true)
	{

		if (ccid.CPUFDesc[n].szDesc == NULL && ccid.CPUFDesc[n].szName == NULL)
			break;
		uint32_t result = ccid.GetField(ccid.CPUFDesc[n].cpuf);
		fprintf(fp, "%s;%s;%s;%" PRIu32"\n", (result == ccid.CPUFDesc[n].reserved ? "false" : "true"), ccid.CPUFDesc[n].szName, ccid.CPUFDesc[n].szDesc, result);
		n++;
	}
	
	fflush(fp);
	DEBUG("Flushing...");
	fclose(fp);

	return 0;
}