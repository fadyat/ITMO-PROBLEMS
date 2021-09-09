#!/bin/bash
fine /var/log/ -name "*.log" | xargs wc -l
exit 0
