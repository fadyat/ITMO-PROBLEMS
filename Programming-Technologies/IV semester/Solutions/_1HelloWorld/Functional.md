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
- Or go to /target/.../Main.scala > Decompile Scala to Java
