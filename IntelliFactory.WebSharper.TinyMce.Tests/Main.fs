﻿namespace IntelliFactory.WebSharper.TinyMce.Tests

open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Formlets.TinyMce
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html.Client
open IntelliFactory.WebSharper.Formlets
open IntelliFactory.WebSharper.Web

(*
type SampleControl () =
    inherit Web.Control()

    [<Inline "eval($s)">]
    let Raw s : 'T = failwith "raw" 

    [<JavaScript>]
    let Init(tId) =
        let listBoxConf =  
            new ControlConfiguration(
                Title = "My list box",
                Onselect = (fun (v) ->
                    TinyMCE.ActiveEditor.WindowManager.Alert("Value selected:" + v)
                )
            )

        let splitButtonConf =  
            new ControlConfiguration(
                Title = "My split button",
                Image = "img/example.gif",
                Onclick = (fun () ->
                    TinyMCE.ActiveEditor.WindowManager.Alert("Button was clicked.")
                )
            )


        let createMenu (name:string, cm:ControlManager) = 
            match name with
            | "mylistbox" -> 
                let mlb = cm.CreateListBox("mylistbox",  listBoxConf)

                mlb.Add("Some item 1", "val1")
                mlb.Add("Some item 2", "val2")
                mlb.Add("Some item 3", "val3")


                mlb :> TinyMce.Control

            | "mysplitbutton" -> 
                let c = cm.CreateSplitButton("mysplitbutton",  splitButtonConf )

                c.OnRenderMenu.Add (fun (c,m:DropMenu) ->
                        m.Add(new ControlConfiguration(Title = "Some title", Class = "mceMenuItemTitle"))
                        |> ignore

                        m.Add(new ControlConfiguration(Title = "Some item 1", Onclick = (fun () ->
                                    TinyMCE.ActiveEditor.WindowManager.Alert("Some  item 1 was clicked")
                                )
                            )
                        ) |> ignore

                        m.Add(new ControlConfiguration(Title = "Some item 2", Onclick = (fun () ->
                                    TinyMCE.ActiveEditor.WindowManager.Alert("Some  item 2 was clicked")
                                )
                            )
                        ) |> ignore

                ) |> ignore


                c :> TinyMce.Control

            | _ -> null


        let plugin = new Plugin ( CreateControl = createMenu )

        TinyMCE.Create("tinymce.plugins.CustomListBoxSplitButtonPlugin", plugin)

        TinyMce.PluginManager.Add("exampleCustomListBoxSplitButton", Raw "tinymce.plugins.CustomListBoxSplitButtonPlugin")

        let editorConfig = 
            new TinyMCEConfiguration (
                Theme = "advanced",
                Mode = Mode.Exact,
                Elements = tId,
                Theme_advanced_toolbar_location = ToolbarLocation.Top,
                Plugins = "-exampleCustomListBoxSplitButton",
                Theme_advanced_buttons1 = "mylistbox,mysplitbutton,bold",
                Theme_advanced_buttons2 = "", 
                Theme_advanced_buttons3 = "",
                Theme_advanced_buttons4 = "" 
            )
        

        TinyMCE.Init(editorConfig)


    [<JavaScript>]
    override this.Body = 
        let tId = NewId()
        Div [
            TextArea [Attr.Id tId; Text "default content"]
            |>! OnAfterRender (fun el ->
                    Init(tId)
            )
        ]
        :> _
type SampleControl () =
    inherit Web.Control()

    [<JavaScript>]
    let Init(tId) =
        let config = 
            new TinyMCEConfiguration (
                Theme = "advanced",
                Mode = Mode.Exact,
                Elements = tId
            )
        TinyMCE.Init(config)

    [<JavaScript>]
    override this.Body = 
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
        :> _ 
type SampleControl () =
    inherit Web.Control()

    [<JavaScript>]
    let Init(tId) =
        let config = 
            new TinyMCEConfiguration (
                Theme = "advanced",
                Mode = Mode.Exact,
                Elements = tId
            )
        TinyMCE.Init(config)

    [<JavaScript>]
    override this.Body = 
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
        :> _

type SampleControl () =
    inherit Web.Control()

    [<JavaScript>]
    let Init () =

        TinyMCE.Init (
            new TinyMCEConfiguration(
                Theme = "simple",
                Mode = Mode.Textareas,
                Oninit = (fun () ->
                    let editor = TinyMCE.Get("test_area")
                    editor.SetContent("New content") |> ignore
                    editor.OnClick.Add (fun (ed:Editor) ->
                                JavaScript.Alert(ed.GetContent())
                    ) |> ignore
                )
                    

            )
        )


    [<JavaScript>]
    override this.Body = 
        TextArea [Text "Default  text"]
        |>! OnAfterRender (fun el ->
            TinyMCE.Init (
                new TinyMCEConfiguration(
                    Theme = "simple",
                    Mode = Mode.Textareas,
                    Oninit = (fun () ->
                        let editor = TinyMCE.Get(el.Id)
                        editor.SetContent("New content") |> ignore
                        editor.OnClick.Add (fun (ed:Editor) ->
                            JavaScript.Alert(ed.GetContent())
                        ) |> ignore
                    )
                )
            )
        )
        :> _

let Editor = 
    TextArea [Text "Default  text"]
    |>! OnAfterRender (fun el ->
        TinyMCE.Init (
            new TinyMCEConfiguration(
                Theme = "simple",
                Mode = Mode.Textareas,
                Oninit = (fun () ->
                    let editor = TinyMCE.Get(el.Id)
                    editor.SetContent("New content") |> ignore
                    editor.OnClick.Add (fun (ed:Editor) ->
                        JavaScript.Alert(ed.GetContent())
                    ) |> ignore
                )
            )
        )
    )

type SampleControl () =
    inherit Web.Control()

    [<JavaScript>]
    let Init () =

        TinyMCE.Init (
            new TinyMCEConfiguration(
                Theme = "simple",
                Mode = Mode.Textareas,
                Oninit = (fun () ->
                    let editor = TinyMCE.Get("test_area")
                    editor.SetContent("New content") |> ignore
                    editor.OnClick.Add (fun (ed:Editor) ->
                                JavaScript.Alert(ed.GetContent())
                    ) |> ignore
                )
                    

            )
        )


    [<JavaScript>]
    override this.Body = 
        TextArea [Id "test_area"] -< [Text "Default  text"]
        |>! OnAfterRender (fun _ ->
            Init()
        )
        :> _
        
*)

[<Sealed>]
type SampleControl () =
    inherit Web.Control()

    [<JavaScript>]
    override this.Body = 
        Test.Run()
        :> _



open IntelliFactory.WebSharper.Sitelets

type Act = | Index

module Site =

    open IntelliFactory.WebSharper.Html.Server

    let HomePage =
        Sitelets.Content.PageContent <| fun ctx ->
            { Page.Default with
                Title = Some "WebSharper TinyMce Tests"
                Body = [Div [new SampleControl()]] }

    let Main = Sitelet.Content "/" Index HomePage

[<Sealed>]
type Website() =
    interface IWebsite<Act> with
        member this.Sitelet = Site.Main
        member this.Actions = [Act.Index]

[<assembly: Website(typeof<Website>)>]
do ()
