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

    [<Rpc>]
    let List () = ["A"; "B"; "C"]

type T [<JavaScript>] () =
    [<JavaScript>]
    [<Name "toString">]
    override this.ToString () = "T"

type SampleControl () =
    inherit Web.Control()

    [<JavaScript>]
    override this.Body = 
        let xs = U.List ()
        xs |> List.iter U.Log

        let t = new T ()
        U.Log( "t", t)
        U.Log ("t.ToString()", t.ToString())
        let conf =
            {AdvancedHtmlEditorConfiguration.Default with
                Width = Some 600
                Height = Some 400
                ToolbarLocation = Some ToolbarLocation.Top
                ToolbarAlign = Some ToolbarAlign.Left
                Buttons =
                    Some [
                        [ ButtonType.Bold; ButtonType.Anchor]
                        []
                        []
                    ]
            }
        
        Controls.AdvancedHtmlEditor conf "default"
        |> Enhance.WithSubmitAndResetButtons
        |> Formlet.Map (fun x ->
            U.Log <| ("submitted" , x)
        )
        |> Enhance.WithFormContainer
        :> _