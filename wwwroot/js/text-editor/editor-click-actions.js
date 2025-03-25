/**
 * Handle click actions for the editor
 * @param {Editor} editor - The editor instance
 * @param {string} selector - The selector for the editor
 */
export function handleEditorClickActions(editor, selector) {
    const actions = [
        {
            id: `${selector} [data-hs-editor-bold]`,
            fn: () => editor.chain().focus().toggleBold().run(),
        },
        {
            id: `${selector} [data-hs-editor-italic]`,
            fn: () => editor.chain().focus().toggleItalic().run(),
        },
        {
            id: `${selector} [data-hs-editor-underline]`,
            fn: () => editor.chain().focus().toggleUnderline().run(),
        },
        {
            id: `${selector} [data-hs-editor-strike]`,
            fn: () => editor.chain().focus().toggleStrike().run(),
        },
        {
            id: `${selector} [data-hs-editor-link]`,
            fn: () => {
                const url = window.prompt("URL");
                editor
                    .chain()
                    .focus()
                    .extendMarkRange("link")
                    .setLink({ href: url })
                    .run();
            },
        },
        {
            id: `${selector} [data-hs-editor-ol]`,
            fn: () => editor.chain().focus().toggleOrderedList().run(),
        },
        {
            id: `${selector} [data-hs-editor-ul]`,
            fn: () => editor.chain().focus().toggleBulletList().run(),
        },
        {
            id: `${selector} [data-hs-editor-blockquote]`,
            fn: () => editor.chain().focus().toggleBlockquote().run(),
        },
        {
            id: `${selector} [data-hs-editor-code]`,
            fn: () => editor.chain().focus().toggleCode().run(),
        },
        // Add alignment actions
        {
            id: `${selector} [data-hs-editor-align-left]`,
            fn: () => editor.chain().focus().setTextAlign('left').run(),
        },
        {
            id: `${selector} [data-hs-editor-align-center]`,
            fn: () => editor.chain().focus().setTextAlign('center').run(),
        },
        {
            id: `${selector} [data-hs-editor-align-right]`,
            fn: () => editor.chain().focus().setTextAlign('right').run(),
        },
        {
            id: `${selector} [data-hs-editor-align-justify]`,
            fn: () => editor.chain().focus().setTextAlign('justify').run(),
        },
    ];

    // Handle click actions
    for (const { id, fn } of actions) {
        // Get the action element
        const action = document.querySelector(id);

        // If the action element is not found, continue to the next action
        if (action === null) continue;

        // Add an event listener to the action element
        action.addEventListener("click", fn);
    }
}

export default handleEditorClickActions;