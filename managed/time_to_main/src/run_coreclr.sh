#!/bin/bash

iterations=10

echo "Running $iterations iterations of CoreCLR."

for i in {1..$iterations}
do
    dotnet bin_dotnet/Release/netcoreapp2.2/time_to_main.dll
done
