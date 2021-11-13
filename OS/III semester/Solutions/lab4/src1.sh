# ! start script from ~

#!/bin/bash
id="$(cat id)"
[ $# -ne 1 ] && exit
prev=$PWD/$1
[ ! -f $prev ] && exit
[ ! -d ~/trash ] && mkdir ~/trash
new=~/trash/$id
ln $prev $new
rm $prev
echo "$prev $id" >> ~/trash.log
((id++))
echo "$id" > id
