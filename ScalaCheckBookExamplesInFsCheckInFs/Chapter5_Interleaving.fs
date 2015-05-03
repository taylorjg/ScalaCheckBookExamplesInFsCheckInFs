module ScalaCheckBookExamplesInFsCheckInFs.Chapter5_Interleaving

let rec interleave (xs: list<'a>) (ys: list<'a>): list<'a> =
    if xs.IsEmpty then ys
    else if ys.IsEmpty then xs
    else xs.Head :: ys.Head :: interleave xs.Tail ys.Tail
