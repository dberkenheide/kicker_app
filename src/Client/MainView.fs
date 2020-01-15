module MainView

open Elmish
open Elmish.Navigation
open Fable.React
open Fable.React.Props
open Fable.FontAwesome
open Fable.FontAwesome.Free
open Fulma

open Shared.Dtos
open Routing

type PageModel =
  | NotFoundModel
  | HomeModel of string
  | LoginModel of Login.Model
  | StandingModel of Standing.Model
  | CreationModel of TournamentCreation.Model
  | PreparationModel of TournamentPreparation.Model
  | RulesModel

type Model = {
  SelectedTournament: TournamentForDropDown option
  AllTournaments: TournamentForDropDown list
  User: UserData option
  PageModel: PageModel
}

// Alle Messages die auftreten können
type Msg =
  // Messages der eingehängten Models
  | LoginMsg of Login.Msg
  | StandingMsg of Standing.Msg
  | CreationMsg of TournamentCreation.Msg
  | PreaparationMsg of TournamentPreparation.Msg
  // Eigene Msgs
  | TournamentsLoaded of TournamentForDropDown list
  | TournamentSelected of TournamentForDropDown
  | NewOpenTournament of OpenTournament
  | Logout

let urlUpdate (result: Page option) (model: Model) =
  match result with
  | None ->
      model, Cmd.none

  | Some Page.NotFound ->
      { model with PageModel = NotFoundModel }, Cmd.none

  | Some Page.Login ->
      let m, c = Login.initModel ()
      { model with PageModel = LoginModel m }, c |> Cmd.map LoginMsg

  | Some Page.Home ->
      { model with PageModel = HomeModel "Can GET/Home" }, Cmd.none //Cmd.map HomePageMsg cmd

  | Some (Page.Standing s) ->
      let m, c = Standing.initModel s
      { model with PageModel = StandingModel m }, c |> Cmd.map LoginMsg

  | Some (Page.CreateNew) ->
      let m, c = TournamentCreation.initModel ()
      { model with PageModel = CreationModel m }, c |> Cmd.map CreationMsg

  | Some (Page.Preparation s) ->
      let m, c = TournamentPreparation.initModel s
      { model with PageModel = PreparationModel m }, c |> Cmd.map PreaparationMsg

let init (page: Page option) : Model * Cmd<Msg> =
  let initialModel = {
    SelectedTournament = None
    AllTournaments = [ ]
    User = None
    PageModel = NotFoundModel
  }

  let loadAllTournaments () = async {
    let! tournaments = ServerApi.api.getAllTournaments ()
    return tournaments
  }

  let loadTournamentsCmd =
    Cmd.OfAsync.perform loadAllTournaments () TournamentsLoaded

  let model, cmd = urlUpdate page initialModel

  model, Cmd.batch [ cmd; loadTournamentsCmd ]

let update (msg: Msg) (model: Model) =
  let navigateTo p m =
    m, (p |> toPath |> Navigation.newUrl)

  match msg, model.PageModel with
  | LoginMsg msg, LoginModel loginModel ->
      match msg with
      | Login.Msg.LoginSuccess newUser ->
          navigateTo Page.Home { model with User = Some newUser }

      | Login.Msg.Intern internMsg ->
          let newLoginModel, loginCmd = Login.update internMsg loginModel
          { model with PageModel = LoginModel newLoginModel }, Cmd.map LoginMsg loginCmd

  | LoginMsg _, _ -> model, Cmd.none

  | StandingMsg msg, StandingModel standingModel ->
      match msg with
      | Standing.Msg.Intern internMsg ->
          let newStandingModel, standingCmd = Standing.update internMsg standingModel
          { model with PageModel = StandingModel newStandingModel }, Cmd.map StandingMsg standingCmd

  | StandingMsg _, _ -> model, Cmd.none

  | PreaparationMsg msg, PreparationModel preparationModel ->
      match msg with
      | TournamentPreparation.Msg.Intern internMsg ->
          let preparationModel, preparationCmd = TournamentPreparation.update internMsg preparationModel
          { model with PageModel = PreparationModel preparationModel }, Cmd.map PreaparationMsg preparationCmd

  | PreaparationMsg _, _ -> model, Cmd.none

  | CreationMsg msg, CreationModel creationModel ->
      match msg with
      | TournamentCreation.Intern internMsg ->
          let newCreationModel, creationCmd = TournamentCreation.update internMsg creationModel
          { model with PageModel = CreationModel newCreationModel }, Cmd.map CreationMsg creationCmd

      | TournamentCreation.NewTournamentCreated newTournament ->
          model, Cmd.OfAsync.perform ServerApi.api.createNewTournament newTournament NewOpenTournament

  | CreationMsg _, _ -> model, Cmd.none

  | TournamentSelected newId, _ ->
      let m, c =
        match model.PageModel with
        | StandingModel _ ->
            navigateTo (Page.Standing newId.Id) model
        | PreparationModel _ ->
            navigateTo (Page.Preparation newId.Id) model
        | _ -> model, Cmd.none

      { m with SelectedTournament = Some newId }, c

  | NewOpenTournament openTournament, _ ->
      let newIds = [ { Title = openTournament.Title; Id = openTournament.Id } ]

      { model with AllTournaments = List.concat [ model.AllTournaments; newIds ]; SelectedTournament = Some newIds.[0] }, Cmd.none

  | TournamentsLoaded newTournamtes, _ ->
      { model with AllTournaments = newTournamtes }, Cmd.none

  | Logout, _ ->
      { model with User = None }, Cmd.none

