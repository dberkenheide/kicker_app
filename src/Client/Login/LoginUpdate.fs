module LoginUpdate

open Elmish
open Elmish.React
open Thoth.Fetch
open Thoth.Json
open Fable.Core

open LoginModel
open LoginMsg
open Shared

let tryLogin (credentials: Credentials) : JS.Promise<LoginResult> =
    promise {
        let url = "/api/login"
        return! Fetch.post(url, credentials, isCamelCase = true)
    }

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | LoginClick -> 
        let credentials = {UserName = currentModel.UserName.Value; Password = currentModel.Password.Value}
        let cmd = Cmd.OfPromise.either tryLogin credentials (fun result -> (LoginSuccess result)) (fun er -> LoginFailed (er.ToString()))
        currentModel, cmd

    | SignUpClick -> currentModel, Cmd.none

    | ForgotPasswordClick -> currentModel, Cmd.none

    | UserChanged userName -> { currentModel with UserName = userName }, Cmd.none

    | PasswordChanged password -> { currentModel with Password = password }, Cmd.none

    | LoginSuccess loginResult ->
        { currentModel with UserName = Some loginResult.Token }, Cmd.none

    | LoginFailed loginFailed -> { currentModel with UserName = Some loginFailed }, Cmd.none