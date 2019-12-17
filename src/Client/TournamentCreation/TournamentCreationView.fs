module TournamentCreationView

open Fable.React
open Fable.React.Props
open Fulma

open TournamentCreationModel
open TournamentCreationMsg

let view (model : Model) (dispatch : Msg -> unit) =
  div
    []
    [
      Dropdown.dropdown [ Dropdown.IsHoverable ]
        [
          div [ ]
              [
                Button.button [ ]
                  [ span [ ]
                      [ str "Dropdown" ]
                    Icon.icon [ Icon.Size IsSmall ]
                      [ ] ]
                Dropdown.menu [ ]
                  [ Dropdown.content [ ]
                      [ Dropdown.Item.a [ ]
                          [ str "Item n°1" ]
                        Dropdown.Item.a [ ]
                          [ str "Item n°2" ]
                        Dropdown.Item.a [ Dropdown.Item.IsActive true ]
                          [ str "Item n°3" ]
                        Dropdown.Item.a [ ]
                          [ str "Item n°4" ]
                        Dropdown.divider [ ]
                        Dropdown.Item.a [ ]
                          [ str "Item n°5" ] ]
                  ]
              ]
        ]
      Button.button [] [ str "+" ]
    ]
