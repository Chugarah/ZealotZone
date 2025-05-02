// Adding TipTap
import { Editor } from "@tiptap/core";
import StarterKit from "@tiptap/starter-kit";
import Placeholder from "@tiptap/extension-placeholder";
import Underline from "@tiptap/extension-underline";
import Link from "@tiptap/extension-link";
import TextAlign from '@tiptap/extension-text-align';

export function createEditor() {
    const editor = new Editor({
        element: document.querySelector("#hs-editor-tiptap [data-hs-editor-field]"),
        editorProps: {
            attributes: {
                class: "relative form-input text-editor-input",
            },
        },
        extensions: [
            StarterKit.configure({
                history: false,
                heading: {
                    levels: [1, 2, 3],
                    HTMLAttributes: {
                        class: "text-inherit text-gray-800 dark:text-neutral-200",
                    },
                },
                paragraph: {
                    HTMLAttributes: {
                        class: "text-inherit text-gray-800 dark:text-neutral-200",
                    },
                },
                bold: {
                    HTMLAttributes: {
                        class: "font-bold",
                    },
                },
                bulletList: {
                    HTMLAttributes: {
                        class: "list-disc list-inside text-gray-800 dark:text-white",
                    },
                },
                orderedList: {
                    HTMLAttributes: {
                        class: "list-decimal list-inside text-gray-800 dark:text-white",
                    },
                },
                listItem: {
                    HTMLAttributes: {
                        class: "marker:text-sm",
                    },
                },
                blockquote: {
                    HTMLAttributes: {
                        class: "relative border-s-4 ps-4 sm:ps-6 dark:border-neutral-700 sm:[&>p]:text-lg",
                    },
                },
            }),
            TextAlign.configure({
                types: ['heading', 'paragraph'],
                alignments: ['left', 'center', 'right', 'justify'],
                defaultAlignment: 'left',
            }),
            Placeholder.configure({
                placeholder: "Type something...",
                emptyNodeClass: "before:text-gray-500 before:content-[attr(data-placeholder)] ",
            }),
            Underline,
            Link.configure({
                HTMLAttributes: {
                    class: "inline-flex items-center gap-x-1 text-blue-600 decoration-2 hover:underline focus:outline-hidden focus:underline font-medium dark:text-white",
                },
            }),
        ],
    });
    
    return editor;
}

export default createEditor;