#!/bin/bash
processCmd=$1
for pid in $(ps -A -o pid=); do
 pidFile="/proc/$pid"
 if [[ $pidFile == "" ]]; then continue; fi
 currentProcessCmd=`ps -p $pid -o cmd=`
 if [[ $currentProcessCmd == $processCmd ]]; then
  echo "PID: " $pid
  exit 0
 fi
done
echo "No such process!"
exit 0
