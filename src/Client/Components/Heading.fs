module Custom.Heading

open Fulma
open Fable.React

let customH3 text = 
    Heading.h3
        [ Heading.Modifiers [ Modifier.TextColor IsGrey ] ]
        [ str text ]

let customP text =
    Heading.p
        [ Heading.Modifiers [ Modifier.TextColor IsGrey ] ]
        [ str text ]