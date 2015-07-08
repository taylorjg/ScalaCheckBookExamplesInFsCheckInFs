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

[<Test>]
let doSample() =
    let xs = sample 10 10 genExpr
    List.map (printfn "%A") xs |> ignore