type TabInfo = {
  Page: Page option
  Label: string
  IsActive: bool
}

let menuView (model: Model) (dispatch: Msg -> unit) =
  let toTab tabInfo =
    Navbar.Item.a [
        yield Navbar.Item.IsTab
        yield Navbar.Item.IsActive tabInfo.IsActive
        match tabInfo.Page with
        | Some p ->
            yield Navbar.Item.Props [ Href (p |> toPath) ]
        | None -> ()
      ] [
        strong [] [ str tabInfo.Label ]
      ]

  let mapPage pageConstructor = model.SelectedTournament |> Option.map (fun s -> pageConstructor s.Id)

  let tabInfos = [
    { Page = mapPage Page.Preparation
      Label = "Vorbereitung"
      IsActive = (match model.PageModel with | PreparationModel _ -> true | _ -> false) }

    { Page = mapPage Page.Standing
      Label = "Stand"
      IsActive = (match model.PageModel with | StandingModel _ -> true | _ -> false) }
  ]

  div [] [
    Navbar.navbar [ Navbar.Color IsPrimary; Navbar.CustomClass "mainNavbar" ] [
      Navbar.Brand.div [] [
        Navbar.Item.div [] [
          Fa.span [ Fa.Solid.Futbol ; Fa.Size Fa.Fa3x ] []
        ]
      ]

      Navbar.Start.div [] [
        yield Navbar.Item.div [ Navbar.Item.HasDropdown; Navbar.Item.IsHoverable ] [
          Navbar.Link.a [ ] [ strong [] [ str (match model.SelectedTournament with | Some s -> s.Title | None -> "kein Turnier ausgewählt") ] ]
          Navbar.Dropdown.div [ ]
            (model.AllTournaments
              |>  List.map (fun c ->
                    Navbar.Item.a
                      [ Navbar.Item.Props [ OnClick (fun _ -> dispatch (TournamentSelected c)) ] ]
                      [ str c.Title ]
                  )
            )
        ]

        yield Navbar.Item.div [] [
          Button.a [ Button.Color IsPrimary; Button.Props [ Href (Page.CreateNew |> toPath) ] ] [ Fa.span [ Fa.Solid.Plus ] [ ] ]
        ]

        yield! (tabInfos |> List.map toTab)

        //    yield Navbar.Item.a [
        //      Navbar.Item.IsTab
        //      Navbar.Item.IsActive (match model.PageModel with | RulesModel _ -> true | _ -> false)
        //      Navbar.Item.Props [  Href ("#rules/" + s.Id.ToString()) ]
        //    ] [
        //      strong [] [ str "Regeln" ]
        //    ]
      ]

      Navbar.End.div [] [
        Navbar.Item.div [] [
          match model.User with
          | Some u ->
              yield u.UserName
                    |> sprintf "Angemeldet als %s"
                    |> str
              yield div [ Style [ MarginLeft 20. ] ] [ ]
              yield Button.a [
                      Button.IsOutlined
                      Button.Color IsWhite
                      Button.Props [ Href "#logout" ]
                      Button.OnClick (fun _ -> dispatch Logout)
                    ] [
                      str "Logout"
                    ]
          | None ->
              yield Button.a [ Button.IsOutlined; Button.Color IsWhite; Button.Props [ Href (Page.Login |> toPath) ] ] [
                      str "Login"
                    ]
        ]
      ]
    ]
  ]

let view (model: Model) (dispatch : Msg -> unit) =
  div [] [
    (menuView model dispatch)
    div [ CustomStyles.centerStyle "column" ] [
      match model.PageModel with
      | LoginModel loginModel ->
          yield Login.view loginModel (LoginMsg >> dispatch)

      | HomeModel name ->
          yield div [] [ str name ]

      | StandingModel standingModel ->
          yield Standing.view standingModel (StandingMsg >> dispatch)

      | PreparationModel preparationModel ->
          yield TournamentPreparation.view preparationModel (PreaparationMsg >> dispatch)

      | CreationModel creationModel ->
          yield TournamentCreation.view creationModel (CreationMsg >> dispatch)

      | RulesModel ->
          yield div [] [ str "Regeln!" ]

      | NotFoundModel ->
          yield div [] [ str "The page is not available." ]
    ]
  ]