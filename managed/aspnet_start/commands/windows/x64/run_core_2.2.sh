#!/bin/bash

dotnet ~/src/aspnet-benchmarks/src/BenchmarksDriver/bin/Release/netcoreapp2.1/BenchmarksDriver.dll \
    --server $SERVER_URL \
    --client $CLIENT_URL \
    --no-clean \
    --console \
    --display-output \
    --jobs https://raw.githubusercontent.com/brianrob/tests/master/managed/aspnet_start/config/windows/x64/aspnet_start_core_2.2.json \
    --scenario aspnet_start_core_2.2
