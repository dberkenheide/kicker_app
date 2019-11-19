module LoginUpdate

open Elmish
open Elmish.React

open LoginModel
open LoginMsg

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | LoginClick -> currentModel, Cmd.none
    | SignUpClick -> currentModel, Cmd.none
    | ForgotPasswordClick -> currentModel, Cmd.none
    | UserChanged userName -> { currentModel with UserName = userName }, Cmd.none
    | PasswordChanged password -> { currentModel with Password = password }, Cmd.none