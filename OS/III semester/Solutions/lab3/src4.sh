#!/bin/bash
cpulimit --limit 10 ./src4g.sh &
cpulimit --limit 5 ./src4g.sh &
./src4g.sh &
kill $!
