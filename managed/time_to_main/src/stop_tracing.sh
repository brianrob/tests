#!/bin/bash

lttng stop time_to_main
lttng view | grep 'mono\|dotnet' | grep 'sched_process_exec\|/function/main'
lttng destroy time_to_main
