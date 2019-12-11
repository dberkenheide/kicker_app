module GroupPhaseView

open Fable.React
open Fable.React.Props

open Fulma
open GroupPhaseModel
open GroupPhaseMsg
open GroupPhaseUpdate

let tableForGroup group =
    let rowForTeam team =
        tr  [ ]
            [
                td [ ] [ str team.Name ]
                td [ ] [ str (team.Point.ToString()) ]
            ]

    div []
        [
            Message.message [ ]
                              [ Message.body [ Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered)
                                                           Modifier.BackgroundColor IsGreyLighter
                                                           Modifier.TextColor IsLink
                                                           Modifier.TextWeight TextWeight.Bold ] ]
                                    [ str group.GroupName] ]
            Table.table
                [ Table.IsBordered; Table.IsNarrow; Table.IsStriped; Table.IsHoverable ]
                [
                    thead
                        [ ]
                        [ tr [ ]
                            [
                                th [ ] [ str "Teamname" ]
                                th [ ] [ str "Punkte" ]
                                ]
                        ]
                    tbody [ ] (group.Teams |> List.map rowForTeam ) ]
        ]

let tableToColumn table =
    Column.column [ ] [ table]

let groups (model : Model) =
    [
        Columns.columns
            [ ]
            (model.Groups |> List.map (tableForGroup >> tableToColumn))
    ]

let view (model : Model) (dispatch : Msg -> unit) =
    Hero.hero
        [ Hero.IsFullHeight ]
        [ Hero.body [ ]
            [ Container.container
                [ Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                [ div [] (groups model) ] ] ]

                //tableForGroup model.Groups.Head