// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2026 IntelliFactory
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
namespace WebSharper.TinyMce

// WIG member lists return the class they were added to; at statement position
// the value is intentionally discarded (one warning per typed class otherwise).
#nowarn "20"

open WebSharper
open WebSharper.JavaScript
open WebSharper.JavaScript.Dom
open WebSharper.InterfaceGenerator

/// The upstream TinyMCE line a binding assembly is generated for.
///
/// <ul>
/// <li><c>V6</c> is the last MIT-licensed line (6.8.x).</li>
/// <li><c>V8</c> is the current line (8.10.x), licensed under
/// GPL-2.0-or-later or a commercial Tiny Technologies license.</li>
/// </ul>
/// The two lines share almost the whole API; only the options and members that
/// actually differ are generated conditionally.
type TinyMceVersion =
    | V6
    | V8

/// Generates the TinyMCE WIG assembly definition.
///
/// The definition is shared by the <c>WebSharper.TinyMce</c> (8.x) and
/// <c>WebSharper.TinyMce6</c> (6.x) binding assemblies; <paramref name="version"/>
/// selects the upstream line and <paramref name="ns"/> is the .NET namespace the
/// generated types live in (so the two assemblies can be referenced side by side).
module Definition =

    let getAssembly (version: TinyMceVersion) (ns: string) =

        let isV8 = version = V8

        // ------------------------------------------------------------------
        // Closed sets of string option values
        // ------------------------------------------------------------------

        let EntityEncoding =
            Pattern.EnumStrings "EntityEncoding" [ "named"; "numeric"; "raw" ]

        let Directionality =
            Pattern.EnumStrings "Directionality" [ "ltr"; "rtl" ]

        let ElementFormat =
            Pattern.EnumStrings "ElementFormat" [ "xhtml"; "html" ]

        let ToolbarLocation =
            Pattern.EnumStrings "ToolbarLocation" [ "auto"; "top"; "bottom" ]

        let ToolbarMode =
            Pattern.EnumStrings "ToolbarMode" [ "floating"; "sliding"; "scrolling"; "wrap" ]

        let NewlineBehavior =
            Pattern.EnumStrings "NewlineBehavior" [ "block"; "linebreak"; "invert"; "default" ]

        let UIMode =
            Pattern.EnumStrings "UIMode" [ "combined"; "split" ]

        let SchemaType =
            Pattern.EnumInlines "SchemaType" [
                "Html4", "'html4'"
                "Html5", "'html5'"
                "Html5Strict", "'html5-strict'"
            ]

        let NotificationType =
            Pattern.EnumStrings "NotificationType" [ "info"; "warning"; "error"; "success" ]

        let ContentFormat =
            Pattern.EnumStrings "ContentFormat" [ "html"; "text"; "tree"; "raw" ]

        let NotificationProgress =
            Class "tinymce.ui.NotificationProgress"

        // ------------------------------------------------------------------
        // Forward declarations
        //
        // The editor and its collaborators refer to each other, so the classes
        // are created up front and their members are attached further down,
        // once every type they mention exists.
        // ------------------------------------------------------------------

        let EditorClass = Class "tinymce.Editor"
        let TinyMCEClass = Class "tinymce" |> WithSourceName "TinyMCE"
        let SelectionClass = Class "tinymce.dom.Selection"
        let UndoManagerClass = Class "tinymce.UndoManager"
        let UndoLevelClass = AbstractClass "tinymce.UndoLevel"
        let BookmarkClass = AbstractClass "tinymce.Bookmark"
        let FormatterClass = Class "tinymce.Formatter"
        let CommandsClass = Class "tinymce.EditorCommands"
        let ModeClass = Class "tinymce.EditorMode"
        let OptionsClass = Class "tinymce.util.Options"
        let UiClass = Class "tinymce.EditorUi"
        let RegistryClass = Class "tinymce.ui.Registry"
        let DomUtilsClass = Class "tinymce.dom.DOMUtils"
        let ShortcutsClass = Class "tinymce.Shortcuts"
        let PluginClass = Class "tinymce.Plugin"
        let PluginManagerClass = Class "tinymce.PluginManager"
        let ScriptLoaderClass = Class "tinymce.ScriptLoader"
        let BlobInfoClass = Class "tinymce.html.BlobInfo"
        let UploadResultClass = Class "tinymce.UploadResult"
        let NotificationManagerClass = Class "tinymce.NotificationManager"
        let NotificationApiClass = Class "tinymce.NotificationApi"
        let WindowManagerClass = Class "tinymce.WindowManager"
        let EventClass = Class "tinymce.EditorEvent"
        let ChangeEventClass = Class "tinymce.ChangeEvent"
        let NodeChangeEventClass = Class "tinymce.NodeChangeEvent"
        let ContentEventClass = Class "tinymce.ContentEvent"
        let SetContentEventClass = Class "tinymce.SetContentEvent"
        let ExecCommandEventClass = Class "tinymce.ExecCommandEvent"
        let FormatEventClass = Class "tinymce.FormatEvent"
        let UndoRedoEventClass = Class "tinymce.UndoRedoEvent"
        let SwitchModeEventClass = Class "tinymce.SwitchModeEvent"

        // Toolbar and menu building blocks (`editor.ui.registry`)
        let ToolbarButtonApiClass = Class "tinymce.ui.ToolbarButtonInstanceApi"
        let ToolbarButtonSpecClass = Class "tinymce.ui.ToolbarButtonSpec"
        let ToolbarToggleButtonApiClass = Class "tinymce.ui.ToolbarToggleButtonInstanceApi"
        let ToolbarToggleButtonSpecClass = Class "tinymce.ui.ToolbarToggleButtonSpec"
        let MenuButtonApiClass = Class "tinymce.ui.ToolbarMenuButtonInstanceApi"
        let MenuButtonSpecClass = Class "tinymce.ui.ToolbarMenuButtonSpec"
        let SplitButtonApiClass = Class "tinymce.ui.ToolbarSplitButtonInstanceApi"
        let SplitButtonSpecClass = Class "tinymce.ui.ToolbarSplitButtonSpec"
        let GroupToolbarButtonSpecClass = Class "tinymce.ui.GroupToolbarButtonSpec"
        let MenuItemApiClass = Class "tinymce.ui.MenuItemInstanceApi"
        let MenuItemSpecClass = Class "tinymce.ui.MenuItemSpec"
        let ToggleMenuItemApiClass = Class "tinymce.ui.ToggleMenuItemInstanceApi"
        let ToggleMenuItemSpecClass = Class "tinymce.ui.ToggleMenuItemSpec"
        let NestedMenuItemApiClass = Class "tinymce.ui.NestedMenuItemInstanceApi"
        let NestedMenuItemSpecClass = Class "tinymce.ui.NestedMenuItemSpec"
        let ContextMenuApiClass = Class "tinymce.ui.ContextMenuApi"
        let SidebarSpecClass = Class "tinymce.ui.SidebarSpec"
        let SidebarApiClass = Class "tinymce.ui.SidebarInstanceApi"

        // Small value types
        let FormatVarsClass = AbstractClass "tinymce.FormatVars"
        let FormatChangeDataClass = Class "tinymce.FormatChangeData"
        let FormatChangedHandleClass = Class "tinymce.FormatChangedHandle"
        let GetContentArgsClass = Class "tinymce.GetContentArgs"
        let UserClass = Class "tinymce.User"

        // ------------------------------------------------------------------
        // Events
        // ------------------------------------------------------------------

        // `tinymce.EditorEvent<T>` is `T & { type, target, preventDefault, … }`.
        // WIG has no intersection types, so the normalized part is modelled on
        // the base class and the event-specific payloads are subclasses.
        EventClass
        |+> Instance [
            "type" =? T<string>
            |> WithComment "The name of the event, for example \"change\" or \"NodeChange\"."

            "target" =? T<obj>
            |> WithComment "The event target: the editor for editor events, a DOM element for native ones. Upstream types this as the event's payload type parameter."

            "preventDefault" => T<unit> ^-> T<unit>
            |> WithComment "Prevents the default action of the event."

            "isDefaultPrevented" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if the event's default action was prevented."

            "stopPropagation" => T<unit> ^-> T<unit>
            |> WithComment "Stops the event from propagating."

            "isPropagationStopped" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if the event's propagation was stopped."

            "stopImmediatePropagation" => T<unit> ^-> T<unit>
            |> WithComment "Stops the event from propagating to any further listeners."

            "isImmediatePropagationStopped" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if immediate propagation was stopped."
        ]

        ChangeEventClass
        |=> Inherits EventClass.Type
        |+> Instance [
            "level" =? UndoLevelClass.Type
            |> WithComment "The undo level that was just created."

            "lastLevel" =? !?UndoLevelClass.Type
            |> WithComment "The previous undo level, if any."
        ]

        NodeChangeEventClass
        |=> Inherits EventClass.Type
        |+> Instance [
            "element" =? T<Element>
            |> WithComment "The element the caret is in or the selection starts at."

            "parents" =? !|T<Node>
            |> WithComment "The node path from the root to the current element."

            "selectionChange" =? T<bool>
            |> WithComment "True if the change was caused by a selection change."

            "initial" =? T<bool>
            |> WithComment "True for the initial node change fired during initialization."
        ]

        ContentEventClass
        |=> Inherits EventClass.Type
        |+> Instance [
            "content" =? T<string>
            |> WithComment "The content being read or written."

            "format" =? ContentFormat.Type
            |> WithComment "The content format: html, text, tree or raw."
        ]

        SetContentEventClass
        |=> Inherits ContentEventClass.Type

        ExecCommandEventClass
        |=> Inherits EventClass.Type
        |+> Instance [
            "command" =? T<string>
            |> WithComment "The command being executed."

            "ui" =? T<bool>
            |> WithComment "True if the command was triggered from the UI."

            "value" =? T<obj>
            |> WithComment "The command's value, which is untyped upstream (`any`)."
        ]

        FormatEventClass
        |=> Inherits EventClass.Type
        |+> Instance [
            "format" =? T<string>
            |> WithComment "The name of the format."

            "node" =? T<Node>
            |> WithComment "The node the format applies to."

            "parents" =? !|T<Element>
            |> WithComment "The parents of the node."
        ]

        UndoRedoEventClass
        |=> Inherits EventClass.Type
        |+> Instance [
            "level" =? UndoLevelClass.Type
            |> WithComment "The undo level that was restored or discarded."
        ]

        SwitchModeEventClass
        |=> Inherits EventClass.Type
        |+> Instance [
            "mode" =? T<string>
            |> WithComment "The mode that was switched to, for example \"design\" or \"readonly\"."
        ]

        // ------------------------------------------------------------------
        // Options
        // ------------------------------------------------------------------

        let mapType = T<obj>

        // Fields typed `obj` are JavaScript records/maps (`Record<string, …>`)
        // or opaque payloads that WIG cannot express; this is called out in the
        // README. Everything else is typed as closely as the upstream `.d.ts`.
        let commonOptions : (string * Type.Type) list = [
            // ---- basic setup
            "selector", T<string>
            "target", T<HTMLElement>
            "id", T<string>
            "inline", T<bool>
            "hidden_input", T<bool>
            "readonly", T<bool>
            "width", T<int> + T<string>
            "height", T<int> + T<string>
            "min_width", T<int>
            "min_height", T<int>
            "max_width", T<int>
            "max_height", T<int>
            "resize", T<bool> + T<string>
            "autoresize_bottom_margin", T<int>
            "autoresize_overflow_padding", T<int>
            "auto_focus", T<string> + T<bool>
            "init_instance_callback", EditorClass.Type ^-> T<unit>
            "setup", EditorClass.Type ^-> T<unit>
            "base_url", T<string>
            "suffix", T<string>
            "cache_suffix", T<string>
            "language", T<string>
            "language_url", T<string>
            "language_load", T<bool>
            "directionality", Directionality.Type
            "placeholder", T<string>
            "promotion", T<bool>
            "branding", T<bool>
            "deprecation_warnings", T<bool>

            // ---- plugins, theme and UI
            "plugins", T<string> + !|T<string>
            "external_plugins", mapType
            "forced_plugins", T<string> + !|T<string>
            "plugin_base_urls", mapType
            "theme", T<string>
            "theme_url", T<string>
            "model", T<string>
            "model_url", T<string>
            "icons", T<string>
            "icons_url", T<string>
            "skin", T<bool> + T<string>
            "skin_url", T<string>
            "content_css", T<bool> + T<string> + !|T<string>
            "content_css_cors", T<bool>
            "content_style", T<string>
            "content_security_policy", T<string>
            "referrer_policy", T<string>
            "menubar", T<bool> + T<string>
            "menu", mapType
            "toolbar", T<bool> + T<string> + !|T<string>
            "toolbar1", T<string>
            "toolbar2", T<string>
            "toolbar3", T<string>
            "toolbar4", T<string>
            "toolbar5", T<string>
            "toolbar6", T<string>
            "toolbar7", T<string>
            "toolbar8", T<string>
            "toolbar9", T<string>
            "toolbar_groups", mapType
            "toolbar_location", ToolbarLocation.Type
            "toolbar_mode", ToolbarMode.Type
            "toolbar_sticky", T<bool>
            "toolbar_sticky_offset", T<int>
            "statusbar", T<bool>
            "elementpath", T<bool>
            "ui_mode", UIMode.Type
            "fixed_toolbar_container", T<string>
            "fixed_toolbar_container_target", T<HTMLElement>
            "sidebar_show", T<string>
            "contextmenu", T<string> + !|T<string> + T<bool>
            "contextmenu_never_use_native", T<bool>
            "removed_menuitems", T<string>
            "quickbars_insert_toolbar", T<bool> + T<string>
            "quickbars_selection_toolbar", T<bool> + T<string>
            "quickbars_image_toolbar", T<bool> + T<string>

            // ---- content and formatting
            "content_langs", mapType
            "document_base_url", T<string>
            "convert_urls", T<bool>
            "relative_urls", T<bool>
            "remove_script_host", T<bool>
            "allow_script_urls", T<bool>
            "allow_html_data_urls", T<bool>
            "allow_svg_data_urls", T<bool>
            "allow_html_in_named_anchor", T<bool>
            "allow_conditional_comments", T<bool>
            "allow_unsafe_link_target", T<bool>
            "convert_unsafe_embeds", T<bool>
            "block_unsupported_drop", T<bool>
            "br_in_pre", T<bool>
            "block_formats", T<string>
            "br_newline_selector", T<string>
            "no_newline_selector", T<string>
            "body_id", T<string>
            "body_class", T<string>
            "doctype", T<string>
            "element_format", ElementFormat.Type
            "encoding", T<string>
            "entities", T<string>
            "entity_encoding", EntityEncoding.Type
            "extended_valid_elements", T<string>
            "custom_elements", T<string> + mapType
            "fix_list_elements", T<bool>
            "forced_root_block", T<string>
            "forced_root_block_attrs", mapType
            "formats", mapType
            "format_noneditable_selector", T<string>
            "indent", T<bool>
            "indent_after", T<string>
            "indent_before", T<string>
            "indent_use_margin", T<bool>
            "indentation", T<string>
            "invalid_elements", T<string>
            "invalid_styles", T<string> + mapType
            "keep_styles", T<bool>
            "newline_behavior", NewlineBehavior.Type
            "noneditable_class", T<string>
            "noneditable_regexp", T<RegExp> + !|T<RegExp>
            "nowrap", T<bool>
            "object_resizing", T<bool> + T<string>
            "pad_empty_with_br", T<bool>
            "preserve_cdata", T<bool>
            "protect", !|T<RegExp>
            "remove_trailing_brs", T<bool>
            "root_name", T<string>
            "schema", SchemaType.Type
            "valid_children", T<string>
            "valid_classes", T<string> + mapType
            "valid_elements", T<string>
            "valid_styles", T<string> + mapType
            "verify_html", T<bool>
            "xss_sanitization", T<bool>
            "visual", T<bool>
            "visual_anchor_class", T<string>
            "visual_table_class", T<string>
            "editable_class", T<string>
            "editable_root", T<bool>

            // ---- style and colour
            "color_map", !|T<string>
            "color_map_foreground", !|T<string>
            "color_map_background", !|T<string>
            "color_cols", T<int>
            "color_cols_foreground", T<int>
            "color_cols_background", T<int>
            "color_default_background", T<string>
            "color_default_foreground", T<string>
            "custom_colors", T<bool>
            "font_css", T<string> + !|T<string>
            "font_family_formats", T<string>
            "font_size_classes", T<string>
            "font_size_formats", T<string>
            "font_size_input_default_unit", T<string>
            "font_size_legacy_values", T<string>
            "font_size_style_values", T<string>
            "line_height_formats", T<string>
            "style_formats", mapType
            "style_formats_autohide", T<bool>
            "style_formats_merge", T<bool>
            "preview_styles", T<bool> + T<string>
            "visualblocks_default_state", T<bool>
            "visualchars_default_state", T<bool>

            // ---- images, uploads and files
            "automatic_uploads", T<bool>
            "images_file_types", T<string>
            "images_replace_blob_uris", T<bool>
            "images_reuse_filename", T<bool>
            "images_upload_base_path", T<string>
            "images_upload_credentials", T<bool>
            "images_upload_handler", (BlobInfoClass.Type * (T<float> ^-> T<unit>)) ^-> T<Promise<_>>[T<string>]
            "images_upload_url", T<string>
            "file_picker_callback", (((T<string> * T<obj>) ^-> T<unit>) * T<string> * T<obj>) ^-> T<unit>
            "file_picker_types", T<string>
            "file_picker_validator_handler", mapType

            // ---- links and media
            "anchor_bottom", T<bool> + T<string>
            "anchor_top", T<bool> + T<string>
            "default_link_target", T<string>
            "link_assume_external_targets", T<bool> + T<string>
            "link_class_list", mapType
            "link_context_toolbar", T<bool>
            "link_default_protocol", T<string>
            "link_list", mapType
            "link_title", T<bool>
            "target_list", mapType
            "typeahead_urls", T<bool>
            "media_alt_source", T<bool> + mapType
            "media_dimensions", T<bool>
            "media_filter_htmldata", T<bool>
            "media_live_embeds", T<bool>
            "media_poster", T<bool> + mapType
            "media_url_resolver", (T<obj> * ((T<string> * T<obj>) ^-> T<unit>)) ^-> T<unit>
            "media_embed", mapType
            "audio_template_callback", (T<obj> ^-> T<string>)
            "video_template_callback", (T<obj> ^-> T<string>)

            // ---- paste and input
            "paste_as_text", T<bool>
            "paste_block_drop", T<bool>
            "paste_data_images", T<bool>
            "paste_merge_formats", T<bool>
            "paste_postprocess", (EditorClass.Type * T<obj>) ^-> T<unit>
            "paste_preprocess", (EditorClass.Type * T<obj>) ^-> T<unit>
            "paste_remove_styles_if_webkit", T<bool>
            "paste_tab_spaces", T<int>
            "paste_webkit_styles", T<string>
            "smart_paste", T<bool>
            "browser_spellcheck", T<bool>
            "spellchecker_languages", T<string>
            "spellchecker_language", T<string>
            "spellchecker_rpc_url", T<string>
            "text_patterns", mapType
            "text_patterns_lookup", mapType
            "end_container_on_empty_block", T<bool> + T<string>
            "submit_patch", T<bool>
            "add_form_submit_trigger", T<bool>
            "add_unload_trigger", T<bool>
            "custom_undo_redo_levels", T<int>
            "custom_ui_selector", T<string>
            "event_root", T<string>
            "disable_nodechange", T<bool>
            "service_message", T<string>

            // ---- URLs and callbacks
            "url_converter", (T<string> * T<string> * T<bool> * T<string>) ^-> T<string>
            "url_converter_scope", T<obj>
            "urlconverter_callback", (T<string> * T<string> * T<bool> * T<string>) ^-> T<string>

            // ---- fiddly multi-shape options
            "iframe_attrs", mapType
            "iframe_aria_text", T<string>
            "table_tab_navigation", T<bool>
            "help_accessibility", T<bool>
            "a11y_advanced_options", T<bool>
            "highlight_on_focus", T<bool>
            "draggable_modal", T<bool>
            "init_content_sync", T<bool>
            "sandbox_iframes", T<bool>
        ]

        // Options that exist in only one of the two upstream lines.
        // Options that only exist on one of the two upstream lines.
        let versionOptions : (string * Type.Type) list =
            if isV8 then
                [
                    // use `'gpl'` to accept the GPL terms, or a commercial key
                    "license_key", T<string>
                    "crossorigin", T<string>
                    "disabled", T<bool>
                    "content_language", T<string>
                    "content_id", T<string>
                    "allow_noneditable", T<bool>
                    "documents_file_types", !|T<string>
                    "list_max_depth", T<int>
                    "onboarding", T<bool>
                    "sandbox_iframes_exclusions", !|T<string>
                    "sidebar_width", T<int>
                    "sidebar_min_width", T<int>
                    "sidebar_max_width", T<int>
                    "view_show", T<string>
                    "user_id", T<string>
                    "fetch_users", !|T<string> ^-> T<Promise<_>>[!|UserClass.Type]
                ]
            else
                [
                    // removed in TinyMCE 7
                    "force_hex_color", T<string>
                ]

        /// `EditorOptions(...)` builds a TinyMCE options object. WIG capitalizes
        /// the .NET members while the JS names stay snake_case, so
        /// `InitInstanceCallback` sets `init_instance_callback`.
        let EditorOptions =
            let fields = commonOptions @ versionOptions
            Class "EditorOptions"
            |+> Instance (
                [ for (jsName, ty) in fields ->
                    Property jsName ty :> CodeModel.IClassMember ]
            )
            |+> Static [
                let ps: Type.Parameters = { This = None; Variable = None; Arguments = [] }
                ObjectConstructor ps
            ]

        // ------------------------------------------------------------------
        // Editor collaborators
        // ------------------------------------------------------------------

        UndoLevelClass
        |+> Instance [
            "type" =? T<string>
            |> WithComment "complete, or fragmented."
        ]

        ShortcutsClass
        |+> Instance [
            "add" => T<string> * T<string> * (T<string> + (T<unit> ^-> T<unit>)) ^-> T<bool>
            |> WithComment "Registers a keyboard shortcut. The command is either a command name or a function."

            "remove" => T<string> ^-> T<bool>
            |> WithComment "Removes a keyboard shortcut by pattern."
        ]

        OptionsClass
        |+> Instance [
            // `options.get(name)` is generic upstream; the WIG exposes typed
            // readers instead of an `obj` return.
            "getString" => T<string>?``name`` ^-> T<string>
            |> WithInline "$this.get($name)"
            |> WithComment "Reads a string option by name (`editor.options.get(name)`)."

            "getBool" => T<string>?``name`` ^-> T<bool>
            |> WithInline "$this.get($name)"
            |> WithComment "Reads a boolean option by name (`editor.options.get(name)`)."

            "getNumber" => T<string>?``name`` ^-> T<int>
            |> WithInline "$this.get($name)"
            |> WithComment "Reads a numeric option by name (`editor.options.get(name)`)."

            "getStringArray" => T<string>?``name`` ^-> !|T<string>
            |> WithInline "$this.get($name)"
            |> WithComment "Reads a string list option by name (`editor.options.get(name)`)."

            "set" => T<string> * T<string> ^-> T<bool>
            |> WithComment "Sets an option. Returns true if the option changed."

            "setBool" => (T<string>?``name`` * T<bool>?``value``) ^-> T<bool>
            |> WithInline "$this.set($name, $value)"

            "setNumber" => (T<string>?``name`` * T<int>?``value``) ^-> T<bool>
            |> WithInline "$this.set($name, $value)"

            "unset" => T<string> ^-> T<bool>
            |> WithComment "Unsets an option. Returns true if the option was set."

            "isSet" => T<string> ^-> T<bool>
            |> WithComment "Returns true if the option has a value (including its default)."

            "isRegistered" => T<string> ^-> T<bool>
            |> WithComment "Returns true if the option is registered."
        ]

        ModeClass
        |+> Instance [
            "get" => T<unit> ^-> T<string>
            |> WithComment "Returns the current mode, for example `design` or `readonly`."

            "set" => T<string> ^-> T<unit>
            |> WithComment "Sets the current mode. Common values are `design` and `readonly`."

            "isReadOnly" => T<unit> ^-> T<bool>
            |> WithComment "Returns true when the editor is read-only."

            "register" => T<string> * T<obj> ^-> T<unit>
            |> WithComment "Registers a custom mode. The API object is `{ activate, deactivate, editorReadOnly }`."
        ]

        let FormatClass =
            Pattern.Config "Format" {
                Required = []
                Optional = [
                    "inline", T<string>
                    "block", T<string>
                    "selector", T<string>
                    "classes", T<string>
                    "attributes", mapType
                    "styles", mapType
                    "title", T<string>
                    "remove_similar", T<bool>
                    "deep", T<bool>
                    "exact", T<bool>
                    "merge_siblings", T<bool>
                ]
            }

        FormatterClass
        |+> Instance [
            "apply" => T<string> ^-> T<unit>
            |> WithComment "Applies the named format to the current selection."

            "apply" => T<string> * T<Node> ^-> T<unit>
            |> WithComment "Applies the named format to a node."

            "remove" => T<string> ^-> T<unit>
            |> WithComment "Removes the named format from the current selection."

            "remove" => T<string> * T<Node> ^-> T<unit>

            "toggle" => T<string> ^-> T<unit>
            |> WithComment "Toggles the named format on the current selection."

            "match" => T<string> ^-> T<bool>
            |> WithComment "Returns true if the current selection has the named format applied."

            "matchAll" => !|T<string> ^-> !|T<string>
            |> WithComment "Returns the formats from the given names that match the current selection."

            "closest" => !|T<string> ^-> !?T<string>
            |> WithComment "Returns the closest matching format name, or null."

            "has" => T<string> ^-> T<bool>
            |> WithComment "Returns true if the format is registered."

            "canApply" => T<string> ^-> T<bool>
            |> WithComment "Returns true if the format can be applied to the current selection."

            "register" => T<string> * FormatClass.Type ^-> T<unit>
            |> WithComment "Registers a custom format definition."

            "unregister" => T<string> ^-> T<unit>
            |> WithComment "Unregisters a custom format definition."

            "getCssText" => T<string> ^-> T<string>
            |> WithComment "Returns the CSS text for the named format."

            "formatChanged" => T<string> * (T<bool> * FormatChangeDataClass.Type ^-> T<unit>) ^-> FormatChangedHandleClass.Type
            |> WithComment "Subscribes to format changes; the string may list several comma-separated formats. Dispose with `Unbind`."
        ]

        FormatChangeDataClass
        |+> Instance [
            "node" =? T<Node>
            |> WithComment "The node the format applies to."

            "format" =? T<string>
            |> WithComment "The format that changed."

            "parents" =? !|T<Element>
            |> WithComment "The parents of the node."
        ]

        FormatChangedHandleClass
        |+> Instance [
            "unbind" => T<unit> ^-> T<unit>
            |> WithComment "Removes the format-changed subscription."
        ]

        CommandsClass
        |+> Instance [
            "execCommand" => T<string> ^-> T<bool>
            |> WithComment "Executes a command on this editor. Returns true when the command was handled."

            "execCommandArgs" => T<string> * T<bool> * T<obj> ^-> T<bool>
            |> WithComment "Executes a command with UI state and a value."

            "queryCommandState" => T<string> ^-> T<bool>
            |> WithComment "Returns the state of a command, for example whether `Bold` is active."

            "queryCommandValue" => T<string> ^-> T<string>
            |> WithComment "Returns the value of a command."

            "queryCommandSupported" => T<string> ^-> T<bool>
            |> WithComment "Returns true if the command is registered."

            "addCommand" => T<string> * (T<bool> * T<obj> ^-> T<unit>) ^-> T<unit>
            |> WithComment "Registers a command; the callback receives the UI flag and the command value (untyped upstream)."

            "addQueryStateHandler" => T<string> * (T<unit> ^-> T<bool>) ^-> T<unit>
            |> WithComment "Registers a state handler for a command."

            "addQueryValueHandler" => T<string> * (T<unit> ^-> T<string>) ^-> T<unit>
            |> WithComment "Registers a value handler for a command."

        ] |> ignore

        if isV8 then
            CommandsClass
            |+> Instance [
                "removeCommand" => T<string> ^-> T<unit>
                |> WithComment "Removes a command."
            ]
            |> ignore

        SelectionClass
        |+> Instance [
            "getContent" => T<unit> ^-> T<string>
            |> WithComment "Returns the selected content as HTML."

            "getContent" => T<string> ^-> T<string>
            |> WithComment "Returns the selected content in the given format."

            "setContent" => T<string> ^-> T<unit>
            |> WithComment "Replaces the selection with the given content."

            "getNode" => T<unit> ^-> T<HTMLElement>
            |> WithComment "Returns the node the selection starts in."

            "setNode" => T<Element> ^-> T<Element>
            |> WithComment "Sets the selection to the given element."

            "select" => T<Node> ^-> T<Node>
            |> WithComment "Selects the given node."

            "collapse" => T<unit> ^-> T<unit>
            |> WithComment "Collapses the selection to its start."

            "collapse" => T<bool> ^-> T<unit>
            |> WithComment "Collapses the selection to its start (false) or end (true)."

            "isCollapsed" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if the selection is collapsed (a caret)."

            "isEditable" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if the selection is inside an editable element."

            "isForward" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if the selection is in a forward direction."

            "getStart" => T<unit> ^-> T<Element>
            |> WithComment "Returns the start element of the selection."

            "getEnd" => T<unit> ^-> T<Element>
            |> WithComment "Returns the end element of the selection."

            "getSelectedBlocks" => T<unit> ^-> !|T<Element>
            |> WithComment "Returns the block elements the selection touches."

            "getRng" => T<unit> ^-> T<Range>
            |> WithComment "Returns the current DOM range."

            "setRng" => T<Range> ^-> T<unit>
            |> WithComment "Sets the current DOM range."

            "getBookmark" => T<unit> ^-> BookmarkClass.Type
            |> WithComment "Stores the selection as a bookmark that survives DOM changes."

            "moveToBookmark" => BookmarkClass.Type ^-> T<unit>
            |> WithComment "Restores a bookmark created with `GetBookmark`."

            "placeCaretAt" => T<float> * T<float> ^-> T<unit>
            |> WithComment "Places the caret at the given client coordinates."

            "scrollIntoView" => T<unit> ^-> T<unit>
            |> WithComment "Scrolls the selection into view."

            "scrollIntoView" => T<HTMLElement> * T<bool> ^-> T<unit>

            "getClientRects" => T<unit> ^-> !|T<obj>
            |> WithComment "Returns the client rectangles of the selection."

            "destroy" => T<unit> ^-> T<unit>
            |> WithComment "Destroys the selection object."
        ]

        UndoManagerClass
        |+> Instance [
            "undo" => T<unit> ^-> !?UndoLevelClass.Type
            |> WithComment "Undoes the last change."

            "redo" => T<unit> ^-> !?UndoLevelClass.Type
            |> WithComment "Redoes the last undone change."

            "clear" => T<unit> ^-> T<unit>
            |> WithComment "Clears the undo history."

            "reset" => T<unit> ^-> T<unit>
            |> WithComment "Resets the undo manager without firing change events."

            "hasUndo" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if there is anything to undo."

            "hasRedo" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if there is anything to redo."

            "add" => T<unit> ^-> !?UndoLevelClass.Type
            |> WithComment "Adds a new undo level."

            "beforeChange" => T<unit> ^-> T<unit>
            |> WithComment "Snapshots the current selection before a change."

            "dispatchChange" => T<unit> ^-> T<unit>
            |> WithComment "Fires the change event for the current level."

            "transact" => (T<unit> ^-> T<unit>) ^-> !?UndoLevelClass.Type
            |> WithComment "Runs a callback as a single undo level."

            "ignore" => (T<unit> ^-> T<unit>) ^-> T<unit>
            |> WithComment "Runs a callback without recording an undo level."

            "extra" => (T<unit> ^-> T<unit>) * (T<unit> ^-> T<unit>) ^-> T<unit>
            |> WithComment "Runs a callback, then applies the second one without adding an undo level."
        ]

        // ------------------------------------------------------------------
        // UI registry (buttons, menus, icons, sidebars)
        // ------------------------------------------------------------------

        ToolbarButtonApiClass
        |+> Instance [
            "isEnabled" => T<unit> ^-> T<bool>
            "setEnabled" => T<bool> ^-> T<unit>
            "setText" => T<string> ^-> T<unit>
            "setIcon" => T<string> ^-> T<unit>
        ]

        ToolbarButtonSpecClass
        |+> Instance [
            "type" =@ T<string>
            "enabled" =@ T<bool>
            "tooltip" =@ T<string>
            "icon" =@ T<string>
            "text" =@ T<string>
            "shortcut" =@ T<string>
            "context" =@ T<string>
            "onAction" =@ (ToolbarButtonApiClass.Type ^-> T<unit>)
            "onSetup" =@ (ToolbarButtonApiClass.Type ^-> T<System.Action>)
            |> WithComment "Called when the button is created; return a teardown callback."
        ]

        ToolbarToggleButtonApiClass
        |=> Inherits ToolbarButtonApiClass.Type
        |+> Instance [
            "isActive" => T<unit> ^-> T<bool>
            "setActive" => T<bool> ^-> T<unit>
        ]

        ToolbarToggleButtonSpecClass
        |=> Inherits ToolbarButtonSpecClass.Type
        |+> Instance [
            "type" =@ T<string>
            "active" =@ T<bool>
            "onAction" =@ (ToolbarToggleButtonApiClass.Type ^-> T<unit>)
            "onSetup" =@ (ToolbarToggleButtonApiClass.Type ^-> T<System.Action>)
        ]

        MenuButtonApiClass
        |=> Inherits ToolbarButtonApiClass.Type
        |+> Instance [
            "isActive" => T<unit> ^-> T<bool>
            "setActive" => T<bool> ^-> T<unit>
            "setTooltip" => T<string> ^-> T<unit>
        ]

        MenuButtonSpecClass
        |=> Inherits ToolbarButtonSpecClass.Type
        |+> Instance [
            "type" =@ T<string>
            "search" =@ T<bool>
            "fetch" =@ (!|T<obj> ^-> T<unit>) * T<obj> * MenuButtonApiClass.Type ^-> T<unit>
            |> WithComment "Fills the menu; call the success callback with a menu name or an array of item specs."
            "onSetup" =@ (MenuButtonApiClass.Type ^-> T<System.Action>)
        ]

        SplitButtonApiClass
        |=> Inherits MenuButtonApiClass.Type
        |+> Instance [
            "setIconFill" => T<string> * T<string> ^-> T<unit>
        ]

        SplitButtonSpecClass
        |=> Inherits ToolbarButtonSpecClass.Type
        |+> Instance [
            "type" =@ T<string>
            "chevronTooltip" =@ T<string>
            "select" =@ (T<string> ^-> T<bool>)
            "presets" =@ T<string>
            "columns" =@ T<int> + T<string>
            "fetch" =@ (!|T<obj> ^-> T<unit>) ^-> T<unit>
            "onAction" =@ (SplitButtonApiClass.Type ^-> T<unit>)
            "onItemAction" =@ (SplitButtonApiClass.Type * T<string> ^-> T<unit>)
            "onSetup" =@ (SplitButtonApiClass.Type ^-> T<System.Action>)
        ]

        GroupToolbarButtonSpecClass
        |=> Inherits ToolbarButtonSpecClass.Type
        |+> Instance [
            "type" =@ T<string>
            "items" =@ T<string>
        ]

        MenuItemApiClass
        |+> Instance [
            "isEnabled" => T<unit> ^-> T<bool>
            "setEnabled" => T<bool> ^-> T<unit>
        ]

        MenuItemSpecClass
        |+> Instance [
            "type" =@ T<string>
            "enabled" =@ T<bool>
            "text" =@ T<string>
            "value" =@ T<string>
            "icon" =@ T<string>
            "shortcut" =@ T<string>
            "context" =@ T<string>
            "onAction" =@ (MenuItemApiClass.Type ^-> T<unit>)
            "onSetup" =@ (MenuItemApiClass.Type ^-> T<System.Action>)
        ]

        ToggleMenuItemApiClass
        |=> Inherits MenuItemApiClass.Type
        |+> Instance [
            "isActive" => T<unit> ^-> T<bool>
            "setActive" => T<bool> ^-> T<unit>
        ]

        ToggleMenuItemSpecClass
        |=> Inherits MenuItemSpecClass.Type
        |+> Instance [
            "type" =@ T<string>
            "active" =@ T<bool>
            "onAction" =@ (ToggleMenuItemApiClass.Type ^-> T<unit>)
            "onSetup" =@ (ToggleMenuItemApiClass.Type ^-> T<System.Action>)
        ]

        NestedMenuItemApiClass
        |=> Inherits MenuItemApiClass.Type
        |+> Instance [
            "setTooltip" => T<string> ^-> T<unit>
            "setIconFill" => T<string> * T<string> ^-> T<unit>
        ]

        NestedMenuItemSpecClass
        |=> Inherits MenuItemSpecClass.Type
        |+> Instance [
            "type" =@ T<string>
            "getSubmenuItems" =@ (T<unit> ^-> (T<string> + !|T<obj>))
            "onSetup" =@ (NestedMenuItemApiClass.Type ^-> T<System.Action>)
        ]

        ContextMenuApiClass
        |+> Instance [
            "update" =@ (T<Element> ^-> !|T<obj>)
            |> WithComment "Returns the menu items for the element under the caret."
        ]

        SidebarApiClass
        |+> Instance [
            "element" => T<unit> ^-> T<HTMLElement>
            |> WithComment "The sidebar's root element."
        ]

        SidebarSpecClass
        |+> Instance [
            "icon" =@ T<string>
            "tooltip" =@ T<string>
            "resizable" =@ T<bool>
            "onShow" =@ (SidebarApiClass.Type ^-> T<unit>)
            "onHide" =@ (SidebarApiClass.Type ^-> T<unit>)
            "onSetup" =@ (SidebarApiClass.Type ^-> T<System.Action>)
        ]

        // Spec objects are plain data: allow `ButtonSpec(Text = …)` construction.
        for specClass in [
            ToolbarButtonSpecClass
            ToolbarToggleButtonSpecClass
            MenuButtonSpecClass
            SplitButtonSpecClass
            GroupToolbarButtonSpecClass
            MenuItemSpecClass
            ToggleMenuItemSpecClass
            NestedMenuItemSpecClass
            SidebarSpecClass
            ContextMenuApiClass
        ] do
            let specCtor: Type.Parameters = { This = None; Variable = None; Arguments = [] }
            specClass |+> Static [ ObjectConstructor specCtor ] |> ignore

        RegistryClass
        |+> Instance [
            "addButton" => T<string> * ToolbarButtonSpecClass.Type ^-> T<unit>
            |> WithComment "Adds a toolbar button."

            "addToggleButton" => T<string> * ToolbarToggleButtonSpecClass.Type ^-> T<unit>
            |> WithComment "Adds a toolbar toggle button."

            "addMenuButton" => T<string> * MenuButtonSpecClass.Type ^-> T<unit>
            |> WithComment "Adds a toolbar menu button."

            "addSplitButton" => T<string> * SplitButtonSpecClass.Type ^-> T<unit>
            |> WithComment "Adds a toolbar split button."

            "addGroupToolbarButton" => T<string> * GroupToolbarButtonSpecClass.Type ^-> T<unit>
            |> WithComment "Adds a toolbar group button."

            "addMenuItem" => T<string> * MenuItemSpecClass.Type ^-> T<unit>
            |> WithComment "Adds a menu item."

            "addNestedMenuItem" => T<string> * NestedMenuItemSpecClass.Type ^-> T<unit>
            |> WithComment "Adds a nested (submenu) menu item."

            "addToggleMenuItem" => T<string> * ToggleMenuItemSpecClass.Type ^-> T<unit>
            |> WithComment "Adds a toggle menu item."

            "addContextMenu" => T<string> * ContextMenuApiClass.Type ^-> T<unit>
            |> WithComment "Adds a context menu."

            "addContextToolbar" => T<string> * mapType ^-> T<unit>
            |> WithComment "Adds a context toolbar."

            "addIcon" => T<string> * T<string> ^-> T<unit>
            |> WithComment "Registers an SVG icon by name."

            "addSidebar" => T<string> * SidebarSpecClass.Type ^-> T<unit>
            |> WithComment "Adds a sidebar."

            "addContext" => T<string> * (T<string> ^-> T<bool>) ^-> T<unit>
            |> WithComment "Adds a custom context predicate for toolbar buttons."
        ]

        UiClass
        |+> Instance [
            "registry" =? RegistryClass.Type
            |> WithComment "The UI registry: buttons, menu items, icons, sidebars, context menus."

            "show" => T<unit> ^-> T<unit>
            "hide" => T<unit> ^-> T<unit>
            "setEnabled" => T<bool> ^-> T<unit>
            "isEnabled" => T<unit> ^-> T<bool>
        ]

        // ------------------------------------------------------------------
        // Notifications and dialogs
        // ------------------------------------------------------------------

        let NotificationSpecClass =
            Pattern.Config "NotificationSpec" {
                Required = [ "text", T<string> ]
                Optional = [
                    "type", NotificationType.Type
                    "icon", T<string>
                    "progressBar", T<bool>
                    "timeout", T<int>
                ]
            }

        NotificationProgress
        |+> Instance [
            "value" => T<int> ^-> T<unit>
            |> WithComment "Sets the progress percentage (0-100)."
        ]

        NotificationApiClass
        |+> Instance [
            "close" => T<unit> ^-> T<unit>
            |> WithComment "Closes the notification."

            "text" => T<string> ^-> T<unit>
            |> WithComment "Replaces the notification text."

            "progressBar" =? NotificationProgress.Type
            |> WithComment "The progress bar of a progress notification."

            "reposition" => T<unit> ^-> T<unit>
            |> WithComment "Repositions the notification."

            "getEl" => T<unit> ^-> T<HTMLElement>
            |> WithComment "Returns the notification's element."

            "settings" =? NotificationSpecClass
            |> WithComment "The notification's settings."
        ]

        NotificationManagerClass
        |+> Instance [
            "open" => NotificationSpecClass ^-> NotificationApiClass.Type
            |> WithComment "Opens a notification."

            "close" => T<unit> ^-> T<unit>
            |> WithComment "Closes all notifications."

            "getNotifications" => T<unit> ^-> !|NotificationApiClass.Type
            |> WithComment "Returns the open notifications."
        ]

        WindowManagerClass
        |+> Instance [
            "alert" => T<string> ^-> T<unit>
            |> WithComment "Shows an alert dialog."

            "alert" => T<string> * (T<unit> ^-> T<unit>) ^-> T<unit>
            |> WithComment "Shows an alert dialog and calls back when it is dismissed."

            "confirm" => T<string> * (T<bool> ^-> T<unit>) ^-> T<unit>
            |> WithComment "Shows a confirm dialog; the callback receives the answer."

            "close" => T<unit> ^-> T<unit>
            |> WithComment "Closes the current dialog."
        ]

        // ------------------------------------------------------------------
        // Plugins, script loading, uploads, DOM
        // ------------------------------------------------------------------

        PluginClass
        |+> Instance [
            "init" =? (EditorClass.Type * T<string> ^-> T<unit>)
            |> WithComment "Called when the plugin is initialized."

            "getMetadata" =? (T<unit> ^-> T<obj>)
            |> WithComment "Optional plugin metadata (`{ name, url }` or `{ name, type, slug }`)."
        ]

        PluginManagerClass
        |+> Instance [
            "add" => T<string> * (EditorClass.Type * T<string> ^-> PluginClass.Type) ^-> PluginClass.Type
            |> WithComment "Registers a plugin by name."

            "get" => T<string> ^-> (!?mapType)
            |> WithComment "Returns the registered plugin constructor, or null. The constructor is `(editor, url) => plugin`."

            "remove" => T<string> ^-> T<unit>
            |> WithComment "Removes a registered plugin."

            "requireLangPack" => T<string> * T<string> ^-> T<unit>
            |> WithComment "Loads the language pack of a plugin."

            "waitFor" => T<string> * T<string> ^-> T<Promise<_>>[T<unit>]
            |> WithComment "Waits for a plugin to reach the given state (`added` or `loaded`)."
        ]

        ScriptLoaderClass
        |+> Instance [
            "load" => T<string> ^-> T<Promise<_>>[T<unit>]
            |> WithComment "Loads a script (once)."

            "loadScripts" => !|T<string> ^-> T<Promise<_>>[T<unit>]
            |> WithComment "Loads several scripts."

            "add" => T<string> ^-> T<Promise<_>>[T<unit>]
            |> WithComment "Adds a script to the load queue."

            "loadQueue" => T<unit> ^-> T<Promise<_>>[T<unit>]
            |> WithComment "Loads the queued scripts."

            "isDone" => T<string> ^-> T<bool>
            |> WithComment "Returns true if the script was already loaded."

            "markDone" => T<string> ^-> T<unit>
            |> WithComment "Marks a script as loaded."

            "remove" => T<string> ^-> T<unit>
            |> WithComment "Removes a script from the queue."
        ]

        BlobInfoClass
        |+> Instance [
            "id" => T<unit> ^-> T<string>
            |> WithComment "A unique identifier for the blob."

            "name" => T<unit> ^-> T<string>
            |> WithComment "The file name, including its extension."

            "filename" => T<unit> ^-> T<string>
            |> WithComment "The sanitized file name."

            "blob" => T<unit> ^-> T<Blob>
            |> WithComment "The underlying blob."

            "base64" => T<unit> ^-> T<string>
            |> WithComment "The base64-encoded data, without the data URI prefix."

            "blobUri" => T<unit> ^-> T<string>
            |> WithComment "An object URL for the blob."

            "uri" => T<unit> ^-> T<string>
            |> WithComment "A data URI for the blob."

            "size" => T<unit> ^-> T<int>
            |> WithComment "The blob size in bytes."
        ]

        UploadResultClass
        |+> Instance [
            "element" =? T<HTMLImageElement>
            |> WithComment "The image element the upload belongs to."

            "status" =? T<bool>
            |> WithComment "True if the upload succeeded."

            "blobInfo" =? BlobInfoClass.Type
            |> WithComment "The uploaded blob."

            "uploadUri" =? T<string>
            |> WithComment "The final upload URI."

            "removed" =? T<bool>
            |> WithComment "True if the element was removed."
        ]

        DomUtilsClass
        |+> Instance [
            "select" => T<string> ^-> !|T<Element>
            |> WithComment "Selects elements matching a CSS selector."

            "get" => T<string> ^-> T<Element>
            |> WithComment "Returns an element by id."

            "getParent" => T<Node> ^-> T<Element>
            |> WithComment "Returns the closest parent element."

            "getRoot" => T<unit> ^-> T<Element>
            |> WithComment "Returns the editor's root element."

            "getOuterHTML" => T<Element> ^-> T<string>
            |> WithComment "Returns an element's outer HTML."

            "setOuterHTML" => T<Element> * T<string> ^-> T<unit>
            |> WithComment "Replaces an element with the given HTML."

            "create" => T<string> ^-> T<HTMLElement>
            |> WithComment "Creates an element by tag name."

            "addClass" => T<Element> * T<string> ^-> T<unit>
            |> WithComment "Adds a class to an element."

            "removeClass" => T<Element> * T<string> ^-> T<unit>
            |> WithComment "Removes a class from an element."

            "hasClass" => T<Element> * T<string> ^-> T<bool>
            |> WithComment "Returns true if the element has the class."

            "is" => T<Element> * T<string> ^-> T<bool>
            |> WithComment "Returns true if the element matches the selector."

            "setStyle" => T<Element> * T<string> * T<string> ^-> T<unit>
            |> WithComment "Sets an inline style property."

            "getStyle" => T<Element> * T<string> ^-> T<string>
            |> WithComment "Reads an inline style property."

            "setAttrib" => T<Element> * T<string> * T<string> ^-> T<unit>
            |> WithComment "Sets an attribute."

            "getAttrib" => T<Element> * T<string> ^-> T<string>
            |> WithComment "Reads an attribute."

            "remove" => T<Node> ^-> T<Node>
            |> WithComment "Removes a node from the DOM."

            "insertAfter" => T<Node> * T<Node> ^-> T<Node>
            |> WithComment "Inserts a node after a reference node."

            "replace" => T<Node> * T<Node> ^-> T<Node>
            |> WithComment "Replaces a node with another."

            "findCommonAncestor" => T<Node> * T<Node> ^-> T<Element>
            |> WithComment "Finds the closest common ancestor of two nodes."
        ]

        // ------------------------------------------------------------------
        // Editor
        // ------------------------------------------------------------------

        EditorClass
        |+> Instance [
            // ---- properties
            "id" =? T<string>
            |> WithComment "The editor id (the id of the target element)."

            "editorUid" =? T<string>
            |> WithComment "A unique, session-wide editor id."

            "targetElm" =? T<HTMLElement>
            |> WithComment "The element the editor was initialized on."

            "startContent" =? T<string>
            |> WithComment "The content the editor started with; useful for dirty checks."

            "initialized" =? T<bool>
            "hidden" =? T<bool>
            "inline" =? T<bool>
            "readonly" =? T<bool>
            "removed" =? T<bool>
            "hasVisual" =? T<bool>
            "isNotDirty" =? T<bool>
            "suffix" =? T<string>

            "contentDocument" =? T<Document>
            |> WithComment "The document of the editable area."

            "contentWindow" =? T<Window>
            |> WithComment "The window of the editable area."

            "bodyElement" =? !?T<HTMLElement>
            |> WithComment "The body element of the editable area (when rendered)."

            "container" =? T<HTMLElement>
            |> WithComment "The editor's container element."

            "contentAreaContainer" =? T<HTMLElement>
            |> WithComment "The element holding the editable area."

            "editorContainer" =? T<HTMLElement>
            |> WithComment "The element holding the toolbar and the editable area."

            "iframeElement" =? !?T<HTMLElement>
            |> WithComment "The iframe element, when the editor is iframe-based."

            "mode" =? ModeClass.Type
            |> WithComment "The editor mode API (`design`/`readonly`)."

            "options" =? OptionsClass.Type
            |> WithComment "Typed access to the editor's options."

            "ui" =? UiClass.Type
            |> WithComment "The UI API, including `ui.registry`."

            "selection" =? SelectionClass.Type
            |> WithComment "The selection API."

            "undoManager" =? UndoManagerClass.Type
            |> WithComment "The undo manager."

            "formatter" =? FormatterClass.Type
            |> WithComment "The format (bold, italic, …) API."

            "editorCommands" =? CommandsClass.Type
            |> WithComment "The command API."

            "DOM" =? DomUtilsClass.Type
            |> WithComment "A DOM helper scoped to the editor."

            "windowManager" =? WindowManagerClass.Type
            |> WithComment "The window (dialog) manager."

            "notificationManager" =? NotificationManagerClass.Type
            |> WithComment "The notification manager."

            "shortcuts" =? ShortcutsClass.Type
            |> WithComment "The keyboard shortcut API."

            "plugins" =? mapType
            |> WithComment "The loaded plugins, keyed by name (a JavaScript record)."

            "editorManager" =? TinyMCEClass.Type
            |> WithComment "The global `tinymce` manager this editor belongs to."

            // ---- lifecycle
            "render" => T<unit> ^-> T<unit>
            |> WithComment "Renders the editor (called by `tinymce.init`)."

            "remove" => T<unit> ^-> T<unit>
            |> WithComment "Removes the editor from the DOM and from the editor manager."

            "destroy" => T<unit> ^-> T<unit>
            |> WithComment "Destroys the editor and frees its resources."

            "setProgressState" => T<bool> * T<int> ^-> T<unit>
            |> WithComment "Shows or hides an indeterminate progress bar for the given number of milliseconds."

            // ---- content
            "getContent" => T<unit> ^-> T<string>
            |> WithComment "Returns the editor content as HTML."

            "getContent" => T<string> ^-> T<string>
            |> WithComment "Returns the editor content in the given format (`html`, `text`, `raw`)."

            "setContent" => T<string> ^-> T<unit>
            |> WithComment "Sets the editor content."

            "insertContent" => T<string> ^-> T<unit>
            |> WithComment "Inserts content at the caret."

            "resetContent" => T<unit> ^-> T<unit>
            |> WithComment "Resets the content to the initial value."

            "resetContent" => T<string> ^-> T<unit>
            |> WithComment "Resets the content to the given value."

            "load" => T<unit> ^-> T<unit>
            |> WithComment "Loads the content from the target element (deprecated upstream)."

            "save" => T<unit> ^-> T<string>
            |> WithComment "Saves the content to the target element (deprecated upstream)."

            "isDirty" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if the content changed since the last `SetDirty(false)`."

            "setDirty" => T<bool> ^-> T<unit>
            |> WithComment "Sets the dirty state."

            "nodeChanged" => T<unit> ^-> T<unit>
            |> WithComment "Fires a node change for the current selection."

            "addVisual" => T<unit> ^-> T<unit>
            |> WithComment "Adds the visual aid classes to the content."

            "convertURL" => T<string> * T<string> ^-> T<string>
            |> WithComment "Converts a URL against the configured base URL."

            "uploadImages" => T<unit> ^-> T<Promise<_>>[!|UploadResultClass.Type]
            |> WithComment "Uploads the blob images in the content."

            "setEditableRoot" => T<bool> ^-> T<unit>
            |> WithComment "Toggles the editable root."

            "hasEditableRoot" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if the editor uses an editable root."

            // ---- focus and visibility
            "focus" => T<unit> ^-> T<unit>
            |> WithComment "Focuses the editor."

            "focus" => T<bool> ^-> T<unit>
            |> WithComment "Focuses the editor, optionally scrolling the selection into view."

            "hasFocus" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if the editor has focus."

            "show" => T<unit> ^-> T<unit>
            |> WithComment "Shows the editor."

            "hide" => T<unit> ^-> T<unit>
            |> WithComment "Hides the editor."

            "isHidden" => T<unit> ^-> T<bool>
            |> WithComment "Returns true if the editor is hidden."

            // ---- commands
            "execCommand" => T<string> ^-> T<bool>
            |> WithComment "Executes a command. Returns true when it was handled."

            "execCommand" => T<string> * T<bool> * T<obj> ^-> T<bool>
            |> WithComment "Executes a command with the UI flag and a value."

            "queryCommandState" => T<string> ^-> T<bool>
            |> WithComment "Returns the state of a command (for example whether Bold is active)."

            "queryCommandValue" => T<string> ^-> T<string>
            |> WithComment "Returns the value of a command."

            "queryCommandSupported" => T<string> ^-> T<bool>
            |> WithComment "Returns true if the command is registered."

            "addCommand" => T<string> * (T<bool> * T<obj> ^-> T<unit>) ^-> T<unit>
            |> WithComment "Registers a command; the callback receives the UI flag and the (upstream-untyped) value."

            "addQueryStateHandler" => T<string> * (T<unit> ^-> T<bool>) ^-> T<unit>
            |> WithComment "Registers a state handler for a command."

            "addQueryValueHandler" => T<string> * (T<unit> ^-> T<string>) ^-> T<unit>
            |> WithComment "Registers a value handler for a command."

            "addShortcut" => T<string> * T<string> * (T<string> + (T<unit> ^-> T<unit>)) ^-> T<unit>
            |> WithComment "Registers a keyboard shortcut."

            // ---- options
            "getParamString" => T<string>?``name`` ^-> T<string>
            |> WithInline "$this.getParam($name, '', 'string')"
            |> WithComment "Reads a string option (deprecated upstream in favour of `editor.options.get`)."

            "getParamBool" => T<string>?``name`` ^-> T<bool>
            |> WithInline "$this.getParam($name, false, 'boolean')"

            "getParamNumber" => T<string>?``name`` ^-> T<int>
            |> WithInline "$this.getParam($name, 0, 'number')"

            "hasPlugin" => T<string> ^-> T<bool>
            |> WithComment "Returns true if the named plugin is loaded."

            // ---- accessibility and helpers
            "announce" => T<string> ^-> T<unit>
            |> WithComment "Announces a message to screen readers."

            "announceAssertive" => T<string>?``message`` ^-> T<unit>
            |> WithInline "$this.announce($message, { assertive: true })"

            "translate" => T<string> ^-> T<string>
            |> WithComment "Translates a UI string."

            "getContainer" => T<unit> ^-> T<HTMLElement>
            |> WithComment "Returns the editor container."

            "getContentAreaContainer" => T<unit> ^-> T<HTMLElement>
            |> WithComment "Returns the content area container."

            "getElement" => T<unit> ^-> T<HTMLElement>
            |> WithComment "Returns the target element."

            "getWin" => T<unit> ^-> T<Window>
            |> WithComment "Returns the editable area's window."

            "getDoc" => T<unit> ^-> T<Document>
            |> WithComment "Returns the editable area's document."

            "getBody" => T<unit> ^-> T<HTMLElement>
            |> WithComment "Returns the editable area's body element."

            // ---- events
            "on" => T<string> * (EventClass.Type ^-> T<unit>) ^-> T<unit>
            |> WithComment "Subscribes to an editor event by name."

            "once" => T<string> * (EventClass.Type ^-> T<unit>) ^-> T<unit>
            |> WithComment "Subscribes to the next occurrence of an editor event."

            "off" => T<string> ^-> T<unit>
            |> WithComment "Removes all listeners of an event."

            "off" => T<string> * (EventClass.Type ^-> T<unit>) ^-> T<unit>
            |> WithComment "Removes a single listener of an event."

            "fire" => T<string> ^-> EventClass.Type
            |> WithComment "Fires an editor event."

            "fire" => T<string> * mapType ^-> EventClass.Type

            "dispatch" => T<string> ^-> EventClass.Type
            |> WithComment "Dispatches an editor event to the listeners."

            "hasEventListeners" => T<string> ^-> T<bool>
            |> WithComment "Returns true if the event has listeners."

            // Typed convenience wrappers around `on(name, callback)`.
            "onChangeTyped" => ((ChangeEventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('change', $callback)"
            |> WithComment "Subscribes to `change` with a typed event."

            "onInitTyped" => ((EventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('init', $callback)"

            "onNodeChangeTyped" => ((NodeChangeEventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('NodeChange', $callback)"

            "onSelectionChangeTyped" => ((EventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('SelectionChange', $callback)"

            "onBeforeGetContentTyped" => ((ContentEventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('BeforeGetContent', $callback)"

            "onGetContentTyped" => ((ContentEventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('GetContent', $callback)"

            "onSetContentTyped" => ((SetContentEventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('SetContent', $callback)"

            "onExecCommandTyped" => ((ExecCommandEventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('ExecCommand', $callback)"

            "onFormatTyped" => ((FormatEventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('FormatApply', $callback)"

            "onUndoTyped" => ((UndoRedoEventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('Undo', $callback)"

            "onRedoTyped" => ((UndoRedoEventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('Redo', $callback)"

            "onSwitchModeTyped" => ((SwitchModeEventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('SwitchMode', $callback)"

            "onDirtyTyped" => ((EventClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('dirty', $callback)"
        ]

        // ------------------------------------------------------------------
        // The `tinymce` global (default import)
        // ------------------------------------------------------------------

        UserClass
        |+> Instance [
            "id" =? T<string> |> WithComment "The user id."
            "name" =? T<string> |> WithComment "The display name."
            "avatar" =? T<string> |> WithComment "The avatar URL."
        ]
        |> ignore

        TinyMCEClass
        |+> Static [
            "init" => EditorOptions ^-> T<Promise<_>>[!|EditorClass.Type]
            |> WithComment "Initializes editors from an options object. Returns all created editors."

            "get" => T<string> ^-> !?EditorClass.Type
            |> WithComment "Returns the editor with the given id, or null."

            "get" => T<int> ^-> !?EditorClass.Type

            "get" => T<unit> ^-> !|EditorClass.Type
            |> WithComment "Returns all editors (`tinymce.get()`)."

            "activeEditor" =? !?EditorClass.Type
            |> WithComment "The currently active editor, or null."

            "focusedEditor" =? !?EditorClass.Type
            |> WithComment "The editor that has focus, or null."

            "majorVersion" =? T<string>
            |> WithComment "The major version of the loaded TinyMCE."

            "minorVersion" =? T<string>
            |> WithComment "The minor version of the loaded TinyMCE."

            "releaseDate" =? T<string>
            |> WithComment "The release date of the loaded TinyMCE."

            "suffix" =? T<string>
            |> WithComment "The suffix used for loaded resources (`.min` in production builds)."

            "baseURL" =? T<string>
            |> WithComment "The base URL TinyMCE loads its resources from."

            "add" => EditorClass.Type ^-> EditorClass.Type
            |> WithComment "Adds an editor instance to the manager."

            "remove" => EditorClass.Type ^-> !?EditorClass.Type
            |> WithComment "Removes an editor instance from the manager."

            "remove" => T<unit> ^-> T<unit>
            |> WithComment "Removes every editor (`tinymce.remove()`)."

            "execCommand" => T<string> * T<bool> * T<obj> ^-> T<bool>
            |> WithComment "Executes a command on the active editor."

            "triggerSave" => T<unit> ^-> T<unit>
            |> WithComment "Saves the content of every editor back to its target element."

            "createEditor" => T<string> * EditorOptions ^-> EditorClass.Type
            |> WithComment "Creates an editor instance without initializing it."

            "overrideDefaults" => EditorOptions ^-> T<unit>
            |> WithComment "Overrides the default options used by `init`."

            "PluginManager" =? PluginManagerClass.Type
            |> WithComment "The plugin manager."

            "ScriptLoader" =? ScriptLoaderClass.Type
            |> WithComment "The script loader."

            "DOM" =? DomUtilsClass.Type
            |> WithComment "A DOM utility instance (`tinymce.DOM`)."

            "translate" => T<string> ^-> T<string>
            |> WithComment "Translates a UI string to the current language."

            "onAddEditorTyped" => ((EditorClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('AddEditor', $callback)"
            |> WithComment "Subscribes to `AddEditor`."

            "onRemoveEditorTyped" => ((EditorClass.Type ^-> T<unit>)?``callback``) ^-> T<unit>
            |> WithInline "$this.on('RemoveEditor', $callback)"
            |> WithComment "Subscribes to `RemoveEditor`."
        ]
        |> ImportDefault "tinymce"
        |> ignore

        // ------------------------------------------------------------------
        // Assembly
        // ------------------------------------------------------------------

        Assembly [
            Namespace ns [
                // options and their value types
                EditorOptions
                EntityEncoding
                Directionality
                ElementFormat
                ToolbarLocation
                ToolbarMode
                NewlineBehavior
                UIMode
                SchemaType
                NotificationType
                ContentFormat

                // editor and collaborators
                EditorClass
                ModeClass
                OptionsClass
                SelectionClass
                UndoManagerClass
                UndoLevelClass
                BookmarkClass
                FormatterClass
                FormatVarsClass
                FormatClass
                FormatChangeDataClass
                FormatChangedHandleClass
                CommandsClass
                ShortcutsClass
                UiClass
                RegistryClass
                DomUtilsClass

                // toolbar and menu specs
                ToolbarButtonApiClass
                ToolbarButtonSpecClass
                ToolbarToggleButtonApiClass
                ToolbarToggleButtonSpecClass
                MenuButtonApiClass
                MenuButtonSpecClass
                SplitButtonApiClass
                SplitButtonSpecClass
                GroupToolbarButtonSpecClass
                MenuItemApiClass
                MenuItemSpecClass
                ToggleMenuItemApiClass
                ToggleMenuItemSpecClass
                NestedMenuItemApiClass
                NestedMenuItemSpecClass
                ContextMenuApiClass
                SidebarApiClass
                SidebarSpecClass

                // notifications and dialogs
                NotificationSpecClass
                NotificationApiClass
                NotificationManagerClass
                NotificationProgress
                WindowManagerClass

                // plugins, uploads, DOM
                PluginClass
                PluginManagerClass
                ScriptLoaderClass
                BlobInfoClass
                UploadResultClass
                UserClass

                // events
                EventClass
                ChangeEventClass
                NodeChangeEventClass
                ContentEventClass
                SetContentEventClass
                ExecCommandEventClass
                FormatEventClass
                UndoRedoEventClass
                SwitchModeEventClass

                // the global
                TinyMCEClass
            ]
        ]