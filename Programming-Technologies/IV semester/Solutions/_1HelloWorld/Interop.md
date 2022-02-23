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

