#!/bin/bash
cnt=0
lst=()
echo "" > report1.log
while true; do
 lst+=(1 1 1 1 1 1 1 1 1 1)
 if [[ $cnt%100000 -eq 0 ]]; then
  echo ${#lst[@]} >> report1.log
 fi
 let cnt=$cnt+1
done
