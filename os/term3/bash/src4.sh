#!/bin/bash
if [[ "$HOME" == "$PWD" ]]
then
 echo "$HOME"
 exit 0
else
 echo "It's not $HOME directory";
 exit 1
fi

