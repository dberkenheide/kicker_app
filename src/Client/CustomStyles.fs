module CustomStyles

open Elmish
open Elmish.Navigation
open Fable.React
open Fable.React.Props
open Fulma

let centerStyle direction =
  Style [ Display DisplayOptions.Flex
          FlexDirection direction
          AlignItems AlignItemsOptions.Center
          JustifyContent "center"
          Padding "40px 0"
  ]
