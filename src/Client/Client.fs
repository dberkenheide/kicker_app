module Client

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open MainMsg
open MainModel
open MainView
open MainUpdate

open Shared

let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ]
        [ str txt ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

let init () : MainModel * Cmd<MainMsg> =
    let initialModel = 
        { 
            IsLoggedIn = false
            LoginModel = { UserName = None; Password = None }
        }
    initialModel, Cmd.none

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
