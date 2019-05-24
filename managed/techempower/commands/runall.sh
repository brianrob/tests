#!/bin/bash

echo "Running PlainText JIT"
for run in {1..10}; do ./run_mono_jit.sh; done
echo "Running PlainText JIT LLVM"
for run in {1..10}; do ./run_mono_jit_llvm.sh; done
echo "Running PlainText JIT FX AOT APP JIT LLVM"
for run in {1..10}; do ./run_mono_fx_aot_app_jit_llvm.sh; done
echo "Running PlainText AOT LLVM"
for run in {1..10}; do ./run_mono_aot_llvm.sh; done
echo "Running PlainText AOT NOLLVM"
for run in {1..10}; do ./run_mono_aot_nollvm.sh; done
echo "Running PlainText FULLAOT"
for run in {1..10}; do ./run_mono_fullaot.sh; done
echo "Running PlainText FULLAOT LLVM"
for run in {1..10}; do ./run_mono_fullaot_llvm.sh; done

echo "Running Fortunes JIT"
for run in {1..10}; do fortunes/run_mono_jit.sh; done
echo "Running Fortunes JIT LLVM"
for run in {1..10}; do fortunes/run_mono_jit_llvm.sh; done
echo "Running Fortunes FX AOT APP JIT LLVM"
for run in {1..10}; do fortunes/run_mono_fx_aot_app_jit_llvm.sh; done
echo "Running Fortunes AOT LLVM"
for run in {1..10}; do fortunes/run_mono_aot_llvm.sh; done
echo "Running Fortunes AOT NO LLVM"
for run in {1..10}; do fortunes/run_mono_aot_nollvm.sh; done
echo "Running Fortunes FULLAOT"
for run in {1..10}; do fortunes/run_mono_fullaot.sh; done
echo "Running Fortunes FULLAOT LLVM"
for run in {1..10}; do fortunes/run_mono_fullaot_llvm.sh; done
