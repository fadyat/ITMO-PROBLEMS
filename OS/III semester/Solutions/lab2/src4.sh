#!/bin/bash
first=true
for pid in $(ps -A -o pid); do
 statusFile="/proc/$pid/status"
 schedFile="/proc/$pid/sched"
 ppid=$(grep -s 'PPid' $statusFile | awk '{print $2}')
 runtime=$(grep -s 'sum_exec_runtime' $schedFile | awk '{print $3}')
 switches=$(grep -s 'nr_switches' $schedFile | awk '{print $3}')
 if [[ $ppid != "" ]] && [[ $runtime != "" ]] && [[ $switches != "" ]]; then
  art=`bc -l <<< "($runtime) / ($switches)" | awk '{ printf("%f\n", $0) }'`
  if [[ $first == true ]]; then
   echo "$pid $ppid $art" > tmp
   first=false
  else
   echo "$pid $ppid $art" >> tmp
  fi
 fi
done
sort -n -k2 tmp |
awk '{
 print "ProcessID=" $1 " : Parent_ProcessID=" $2 " : Average_Running_Time=" $3
}' > src4_data
rm tmp
exit 0
