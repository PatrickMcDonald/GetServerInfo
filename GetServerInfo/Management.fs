module Management

open System.Management
open Types

let lastBootUpTime machine =

    let machineName =
        match machine with
        | Local -> "localhost"
        | Remote(other) -> other

    let path = sprintf @"\\%s\ROOT\CIMV2" machineName

    let scope = ManagementScope(path)
    scope.Connect()
    let processClass = new ManagementClass(scope, ManagementPath("Win32_OperatingSystem"), ObjectGetOptions())
    let managementObjectSeq = processClass.GetInstances()

//    let searcher = new ManagementObjectSearcher(path, "SELECT * FROM Win32_OperatingSystem")
//    let managementObjectSeq = searcher.Get()

    managementObjectSeq
    |> Seq.cast<ManagementObject>
    |> Seq.map (fun m -> m.["LastBootUpTime"].ToString() )
    |> Seq.map (fun s -> ManagementDateTimeConverter.ToDateTime(s))
    |> Seq.exactlyOne



