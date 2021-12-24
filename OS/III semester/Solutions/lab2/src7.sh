#!/bin/bash
for pid in $(ps -A -o pid=); do
 etime=`ps -p $pid -o etimes=`
 seconds=60
 if [[ $etime -le $seconds ]]; then
  io="/proc/$pid/io"
  rdBytes=`grep -s "^read_bytes:" $io | awk '{ print $2 }'`
  if [[ $rdBytes != "" ]]; then
   cmd=`ps -p $pid -o cmd=`
   echo $pid : $rdBytes : $cmd >> tmp
  fi
 fi
done
sort -nk2 tmp | tail -3 > src7_data
rm tmp
exit 0
