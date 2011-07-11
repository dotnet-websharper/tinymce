namespace IntelliFactory.WebSharper.Formlet.TinyMce



open IntelliFactory.Formlet
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Html

type HtmlEditorConfiguration=
    {
        Theme : string
        Width : option<int>
        Height : option<int>
        CustomElements : option<string>
        Plugins : option<string>
        ThemeAdvancedToolbarLocation : option<string>
        ThemeAdvancedToolbarAlign : option<string>
        ThemeAdvancedStatusbarLocation : option<string>
        ThemeAdvancedButtons1 : option<string>
        ThemeAdvancedButtons2 : option<string>
        ThemeAdvancedButtons3 : option<string>
        ThemeAdvancedButtons4 : option<string>
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
                ThemeAdvancedButtons2 = None 
                ThemeAdvancedButtons3 = None 
                ThemeAdvancedButtons4 = None 
            }

module Controls =



    [<JavaScript>]
    let HtmlEditor conf (default_content: string) : Formlet<string> =
        Formlet.BuildFormlet <| fun _ ->
            
            let state = new Event<_>()
            let trigger v =
                Result.Success v
                |> state.Trigger

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

                match conf.ThemeAdvancedToolbarLocation with
                | Some s -> tConf.Theme_advanced_toolbar_location <- s
                | None   -> ()

                match conf.ThemeAdvancedToolbarAlign with
                | Some s -> tConf.Theme_advanced_toolbar_align <- s
                | None   -> ()

                match conf.ThemeAdvancedStatusbarLocation with
                | Some s -> tConf.Theme_advanced_statusbar_location <- s
                | None   -> ()

                match conf.ThemeAdvancedButtons1 with
                | Some s -> tConf.Theme_advanced_buttons1 <- s 
                | None   -> ()

                match conf.ThemeAdvancedButtons2 with
                | Some s -> tConf.Theme_advanced_buttons2 <- s
                | None   -> ()

                match conf.ThemeAdvancedButtons3 with
                | Some s -> tConf.Theme_advanced_buttons3 <- s
                | None   -> ()

                match conf.ThemeAdvancedButtons4 with
                | Some s -> tConf.Theme_advanced_buttons4 <- s
                | None   -> ()
                tConf

            let body =
                TextArea [Attr.Id tId; Text default_content]
                |>! OnAfterRender (fun el ->
                    TinyMCE.Init tConf
                    trigger default_content
                )
            let reset () = 
                let tinyMce = TinyMCE.Get(tId)
                tinyMce.SetContent(default_content)
                |> ignore
                trigger default_content

            body, reset, state.Publish

    [<JavaScript>]
    let Editor default_content = HtmlEditor HtmlEditorConfiguration.Default default_content
