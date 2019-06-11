#/bin/bash

echo "Run: Go"

for mem in {5..25};
do
    for run in {1..3};
    do
        ./run_go_fasthttp_docker.sh -m $mem -s 64
    done
done

#echo "Run: Mono JIT"

#for mem in {5..25};
#do
#    for run in {1..3};
#    do
#        ./run_mono_jit_docker.sh -m $mem -s 64
#    done
#done
