module Console

let pause() =
    printf "Press any key to continue . . . "
    System.Console.ReadKey true |> ignore
