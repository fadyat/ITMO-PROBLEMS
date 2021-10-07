#!/bin/bash
dateTime=$(date + "%d-%m-%y_%T")
mkdir ~/test && echo "catalog test was created successfully" > ~/report &&
    touch ~/test/$dateTime
web="www.net_nikogo.ru"
ping -c 1 $web || echo $dateTime $web "doesn't exist!" >> ~/report
exit 0
