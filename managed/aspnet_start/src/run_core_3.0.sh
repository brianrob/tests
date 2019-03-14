#!/bin/bash

iterations=10
echo "----------------------------------------------------------------"
echo "Running $iterations iterations of Core 3.0 (Plus 1 warm-up iteration.)."
echo "----------------------------------------------------------------"

for (( i=0; i<=$iterations; i++ ))
do
    echo "Iteration $i"
    dotnet bin_core_3.0/Release/netcoreapp3.0/aspnet_start.dll
done

echo "----------------------------------------------------------------"
echo "Finished."
echo "----------------------------------------------------------------"
