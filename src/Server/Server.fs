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
open MySql.Data.MySqlClient

let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x

let publicPath = Path.GetFullPath "../Client/public"

let dbPath = Path.GetFullPath "../Database"

//let connectionString =  @"Filename=" + Path.Combine [| dbPath; "kickerApp.db" |]
let connectionString = "server=localhost;userid=kickerapp_db;database=kickerapp_db;Pwd=kickerapp_db;Port=3306" //  @"Filename=" + Path.Combine [| dbPath; "kickerApp.db" |]

let port =
  "SERVER_PORT"
  |> tryGetEnv |> Option.map uint16 |> Option.defaultValue 8085us

let authApp connection =
  Remoting.createApi()
  |> Remoting.withRouteBuilder Shared.Apis.Route.builder
  |> Remoting.fromValue AuthApi.authApi
  |> Remoting.buildHttpHandler

let tournamentApp connection =
  Remoting.createApi()
  |> Remoting.withRouteBuilder Shared.Apis.Route.builder
  |> Remoting.fromValue (TournamentApi.createTournamentApi connection)
  |> Remoting.buildHttpHandler

let app connection = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    use_router (authApp connection)
    use_router (tournamentApp connection)
    memory_cache
    use_static publicPath
    use_gzip
}

do
  use connection = new MySqlConnection(connectionString)
  run (app connection)
