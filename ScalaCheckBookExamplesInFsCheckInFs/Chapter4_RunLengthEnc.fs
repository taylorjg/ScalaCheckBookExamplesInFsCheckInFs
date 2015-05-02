module ScalaCheckBookExamplesInFsCheckInFs.Chapter4_RunLengthEnc

open FsCheck
open FsCheckUtils
open Gen
open Arb
open NUnit.Framework

let config = Config.VerboseThrowOnFailure

let runLengthEnc (xs: seq<'a>): seq<int * 'a> = Seq.empty

let runLengthDec (r: seq<int * 'a>): seq<'a> =
    Seq.collect (fun (n, x) -> List.replicate n x) r

let genOutput: Gen<seq<int * char>> =
    let rleItem: Gen<int * char> = gen {
        let! n = choose (1, 20)
        let! c = GenExtensions.AlphaNumChar
        return (n, c)
    }
    let rec rleList (size: int): Gen<seq<int * char>> =
        if (size <= 1) then map Seq.singleton rleItem
        else gen {
            let! xs = rleList (size - 1)
            let (_, c1) = Seq.head xs
            let tail = Seq.skip 1 xs
            let! head = suchThat (fun (_, c2) -> c2 <> c1) rleItem
            return Seq.append (Seq.singleton head) tail
        }
    sized rleList

[<Test>]
let runLengthEncTest() =
    let arb = fromGen genOutput
    let property = Prop.forAll arb (fun r -> runLengthEnc(runLengthDec(r)) = r)
    Check.One (config, property)
