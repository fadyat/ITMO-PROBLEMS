#!/bin./bash
ps -A -o pid=,stime= |
sort -nk2 |
tail -1 |
awk '{ print $1 }'
exit 0
