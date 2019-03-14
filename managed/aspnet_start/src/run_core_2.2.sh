#!/bin/bash

iterations=10
echo "----------------------------------------------------------------"
echo "Running $iterations iterations of Core 2.2 (Plus 1 warm-up iteration.)."
echo "----------------------------------------------------------------"

for (( i=0; i<=$iterations; i++ ))
do
    echo "Iteration $i"
    dotnet bin_core_2.2/Release/netcoreapp2.2/aspnet_start.dll
done

echo "----------------------------------------------------------------"
echo "Finished."
echo "----------------------------------------------------------------"
