#!/bin/bash

iterations=10
echo "----------------------------------------------------------------"
echo "Running $iterations iterations of Mono (Plus 1 warm-up iteration.)."
echo "----------------------------------------------------------------"

for (( i=0; i<=$iterations; i++ ))
do
    echo "Iteration $i"
    mono bin_mono/Release/net471/linux-x64/publish/aspnet_start.exe
done

echo "----------------------------------------------------------------"
echo "Finished."
echo "----------------------------------------------------------------"
