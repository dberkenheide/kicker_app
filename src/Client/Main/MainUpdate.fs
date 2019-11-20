module MainUpdate

open Elmish
open MainModel
open MainMsg

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : MainMsg) (currentModel :MainModel) : MainModel * Cmd<MainMsg> =
    match msg with
    | LoginMsg loginMsg -> 
        match loginMsg with        
        | LoginMsg.LoginSuccess success -> 
            { currentModel with IsLoggedIn = true }, Cmd.none

        | _ ->
            let (newLoginModel, newLoginCmd) = LoginUpdate.update loginMsg currentModel.LoginModel
            let cmdNew = Cmd.map LoginMsg newLoginCmd

            { currentModel with LoginModel = newLoginModel }, cmdNew