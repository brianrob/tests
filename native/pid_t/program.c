#include <stdio.h>
#include <sys/types.h>

int main(int argc, char* argv[])
{
    int size = sizeof(pid_t);
    printf("sizeof(pid_t) = %d\n", size);
}
