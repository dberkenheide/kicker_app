module LoginUpdate

open Elmish
open Elmish.React

open LoginModel
open LoginMsg

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | Login -> currentModel, Cmd.none