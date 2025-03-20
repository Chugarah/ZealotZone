// Initialize Lucide icons
import { createIcons } from "lucide";
import { Menu, Plus, Sun, Moon } from "lucide";
import initializeEditor from "./text-editor/editor-init";


createIcons({
	icons: {
		Menu,
		Plus,
		Sun,
		Moon,
	},
});

// Theme Switcher
// https://preline.co/docs/dark-mode.html
// Import Preline's HSThemeSwitch
import HSThemeSwitch from "@preline/theme-switch";

document.addEventListener("DOMContentLoaded", () => {
	// Initialize theme switch
	HSThemeSwitch.autoInit();

	// Handle icon visibility function
	const updateIconVisibility = (theme) => {
		// Sun icons - show in light mode, hide in dark mode
		for (const icon of document.querySelectorAll(".hs-theme-icon-sun")) {
			icon.style.display = theme === "dark" ? "none" : "block";
		}

		// Moon icons - hide in light mode, show in dark mode
		for (const icon of document.querySelectorAll(".hs-theme-icon-moon")) {
			icon.style.display = theme === "dark" ? "block" : "none";
		}
	};

	// Listen for theme changes
	window.addEventListener("on-hs-appearance-change", (e) => {
		// Apply custom theme-related changes
		updateIconVisibility(e.detail);

		// Optional: trigger any animations or transitions
		document.documentElement.classList.add("theme-transition");
		setTimeout(() => {
			document.documentElement.classList.remove("theme-transition");
		}, 300); // Match your transition duration

		// Optional: dispatch a custom event for other components
		window.dispatchEvent(
			new CustomEvent("custom-theme-changed", {
				detail: { theme: e.detail },
			}),
		);
	});

	// Set initial icon state
	const currentTheme = localStorage.getItem("hs_theme") || "default";
	const isDark = document.documentElement.classList.contains("dark");
	updateIconVisibility(isDark ? "dark" : currentTheme);


    // Initialize the editor
    initializeEditor();
});

















  
 
