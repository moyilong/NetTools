const CPUIDFIELDDESC CCPUID::CPUFDesc[] = {
{CPUF_LFuncStd, 0, "LFuncStd", "largest standard function."}
, { CPUF_Stepping, 0, "步进", "处理器步进" }
, { CPUF_BaseModel, 0, "基本型号", "基本处理器型号" }
, { CPUF_BaseFamily, 0, "基本家族", "基本处理器家族" }
, { CPUF_ProcessorType, 0, "处理器类型", "处理器类型" }
, { CPUF_ExtModel, 0, "扩展型号", "扩展处理器型号" }
, { CPUF_ExtFamily, 0, "扩展家族", "扩展处理器家族" }


//CPU指令集
, { CPUF_MMX, 0, "MMX", "MMX 指令集." }
, { CPUF_MmxExt, 0, "MmxExt", "MMX指令集的AMD扩展" }
, { CPUF_3DNow, 0, "3DNow", "3DNow! 指令集." }
, { CPUF_3DNowExt, 0, "3DNowExt", "AMD 3DNow! 指令集扩展" }
, { CPUF_AVX2, 0, "AVX2", "AVX2 指令集." }
, { CPUF_SSE, 0, "SSE", "SSE指令集" }
, { CPUF_SSE2, 0, "SSE2", "SSE2指令集" }
, { CPUF_SSSE3, 0, "SSSE3", "SSSE3指令集" }
, { CPUF_SSE3, 0, "SSE3", "SSE3 指令集" }
, { CPUF_SSE41, 0, "SSE41", "SSE4.1 指令集." }
, { CPUF_SSE42, 0, "SSE42", "SSE4.2 指令集." }
, { CPUF_SSE4A, 0, "SSE4A", "SSE4A 指令集." }
, { CPUF_AVX, 0, "AVX", "AVX 指令集." }
, { CPUF_PCLMULQDQ, 0, "PCLMULQDQ", "PCLMULQDQ指令集" }
, { CPUF_CX8, 0, "CX8", "CMPXCHG8B 指令集" }
, { CPUF_CMPXCHG16B, 0, "CMPXCHG16B", "CMPXCHG16B 指令集" }
, { CPUF_SYSCALL, 0, "SYSCALL", "SYSCALL 和 SYSRET 指令集." }
, { CPUF_CLFSH, 0, "CLFSH", "CLFLUSH 指令集" }
, { CPUF_FXSR, 0, "FXSR", "FXSAVE 和 FXRSTOR 指令集." }
, { CPUF_CMOV, 0, "CMOV", "Conditional Move 指令集." }
, { CPUF_SEP, 0, "SEP", "快速系统调用指令集(SYSENTER和SYSEXIT)" }
, { CPUF_MSR, 0, "MSR", "特殊型号寄存器 (RDMSR 和 WRMSR) 指令集." }
, { CPUF_MONITOR, 0, "MONITOR", "MONITOR/MWAIT 指令集." }
, { CPUF_MOVBE, 0, "MOVBE", "MOVBE 指令集" }
, { CPUF_POPCNT, 0, "POPCNT", "POPCNT 指令集" }
, { CPUF_AES, 0, "AES", "Advanced Encryption Standard (AES) 指令集." }
, { CPUF_XSAVE, 0, "XSAVE", "XSAVE (and related) 指令集" }
, { CPUF_OSXSAVE, 0, "OSXSAVE", "XSAVE (and related) 指令集" }
, { CPUF_FMA4, 0, "FMA4", "4操作符FMA指令集" }
, { CPUF_3DNowPrefetch, 0, "3DNowPrefetch", "3DNow预取(PREFETCH 和 PREFETCHW)指令集" }
, { CPUF_XOP, 0, "XOP", "扩展操作符" }

, { CPUF_FP128, 0, "FP128", "128-bit SSE (multimedia) 指令集 are executed with full-width internal operations and pipelines rather than decomposing them into internal 64-bit suboperations." }
, { CPUF_MOVU, 0, "MOVU", "MOVU SSE (multimedia) 指令集 are more efficient and should be preferred to SSE(multimedia) MOVL/MOVH. MOVUPS is more efficient than MOVLPS/MOVHPS." }


, { CPUF_VMX, 0, "VMX", "虚拟机扩展" }
, { CPUF_SMX, 0, "SMX", "安全模式扩展" }

, { CPUF_BrandId8, 0, "BrandId8", "8-bit brand ID." }
, { CPUF_CLFlush, 0, "CLFlush", "CLFLUSH line size. (*8)" }
, { CPUF_MaxApicId, 0, "MaxApicId", "Maximum number of addressable IDs for logical processors in this physical package." }
, { CPUF_ApicId, 0, "ApicId", "Initial local APIC physical ID(8-bit)." }
, { CPUF_DTES64, 0, "DTES64", "64-bit DS Area." }
, { CPUF_DS_CPL, 0, "DS_CPL", "CPL Qualified Debug Store." }
, { CPUF_EIST, 0, "EIST", "Enhanced Intel SpeedStep technology." }
, { CPUF_TM2, 0, "TM2", "温控 2." }

, { CPUF_CNXT_ID, 0, "CNXT_ID", "L1 Context ID." }
, { CPUF_FMA, 0, "FMA", "supports FMA extensions using YMM state." }

, { CPUF_xTPR, 0, "xTPR", "xTPR Update Control. Can disable sending Task Priority messages." }
, { CPUF_PDCM, 0, "PDCM", "Perfmon and Debug Capability." }
, { CPUF_PCID, 0, "PCID", "Process Context Identifiers." }
, { CPUF_DCA, 0, "DCA", "Direct Cache Access." }

, { CPUF_x2APIC, 0, "x2APIC", "扩展XAPIC支持" }

, { CPUF_TSC_DEADLINE, 0, "TSC_DEADLINE", "Local APIC timer supports one-shot operation using a TSC deadline value." }

, { CPUF_F16C, 0, "F16C", "half-precision convert  support." }
, { CPUF_RDRAND, 0, "RDRAND", "RDRAND ." }
, { CPUF_FPU, 0, "FPU", "集成浮点处理器" }
, { CPUF_VME, 0, "VME", "虚拟86模式扩展" }
, { CPUF_DE, 0, "DE", "调试扩展" }
, { CPUF_PSE, 0, "PSE", "页大小扩展" }
, { CPUF_TSC, 0, "TSC", "计时器" }

, { CPUF_PAE, 0, "PAE", "物理地址线扩展" }
, { CPUF_MCE, 0, "MCE", "Machine Check Exception." }

, { CPUF_APIC, 0, "APIC", "APIC(Advanced Programmable Interrupt Controller) On-Chip." }


, { CPUF_MTRR, 0, "MTRR", "Memory Type Range Registers." }
, { CPUF_PGE, 0, "PGE", "Page Global Enable." }
, { CPUF_MCA, 0, "MCA", "Machine-Check Architecture." }

, { CPUF_PAT, 0, "PAT", "页属性表" }
, { CPUF_PSE36, 0, "PSE36", "36-Bit 页大小扩展" }
, { CPUF_PSN, 0, "PSN", "处理器序列号" }

, { CPUF_DS, 0, "DS", "调试存储" }
, { CPUF_ACPI, 0, "ACPI", "温度监控和软件控制器" }

, { CPUF_SS, 0, "SS", "Self Snoop." }
, { CPUF_HTT, 0, "HTT", "最大APIC ID" }
, { CPUF_TM, 0, "TM", "温度监控" }
, { CPUF_PBE, 0, "PBE", "Pending Break Enable." }
, { CPUF_Cache_Type, 0, "Cache_Type", "缓存类型 (0=无, 1=数据, 2=, 3=混合)." }
, { CPUF_Cache_Level, 0, "Cache_Level", "缓存等级(从1开始)" }
, { CPUF_CACHE_SI, 0, "CACHE_SI", "Self Initializing cache level." }
, { CPUF_CACHE_FA, 0, "CACHE_FA", "Fully Associative cache." }
, { CPUF_MaxApicIdShare, 0, "MaxApicIdShare", "Maximum number of addressable IDs for logical processors sharing this cache (plus 1 encoding)." }
, { CPUF_MaxApicIdCore, 0, "MaxApicIdCore", "Maximum number of addressable IDs for processor cores in the physical package (plus 1 encoding)." }
, { CPUF_Cache_LineSize, 0, "Cache_LineSize", "System Coherency Line Size (plus 1 encoding)." }
, { CPUF_Cache_Partitions, 0, "Cache_Partitions", "Physical Line partitions (plus 1 encoding)." }
, { CPUF_Cache_Ways, 0, "Cache_Ways", "Ways of Associativity (plus 1 encoding)." }
, { CPUF_Cache_Sets, 0, "Cache_Sets", "Number of Sets (plus 1 encoding)." }
, { CPUF_CACHE_INVD, 0, "CACHE_INVD", "WBINVD/INVD behavior on lower level caches." }
, { CPUF_CACHE_INCLUSIVENESS, 0, "CACHE_INCLUSIVENESS", "Cache is inclusive of lower cache levels." }
, { CPUF_CACHE_COMPLEXINDEX, 0, "CACHE_COMPLEXINDEX", "Complex Cache Indexing." }
, { CPUF_MonLineSizeMin, 0, "MonLineSizeMin", "Smallest monitor line size in bytes." }
, { CPUF_MonLineSizeMax, 0, "MonLineSizeMax", "Largest monitor-line size in bytes." }
, { CPUF_EMX, 0, "EMX", "Enumerate MONITOR/MWAIT extensions." }
, { CPUF_IBE, 0, "IBE", "Interrupt Break-Event." }
, { CPUF_MWAIT_Number_C0, 0, "MWAIT_Number_C0", "Number of C0 sub C-states supported using MWAIT." }
, { CPUF_MWAIT_Number_C1, 0, "MWAIT_Number_C1", "Number of C1 sub C-states supported using MWAIT." }
, { CPUF_MWAIT_Number_C2, 0, "MWAIT_Number_C2", "Number of C2 sub C-states supported using MWAIT." }
, { CPUF_MWAIT_Number_C3, 0, "MWAIT_Number_C3", "Number of C3 sub C-states supported using MWAIT." }
, { CPUF_MWAIT_Number_C4, 0, "MWAIT_Number_C4", "Number of C4 sub C-states supported using MWAIT." }
, { CPUF_DTS, 0, "DTS", "Digital Thermal Sensor." }
, { CPUF_TURBO_BOOST, 0, "TURBO_BOOST", "Intel Turbo Boost Technology." }
, { CPUF_ARAT, 0, "ARAT", "Always Running APIC Timer." }
, { CPUF_PLN, 0, "PLN", "Power Limit Notification." }
, { CPUF_ECMD, 0, "ECMD", "Extended Clock Modulation Duty." }
, { CPUF_PTM, 0, "PTM", "Package Thermal Management." }
, { CPUF_DTS_ITs, 0, "DTS_ITs", "Number of Interrupt Thresholds in Digital Thermal Sensor." }
, { CPUF_PERF, 0, "PERF", "Hardware Coordination Feedback Capability." }
, { CPUF_ACNT2, 0, "ACNT2", "ACNT2 Capability." }
, { CPUF_ENERGY_PERF_BIAS, 0, "ENERGY_PERF_BIAS", "Performance-Energy Bias capability." }
, { CPUF_Max07Subleaf, 0, "Max07Subleaf", "Reports the maximum supported leaf 7 sub-leaf." }
, { CPUF_FSGSBASE, 0, "FSGSBASE", "Supports RDFSBASE/RDGSBASE/WRFSBASE/WRGSBASE." }
, { CPUF_BMI1, 0, "BMI1", "The first group of advanced bit manipulation extensions (ANDN, BEXTR, BLSI, BLSMK, BLSR, TZCNT)." }
, { CPUF_HLE, 0, "HLE", "Hardware Lock Elision." }

, { CPUF_SMEP, 0, "SMEP", "Supervisor Mode Execution Protection." }
, { CPUF_BMI2, 0, "BMI2", "The second group of advanced bit manipulation extensions (BZHI, MULX, PDEP, PEXT, RORX, SARX, SHLX, SHRX)." }
, { CPUF_ERMS, 0, "ERMS", "Supports Enhanced REP MOVSB/STOSB." }
, { CPUF_INVPCID, 0, "INVPCID", "INVPCID ." }
, { CPUF_RTM, 0, "RTM", "" }
, { CPUF_PLATFORM_DCA_CAP, 0, "PLATFORM_DCA_CAP", "Value of PLATFORM_DCA_CAP MSR Bits [31:0] (Offset 1F8h)." }
, { CPUF_APM_Version, 0, "APM_Version", "Version ID of architectural performance monitoring." }
, { CPUF_APM_Counters, 0, "APM_Counters", "Number of general-purpose performance monitoring counters per logical processor." }
, { CPUF_APM_Bits, 0, "APM_Bits", "Bit width of general-purpose, performance monitoring counters." }
, { CPUF_APM_Length, 0, "APM_Length", "Length of EBX bit vector to enumerate architectural performance monitoring events." }
, { CPUF_APM_CC, 0, "APM_CC", "Core cycle event not available if 1." }
, { CPUF_APM_IR, 0, "APM_IR", " retired event not available if 1." }
, { CPUF_APM_RC, 0, "APM_RC", "Reference cycles event not available if 1." }
, { CPUF_APM_LLCR, 0, "APM_LLCR", "Last-level cache reference event not available if 1." }
, { CPUF_APM_LLCM, 0, "APM_LLCM", "Last-level cache misses event not available if 1." }
, { CPUF_APM_BIR, 0, "APM_BIR", "Branch  retired event not available if 1." }
, { CPUF_APM_BMR, 0, "APM_BMR", "Branch mispredict retired event not available if 1." }
, { CPUF_APM_FC_Number, 0, "APM_FC_Number", "Number of fixed-function performance counters." }
, { CPUF_APM_FC_Bits, 0, "APM_FC_Bits", "Bit width of fixed-function performance counters." }
, { CPUF_Topology_Bits, 0, "Topology_Bits", "Number of bits to shift right on x2APIC ID to get a unique topology ID of the next level type." }
, { CPUF_Topology_Number, 0, "Topology_Number", "Number of factory-configured logical processors at this level." }
, { CPUF_Topology_Level, 0, "Topology_Level", "Level number. Same value in ECX input." }
, { CPUF_Topology_Type, 0, "Topology_Type", "Level Type (0=Invalid, 1=Thread, 2=Core)." }
, { CPUF_X2APICID, 0, "X2APICID", "x2APIC ID." }
, { CPUF_XFeatureSupportedMaskLo, 0, "XFeatureSupportedMaskLo", "The lower 32 bits of XCR0(XFEATURE_ENABLED_MASK register)." }
, { CPUF_XFeatureEnabledSizeMax, 0, "XFeatureEnabledSizeMax", "Size in bytes of XSAVE/XRSTOR area for the currently enabled features in XCR0." }
, { CPUF_XFeatureSupportedSizeMax, 0, "XFeatureSupportedSizeMax", "Size in bytes of XSAVE/XRSTOR area for all features that the core supports." }
, { CPUF_XFeatureSupportedMaskHi, 0, "XFeatureSupportedMaskHi", "The upper 32 bits of XCR0(XFEATURE_ENABLED_MASK register)." }
, { CPUF_XSAVEOPT, 0, "XSAVEOPT", "XSAVEOPT is available." }
, { CPUF_YmmSaveStateSize, 0, "YmmSaveStateSize", "YMM save state byte size." }
, { CPUF_YmmSaveStateOffset, 0, "YmmSaveStateOffset", "YMM save state byte offset." }
, { CPUF_LwpSaveStateSize, 0, "LwpSaveStateSize", "LWP save state byte size." }
, { CPUF_LwpSaveStateOffset, 0, "LwpSaveStateOffset", "LWP save state byte offset." }
, { CPUF_LFuncExt, 0, "LFuncExt", "Largest extended function." }
, { CPUF_BrandId16, 0, "BrandId16", "16-bit Brand ID." }
, { CPUF_PkgType, 0, "PkgType", "Package type (Family[7:0] >= 10h)." }
, { CPUF_LahfSahf, 0, "LahfSahf", "LAHF and SAHF  support in 64-bit mode." }
, { CPUF_CmpLegacy, 0, "CmpLegacy", "core multi-processing legacy mode." }
, { CPUF_SVM, 0, "SVM", "secure virtual machine." }
, { CPUF_ExtApicSpace, 0, "ExtApicSpace", "extended APIC space." }
, { CPUF_AltMovCr8, 0, "AltMovCr8", "LOCK MOV CR0 means MOV CR8." }
, { CPUF_ABM, 0, "ABM", "advanced bit manipulation (LZCNT)." }

, { CPUF_MisAlignSse, 0, "MisAlignSse", "misaligned SSE mode." }
, { CPUF_OSVW, 0, "OSVW", "OS visible workaround." }
, { CPUF_IBS, 0, "IBS", " based sampling." }
, { CPUF_SKINIT, 0, "SKINIT", "SKINIT and STGI are supported, independent of the value of MSRC000_0080[SVME]." }
, { CPUF_WDT, 0, "WDT", "watchdog timer support." }
, { CPUF_LWP, 0, "LWP", "lightweight profiling support." }
, { CPUF_BIT_NODEID, 0, "BIT_NODEID", "Indicates support for MSRC001_100C[NodeId, NodesPerProcessor]." }
, { CPUF_TBM, 0, "TBM", "Trailing bit manipulation  support." }
, { CPUF_TopologyExtensions, 0, "TopologyExtensions", "Topology extensions support." }
, { CPUF_XD, 0, "XD", "禁止执行位" }
, { CPUF_FFXSR, 0, "FFXSR", "FXSAVE and FXRSTOR  optimizations." }
, { CPUF_Page1GB, 0, "Page1GB", "1-GB large page support." }
, { CPUF_RDTSCP, 0, "RDTSCP", "RDTSCP and TSC_AUX." }
, { CPUF_LM, 0, "LM", "64位模式" }
, { CPUF_L1ITlb2and4MSize, 0, "L1ITlb2and4MSize", " TLB number of entries for 2-MB and 4-MB pages." }
, { CPUF_L1ITlb2and4MAssoc, 0, "L1ITlb2and4MAssoc", " TLB associativity for 2-MB and 4-MB pages." }
, { CPUF_L1DTlb2and4MSize, 0, "L1DTlb2and4MSize", "Data TLB number of entries for 2-MB and 4-MB pages." }
, { CPUF_L1DTlb2and4MAssoc, 0, "L1DTlb2and4MAssoc", "Data TLB associativity for 2-MB and 4-MB pages." }
, { CPUF_L1ITlb4KSize, 0, "L1ITlb4KSize", " TLB number of entries for 4 KB pages." }
, { CPUF_L1ITlb4KAssoc, 0, "L1ITlb4KAssoc", " TLB associativity for 4KB pages." }
, { CPUF_L1DTlb4KSize, 0, "L1DTlb4KSize", "Data TLB number of entries for 4 KB pages." }
, { CPUF_L1DTlb4KAssoc, 0, "L1DTlb4KAssoc", "Data TLB associativity for 4 KB pages." }
, { CPUF_L1DcLineSize, 0, "L1DcLineSize", "L1 data cache line size in bytes." }
, { CPUF_L1DcLinesPerTag, 0, "L1DcLinesPerTag", "L1 data cache lines per tag." }
, { CPUF_L1DcAssoc, 0, "L1DcAssoc", "L1 data cache associativity." }
, { CPUF_L1DcSize, 0, "L1DcSize", "L1 data cache size in KB." }
, { CPUF_L1IcLineSize, 0, "L1IcLineSize", "L1  cache line size in bytes" }
, { CPUF_L1IcLinesPerTag, 0, "L1IcLinesPerTag", "L1  cache lines per tag." }
, { CPUF_L1IcAssoc, 0, "L1IcAssoc", "L1  cache associativity." }
, { CPUF_L1IcSize, 0, "L1IcSize", "L1  cache size KB." }
, { CPUF_L2ITlb2and4MSize, 0, "L2ITlb2and4MSize", "L2  TLB number of entries for 2 MB and 4 MB pages." }
, { CPUF_L2ITlb2and4MAssoc, 0, "L2ITlb2and4MAssoc", "L2  TLB associativity for 2 MB and 4 MB pages." }
, { CPUF_L2DTlb2and4MSize, 0, "L2DTlb2and4MSize", "L2 data TLB number of entries for 2 MB and 4 MB pages." }
, { CPUF_L2DTlb2and4MAssoc, 0, "L2DTlb2and4MAssoc", "L2 data TLB associativity for 2 MB and 4 MB pages." }
, { CPUF_L2ITlb4KSize, 0, "L2ITlb4KSize", "L2  TLB number of entries for 4 KB pages." }
, { CPUF_L2ITlb4KAssoc, 0, "L2ITlb4KAssoc", "L2  TLB associativity for 4 KB pages." }
, { CPUF_L2DTlb4KSize, 0, "L2DTlb4KSize", "L2 data TLB number of entries for 4 KB pages." }
, { CPUF_L2DTlb4KAssoc, 0, "L2DTlb4KAssoc", "L2 data TLB associativity for 4 KB pages." }
, { CPUF_L2LineSize, 0, "L2LineSize", "L2 cache line size in bytes." }
, { CPUF_L2LinesPerTag, 0, "L2LinesPerTag", "L2 cache lines per tag." }
, { CPUF_L2Assoc, 0, "L2Assoc", "L2 cache associativity." }
, { CPUF_L2Size, 0, "L2Size", "L2 cache size in KB." }
, { CPUF_L3LineSize, 0, "L3LineSize", "L3 cache line size in bytes." }
, { CPUF_L3LinesPerTag, 0, "L3LinesPerTag", "L3 cache lines per tag." }
, { CPUF_L3Assoc, 0, "L3Assoc", "L3 cache associativity." }
, { CPUF_L3Size, 0, "L3Size", "L3 cache size." }
, { CPUF_TS, 0, "TS", "Temperature sensor." }
, { CPUF_FID, 0, "FID", "频率ID控制" }
, { CPUF_VID, 0, "VID", "电压ID控制" }
, { CPUF_TTP, 0, "TTP", "THERMTRIP." }
, { CPUF_HTC, 0, "HTC", "硬件温度控制" }
, { CPUF_100MHzSteps, 0, "100MHzSteps", "10Mhz 分频器" }
, { CPUF_HwPstate, 0, "HwPstate", "硬件电源状态控制" }
, { CPUF_TscInvariant, 0, "TscInvariant", "TSC invariant." }
, { CPUF_CPB, 0, "CPB", "核心性能加速" }
, { CPUF_EffFreqRO, 0, "EffFreqRO", "Read-only effective frequency interface." }
, { CPUF_PhysAddrSize, 0, "PhysAddrSize", "最高物理地址位宽" }
, { CPUF_LinAddrSize, 0, "LinAddrSize", "最高位宽" }
, { CPUF_GuestPhysAddrSize, 0, "GuestPhysAddrSize", "Maximum guest physical byte address size in bits." }
, { CPUF_NC, 0, "NC", "最大处理器ID" }
, { CPUF_ApicIdCoreIdSize, 0, "ApicIdCoreIdSize", "APIC ID size. The number of bits in the initial APIC20[ApicId] value that indicate core ID within a processor." }
, { CPUF_SvmRev, 0, "SvmRev", "SVM revision." }
, { CPUF_NASID, 0, "NASID", "number of address space identifiers (ASID)." }
, { CPUF_NP, 0, "NP", "Nested paging." }
, { CPUF_LbrVirt, 0, "LbrVirt", "LBR virtualization." }
, { CPUF_SVML, 0, "SVML", "SVM lock. Indicates support for SVM-Lock." }
, { CPUF_NRIPS, 0, "NRIPS", "NRIP save. Indicates support for NRIP save on #VMEXIT." }
, { CPUF_TscRateMsr, 0, "TscRateMsr", "MSR based TSC rate control." }
, { CPUF_VmcbClean, 0, "VmcbClean", "VMCB clean bits. Indicates support for VMCB clean bits." }
, { CPUF_FlushByAsid, 0, "FlushByAsid", "Flush by ASID." }
, { CPUF_DecodeAssists, 0, "DecodeAssists", "Decode assists." }
, { CPUF_PauseFilter, 0, "PauseFilter", "Pause intercept filter." }
, { CPUF_PauseFilterThreshold, 0, "PauseFilterThreshold", "PAUSE filter threshold." }
, { CPUF_L1ITlb1GSize, 0, "L1ITlb1GSize", "L1  TLB number of entries for 1 GB pages." }
, { CPUF_L1ITlb1GAssoc, 0, "L1ITlb1GAssoc", "L1  TLB associativity for 1 GB pages." }
, { CPUF_L1DTlb1GSize, 0, "L1DTlb1GSize", "L1 data TLB number of entries for 1 GB pages." }
, { CPUF_L1DTlb1GAssoc, 0, "L1DTlb1GAssoc", "L1 data TLB associativity for 1 GB pages." }
, { CPUF_L2ITlb1GSize, 0, "L2ITlb1GSize", "L2  TLB number of entries for 1 GB pages." }
, { CPUF_L2ITlb1GAssoc, 0, "L2ITlb1GAssoc", "L2  TLB associativity for 1 GB pages." }
, { CPUF_L2DTlb1GSize, 0, "L2DTlb1GSize", "L2 data TLB number of entries for 1 GB pages." }
, { CPUF_L2DTlb1GAssoc, 0, "L2DTlb1GAssoc", "L2 data TLB associativity for 1 GB pages." }
, { CPUF_IBSFFV, 0, "IBSFFV", "IBS feature flags valid." }
, { CPUF_FetchSam, 0, "FetchSam", "IBS fetch sampling supported." }
, { CPUF_OpSam, 0, "OpSam", "IBS execution sampling supported." }
, { CPUF_RdWrOpCnt, 0, "RdWrOpCnt", "Read write of op counter supported." }
, { CPUF_OpCnt, 0, "OpCnt", "Op counting mode supported." }
, { CPUF_BrnTrgt, 0, "BrnTrgt", "Branch target address reporting supported." }
, { CPUF_OpCntExt, 0, "OpCntExt", "IbsOpCurCnt and IbsOpMaxCnt extend by 7 bits." }
, { CPUF_RipInvalidChk, 0, "RipInvalidChk", "Invalid RIP indication supported." }
, { CPUF_LwpAvail, 0, "LwpAvail", "LWP available." }
, { CPUF_LwpVAL, 0, "LwpVAL", "LWPVAL  available." }
, { CPUF_LwpIRE, 0, "LwpIRE", "instraction retired event available." }
, { CPUF_LwpBRE, 0, "LwpBRE", "branch retired event available." }
, { CPUF_LwpDME, 0, "LwpDME", "DC miss event available." }
, { CPUF_LwpCNH, 0, "LwpCNH", "core clocks not halted event available." }
, { CPUF_LwpRNH, 0, "LwpRNH", "core reference clocks not halted event available." }
, { CPUF_LwpInt, 0, "LwpInt", "interrupt on threshold overflow available." }
, { CPUF_LwpCbSize, 0, "LwpCbSize", "control block size. Size in bytes of the LWPCB." }
, { CPUF_LwpEventSize, 0, "LwpEventSize", "event record size. Size in bytes of an event record in the LWP event ring buffer." }
, { CPUF_LwpMaxEvents, 0, "LwpMaxEvents", "maximum EventId. Maximum EventId value that is supported." }
, { CPUF_LwpEventOffset, 0, "LwpEventOffset", "offset to the EventInterval1 field. Offset from the start of the LWPCB to the EventInterval1 field." }
, { CPUF_LwpLatencyMax, 0, "LwpLatencyMax", "latency counter bit size. Size in bits of the cache latency counters." }
, { CPUF_LwpDataAddress, 0, "LwpDataAddress", "data cache miss address valid." }
, { CPUF_LwpLatencyRnd, 0, "LwpLatencyRnd", "amount cache latency is rounded." }
, { CPUF_LwpVersion, 0, "LwpVersion", "version. Version of LWP implementation." }
, { CPUF_LwpMinBufferSize, 0, "LwpMinBufferSize", "event ring buffer size. Minimum size of the LWP event ring buffer, in units of 32 event records." }
, { CPUF_LwpBranchPrediction, 0, "LwpBranchPrediction", "branch prediction filtering supported." }
, { CPUF_LwpIpFiltering, 0, "LwpIpFiltering", "IP filtering supported." }
, { CPUF_LwpCacheLevels, 0, "LwpCacheLevels", "cache level filtering supported." }
, { CPUF_LwpCacheLatency, 0, "LwpCacheLatency", "cache latency filtering supported." }
, { CPUF_D_LwpAvail, 0, "D_LwpAvail", "lightweight profiling supported." }
, { CPUF_D_LwpVAL, 0, "D_LwpVAL", "LWPVAL  supported." }
, { CPUF_D_LwpIRE, 0, "D_LwpIRE", "instraction retired event supported." }
, { CPUF_D_LwpBRE, 0, "D_LwpBRE", "branch retired event supported." }
, { CPUF_D_LwpDME, 0, "D_LwpDME", "DC miss event supported." }
, { CPUF_D_LwpCNH, 0, "D_LwpCNH", "core clocks not halted event supported." }
, { CPUF_D_LwpRNH, 0, "D_LwpRNH", "core reference clocks not halted event supported." }
, { CPUF_D_LwpInt, 0, "D_LwpInt", "interrupt on threshold overflow supported." }
, { CPUF_CacheType, 0, "CacheType", "缓存类型 (0=无, 1=数据, 2=, 3=所有)." }
, { CPUF_CacheLevel, 0, "CacheLevel", "缓存等级(从1开始计数)" }
, { CPUF_SelfInitialization, 0, "SelfInitialization", "Self Initializing cache level." }
, { CPUF_FullyAssociative, 0, "FullyAssociative", "Fully Associative cache." }
, { CPUF_NumSharingCache, 0, "NumSharingCache", "Number of cores sharing cache. The number of cores sharing this cache is NumSharingCache+1." }
, { CPUF_CacheLineSize, 0, "CacheLineSize", "Cache line size in bytes (plus 1 encoding)." }
, { CPUF_CachePhysPartitions, 0, "CachePhysPartitions", "Cache physical line partitions (plus 1 encoding)." }
, { CPUF_CacheNumWays, 0, "CacheNumWays", "Cache number of ways (plus 1 encoding)." }
, { CPUF_CacheNumSets, 0, "CacheNumSets", "Cache number of sets (plus 1 encoding)." }
, { CPUF_WBINVD, 0, "WBINVD", "Write-Back Invalidate/Invalidate (WBINVD/INVD)." }
, { CPUF_CacheInclusive, 0, "CacheInclusive", "Cache inclusive." }
, { CPUF_ExtendedApicId, 0, "ExtendedApicId", "扩展 APIC ID." }
, { CPUF_ComputeUnitId, 0, "ComputeUnitId", "compute unit ID. Identifies the processor compute unit ID." }
, { CPUF_CoresPerComputeUnit, 0, "CoresPerComputeUnit", "cores per compute unit. The number of cores per compute unit is CoresPerComputeUnit+1." }
, { CPUF_NodeId, 0, "NodeId", "Specifies the node ID." }
, { CPUF_NodesPerProcessor, 0, "NodesPerProcessor", "Specifies the number of nodes per processor." }
};