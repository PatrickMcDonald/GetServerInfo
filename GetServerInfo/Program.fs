// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

module Program

open Types
open Registry
open Management

type HttpParameters = {
    MaxFieldLength : int option;
    MaxRequestBytes: int option;
    //MaxRequestedBytes: int option;
    MaxTokenSize: int option;
    LastBootUpTime: System.DateTime }

let readRegistry machine =

    use httpParameters = getLocalMachineKey machine |> openNested ["System"; "CurrentControlSet"; "Services"; "HTTP"; "Parameters"] 
    use kerberosParameters = getLocalMachineKey machine |> openNested ["System"; "CurrentControlSet"; "Control"; "Lsa"; "Kerberos"; "Parameters"]

    { MaxFieldLength = readValueInt32 "MaxFieldLength" httpParameters;
      MaxRequestBytes = readValueInt32 "MaxRequestBytes" httpParameters;
      //MaxRequestedBytes = readValueInt32 "MaxRequestedBytes" httpParameters;
      MaxTokenSize = readValueInt32 "MaxTokenSize" kerberosParameters;
      LastBootUpTime = lastBootUpTime machine }

let description machine =
    match machine with
    | Local -> "Local"
    | Remote(machineName) -> machineName

let doReadRegistry machine =
    try
        description machine |> printfn "%s"
        readRegistry machine |> printfn "%A"
    with
        | :? System.Security.SecurityException as ex -> printfn "%s" ex.Message
        | :? System.IO.IOException as ex -> printfn "%s" ex.Message
    printfn ""

[<EntryPoint>]
let main argv = 

    let machines = [
        Local;
        Remote "DHX34244";
        Remote "SERV8460";
        Remote "SERV8279";
        Remote "SERV8303";
    ]

    machines
    |> Seq.iter doReadRegistry

    Console.pause()

    0 // return an integer exit code
