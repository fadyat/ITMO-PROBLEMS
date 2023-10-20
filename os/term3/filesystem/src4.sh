#!/bin/bash
actualBackup=`ls /home/user | grep -E "^Backup-" | sort -n | tail -1`
restore=/home/user/restore
[ ! -d $restore ] && mkdir $restore
path=/home/user/$actualBackup
backupData=`ls $path`
for file in $backupData; do
 copy=`echo $file | grep -E -o "[[:alnum:]._-]{1,}.[[:digit:]]{4}-[[:digit:]]{2}-[[:digit:]]{2}"`
 [ "$copy" == "$file" ] && continue
 cp $path/$file $restore
done

ls $restore
