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

### F#

```F#
open System
open Microsoft.FSharp.Collections
open Microsoft.FSharp.Core

let pipeline_operator list =
    list
    |> Seq.filter (fun x -> x > 0)
    |> Seq.map (fun x -> Convert.ToInt32(Math.Floor(Math.Sqrt(x))) + x)
    |> Seq.filter (fun x -> x % 2 = 0)
    |> Seq.isEmpty

//let result = pipeline_operator [1; 4; 5; 6]
//printf $"%b{result}"

type Composition = string
type Amount = double

type Raisins(have: bool) =
    member this.have = have

    override this.ToString() =
        if this.have = true then
            "with raisins"
        else
            "without raisins"

type WrapperType =
    | Paper
    | Foil
    | Empty of Nullable

type Yummy =
    | Sweet of (Composition * Amount * WrapperType)
    | Cookie of Composition
    | Muffin of (Composition * Raisins)
    | Empty of Nullable


let pick yummy =
    match yummy with
    | Yummy.Sweet (composition, amount, wrapperType) -> printfn $"{amount} sweets with {composition} in {wrapperType}"
    | Yummy.Cookie composition -> printfn $"Delicious {composition} cookie"
    | Yummy.Muffin (composition, raisins) -> printfn $"{composition} muffin {raisins}"
    | _ -> failwith "todo"

//do pick (Yummy.Muffin("Chocolate", Raisins(true)))

type LoggingBuilder() =
    let log p = printfn $"Expression is %A{p}"

    member this.Bind(x, f) =
        log x
        f x

    member this.Return(x) = x

//let logger = LoggingBuilder()
//let loggedWorkflow =
//    logger {
//        let! x = "Bebra"
//        let! y = "El-Primo"
//        let! z = x + " <3 " + y
//        return z
//    }

```

