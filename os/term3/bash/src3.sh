#!/bin/bash
while [ true ]
do
	echo "
	-------------
	| 1. nano   |
	| 2. vi     |
	| 3. links  |
	| 4. exit   |
	------------- "
	read b
	case "$b" in
	 1 ) nano ;;
	 2 ) vi ;;
	 3 ) links ;;
	 4 ) exit 0 ;;
	esac
	clear
done
exit 0
