module MainModel

open Shared
open Elmish
open Elmish.React

open Elmish
open Elmish.UrlParser
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

let initialCounter () = Fetch.fetchAs<int> "/api/init"

type Page =
  | Home
  | Login
  | Tournament

let toPage = function
  | Home -> "#/home"
  | Login -> "#/login"
  | Tournament-> "#/tournament"

let pageParser : Parser<Page->Page,Page> =
    oneOf [
        UrlParser.map Login (UrlParser.s "/")
        UrlParser.map Login (UrlParser.s "login")
        UrlParser.map Home (UrlParser.s "home")
        UrlParser.map Tournament (UrlParser.s "tournament")
    ]

type MainModel =
    {
        IsLoggedIn: bool
        ActivePage: Page
        LoginModel: LoginModel.Model
        GroupPhasePage: GroupPhaseModel.Model option
        TournamentCreationPage: TournamentCreationModel.Model
    }