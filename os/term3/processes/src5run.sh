#!/bin/bash
mkfifo pipe
./src5.sh &
./src5g.sh
rm -rf pipe
