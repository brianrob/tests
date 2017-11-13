#undef TRACEPOINT_PROVIDER
#define TRACEPOINT_PROVIDER test_provider

#undef TRACEPOINT_INCLUDE
#define TRACEPOINT_INCLUDE "./tp.h"

#if !defined(_TP_H) || defined(TRACEPOINT_HEADER_MULTI_READ)
#define _TP_H

#include <lttng/tracepoint.h>

/*
 * Use TRACEPOINT_EVENT(), TRACEPOINT_EVENT_CLASS(),
 * TRACEPOINT_EVENT_INSTANCE(), and TRACEPOINT_LOGLEVEL() here.
 */

#endif /* _TP_H */

#include <lttng/tracepoint-event.h>

TRACEPOINT_EVENT(
    test_provider,
    test_tracepoint,
    TP_ARGS(
        int, int_val,
        const char*, str_val
    ),
    TP_FIELDS(
        ctf_integer(int, int_val, int_val)
        ctf_string(str_val, str_val)
    )
)
