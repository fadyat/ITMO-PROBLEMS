#!/bin/bash
prevPlayer=0
let firstPlayerNumber=$RANDOM%900+100
let secondPlayerNumber=$RANDOM%900+100
echo "| b c | num | crt p |"
echo "---------------------"
allNumbers=()
for i in 1 2 .. 1001; do
 allNumbers+=(0)
done

while [ true ]; do
 let playerNumber=$RANDOM%2+1
 [ $playerNumber -eq $prevPlayer ] && continue;

 let number=$RANDOM%900+100
 [ "${allNumbers[$number]}" == "1" ] && continue;
 allNumbers[$number]=1
 
 cnt=$(echo $number | grep -o . | sort -u | wc -l)
 [ $cnt -ne 3 ] && continue;

 [ "1" == "$playerNumber" ] && pickedNumber=$firstPlayerNumber
 [ "2" == "$playerNumber" ] && pickedNumber=$secondPlayerNumber

 echo $pickedNumber $number $playerNumber > pipe
 prevPlayer=$playerNumber
done

