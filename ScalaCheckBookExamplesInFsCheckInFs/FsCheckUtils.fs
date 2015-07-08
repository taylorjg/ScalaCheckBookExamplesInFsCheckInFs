module FsCheckUtils

open FsCheck.Gen

let private genCharInRange (l, h) = choose (int l, int h) |> map char

let numChar = genCharInRange ('0', '9')
let alphaUpperChar = genCharInRange ('A', 'Z')
let alphaLowerChar = genCharInRange ('a', 'z')

let alphaChar =
    frequency [
        (1, alphaUpperChar);
        (9, alphaLowerChar)]

let alphaNumChar =
    frequency [
        (1, numChar);
        (9, alphaChar)]
