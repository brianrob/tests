#include <stdio.h>

void SpinYetAgain()
{
    volatile unsigned int count = 0;
    int i;
    for(i=0; i<1000; i++)
    {
        count++;
    }
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

int main(int argc, char* argv[])
{
    while(1)
    {
        Spin();
    }
}
