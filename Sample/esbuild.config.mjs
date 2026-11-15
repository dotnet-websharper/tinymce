import { readFileSync, writeFileSync, copyFileSync, mkdirSync } from 'fs'
import { resolve } from 'path'
import { createRequire } from 'module'

// The config is shared by both sample projects and runs with the project
// directory as the working directory, so `esbuild` and `tinymce` resolve from
// the project's node_modules.
const require = createRequire(resolve('package.json'))
const { build } = require('esbuild')

const name = process.env.WS_NAME || 'WebSharper.TinyMce.Sample'
const dist = process.env.WS_DIST || resolve('../dist')
const plugins = [
    'lists', 'link', 'code', 'table', 'wordcount', 'searchreplace',
    'preview', 'visualblocks', 'charmap', 'fullscreen'
]

// TinyMCE 6/8 are UMD bundles: the theme, model, icons and plugins are separate
// module-loader entry points that register themselves on the imported instance.
// The WebSharper-generated module (which imports `tinymce` itself) is bundled
// after them so everything shares one instance.
const entry = `
import tinymce from 'tinymce'
// TinyMCE's theme/model/icon/plugin files are UMD modules that register
// themselves through the *global* \`tinymce\`, which a bundler does not
// provide. Expose the imported instance, then load them on demand so the
// assignment happens before they run.
globalThis.tinymce = tinymce
;(async () => {
    await import('tinymce/themes/silver')
    await import('tinymce/models/dom')
    await import('tinymce/icons/default')
${plugins.map(p => `    await import('tinymce/plugins/${p}')`).join('\n')}
    await import('./build/${name}.js')
})()
`

await build({
    stdin: {
        contents: entry,
        resolveDir: resolve('.'),
        sourcefile: 'entry.js',
        loader: 'js'
    },
    bundle: true,
    minify: process.env.WS_MINIFY !== '0',
    format: 'iife',
    globalName: 'wsbundle',
    outfile: `${dist}/Scripts/${name}.min.js`,
    logLevel: 'info'
})

mkdirSync(`${dist}/Scripts`, { recursive: true })

// The UI skin ships with the npm package; the page links it directly. The
// editable area is styled with `content_style` instead (CSS imports land in the
// page, not in the editor iframe).
copyFileSync('./node_modules/tinymce/skins/ui/oxide/skin.css', `${dist}/tinymce-skin.css`)
copyFileSync('../Sample/site.css', `${dist}/site.css`)

// Fold the WebSharper head (module scripts) into the page.
const head = readFileSync(`./build/${name}.head.html`, 'utf8')
const licence = name.includes('6')
    ? 'TinyMCE 6.x is MIT-licensed &mdash; the last MIT-licensed TinyMCE line.'
    : 'TinyMCE 8.x is licensed under GPL-2.0-or-later, or a commercial Tiny Technologies license.'
const html = readFileSync('../Sample/index.html', 'utf8')
    .replace('<!-- ws-head -->', head)
    .replace('<!-- ws-bundle -->', `<script src="Scripts/${name}.min.js"></script>`)
    .replace('<!-- ws-licence -->', `<p class="licence-line">${licence}</p>`)
writeFileSync(`${dist}/index.html`, html)

console.log('Bundled:', name, '->', dist)