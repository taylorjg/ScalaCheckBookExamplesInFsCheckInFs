module Chapter6_BuiltInTestCaseSimplification

open FsCheck
open Arb
open Gen
open Prop
open NUnit.Framework

[<StructuredFormatDisplay("{DisplayValue}")>]
type Expression
    = Const of int
    | Add of Expression * Expression
    | Mul of Expression * Expression

let rec show expr =
    match expr with
        | Const n -> sprintf "%d" n
        | Add (e1, e2) -> sprintf "(%s + %s)" (show e1) (show e2)
        | Mul (e1, e2) -> sprintf "(%s * %s)" (show e1) (show e2)

type Expression with
    member public this.DisplayValue = show this

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

let propRewrite expr =
    eval (rewrite expr) = eval expr

[<Test>]
let testRewrite() =
    let config = { Config.QuickThrowOnFailure with
                    MaxTest = 1000;
                    StartSize = 1000;
                    EveryShrink = Config.VerboseThrowOnFailure.EveryShrink }
    Check.One (config, propRewrite)
