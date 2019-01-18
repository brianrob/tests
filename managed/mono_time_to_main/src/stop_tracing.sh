#!/bin/bash

lttng stop time_to_main
lttng view | grep mono | grep sched_process_exec
lttng view | grep mono | grep /function/main
lttng destroy time_to_main
