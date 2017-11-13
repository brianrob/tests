#define TRACEPOINT_DEFINE
#include "tp.h"

int main(int argc, char* argv[])
{
    printf("Writing 10 events.\n");
    for(int i=0; i<10; i++)
    {
        tracepoint(test_provider, test_tracepoint, i+1, "Hello World!");
    }
    printf("Done.\n");
    return 0;
}
