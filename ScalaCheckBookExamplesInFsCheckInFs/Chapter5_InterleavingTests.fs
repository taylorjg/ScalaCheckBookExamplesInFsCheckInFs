module ScalaCheckBookExamplesInFsCheckInFs.Chapter5_InterleavingTests

open FsCheck
open NUnit.Framework
open Chapter5_Interleaving
open System.Linq

let config = Config.VerboseThrowOnFailure

let property (xs: list<int>) (ys: list<int>): Property =
    let res = interleave xs ys
    let is = [0..((min xs.Length ys.Length) - 1)]
    let p1 = "length" @| (xs.Length + ys.Length = res.Length)
    let p2 = "zip xs" @| (xs = List.map (fun i -> List.nth res (2*i)) is @ (Enumerable.Skip(res, 2*ys.Length) |> Seq.toList))
    let p3 = "zip ys" @| (ys = List.map (fun i -> List.nth res (2*i+1)) is @ (Enumerable.Skip(res, 2*xs.Length) |> Seq.toList))
    p1 .&. p2 .&. p3

[<Test>]
let interleavingTest() =
    Check.One (config, property)
