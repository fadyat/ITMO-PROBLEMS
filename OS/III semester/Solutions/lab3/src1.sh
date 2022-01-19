#!/bin/bash
dateTime=$(date "+%d-%m-%Y_%T")
mkdir ~/test && echo "catalog test was created successfully" > ~/report &&
	touch ~/test/$dateTime
ping -c 1 https://www.net_nikogo.ru || echo "$dateTime net_nikogo.ru doesn't exist!" >> ~/report
