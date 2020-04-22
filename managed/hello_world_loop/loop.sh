#!/bin/bash

COUNT=0
START=`date +%s`
echo `date`
while [ $(( $(date +%s) - 3600 )) -lt $START ];
do
    bin/Release/netcoreapp3.1/hello_world_loop > /dev/null
    COUNT=$(($COUNT+1))
    if [ $(( $COUNT % 1000 )) -eq 0 ];
    then
        echo "Current Count = $COUNT"
    fi
done

echo "Final Count = $COUNT"
echo `date`
