namespace IntelliFactory.WebSharper.Formlet.TinyMce



open IntelliFactory.Formlet
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.TinyMce
open IntelliFactory.WebSharper.Html

module Controls =

    [<JavaScript>]
    let (:=) x y : (string * obj) = (x, y)

    [<Inline "void($r[$k]=$v)">]
    let Set (r: obj) (k: string) (v: obj) : unit = ()

    [<JavaScript>]
    let (!) (fields: seq<(string * obj)>) =
        let r = obj ()
        for (k,v) in fields do
            Set r k v
        r

    [<JavaScript>]
    let TinyMCE () =
        Formlet.BuildFormlet <| fun _ ->
            let body =
                TextArea []
                |>! OnAfterRender (fun el ->
                    let conf =
                        ![
                            "onchange_callback" := fun inst -> ()
                            "mode" := "exact"
                            "elements" := el.Id
                        ]
                    TinyMCE.Init (unbox box conf)
                )
            let reset () = ()
            let state = failwith ""
            body, reset, state
