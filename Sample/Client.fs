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
namespace WebSharper.TinyMceSample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.UI.Templating

#if TINYMCE6
open WebSharper.TinyMce6
#else
open WebSharper.TinyMce
#endif

/// The sample doubles as the binding's test case: it initializes an editor and
/// drives the options, the UI registry, the formatter, the command, selection
/// and undo APIs, notifications, dialogs and the editor lifecycle.
[<JavaScript>]
module Client =

    // The template is the whole document, loaded from the DOM, so editing
    // index.html and refreshing the browser is enough.
    type IndexTemplate = Template<"../Sample/index.html", ClientLoad.FromDocument>

    // ------------------------------------------------------------------
    // Theme (light by default, choice persisted)
    // ------------------------------------------------------------------

    let themeStorageKey = "websharper-tinymce-theme"
    let theme = Var.Create "light"
    let themeLabel = Var.Create "🌙 Dark mode"

    let applyTheme () =
        JS.Document.DocumentElement.SetAttribute("data-theme", theme.Value)
        themeLabel.Value <- if theme.Value = "dark" then "☀️ Light mode" else "🌙 Dark mode"

    let toggleTheme () =
        theme.Value <- (if theme.Value = "dark" then "light" else "dark")
        try JS.Window.LocalStorage.SetItem(themeStorageKey, theme.Value) with _ -> ()
        applyTheme ()

    // ------------------------------------------------------------------
    // Upstream line (the two samples share this file)
    // ------------------------------------------------------------------

#if TINYMCE6
    let upstreamLine = "6.x"
    let otherSampleUrl = "../"
    let otherSampleLabel = "8.x sample →"
#else
    let upstreamLine = "8.x"
    let otherSampleUrl = "v6/"
    let otherSampleLabel = "6.x sample →"
