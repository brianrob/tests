#include <stdio.h>
#include <unistd.h>
#include <pthread.h>

#define TRACEPOINT_DEFINE
#include "tp.h"

void SpinYetAgain()
{
    volatile unsigned int count = 0;
    int i;
    for(i=0; i<1000; i++)
    {
        count++;
    }
    usleep(10000);
}

void SpinSomeMore()
{
    volatile unsigned int count = 0;
    int i;
    for(i=0; i<1000; i++)
    {
        count++;
    }

    SpinYetAgain();
}

void Spin()
{
    volatile unsigned int count = 0;
    int i;
    for(i=0; i<1000; i++)
    {
        count++;
    }

    SpinSomeMore();
}

void *SpinThreadBase(void *arg)
{
    printf("New Thread!\n");
    tracepoint(test_provider, test_tracepoint, 1, "New Thread!");
    while(1)
    {
        Spin();
    }
}

int main(int argc, char* argv[])
{
    pthread_t th;

    if(pthread_create(&th, NULL, SpinThreadBase, NULL))
    {
        fprintf(stderr, "Error creating thread.\n");
        return 1;
    }

    SpinThreadBase(NULL);
    return 0;
}
