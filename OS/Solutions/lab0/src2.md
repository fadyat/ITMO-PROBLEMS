#!/bin/bash
total=""
s=""
bad="q"
while [ "$s" != "$bad" ]
do
	read s
	if [[ "$s" != "$bad" ]]
	then total+=$s
	fi
done
echo $total
exit 0
