#!/bin/bash

iterations=10
echo "----------------------------------------------------------------"
echo "Running $iterations iterations (Plus 1 warm-up iteration.)."
echo "Command: $RUNCMD"
echo "----------------------------------------------------------------"

for (( i=0; i<=$iterations; i++ ))
do
    echo "Iteration $i"
    $RUNCMD
done

echo "----------------------------------------------------------------"
echo "Finished."
echo "----------------------------------------------------------------"
