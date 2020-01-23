module Login

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Shared.Dtos
open System
open Thoth.Json
open Fable.Core.JsInterop

type Model = {
    Login: Login
    Running: bool
    ErrorMsg: string option
  }

type InternMsg =
  | SetUserName of string
  | SetPassword of string
  | LogInClicked
  | LoginHandled of Result<UserData, string>
  | ErrorMsgDiscarded

type Msg =
  | Intern of InternMsg
  | LoginSuccess of UserData

let initModel () : Model * Cmd<Msg> =
  {
    Login = { UserName = ""; Password = "" }
    Running = false
    ErrorMsg = None
  }, Cmd.none

let authUser (login: Login) =
  ServerApi.api.login login

let update (msg: InternMsg) model : Model * Cmd<Msg> =
  match msg with
  | SetUserName name ->
      { model with Login = { model.Login with UserName = name } }, Cmd.none

  | SetPassword pw ->
      { model with Login = { model.Login with Password = pw } }, Cmd.none

  | LogInClicked ->
      { model with Running = true }, Cmd.OfAsync.perform authUser model.Login (LoginHandled >> Intern)

  | LoginHandled loginResult ->
      match loginResult with
      | Ok user ->
          { model with Running = false }, user |> LoginSuccess |> Cmd.ofMsg
      | Error err ->
          { model with Running = false; ErrorMsg = Some err }, Cmd.none

  | ErrorMsgDiscarded ->
      { model with ErrorMsg = None }, Cmd.none

let view (model: Model) (dispatch: Msg -> unit) =
  let buttonIsDisabled m =
      String.IsNullOrEmpty m.Login.UserName ||
      String.IsNullOrEmpty m.Login.Password ||
      m.Running

  div [ CustomStyles.centerStyle "column" ] [
    Container.container [ ]
      [ yield Box.box' [ Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [
              figure [ Class "avatar" ] [
                img [ Src "/lmis-ag-logo.svg" ]
              ]
              div [] [
                Field.div [ ] [
                  Control.div [ ]
                        [ Input.text
                            [ Input.Size IsLarge
                              Input.Placeholder "Dein Kürzel"
                              Input.OnChange (fun ev -> dispatch (ev.Value |> SetUserName |> Intern))
                              Input.Props [ AutoFocus true ] ] ] ]
                Field.div [ ] [
                  Control.div [ ] [
                    Input.password  [
                      Input.Size IsLarge
                      Input.OnChange (fun ev -> dispatch (ev.Value |> SetPassword |> Intern))
                      Input.Placeholder "Dein Passwort"
                    ]
                  ]
                ]
                br [ ]
                Button.button [
                  Button.Color IsPrimary
                  Button.IsFullWidth
                  Button.OnClick (fun _ -> dispatch (LogInClicked |> Intern))
                  Button.Disabled (buttonIsDisabled model)
                ] [
                  str "Login"
                ]
              ]
            ]

        match model.ErrorMsg with
        | Some err ->
            yield Message.message [ Message.Color IsDanger ]
              [ Message.header [] [
                  str "Login fehlgeschlagen"
                  Delete.delete [ Delete.OnClick (fun _ -> dispatch (ErrorMsgDiscarded |> Intern)) ] [ ]
                ]
                Message.body [ ] [ str err ]
              ]
        | None -> ()
      ]
    ]