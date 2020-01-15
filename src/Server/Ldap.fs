module Ldap

open System
open System.IO
open System.Security.Claims
open System.IdentityModel.Tokens.Jwt
open Novell.Directory.Ldap
open Shared
open Shared.Dtos

let private createPassPhrase() =
    let crypto = System.Security.Cryptography.RandomNumberGenerator.Create()
    let randomNumber = Array.init 32 byte
    crypto.GetBytes(randomNumber)
    randomNumber

let secret =
    let fi = FileInfo("./temp/token.txt")
    if not fi.Exists then
        let passPhrase = createPassPhrase()
        if not fi.Directory.Exists then
            fi.Directory.Create()
        File.WriteAllBytes(fi.FullName,passPhrase)
    File.ReadAllBytes(fi.FullName)
    |> System.Text.Encoding.UTF8.GetString

let issuer = "lmis_kickerapp"

let private algorithm = Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256

let generateToken username =
    [ Claim(JwtRegisteredClaimNames.Sub, username); Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) ]
    |> Saturn.Auth.generateJWT (secret, algorithm) issuer (DateTime.UtcNow.AddHours(1.0))

let private memberOfAttribute = "memberOf"
let private displayNameAttribute = "dsiplayName"
let private samAccountNameAttribute = "sAMAccountName"

type LoginError =
  | LdapConnectionFailed
  | LdapConnectionLost
  | UserNameNotFound
  | PasswordIncorrect
  | AuthenticateFailed

let loginErrorText =
  function
  | LdapConnectionFailed -> "Verbindung zu Ldap konnte nicht hergestellt werden."
  | LdapConnectionLost -> "Die Verbindung zu Ldap ist abgebrochen."
  | UserNameNotFound -> "Das Kürzel ist nicht vergeben."
  | PasswordIncorrect -> "Das Passwort ist nicht korrekt."
  | AuthenticateFailed -> "Die Authentifizierung ist fehlgeschlagen."

let findMatchingUser (result : ILdapSearchResults) =
  if (result.HasMore()) then
    () |> result.Next |> Ok
  else
    UserNameNotFound |> Error

let login (connection :LdapConnection) (login: Login) =
  try
    connection.Bind(login.UserName, login.Password) |> ignore
    Ok connection    
  with
    | _ -> PasswordIncorrect |> Error

let getUserName (connection :LdapConnection) (user: LdapEntry) =
  if (connection.Bound) then
    Ok (user.GetAttribute(samAccountNameAttribute).StringValue)
  else
    AuthenticateFailed |> Error

let validateUserName (connection: LdapConnection) (searchResult: ILdapSearchResults) (password: string) =
  result {
    let! matchingUser = findMatchingUser searchResult
    let! loginResult = login connection { UserName = matchingUser.Dn; Password = password }
    let! userName = getUserName loginResult matchingUser
    return userName
  }
      
let findLdapUserWithGivenUserName (connection:LdapConnection) username =
  let searchFilter = sprintf "(&(objectClass=User)(sAMAccountName=%s))" username
  let searchBase = "ou=users,ou=company,dc=lmis,dc=de"
  let attributes = [|memberOfAttribute; displayNameAttribute; samAccountNameAttribute|]
  try
    connection.Search(searchBase, LdapConnection.ScopeSub, searchFilter, attributes, false) |> Ok
  with _ -> LdapConnectionLost |> Error

let establishConnection (connection: LdapConnection) (login: Login) =
  try
    connection.Connect("192.168.3.74", 389)
    connection.StartTls |> ignore
    connection.Bind(Constants.LdabUser, Constants.LdabPassword) |> Ok
  with _ -> LdapConnectionFailed |> Error

let authenticateWithLdap (login: Login) =
  use connection = new LdapConnection()
   
  let connectedUser = result {
    do! establishConnection connection login
    let! foundUsers = findLdapUserWithGivenUserName connection login.UserName
    return! validateUserName connection foundUsers login.Password
  }

  connection.StopTls |> ignore
  connection.Disconnect() |> ignore

  connectedUser