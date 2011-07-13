namespace IntelliFactory.WebSharper.TinyMce.Test

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Web
open IntelliFactory.WebSharper.JQuery
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
        let CreatingTinyMce() =
            let Init(tId) =
                let config = 
                    new TinyMCEConfiguration (
                        Theme = "advanced",
                        Mode = Mode.Exact,
                        Elements = tId
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
        let CreatingTinyMceWithOninitCallback() =
            let Init(tId) =
                let config = 
                    new TinyMCEConfiguration (
                        Theme = "advanced",
                        Mode = Mode.Exact,
                        Elements = tId,
                        
                        Oninit = (fun () ->
                            let editor = TinyMCE.Get(tId)
                            JQuery.Of("#change_on_init").Html("Oninit event executed, editor content: " + editor.GetContent()).Ignore
                        )
                    )
                TinyMCE.Init(config)

            let tId = NewId()
            Div [
                TextArea [Attr.Id tId; Text "default content: oninit"]
                P [Id "change_on_init"]
                |>! OnAfterRender (fun el ->
                        Init(tId)
                    )
                ]

        [<JavaScript>]
        let CreatingTinyMceWithOnchangeCallback() =
            let Init(tId) =
                let config = 
                    new TinyMCEConfiguration (
                        Theme = "advanced",
                        Mode = Mode.Exact,
                        Elements = tId,
                        Onchange_callback = (fun ed ->
                            JavaScript.Alert(ed.GetContent()) 
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
        let OnKeyupCallback() =
            let Init(tId) =
                let config = 
                    new TinyMCEConfiguration (
                        Theme = "advanced",
                        Mode = Mode.Exact,
                        Elements = tId,
                        Oninit = (fun () ->
                            let editor = TinyMCE.Get(tId)
                            editor.OnKeyUp.Add( fun (ed:Editor) ->
                                JavaScript.Alert(ed.GetContent()) 
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
        let OnClickCallback() =
            let Init(tId) =
                let config = 
                    new TinyMCEConfiguration (
                        Theme = "advanced",
                        Mode = Mode.Exact,
                        Elements = tId,
                        Oninit = (fun () ->
                            let editor = TinyMCE.Get(tId)
                            editor.OnClick.Add( fun (ed:Editor) ->
                                JavaScript.Alert(ed.GetContent()) 
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
        let UndoManagerUndoAndRedoButtons() =
            let Init(tId) =
                let config = 
                    new TinyMCEConfiguration (
                        Theme = "advanced",
                        Mode = Mode.Exact,
                        Elements = tId
                    )
                TinyMCE.Init(config)

            let tId = NewId()
            Div [
                TextArea [Attr.Id tId; Text "default content"]
                |>! OnAfterRender (fun el ->
                        Init(tId)
                    )
                Button [Text "undo"]
                |>! OnClick (fun el e ->
                        let undoManager = TinyMCE.Get(tId).UndoManager
                        undoManager.Undo()
                        |> ignore
                    )
                Button [Text "redo"]
                |>! OnClick (fun el e ->
                        let undoManager = TinyMCE.Get(tId).UndoManager
                        undoManager.Redo()
                        |> ignore
                    )
                ]


        [<JavaScript>]
        let EditorSelectionGetReplace() =
            let Init(tId) =
                let config = 
                    new TinyMCEConfiguration (
                        Theme = "advanced",
                        Mode = Mode.Exact,
                        Elements = tId
                    )
                TinyMCE.Init(config)

            let tId = NewId()
            Div [
                TextArea [Attr.Id tId; Text "default content"]
                |>! OnAfterRender (fun el ->
                        Init(tId)
                    )
                Button [Text "get selection"]
                |>! OnClick (fun el e ->
                        let selection = TinyMCE.Get(tId).Selection
                        JavaScript.Alert(selection.GetContent())
                    )
                Button [Text "replace selection with foo"]
                |>! OnClick (fun el e ->
                        let selection = TinyMCE.Get(tId).Selection
                        selection.SetContent("foo")
                    )
                ]



    // Test helpers
    
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
    let TestDirectBindings (name: string)  (descr: string) (element: Element) =
        Div [
            H3 [Text name]
            P [Text descr] 
            element
        ]



    // Test runner

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
            TestDirectBindings "Creating TinyMCE with direct bindings" "TinyMCE should be visible" <|
                DirectBindings.CreatingTinyMce()

            TestDirectBindings "Creating TinyMCE with direct bindings, Oninit callback" "When TinyMCE is initialized p element below the editor should have the editor's content" <|
                DirectBindings.CreatingTinyMceWithOninitCallback()

            TestDirectBindings "Creating TinyMCE with direct bindings, Onchange callback" "When TinyMCE content is changed JavaScript Alert dialog is shown with the editor's content" <|
                DirectBindings.CreatingTinyMceWithOnchangeCallback()

            TestDirectBindings "Creating TinyMCE with direct bindings, OnKeyUp event" "When OnKeyUp event fires JavaScript alert box with editor's content is shown" <|
                DirectBindings.OnKeyupCallback()

            TestDirectBindings "Creating TinyMCE with direct bindings, OnClick event" "When OnClick event fires JavaScript alert box with editor's content is shown" <|
                DirectBindings.OnClickCallback()

            TestDirectBindings "Using UndoManager to undo and redo changes" "Buttons below undo and redo changes" <|
                DirectBindings.UndoManagerUndoAndRedoButtons()

            TestDirectBindings "The editor's selection" "Clicking buttons below gets selection and replaces selected content" <|
                DirectBindings.EditorSelectionGetReplace()
        ]