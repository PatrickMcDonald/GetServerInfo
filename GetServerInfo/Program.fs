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
    LastBootUpTime: System.DateTime }

let ReadRegistry machine =

    use parameters = getLocalMachineKey machine |> openNested ["System"; "CurrentControlSet"; "Services"; "HTTP"; "Parameters"] 

    { MaxFieldLength = readValueInt32 "MaxFieldLength" parameters;
      MaxRequestBytes = readValueInt32 "MaxRequestBytes" parameters;
      //MaxRequestedBytes = readValueInt32 "MaxRequestedBytes" parameters;
      LastBootUpTime = Management.LastBootUpTime machine }

let description machine =
    match machine with
    | Local -> "Local"
    | Remote(machineName) -> machineName

let DoReadRegistry machine =
    try
        description machine |> printfn "%s"
        ReadRegistry machine |> printfn "%A"
    with
        | :? System.Security.SecurityException as ex -> printfn "%s" ex.Message
    printfn ""

[<EntryPoint>]
let main argv = 

    let machines = [
        Local;
        Remote "SERV8460";
        Remote "SERV8279";
        Remote "SERV8303";
        Remote "SERV8624";
        Remote "SERV8709";
        Remote "SERV8303A";
        Remote "SERV8814";
    ]

    machines
    |> Seq.iter DoReadRegistry

    Console.pause()

    0 // return an integer exit code
