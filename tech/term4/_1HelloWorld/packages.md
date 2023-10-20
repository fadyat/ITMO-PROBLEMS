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
