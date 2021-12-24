
#!/bin/bash
ps -A -o pid=,stime= |
sort -k2 |
tail -1 |
awk '{
 print $1
}' > src3_data
exit 0
