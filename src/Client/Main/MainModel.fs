module MainModel

open Shared
open Elmish
open Elmish.React

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

let initialCounter () = Fetch.fetchAs<int> "/api/init"

// The model holds data that you want to keep track of while the application is running
// in this case, we are keeping track of a counter
// we mark it as optional, because initially it will not be available from the client
// the initial value will be requested from server
type MainModel = 
    { 
        LoginModel: LoginModel.Model
    }


