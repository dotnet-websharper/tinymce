namespace IntelliFactory.WebSharper.TinyMce.Test

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Web
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Formlet.TinyMce

type SampleControl () =
    inherit Web.Control()

    [<JavaScript>]
    override this.Body = 
        Test.Run()
        :> _