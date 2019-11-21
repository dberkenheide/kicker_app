module Shared

type Credentials = {UserName: string; Password: string}
type LoginResult = {UserName: string; Token: string}

type Team = {TeamName: string}

let noneWhenEmpty str =
    if System.String.IsNullOrWhiteSpace str then None else Some str            