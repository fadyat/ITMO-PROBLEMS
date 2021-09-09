#!/bin/bash
touch full.log
sed -n '/(WW) w/!s/(WW)/Warning/p' /var/log/anaconda/X.log > full.log
sed -n '/(II) i/!s/(II)/Information/p' /var/log/anaconda/X.log >> full.log
cat full.log
exit 0
