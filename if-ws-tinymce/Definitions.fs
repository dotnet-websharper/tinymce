namespace IntelliFactory.WebSharper.TinyMceExtension

module TinyMce =
    open IntelliFactory.WebSharper.InterfaceGenerator
    open IntelliFactory.WebSharper.Dom;
    open IntelliFactory.WebSharper.EcmaScript;
        
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
    let Editor = Type.New ()

    let TinyMCEConfigurationClass =
        Pattern.Config "TinyMCEConfiguration" {
            Required = []
            Optional = 
                [
                    // general
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

                    // callbacks
                    "cleanup_callback", T<string> * T<string> ^-> T<string>
                    "execcommand_callback", T<string * string * string * string * string -> bool>
                    "file_browser_callback", T<string * string * string * obj -> unit>
                    "handle_event_callback", T<Event -> bool>
                    "handle_node_change_callback", T<string * Node * int * int * bool * bool -> unit>
                    "init_instance_callback", Editor ^-> T<unit>
                    "onchange_callback", Editor ^-> T<unit>
                    "oninit", T<unit -> unit> 
                    "onpageload", T<unit -> unit> 
                    "remove_instance_callback", Editor ^-> T<unit>
                    "save_callback", T<string * string * string -> string>
                    "setup", Editor ^-> T<unit>
                    "setupcontent_callback", T<string * Node * string -> unit>
                    "urlconverter_callback", T<string * string * bool -> string>

                    // cleanup/output
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

                    // url
                    "convert_urls" , T<bool>
                    "relative_urls" , T<bool>
                    "remove_script_host" , T<bool>
                    "document_base_url" , T<string>

                    // layout
                    "body_id" , T<string>
                    "body_class" , T<string>
                    "constrain_menus" , T<bool>
                    "content_css" , T<string>
                    "popup_css" , T<string>
                    "popup_css_add" , T<string>
                    "editor_css" , T<string>
                    "width" , T<string>
                    "height" , T<string>

                    // visual aids
                    "visual" , T<bool>
                    "visual_table_class" , T<string>
                    
                    // undo/Redo
                    "custom_undo_redo" , T<bool>
                    "custom_undo_redo_levels" , T<int>
                    "custom_undo_redo_keyboard_shortcuts" , T<int>
                    "custom_undo_redo_restore_selection" , T<bool>

                    // file lists
                    "external_link_list_url" , T<string>
                    "external_image_list_url" , T<string>
                    "external_media_list_url" , T<string>
                    "external_template_list_url" , T<string>

                    // triggers/patches
                    "add_form_submit_trigger" , T<bool>
                    "add_unload_trigger" , T<bool>
                    "submit_patch" , T<bool>

                    // advanced theme
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


    let UndoManager = Type.New ()

    let UndoManagerClass = 
        Class "tinymce.UndoManager"
        |=> UndoManager
        |+> Protocol
            [
                // methods
                "add" => T<obj> ^-> T<obj>
                |> WithComment "Adds a new undo level to the undo list."

                "undo" => T<unit> ^-> T<obj>
                |> WithComment "Undoes the last changes."

                "redo" => T<unit> ^-> T<obj>
                |> WithComment "Redoes the last changes."

                "clear" => T<unit> ^-> T<unit>
                |> WithComment "Clears undo list."

                "hasUndo" => T<unit> ^-> T<bool>
                |> WithComment "Return true/false if tere are undo levels or not."

                "hasRedo" => T<unit> ^-> T<bool>
                |> WithComment "Return true/false if tere are redo levels or not."

                // events
                "onAdd" => (UndoManager * T<obj> ^-> T<unit>) ^-> T<unit>
                |> WithComment "Fires when new undo level is added to the undo manager."

                "onUndo" => (UndoManager * T<obj> ^-> T<unit>) ^-> T<unit>
                |> WithComment "Fires when the user makes an undo."

                "onRedo" => (UndoManager * T<obj> ^-> T<unit>) ^-> T<unit>
                |> WithComment "Fires when the user makes an redo."

            ]


    let Dispatcher = Type.New ()
        
    let DispatcherClass = 
        Class "tinymce.util.Dispatcher"
        |=> Dispatcher
        |+> Protocol
            [
                // methods
                "add" => (Editor ^-> T<unit>) ^-> (Editor ^-> T<unit>)
                |> WithComment "Adds an observer function."

                "add" => (Editor * T<Event> ^-> T<unit>) ^-> (Editor * T<Event> ^-> T<unit>)
                |> WithComment "Adds an observer function."

                "addToTop" => (Editor ^-> T<unit>) ^-> (Editor ^-> T<unit>)
                |> WithComment "Adds an observer function to the top of the list of observers."

                "addToTop" => (Editor * T<Event> ^-> T<unit>) ^-> (Editor * T<Event> ^-> T<unit>)
                |> WithComment "Adds an observer function to the top of the list of observers."

                "remove" => (Editor ^-> T<unit>) ^-> (Editor ^-> T<unit>)
                |> WithComment "Removes an observer function."

                "remove" => (Editor * T<Event> ^-> T<unit>) ^-> (Editor * T<Event> ^-> T<unit>)
                |> WithComment "Removes an observer function."
            ]

    
    let Selection = Type.New ()
        
    let SelectionClass = 
        Class "tinymce.dom.Selection"
        |=> Selection
        |+> Protocol
            [
                // methods
                "getContent" => T<unit> ^-> T<string> 
                |> WithComment "Returns the selected content."

                "setContent" => T<string> ^-> T<unit> 
                |> WithComment "Replaces the selection with specified content."
            ]

        
    let EditorClass = 
        Class "tinymce.Editor"
        |=> Editor
        |+> Protocol
            [
                // properties
                "id" =? T<string>
                |> WithComment "An editor id."

                "isNotDirty" =? T<bool>
                |> WithComment "Changes the editor state to no dirty."

                "dom" =? T<Node>
                |> WithComment "The editor's DOM instance."

                "selection" =? Selection 
                |> WithComment "The Selection instance for the editor."

                "undoManager" =? UndoManager 
                |> WithComment "The UndoManager instance for the editor."


                // methods
                "focus" => T<bool> ^-> T<unit> 
                |> WithComment "Focuses and activates the editor."

                "execCommand" => T<string> ^-> T<unit> 
                |> WithComment "Executes the specified command."

                "execCommand" => T<string> * T<bool> * T<string> ^-> T<unit> 
                |> WithComment "Executes the specified command."

                "show" => T<unit> ^-> T<unit> 
                |> WithComment ""

                "hide" => T<unit> ^-> T<unit> 
                |> WithComment ""

                "isHidden" => T<unit> ^-> T<bool> 
                |> WithComment "Returns true if the editor is hidden, false if it is not."

                "load" => T<unit> ^-> T<string> 
                |> WithComment "Loads the content from the editor's textarea or div into the editor instance."

                "save" => T<unit> ^-> T<string> 
                |> WithComment "Saves the content from the editor into the editor's textarea or div."

                "setContent" => T<string> ^-> T<string> 
                |> WithComment "Sets the specified content to the editor instance."

                "getContent" => T<unit> ^-> T<string> 
                |> WithComment "Gets the content from the editor instance."

                "isDirty" => T<unit> ^-> T<bool> 
                |> WithComment "Returns true/false if the user made modifications to the content or not."

                "remove" => T<unit> ^-> T<unit> 
                |> WithComment "Removes the editor from DOM and the editor collection."


                // events
                "onPreInit" =? Dispatcher 
                |> WithComment "Fires before the editor is initialized."

                "onInit" =? Dispatcher 
                |> WithComment "Fires after the editor is initialized."
                
                "onActivate" =? Dispatcher 
                |> WithComment "Fires when editor is activated."
                
                "onDeactivate" =? Dispatcher 
                |> WithComment "Fires when the editor is deactivated."
                
                "onClick" =? Dispatcher 
                |> WithComment "Fires when the editor's body is clicked."
                
                "onMouseUp" =? Dispatcher 
                |> WithComment "Fires when a mouseUp event occurs inside editor."
                
                "onMouseDown" =? Dispatcher 
                |> WithComment "Fires when a mouseDown event occurs inside editor."
                
                "onDblClick" =? Dispatcher 
                |> WithComment "Fires when the editor's body is double clicked."
                
                "onKeyDown" =? Dispatcher 
                |> WithComment "Fires when a keyDown event occurs inside editor."
                
                "onKeyUp" =? Dispatcher 
                |> WithComment "Fires when a keyUp event occurs inside editor."
                
                "onKeyPress" =? Dispatcher 
                |> WithComment "Fires when a keyPress event occurs inside editor."
                
                "onContextMenu" =? Dispatcher 
                |> WithComment "Fires when a contextMenu event occurs inside editor."
                
                "onSubmit" =? Dispatcher 
                |> WithComment "Fires when a form submit event occurs."
                
                "onReset" =? Dispatcher 
                |> WithComment "Fires when a form reset event occurs."
                
                "onPaste" =? Dispatcher 
                |> WithComment "Fires when a paste event occurs inside editor."
                
                "onLoadContent" =? Dispatcher 
                |> WithComment "Fires when the editor gets loaded with content."
                
                "onSaveContent" =? Dispatcher 
                |> WithComment "Fires when the editor content gets saved."
                
                "onChange" =? Dispatcher 
                |> WithComment "Fires when the editor content was modified."
                
                "onUndo" =? Dispatcher 
                |> WithComment "Fires when the editor content is undoed."
                
                "onRedo" =? Dispatcher 
                |> WithComment "Fires when the editor content is redoed."
                
            ]


    let TinyMCEClass =
        Class "tinyMCE"
        |=> TinyMCE
        |+> [
                // properties
                "editors" =? T<Array>
                |> WithComment "The editor collection."

                "activeEditor" =? T<Array>
                |> WithComment "Currently active editor."


                // methods
                "init" => TinyMCEConfiguration ^-> T<unit>
                |> WithComment "Initializes an editor."

                "get" => T<string> ^-> Editor
                |> WithComment "Returns a editor instance with given id."

                "get" => T<int> ^-> Editor
                |> WithComment "Returns a editor instance with given id."

                "add" => Editor ^-> Editor
                |> WithComment "Adds an editor instance to the editor collection and sets it as the active editor."
                
                "remove" => Editor ^-> Editor
                |> WithComment "Removes an editor instance from the editor collection."
                
                "execCommand" => T<string> * T<bool> * T<string> ^-> T<bool> 
                |> WithComment "Executes povided command on the active editor."
                

                // events
                "onAddEditor" => (TinyMCE * Editor ^-> T<unit>) ^-> T<unit>
                |> WithComment "Executes when an new editor instance is added to the editor collection."
                
                "onRemoveEditor" => (TinyMCE * Editor ^-> T<unit>) ^-> T<unit>
                |> WithComment "Executes when an editor instance is removed from the editor collection."
                
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
                EditorClass
                UndoManagerClass
                DispatcherClass
                SelectionClass
            ]
        ]

