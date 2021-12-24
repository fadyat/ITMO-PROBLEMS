#!/bin/bash
cnt=0
while [ "$cnt" -lt "$2" ]; do
 ./src1new.sh $1 &
 ((cnt++))
 sleep 1
done
