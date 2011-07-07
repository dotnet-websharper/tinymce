﻿namespace IntelliFactory.WebSharper.TinyMceExtension

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


    let ThemeAdvancedLayoutManager = Type.New ()
        
    let ThemeAdvancedLayoutManagerClass = 
        Class "ThemeAdvancedLayoutManager"
        |=> ThemeAdvancedLayoutManager
        |+> ConstantStrings ThemeAdvancedLayoutManager ["SimpleLayout"; "RowLayout"; "CustomLayout"]
        

    let EntityEncoding = Type.New ()
        
    let EntityEncodingClass = 
        Class "EntityEncoding"
        |=> EntityEncoding
        |+> ConstantStrings EntityEncoding ["named"; "numeric"; "raw"]



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

                    // Cleanup/Output
                    "apply_source_formatting" , T<bool>
                    "convert_fonts_to_spans" , T<bool>
                    "convert_newlines_to_brs" , T<bool>
                    "custom_elements" , T<string>
                    "doctype" , T<string>
                    "element_format" , T<string>
                    "encoding" , T<string>
                    "entities" , T<string>
                    "entity_encoding" , EntityEncoding  
                    "extended_valid_elements" , T<string>
                    "fix_list_elements" , T<bool>
                    "font_size_classes" , T<string>
                    "font_size_style_values" , T<string>
                    "force_p_newlines" , T<bool>
                    "force_br_newlines" , T<bool>
                    "force_hex_style_colors" , T<bool>
                    "forced_root_block" , T<string>
                    // "formats" , T<string> NOT IMPLEMENTED !!!
                    "indentation" , T<string>
                    "inline_styles" , T<bool>
                    "invalid_elements" , T<string>
                    "remove_linebreaks" , T<bool>
                    "preformatted" , T<bool>
                    // "style_formats" , T<string> NOT IMPLEMENTED !!!
                    "valid_children" , T<string>
                    "valid_elements" , T<string>
                    "verify_css_classes" , T<bool>
                    "verify_html" , T<bool>
                    "removeformat_selector" , T<string>

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

                    // File lists
                    "external_link_list_url" , T<string>
                    "external_image_list_url" , T<string>
                    "external_media_list_url" , T<string>
                    "external_template_list_url" , T<string>

                    // Triggers/Patches
                    "add_form_submit_trigger" , T<bool>
                    "add_unload_trigger" , T<bool>
                    "submit_patch" , T<bool>

                    // Advanced theme
                    "theme_advanced_layout_manager" , ThemeAdvancedLayoutManager 
                    "theme_advanced_blockformats" , T<string>
                    "theme_advanced_styles" , T<string>
                    "theme_advanced_source_editor_width" , T<int>
                    "theme_advanced_source_editor_height" , T<int>
                    "theme_advanced_source_editor_wrap" , T<bool>
                    "theme_advanced_toolbar_location" , T<string>
                    "theme_advanced_toolbar_align" , T<string>
                    "theme_advanced_statusbar_location" , T<string>
                    "theme_advanced_buttons1" , T<string>
                    "theme_advanced_buttons2" , T<string>
                    "theme_advanced_buttons3" , T<string>
                    "theme_advanced_buttons4" , T<string>
                    "theme_advanced_buttons1_add" , T<string>
                    "theme_advanced_buttons2_add" , T<string>
                    "theme_advanced_buttons3_add" , T<string>
                    "theme_advanced_buttons4_add" , T<string>
                    "theme_advanced_buttons1_add_before" , T<string>
                    "theme_advanced_buttons2_add_before" , T<string>
                    "theme_advanced_buttons3_add_before" , T<string>
                    "theme_advanced_buttons4_add_before" , T<string>
                    "theme_advanced_disable" , T<string>
                    "theme_advanced_containers" , T<string>
                    "theme_advanced_containers_default_class" , T<string>
                    "theme_advanced_containers_default_align" , T<string>
                    // "theme_advanced_container_container" , T<string> NOT IMPLEMENTED !!!
                    // "theme_advanced_container_container_class" , T<string> NOT IMPLEMENTED !!!
                    // "theme_advanced_container_container_class" , T<string> NOT IMPLEMENTED !!!
                    "theme_advanced_custom_layout" , T<string>
                    "theme_advanced_link_targets" , T<string>
                    "theme_advanced_resizing" , T<bool>
                    "theme_advanced_resizing_min_width" , T<int>
                    "theme_advanced_resizing_min_height" , T<int>
                    "theme_advanced_resizing_max_widthEdit " , T<int>
                    "theme_advanced_resizing_max_height " , T<int>
                    "theme_advanced_resizing_use_cookie" , T<bool>
                    "theme_advanced_resize_horizontal" , T<bool>
                    "theme_advanced_path" , T<bool>
                    "theme_advanced_fonts" , T<string>
                    "theme_advanced_font_sizes" , T<string>
                    "theme_advanced_text_colors" , T<string>
                    "theme_advanced_background_colors" , T<string>
                    "theme_advanced_default_foreground_color" , T<string>
                    "theme_advanced_default_background_color" , T<string>
                    "theme_advanced_more_colors" , T<string>



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

                "execCommand" => T<string> * T<bool> * T<string> ^-> T<unit> 
                |> WithComment ""

            ]

    let Assembly =
        Assembly [
            Namespace "IntelliFactory.WebSharper.TinyMce" [
                TinyMCEConfigurationClass
                TinyMCEClass
                DialogTypeClass
                ModeClass
                ThemeAdvancedLayoutManagerClass
                EntityEncodingClass
            ]
        ]

