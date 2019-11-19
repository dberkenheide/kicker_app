module ClientTests

open Expecto
open System.IO

open LoginModel
open Shared

let tests =
    testList "client tests" 
        [
            testCase "Login with empty inputs" (fun () ->
                let emptyNameAndPassword = { UserName= None; Password = None }
                Expect.isTrue (emptyNameAndPassword |> hasEmtptyLoginFields) "The fields should be empty."

                let emptyPassword = { emptyNameAndPassword with UserName = (Some "username")}
                Expect.isTrue (emptyPassword |> hasEmtptyLoginFields) "The password should be empty."
                
                let filledNameAndPassword = { emptyPassword with Password = (Some "password")}
                Expect.isFalse (filledNameAndPassword |> hasEmtptyLoginFields) "The fields should not be empty."
            )

            testCase "login with test user" (fun () ->
                // startApp ()
                // login ()
                // logout ()        
                ()
            )
        ]
