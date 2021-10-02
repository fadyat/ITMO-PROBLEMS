<h2 align="center"> #Problems</h2>
<div>
  <a href = "https://github.com/fadyat/ITMO-PROBLEMS/blob/master/OS/III%20semester/Solutions/lab2/src1.sh">1.</a>
  Посчитать количество процессов, запущенных пользователем user, и вывести в файл получившееся
число, а затем пары PID:команда для таких процессов.
</div>
<div>
  <a href = "https://github.com/fadyat/ITMO-PROBLEMS/blob/master/OS/III%20semester/Solutions/lab2/src2.sh">2.</a>
  Вывести в файл список PID всех процессов, которые были запущены командами, расположенными в
/sbin/
</div>
<div>
  <a href = "https://github.com/fadyat/ITMO-PROBLEMS/blob/master/OS/III%20semester/Solutions/lab2/src3.sh">3.</a>
  Вывести на экран PID процесса, запущенного последним (с последним временем запуска).
</div>  
<div>
  <a href = "https://github.com/fadyat/ITMO-PROBLEMS/blob/master/OS/III%20semester/Solutions/lab2/src4.sh">4.</a>
  Для всех зарегистрированных в данный момент в системе процессов определить среднее время
непрерывного использования процессора (CPU_burst) и вывести в один файл строки
ProcessID=PID : Parent_ProcessID=PPID : Average_Running_Time=ART. Значения PPid взять из файлов status, которые находятся в директориях с названиями, соответствующими PID процессов в /proc. Значения ART получить, разделив значение sum_exec_runtime на nr_switches, взятые из файлов sched в этих же директориях. Отсортировать эти строки по идентификаторам родительских процессов.
</div>
<div>
  <a href = "https://github.com/fadyat/ITMO-PROBLEMS/blob/master/OS/III%20semester/Solutions/lab2/src5.sh">5.</a>  
  В полученном на предыдущем шаге файле после каждой группы записей с одинаковым идентификатором родительского процесса вставить строку вида Average_Running_Children_of_ParentID=N is M,
где N = PPID, а M – среднее, посчитанное из ART для всех процессов этого родителя.
</div>
<div>
  <a href = "https://github.com/fadyat/ITMO-PROBLEMS/blob/master/OS/III%20semester/Solutions/lab2/src6.sh">6.</a>
  Используя псевдофайловую систему /proc найти процесс, которому выделено больше всего оперативной памяти. Сравнить результат с выводом команды top.
</div>
<div>
  <a href = "https://github.com/fadyat/ITMO-PROBLEMS/blob/master/OS/III%20semester/Solutions/lab2/src7.sh">7.</a>
  Написать скрипт, определяющий три процесса, которые за 1 минуту, прошедшую с момента запуска скрипта, считали максимальное количество байт из устройства хранения данных. Скрипт должен выводить PID, строки запуска и объем считанных данных, разделенные двоеточием.
</div>

<h2></h2>

<div align=middle>
  <a href = "https://github.com/fadyat/ITMO-PROBLEMS/blob/master/OS/III%20semester/Solutions/lab2/test.sh"> TEST</a>
</div>
