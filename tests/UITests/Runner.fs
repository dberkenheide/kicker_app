// Learn more about F# at http://fsharp.org

open System

//open canopy.classic
//open canopy.types
open Expecto

open UITests.Tests


let startBrowser() =
    //start Chrome // Use this if you want to see your tests in the browser
    //start ChromeHeadless
    //resize (1280, 960)
    ()

[<EntryPoint>]
let main args =   
    try
        startBrowser()
        runTestsWithArgs { defaultConfig with ``parallel`` = false } args tests
    with e ->
        printfn "Error: %s" e.Message
        -1
        //quit()
