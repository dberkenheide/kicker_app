module MainMsg
open Shared

type Msg =
    | Increment
    | Decrement
    | InitialCountLoaded of Counter