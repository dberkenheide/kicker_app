module Server

open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn
open Microsoft.Data.Sqlite

open Fable.Remoting.Server
open Fable.Remoting.Giraffe

let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x

let publicPath = Path.GetFullPath "../Client/public"

let dbPath = Path.GetFullPath "../Database"

let connectionString =  @"Filename=C:\repos\BrainTeaser\kicker_app\src\Database\kickerApp.db"//" + Path.Combine [| dbPath; "kickerApp.db" |]

let connection = new SqliteConnection(connectionString)

let port =
  "SERVER_PORT"
  |> tryGetEnv |> Option.map uint16 |> Option.defaultValue 8085us

let authApp =
  Remoting.createApi()
  |> Remoting.withRouteBuilder Shared.Apis.Route.builder
  |> Remoting.fromValue AuthApi.authApi
  |> Remoting.buildHttpHandler

let tournamentApp =
  Remoting.createApi()
  |> Remoting.withRouteBuilder Shared.Apis.Route.builder
  |> Remoting.fromValue (TournamentApi.createTournamentApi connection)
  |> Remoting.buildHttpHandler

let app = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    use_router authApp
    use_router tournamentApp
    memory_cache
    use_static publicPath
    use_gzip
}

run app
