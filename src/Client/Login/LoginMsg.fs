module LoginMsg

type Msg =
    | LoginClick
    | UserChanged of string option
    | PasswordChanged of string option
    | LoginSuccess of Shared.LoginResult
    | LoginFailed of string