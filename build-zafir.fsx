#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.TinyMce")
        .VersionFrom("WebSharper")
        .WithFSharpVersion(FSharpVersion.FSharp30)
        .WithFramework(fun fw -> fw.Net40)

let main =
    bt.WebSharper4.Extension("WebSharper.TinyMce")
        .SourcesFromProject()

let formlet =
    bt.WebSharper4.Library("WebSharper.Formlets.TinyMce")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.NuGet("WebSharper.Reactive").Latest(true).ForceFoundVersion().Reference()
                r.NuGet("WebSharper.Formlets").Latest(true).ForceFoundVersion().Reference()
                r.NuGet("WebSharper.Html").Latest(true).ForceFoundVersion().Reference()
                r.Project main
            ])

let test =
    bt.WebSharper4.HtmlWebsite("WebSharper.TinyMce.Tests")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.NuGet("WebSharper.Reactive").Latest(true).Reference()
                r.NuGet("WebSharper.Formlets").Latest(true).Reference()
                r.NuGet("WebSharper.Html").Latest(true).Reference()
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
