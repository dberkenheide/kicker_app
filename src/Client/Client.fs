module Client

open Elmish
open Elmish.React
open Elmish.UrlParser
open Fable.FontAwesome
open Fable.FontAwesome.Free
open Fable.React
open Fable.React.Props
open Fulma
open Thoth.Json

open Shared.Dtos

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram MainView.init MainView.update MainView.view
|> Program.toNavigable Routing.urlParser MainView.urlUpdate
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
