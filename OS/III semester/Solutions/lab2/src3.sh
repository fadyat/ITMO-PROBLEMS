#!/bin./bash
ps -A -o pid=,stime= |
sort -r -k2 |
head -1 |
tail -1 |
awk '{ print $1 }'
exit 0
