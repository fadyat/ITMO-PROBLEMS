# lab-1. Hello world

Задания на лабораторную работу:

1. Изучить механизм интеропа между языками, попробовать у себя вызывать C++ код (суммы чисел достаточно) из Java и C#. В отчёте описать логику работы, сложности и ограничения этих механизмов.

2. Написать немного кода на Scala и F# с использованием функциональных возможностей языка - Pipe operator, Discriminated Union, CE и т.д. . Вызвать написанный код из обычных соответствующих ООП языков (Java/Kotlin и С#) и посмотреть во что превращается написанный раннее код.

3. Написать алгоритм обхода графа (DFS и BFS) на языке Java, собрать в пакет и опубликовать (хоть в Maven, хоть в Gradle, не имеет значения). Использовать в другом проекте на Java/Kotlin этот пакет. Повторить это с F# → C#. В отчёте написать про алгоритм работы пакетных менеджеров, особенности их работы в C# и Java мирах.

4. Изучить инструменты для оценки производительности в C# и Java. Написать несколько алгоритмов сортировок (и взять стандартную) и запустить бенчмарки (в бенчмарках помимо времени выполнения проверить аллокации памяти). В отчёт написать про инструменты для бенчмаркинга, их особености, анализ результатов проверок.

5. Используя инструменты dotTrace, dotMemory, всё-что-угодно-хоть-windbg, проанализировать работу написанного кода для бекапов. Необходимо написать сценарий, когда в цикле будет выполняться много запусков, будут создаваться и удаляться точки. Проверить два сценария: с реальной работой с файловой системой и без неё. В отчёте неоходимо проанализировать полученные результаты, сделать вывод о написанном коде. Опционально: предложить варианты по модернизации или написать альтернативную имплементацию.

6. **Опционально**. Надо будет делать коллективным разумом и с помощью хрустика.  Ознакомиться с исходным кодом dotnet runtime (‣). Склонировать репозиторий, собрать его локально. Вспомнить какого метода вам не хватало в стандартной библиотеке при выполнении лабораторных на ООП и добавить его. Собрать с добавленным методом, убедиться, что он работает.
        