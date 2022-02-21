# Hello World

## 1. Interop

> Изучить механизм интеропа между языками, попробовать у себя вызывать C/C++ (Не C++/CLI) код (суммы чисел достаточно) из Java и C#. В отчёте описать логику работы, сложности и ограничения этих механизмов.

- Создадим C++ файл с функцией для суммы чисел:

### C++
```cpp
extern "C" int sum(int a, int b) {
    return a + b;
}
```

- Напишем скрипт который скомпилирует наш **.cpp** в **.dylib**

```bash
#!/bin/bash
g++ --verbose -dynamiclib -o sum.dylib sum.cpp
```

### C#

- Создадим проект C# 

```C#
using System.Runtime.InteropServices;

namespace Project
{
    internal static class Example
    {
        [DllImport("sum.dylib")]
        private static extern int sum(int a, int b);
        
        public static void Main()
        {
            Console.WriteLine(sum(a:5, b:10));
        }
    }
}
```

- Перекинем **.dylib** в **Project/bin/Debug/net6.0**


### Java

- Создадим проект Java

```Java
package com.company;

public class Main {

    native int sum(int a, int b);

    static {
	System.load("/Users/artyomfadeyev/IdeaProjects/Project/Java_Main_sum.dylib");
    }

    public static void main(String[] args) {
        System.out.println(new Main().sum(5, 3));
    }
}
```

- Перекинем файл в удобное нам место, например, в папку с проектом и укажем его путь.


## 2. Functional programming

> Написать немного кода на Scala **и** F# с использованием уникальных возможностей языка - Pipe operator, Discriminated Union, Computation expressions и т.д. . Вызвать написанный код из обычных соответствующих ООП языков (Java **и** С#) и посмотреть во что превращается написанный раннее код после декомпиляции в них.

...

## 3. Packages

> Написать алгоритм обхода графа (DFS и BFS) на языке Java, собрать в пакет и опубликовать (хоть в Maven, хоть в Gradle, не имеет значения). Использовать в другом проекте на Java/Scala этот пакет. Повторить это с C#/F#. В отчёте написать про алгоритм работы пакетных менеджеров, особенности их работы в C# и Java мирах.

### Kotlin
- Create Gradle Project > Kotlin
- Write the name of the organization that will be used in our package 'com.artyomfadeyev'
- Write the name of the project 'graphsAlgo'
- Create a package named 'com.artyomfadeyev.graphAlgo' inside the 'Kotlin' folder.
- Create your files in the package folder

Graph.kp
```Kotlin
import java.util.LinkedList
import java.util.Queue

class Graph(size: Int) {
    private val n: Int
    private val graph: MutableList<MutableList<Int>>

    init {
        n = size
        graph = MutableList(n) { mutableListOf() }
    }

    fun addEdge(v: Int, u: Int) {
        graph[v].add(u)
    }

    fun bfs(v: Int) {
        val queue: Queue<Int> = LinkedList()
        val d = Array(n) { 0 }
        val used = Array(n) { false }

        queue.add(v)
        used[v] = true
        while (!queue.isEmpty()) {
            val u: Int = queue.poll()
            for (to in graph[u]) {
                if (!used[to]) {
                    used[to] = true
                    d[to] = d[u] + 1
                    queue.add(to)
                }
            }
        }
    }

    fun dfs(v: Int) {
        val used = Array(n) { false }
        val d = Array(n) { 0 }

        fun dfsExec(v: Int) {
            used[v] = true
            graph[v].forEach {
                if (!used[it]) {
                    d[it] = d[v] + 1
                    dfsExec(it)
                }
            }
        }

        dfsExec(v)
    }

    fun print() {
        graph.forEach {
            it.forEach { k -> print(k) }
            println()
        }
    }
}
```

Main.kp
```Kotlin
@file:JvmName("Main")
package com.artyomfadeyev.graphsAlgo

fun main() {

}
```

build.gradle:
```gradle
plugins {
    id 'org.jetbrains.kotlin.jvm' version '1.5.10'
    id 'application'
}

group 'com.artyomfadeyev'
version '1.0-SNAPSHOT'

mainClassName = 'com.artyomfadeyev.graphsAlgo.Main'

repositories {
    mavenCentral()
}

jar {
    manifest {
        attributes 'Main-Class': mainClassName
    }
}

dependencies {
    implementation "org.jetbrains.kotlin:kotlin-stdlib"
}
```

