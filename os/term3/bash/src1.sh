#!/bin/bash
if [[ $1 -gt $2 ]]
 then
  if [[ $1 -gt $3 ]]
   then
    echo $1
   else
    echo $3
  fi
elif [[ $2 -gt $3 ]]
 then
  echo $2
 else
  echo $3
fi
exit 0
