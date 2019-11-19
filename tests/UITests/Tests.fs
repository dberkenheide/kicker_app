module UITests.Tests

open canopy.classic
open Expecto
open System.IO

let tests =
    testList "client tests" 
        [
            testCase "sound check - server is online" (fun () ->
                // startApp ()
                let subject = "Hello World"
                Expect.equal subject "Hello World!" "The strings should equal"
            )

            testCase "login with test user" (fun () ->
                // startApp ()
                // login ()
                // logout ()        
                ()
            )
        ]