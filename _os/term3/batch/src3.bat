sc query type=service state=active | find "SERVICE_NAME" > services.txt

sc stop dnscache
timeout /t 5
sc query type=service state=active | find "SERVICE_NAME" > newservices.txt
sc start dnscache

fc /c .\services.txt .\newservices.txt > difference.txt