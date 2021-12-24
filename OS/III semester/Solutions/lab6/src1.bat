md lab5
systeminfo > systeminfo.txt
taskinfo > taskinfo.txt
wmic diskdrive get > diskinfo.txt

md test
copy /y . .\test

type nul > totalinfo.txt
copy /y .\test\* .\totalinfo.txt

rmdir /q /s lab5