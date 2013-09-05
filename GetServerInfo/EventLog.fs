module EventLog

open Types

let ReadEventLog machine =
    let eventLog1 = 
        match machine with 
        | Local -> new System.Diagnostics.EventLog("System")
        | Remote(machineName) -> new System.Diagnostics.EventLog("System", machineName)

    let restartEvent = 
        eventLog1.Entries
        |> Seq.cast<System.Diagnostics.EventLogEntry>
        |> Seq.filter (fun e -> e.EventID = 6005)
        |> Seq.sortBy (fun e -> e.TimeGenerated)
        |> Seq.last

    ignore()
