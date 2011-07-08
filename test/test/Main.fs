namespace IntelliFactory.WebSharper.TinyMce.Test

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Web
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Formlet.TinyMce

module U =
    [<Inline "console.log($x)">]
    let Log x = ()

type SampleControl () =
    inherit Web.Control()

    [<JavaScript>]
    override this.Body = 
        let conf =
            {EditorConfiguration.Default with
                Theme = "advanced"
                Width = Some 400
                Height = Some 400
            }
        Controls.CustomEditor conf "default"
        |> Enhance.WithSubmitAndResetButtons
        |> Formlet.Map (fun v ->
            U.Log ("Output", v)
            ()
        )
        |> Enhance.WithFormContainer
        :> _


