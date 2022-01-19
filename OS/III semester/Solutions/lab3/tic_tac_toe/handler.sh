#!/bin/bash
(tail -f pipe) |
while true; do
 read line
 > tmpFile
 i=`echo "$line" | awk '{ print $1 }'`
 j=`echo "$line" | awk '{ print $2 }'`
 [ $i == "QUIT" ] && killall "generator.sh" && exit
 playerNumber=`echo "$line" | awk '{ print $3 }'`

 cnt=1
 while read l; do
  [ $cnt -eq $i ] && pickedLine=$l && break
  echo $l >> tmpFile
  ((cnt++))
 done < modField.txt

 firstPart=$(echo "$pickedLine" | head -c $(echo "($j-1)*2" | bc))
 secondPart=$(echo "$playerNumber ")
 thirdPart=$(echo "$pickedLine" | tail -c $(echo "2*(3-$j)" | bc))

 echo $firstPart$secondPart$thirdPart >> tmpFile
 cnt=1
 while read l; do
  [ $cnt -le $i ] && ((cnt++)) && continue
  echo $l >> tmpFile
 done < modField.txt

 cat tmpFile > modField.txt
 cat modField.txt
 echo " "
done
rm tmpFile
