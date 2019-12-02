#!/bin/bash

dotnet ~/src/aspnet-benchmarks/src/BenchmarksDriver/bin/Release/netcoreapp3.0/BenchmarksDriver.dll \
    --server $SERVER_URL \
    --client $CLIENT_URL \
    --no-clean \
    --jobs https://raw.githubusercontent.com/brianrob/benchmarks/master/src/Benchmarks/benchmarks.te.core-3.0.json \
    --scenario Plaintext-AspNetCore
