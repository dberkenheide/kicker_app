module GroupPhaseUpdate

open Elmish

open ExternalMsg
open GroupPhaseModel
open GroupPhaseMsg

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> * ExternalMsg =
    match msg with
    | Refresh ->
        currentModel, Cmd.none, NoMsg
    