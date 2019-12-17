module MainView

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open MainModel
open MainMsg

let private getNavbarTab link name =
  Navbar.Item.a
    [ Navbar.Item.IsTab; Navbar.Item.Props [Href link] ]
    [ str name ]

let getNavbar title (dispatch : MainMsg -> unit) =
  Navbar.navbar
    [ Navbar.Color IsPrimary ]
    [
      Navbar.Item.div
        [ ]
        [ Heading.h2 [ ] [ str title ] ]

      Navbar.Start.div []
        [ getNavbarTab "Tunier erstellen" "Tunier erstellen" ]
      Navbar.End.div []
        [ getNavbarTab "Logout" "Logout" ]
    ]

let getLogin (model : LoginModel.Model) (dispatch : MainMsg -> unit) =
  Container.container
    []
    [
      Content.content
        [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
        [ div [ ] [ LoginView.view model (LoginMsg >> dispatch)] ]
    ]

let getFooter () =
  Footer.footer
    [ ]
    [
      Content.content
        [ Content.CustomClass "main-container"; Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
        [ str "Nix!" ]
    ]

let view (model : MainModel) (dispatch : MainMsg -> unit) =
    let getElements model dispatch =
      seq {
        if (model.IsLoggedIn) then
            yield getNavbar "LMIS Kicker-App" dispatch

            //match model.ActivePage with
            //| GroupPhase gp -> yield (GroupPhaseView.view gp (GroupPhaseMsg >> dispatch))
            //| _ -> ()

            yield getFooter ()
        else
            yield getLogin model.LoginModel dispatch
      }

    if model.IsLoggedIn then
        div [] (getElements model dispatch)
    else
        div [] [ (getLogin model.LoginModel dispatch) ]

