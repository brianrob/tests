#!/bin/bash

if [ -z "$PROCESSNAME" ];
then
    echo "Set PROCESSNAME environment variable before running."
    exit
fi

lttng stop techempower_startup
lttng view | grep $PROCESSNAME | grep 'sched_process_exec\|syscall_exit_socket'
lttng destroy techempower_startup
