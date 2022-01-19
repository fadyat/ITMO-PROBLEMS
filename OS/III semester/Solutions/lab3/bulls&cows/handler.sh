#!/bin/bash
(tail -f pipe) |
while [ true ]; do
 read line
 pickedNumber=$(echo $line | awk '{ print $1 }')
 number=$(echo $line | awk '{ print $2 }')
 player=$(echo $line | awk '{ print $3 }')

 let bulls=0
 let cows=0

 for i in 1 2 3; do
  s1=$(expr substr $pickedNumber $i 1)
  s2=$(expr substr $number $i 1)
  [ $s1 == $s2 ] && ((bulls++));

  if [[ "$pickedNumber" == *"$s2"* ]]; then
   ((cows++))
  fi
 done

 let cows=cows-bulls

 echo "| $bulls $cows | $number | $pickedNumber $player |"

 [ $bulls -eq 3 ] && echo "Player $player  wins!" && killall "generator.sh" && exit
done
