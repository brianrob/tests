#!/bin/bash

dotnet ~/src/aspnet-benchmarks/src/BenchmarksDriver/bin/Release/netcoreapp2.1/BenchmarksDriver.dll \
    --server $SERVER_URL \
    --client $CLIENT_URL \
    --no-clean \
    --display-output \
    --jobs https://raw.githubusercontent.com/brianrob/tests/master/managed/aspnet_start/config/aspnet_start_mono_aot+jit.json \
    --scenario aspnet_start_mono_aot+jit
