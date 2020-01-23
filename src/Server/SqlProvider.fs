module SqlProvider

open System
open FSharp.Data.Sql
open Shared.Dtos

[<Literal>]
let connStr = @"Data Source=localhost\SQLExpress,1433; Initial Catalog=kickerapp_db; User Id=SA;Password=12!kickerapp_db"

type Db = SqlDataProvider<Common.DatabaseProviderTypes.MSSQLSERVER, connStr, UseOptionTypes = true>

type Dbo = Db.dataContext

let ctx = Db.GetDataContext()
