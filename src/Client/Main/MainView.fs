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
      div [ ClassName "navbar-brand"]
        [
          a [ ClassName "navbar-item"; Href "https://bulma.io/documentation/components/navbar/" ]
            [
              i [ ClassName "far fa-futbol" ] []
            ]
        ]

      Navbar.Start.div []
        [ getNavbarTab "#/tournament" "Tuniere" ]
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

            yield match model.ActivePage with
                  | Login -> div [] []
                  | Tournament -> (TournamentCreationView.view model.TournamentCreationPage (TournamentsMsg >> dispatch))
                  | Home -> div [] []

            yield getFooter ()
        else
            yield getLogin model.LoginModel dispatch
      }

    if model.IsLoggedIn then
        div [] (getElements model dispatch)
    else
        div [] [ (getLogin model.LoginModel dispatch) ]

