module Ldap

open System
open System.IO
open System.Security.Claims
open System.IdentityModel.Tokens.Jwt
open Novell.Directory.Ldap
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

let connectToLdapWithUserCredentials password (connection :LdapConnection) (result : ILdapSearchResults) =
    let user = result.Next()

    if (user |> isNull) then
        Error "Authenticate failed."
    else   
        connection.Bind(user.Dn, password)

        if (connection.Bound) then
            Ok (user.GetAttribute(samAccountNameAttribute).StringValue)
        else
            Error "Authenticate failed."

let findLdapUserWithGivenUserName username (connection:LdapConnection) =
    let searchFilter = sprintf "(&(objectClass=User)(sAMAccountName=%s))" username
    let searchBase = "ou=users,ou=company,dc=lmis,dc=de"
    let attributes = [|memberOfAttribute; displayNameAttribute; samAccountNameAttribute|]
    connection.Search(searchBase, LdapConnection.ScopeSub, searchFilter, attributes, false)

let authenticateWithLdap (login: Login) =
    use connection = new LdapConnection()

    connection.Connect("192.168.3.74", 389)
    connection.StartTls |> ignore

    connection.Bind(Constants.LdabUser, Constants.LdabPassword) |> ignore

    let connectedUser = findLdapUserWithGivenUserName login.UserName connection
                       |> connectToLdapWithUserCredentials login.Password connection

    connection.StopTls |> ignore
    connection.Disconnect() |> ignore

    connectedUser

