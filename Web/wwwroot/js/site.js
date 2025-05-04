// Instead, reference the global objects directly
// These are already available from your script tags
const Calendar = window.Calendar || window.VanillaCalendarPro?.Calendar;
const _ = window._;

// Import other dependencies
// import _ from 'lodash';
window._ = _;

// Make Calendar globally available with the correct reference
window.Calendar = Calendar;

// Initialize Lucide icons
// https://lucide.dev/icons/
import {
	Menu,
	Plus,
	Sun,
	Moon,
	CalendarDays,
	DollarSign,
	Clock,
	Pencil,
	UserPlus,
	Trash2,
	Ellipsis,
	LogOut,
	Settings,
	Bell,
	X,
	OctagonAlert,
	Camera,
	PenLine,
	BriefcaseBusiness,
} from "lucide";
import { createIcons, icons } from "lucide";
import initializeEditor from "./text-editor/editor-init";

// Imporitng AlpineJS
import Alpine from "alpinejs";
window.Alpine = Alpine;
// You can import your own modules to be bundled
import {
	formatDatePicker,
	resetDatePickerYear,
	avatarUploader,
	ajaxFormValidation,
	birthDateValidationCallback,
} from "./forms/form-helpers";
import {
	sendAjaxRequest,
} from "./components/fetch-ajax";
import { applyRandomCssGradient } from "./components/random-gradient.js";

// Make formatDatePicker available globally if needed
window.formatDatePicker = formatDatePicker;
window.resetDatePickerYear = resetDatePickerYear;
window.avatarUploader = avatarUploader;
window.ajaxFormValidation = ajaxFormValidation;
window.birthDateValidationCallback = birthDateValidationCallback; 
window.sendAjaxRequest = sendAjaxRequest;


// Initialize icons
createIcons({
	icons: {
		Menu,
		Plus,
		Sun,
		Moon,
		CalendarDays,
		DollarSign,
		Clock,
		Pencil,
		UserPlus,
		Trash2,
		Ellipsis,
		LogOut,
		Settings,
		Bell,
		X,
		OctagonAlert,
		Camera,
		PenLine,
		BriefcaseBusiness,
	},
});

// Theme Switcher
// https://preline.co/docs/dark-mode.html
document.addEventListener("DOMContentLoaded", () => {
	// Initialize Preline components - using the global object
	if (typeof window.HSStaticMethods !== "undefined") {
		window.HSStaticMethods.autoInit();
	}

	// --- Register Alpine Component ---
	// https://github.com/colinaut/alpinejs-plugin-simple-validate?tab=readme-ov-file
	document.addEventListener("alpine:init", () => {
		Alpine.data("avatarUpload", avatarUploader);
        Alpine.data("ajaxFormValidation", ajaxFormValidation);
		console.log("Alpine initialized, registering component."); // Debug log
	});
	Alpine.start();

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

	// Format the date picker
	resetDatePickerYear();
	formatDatePicker();

	// Initialize the editor
	initializeEditor();

	// Apply random gradients to designated elements
	applyRandomCssGradient();
	console.log("Site JS loaded and avatars generated");
});



