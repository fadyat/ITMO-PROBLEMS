#!/bin/bash
echo $$ > .pid
result=1
operation="+"

add(){
 operation="+"
}

multiply(){
 operation="*"
}

terminate(){
 operation="deadinside"
}

trap 'add' USR1
trap 'multiply' USR2
trap 'terminate' SIGTERM

while true; do
 case "$operation" in
  "+") result=`echo "$result + 2" | bc` ;;
  "*") result=`echo "$result * 2" | bc` ;;
  "deadinside") echo "Done"; exit ;;
 esac
 echo $result
 sleep 3
done
