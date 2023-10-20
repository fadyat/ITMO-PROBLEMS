#!/bin/bash
id="$(cat id)"
[ $# -ne 1 ] && exit
[ ! -f $PWD/"$1" ] && exit
[ ! -d ~/trash ] && mkdir ~/trash
new=~/trash/$id
ln $PWD/"$1" $new
rm $PWD/"$1"
echo "$PWD@$1@$id" >> ~/.trash.log
((id++))
echo "$id" > id

