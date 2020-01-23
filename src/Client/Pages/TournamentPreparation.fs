module TournamentPreparation

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open System
open Shared
open Shared.ResultBuilder

open Elmish
open Elmish.Navigation
open Fable.React
open Fable.React.Props
open Fable.FontAwesome
open Fable.FontAwesome.Free
open Fulma

type Team = {
  Name: string
  FirstPlayer: Dtos.Player option
  SecondPlayer: Dtos.Player option
}

type Model = {
    Title: string
    StartDate: DateTime
    Players: Dtos.Player list
    Teams: Team list
  }

type InternMsg =
  | PlayersAndTeamsLoaded of (Dtos.Player list * Team list)
  | FirstPlayerSelected of (Dtos.Player * Team)
  | SecondPlayerSelected of (Dtos.Player * Team)
  | TeamNameChanged of (string * Team)
  | AddTeam
  | RemoveTeam of Team
  | SaveTournament

type Msg =
  | Intern of InternMsg

let initModel id =
  let mapTeam (allPlayers: Dtos.Player list) (teamDto: Dtos.Team) =
    let findPlayer (playerId: int option) =
      match playerId with
      | Some id -> (allPlayers |> List.tryFind (fun p -> p.PlayerId = id))
      | None -> None

    {
      Name = teamDto.Name
      FirstPlayer = findPlayer teamDto.PlayerOne
      SecondPlayer = findPlayer teamDto.PlayerTwo
    }

  let loadAllPlayers () = async {
    let! allPlayers = ServerApi.api.getAllPlayers ()
    let! tournament = ServerApi.api.getTournamentPreparationById id

    match tournament with
    | Ok t ->
        return (allPlayers, t.Teams |> List.map (mapTeam allPlayers))
    | Error er ->
        return ([], [])
  }

  let loadTournamentsCmd = Cmd.OfAsync.perform loadAllPlayers () (PlayersAndTeamsLoaded >> Intern)

  { Title = String.Empty
    StartDate = DateTime.MinValue
    Players = []
    Teams = []
  }, loadTournamentsCmd

let replaceItem list oldItem newItem =
  list
  |> List.map (fun o -> if o = oldItem then newItem else o)

let update (msg: InternMsg) model : Model * Cmd<Msg> =
  match msg with
  | AddTeam ->
      let newTeams = { Name = ""; FirstPlayer = None; SecondPlayer = None } :: model.Teams
      { model with Teams = newTeams }, Cmd.none

  | SaveTournament ->
      model, Cmd.none

  | PlayersAndTeamsLoaded (players, teams) ->
      { model with Players = players; Teams = teams }, Cmd.none

  | FirstPlayerSelected (player, team) ->
      let newTeam = {team with FirstPlayer = Some player}
      { model with Teams = (replaceItem model.Teams team newTeam ) }, Cmd.none

  | SecondPlayerSelected (player, team) ->
      let newTeam = {team with SecondPlayer = Some player}
      { model with Teams = (replaceItem model.Teams team newTeam ) }, Cmd.none

  | TeamNameChanged (newName, team) ->
      let newTeam = {team with Name = newName}
      { model with Teams = (replaceItem model.Teams team newTeam ) }, Cmd.none

  | RemoveTeam deletedTeam ->
      let teamsWithoutDeletedTeam = model.Teams |> List.filter (fun t -> t <> deletedTeam)
      { model with Teams = teamsWithoutDeletedTeam }, Cmd.none

let view (model: Model) (dispatch: Msg -> unit) =
  let dropDownForPlayers (players: Dtos.Player list) (team: Team) (msg: (Dtos.Player * Team) -> Msg) (selectedPlayer: Dtos.Player option) =
      Column.column [] [
        Dropdown.dropdown [ Dropdown.IsHoverable; ]  [
          div [ ] [
            Button.button [ ] [
              span [ ] [ str (match selectedPlayer with | Some s -> s.Abbreviation | None -> "") ]
              Icon.icon [ Icon.Size IsSmall ] [
                Fa.i [ Fa.Solid.AngleDown ] [ ]
              ]
            ]
          ]
          Dropdown.menu [ ] [
            Dropdown.content [ ]
              (players |> List.map (fun p -> Dropdown.Item.a [ Dropdown.Item.Props [ OnClick (fun _ -> (p, team) |> msg |> dispatch) ] ] [ str p.Abbreviation ] ))
          ]
        ]
      ]

  let createTeamRow (team: Team) =
    Columns.columns [ ] [
      yield Column.column [ Column.Width (Screen.All, Column.IsNarrow) ] [
        Button.button [
          Button.Color IsDanger
          Button.IsInverted
          Button.OnClick (fun _ -> team |> RemoveTeam |> Intern |> dispatch)
        ] [
          Fa.span [ Fa.Solid.Times ] [ ]
        ]
      ]
      yield Column.column [ ] [
        Input.text [
          Input.Placeholder "Teamname"
          Input.Value team.Name
          Input.OnChange (fun ev -> dispatch ((ev.Value, team) |> TeamNameChanged |> Intern))
        ]
      ]
      yield (dropDownForPlayers model.Players team (FirstPlayerSelected >> Intern) team.FirstPlayer )
      yield (dropDownForPlayers model.Players team (SecondPlayerSelected >> Intern) team.SecondPlayer)
    ]

  div [ Style [ PaddingLeft "3%"; PaddingRight "3%" ] ] [
    Box.box' [ ] [
      Columns.columns [ ] [
        Column.column [ Column.Width (Screen.All, Column.IsNarrow) ] [ strong [ ] [ str "Titel:"] ]
        Column.column [ Column.Width (Screen.All, Column.IsNarrow) ] [ str model.Title ]
        Column.column [ Column.Width (Screen.All, Column.IsNarrow) ] [ strong [] [ str "Datum:" ] ]
        Column.column [ Column.Width (Screen.All, Column.IsNarrow) ] [ str (model.StartDate.ToShortDateString()) ]
      ]
    ]
    Box.box' [ ] [
      yield! (model.Teams |> List.map createTeamRow)

      yield Button.button [ Button.OnClick (fun _ -> AddTeam |> Intern |> dispatch)] [ Fa.span [ Fa.Solid.Plus ] [ ] ]
    ]
  ]