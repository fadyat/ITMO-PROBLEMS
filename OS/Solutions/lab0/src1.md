#!/bin/bash
let a=0
if [[ $1 -gt $2 ]]
then a=$1
else a=$2
fi

if [[ $3 -gt $a ]]
then a=$3
fi

echo $a
exit
