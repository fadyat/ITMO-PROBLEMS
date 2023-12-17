#!/bin/bash

git submodule update --init --recursive

git fetch origin
git submodule foreach --recursive 'git fetch origin'

git submodule foreach --recursive 'git checkout master && git pull origin master'

echo "Commit information for submodules:"
git submodule foreach --recursive '
    echo "Submodule: $name"
    git --no-pager log -n 5 --oneline
    echo "---------------------------"
'
