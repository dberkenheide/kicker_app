module TournamentCreation

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open System
open Shared.Dtos
open Shared.ResultBuilder

type ValidationError =
  | ForTitle of string
  | ForStartDate of string

type Model = {
    Title: string
    StartDate: DateTime option
    FailureMessage: ValidationError option
  }

type InternMsg =
  | SetTitle of string
  | SelectDate of DateTime option
  | CreateTournament
  | TournamentCreationFailed of ValidationError

type Msg =
  | Intern of InternMsg
  | NewTournamentCreated of NewTournament

let initModel () =
  { Title = ""
    StartDate = None
    FailureMessage = None
  }, Cmd.none

let validateInputFields (model: Model) =
  let validateTitle (t: string) =
    match t with
    | t when String.IsNullOrWhiteSpace t ->
       "Der Titel darf nicht leer sein!" |> ForTitle |> Error
    | t when t.Length > 50 ->
       "Der Titel darf maximal 50 Zeichen haben!" |> ForTitle |> Error
    | t -> Ok t

  let validateDate (date: DateTime option) =
    match date with
    | None ->
        "Es muss ein Datum ausgewählt sein!" |> ForStartDate |> Error
    | Some d when d < DateTime.Today ->
        "Das Datum darf nicht in der Vergangenheit liegen!" |> ForStartDate |> Error
    | Some d ->
        Ok d

  result {
    let! title = validateTitle model.Title
    let! startDate = validateDate model.StartDate

    let (newTournament: NewTournament) = {
      Title = title
      StartDate = startDate
    }
    return newTournament
  }

let update (msg: InternMsg) model : Model * Cmd<Msg> =
  match msg with
  | SetTitle newTitle ->
      { model with Title = newTitle }, Cmd.none
  | SelectDate date ->
      { model with StartDate = date }, Cmd.none
  | CreateTournament ->
      match validateInputFields model with
      | Ok newTournament ->
          model, Cmd.ofMsg (NewTournamentCreated newTournament)
      | Error validationError ->
          model, Cmd.ofMsg (validationError |> TournamentCreationFailed |> Intern)

  | TournamentCreationFailed message ->
      { model with FailureMessage = Some message }, Cmd.none

let parseDate stringDate =
  match DateTime.TryParse stringDate with
  | true, r -> Some(r)
  | _ -> None

let view (model: Model) (dispatch: Msg -> unit) =
  div [ ]
    [ Field.div [ ]
        [ Control.div [  ]
            [ Label.label [] [str "Titel"]
              Input.text
                [ Input.Size IsLarge
                  Input.Placeholder "Titel des Turniers"
                  Input.OnChange (fun ev -> ev.Value |> SetTitle |> Intern |> dispatch)
                  Input.Props [ AutoFocus true ] ] ] ]

      br [ ]
      Field.div [ ]
        [ Control.div [  ]
            [ Label.label [] [ str "Startdatum des Turniers"]
              Input.date
                [ Input.Size IsLarge
                  Input.OnChange (fun ev -> ev.Value |> parseDate |> SelectDate |> Intern |> dispatch) ] ] ]
      br [ ]
      Button.button
        [ Button.Color IsPrimary
          Button.IsFullWidth
          Button.OnClick (fun _ -> CreateTournament |> Intern |> dispatch) ]
        [ str "Erstellen" ]

      br [ ]
      match model.FailureMessage with
      | Some failure ->
          match failure with
          | ForTitle titleFailure ->
              Notification.notification [ Notification.Color IsDanger ]
                [ str titleFailure ]

          | ForStartDate dateFailure ->
              Notification.notification [ Notification.Color IsDanger ]
                [ str dateFailure ]
      | None ->
          div [] []
      ]



