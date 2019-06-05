#!/bin/bash
set -x

MEMLIMIT=''
SWAPLIMIT=''
CLIENTTHREADS=''
CONNECTIONS=''

while [[ $# -gt 0 ]]
do
    key="$1"

    case $key in
        -m|--memory)
            MEMLIMIT="$2"
            shift
            shift
            ;;
        -s|--swap)
            SWAPLIMIT="$2"
            shift
            shift
            ;;
        --clientThreads)
            CLIENTTHREADS="$2"
            shift
            shift
            ;;
        --connections)
            CONNECTIONS="$2"
            shift
            shift
            ;;
    esac
done

if [[ "$MEMLIMIT" -ne "" ]] && [[ "$SWAPLIMIT" -ne "" ]];
then
    MEMARG="--memory=${MEMLIMIT}m --memory-swap=${SWAPLIMIT}m"
fi
if [[ "$CLIENTTHREADS" -ne "" ]];
then
    CLIENTTHREADSARG="--clientThreads $CLIENTTHREADS"
fi
if [[ "$CONNECTIONS" -ne "" ]];
then
    CONNECTIONSARG="--connections $CONNECTIONS"
fi

dotnet ~/src/aspnet-benchmarks/src/BenchmarksDriver/bin/Release/netcoreapp2.1/BenchmarksDriver.dll \
    --server $SERVER_URL \
    --client $CLIENT_URL \
    --no-clean \
    --jobs https://raw.githubusercontent.com/brianrob/benchmarks/master/src/Benchmarks/benchmarks.te.core-3.0.json \
    --scenario Plaintext-AspNetCore \
    --arg "$MEMARG" \
    $CLIENTTHREADSARG \
    $CONNECTIONSARG \
