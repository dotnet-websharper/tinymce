namespace IntelliFactory.WebSharper.TinyMceExtension

module TinyMce =
    open IntelliFactory.WebSharper.InterfaceGenerator
    open IntelliFactory.WebSharper.Dom;
        
    let private ConstantStrings ty l =
        List.map (fun s -> (s =? ty |> WithGetterInline ("'" + s + "'")) :> CodeModel.IClassMember) l


    let DialogType = Type.New ()

    let DialogTypeClass = 
        Class "DialogType"
        |=> DialogType
        |+> ConstantStrings DialogType ["window"; "modal"]


    let Mode = Type.New ()
        
    let ModeClass = 
        Class "Mode"
        |=> Mode
        |+> ConstantStrings Mode ["textareas"; "specific_textareas"; "exact"; "none"]


    let TinyMCE = Type.New ()
    let TinyMCEConfiguration = Type.New ()

    let TinyMCEConfigurationClass =
        Pattern.Config "TinyMCEConfiguration" {
            Required = []
            Optional = 
                [
                    // General
                    "accessibility_warnings" , T<bool>
                    "auto_focus" , T<string>
                    "browsers" , T<string>
                    "class_filter " , T<string> * T<string> ^-> T<obj>
                    "custom_shortcuts" , T<bool>
                    "dialog_type" , DialogType
                    "directionality" , T<string>
                    "editor_deselector" , T<obj>
                    "editor_selector" , T<obj>
                    "elements" , T<string>
                    "gecko_spellcheck" , T<bool>
                    "keep_styles" , T<bool>
                    "language" , T<string>
                    "mode" , Mode 
                    "nowrap" , T<bool>
                    "object_resizing" , T<bool>
                    "plugins" , T<string>
                    "readonly" , T<int>
                    "skin" , T<string>
                    "skin_variant" , T<string>
                    "table_inline_editing" , T<bool>
                    "theme" , T<string>
                    "imagemanager_contextmenu" , T<bool>

                    // Callbacks
                    "cleanup_callback", T<string> * T<string> ^-> T<string>
                    "execcommand_callback", T<string * string * string * string * string -> bool>
                    "file_browser_callback", T<string * string * string * obj -> unit>
                    "handle_event_callback", T<Event -> bool>
                    "handle_node_change_callback", T<string * Node * int * int * bool * bool -> unit>
                    "init_instance_callback", TinyMCE ^-> T<unit>
                    "onchange_callback", TinyMCE ^-> T<unit>
                    "oninit", T<unit -> unit> 
                    "onpageload", T<unit -> unit> 
                    "remove_instance_callback", TinyMCE ^-> T<unit>
                    "save_callback", T<string * string * string -> string>
                    "setup", TinyMCE ^-> T<unit>
                    "setupcontent_callback", T<string * Node * string -> unit>
                    "urlconverter_callback", T<string * string * bool -> string>

                    // URL
                    "convert_urls" , T<bool>
                    "relative_urls" , T<bool>
                    "remove_script_host" , T<bool>
                    "document_base_url" , T<string>

                    // Layout
                    "body_id" , T<string>
                    "body_class" , T<string>
                    "constrain_menus" , T<bool>
                    "content_css" , T<string>
                    "popup_css" , T<string>
                    "popup_css_add" , T<string>
                    "editor_css" , T<string>
                    "width" , T<string>
                    "height" , T<string>

                    // Visual aids
                    "visual" , T<bool>
                    "visual_table_class" , T<string>
                    
                    // Undo/Redo
                    "custom_undo_redo" , T<bool>
                    "custom_undo_redo_levels" , T<int>
                    "custom_undo_redo_keyboard_shortcuts" , T<int>
                    "custom_undo_redo_restore_selection" , T<bool>

                    // TO DO Configuration options:
                    // Cleanup/Output
                    // File lists
                    // Triggers/Patches
                    // Advanced theme

                ]
        }
        |=> TinyMCEConfiguration

    

    let TinyMCEClass =
        Class "tinyMCE"
        |=> TinyMCE
        |+> [
                "init" => TinyMCEConfiguration ^-> T<unit>
                |> WithComment ""

                "get" => T<string> ^-> TinyMCE
                |> WithComment ""
                
            ]
        |+> Protocol
            [
                "getContent" => T<unit> ^-> T<string> 
                |> WithComment ""

                "show" => T<unit> ^-> T<unit> 
                |> WithComment ""

                "hide" => T<unit> ^-> T<unit> 
                |> WithComment ""

                "execCommand" => T<string> ^-> T<unit> 
                |> WithComment ""

            ]

    let Assembly =
        Assembly [
            Namespace "IntelliFactory.WebSharper.TinyMce" [
                TinyMCEConfigurationClass
                TinyMCEClass
                DialogTypeClass
                ModeClass
            ]
        ]

