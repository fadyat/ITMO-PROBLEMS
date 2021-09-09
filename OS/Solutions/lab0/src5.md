#!/bin/bash
touch info.log
awk '{
if ($2 == "INFO")
 print $0
}' /var/log/anaconda/syslog > info.log
exit 0
