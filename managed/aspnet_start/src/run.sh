#!/bin/bash

iterations=10
echo "----------------------------------------------------------------"
echo "Running $iterations iterations (Plus 1 warm-up iteration.)."
echo "Command: $runcmd"
echo "----------------------------------------------------------------"

for (( i=0; i<=$iterations; i++ ))
do
    echo "Iteration $i"
    $runcmd
done

echo "----------------------------------------------------------------"
echo "Finished."
echo "----------------------------------------------------------------"
