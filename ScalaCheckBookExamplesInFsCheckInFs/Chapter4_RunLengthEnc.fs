module ScalaCheckBookExamplesInFsCheckInFs.Chapter4_RunLengthEnc

open FsCheck
open FsCheckUtils
open Gen
open Arb
open NUnit.Framework

let config = Config.VerboseThrowOnFailure

let runLengthEnc (xs: list<'a>): list<int * 'a> =
    let rec loop (xs: list<'a>) (acc: list<int * 'a>) (pe: option<int * 'a>): list<int * 'a> =
        match xs with
            | [] ->
                match pe with
                    | Some(t) -> acc @ [t]
                    | _ -> acc
            | x::rest ->
                match pe with
                    | Some((n, e) as t) ->
                        if x = e then loop rest acc (Some(n + 1, e))
                        else loop rest (acc @ [t]) (Some(1, x))
                    | _ -> loop rest acc (Some(1, x))
    loop xs [] None

let runLengthDec (r: list<int * 'a>): list<'a> =
    List.collect (fun t -> t ||> List.replicate) r

let genOutput: Gen<list<int * char>> =
    let rleItem: Gen<int * char> = gen {
        let! n = choose (1, 20)
        let! c = GenExtensions.AlphaNumChar
        return (n, c)
    }
    let rec rleList (size: int): Gen<list<int * char>> =
        if (size <= 1) then map (fun x -> [x]) rleItem
        else gen {
            let! tail = rleList (size - 1)
            let (_, c1) = List.head tail
            let! head = suchThat (fun (_, c2) -> c2 <> c1) rleItem
            return head :: tail
        }
    sized rleList

[<Test>]
let runLengthEncTest() =
    let arb = fromGen genOutput
    let property = Prop.forAll arb (fun r -> runLengthEnc(runLengthDec(r)) = r)
    Check.One (config, property)
