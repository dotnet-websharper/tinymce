namespace IntelliFactory.WebSharper.TinyMce.Test

module Test =

    open IntelliFactory.WebSharper
    open IntelliFactory.WebSharper.Html
    open IntelliFactory.WebSharper.Formlet
    open IntelliFactory.WebSharper.Web
    open IntelliFactory.WebSharper
    open IntelliFactory.WebSharper.TinyMce
    open IntelliFactory.WebSharper.Formlet.TinyMce
    
    
    // Utils

    module L =
        [<Inline "console.log($x)">]
        let Log x = ()
    

    // Test 1

    module SimpleTinyMce =
        [<JavaScript>]
        let Init () =
            let conf =
                new TinyMCEConfiguration(
                    Theme = "advanced",
                    Mode = Mode.Textareas,
                    Theme_advanced_toolbar_location = ToolbarLocation.Top,
                    Theme_advanced_toolbar_align =ToolbarAlign.Right,
                    Theme_advanced_statusbar_location = StatusbarLocation.Top,
                    
                    Oninit = fun () ->
                        let e = TinyMCE.Get("Test1")
                        e.ExecCommand("bold")
                        ()
                )
            conf.Execcommand_callback <- fun (a,b,c,d,e) ->
                L.Log(a,b,c,d,e)
                L.Log <| JavaScript.Get "innerHtml" b
                false
        
            TinyMCE.Init (conf)
        
    
        
        [<JavaScript>]
        let TinyMCE =
            Div [
                TextArea [Id "Test1"] -< [Text "Default  text"]
                |>! OnAfterRender (fun _ ->
                    Init()
                )
            
                Button [Text "Foo"]
                |>! OnClick (fun _ _->
                    let e = TinyMCE.Get("Test1")
                    e.ExecCommand("italic")
                )
            ] :> IPagelet 
    module Test1 =
        [<JavaScript>]
        let Init () =
            let conf =
                new TinyMCEConfiguration(
                    Theme = "advanced",
                    Mode = Mode.Textareas,
                    Theme_advanced_toolbar_location = ToolbarLocation.Top,
                    Theme_advanced_toolbar_align =ToolbarAlign.Right,
                    Theme_advanced_statusbar_location = StatusbarLocation.Top,
                    
                    Oninit = fun () ->
                        let e = TinyMCE.Get("Test1")
                        e.ExecCommand("bold")
                        ()
                )
            conf.Execcommand_callback <- fun (a,b,c,d,e) ->
                L.Log(a,b,c,d,e)
                L.Log <| JavaScript.Get "innerHtml" b
                false
        
            TinyMCE.Init (conf)
        
    
        
        [<JavaScript>]
        let TinyMCE =
            Div [
                TextArea [Id "Test1"] -< [Text "Default  text"]
                |>! OnAfterRender (fun _ ->
                    Init()
                )
            
                Button [Text "Foo"]
                |>! OnClick (fun _ _->
                    let e = TinyMCE.Get("Test1")
                    e.ExecCommand("italic")
                )
            ] :> IPagelet 