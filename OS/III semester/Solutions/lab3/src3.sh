# use crontab [-l] for all crontab process from root
# 	          [-r] for reset all root process
#             [-e] for all started crontab processes

#!/bin/bash
echo "*/5 * * * Sun /home/user/lab2/src1.sh" | crontab
