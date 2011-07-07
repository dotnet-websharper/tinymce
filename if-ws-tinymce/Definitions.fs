namespace IntelliFactory.WebSharper.TinyMceExtension

open IntelliFactory.WebSharper.Dom

module TinyMce =
    open IntelliFactory.WebSharper.InterfaceGenerator

    let TinyMCE = Type.New()
    let TinyMCEConfiguration = Type.New ()

    let TinyMCEConfigurationClass =
        Pattern.Config "TinyMCEConfiguration" {
            Required = []
            Optional = 
                [
                    "mode" , T<string>
                    "theme" , T<string>
                ]
        }
        |=> TinyMCEConfiguration


    let TinyMCEClass =
        Class "tinyMCE"
        |=> TinyMCE
        |+> [
                "init" => TinyMCEConfiguration ^-> T<unit>
                |> WithComment ""
                
            ]

    let Assembly =
        Assembly [
            Namespace "IntelliFactory.WebSharper.TinyMce" [
                TinyMCEConfigurationClass
                TinyMCEClass
            ]
        ]

