#!/bin/bash

iterations=10
echo "----------------------------------------------------------------"
echo "Running $iterations iterations of CoreCLR (Plus 1 warm-up iteration.)."
echo "----------------------------------------------------------------"

for (( i=0; i<=$iterations; i++ ))
do
    echo "Iteration $i"
    dotnet bin_dotnet/Release/netcoreapp2.2/time_to_main.dll
done

echo "----------------------------------------------------------------"
echo "Finished."
echo "----------------------------------------------------------------"
