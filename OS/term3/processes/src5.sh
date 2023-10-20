#!/bin/bash
result=1
operation='+'
(tail -f pipe) |
while true; do
 read line
 case "$line" in
  "QUIT")
   echo "Done"
   killall "src5g.sh"
   exit
  ;;
  "+")
   operation='+'
   echo "result + number"
  ;;
  "*")
   operation="*"
   echo "result * number"
  ;;
  *)
   if [[ "$line" =~ ^[+-]?[0-9] ]]; then
    case "$operation" in
     "+") result=`echo "$result+$line" | bc` ;;
     "*") result=`echo "$result*$line" | bc` ;;
    esac
    echo "$result"
   else
    echo "Invalid number!"
   fi
  ;;
esac
done
