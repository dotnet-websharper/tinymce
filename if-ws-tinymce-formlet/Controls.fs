namespace IntelliFactory.WebSharper.Formlet.TinyMce



open IntelliFactory.Formlet
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Html

type ButtonType =
    | Bold
    | Italic
    | Underline
    | Strikethrough
    | Justifyleft
    | Justifycenter
    | Justifyright
    | Justifyfull
    | Bullist
    | Numlist
    | Outdent
    | Indent
    | Cut
    | Copy
    | Paste
    | Undo
    | Redo
    | Link
    | Unlink
    | Image
    | Cleanup
    | Help
    | Code
    | Hr
    | Removeformat
    | Formatselect
    | Fontselect
    | Fontsizeselect
    | Styleselect
    | Sub
    | Sup
    | Forecolor
    | Backcolor
    | Forecolorpicker
    | Backcolorpicker
    | Charmap
    | Visualaid
    | Anchor
    | Newdocument
    | Blockquote
    | Separator
    | Custom of string

    with 
        [<JavaScript>]
        member this.Show () =
            match this with
            | Bold -> "bold"
            | Italic -> "italic"
            | Underline -> "underline"
            | Strikethrough -> "strikethrough"
            | Justifyleft -> "justifyleft"
            | Justifycenter -> "justifycenter"
            | Justifyright -> "justifyright"
            | Justifyfull -> "justifyfull"
            | Bullist -> "bullist"
            | Numlist -> "numlist"
            | Outdent -> "outdent"
            | Indent -> "indent"
            | Cut -> "cut"
            | Copy -> "copy"
            | Paste -> "paste"
            | Undo -> "undo"
            | Redo -> "redo"
            | Link -> "link"
            | Unlink -> "unlink"
            | Image -> "image"
            | Cleanup -> "cleanup"
            | Help -> "help"
            | Code -> "code"
            | Hr -> "hr"
            | Removeformat -> "removeformat"
            | Formatselect-> "formatselect"
            | Fontselect -> "fontselect"
            | Fontsizeselect -> "fontselect"
            | Styleselect -> "styleselect"
            | Sub -> "sub"
            | Sup -> "sup"
            | Forecolor -> "forecolor"
            | Backcolor -> "backcolor"
            | Forecolorpicker -> "forecolorpicker"
            | Backcolorpicker -> "backcolorpicker"
            | Charmap -> "charmap"
            | Visualaid -> "visualaid"
            | Anchor -> "anchor"
            | Newdocument -> "newdocument"
            | Blockquote -> "blackquote"
            | Separator -> "separator"
            | Custom  s -> s


type ButtonRow = list<ButtonType>


type HtmlEditorConfiguration=
    {
        Theme : string
        Width : option<int>
        Height : option<int>
        CustomElements : option<string>
        Plugins : option<string>
        ThemeAdvancedToolbarLocation : option<TinyMce.ToolbarLocation>
        ThemeAdvancedToolbarAlign : option<ToolbarAlign>
        ThemeAdvancedStatusbarLocation : option<StatusbarLocation>
        ThemeAdvancedButtons1 : option<list<ButtonRow>>
    }
    with
        [<JavaScript>]
        static member Default = 
            {
                Theme = "simple"
                Width = None
                Height = None
                CustomElements = None
                Plugins = None
                ThemeAdvancedToolbarLocation = None 
                ThemeAdvancedToolbarAlign = None 
                ThemeAdvancedStatusbarLocation = None 
                ThemeAdvancedButtons1 = None 
            }

module Controls =
    [<Inline "console.log($x)">]
    let Log x = ()

    [<JavaScript>]
    let HtmlEditor conf (defContent: string) : Formlet<string> =
        Formlet.BuildFormlet <| fun _ ->
            
            let state = new Event<_>()
            let oldValue = ref None
            let trigger v =
                let t () =
                    oldValue := Some v
                    Result.Success v
                    |> state.Trigger
                match oldValue.Value with
                | Some ov ->
                    if v <> ov then t ()
                | None ->
                    t ()

            let tId = NewId ()

            // Set up configuration
            let tConf =
                let tConf =
                    new TinyMCEConfiguration (
                        Theme = conf.Theme,
                        Mode = Mode.Exact,
                        Elements = tId,
                        Onchange_callback = (fun tMce ->
                            trigger <| tMce.GetContent ()
                        ),
                        
                        Oninit = (fun () ->
                            let e = TinyMCE.Get(tId)
                            e.OnKeyUp.Add (fun (e: Editor) ->
                                e.GetContent() |> trigger
                            )
                            |> ignore
                        )
                )

                match conf.Height with
                | Some h -> tConf.Height <- (string h) + "px"
                | None   -> ()

                match conf.Width with
                | Some w -> tConf.Width <- (string w) + "px"
                | None   -> ()

                match conf.CustomElements with
                | Some s -> tConf.Custom_elements <- s
                | None   -> ()

                match conf.Plugins with
                | Some s -> tConf.Plugins <- s
                | None   -> ()

                match conf.ThemeAdvancedToolbarAlign with
                | Some ta -> 
                    tConf.Theme_advanced_toolbar_align <- ta
                | None ->
                    ()

                match conf.ThemeAdvancedStatusbarLocation with
                | Some l -> 
                    tConf.Theme_advanced_statusbar_location <- l
                | None ->
                    ()

                match conf.ThemeAdvancedToolbarLocation with
                | Some l -> 
                    tConf.Theme_advanced_toolbar_location <- l
                | None ->
                    ()

                match conf.ThemeAdvancedButtons1 with
                | Some bs -> 
                    bs
                    |> List.iteri (fun ix row ->
                        let prop = "theme_advanced_buttons" + (string <| ix + 1)
                        Log ("th:", prop)
                        match row with
                        | [] ->
                            JavaScript.Set tConf prop ""
                        | _  ->
                            row
                            |> Seq.map (fun x -> 
                                x.Show()
                            )
                            |> Seq.reduce (fun x y -> x + "," + y)
                            |> fun x ->
                                Log ("string" , x)
                                JavaScript.Set tConf prop x
                    )
                | None   -> ()

                tConf

            let body =
                TextArea [Attr.Id tId; Text defContent]
                |>! OnAfterRender (fun el ->
                    TinyMCE.Init tConf


                    trigger defContent
                )
            let reset () = 
                let tinyMce = TinyMCE.Get(tId)
                tinyMce.SetContent(defContent)
                |> ignore
                trigger defContent

            body, reset, state.Publish

    [<JavaScript>]
    let Editor defContent = HtmlEditor HtmlEditorConfiguration.Default defContent
