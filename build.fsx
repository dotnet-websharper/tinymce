#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.TinyMce", "3.0-alpha")
        .References(fun r ->
            [
                r.Assembly "System.Web"
            ])
    |> fun bt -> bt.WithFramework(bt.Framework.Net40)

let main =
    bt.WebSharper.Extension("IntelliFactory.WebSharper.TinyMce")
        .SourcesFromProject()

let formlet =
    bt.WebSharper.Library("IntelliFactory.WebSharper.Formlets.TinyMce")
        .SourcesFromProject()
        .References(fun r -> [r.Project main])

let test =
    bt.WebSharper.HtmlWebsite("IntelliFactory.WebSharper.TinyMce.Tests")
        .SourcesFromProject()
        .References(fun r -> [r.Project main; r.Project formlet])

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
