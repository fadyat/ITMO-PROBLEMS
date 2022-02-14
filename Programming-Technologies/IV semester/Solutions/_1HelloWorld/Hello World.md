# Hello World

## 1. Interop

> Изучить механизм интеропа между языками, попробовать у себя вызывать C++ код (суммы чисел достаточно) из Java и C#. В отчёте описать логику работы, сложности и ограничения этих механизмов.

- Создадим C++ файл с функцией для суммы чисел:

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

### C++ >> C#

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


### C++ >> Java

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

> Написать немного кода на Scala и F# с использованием функциональных возможностей языка - Pipe operator, Discriminated Union, CE и т.д. . Вызвать написанный код из обычных соответствующих ООП языков (Java/Kotlin и С#) и посмотреть во что превращается написанный раннее код.

...

## 3. Packages

> Написать алгоритм обхода графа (DFS и BFS) на языке Java, собрать в пакет и опубликовать (хоть в Maven, хоть в Gradle, не имеет значения). Использовать в другом проекте на Java/Kotlin этот пакет. Повторить это с F# → C#. В отчёте написать про алгоритм работы пакетных менеджеров, особенности их работы в C# и Java мирах.

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
- Click gradle/build/assemble to create a snapshot with our programm, he will be located in /build/libs/
- Create new project
- Go to File > Project Structure > Dependencies and add your .jar file
- Now you can use `import com.artyomfadeyev.graphAlgo` in your new project

...

## 4. Benchmarking

> Изучить инструменты для оценки производительности в C# и Java. Написать несколько алгоритмов сортировок (и взять стандартную) и запустить бенчмарки (в бенчмарках помимо времени выполнения проверить аллокации памяти). В отчёт написать про инструменты для бенчмаркинга, их особености, анализ результатов проверок.

## 5. Code analysis

> Используя инструменты dotTrace, dotMemory, всё-что-угодно-хоть-windbg, проанализировать работу написанного кода для бекапов. Необходимо написать сценарий, когда в цикле будет выполняться много запусков, будут создаваться и удаляться точки. Проверить два сценария: с реальной работой с файловой системой и без неё. В отчёте неоходимо проанализировать полученные результаты, сделать вывод о написанном коде. Опционально: предложить варианты по модернизации или написать альтернативную имплементацию.

## 6. .NET Runtime

> Ознакомиться с исходным кодом dotnet runtime (‣). Склонировать репозиторий, собрать его локально. Вспомнить какого метода вам не хватало в стандартной библиотеке при выполнении лабораторных на ООП и добавить его. Собрать с добавленным методом, убедиться, что он работает.


