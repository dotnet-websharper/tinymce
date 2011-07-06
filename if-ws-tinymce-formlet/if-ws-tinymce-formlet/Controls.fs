namespace IntelliFactory.WebSharper.TinyMce.Formlet



open IntelliFactory.Formlet
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html

module Controls =
    let TinyMCE () =
        Formlet.BuildFormlet <| fun _ ->
            // TinyMce.TinyMCE ()
            let body = failwith ""
            let reset = ignore
            let state = failwith ""
            body, reset, state
