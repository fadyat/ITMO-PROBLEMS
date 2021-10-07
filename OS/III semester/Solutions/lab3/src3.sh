# use crontab [-l] for all crontab process from root
# 	      [-r] for reset all root process

#!/bin/bash
echo "*/5 * * * * ./src1.sh" | crontab
exit 0
