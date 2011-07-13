namespace IntelliFactory.WebSharper.TinyMce.Test

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Web
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Formlet.TinyMce

module Test =

    module U = 
        [<Inline "console.log($x)">]
        let Log x = ()
        
    module Formlet =
            
        [<JavaScript>]
        let SimpleEditorDefaultConfiguration() =
            let conf = SimpleHtmlEditorConfiguration.Default
    
            Controls.SimpleHtmlEditor conf "default"
            |> Enhance.WithSubmitAndResetButtons


        [<JavaScript>]
        let SimpleEditorCustomConfiguration() =
            let conf = { SimpleHtmlEditorConfiguration.Default with
                            Width = Some 500
                            Height = Some 500
                        }
    
            Controls.SimpleHtmlEditor conf "default"
            |> Enhance.WithSubmitAndResetButtons


        [<JavaScript>]
        let AdvancedEditorDefaultConfiguration() =
            let conf = AdvancedHtmlEditorConfiguration.Default
    
            Controls.AdvancedHtmlEditor conf "default"
            |> Enhance.WithSubmitAndResetButtons
    
    
        [<JavaScript>]
        let AdvancedEditorCustomConfiguration() =
            let conf =
                {AdvancedHtmlEditorConfiguration.Default with
                    Width = Some 600
                    Height = Some 400
                    ToolbarLocation = Some ToolbarLocation.Top
                    ToolbarAlign = Some ToolbarAlign.Left
                    Plugins = Some "table,contextmenu,paste" 
                    Buttons =
                        Some [
                            [ ButtonType.Bold; ButtonType.Anchor]
                            []
                            []
                        ]
                }
    
            Controls.AdvancedHtmlEditor conf "default"
            |> Enhance.WithSubmitAndResetButtons


    // Direct bindings
    module DirectBindings =
            
        [<JavaScript>]
        let SimpleTest() =
            let Init(tId) =
                let config = 
                    new TinyMCEConfiguration (
                        Theme = "advanced",
                        Mode = Mode.Exact,
                        Elements = tId,
                        Onchange_callback = (fun editor ->
                             JavaScript.Alert(editor.GetContent ())
                        ),
                        
                        Oninit = (fun () ->
                            let e = TinyMCE.Get(tId)
                            e.OnKeyUp.Add (fun (editor: Editor) ->
                                 JavaScript.Alert(editor.GetContent ())
                            )
                            |> ignore
                        )
                    )
                TinyMCE.Init(config)

            let tId = NewId()
            Div [
                TextArea [Attr.Id tId; Text "default content"]
                |>! OnAfterRender (fun el ->
                        Init(tId)
                    )
                ]


    
    [<JavaScript>]
    let TestFormlet (name: string)  (descr: string) (formlet: Formlet<string>) =
        Div [
            H3 [Text name]
            P [Text descr] 
            P [
                Formlet.Do {
                    let! v = formlet
                    let! _ = Controls.TextArea v
                    return ()
                }
                |> Enhance.WithFormContainer
            ]
        ]
    
    [<JavaScript>]
    let TestDirectBindings (name: string)  (descr: string) (formlet: Formlet<string>) =
        Div [
            H3 [Text name]
            P [Text descr] 
            P [
                Formlet.Do {
                    let! v = formlet
                    let! _ = Controls.TextArea v
                    return ()
                }
                |> Enhance.WithFormContainer
            ]
        ]

    [<JavaScript>]
    let Run() =
        Div [
            // Formlet tests
            TestFormlet "SimpleHtmlEditor" "Creates SimpleHtmlEditor with default configuration"  <|
                Formlet.SimpleEditorDefaultConfiguration()

            TestFormlet "SimpleHtmlEditor" "Creates SimpleHtmlEditor with custom configuration: width = 500, height = 500"  <|
                Formlet.SimpleEditorCustomConfiguration()

            TestFormlet "AdvancedHtmlEditor" "Creates AdvancedHtmlEditor with default configuration"  <|
                Formlet.AdvancedEditorDefaultConfiguration()

            TestFormlet "AdvancedHtmlEditor" "Creates AdvancedHtmlEditor with custom configuration: width = 600, height = 400, ToolbarLocation = Top, ToolbarAlign = Left, Plugins = table,contextmenu,paste, Only first row of Buttons = Bold, Anchor"  <|
                Formlet.AdvancedEditorCustomConfiguration()

            // Direct bindings tests
            DirectBindings.SimpleTest()
        ]