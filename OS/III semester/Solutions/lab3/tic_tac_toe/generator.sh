#!/bin/bash
prevPlayer=0
cp field.txt modField.txt
while [ true ]; do
 let playerNumber=$RANDOM%2+1
 [ $prevPlayer -eq $playerNumber ] && continue;
 let i=$RANDOM%3+1
 let j=$RANDOM%3+1

 if [[ `cat modField.txt | grep '@' | wc -l` == "0" ]]; then
  echo "QUIT QUIT QUIT" > pipe
 fi

 if [[ `cat modField.txt | awk -v ii=$i -v jj=$j 'FNR == ii { print $jj }'` == '@' ]]; then
  echo "$i $j $playerNumber" > pipe
  prevPlayer=$playerNumber
 fi
 sleep 1
done

