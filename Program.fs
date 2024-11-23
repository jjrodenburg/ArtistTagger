open System
open System.IO
open TagLib

module AudioMetadata =

    /// Sets the Contributing Artist metadata for a given audio file.
    let setContributingArtist (filePath: string) (artist: string) =
        try
            let file = File.Create(filePath)
            file.Tag.Performers <- [| artist |]
            file.Save()
            printfn "Updated: %s" filePath
        with
        | ex -> printfn "Failed to update: %s - %s" filePath ex.Message

    /// Recursively updates the Contributing Artist metadata for all audio files in the given directory.
    let updateArtistInDirectory (rootDir: string) (artist: string) =
        Directory.EnumerateFiles(rootDir, "*.*", SearchOption.AllDirectories)
        |> Seq.filter (fun file -> 
            file.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
            file.EndsWith(".flac", StringComparison.OrdinalIgnoreCase) ||
            file.EndsWith(".wav", StringComparison.OrdinalIgnoreCase) ||
            file.EndsWith(".wma", StringComparison.OrdinalIgnoreCase))
        |> Seq.iter (fun file -> setContributingArtist file artist)

[<EntryPoint>]
let main argv =
    printf "Artist Tagger"
    printf "\nEnter the path to the root directory: "
    let rootDirectory = Console.ReadLine()
    printf "Enter the name of the contributing artist: "
    let artistName = Console.ReadLine()
    
    AudioMetadata.updateArtistInDirectory rootDirectory artistName
    printfn "Contributing Artist set to %s for all audio files in %s" artistName rootDirectory

    0
