#!/bin/bash
[ $# -ne 1 ] && exit
while read line; do
 path=`echo $line | awk '{ print $1 }'`
 name=`basename $path`
 id=`echo $line | awk '{ print $2 }'`
 echo "$line" >> ~/trash.logU
 [ $name != $1 ] && continue
 [ ! -f ~/trash/$id ] && continue
 printf "\nRestore file %s? " $path
 read status </dev/tty
 [ $status != "y" ] && continue

 [ ! -d `dirname $path` ] &&
        printf "\tLast dir doesn't exist! File will be saved in /root\n" &&
        path=~/$name

 while true; do
  [ ! -f $path ] && break

  printf "\tRename, \"$name\" is already exist: "
  read name </dev/tty
  path=`dirname $path`/$name
 done

 echo "New file location: $path"
 ln ~/trash/$id $path
 rm ~/trash/$id
 cat ~/trash.logU | head -n -1 > ~/trash.logU
done < ~/.trash.log

cat ~/trash.logU > ~/.trash.log
rm ~/trash.logU