#endif

    // ------------------------------------------------------------------
    // Bound state
    // ------------------------------------------------------------------

    let versionLabel = Var.Create "…"
    let status = Var.Create "Waiting for the editor to initialize…"
    let preview = Var.Create ""
    let stats = Var.Create ""
    let dirtyLabel = Var.Create "clean"
    let formatState = Var.Create "no formats"
    let selectionInfo = Var.Create "no selection"
    let modeLabel = Var.Create "design"
    let log = Var.Create ""

    let mutable editor : Editor option = None
    let mutable stamps = 0

    let logLine (message: string) =
        log.Value <-
            if log.Value = "" then message
            else message + "\n" + log.Value

    let withEditor (action: Editor -> unit) =
        match editor with
        | Some ed -> action ed
        | None -> logLine "The editor is not ready yet."

    // ------------------------------------------------------------------
    // Readouts
    // ------------------------------------------------------------------

    let countWords (text: string) =
        text.Split([| ' '; '\n'; '\r'; '\t'; '\u00a0' |])
        |> Array.filter (fun w -> w.Length > 0)
        |> Array.length

    let refresh () =
        withEditor (fun ed ->
            let html = ed.GetContent()
            let text = ed.GetContent("text")
            preview.Value <- html
            stats.Value <- sprintf "%d characters · %d words" text.Length (countWords text)
            dirtyLabel.Value <- if ed.IsDirty() then "modified" else "clean"
            modeLabel.Value <- ed.Mode.Get()
            let active =
                [ if ed.Formatter.Match "bold" then yield "bold"
                  if ed.Formatter.Match "italic" then yield "italic"
                  if ed.Formatter.Match "underline" then yield "underline"
                  if ed.Formatter.Match "ws-highlight" then yield "highlight" ]
            formatState.Value <-
                if List.isEmpty active then "no formats" else String.concat ", " active
            if ed.Selection.IsCollapsed() then
                selectionInfo.Value <-
                    if ed.Selection.IsEditable() then "caret in the editable area"
                    else "caret outside the editable area"
            else
                selectionInfo.Value <-
                    sprintf "%d block(s) selected" (ed.Selection.GetSelectedBlocks().Length))

    let notify (kind: string) (text: string) =
        withEditor (fun ed ->
            let spec = NotificationSpec(text)
            spec.Type <-
                match kind with
                | "success" -> NotificationType.Success
                | "warning" -> NotificationType.Warning
                | "error" -> NotificationType.Error
                | _ -> NotificationType.Info
            let _ = ed.NotificationManager.Open spec
            logLine (sprintf "notification (%s): %s" kind text))

    // ------------------------------------------------------------------
    // Editor setup
    // ------------------------------------------------------------------

    /// A JS record; WebSharper records/maps are untyped upstream.
    [<Inline "({ 'class': 'tiny-highlight' })">]
    let highlightAttributes : obj = X<obj>

    let registerUi (ed: Editor) =
        // An SVG icon registered by name.
        ed.Ui.Registry.AddIcon(
            "ws-star",
            "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\"><path d=\"M12 2l2.9 6.3 6.9.7-5.1 4.6 1.4 6.8L12 17l-6.1 3.4 1.4-6.8L2.2 9l6.9-.7z\"/></svg>")

        // A toolbar button that runs the custom `wsInsertStamp` command.
        let stamp = ToolbarButtonSpec()
        stamp.Text <- "Stamp"
        stamp.Icon <- "ws-star"
        stamp.Tooltip <- "Insert a timestamp (custom command)"
        stamp.OnAction <-
            (fun _ ->
                let _ = ed.ExecCommand "wsInsertStamp"
                refresh ())
        ed.Ui.Registry.AddButton("wsstamp", stamp)

        // A toggle button synced with the custom format.
        let highlight = ToolbarToggleButtonSpec()
        highlight.Text <- "Mark"
        highlight.Tooltip <- "Highlight the selection (custom format)"
        highlight.OnSetup <-
            System.Func<_, _>(fun api ->
                let sync () = api.SetActive(ed.Formatter.Match "ws-highlight")
                ed.On("NodeChange", fun _ -> sync ())
                sync ()
                System.Action(fun () -> ed.Off "NodeChange"))
        highlight.OnAction <-
            (fun _ ->
                ed.Formatter.Toggle "ws-highlight"
                refresh ())
        ed.Ui.Registry.AddToggleButton("wsmark", highlight)

        // A menu button whose items are fetched on demand.
        let boxes = ToolbarMenuButtonSpec()
        boxes.Text <- "Insert"
        boxes.Tooltip <- "Insert a callout box"
        boxes.Fetch <-
            (fun success _ _ ->
                let item = MenuItemSpec()
                item.Type <- "menuitem"
                item.Text <- "Note box"
                item.OnAction <-
                    (fun _ ->
                        ed.InsertContent("<div class=\"note-box\"><strong>Note.</strong> Inserted from a menu button.</div>")
                        refresh ())
                success.Invoke([| item :> obj |]))
        ed.Ui.Registry.AddMenuButton("wsbox", boxes)

        // A context menu, wired through the `contextmenu` option.
        let menu = ContextMenuApi()
        menu.Update <-
            (fun _ ->
                let item = MenuItemSpec()
                item.Type <- "menuitem"
                item.Text <- "Highlight"
                item.Icon <- "ws-star"
                item.OnAction <- (fun _ -> ed.Formatter.Toggle "ws-highlight"; refresh ())
                [| item :> obj |])
        ed.Ui.Registry.AddContextMenu("wscontext", menu)

        // A custom command, a state handler and a shortcut.
        ed.AddCommand(
            "wsInsertStamp",
            fun _ ->
                stamps <- stamps + 1
                ed.InsertContent(sprintf "<em class=\"stamp\">stamp %d</em> " stamps)
                refresh ())
        ed.AddQueryStateHandler("wsInsertStamp", fun () -> false)
        ed.AddShortcut("ctrl+alt+s", "Insert a timestamp", "wsInsertStamp")

        logLine "registered: icon, toolbar button, toggle button, menu button, context menu, command, shortcut"

    let initEditor () =
        let target = JS.Document.QuerySelector("#editor-target") |> As<HTMLElement>
        let options = EditorOptions()
        options.Target <- target
        options.Height <- 420
        options.Plugins <-
            "lists link code table wordcount searchreplace preview visualblocks charmap fullscreen"
        options.Toolbar <-
            "undo redo | blocks | bold italic underline | wsmark | alignleft aligncenter alignright | bullist numlist | link table | wsbox wsstamp | code fullscreen"
        options.Menubar <- "edit insert format table tools"
        options.Toolbar_mode <- ToolbarMode.Wrap
        options.Statusbar <- true
        options.Promotion <- false
        options.Skin <- false          // the skin CSS is bundled by esbuild
        options.Suffix <- ""
        options.Content_css <- false
        options.Content_style <-
            "body { font-family: ui-sans-serif, system-ui, sans-serif; font-size: 15px; line-height: 1.6; color: #1e2430; padding: 12px; } .note-box { border-left: 3px solid #4f46e5; background: #eef2ff; padding: 8px 12px; margin: 8px 0; } .stamp { color: #4f46e5; font-style: normal; font-weight: 600; }"
        options.Placeholder <- "Write something…"
        options.Contextmenu <- "wscontext"
        options.Setup <- (fun ed -> registerUi ed)
        options.Init_instance_callback <-
            (fun ed ->
                // `editor.formatter` only exists once the editor is initialized.
                let highlight = Format()
                highlight.Inline <- "mark"
                highlight.Attributes <- highlightAttributes
                ed.Formatter.Register("ws-highlight", highlight)
                logLine "registered the custom ws-highlight format"
                status.Value <- "Editor ready.")
