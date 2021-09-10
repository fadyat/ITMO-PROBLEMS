#!/bin/bash
find /var/log/ -name "*.log" | xargs wc -l
exit 0
