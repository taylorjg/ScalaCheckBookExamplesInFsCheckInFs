module Chapter6_CustomTestCaseSimplification

open FsCheck
open Gen
open Prop
open NUnit.Framework

type Expression
    = Const of int
    | Add of Expression * Expression
    | Mul of Expression * Expression

let rec eval expr =
    match expr with
        | Const n -> n
        | Add (e1, e2) -> eval e1 + eval e2
        | Mul (e1, e2) -> eval e1 * eval e2

let rewrite e = 
    match e with
         Add (e1, e2) when e1 = e2 -> Mul (Const 2, e1)
        | Mul (Const 0, _) -> Const 0
        // "Add (Const 1, e) -> e" is a bug. Use it instead of
        // "Add (Const 0, e) -> e" to demonstrate shrinking.
        // | Add (Const 1, e) -> e
        | Add (Const 0, e) -> e
        | _ -> e

#nowarn "40"
let rec genExpr =
    sized <| fun sz ->
        let k = sz - (int <| sqrt (float sz))
        frequency [
            (1, genConst);
            (k, resize (sz/2) genAdd);
            (k, resize (sz/2) genMul)]

and genConst =
    choose (0, 10) |> map Const

and genAdd =
    gen {
        let! e1 = genExpr
        let! e2 = genExpr
        return Add (e1, e2)
    }

and genMul =
    gen {
        let! e1 = genExpr
        let! e2 = genExpr
        return Mul (e1, e2)
    }

type MyGenerators =
  static member Expression() =
    { new Arbitrary<Expression>() with
          override x.Generator = genExpr
          // That's interesting. I haven't implemented shrinking but it still shrinks anyway!
          override x.Shrinker t = Seq.empty
    }

Arb.register<MyGenerators>() |> ignore

let propRewrite expr =
    eval (rewrite expr) = eval expr

[<Test>]
let testRewrite() =
    let config = { Config.QuickThrowOnFailure with MaxTest = 1000 }
    Check.One (config, propRewrite)
