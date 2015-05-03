module ScalaCheckBookExamplesInFsCheckInFs.Chapter4_RunLengthEncTests

open FsCheck
open FsCheckUtils
open Gen
open Arb
open NUnit.Framework
open Chapter4_RunLengthEnc

let config = Config.VerboseThrowOnFailure

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
