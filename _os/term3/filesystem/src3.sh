#!/bin/bash
prevTime=`ls /home/user | grep -E "^Backup-" | sort -n | tail -1 | cut -d '-' -f 2,3,4`
prevTimeSeconds=0
[ $prevTime ] && prevTimeSeconds=`date -d "$prevTime" +%s`
curTime=`date +%Y-%m-%d`
curTimeSeconds=`date -d "$curTime" +%s`
daysAgo=`echo "($curTimeSeconds - $prevTimeSeconds) / 60 / 60 / 24" | bc`

directory=/home/user/Backup-$prevTime
[ $daysAgo -gt 7 ] && { directory=/home/user/Backup-$curTime; mkdir $directory; }
source=/home/user/source

for file in `ls $source`; do
 [ ! -f $directory/$file ] && {
  cp $source/$file $directory
  echo " New file: $file" >> newFiles
  continue
 }
 prevSize=`ls -l $directory/$file | awk '{ print $5 }'`
 curSize=`ls -l $source/$file | awk '{ print $5 }'`
 [ $curSize -eq $prevSize ] && continue
 mv $directory/$file $directory/$file.$curTime
 cp $source/$file $directory
 echo " Rename: $file to $file.$curTime" >> renamedFiles
done

[ ! -f newFiles ] && [ ! -f renamedFiles ] && {
 echo "Nothing changed"
 exit
}

if [ $daysAgo -gt 7 ]; then
 echo "New backup: Backup-$curTime" >> /home/user/backup-report
 cat newFiles >> /home/user/backup-report
else
 echo "Update backup: Backup-$prevTime" >> /home/user/backup-report
 cat newFiles >> /home/user/backup-report
 cat renamedFiles >> /home/user/backup-report
fi

cat /home/user/backup-report
[ -f newFiles ] && rm newFiles
[ -f renamedFiles ] && rm renamedFiles
