import createEditor from "./tiptap-config";
import handleEditorClickActions from "./editor-click-actions";

export function initializeEditor() {
    // Initialize TipTap editor
    const editor = createEditor();
    if (editor) {
        // Initialize click actions with the editor instance
        handleEditorClickActions(editor, '#hs-editor-tiptap');
    }

    return editor;
}

export default initializeEditor;