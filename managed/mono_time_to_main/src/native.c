/**
 * Provide a native function that managed code can call to perform a "marker" syscall.
 */

#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <unistd.h>

void write_marker(const char * name)
{
    int descriptor = open(name, O_RDONLY);
    if(descriptor != -1)
    {
        close(descriptor);
    }
}
