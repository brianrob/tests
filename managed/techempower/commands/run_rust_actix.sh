#!/bin/bash

dotnet ~/src/aspnet-benchmarks/src/BenchmarksDriver/bin/Release/netcoreapp2.1/BenchmarksDriver.dll \
    --server $SERVER_URL \
    --client $CLIENT_URL \
    --no-clean \
    --jobs https://raw.githubusercontent.com/brianrob/tests/master/managed/techempower/config/rust-actix.json \
    --scenario Plaintext-RustFastHttp \
