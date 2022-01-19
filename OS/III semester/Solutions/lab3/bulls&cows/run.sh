#!/bin/bash
mkfifo pipe
./generator.sh &
./handler.sh
rm -rf pipe
