#!/bin./bash
ps -A -o pid,stime |
sort -r -k2 |
head -2 |
awk '{
 print $1
}'
exit 0
