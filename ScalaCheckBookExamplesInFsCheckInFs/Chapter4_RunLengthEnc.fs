module ScalaCheckBookExamplesInFsCheckInFs.Chapter4_RunLengthEnc

let runLengthEnc (xs: list<'a>): list<int * 'a> =
    let rec loop (xs: list<'a>) (pe: option<int * 'a>): seq<int * 'a> =
        seq {
            match (xs, pe) with
                | ([], None) -> yield! Seq.empty
                | ([], Some(t)) -> yield t
                | (x::rest, None) -> yield! loop rest (Some(1, x))
                | (x1::rest, Some((n, x2))) when x1 = x2 -> yield! loop rest (Some(n + 1, x2))
                | (x::rest, Some(t)) -> yield t; yield! loop rest (Some(1, x))
        }
    loop xs None |> Seq.toList

let runLengthDec (r: list<int * 'a>): list<'a> =
    List.collect (fun t -> t ||> List.replicate) r
