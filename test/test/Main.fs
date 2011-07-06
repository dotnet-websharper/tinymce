namespace IntelliFactory.WebSharper.TinyMce.Test

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Web
open IntelliFactory.WebSharper

open IntelliFactory.WebSharper.TinyMce

module Sample =
    [<JavaScript>]
    let Init () =
        JavaScript.Alert "he"
        TinyMCE.Init (
            new TinyMCEConfiguration(
                Theme = "simple",
                Mode = "textareas"
            )
        )
type SampleControl () =
    inherit Web.Control()

    [<JavaScript>]
    override this.Body = 
        TextArea [Text "Default  text"]
        |>! OnAfterRender (fun _ ->
            Sample.Init ()
        )
        :> _