#if TINYMCE6
#else
        options.License_key <- "gpl"   // accept the GPL terms; a key selects the commercial license
#endif

        TinyMCE.Init(options).Then(fun editors ->
            if editors.Length > 0 then
                let ed = editors.[0]
                editor <- Some ed
                status.Value <- sprintf "Editor %s initialized." ed.Id
                logLine (sprintf "tinymce.init resolved with %d editor(s)" editors.Length)

                ed.OnChangeTyped(fun _ ->
                    refresh ()
                    logLine "change")
                ed.OnInitTyped(fun _ -> logLine "init")
                ed.OnSetContentTyped(fun e -> logLine (sprintf "SetContent (%d chars)" e.Content.Length))
                ed.OnExecCommandTyped(fun e -> logLine (sprintf "ExecCommand %s" e.Command))
                ed.OnUndoTyped(fun _ -> refresh (); logLine "undo")
                ed.OnRedoTyped(fun _ -> refresh (); logLine "redo")
                ed.OnSwitchModeTyped(fun e -> modeLabel.Value <- e.Mode; logLine (sprintf "mode: %s" e.Mode))
                ed.OnDirtyTyped(fun _ -> dirtyLabel.Value <- "modified")

                versionLabel.Value <- sprintf "%s.%s" TinyMCE.MajorVersion TinyMCE.MinorVersion
                refresh ())
        |> ignore

    // ------------------------------------------------------------------
    // Actions
    // ------------------------------------------------------------------

    let sampleContent =
        "<h2>WebSharper + TinyMCE</h2>"
        + "<p>This editor is a <strong>TinyMCE</strong> instance created from F# through a strongly typed binding. "
        + "Select some text and use the <em>Mark</em> button, the custom <em>Stamp</em> command, or the context menu.</p>"
        + "<ul><li>Options are typed properties (<code>init_instance_callback</code> included).</li>"
        + "<li>Toolbar buttons, menu buttons, icons and context menus come from <code>editor.ui.registry</code>.</li>"
        + "<li>Content, selection, commands, formats and undo are typed APIs.</li></ul>"

    let setSampleContent () =
        withEditor (fun ed ->
            ed.SetContent sampleContent
            ed.SetDirty false
            status.Value <- "Sample content set."
            refresh ())

    let insertContent () =
        withEditor (fun ed ->
            ed.InsertContent "<p><strong>Inserted</strong> at the caret.</p>"
            refresh ())

    let getContent () =
        withEditor (fun ed ->
            let text = ed.GetContent("text")
            notify "success" (sprintf "getContent(text): %d characters" text.Length))

    let resetContent () =
        withEditor (fun ed ->
            ed.ResetContent()
            refresh ())

    let toggleFormat (name: string) =
        withEditor (fun ed ->
            ed.Formatter.Toggle name
            refresh ())

    let execCommand (cmd: string) =
        withEditor (fun ed ->
            let handled = ed.ExecCommand cmd
            logLine (sprintf "execCommand %s → %b" cmd handled)
            status.Value <- sprintf "queryCommandState(%s) = %b" cmd (ed.QueryCommandState cmd)
            refresh ())

    let undo () =
        withEditor (fun ed ->
            ed.UndoManager.Undo() |> ignore
            logLine (sprintf "hasUndo=%b hasRedo=%b" (ed.UndoManager.HasUndo()) (ed.UndoManager.HasRedo()))
            refresh ())

    let redo () =
        withEditor (fun ed ->
            ed.UndoManager.Redo() |> ignore
            refresh ())

    let clearHistory () =
        withEditor (fun ed ->
            ed.UndoManager.Clear()
            logLine "undo history cleared")

    let toggleReadonly () =
        withEditor (fun ed ->
            let next = if ed.Mode.Get() = "readonly" then "design" else "readonly"
            ed.Mode.Set next
            modeLabel.Value <- next
            status.Value <- sprintf "mode.isReadOnly() = %b" (ed.Mode.IsReadOnly()))

    let focusEditor () =
        withEditor (fun ed ->
            ed.Focus()
            status.Value <- sprintf "hasFocus = %b" (ed.HasFocus()))

    let selectAll () =
        withEditor (fun ed ->
            ed.Selection.Select(ed.GetBody()) |> ignore
            refresh ())

    let replaceSelection () =
        withEditor (fun ed ->
            let selected = ed.Selection.GetContent()
            if selected.Length = 0 then
                logLine "nothing selected"
            else
                ed.Selection.SetContent(sprintf "<mark>%s</mark>" selected)
                refresh ())

    let showNotification (kind: string) () =
        notify kind (sprintf "A %s notification from the sample." kind)

    let showProgress () =
        withEditor (fun ed ->
            let spec = NotificationSpec("Uploading…")
            spec.ProgressBar <- true
            let n = ed.NotificationManager.Open spec
            let mutable step = 0
            let tick () =
                step <- step + 1
                n.ProgressBar.Value(step * 20)
                if step >= 5 then
                    n.Close()
                    logLine "notification closed"
            JS.Window.SetInterval(tick, 250) |> ignore)

    let alertDialog () =
        withEditor (fun ed ->
            ed.WindowManager.Alert "This dialog comes from editor.windowManager.")

    let saveToTarget () =
        TinyMCE.TriggerSave()
        logLine (sprintf "triggerSave: %d editor(s) saved" (TinyMCE.Get().Length))

    let removeEditor () =
        withEditor (fun ed ->
            ed.Remove()
            editor <- None
            status.Value <- "Editor removed."
            logLine (sprintf "editors left: %d" (TinyMCE.Get().Length)))

    let recreateEditor () =
        match editor with
        | Some ed ->
            ed.Remove()
            editor <- None
            initEditor ()
        | None -> initEditor ()

    let openOtherSample () =
        JS.Window.Location.Href <- otherSampleUrl

    // ------------------------------------------------------------------
    // Entry point
    // ------------------------------------------------------------------

    [<SPAEntryPoint>]
    let Main () =
        try
            let stored = JS.Window.LocalStorage.GetItem themeStorageKey
            if stored = "dark" || stored = "light" then theme.Value <- stored
        with _ -> ()
        applyTheme ()

        IndexTemplate()
            .themeLabel(themeLabel.V)
            .versionLabel(versionLabel.V)
            .upstreamLine(upstreamLine)
            .otherSampleLabel(otherSampleLabel)
            .status(status.V)
            .stats(stats.V)
            .dirtyLabel(dirtyLabel.V)
            .formatState(formatState.V)
            .selectionInfo(selectionInfo.V)
            .modeLabel(modeLabel.V)
            .preview(preview.V)
            .log(log.V)
            .ToggleTheme(fun _ -> toggleTheme ())
            .SetSampleContent(fun _ -> setSampleContent ())
            .InsertContent(fun _ -> insertContent ())
            .GetContent(fun _ -> getContent ())
            .ResetContent(fun _ -> resetContent ())
            .ToggleBold(fun _ -> toggleFormat "bold")
            .ToggleItalic(fun _ -> toggleFormat "italic")
            .ToggleUnderline(fun _ -> toggleFormat "underline")
            .Highlight(fun _ -> toggleFormat "ws-highlight")
            .AlignLeft(fun _ -> execCommand "JustifyLeft")
            .AlignCenter(fun _ -> execCommand "JustifyCenter")
            .Undo(fun _ -> undo ())
            .Redo(fun _ -> redo ())
            .ClearHistory(fun _ -> clearHistory ())
            .ToggleReadonly(fun _ -> toggleReadonly ())
            .FocusEditor(fun _ -> focusEditor ())
            .SelectAll(fun _ -> selectAll ())
            .ReplaceSelection(fun _ -> replaceSelection ())
            .InfoNotification(fun _ -> showNotification "info" ())
            .SuccessNotification(fun _ -> showNotification "success" ())
            .ProgressNotification(fun _ -> showProgress ())
            .AlertDialog(fun _ -> alertDialog ())
            .SaveToTarget(fun _ -> saveToTarget ())
            .RemoveEditor(fun _ -> removeEditor ())
            .RecreateEditor(fun _ -> recreateEditor ())
            .OpenOtherSample(fun _ -> openOtherSample ())
            .Bind()

        // The whole body is the template, so the editor is created only after
        // `.Bind()` has put the target textarea in the DOM.
        initEditor ()