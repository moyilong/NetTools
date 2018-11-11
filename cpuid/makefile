# flags
CC = g++
CFLAGS = -Wall -msse
LFLAGS =

# args
RELEASE =0
BITS =

# [args] 生成模式. 0代表debug模式, 1代表release模式. make RELEASE=1.
ifeq ($(RELEASE),0)
	# debug
	CFLAGS += -g
else
	# release
	CFLAGS += -static -O3 -DNDEBUG
	LFLAGS += -static
endif

# [args] 程序位数. 32代表32位程序, 64代表64位程序, 其他默认. make BITS=32.
ifeq ($(BITS),32)
	CFLAGS += -m32
	LFLAGS += -m32
else
	ifeq ($(BITS),64)
		CFLAGS += -m64
		LFLAGS += -m64
	else
	endif
endif


.PHONY : all clean

# files
TARGETS = testccpuid
OBJS = testccpuid.o ccpuid.o

all : $(TARGETS)

testccpuid : $(OBJS)
	$(CC) $(LFLAGS) -o $@ $^


ccpuid.o : ccpuid.cpp ccpuid.h
	$(CC) $(CFLAGS) -c $<

testccpuid.o : testccpuid.cpp ccpuid.h
	$(CC) $(CFLAGS) -c $<


clean :
	rm -f $(OBJS) $(TARGETS) $(addsuffix .exe,$(TARGETS))
