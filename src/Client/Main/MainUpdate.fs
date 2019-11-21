module MainUpdate

open Elmish
open MainModel
open MainMsg
open ExternalMsg

let private updateExternalMsg externalMsg currentModel =
    match externalMsg with
    | NoMsg ->
        currentModel

    | LoginSuccess ->
        { currentModel with IsLoggedIn = true; ActivePage = GroupPhase {Groups=[]} }

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : MainMsg) (currentModel :MainModel) : MainModel * Cmd<MainMsg> =
    match msg, currentModel.ActivePage with
    | LoginMsg loginMsg, _ ->
        let (newLoginModel, newLoginCmd, externalMsg) = LoginUpdate.update loginMsg currentModel.LoginModel

        let newCmd = Cmd.map LoginMsg newLoginCmd
        let newModel = updateExternalMsg externalMsg currentModel

        { newModel with LoginModel = newLoginModel }, newCmd

    | GroupPhaseMsg groupMsg, ActivePage.GroupPhase page ->
        
        let (newGroupModel, newLoginCmd, externalMsg) = GroupPhaseUpdate.update groupMsg page

        let newCmd = Cmd.map GroupPhaseMsg newLoginCmd
        let newModel = updateExternalMsg externalMsg currentModel
        
        { newModel with ActivePage = GroupPhase newGroupModel }, newCmd

    | _ ->
        failwith "Not matched!"
        