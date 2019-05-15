#!/bin/bash

dotnet ~/src/aspnet-benchmarks/src/BenchmarksDriver/bin/Release/netcoreapp2.1/BenchmarksDriver.dll \
    --server $SERVER_URL \
    --client $CLIENT_URL \
    --jobs https://raw.githubusercontent.com/brianrob/benchmarks/master/src/Benchmarks/benchmarks.te.aspnetcore.json \
    --scenario FortunesPostgreSql-AspNetCore \
