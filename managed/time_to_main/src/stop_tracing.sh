#!/bin/bash

sudo lttng stop time_to_main
sudo lttng view | grep mono | grep 'sched_process_exec\|/function/main'
sudo lttng destroy time_to_main
