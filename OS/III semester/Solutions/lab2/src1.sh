#!/bin/bash
touch src1_data
touch tmp
ps -U "root" -o pid,comm |
sed 's/^ \{1,\}//g' |
sed 's/ /:/g' > tmp
wc -l tmp |
awk '{
 print "Processes: " $1 "\n"
}' > src1_data
cat tmp >> src1_data
rm tmp
exit 0
