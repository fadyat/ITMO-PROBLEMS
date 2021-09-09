#!/bin/bash

while [ true ]
do
	echo "
	------------
	| 1. vi    |
	| 2. nano  |
	| 3. links |	
	| 4. exit  |
	------------"
	read b
	case "$b" in
	 1 ) vi ;;
	 2 ) nao ;;
	 3 ) links ;;
	 4 ) exit 0 ;;
	esac
	clear
done
exit 0

