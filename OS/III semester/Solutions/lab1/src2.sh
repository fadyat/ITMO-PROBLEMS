#!/bin/bash
touch src2_data
ps -A -o pid,cmd |
grep "/sbin" |
awk '{
 print $1
}' > src2_data
exit 0
