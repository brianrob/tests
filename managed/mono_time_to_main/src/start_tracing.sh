#!/bin/bash

sudo lttng create time_to_main
sudo lttng enable-event -k --tracepoint 'sched_process_exec'
sudo lttng enable-event --kernel --syscall open
sudo lttng add-context -k -t procname
sudo lttng add-context -k -t vpid
sudo lttng start
