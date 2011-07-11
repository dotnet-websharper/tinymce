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

//type SampleControl () =
//    inherit Web.Control()
//
//    [<JavaScript>]
//    override this.Body = 
//        let conf =
//            {HtmlEditorConfiguration.Default with
//                Theme = "advanced"
//                Width = Some 600
//                Height = Some 400
//                ThemeAdvancedToolbarLocation = Some "top" 
//                ThemeAdvancedToolbarAlign = Some "left"
//                
//                // Some "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,formatselect,|,bullist,numlist"
//                ThemeAdvancedButtons1 = Some [
//                    [ButtonType.Foo; ButtonType.BarBar]
//                ]
//            }
//        
//        Controls.HtmlEditor conf "default"
//        |> Enhance.WithSubmitAndResetButtons
//        |> Formlet.Map (fun x ->
//            U.Log <| ("submitted" , x)
//        )
//        |> Enhance.WithFormContainer
//        :> _

type SampleControl () =
    inherit Web.Control()

    
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
                    let e = TinyMCE.Get("test_area")
                    e.ExecCommand("foo")
                    ()
            )
        conf.Execcommand_callback <- fun (a,b,c,d,e) ->
            U.Log(a,b,c,d,e)
            U.Log <| JavaScript.Get "innerHtml" b
            true

        TinyMCE.Init (conf)

    [<JavaScript>]
    override this.Body = 
        Div [
            TextArea [Id "test_area"] -< [Text "Default  text"]
            |>! OnAfterRender (fun _ ->
                Init()
            )
            
            Button [Text "Foo"]
            |>! OnClick (fun _ _->
                let e = TinyMCE.Get("test_area")
                e.ExecCommand("foo")
            )
        ] :> _
