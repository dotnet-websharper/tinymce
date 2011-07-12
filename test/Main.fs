namespace IntelliFactory.WebSharper.TinyMce.Test

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Web
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Formlet.TinyMce

open Test

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
//                ThemeAdvancedToolbarLocation = Some ToolbarLocation.Top
//                ThemeAdvancedToolbarAlign = Some ToolbarAlign.Left
//                ThemeAdvancedButtons1 = 
//                    Some [
//                        [ ButtonType.Bold; ButtonType.Anchor]
//                        []
//                        []
//                    ]
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
    override this.Body = 
        Div [
            SimpleTinyMce.TinyMCE
        ] :> _
