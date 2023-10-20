#!/bin/bash
man bash |
grep -o "[[:alpha:]]\{4,\}" |
tr "[:upper:]" "[:lower:]" |
sort |
uniq -c |
sort -n |
tail -3
exit 0
