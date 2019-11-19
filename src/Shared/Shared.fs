module Shared

let noneWhenEmpty str =
    if System.String.IsNullOrWhiteSpace str then None else Some str            