- We will use [shrarplab.io](https://sharplab.io/) for decompiling F# > C#
- F# > IL-code > C#

### Scala
```Scala
package com.artyomfadeyev

import Main.PipeOperator._
import Main.DiscriminatedUnions._
import Main.Generators._

object Main {
  object PipeOperator {
    implicit class Pipe[T](t: T) {
      def |>[S](f: T => S): S = f(t)
    }

    def filter[T](f: T => Boolean): List[T] => List[T] = _.filter(f)

    def map[A, B](f: A => B): List[A] => List[B] = _.map(f)
  }

  object DiscriminatedUnions {
    class Composition(composition: String) {
      override def toString: String = composition
    }

    class Amount(amount: Double) {
      override def toString: String = amount.toString
    }

    class Raisins(raisins: Boolean) {
      override def toString: String = {
        if (raisins) "with raisins" else "without raisins"
      }
    }

    sealed trait Yummy

    case class Sweet(composition: Composition, amount: Amount, wrapperType: WrapperType) extends Yummy

    case class Cookie(composition: Composition) extends Yummy

    case class Muffin(composition: Composition, raisins: Raisins) extends Yummy

    case object Empty extends Yummy

    sealed trait WrapperType

    case class Paper() extends WrapperType

    case class Foil() extends WrapperType

    case object None extends WrapperType

    def pick(yummy: Yummy): String = yummy match {
      case Sweet(composition, amount, wrapperType) => s"$amount sweets with $composition in $wrapperType"
      case Cookie(composition) => s"Delicious $composition cookie"
      case Muffin(composition, raisins) => s"$composition muffin $raisins"
      case _ => ""
    }
  }

  object Generators {
    case class User(name: String, age: Int)

    def splitUsers(users: List[User]): List[User] =
      for (user <- users
           if user.age >= 18 && user.age <= 24)
      yield user
  }

  def main(args: Array[String]): Unit = {
    def pipeline_operator(list: List[Int]): Int =
      list.sorted |>
        filter(x => x % 2 == 0) |>
        map(x => x + 1) |>
        (x => x.head) |>
        (x => x + 1)

    val result = pipeline_operator(List(1, 3, 4, 5, 6))
    println(result)
    
    println(List(1, 3, 4, 5, 6).filter(x => x % 2 == 0)
      .map(x => x + 1)
      .map(x => x * x))

    val wrapper = Muffin(new Composition("Chocolate"), new Raisins(false))
    println(pick(wrapper))

    val users = List(
      User("Artyom", 19),
      User("Sergo", 20),
      User("Vladimir", 44),
      User("Alyona", 18)
    )

    splitUsers(users) foreach (
      user => println(user)
      )
  }
}
```

- We will use [javadecompilers](http://www.javadecompilers.com/) for decompiling Scala > Java
- Create .jar with `scalac Main.scala -d Main.jar`

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

        <!-- Link to my local directory with packages -->
        <add key="Documents" value="/Users/artyomfadeyev/Documents/packages" />
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

public static class Sorts
{
    public static void bubbleSort(List<int> lst)
    {
        var n = lst.Count;
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

    public static void mergeSort(List<int> lst, int l, int r)
    {
        if (l == r)
            return;

        var m = (l + r) / 2;

        mergeSort(lst, l, m);
        mergeSort(lst, m + 1, r);

        var new_lst = new List<int>();
        for (int i = l, j = m + 1; i <= m || j <= r;)
        {
            if (i > m)
                new_lst.Add(lst[j++]);
            else if (j > r)
                new_lst.Add(lst[i++]);
            else if (lst[i] <= lst[j])
                new_lst.Add(lst[i++]);
            else
                new_lst.Add(lst[j++]);
        }

        for (var i = 0; i < new_lst.Count; i++)
            lst[l + i] = new_lst[i];
    }

    public static void standardSort(List<int> lst)
    {
        lst.Sort();
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
    private readonly List<int> _lst;

    public SortsBenchmark()
    {
        var random = new Random();
        // var n = rnd.Next(0, Convert.ToInt32(1e4));
        const int n = 10000;
        _lst = new List<int>();
        for (var i = 0; i < n; i++)
        {
            _lst.Add(random.Next());
        }
    }

    [Benchmark]
    public void mergeSort()
    {
        var lst = new List<int>(_lst);
        Sorts.mergeSort(lst, 0, lst.Count - 1);
    }

    [Benchmark]
    public void standardSort()
    {
        var lst = new List<int>(_lst);
        Sorts.standardSort(lst);
    }

    [Benchmark]
    public void bubbleSort()
    {
        var lst = new List<int>(_lst);
        Sorts.bubbleSort(lst);
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

|       Method |         Mean |     Error |    StdDev | Rank |     Gen 0 | Allocated |
|------------- |-------------:|----------:|----------:|-----:|----------:|----------:|
| standardSort |     362.1 us |   3.00 us |   2.51 us |    1 |   18.5547 |     39 KB |
|    mergeSort |   1,358.0 us |  13.38 us |  11.86 us |    2 | 1138.6719 |  2,329 KB |
|   bubbleSort | 133,028.1 us | 931.28 us | 871.12 us |    3 |         - |     39 KB |


### Java

- Create Java project with Maven
- Update `pom.xml`:
```xml
...

<properties>
    <jmh.version>1.21</jmh.version>
</properties>

<dependencies>
    <dependency>
        <groupId>org.openjdk.jmh</groupId>
        <artifactId>jmh-core</artifactId>
        <version>${jmh.version}</version>
    </dependency>
    <dependency>
        <groupId>org.openjdk.jmh</groupId>
        <artifactId>jmh-generator-annprocess</artifactId>
        <version>${jmh.version}</version>
    </dependency>
</dependencies>
```

- Install JMH plugin

Sorts.java
```Java
package com.artyomfadeyev;

import java.util.ArrayList;
import java.util.Collections;

public class Sorts {
    public static void bubbleSort(ArrayList<Integer> lst) {
        int n = lst.size();
        for (int i = 0; i < n - 1; i++) {
            for (int j = 0; j < n - i - 1; j++) {
                if (lst.get(j) > lst.get(j + 1)) {
                    int temp = lst.get(j);
                    lst.set(j, lst.get(j + 1));
                    lst.set(j + 1, temp);
                }
            }
        }
    }

    static void mergeSort(ArrayList<Integer> vec, int l, int r) {
        if (l == r)
            return;

        int m = (l + r) / 2;

        mergeSort(vec, l, m);
        mergeSort(vec, m + 1, r);

        ArrayList<Integer> new_vec = new ArrayList<>();
        for (int i = l, j = m + 1; i <= m || j <= r; ) {
            if (i > m) {
                new_vec.add(vec.get(j++));
            } else if (j > r) {
                new_vec.add(vec.get(i++));
            } else if (vec.get(i) <= vec.get(j)) {
                new_vec.add(vec.get(i++));
            } else {
                new_vec.add(vec.get(j++));
            }
        }

        for (int i = 0; i < new_vec.size(); i++)
            vec.set(l + i, new_vec.get(i));
    }


    public static void standardSort(ArrayList<Integer> lst) {
        Collections.sort(lst);
    }
}

```

SortsBenchmark.java
```Java
package com.artyomfadeyev;

import org.openjdk.jmh.annotations.*;

import java.util.ArrayList;
import java.util.Random;

@State(Scope.Benchmark)
@Fork(value = 1)
@Warmup(iterations = 2)
@Measurement(iterations = 2)
public class SortsBenchmark {
    private ArrayList<Integer> arr;

    @Setup
    public void setUp() {
        Random random = new Random();
//        var n = random.nextInt(10000);
        var n = 10000;
        arr = new ArrayList<>();
        for (int i = 0; i < n; i++) {
            arr.add(random.nextInt());
        }
    }

    @Benchmark
    public void mergeSort() {
        var lst = new ArrayList<>(arr);
        Sorts.mergeSort(lst, 0, lst.size() - 1);
    }

    @Benchmark
    public void standardSort() {
        var lst = new ArrayList<>(arr);
        Sorts.standardSort(lst);
    }

    @Benchmark
    public void bubbleSort() {
        var lst = new ArrayList<>(arr);
        Sorts.bubbleSort(lst);
    }
}

```

Main.java
```Java
package com.artyomfadeyev;

public class Main {
    public static void main(String[] args) {

    }
}
```

|Benchmark                   |  Mode  | Cnt  |   Score|   Error|  Units|
| --- | --- | --- | --- | --- | --- |
| SortsBenchmark.bubbleSort  |  thrpt  |  2 |    3.100    | |      ops/s|
| SortsBenchmark.mergeSort    | thrpt |   2  | 538.312      | |   ops/s|
| SortsBenchmark.standardSort  |thrpt|    2  |1161.172       | |   ops/s|

## 5. Code analysis

> Используя инструменты dotTrace, dotMemory, всё-что-угодно-хоть-windbg, проанализировать работу написанного кода для бекапов. Необходимо написать сценарий, когда в цикле будет выполняться много запусков, будут создаваться и удаляться точки. Проверить два сценария: с реальной работой с файловой системой и без неё. В отчёте неоходимо проанализировать полученные результаты, сделать вывод о написанном коде. Опционально: предложить варианты по модернизации или написать альтернативную имплементацию.

## 6. .NET Runtime

> Ознакомиться с исходным кодом dotnet runtime (‣). Склонировать репозиторий, собрать его локально. Вспомнить какого метода вам не хватало в стандартной библиотеке при выполнении лабораторных на ООП и добавить его. Собрать с добавленным методом, убедиться, что он работает.


