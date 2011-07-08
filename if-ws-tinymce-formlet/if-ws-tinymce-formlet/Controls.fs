namespace IntelliFactory.WebSharper.Formlet.TinyMce



open IntelliFactory.Formlet
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Html

type EditorConfiguration=
    {
        Theme : string
        Width : option<int>
        Height : option<int>
        CustomElements : option<string>
        Plugins : option<string>
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

            }

module Controls =



    [<JavaScript>]
    let CustomEditor conf (def: string) : Formlet<string> =
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
                tConf

            let body =
                TextArea [Attr.Id tId; Text def]
                |>! OnAfterRender (fun el ->
                    TinyMCE.Init tConf
                    trigger def
                )
            let reset () = 
                let tinyMce = TinyMCE.Get(tId)
                tinyMce.Value <- def
                tinyMce.SetContent(def)
                trigger def

            body, reset, state.Publish

    [<JavaScript>]
    let Editor def = CustomEditor EditorConfiguration.Default def