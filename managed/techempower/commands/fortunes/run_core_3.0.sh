#!/bin/bash

dotnet ~/src/aspnet-benchmarks/src/BenchmarksDriver/bin/Release/netcoreapp2.1/BenchmarksDriver.dll \
    --server $SERVER_URL \
    --client $CLIENT_URL \
    --jobs https://raw.githubusercontent.com/aspnet/Benchmarks/master/src/Benchmarks/benchmarks.html.json \
    --scenario DbFortunesRaw \
    --database PostgresQL
