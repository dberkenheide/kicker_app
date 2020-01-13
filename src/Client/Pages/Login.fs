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
  | AuthError of exn
  | LogInClicked

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
  ServerApi.auth.login login

let update (msg: InternMsg) model : Model * Cmd<Msg> =
  match msg with
  | SetUserName name ->
      { model with Login = { model.Login with UserName = name } }, Cmd.none

  | SetPassword pw ->
      { model with Login = { model.Login with Password = pw } }, Cmd.none

  | LogInClicked ->
      { model with Running = true }, Cmd.OfAsync.either authUser model.Login LoginSuccess (AuthError >> Intern)

  | AuthError exn ->
      { model with Running = false; ErrorMsg = Some exn.Message }, Cmd.none

let view (model: Model) (dispatch: Msg -> unit) =
  let buttonIsDisabled m =
      String.IsNullOrEmpty m.Login.UserName ||
      String.IsNullOrEmpty m.Login.Password ||
      m.Running

  Container.container [ ] [
    Box.box' [ Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
          [ figure [ Class "avatar" ]
              [ img [ Src "/lmis-ag-logo.svg" ] ]
            div [ ]
              [ Field.div [ ]
                  [ Control.div [ ]
                      [ Input.text
                          [ Input.Size IsLarge
                            Input.Placeholder "Dein Kürzel"
                            Input.OnChange (fun ev -> dispatch (ev.Value |> SetUserName |> Intern))
                            Input.Props [ AutoFocus true ] ] ] ]
                Field.div [ ]
                  [ Control.div [ ]
                      [ Input.password
                          [ Input.Size IsLarge
                            Input.OnChange (fun ev -> dispatch (ev.Value |> SetPassword |> Intern))
                            Input.Placeholder "Dein Passwort" ] ] ]
                br [ ]
                Button.button
                  [ Button.Color IsPrimary
                    Button.IsFullWidth
                    Button.OnClick (fun _ -> dispatch (LogInClicked |> Intern))
                    Button.Disabled (buttonIsDisabled model) ]
                  [ str "Login" ] ] ]
  ]