#!/bin/bash
total=""
s=""
bad="q"

while [ "$s" != "$bad" ]
do
 total+=$s
 read s
done

echo $total
exit 0
