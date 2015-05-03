module ScalaCheckBookExamplesInFsCheckInFs.Chapter4_RunLengthEnc

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

