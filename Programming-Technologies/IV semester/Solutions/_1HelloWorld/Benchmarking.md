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
@BenchmarkMode(Mode.All)
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

| Benchmark                                        |    Mode |   Cnt |    Score |   Error | Units |
| ---                                              |  ---    | ---   |  ---     |     --- | ---   |
| SortsBenchmark.mergeSort                         |   thrpt |     2 |  547.473 |         | ops/s |
| SortsBenchmark.bubbleSort                        |    avgt |     2 |    0.297 |         |  s/op |
| SortsBenchmark.standardSort                      |   thrpt |     2 | 1175.338 |         | ops/s |
| SortsBenchmark.mergeSort                         |    avgt |     2 |    0.002 |         |  s/op |
| SortsBenchmark.bubbleSort                        |  sample |    69 |    0.295 |±  0.002 |  s/op |
| SortsBenchmark.standardSort                      |    avgt |     2 |    0.001 |         |  s/op |
| SortsBenchmark.bubbleSort:bubbleSort·p0.00       |  sample |       |    0.284 |         |  s/op |
| SortsBenchmark.bubbleSort:bubbleSort·p0.90       |  sample |       |    0.305 |         |  s/op |
| SortsBenchmark.bubbleSort:bubbleSort·p0.50       |  sample |       |    0.295 |         |  s/op |
| SortsBenchmark.bubbleSort:bubbleSort·p0.95       |  sample |       |    0.307 |         |  s/op |
| SortsBenchmark.bubbleSort:bubbleSort·p0.999      |  sample |       |    0.312 |         |  s/op |
| SortsBenchmark.bubbleSort:bubbleSort·p0.99       |  sample |       |    0.312 |         |  s/op |
| SortsBenchmark.bubbleSort:bubbleSort·p0.9999     |  sample |       |    0.312 |         |  s/op |
| SortsBenchmark.mergeSort                         |  sample | 10947 |    0.002 |±  0.001 |  s/op |
| SortsBenchmark.bubbleSort:bubbleSort·p1.00       |  sample |       |    0.312 |         |  s/op |
| SortsBenchmark.mergeSort:mergeSort·p0.00         |  sample |       |    0.002 |         |  s/op |
| SortsBenchmark.mergeSort:mergeSort·p0.90         |  sample |       |    0.002 |         |  s/op |
| SortsBenchmark.mergeSort:mergeSort·p0.50         |  sample |       |    0.002 |         |  s/op |
| SortsBenchmark.mergeSort:mergeSort·p0.95         |  sample |       |    0.002 |         |  s/op |
| SortsBenchmark.mergeSort:mergeSort·p0.999        |  sample |       |    0.003 |         |  s/op |
| SortsBenchmark.mergeSort:mergeSort·p0.99         |  sample |       |    0.002 |         |  s/op |
| SortsBenchmark.mergeSort:mergeSort·p0.9999       |  sample |       |    0.003 |         |  s/op |
| SortsBenchmark.standardSort                      |  sample | 22326 |    0.001 |±  0.001 |  s/op |
| SortsBenchmark.standardSort:standardSort·p0.00   |  sample |       |    0.001 |         |  s/op |
| SortsBenchmark.standardSort:standardSort·p0.50   |  sample |       |    0.001 |         |  s/op |
| SortsBenchmark.standardSort:standardSort·p0.90   |  sample |       |    0.001 |         |  s/op |
| SortsBenchmark.standardSort:standardSort·p0.95   |  sample |       |    0.001 |         |  s/op |
| SortsBenchmark.standardSort:standardSort·p0.99   |  sample |       |    0.001 |         |  s/op |
| SortsBenchmark.standardSort:standardSort·p0.999  |  sample |       |    0.001 |         |  s/op |
| SortsBenchmark.standardSort:standardSort·p0.9999 |  sample |       |    0.005 |         |  s/op |
| SortsBenchmark.standardSort:standardSort·p1.00   |  sample |       |    0.009 |         |  s/op |
| SortsBenchmark.bubbleSort                        |      ss |     2 |    0.311 |         |  s/op |
| SortsBenchmark.mergeSort                         |      ss |     2 |    0.006 |         |  s/op |
| SortsBenchmark.standardSort                      |      ss |     2 |    0.002 |         |  s/op |
| SortsBenchmark.mergeSort:mergeSort·p1.00         |  sample |       |    0.004 |         |  s/op |