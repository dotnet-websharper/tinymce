namespace IntelliFactory.WebSharper.TinyMce.Test

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Web
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Formlet.TinyMce


module U =
    [<Inline "console.log($x)">]
    let Log x = ()

type SampleControl () =
    inherit Web.Control()

    [<JavaScript>]
    override this.Body = 
                let conf =
                    {HtmlEditorConfiguration.Default with
                        Theme = "advanced"
                        Width = Some 600
                        Height = Some 400
                        ThemeAdvancedToolbarLocation = Some "top" 
                        ThemeAdvancedToolbarAlign = Some "left"
                        ThemeAdvancedButtons1 = Some "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,formatselect,|,bullist,numlist"
                        ThemeAdvancedButtons2 = Some "" 
                        ThemeAdvancedButtons3 = Some ""
                        ThemeAdvancedButtons4 = Some ""
                    }
                Controls.HtmlEditor conf "default"
                |> Enhance.WithSubmitAndResetButtons
                |> Formlet.Map (fun v ->
                    U.Log ("Output", v)
                    ()
                )
                |> Enhance.WithFormContainer
                :> _
//
//type SampleControl () =
//    inherit Web.Control()
//
//    [<JavaScript>]
//    let Init () =
//
//        TinyMCE.Init (
//            new TinyMCEConfiguration(
//                Theme = "simple",
//                Mode = Mode.Textareas,
//                Oninit = fun () ->
//                    let e = TinyMCE.Get("test_area")
//                    ()
//            )
//        )
//
//    [<JavaScript>]
//    override this.Body = 
//        TextArea [Id "test_area"] -< [Text "Default  text"]
//        |>! OnAfterRender (fun _ ->
//            Init()
//        )
//        :> _