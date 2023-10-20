#!/bin/bash
for pid in $(ps -A -o pid=); do
 statusFile="/proc/$pid/status"
 ram=`(grep -s 'VmRSS' $statusFile) | awk '{ print $2 }'`
 if [[ $ram != "" ]]; then
  echo $pid $ram >> tmp
 fi
done
sort tmp -nk2 | tail -1 | awk '{ print $1 }' > src6_data
rm tmp
exit 0


