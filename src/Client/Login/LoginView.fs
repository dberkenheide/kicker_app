module LoginView

open Elmish
open Elmish.React
open Fable.FontAwesome
open Fable.FontAwesome.Free
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open Shared
open LoginModel
open LoginMsg
open LoginUpdate

open Custom.Heading

let column (model : Model) (dispatch : Msg -> unit) =
  Column.column
    [ Column.Width (Screen.All, Column.Is4)
      Column.Offset (Screen.All, Column.Is4) ]
    [
      customH3 "Login"
      customP "Bitte melde dich an."
      Box.box' [ ]
        [ figure [ Class "avatar" ]
            [ img [ Src "https://placehold.it/128x128" ] ]
          div [ ]
            [ Field.div [ ]
                [ Control.div [ ]
                    [ Input.text
                        [ Input.Size IsLarge
                          Input.Placeholder "Dein Kürzel"
                          Input.Props [ AutoFocus true ]
                          Input.Value (model.UserName |> Option.defaultWith (fun _ -> ""))
                          Input.OnChange (fun e -> dispatch (e.Value |> noneWhenEmpty |> UserChanged))] ] ]
              Field.div [ ]
                [ Control.div [ ]
                    [ Input.password
                        [ Input.Size IsLarge
                          Input.Placeholder "Dein Passwort"
                          Input.OnChange (fun e -> dispatch (e.Value |> noneWhenEmpty |> PasswordChanged))] ] ]
              Button.button
                [ Button.Color IsInfo
                  Button.IsFullWidth
                  Button.CustomClass "is-large is-block"
                  Button.OnClick (fun _ -> dispatch LoginClick)
                  Button.Disabled (hasEmtptyLoginFields model) ]
                [ str "Login" ] ]
        ]
      br [ ]
    ]

let view (model : Model) (dispatch : Msg -> unit) =
    Hero.hero
        [ Hero.IsFullHeight ]
        [ Hero.body [ ]
            [ Container.container
                [ Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                [ column model dispatch ] ] ]