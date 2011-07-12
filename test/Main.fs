namespace IntelliFactory.WebSharper.TinyMce.Test

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Web
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Formlet.TinyMce

open Test
    

    [<Rpc>]
    let List () = ["A"; "B"; "C"]

type T [<JavaScript>] () =
    [<JavaScript>]
    [<Name "toString">]
    override this.ToString () = "T"

        let xs = U.List ()
        xs |> List.iter U.Log

        let t = new T ()
        U.Log( "t", t)
        U.Log ("t.ToString()", t.ToString())
            {AdvancedHtmlEditorConfiguration.Default with
                ToolbarLocation = Some ToolbarLocation.Top
                ToolbarAlign = Some ToolbarAlign.Left
                Buttons =
        Controls.AdvancedHtmlEditor conf "default"
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