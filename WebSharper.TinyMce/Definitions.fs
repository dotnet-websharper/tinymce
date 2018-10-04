// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
namespace WebSharper.TinyMceExtension

open WebSharper
open WebSharper.InterfaceGenerator

module TinyMce =
    open WebSharper.JavaScript.Dom

    let Res = (Resource "TinyMce" "https://cdnjs.cloudflare.com/ajax/libs/tinymce/3.5.8/tiny_mce.js").AssemblyWide()

    let private ConstantStrings ty l =
        List.map (fun s -> (s =? ty |> WithGetterInline ("'" + s + "'")) :> CodeModel.IClassMember) l
        |> Static


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

        
    let ToolbarLocation = Type.New ()
        
    let ToolbarLocationClass = 
        Class "ToolbarLocation"
        |=> ToolbarLocation
        |+> ConstantStrings ToolbarLocation ["top"; "bottom"; "external"]


    let ToolbarAlign = Type.New ()
        
    let ToolbarAlignClass = 
        Class "ToolbarAlign"
        |=> ToolbarAlign
        |+> ConstantStrings ToolbarAlign ["left"; "center"; "right"]


    let StatusbarLocation = Type.New ()
        
    let StatusbarLocationClass = 
        Class "StatusbarLocation"
        |=> StatusbarLocation
        |+> ConstantStrings StatusbarLocation ["top"; "bottom"; "none"]


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
                    "theme_advanced_toolbar_location" , ToolbarLocation 
                    "theme_advanced_toolbar_align" , ToolbarAlign
                    "theme_advanced_statusbar_location" , StatusbarLocation
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
        |+> Instance
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


    let Dispatcher = Type.New()

    let DispatcherClass = 
        Generic -- fun t1 t2 ->
            Class "Dispatcher"
            |=> Dispatcher
            |+> Instance
                [
                    // methods
                    "add" => (t1 ^-> T<unit>) ^-> (t1 ^-> T<unit>)
                    |> WithComment "Adds an observer function."
    
                    "add" => (t1 * t2 ^-> T<unit>) ^-> (t1 * t2 ^-> T<unit>)
                    |> WithComment "Adds an observer function."
    
                    "addToTop" => (t1 ^-> T<unit>) ^-> (t1 ^-> T<unit>)
                    |> WithComment "Adds an observer function to the top of the list of observers."
    
                    "addToTop" => (t1 * t2 ^-> T<unit>) ^-> (t1 * t2 ^-> T<unit>)
                    |> WithComment "Adds an observer function to the top of the list of observers."
    
                    "remove" => (t1 ^-> T<unit>) ^-> (t1 ^-> T<unit>)
                    |> WithComment "Removes an observer function."
    
                    "remove" => (t1 * t2 ^-> T<unit>) ^-> (t1 * t2 ^-> T<unit>)
                    |> WithComment "Removes an observer function."
                ]


    (*
    let Dispatcher = Type.New ()
        
    let DispatcherClass = 
        Class "tinymce.util.Dispatcher"
        |=> Dispatcher
        |+> Instance
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
            *)

    
    let Selection = Type.New ()
        
    let SelectionClass = 
        Class "tinymce.dom.Selection"
        |=> Selection
        |+> Instance
            [
                // methods
                "getContent" => T<unit> ^-> T<string> 
                |> WithComment "Returns the selected content."

                "setContent" => T<string> ^-> T<unit> 
                |> WithComment "Replaces the selection with specified content."
            ]
                        

    let ControlConfiguration = Type.New ()

    let ControlConfigurationClass =
        Pattern.Config "ControlConfiguration" {
            Required = []
            Optional = 
                [
                    "title", T<string>
                    //"class", T<string>
                    "image", T<string>
                    "icons", T<bool>
                    "onselect", T<string> ^-> T<unit>
                    "onclick", T<unit> ^-> T<unit>
                ]
        }
        |=> ControlConfiguration
        |+> Instance [
                "class" =@ T<string>
                |> WithSourceName "Class"
            ]


    let Control = Type.New ()
        
    let ControlClass = 
        Class "tinymce.ui.Control"
        |=> Control
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Constructs a new control instance."

                Constructor (T<string> * T<obj>)
                |> WithComment "Constructs a new control instance."
            ]
        |+> Instance
            [
                "setDisabled" => T<bool> ^-> T<unit> 
                |> WithComment "Sets the disabled state for the control. This will add CSS classes to the element that contains the control. So that it can be disabled visually."

                "isDisabled" => T<unit> ^-> T<bool> 
                |> WithComment "Returns true/false if the control is disabled or not. This is a method since you can then choose to check some class or some internal bool state in subclasses."

                "setActive" => T<bool> ^-> T<unit> 
                |> WithComment "Sets the activated state for the control. This will add CSS classes to the element that contains the control. So that it can be activated visually."

                "isActive" => T<unit> ^-> T<bool> 
                |> WithComment "Returns true/false if the control is disabled or not. This is a method since you can then choose to check some class or some internal bool state in subclasses."

                "setState" => T<string> * T<bool> ^-> T<unit> 
                |> WithComment "Sets the specified class state for the control."

                "isRendered" => T<unit> ^-> T<bool> 
                |> WithComment "Returns true/false if the control has been rendered or not."

                "renderHTML" => T<unit> ^-> T<string> 
                |> WithComment "Renders the control as a HTML string. This method is much faster than using the DOM and when creating a whole toolbar with buttons it does make a lot of difference."

                "renderTo" => T<Element> ^-> T<unit> 
                |> WithComment "Renders the control to the specified container element."

                "remove" => T<unit> ^-> T<unit> 
                |> WithComment "Removes the control. This means it will be removed from the DOM and any events tied to it will also be removed."

                "destroy" => T<unit> ^-> T<unit> 
                |> WithComment "Destroys the control will free any memory by removing event listeners etc."

                // events
                "postRender" => Dispatcher.[T<obj>, Control]
                |> WithComment "Post render event. This will be executed after the control has been rendered and can be used to set states, add events to the control etc. It's recommended for subclasses of the control to call this method by using this.parent()."
            ]
            

    let Separator = Type.New ()
        
    let SeparatorClass = 
        Class "tinymce.ui.Separator"
        |=> Inherits ControlClass 
        |=> Separator
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Separator constructor."

                Constructor (T<string> * T<obj>)
                |> WithComment "Separator constructor."
            ]
        |+> Instance
            [
                "renderHTML" => T<unit> ^-> T<string> 
                |> WithComment "Renders the separator as a HTML string. This method is much faster than using the DOM and when creating a whole toolbar with buttons it does make a lot of difference."
            ]



    let MenuButton = Type.New ()
    let DropMenu = Type.New ()
        
    let MenuButtonClass = 
        Class "tinymce.ui.MenuButton"
        |=> Inherits ControlClass 
        |=> MenuButton
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Constructs a new split button control instance."

                Constructor (T<string> * T<obj>)
                |> WithComment "Constructs a new split button control instance."

                Constructor (T<string> * T<obj> * Editor)
                |> WithComment "Constructs a new split button control instance."
            ]
        |+> Instance
            [

                // methods
                "hideMenu" => T<Event> ^-> T<unit> 
                |> WithComment "Hides the menu. The optional event parameter is used to check where the event occured so it doesn't close them menu if it was a event inside the menu."

                "postRender" => T<unit> ^-> T<unit> 
                |> WithComment "Post render handler. This function will be called after the UI has been rendered so that events can be added."

                "renderMenu" => T<unit> ^-> T<unit> 
                |> WithComment "Renders the menu to the DOM."
                
                "showMenu" => T<unit> ^-> T<unit> 
                |> WithComment "Shows the menu."

                // events
                "onRender" =? Dispatcher.[T<obj>, MenuButton]
                |> WithComment ""

                "onRenderMenu" =? Dispatcher.[T<obj>, DropMenu]
                |> WithComment "Fires when the menu is rendered."
            ]
            

    let MenuItem = Type.New ()
        
    let MenuItemClass = 
        Class "tinymce.ui.MenuItem"
        |=> Inherits ControlClass 
        |=> MenuItem
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Constructs a new button control instance."

                Constructor (T<string> * T<obj>)
                |> WithComment "Constructs a new button control instance."
            ]
        |+> Instance
            [

                // methods
                "isSelected" => T<unit> ^-> T<bool> 
                |> WithComment "Returns true/false if the control is selected or not."

                "postRender" => T<unit> ^-> T<unit> 
                |> WithComment "Post render handler. This function will be called after the UI has been rendered so that events can be added."
                
                "setSelected" => T<bool> ^-> T<unit> 
                |> WithComment "Sets the selected state for the control. This will add CSS classes to the element that contains the control. So that it can be selected visually."
            ]
            

    let Menu = Type.New ()
        
    let MenuClass = 
        Class "tinymce.ui.Menu"
        |=> Inherits MenuItem 
        |=> Menu
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Constructs a new button control instance."

                Constructor (T<string> * T<obj>)
                |> WithComment "Constructs a new button control instance."
            ]
        |+> Instance
            [

                // methods
                "add" => Control ^-> Control 
                |> WithComment "Adds a new menu, menu item or sub classes of them to the drop menu."

                "add" => ControlConfiguration ^-> Control 
                |> WithComment "Adds a new menu, menu item or sub classes of them to the drop menu."

                "addMenu" => T<obj> ^-> Menu 
                |> WithComment "Adds a sub menu to the menu."

                "addSeparator" => T<obj> ^-> MenuItem 
                |> WithComment "Adds a menu separator between the menu items."

                "collapse" => T<unit> ^-> T<unit> 
                |> WithComment "Collapses the menu, this will hide the menu and all menu items."

                "collapse" => T<bool> ^-> T<unit> 
                |> WithComment "Collapses the menu, this will hide the menu and all menu items."

                "createMenu" => T<obj> ^-> Menu 
                |> WithComment "Created a new sub menu for the menu control."

                "expand" => T<bool> ^-> T<unit> 
                |> WithComment "Expands the menu, this will show them menu and all menu items."

                "hasMenus" => T<unit> ^-> T<bool> 
                |> WithComment "Returns true/false if the menu has sub menus or not."

                "isCollapsed" => T<unit> ^-> T<bool> 
                |> WithComment "Returns true/false if the menu has been collapsed or not."

                "remove" => Control ^-> Control 
                |> WithComment "Removes a specific sub menu or menu item from the menu."

                "removeAll" => T<unit> ^-> T<unit> 
                |> WithComment "Removes all menu items and sub menu items from the menu."
            ]
            

        
    let DropMenuClass = 
        Class "tinymce.ui.DropMenu"
        |=> Inherits Menu
        |=> DropMenu
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Constructs a new drop menu control instance."

                Constructor (T<string> * T<obj>)
                |> WithComment "Constructs a new drop menu control instance."
            ]
        |+> Instance
            [

                // methods
                "add" => Control ^-> Control 
                |> WithComment "Adds a new menu, menu item or sub classes of them to the drop menu."

                "add" => ControlConfiguration ^-> Control 
                |> WithComment "Adds a new menu, menu item or sub classes of them to the drop menu."

                "collapse" => T<unit> ^-> T<unit> 
                |> WithComment "Collapses the menu, this will hide the menu and all menu items."

                "collapse" => T<bool> ^-> T<unit> 
                |> WithComment "Collapses the menu, this will hide the menu and all menu items."

                "createMenu" => T<obj> ^-> DropMenu 
                |> WithComment "Created a new sub menu for the drop menu control."

                "destroy" => T<unit> ^-> T<unit> 
                |> WithComment "Destroys the menu. This will remove the menu from the DOM and any events added to it etc."

                "hideMenu" => T<unit> ^-> T<unit> 
                |> WithComment "Hides the displayed menu."

                "remove" => Control ^-> Control 
                |> WithComment "Removes a specific sub menu or menu item from the drop menu."

                "renderNode" => T<unit> ^-> T<Element> 
                |> WithComment "Renders the specified menu node to the dom."

                "showMenu" => T<int> * T<int> ^-> T<unit> 
                |> WithComment "Displays the menu at the specified cordinate."

                "showMenu" => T<int> * T<int> * T<int> ^-> T<unit> 
                |> WithComment "Displays the menu at the specified cordinate."

                "update" => T<unit> ^-> T<unit> 
                |> WithComment "Repaints the menu after new items have been added dynamically."

                // events
                "onRender" =? Dispatcher.[T<obj>, DropMenu]
                |> WithComment ""
            ]


    let Button = Type.New ()
        
    let ButtonClass = 
        Class "tinymce.ui.Button"
        |=> Inherits ControlClass 
        |=> Button
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Constructs a new button control instance."

                Constructor (T<string> * T<obj>)
                |> WithComment "Constructs a new button control instance."

                Constructor (T<string> * T<obj> * Editor)
                |> WithComment "Constructs a new button control instance."
            ]
        |+> Instance
            [
                "postRender" => T<unit> ^-> T<unit> 
                |> WithComment "Post render handler. This function will be called after the UI has been rendered so that events can be added."

                "renderHTML" => T<unit> ^-> T<string> 
                |> WithComment "Renders the button as a HTML string. This method is much faster than using the DOM and when creating a whole toolbar with buttons it does make a lot of difference."
            ]
    

    let SplitButton = Type.New ()
        
    let SplitButtonClass = 
        Class "tinymce.ui.SplitButton"
        |=> Inherits ButtonClass
        |=> SplitButton
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Constructs a new split button control instance."

                Constructor (T<string> * T<obj>)
                |> WithComment "Constructs a new split button control instance."

                Constructor (T<string> * T<obj> * Editor)
                |> WithComment "Constructs a new split button control instance."
            ]
        |+> Instance
            [
                // methods
                "postRender" => T<unit> ^-> T<unit> 
                |> WithComment "Post render handler. This function will be called after the UI has been rendered so that events can be added."

                "renderHTML" => T<unit> ^-> T<string> 
                |> WithComment "Renders the split button as a HTML string. This method is much faster than using the DOM and when creating a whole toolbar with buttons it does make a lot of difference."

                "onRenderMenu" =? Dispatcher.[T<obj>, DropMenu]
                |> WithComment "Fires when the menu is rendered."
            ]


    let ColorSplitButton = Type.New ()
        
    let ColorSplitButtonClass = 
        Class "tinymce.ui.ColorSplitButton"
        |=> Inherits SplitButton
        |=> ColorSplitButton
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Constructs a new button control instance."

                Constructor (T<string> * Editor)
                |> WithComment "Constructs a new button control instance."

                Constructor (T<string> * T<obj> * Editor)
                |> WithComment "Constructs a new button control instance."
            ]
        |+> Instance
            [
                // properties
                "settings" =? T<obj>
                |> WithComment "Settings object."

                "value" =? T<string>
                |> WithComment "Current color value."

                
                // methods
                "destroy" => T<unit> ^-> T<unit> 
                |> WithComment "Destroys the control. This means it will be removed from the DOM and any events tied to it will also be removed."

                "displayColor" => T<string> ^-> T<string> 
                |> WithComment "Change the currently selected color for the control."

                "hideMenu" => T<Event> ^-> T<unit> 
                |> WithComment "Hides the color menu. The optional event parameter is used to check where the event occured so it doesn't close them menu if it was a event inside the menu."

                "postRender" => T<unit> ^-> T<unit> 
                |> WithComment "Post render event. This will be executed after the control has been rendered and can be used to set states, add events to the control etc. It's recommended for subclasses of the control to call this method by using this.parent()."

                "renderMenu" => T<unit> ^-> T<unit> 
                |> WithComment "Renders the menu to the DOM."

                "setColor" => T<string> ^-> T<unit> 
                |> WithComment "Sets the current color for the control and hides the menu if it should be visible."

                "showMenu" => T<unit> ^-> T<unit> 
                |> WithComment "Shows the color menu. The color menu is a layer places under the button and displays a table of colors for the user to pick from."


                // events
                "onHideMenu" => T<unit> ^-> T<unit> 
                |> WithComment "Fires when the menu is hidden."

                "onShowMenu" => T<unit> ^-> T<unit> 
                |> WithComment "Fires when the menu is shown."

                "onRenderMenu" =? Dispatcher.[T<obj>, DropMenu]
                |> WithComment "Fires when the menu is rendered."
            ]


    let Container = Type.New ()
        
    let ContainerClass = 
        Class "tinymce.ui.Container"
        |=> Inherits ControlClass 
        |=> Container
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Base contrustor a new container control instance."

                Constructor (T<string> * T<obj>)
                |> WithComment "Base contrustor a new container control instance."

            ]
        |+> Instance
            [
                // properties
                "controls" =? Type.ArrayOf Control
                |> WithComment "Settings object."


                // methods
                "add" => Control ^-> Control 
                |> WithComment "Adds a control to the collection of controls for the container."

                "get" => T<string> ^-> Control 
                |> WithComment "Returns a control by id from the containers collection."
            ]


    let Toolbar = Type.New ()
        
    let ToolbarClass = 
        Class "tinymce.ui.Toolbar"
        |=> Inherits ContainerClass 
        |=> Toolbar
        |+> Static [
            ]
        |+> Instance
            [
                // properties
                "controls" =? Type.ArrayOf Control
                |> WithComment "Array of controls added to the container."


                // methods
                "renderHTML" => T<unit> ^-> T<string> 
                |> WithComment "Renders the toolbar as a HTML string."
            ]

    let ToolbarGroup = Type.New ()
        
    let ToolbarGroupClass = 
        Class "tinymce.ui.ToolbarGroup"
        |=> Inherits ContainerClass 
        |=> ToolbarGroup
        |+> Static [
            ]
        |+> Instance
            [
                // properties
                "controls" =? Type.ArrayOf Control
                |> WithComment "Settings object."


                // methods
                "renderHTML" => T<unit> ^-> T<string> 
                |> WithComment "Renders the toolbar group as a HTML string."
            ]
            

    let ListBox = Type.New ()
        
    let ListBoxClass = 
        Class "tinymce.ui.ListBox"
        |=> Inherits ControlClass 
        |=> ListBox
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Constructs a new listbox control instance."

                Constructor (T<string> * T<obj>)
                |> WithComment "Constructs a new listbox control instance."

                Constructor (T<string> * T<obj> * Editor)
                |> WithComment "Constructs a new listbox control instance."
            ]
        |+> Instance
            [
                // properties
                "items" =? Type.ArrayOf Control
                |> WithComment "Array of ListBox items."


                // methods
                "add" => T<string> * T<string> ^-> T<unit> 
                |> WithComment "Adds a option item to the list box."

                "add" => T<string> * T<string> * T<obj> ^-> T<unit> 
                |> WithComment "Adds a option item to the list box."

                "destroy" => T<unit> ^-> T<unit> 
                |> WithComment "Destroys the ListBox i.e. clear memory and events."

                "getLength" => T<unit> ^-> T<int> 
                |> WithComment "Returns the number of items inside the list box."

                "hideMenu" => T<unit> ^-> T<unit> 
                |> WithComment "Hides the drop menu."

                "postRender" => T<unit> ^-> T<unit> 
                |> WithComment "Post render event. This will be executed after the control has been rendered and can be used to set states, add events to the control etc. It's recommended for subclasses of the control to call this method by using this.parent()."

                "renderHTML" => T<unit> ^-> T<string> 
                |> WithComment "Renders the list box as a HTML string. This method is much faster than using the DOM and when creating a whole toolbar with buttons it does make a lot of difference."

                "renderMenu" => T<unit> ^-> T<unit> 
                |> WithComment "Renders the menu to the DOM."
                
                "select" => T<string> ^-> T<unit> 
                |> WithComment "Selects a item/option by value. This will both add a visual selection to the item and change the title of the control to the title of the option."
                
                "select" => T<obj -> bool> ^-> T<unit> 
                |> WithComment "Selects a item/option by value. This will both add a visual selection to the item and change the title of the control to the title of the option."
                
                "selectByIndex" => T<string> ^-> T<unit> 
                |> WithComment "Selects a item/option by index. This will both add a visual selection to the item and change the title of the control to the title of the option."
                
                "showMenu" => T<unit> ^-> T<unit> 
                |> WithComment "Displays the drop menu with all items."

                // events
                "onAdd" =? Dispatcher.[T<obj>, T<obj>] 
                |> WithComment "Fires when a new item is added."

                "onChange" =? Dispatcher.[T<obj>, T<obj>] 
                |> WithComment "Fires when the selection has been changed."

                "onPostRender" =? Dispatcher.[T<obj>, T<obj>]
                |> WithComment "Fires after the element has been rendered to DOM."

                "onRenderMenu" =? Dispatcher.[T<obj>, DropMenu]
                |> WithComment "Fires when the menu gets rendered."
            ]
            

            

    let NativeListBox = Type.New ()
        
    let NativeListBoxClass = 
        Class "tinymce.ui.NativeListBox"
        |=> Inherits ListBoxClass 
        |=> NativeListBox
        |+> Static [
                // constructors
                Constructor (T<string>)
                |> WithComment "Constructs a new button control instance."

                Constructor (T<string> * T<obj>)
                |> WithComment "Constructs a new button control instance."
            ]
        |+> Instance
            [
                // methods
                "add" => T<string> * T<string> ^-> T<unit> 
                |> WithComment "Adds a option item to the list box."

                "add" => T<string> * T<string> * T<obj> ^-> T<unit> 
                |> WithComment "Adds a option item to the list box."

                "getLength" => T<unit> ^-> T<int> 
                |> WithComment "Returns the number of items inside the list box."

                "isDisabled" => T<unit> ^-> T<bool> 
                |> WithComment "Returns true/false if the control is disabled or not. This is a method since you can then choose to check some class or some internal bool state in subclasses."

                "postRender" => T<unit> ^-> T<unit> 
                |> WithComment "Post render handler. This function will be called after the UI has been rendered so that events can be added."

                "renderHTML" => T<unit> ^-> T<string> 
                |> WithComment "Renders the list box as a HTML string. This method is much faster than using the DOM and when creating a whole toolbar with buttons it does make a lot of difference."

                "select" => T<string> ^-> T<unit> 
                |> WithComment "Selects a item/option by value. This will both add a visual selection to the item and change the title of the control to the title of the option."
                
                "select" => T<obj -> bool> ^-> T<unit> 
                |> WithComment "Selects a item/option by value. This will both add a visual selection to the item and change the title of the control to the title of the option."
                
                "selectByIndex" => T<string> ^-> T<unit> 
                |> WithComment "Selects a item/option by index. This will both add a visual selection to the item and change the title of the control to the title of the option."

                "setDisabled" => T<bool> ^-> T<unit> 
                |> WithComment "Sets the disabled state for the control. This will add CSS classes to the element that contains the control. So that it can be disabled visually."
            ]


    let PluginManager = Type.New ()
        
    let PluginManagerClass = 
        Class "tinymce.PluginManager"
        |=> PluginManager
        |+> Static [
                // methods
                "add" => T<string> * T<obj> ^-> T<unit> 
                |> WithComment "Register plugin with a short name."
            ]
        |+> Instance
            [
            ]


    let WindowManager = Type.New ()
        
    let WindowManagerClass = 
        Class "tinymce.WindowManager"
        |=> WindowManager
        |+> Static [
                // constructors
                Constructor (Editor)
                |> WithComment "Constructs a new window manager instance."
            ]
        |+> Instance
            [
                // methods
                "alert" => T<string> ^-> T<unit> 
                |> WithComment "Creates a alert dialog. Please don't use the blocking behavior of this native version use the callback method instead then it can be extended."

                "alert" => T<string> * T<unit -> unit> ^-> T<unit> 
                |> WithComment "Creates a alert dialog. Please don't use the blocking behavior of this native version use the callback method instead then it can be extended."

                "alert" => T<string> * T<unit -> unit> * T<obj> ^-> T<unit> 
                |> WithComment "Creates a alert dialog. Please don't use the blocking behavior of this native version use the callback method instead then it can be extended."

            ]


    let ControlManager = Type.New ()
        
    let ControlManagerClass = 
        Class "tinymce.ControlManager"
        |=> ControlManager
        |+> Static [
                // constructors
                Constructor (Editor)
                |> WithComment "Constructs a new control manager instance."

                Constructor (Editor * T<obj>)
                |> WithComment "Constructs a new control manager instance."

            ]
        |+> Instance
            [
                // methods
                "get" => T<string> ^-> Control 
                |> WithComment "Returns a control by id or undefined it it wasn't found."

                "setActive" => T<string> * T<bool> ^-> Control 
                |> WithComment "Sets the active state of a control by id."

                "setDisabled" => T<string> * T<bool> ^-> Control 
                |> WithComment "Sets the dsiabled state of a control by id."

                "add" => Control ^-> Control 
                |> WithComment "Adds a control to the control collection inside the manager."

                "createControl" => T<string> ^-> Control 
                |> WithComment "Creates a control by name, when a control is created it will automatically add it to the control collection. It first ask all plugins for the specified control if the plugins didn't return a control then the default behavior will be used."

                "createDropMenu" => T<string> ^-> DropMenu 
                |> WithComment "Creates a drop menu control instance by id."

                "createDropMenu" => T<string> * ControlConfiguration ^-> DropMenu 
                |> WithComment "Creates a drop menu control instance by id."

                "createDropMenu" => T<string> * ControlConfiguration * T<obj> ^-> DropMenu 
                |> WithComment "Creates a drop menu control instance by id."

                "createListBox" => T<string> ^-> ListBox 
                |> WithComment "Creates a list box control instance by id. A list box is either a native select element or a DOM/JS based list box control. This depends on the use_native_selects settings state."

                "createListBox" => T<string> * ControlConfiguration ^-> ListBox 
                |> WithComment "Creates a list box control instance by id. A list box is either a native select element or a DOM/JS based list box control. This depends on the use_native_selects settings state."

                "createListBox" => T<string> * ControlConfiguration * T<obj> ^-> ListBox 
                |> WithComment "Creates a list box control instance by id. A list box is either a native select element or a DOM/JS based list box control. This depends on the use_native_selects settings state."

                "createButton" => T<string> ^-> Button 
                |> WithComment "Creates a button control instance by id."

                "createButton" => T<string> * ControlConfiguration^-> Button 
                |> WithComment "Creates a button control instance by id."

                "createButton" => T<string> * ControlConfiguration * T<obj> ^-> Button 
                |> WithComment "Creates a button control instance by id."

                "createMenuButton" => T<string> ^-> MenuButton 
                |> WithComment "Creates a menu button control instance by id."

                "createMenuButton" => T<string> * ControlConfiguration ^-> MenuButton 
                |> WithComment "Creates a menu button control instance by id."

                "createMenuButton" => T<string> * ControlConfiguration * T<obj> ^-> MenuButton 
                |> WithComment "Creates a menu button control instance by id."

                "createSplitButton" => T<string> ^-> SplitButton 
                |> WithComment "Creates a split button control instance by id."

                "createSplitButton" => T<string> * ControlConfiguration ^-> SplitButton 
                |> WithComment "Creates a split button control instance by id."

                "createSplitButton" => T<string> * ControlConfiguration * T<obj> ^-> SplitButton 
                |> WithComment "Creates a split button control instance by id."

                "createColorSplitButton" => T<string> ^-> ColorSplitButton 
                |> WithComment "Creates a color split button control instance by id."

                "createColorSplitButton" => T<string> * ControlConfiguration ^-> ColorSplitButton 
                |> WithComment "Creates a color split button control instance by id."

                "createColorSplitButton" => T<string> * ControlConfiguration * T<obj> ^-> ColorSplitButton 
                |> WithComment "Creates a color split button control instance by id."

                "createToolbar" => T<string> ^-> Toolbar 
                |> WithComment "Creates a toolbar container control instance by id."

                "createToolbar" => T<string> * ControlConfiguration ^-> Toolbar 
                |> WithComment "Creates a toolbar container control instance by id."

                "createToolbar" => T<string> * ControlConfiguration * T<obj> ^-> Toolbar 
                |> WithComment "Creates a toolbar container control instance by id."

                "createSeparator" => T<unit> ^-> Separator 
                |> WithComment "Creates a separator control instance."

                "createSeparator" => T<obj> ^-> Separator 
                |> WithComment "Creates a separator control instance."

                "setControlType" => T<string> ^-> Separator 
                |> WithComment "Overrides a specific control type with a custom class."

                "destroy" => T<string> ^-> Control 
                |> WithComment "Destroy."
            ]


    let Plugin = Type.New ()

    let PluginClass =
        Pattern.Config "Plugin" {
            Required = []
            Optional = 
                [
                    "createControl", T<string> * ControlManager ^-> Control
                    "getInfo", T<unit> ^-> T<obj>
                    "init", Editor * T<string> ^-> T<unit> 
                ]
        }
        |=> Plugin

        
    let EditorClass = 
        Class "tinymce.Editor"
        |=> Editor
        |+> Instance
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

                "windowManager" =? WindowManager 
                |> WithComment "Window manager reference, use this to open new windows and dialogs."


                // methods
                "addButton" => T<string> * T<obj> ^-> T<unit> 
                |> WithComment "Adds a button that later gets created by the ControlManager. This is a shorter and easier method of adding buttons without the need to deal with the ControlManager directly. But it's also less powerfull if you need more control use the ControlManagers factory methods instead."

                "addCommand" => T<string> * T<unit -> unit> ^-> T<unit> 
                |> WithComment "Adds a custom command to the editor, you can also override existing commands with this method. The command that you add can be executed with execCommand."

                "addCommand" => T<string> * T<unit -> unit> * T<obj> ^-> T<unit> 
                |> WithComment "Adds a custom command to the editor, you can also override existing commands with this method. The command that you add can be executed with execCommand."

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
                "onPreInit" =? Dispatcher.[Editor, T<Event>] 
                |> WithComment "Fires before the editor is initialized."

                "onInit" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires after the editor is initialized."
                
                "onActivate" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when editor is activated."
                
                "onDeactivate" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when the editor is deactivated."
                
                "onClick" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when the editor's body is clicked."
                
                "onMouseUp" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when a mouseUp event occurs inside editor."
                
                "onMouseDown" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when a mouseDown event occurs inside editor."
                
                "onDblClick" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when the editor's body is double clicked."
                
                "onKeyDown" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when a keyDown event occurs inside editor."
                
                "onKeyUp" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when a keyUp event occurs inside editor."
                
                "onKeyPress" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when a keyPress event occurs inside editor."
                
                "onContextMenu" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when a contextMenu event occurs inside editor."
                
                "onSubmit" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when a form submit event occurs."
                
                "onReset" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when a form reset event occurs."
                
                "onPaste" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when a paste event occurs inside editor."
                
                "onLoadContent" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when the editor gets loaded with content."
                
                "onSaveContent" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when the editor content gets saved."
                
                "onChange" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when the editor content was modified."
                
                "onUndo" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when the editor content is undoed."
                
                "onRedo" =? Dispatcher.[Editor, T<Event>]
                |> WithComment "Fires when the editor content is redoed."
                
            ]


    let TinyMCEClass =
        Class "tinyMCE"
        |=> TinyMCE
        |+> Static [
                // properties
                "editors" =? Type.ArrayOf Editor 
                |> WithComment "The editor collection."

                "activeEditor" =? Editor 
                |> WithComment "Currently active editor."

                "PluginManager" =? PluginManager 
                |> WithComment "PluginManager instance."


                // methods
                "create" => T<string> * T<obj> ^-> T<unit>
                |> WithComment "Creates a class, subclass or static singleton."

                "create" => T<string> * T<obj> * T<obj> ^-> T<unit>
                |> WithComment "Creates a class, subclass or static singleton."

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
            Namespace "WebSharper.TinyMce.Resources" [
                Res
            ]
            Namespace "WebSharper.TinyMce" [
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
                ToolbarLocationClass
                ToolbarAlignClass
                StatusbarLocationClass
                ControlConfigurationClass
                ControlManagerClass
                PluginManagerClass
                WindowManagerClass
                ButtonClass
                ColorSplitButtonClass
                ContainerClass
                ControlClass
                DropMenuClass
                ListBoxClass
                MenuClass
                MenuButtonClass
                MenuItemClass
                NativeListBoxClass
                SeparatorClass
                SplitButtonClass
                ToolbarClass
                ToolbarGroupClass
                PluginClass
            ]
        ]



[<Sealed>]
type TinyMceExtension() =
    interface IExtension with
        member x.Assembly = TinyMce.Assembly

[<assembly: Extension(typeof<TinyMceExtension>)>]
do ()
