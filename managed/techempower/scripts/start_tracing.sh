#!/bin/bash

lttng create techempower_startup
lttng enable-event -k --tracepoint 'sched_process_exec'
lttng enable-event --kernel --syscall socket
lttng add-context -k -t procname
lttng add-context -k -t vpid
lttng start
