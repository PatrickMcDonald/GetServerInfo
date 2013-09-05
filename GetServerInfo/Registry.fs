module Registry

open Microsoft.Win32
open Types

let readValueInt32 name (key:RegistryKey) =
    match key.GetValue name with
    | null -> None
    | result -> Some(result :?> int)

let getLocalMachineKey machine =
    match machine with
    | Local -> Registry.LocalMachine
    | Remote(machineName) -> RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machineName)

let openSubKey name (key:RegistryKey) =
    key.OpenSubKey name

let rec openNested path key =
    match path with
    | head :: [] -> openSubKey head key
    | head :: tail ->
        use child = openSubKey head key 
        openNested tail child
    | [] -> failwith "Empty list"

