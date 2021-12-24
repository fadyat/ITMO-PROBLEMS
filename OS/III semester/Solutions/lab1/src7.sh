#!/bin/bash
touch emails.lst
grep --text -s -h -R -E -o "\b[[:alnum:]\_\.\-]+@[[:alpha:]]+\.[[:alpha:]]{2,6}\b" /etc/ |
sort |
uniq -c |
awk '{
 printf("%s%s", $2, ", ")
}' > emails.lst
sed -i "s/..$//" emails.lst
echo -e >> emails.lst
exit 0
