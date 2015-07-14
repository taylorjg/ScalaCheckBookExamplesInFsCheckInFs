
## Description

The book, [_ScalaCheck: The Definitive Guide_](http://www.artima.com/shop/scalacheck), contains some nice examples
of property-based testing. The idea of this repo is to port some of these
examples from Scala / ScalaCheck to F# / FsCheck 2.

## Progress

Below is a list of examples that I have converted so far:

* Chapter 4
    * Constructing optimal output - run length encoding (pages 45-46)
        * [Chapter4_RunLengthEnc.fs](https://github.com/taylorjg/ScalaCheckBookExamplesInFsCheckInFs/blob/master/ScalaCheckBookExamplesInFsCheckInFs/Chapter4_RunLengthEnc.fs)
        * [Chapter4_RunLengthEncTests.fs](https://github.com/taylorjg/ScalaCheckBookExamplesInFsCheckInFs/blob/master/ScalaCheckBookExamplesInFsCheckInFs/Chapter4_RunLengthEncTests.fs)
        * (_original example code_ can be found within [here](http://booksites.artima.com/scalacheck/examples/html/ch04.html#sec6))
* Chapter 5
    * Labelling properties - interleaving (pages 51-52)
        * [Chapter5_Interleaving.fs](https://github.com/taylorjg/ScalaCheckBookExamplesInFsCheckInFs/blob/master/ScalaCheckBookExamplesInFsCheckInFs/Chapter5_Interleaving.fs)
        * [Chapter5_InterleavingTests.fs](https://github.com/taylorjg/ScalaCheckBookExamplesInFsCheckInFs/blob/master/ScalaCheckBookExamplesInFsCheckInFs/Chapter5_InterleavingTests.fs)
        * (_original example code_ can be found within [here](http://booksites.artima.com/scalacheck/examples/html/ch05.html#sec1))
* Chapter 6
    * Custom test case simplification (pages 82-87)
        * [Chapter6_CustomTestCaseSimplification.fs](https://github.com/taylorjg/ScalaCheckBookExamplesInFsCheckInFs/blob/master/ScalaCheckBookExamplesInFsCheckInFs/Chapter6_CustomTestCaseSimplification.fs)
        * (_original example code_ can be found within [here](http://booksites.artima.com/scalacheck/examples/html/ch06.html#sec2))
        * In my port of this example, I used a discriminated union, `Expression`. It turns out that FsCheck can automatically generate and shrink such types - see [Chapter6_BuiltInTestCaseSimplification.fs](https://github.com/taylorjg/ScalaCheckBookExamplesInFsCheckInFs/blob/master/ScalaCheckBookExamplesInFsCheckInFs/Chapter6_BuiltInTestCaseSimplification.fs) where I have removed the custom generating and shrinking code.

## Links

* http://booksites.artima.com/scalacheck
* http://booksites.artima.com/scalacheck/ScalaCheckExamples.zip
* https://github.com/fsharp/FsCheck
* https://fsharp.github.io/FsCheck/
* [Port of the examples in the ScalaCheck book to C# and FsCheck 2.0](https://github.com/taylorjg/ScalaCheckBookExamplesInFsCheck2)
* [Port of the examples in the ScalaCheck book to Haskell and QuickCheck](https://github.com/taylorjg/ScalaCheckBookExamplesInQuickCheck)
