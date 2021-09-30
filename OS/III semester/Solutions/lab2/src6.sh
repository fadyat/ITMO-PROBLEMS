# Use command 'top' if u want to check the correctness
# Press 'x' to see the column which they are sorted
# Use '<' & '>' to change the column
# RES - is ur column

#!/bin/bash
first=true
for pid in $(ps -A -o pid); do
 statusFile="/proc/$pid/status"
 ram=$((grep -s 'VmRSS' $statusFile) | awk '{ print $2 }')
 if [[ $ram != "" ]]; then
  if [[ $ first == true ]]; then
   echo $pid $ram > tmp
   first=false
  else
   echo $pid $ram >> tmp
  fi
 fi
done
sort tmp -nk2 | tail -1 | awk '{ print $1}' > src6_data
rm tmp
exit 0
