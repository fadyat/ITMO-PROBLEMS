#!/bin/bash
ps -U "root" -o pid=,comm= | awk '{ print $1 ":" $2 }' > tmp
wc -l tmp | awk '{ print "Processes: " $1 }' > src1_data
cat tmp >> src1_data
rm tmp
exit 0
