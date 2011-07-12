namespace IntelliFactory.WebSharper.Formlet.TinyMce



open IntelliFactory.Formlet
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Html

/// Represents available buttons.
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



/// Button Row
type ButtonRow = list<ButtonType>

type Theme =
    | Simple 
    | Advanced
    | Custom of string


module internal Utils =

    [<JavaScript>]
    let ShowTheme theme =
        match theme with
        | Simple -> "simple"
        | Advanced -> "advanced"
        | Custom s -> s

    [<JavaScript>]
    let ShowButtonType btype =
        match btype with
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
        | ButtonType.Custom  s -> s


/// Editor Configuration
type internal HtmlEditorConfiguration=
    {
        Theme : Theme
        Width : option<int>
        Height : option<int>
        CustomElements : option<string>
        Plugins : option<string>
        AdvancedToolbarLocation : option<TinyMce.ToolbarLocation>
        AdvancedToolbarAlign : option<ToolbarAlign>
        AdvancedStatusbarLocation : option<StatusbarLocation>
        AdvancedButtons : option<list<ButtonRow>>
    }
    with
        [<JavaScript>]
        static member Default = 
            {
                Theme = Theme.Simple
                Width = None
                Height = None
                CustomElements = None
                Plugins = None
                AdvancedToolbarLocation = None 
                AdvancedToolbarAlign = None 
                AdvancedStatusbarLocation = None 
                AdvancedButtons = None 
            }

type SimpleHtmlEditorConfiguration =
    {
        Width : option<int>
        Height : option<int>
        CustomElements : option<string>
        Plugins : option<string>
    }
    with 
        [<JavaScript>]
        static member Default =
            {
                Width = None
                Height = None
                CustomElements = None
                Plugins = None
            }

type AdvancedHtmlEditorConfiguration =
    {
        Width : option<int>
        Height : option<int>
        CustomElements : option<string>
        Plugins : option<string>
        ToolbarLocation : option<TinyMce.ToolbarLocation>
        ToolbarAlign : option<ToolbarAlign>
        StatusbarLocation : option<StatusbarLocation>
        Buttons : option<list<ButtonRow>>
    }
    with 
        [<JavaScript>]
        static member Default =
            {
                Width = None
                Height  = None
                CustomElements = None
                Plugins = None
                ToolbarLocation  = None
                ToolbarAlign = None
                StatusbarLocation  = None
                Buttons = None
            }
    


module Controls =

    [<JavaScript>]
    let internal HtmlEditor conf (defContent: string) : Formlet<string> =
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
                        Theme = Utils.ShowTheme conf.Theme,
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

                match conf.AdvancedToolbarAlign with
                | Some ta -> 
                    tConf.Theme_advanced_toolbar_align <- ta
                | None ->
                    ()

                match conf.AdvancedStatusbarLocation with
                | Some l -> 
                    tConf.Theme_advanced_statusbar_location <- l
                | None ->
                    ()

                match conf.AdvancedToolbarLocation with
                | Some l -> 
                    tConf.Theme_advanced_toolbar_location <- l
                | None ->
                    ()

                match conf.AdvancedButtons with
                | Some bs -> 
                    bs
                    |> List.iteri (fun ix row ->
                        let prop = "theme_advanced_buttons" + (string <| ix + 1)
                        match row with
                        | [] ->
                            JavaScript.Set tConf prop ""
                        | _  ->
                            row
                            |> Seq.map Utils.ShowButtonType
                            |> Seq.reduce (fun x y -> x + "," + y)
                            |> fun x ->
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
    let SimpleHtmlEditor (conf: SimpleHtmlEditorConfiguration)  = 
        { HtmlEditorConfiguration.Default with
            Theme = Theme.Simple
            Width = conf.Width
            Height = conf.Height
            CustomElements = conf.CustomElements
            Plugins = conf.CustomElements
        }
        |> HtmlEditor

    [<JavaScript>]
    let AdvancedHtmlEditor (conf: AdvancedHtmlEditorConfiguration)  = 
        { HtmlEditorConfiguration.Default with
            Theme = Theme.Advanced
            Width = conf.Width
            Height = conf.Height
            CustomElements = conf.CustomElements
            Plugins = conf.Plugins
            AdvancedToolbarLocation = conf.ToolbarLocation
            AdvancedToolbarAlign = conf.ToolbarAlign
            AdvancedStatusbarLocation = conf.StatusbarLocation
            AdvancedButtons = conf.Buttons
        }
        |> HtmlEditor