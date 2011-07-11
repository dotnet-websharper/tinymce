namespace IntelliFactory.WebSharper.Formlet.TinyMce



open IntelliFactory.Formlet
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Html

type ButtonType =
    | [<Name "foo">] Foo
    | [<Name "bar">] BarBar

type ButtonRow = list<ButtonType>


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
        ThemeAdvancedButtons1 : option<list<ButtonRow>>

//        ThemeAdvancedButtons2 : option<string>
//        ThemeAdvancedButtons3 : option<string>
//        ThemeAdvancedButtons4 : option<string>
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

//                match conf.ThemeAdvancedToolbarLocation with
//                | Some s -> tConf.Theme_advanced_toolbar_location <- s
//                | None   -> ()

//                match conf.ThemeAdvancedToolbarAlign with
//                | Some s -> tConf.Theme_advanced_toolbar_align <- s
//                | None   -> ()
//
//                match conf.ThemeAdvancedStatusbarLocation with
//                | Some s -> tConf.Theme_advanced_statusbar_location <- s
//                | None   -> ()

                match conf.ThemeAdvancedButtons1 with
                | Some bs -> 
                    bs
                    |> List.iteri (fun ix row ->
                        let prop = "theme_advanced_buttons" + (string ix)
                        match row with
                        | [] ->
                            ()
                        | _  ->
                            row
                            |> Seq.map string
                            |> Seq.reduce (fun x y -> x + "," + y)
                            |> fun x ->
                                Log ("string" , x)
                                JavaScript.Set tConf prop x
                    )
                | None   -> ()

//                match conf.ThemeAdvancedButtons2 with
//                | Some s -> tConf.Theme_advanced_buttons2 <- s
//                | None   -> ()
//
//                match conf.ThemeAdvancedButtons3 with
//                | Some s -> tConf.Theme_advanced_buttons3 <- s
//                | None   -> ()
//
//                match conf.ThemeAdvancedButtons4 with
//                | Some s -> tConf.Theme_advanced_buttons4 <- s
//                | None   -> ()

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
