#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("Zafir.TinyMce")
        .VersionFrom("Zafir")
        .WithFSharpVersion(FSharpVersion.FSharp30)
        .WithFramework(fun fw -> fw.Net40)

let main =
    bt.Zafir.Extension("WebSharper.TinyMce")
        .SourcesFromProject()

let formlet =
    bt.Zafir.Library("WebSharper.Formlets.TinyMce")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.NuGet("Zafir.Reactive").Latest(true).ForceFoundVersion().Reference()
                r.NuGet("Zafir.Formlets").Latest(true).ForceFoundVersion().Reference()
                r.NuGet("Zafir.Html").Latest(true).ForceFoundVersion().Reference()
                r.Project main
            ])

let test =
    bt.Zafir.HtmlWebsite("WebSharper.TinyMce.Tests")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.NuGet("Zafir.Reactive").Latest(true).Reference()
                r.NuGet("Zafir.Formlets").Latest(true).Reference()
                r.NuGet("Zafir.Html").Latest(true).Reference()
                r.Project main
                r.Project formlet
            ])

bt.Solution [
    main
    formlet
    test

    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "WebSharper.TinyMce"
                LicenseUrl = Some "http://websharper.com/licensing"
                Description = "WebSharper Extensions and Formlets for TinyMce"
                RequiresLicenseAcceptance = true })
        .Add(main)
        .Add(formlet)

]
|> bt.Dispatch
