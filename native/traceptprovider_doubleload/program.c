//#define _GNU_SOURCE
#include <dlfcn.h>
#include <stdio.h>
#include <string.h>

void *dlopen(const char *filename, int flag)
{
    static void* (*dlopenImpl)(const char *filename, int flag) = 0;
    if(!dlopenImpl)
    {
        dlopenImpl = dlsym(RTLD_NEXT, "dlopen");
    }

    if(strcmp(filename, "2//libcoreclrtraceptprovider.so") == 0)
    {
        printf("Skip loading 2//libcoreclrtraceptprovider.so.\n");
        return 0;
    }

    printf("Calling dlopen(%s).\n", filename);
    return dlopenImpl(filename, flag);
}

int main(int argc, char* argv[])
{
    dlopen("1//libcoreclr.so", RTLD_NOW | RTLD_GLOBAL);
    dlopen("2//libcoreclr.so", RTLD_NOW | RTLD_GLOBAL);

    return 0;
}
