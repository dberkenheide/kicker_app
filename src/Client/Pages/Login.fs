module Login

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Shared
open System
open Thoth.Json
open Fable.Core.JsInterop

type Model =
  {
    Login : Login
    Running : bool
    ErrorMsg : string option
  }

type Msg =
  | LoginSuccess of UserData
  | SetUserName of string
  | SetPassword of string
  | AuthError of exn
  | LogInClicked

let initModel () =
  {
    Login= {UserName= ""; Password= ""; PasswordId = Guid.Empty }
    Running=false
    ErrorMsg= None
  }

let authUser (login: Login) =
  ServerApi.auth.login login


let update (msg:Msg) model : Model*Cmd<Msg> =
  match msg with
  | LoginSuccess _ ->
      model, Cmd.none

  | SetUserName name ->
      { model with Login = { model.Login with UserName = name } }, Cmd.none

  | SetPassword pw ->
      { model with Login = { model.Login with Password = pw } }, Cmd.none

  | LogInClicked ->
      { model with Running = true },
          Cmd.OfAsync.either authUser model.Login LoginSuccess AuthError

  | AuthError exn ->
      { model with Running = false; ErrorMsg = Some exn.Message }, Cmd.none

let view (model : Model) (dispatch : Msg -> unit) =
  let buttonIsDisabled () =
      String.IsNullOrEmpty model.Login.UserName ||
      String.IsNullOrEmpty model.Login.Password ||
      model.Running

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
                            Input.OnChange (fun ev -> dispatch (SetUserName ev.Value))
                            Input.Props [ AutoFocus true ] ] ] ]
                Field.div [ ]
                  [ Control.div [ ]
                      [ Input.password
                          [ Input.Size IsLarge
                            Input.OnChange (fun ev -> dispatch (SetPassword ev.Value))
                            Input.Placeholder "Dein Passwort" ] ] ]
                br [ ]
                Button.button
                  [ Button.Color IsPrimary
                    Button.IsFullWidth
                    Button.OnClick (fun _ -> dispatch LogInClicked)
                    Button.Disabled (buttonIsDisabled ()) ]
                  [ str "Login" ] ] ]
  ]

  // Column.column
  //     [ Column.Width (Screen.All, Column.IsOneThird) ]
  //     [ Box.box' [ Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
  //         [ figure [ Class "avatar" ]
  //             [ img [ Src "/lmis-ag-logo.svg" ] ]
  //           form [ ]
  //             [ Field.div [ ]
  //                 [ Control.div [ ]
  //                     [ Input.email
  //                         [ Input.Size IsLarge
  //                           Input.Placeholder "Dein Kürzel"
  //                           Input.Props [ AutoFocus true ] ] ] ]
  //               Field.div [ ]
  //                 [ Control.div [ ]
  //                     [ Input.password
  //                         [ Input.Size IsLarge
  //                           Input.Placeholder "Dein Passwort" ] ] ]
  //               br [ ]
  //               Button.button
  //                 [ Button.Color IsPrimary
  //                   Button.IsFullWidth
  //                   Button.Disabled (not buttonActive) ]
  //                 [ str "Login" ] ] ]
  //       ]