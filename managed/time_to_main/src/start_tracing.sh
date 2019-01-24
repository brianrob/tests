#!/bin/bash

lttng create time_to_main
lttng enable-event -k --tracepoint 'sched_process_exec'
lttng enable-event --kernel --syscall open
lttng add-context -k -t procname
lttng add-context -k -t vpid
lttng start
