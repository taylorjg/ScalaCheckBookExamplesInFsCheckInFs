module ScalaCheckBookExamplesInFsCheckInFs.Chapter4_RunLengthEnc

open FsCheck
open FsCheckUtils
open Gen
open Arb
open NUnit.Framework

let config = Config.VerboseThrowOnFailure

let runLengthEnc (xs: list<'a>): list<int * 'a> =
    let rec loop (xs: list<'a>) (acc: list<int * 'a>) (ope: option<'a>) (pec: int): list<int * 'a> =
        match xs with
            | [] ->
                match ope with
                    | Some(pe) -> List.append acc [(pec, pe)]
                    | _ -> acc
            | e::rest ->
                match ope with
                    | Some(pe) ->
                        if e = pe then loop rest acc ope (pec + 1)
                        else loop rest (List.append acc [(pec, pe)]) (Some(e)) 1
                    | _ -> loop rest acc (Some(e)) 1
    loop xs [] None 0

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
