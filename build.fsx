#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.TinyMce", "3.0")
        .References(fun r ->
            [
                r.Assembly "System.Web"
            ])
    |> fun bt -> bt.WithFramework(bt.Framework.Net40)

let main =
    bt.WebSharper.Extension("WebSharper.TinyMce")
        .SourcesFromProject()

let formlet =
    bt.WebSharper.Library("WebSharper.Formlets.TinyMce")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.NuGet("IntelliFactory.Reactive").Reference()
                r.NuGet("WebSharper.Formlets").Reference()
                r.Project main
            ])

let test =
    bt.WebSharper.HtmlWebsite("WebSharper.TinyMce.Tests")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.NuGet("IntelliFactory.Reactive").Reference()
                r.NuGet("WebSharper.Formlets").Reference()
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
