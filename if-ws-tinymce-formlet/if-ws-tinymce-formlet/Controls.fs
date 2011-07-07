namespace IntelliFactory.WebSharper.Formlet.TinyMce



open IntelliFactory.Formlet
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html

module Controls =
    [<JavaScript>]
    let TinyMCE () =
        Div [Text "Ff"]
//        Formlet.BuildFormlet <| fun _ ->
//            // TinyMce.TinyMCE ()
//            let body = failwith ""
//            let reset = ignore
//            let state = failwith ""
//            body, reset, state
