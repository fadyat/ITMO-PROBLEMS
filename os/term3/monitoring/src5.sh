#!/bin/bash
prevPpid=`echo "$(head -1 src4_data)" | awk -F: '{print $2}' | sed 's/ Parent_ProcessID=//'`
sum=0; cnt=0
while read line; do
 nowPpid=`echo $line | awk -F: '{ print $2 }' | sed 's/ Parent_ProcessID=//'`
 art=`echo $line | awk -F: '{ print $3 }' | sed 's/ Average_Running_Time=//'`
 if [[ $prevPpid != $nowPpid ]]; then
  avg=`bc -l <<< "($sum) / ($cnt)" | awk '{ printf("%f", $0) }'`
  echo "Average_Running_Children_of_ParentID="$prevPpid"is "$avg
  sum=0; cnt=0
 fi
 sum=`bc -l <<< "($sum) + ($art)" | awk '{ printf("%f", $0) }'`
 prevPpid=$nowPpid
 ((cnt++))
done < src4_data > src5_data
avg=`bc -l <<< "($sum) / ($cnt)" | awk '{ printf("%f", $0) }'`
echo "Average_Running_Children_of_ParentID="$prevPpid"is "$avg >> src5_data
exit 0
