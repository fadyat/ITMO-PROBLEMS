#!/bin/bash
[ $# -ne 1 ] && exit
while read line; do
 path=`echo $line | awk -F '@' '{ print $1 }'`
 name=`echo $line | awk -F '@' '{ print $2 }'`
 id=`echo $line | awk -F '@' '{ print $3 }'`
 echo "$line" >> ~/trash.logU
 [ "$name" != "$1" ] && continue
 [ ! -f ~/trash/$id ] && continue
 printf "\nRestore file %s? " "$path/$name"
 read status < /dev/tty
 [ $status != "y" ] && continue

 [ ! -d `dirname $path` ] &&
        printf "\tLast dir doesn't exist! File will be saved in /root\n" &&
        path=~

 while true; do
  [ ! -f $path/"$name" ] && break

  printf "\tRename, \"$name\" is already exist: "
  read name </dev/tty
 done

 echo "New file location: $path/$name"
 ln ~/trash/$id $path/"$name"
 rm ~/trash/$id
 cat ~/trash.logU | head -n -1 > ~/trash.logU
done < ~/.trash.log

cat ~/trash.logU > ~/.trash.log
rm ~/trash.logU
