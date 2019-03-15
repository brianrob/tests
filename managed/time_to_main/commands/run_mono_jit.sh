#!/bin/bash

dotnet ~/src/aspnet-benchmarks/src/BenchmarksDriver/bin/Release/netcoreapp2.1/BenchmarksDriver.dll \
    --server $SERVER_URL \
    --client $CLIENT_URL \
    --no-clean \
    --display-output \
    --jobs https://raw.githubusercontent.com/brianrob/tests/master/managed/time_to_main/config/time_to_main_mono_jit.json \
    --scenario time_to_main_mono_jit
