#include <dlfcn.h>

int main(int argc, char* argv[])
{
    dlopen("1//libcoreclr.so", RTLD_NOW | RTLD_GLOBAL);
    dlopen("2//libcoreclrtraceptprovider.so", RTLD_LAZY);
    dlopen("2//libcoreclr.so", RTLD_NOW | RTLD_GLOBAL);

    return 0;
}