- Add an identifier to the plugins that tells Gradle that we are creating an application.
- Write 'mainClassName' and 'jar.manifest' to understand the Java entry point.
- Click **gradle/build/assemble** to create a snapshot with our programm, he will be located in **/build/libs/**
- Create new project
- Go to **File > Project Structure > Dependencies** and add your .jar file
- Now you can use `import com.artyomfadeyev.graphAlgo` in your new project

### C#
- Pick your project and right click on it
- Go to **Properties > NuGet** and configure your package settings
- Write `dotnet pack` at console.
- We can find your package at **./bin/Debug/ProjectName.nupkg**
- Create new project
- Go to NuGet/Sources/Feeds click '+' and add new package OR go to 'NuGet.Config.xml' and add new packageSource

Graph.cs
```C#
namespace Graphs;

public class Graph
{
    private int _n;
    private readonly List<List<int>> _graph;

    public Graph(int n)
    {
        _n = n;
        _graph = new List<List<int>>();
        for (var i = 0; i < n; i++)
        {
            _graph.Add(new List<int>());
        }
    }

    public void AddEdge(int v, int u)
    {
        _graph[v].Add(u);
    }

    public void Dfs(int v)
    {
        // ...
    }

    public void Bfs(int v)
    {
        // ...
    }

    public void Print()
    {
        // ...  
    }
}
```

Main.cs
```C#
namespace Graphs;

public static class Program
{
    public static void Main()
    {
        // ...
    }
}
```

NuGet.Config.xml
```xml
<?xml version="1.0" encoding="utf-8"?>

<configuration>
    <packageSources>
        <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />

        - Link to my local directory with packages
        <add key="Documents" value="/Users/artyomfadeyev/Documents/packages" /> /
    </packageSources>
</configuration>
```

## 4. Benchmarking

> Изучить инструменты для оценки производительности в C# и Java. Написать несколько алгоритмов сортировок (и взять стандартную) и запустить бенчмарки (в бенчмарках помимо времени выполнения проверить аллокации памяти). В отчёт написать про инструменты для бенчмаркинга, их особености, анализ результатов проверок.

### C#
- Create classes at project

Sorts.cs
```C#
namespace Example;

public class Sorts
{
    public static void bubbleSort(int[] lst)
    {
        var n = lst.Length;
        for (var i = 0; i < n - 1; i++)
        {
            for (var j = 0; j < n - i - 1; j++)
            {
                if (lst[j] > lst[j + 1])
                {
                    (lst[j], lst[j + 1]) = (lst[j + 1], lst[j]);
                }
            }
        }
    }

    private static void merge(int[] lst, int l, int m, int r)
    {
        var left = new int[m - l + 1];
        var right = new int[r - m];
        
        Array.Copy(lst, l, left, 0, m - l + 1);
        Array.Copy(lst, m + 1, right, 0, r - m);

        var i = 0;
        var j = 0;
        for (var k = l; k < r + 1; k++)
        {
            if (i == left.Length)
            {
                lst[k] = right[j++];
            }
            else if (j == right.Length)
            {
                lst[k] = left[i++];
            }
            else if (left[i] <= right[j])
            {
                lst[k] = left[i++];
            }
            else
            {
                lst[k] = right[j++];
            }
        }
    }

    public static void mergeSort(int[] lst, int l = 0, int r = -1)
    {
        if (r == -1) r = lst.Length - 1;
        if (l >= r) return;
        var m = (l + r) / 2;

        mergeSort(lst, l, m);
        mergeSort(lst, m + 1, r);
        merge(lst, l, m, r);
    }

    public static void standartSort(int[] lst)
    {
        Array.Sort(lst);
    }

    public static void print(IEnumerable<int> list)
    {
        foreach (var i in list)
        {
            Console.Write($"{i} ");
        }
        Console.WriteLine();
    }
}
```

SortsBenchmark.cs
```C#
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Example;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class SortsBenchmark
{
    private readonly int[] _list;

    public SortsBenchmark()
    {
        var rnd = new Random();
        var n = rnd.Next(0, Convert.ToInt32(1e4));

        _list = new int[n];
        for (var i = 0; i < n; i++)
        {
            _list[i] = rnd.Next(0, int.MaxValue);
        }
    }

    [Benchmark]
    public void mergeSort()
    {
        var lst = new int[_list.Length];
        _list.CopyTo(lst, 0);
        Sorts.mergeSort(_list);
    }
    
    [Benchmark]
    public void standartSort()
    {
        var lst = new int[_list.Length];
        _list.CopyTo(lst, 0);
        Sorts.standartSort(_list);
    }
    
    [Benchmark]
    public void bubbleSort()
    {
        var lst = new int[_list.Length];
        _list.CopyTo(lst, 0);
        Sorts.bubbleSort(_list);
    }
}
```

Program.cs
```C#
using BenchmarkDotNet.Running;
using Example;


BenchmarkRunner.Run<SortsBenchmark>();
```

- Build project `dotnet build -c Release`
- Run benchmarking `dotnet /Users/artyomfadeyev/RiderProjects/Benchmarking/Example/bin/Release/net6.0/Example.dll`

|       Method |         Mean |      Error |     StdDev | Rank |    Gen 0 | Allocated |
|------------- |-------------:|-----------:|-----------:|-----:|---------:|----------:|
| standartSort |     3.818 us |  0.0172 us |  0.0161 us |    1 |   1.9836 |      4 KB |
|    mergeSort |   178.300 us |  0.3572 us |  0.3166 us |    2 | 224.6094 |    459 KB |
|   bubbleSort | 7,599.588 us | 20.9108 us | 19.5600 us |    3 |        - |     16 KB |

### Kotlin




## 5. Code analysis

> Используя инструменты dotTrace, dotMemory, всё-что-угодно-хоть-windbg, проанализировать работу написанного кода для бекапов. Необходимо написать сценарий, когда в цикле будет выполняться много запусков, будут создаваться и удаляться точки. Проверить два сценария: с реальной работой с файловой системой и без неё. В отчёте неоходимо проанализировать полученные результаты, сделать вывод о написанном коде. Опционально: предложить варианты по модернизации или написать альтернативную имплементацию.

## 6. .NET Runtime

> Ознакомиться с исходным кодом dotnet runtime (‣). Склонировать репозиторий, собрать его локально. Вспомнить какого метода вам не хватало в стандартной библиотеке при выполнении лабораторных на ООП и добавить его. Собрать с добавленным методом, убедиться, что он работает.


