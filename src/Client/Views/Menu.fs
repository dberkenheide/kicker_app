module Menu

open Fulma
open Fable.React
open Fable.React.Props
open Fable.FontAwesome
open Fable.FontAwesome.Free

open Shared

let view (user: UserData option) =
  let choices = [ "Item 1"; "Item 2"; "Item 3" ]

  Navbar.navbar [ Navbar.Color IsPrimary; Navbar.CustomClass "mainNavbar" ] [
    Navbar.Brand.div [] [
      Navbar.Item.div [] [
        Fa.span [ Fa.Solid.Futbol ; Fa.Size Fa.Fa3x ] []
      ]
    ]

    Navbar.Start.div [] [
      Navbar.Item.div [ Navbar.Item.HasDropdown; Navbar.Item.IsHoverable ] [
        Navbar.Link.a [ ] [ strong [] [ str "Tunier 1" ] ]
        Navbar.Dropdown.div [ ] (choices |> List.map (fun c -> Navbar.Item.a [] [str c]))
      ]

      Navbar.Item.div [] [
        Button.button [ Button.Color IsPrimary ] [ Fa.span [ Fa.Solid.Plus ] [ ] ]
      ]

      Navbar.Item.a [ Navbar.Item.IsTab; Navbar.Item.IsActive true ] [
        strong [] [ str "Stand" ]
      ]

      Navbar.Item.a [ Navbar.Item.IsTab; Navbar.Item.IsActive false] [
        strong [] [ str "Regeln" ]
      ]
    ]

    Navbar.End.div [] [
      Navbar.Item.div [] [
        match user with
        | Some u ->
            yield u.UserName
                  |> sprintf "Angemeldet als %s  "
                  |> str
            yield Button.a [ Button.IsOutlined; Button.Color IsWhite; Button.Props [ Href "#logout" ] ] [
                    str "Logout"
                  ]
        | None ->
            yield Button.a [ Button.IsOutlined; Button.Color IsWhite; Button.Props [ Href "#login" ] ] [
                    str "Login"
                  ]
      ]
    ]
  ]