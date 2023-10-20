#!/bin/bash
correctLine="#!/bin/bash"
for file in $PWD/*
do
 line=$(head -n 1 $file)
 if [[ $line == $correctLine ]]
  then
   tmp=`echo "$file" | sed 's/\.[[:lower:]]\{1,\}//g'`
   if [[ "$file" != "$tmp.sh" ]]
   then
     mv "$file" "$tmp.sh"
   fi
 fi
done
exit 0
