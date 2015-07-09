module Chapter6_CustomTestCaseSimplification

open FsCheck
open Arb
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
        | Add (Const 1, e) -> e
        // | Add (Const 0, e) -> e
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

let shrinkExpr expr =
    match expr with
        | Const n -> shrink n |> Seq.map Const
        | Add (e1, e2) -> Seq.concat [
                            [e1; e2] |> List.toSeq;
                            shrink e1 |> Seq.map (fun e -> Add (e, e2));
                            shrink e2 |> Seq.map (fun e -> Add (e1, e))]
        | Mul (e1, e2) -> Seq.concat [
                            [e1; e2] |> List.toSeq;
                            shrink e1 |> Seq.map (fun e -> Mul (e, e2));
                            shrink e2 |> Seq.map (fun e -> Mul (e1, e))]

type MyGenerators =
  static member Expression() =
    { new Arbitrary<Expression>() with
          override x.Generator = genExpr
          override x.Shrinker t = shrinkExpr t
    }

let propRewrite expr =
    eval (rewrite expr) = eval expr

[<Test>]
let testRewrite() =
    Arb.register<MyGenerators>() |> ignore
    let config = { Config.QuickThrowOnFailure with
                    MaxTest = 1000;
                    StartSize = 20;
                    EveryShrink = Config.VerboseThrowOnFailure.EveryShrink }
    Check.One (config, propRewrite)
