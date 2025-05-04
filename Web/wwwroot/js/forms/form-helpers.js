import { getDate } from "vanilla-calendar-pro/utils";

/**
 * Form helpers
 * Got Assisted by Phind AI and documentation from Vanilla Calendar Pro
 * and Preline. This one displays the date in Swedish format in the input field
 * @returns {void}
 */

/**
 * Formats all date pickers on the page to display dates in a user-friendly format
 * This function handles multiple date pickers with the class "hs-datepicker"
 * @returns {void}
 */
function formatDatePicker() {
	const datePickers = document.querySelectorAll(".hs-datepicker");

	for (const inputElement of datePickers) {
		// Get the date picker element
		const { element } = HSDatepicker.getInstance(inputElement, true);

		// Listen for the change event
		element.on("change", (data) => {
			// If the date is selected, log the date
			if (data.selectedDates[0]) {
				// Get the date string
				const dateString = data.selectedDates[0];
				// Get the date object
				// https://vanilla-calendar.pro/docs/reference/utilities
				const dateObject = getDate(dateString);
				// Format the date
				const formattedDate = dateObject.toLocaleDateString("en-US", {
					month: "long",
					day: "numeric",
					year: "numeric",
				});

				// Set the value of the date picker
				inputElement.value = formattedDate;
			}
		});
	}
}

/**
 * Resets the datepicker year selector to show the current year
 */
function resetDatePickerYear() {
	const datePickers = document.querySelectorAll(".hs-datepicker");

	for (const inputElement of datePickers) {
		inputElement.addEventListener("click", (data) => {
			// Get the date picker element
			const { element } = HSDatepicker.getInstance(inputElement, true);

			if (element) {
				setTimeout(() => {
					console.log(element);
					const getDateSelector = inputElement.querySelector(
						".form-date-picker-years",
					);
					console.log(getDateSelector);
				}, 1000);
			}
		});
	}
}

// TODO: Implement File size limitation or Canvas/Cropping Library or both :D
function avatarUploader(config) {
	return {
		imageUrl: config.initialImageUrl || "",
		inputId: config.inputid || null,

		// Show preview of image
		showPreview(event) {
			// Get the selected file and the preview element
			const file = event.target.files[0];
			// Get the preview element
			const previewElement = this.$refs.preview;

			// Check if file is selected and preview element exists
			if (file && previewElement) {
				// Create a URL for the selected file
				const reader = new FileReader();
				reader.onload = (e) => {
					// Update the imageUrl property, which is bound to the :src of the img tag
					this.imageUrl = e.target.result;
				};
				// Read the file as a data URL
				reader.readAsDataURL(file);
			} else {
				// If no file is selected, reset the image to the initial URL
				this.imageUrl = config.initialImageUrl || "";
			}
		},
	};
}

/**
 * Alpine.js component logic for handling form validation via AJAX.
 * This code is based on the following webiste
 * Got assisted to finish and refactor this code using Google Gemini Pro
 * https://alpine-ajax.js.org/
 * https://github.com/imacrayon/alpine-ajax
 * https://technotrampoline.com/articles/building-an-ajax-form-with-alpinejs/
 * https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch
 * https://plbonneville.com/blog/sending-an-anti-forgery-token-with-asp.net-core-mvc-ajax-requests/
 * https://github.com/colinaut/alpinejs-plugin-simple-validate
 * https://fabform.io/a/alpinejs-validate/
 * https://www.w3schools.com/TAGS/att_input_pattern.asp
 * @param {object} config - Configuration object.
 * @param {object} config.initialData - Initial form data.
 * @param {string} config.validationEndpoint - URL for the validation endpoint.
 * @param {string} config.antiforgeryToken - The anti-forgery token.
 * @returns {object} Alpine.js component object.
 */

/**
 * This Generic function is based on the memberFormValidation function
 * and has been Refactored by Google Gemeni Pro to work with multiple types of forms
 * Alpine.js component logic for handling generic form validation via AJAX.
 * @param {object} config - Configuration object.
 * @param {string} config.formRefName - The x-ref name of the form element (e.g., 'addMemberForm', 'addProjectForm').
 * @param {object} config.initialData - Initial form data (key-value pairs matching input names).
 * @param {string} config.validationEndpoint - URL for the validation endpoint.
 * @param {string} config.antiforgeryToken - The anti-forgery token.
 * @param {object} [config.groups={}] - Optional: Definitions for field groups.
 *                                      Example: { groupName: { fields: [], allOrNoneMessage: '', validationCallback: function|null, unselectedValue: '' } }
 * @param {object} [config.validationCallbacks={}] - Optional: Container for custom validation callback functions used by groups.
 * @param {string[]} [config.fileInputNames=[]] - Optional: Array of names for file input fields.
 * @param {string} [config.unselectedValue="0"] - Optional: Default value representing an unselected state for inputs like selects.
 * @returns {object} Alpine.js component object.
 */
function ajaxFormValidation(config) {
	// --- Configuration Validation ---
	if (!config.formRefName)
		throw new Error("ajaxFormValidation requires config.formRefName.");
	if (!config.initialData)
		throw new Error("ajaxFormValidation requires config.initialData.");
	if (!config.validationEndpoint)
		throw new Error("ajaxFormValidation requires config.validationEndpoint.");
	if (!config.antiforgeryToken)
		throw new Error("ajaxFormValidation requires config.antiforgeryToken.");

	// --- Initialize formData dynamically ---
	const initialFormData = {};
	for (const key in config.initialData) {
		initialFormData[key] = config.initialData[key] ?? null;
	}

	return {
		// --- Core Properties ---
		formRefName: config.formRefName,
		formData: initialFormData,
		// Use provided groups or defaulted to empty
		groups: config.groups || {},
		// Use provided callbacks or default
		validationCallbacks: config.validationCallbacks || {},
		// Use provided file names or default
		fileInputNames: config.fileInputNames || [],
		// Default unselected value
		unselectedValue: config.unselectedValue ?? "0",
		validationEndpoint: config.validationEndpoint,
		antiforgeryToken: config.antiforgeryToken,
		errors: {},
		isSubmitting: false,
		// Keep if used elsewhere
		loadingFields: {},

		// --- Helper Methods ---
		// Refactored v1.4
		updateValidationUI(fieldName, errors, inputElement) {
			// Safety we are checking if we have an element or not
			if (!inputElement) {
				// This is for debugging purposes
				console.warn(
					`updateValidationUI: Input element for field "${fieldName}" not provided.`,
				);
				return;
			}

			const errorMsgId = inputElement.getAttribute("aria-describedby");

			/*
			 We will try to get the element by two ways; the first one is by aria-describedby
			 that normally is generated on checkboxes/radio
			 Second if the first option is to use standard query-selector,
			 We are using Document instead of an element because we don't want to lock in
			 search by children search
			*/
			const getErrorElement = errorMsgId
				? document.getElementById(errorMsgId)
				: document.querySelector(
						`[data-valmsg-for="${fieldName}"], #${fieldName}-error`,
					);
			const iconElement = this.$refs[`${fieldName}IconRef`];

			// Defining classes
			const errorClassInput = "input-validation-error";
			const validClassInput = "alpha-field-validation-valid";
			const errorClassMsg = "alpha-field-validation-invalid";



			// Define hasErrors based on errors parameters
			const hasErrors = errors && errors.length > 0;
			if (hasErrors) {
				// Adding class errors to input
				inputElement.classList.add(errorClassInput);
				inputElement.classList.add(errorClassMsg);
				inputElement.classList.remove(validClassInput);
				inputElement.setAttribute("aria-invalid", "true");

				// Update Error message
				if (getErrorElement) {
					getErrorElement.textContent = errors.join(" ");
					getErrorElement.classList.add(errorClassMsg);
					getErrorElement.style.display = "";
				}
			} else {
				inputElement.classList.remove(errorClassMsg);
				inputElement.classList.remove(errorClassInput);
				inputElement.classList.add(validClassInput);
				inputElement.setAttribute("aria-invalid", "false");

				// Hide and clear the error message element
				if (getErrorElement) {
					getErrorElement.textContent = "";
					getErrorElement.classList.remove(errorClassMsg);
					getErrorElement.classList.remove(errorClassInput);
				}
			}

			// This runs after the if/else block to set the final icon state
			if (iconElement) {
				iconElement.style.display = hasErrors ? "inline-block" : "none";
			}
		},

		// --- Validation Logic ---
		/**
		 * Validates a group of related form fields based on an "all or none" selection rule
		 * and optional custom validation logic.
		 *
		 * Dock Generated by Google Gemini Pro
		 * validateFieldGroup Helper function Was 100% generated by Google Gemini Pro
		 *
		 * Ensures that either all fields in the specified group have a selected value
		 * (not equal to `unselectedValue` and not an empty string after trimming) or none of them do.
		 * If all fields are selected and a `groupValidationLogic` function is provided,
		 * that function is executed to perform additional validation on the group's values.
		 * Updates the validation UI and the component's error state (`this.errors`) for all fields in the group.
		 *
		 * @param {string[]} fieldNames - An array of strings, where each string is the name of a field belonging to the group.
		 * @param {string} allOrNoneMessage - The error message to display if the "all or none" rule is violated (i.e., some but not all fields are selected).
		 * @param {function(Object.<string, any>): (string[]|null)} [groupValidationLogic] - An optional callback function for custom validation logic. This function is executed only if all fields in the group are selected. It receives an object containing the current values of the fields in the group (keyed by field name). It should return an array of error messages if validation fails, or `null` or an empty array if validation passes.
		 * @param {any} [unselectedValue=this.unselectedValue] - The value that represents an unselected state for a field within the group. Defaults to the component's `unselectedValue` property.
		 * @returns {boolean} Returns `true` if the field group passes validation (either all selected and custom logic passes, or none selected), `false` otherwise.
		 */
		validateFieldGroup(
			fieldNames,
			allOrNoneMessage,
			groupValidationLogic,
			unselectedValue = this.unselectedValue
		) {
			// Initialize variables
			let selectedCount = 0;
			const groupValues = {};
			let groupErrorMessages = null;
			let isGroupValid = true;

			// Count selected fields and gather values
			for (const fieldName of fieldNames) {
				const value = this.formData[fieldName];
				groupValues[fieldName] = value; // Store value for the callback
				if (value && value !== unselectedValue && String(value).trim() !== "") {
					selectedCount++;
				}
			}

			// 1. Check if all or none are selected
			const allSelected = selectedCount === fieldNames.length;
			const noneSelected = selectedCount === 0;

			// 2. Check the "all or none" rule
			if (!allSelected && !noneSelected) {
				isGroupValid = false;
				groupErrorMessages = [allOrNoneMessage];
			}

			// 3. If all selected and custom logic exists, run it
			else if (allSelected && typeof groupValidationLogic === "function") {
				// Call the validation logic function directly
				const validationResult = groupValidationLogic(groupValues);
				if (
					validationResult !== null &&
					Array.isArray(validationResult) &&
					validationResult.length > 0
				) {
					isGroupValid = false;
					groupErrorMessages = validationResult;
				}
			}


		//updateValidationUI(fieldName, errors, inputElement) 


			// 4. Update UI and error state for all fields in the group
			// TODO: THis need update because of Radio Boxes
			for (const fieldName of fieldNames) {
				// Getting the input element
				const inputElement = this.$refs[fieldName];
				
				this.updateValidationUI(
					fieldName,
					isGroupValid ? null : groupErrorMessages, inputElement,
				);

				// Update error state
				if (!isGroupValid) {
					this.errors[fieldName] = groupErrorMessages;
				} else {
					const currentError = this.errors[fieldName]?.[0];
					const potentialGroupError1 = allOrNoneMessage;
					if (this.errors[fieldName] && currentError === potentialGroupError1) {
						delete this.errors[fieldName];
					}
				}
			}
			return isGroupValid;
		},

		/**
		 * Validates a specific form field based on configured rules.
		 *
		 * Documentation generated by Google Gemini Pro
		 *
		 * This function checks if the field belongs to a validation group defined in `this.groups`.
		 * If it does, it delegates validation to `validateFieldGroup` using the group's configuration
		 * (fields, message, callback, unselectedValue).
		 *
		 * If the field does not belong to a group, it performs individual validation checks based on
		 * `data-val-*` attributes on the input element:
		 * - `data-val-required`: Checks if the field is required and not empty or equal to `this.unselectedValue`.
		 * - `data-val-minlength`, `data-val-minlength-min`: Checks for minimum length.
		 * - `data-val-length`, `data-val-length-max`: Checks for maximum length (alternative to maxlength).
		 * - `data-val-maxlength`, `data-val-maxlength-max`: Checks for maximum length.
		 * - `data-val-range`, `data-val-range-min`, `data-val-range-max`: Checks if a numeric value falls within a range.
		 * - `data-val-regex`, `data-val-regex-pattern`: Checks if the value matches a regular expression.
		 *
		 * Validation errors are stored in the `this.errors` object, keyed by the field name.
		 * The UI is updated via `this.updateValidationUI` to reflect the validation status.
		 *
		 * @param {string} fieldName - The name attribute of the form field to validate.
		 * @returns {boolean} True if the field is valid according to the applied rules, false otherwise.
		 */
		extendedValidator(fieldName) {
			// Getting the form and input element
			const form = this.$refs[this.formRefName];
			if (!form) return true;
			let inputElement = form.elements[fieldName];
			if (!inputElement) return true;

			// Radio/CheckBox Validation
			// Check if the result is a RadioNodeList (common for checkboxes/radios with the same name)
			if (inputElement instanceof RadioNodeList) {
				if (inputElement.length > 0) {
					// If it's a list, take the first element which should be the actual input
					inputElement = inputElement[0];
				} else {
					// If the list is empty, treat it as element not found
					inputElement = null;
				}
			}

			// Getting the input element data
			const inputValues =
				inputElement.type === "checkbox"
					? inputElement.checked
					: inputElement.value;
			const inputDataset = inputElement.dataset;
			const currentUnselectedValue = this.unselectedValue;

			let isFieldValid = true;
			let fieldBelongsToGroup = false;

			// --- Check if the field belongs to any defined group ---
			// that being loaded from the config
			for (const groupName in this.groups) {
				const group = this.groups[groupName];

				// If the field is part of the group
				if (group.fields.includes(fieldName)) {
					fieldBelongsToGroup = true;
					// Get the actual callback function if specified
					const callbackFn =
						typeof group.validationCallback === "string"
							? this.validationCallbacks[group.validationCallback] // Look up by name if string
							: group.validationCallback; // Assume it's the function itself

					// Delegate validation entirely to the group validator
					isFieldValid = this.validateFieldGroup(
						group.fields,
						group.allOrNoneMessage,
						callbackFn, // Pass the actual function
						// Use group-specific or default unselectedValue
						group.unselectedValue ?? currentUnselectedValue
					);
					break;
				}
			}

			// If the field does not belong to any group, perform individual validation checks
			if (!fieldBelongsToGroup) {
				const inputErrors = [];

				// === Individual Validation Rules ===
				// Required Check, now supports checkBoxes
				if (inputDataset?.valRequired) {
					if (inputElement.type === "checkbox" && !inputElement.checked) {
						inputErrors.push(inputDataset.valRequired);
					}
					// Not a checkbox, standard input
					else if (
						inputElement.type !== "checkbox" &&
						(!inputValues ||
							inputValues === currentUnselectedValue ||
							String(inputValues).trim() === "")
					) {
						inputErrors.push(inputDataset.valRequired);
					}
				}

				// MinLength Check
				if (
					inputDataset.valMinlength &&
					inputValues &&
					String(inputValues).trim() !== "" &&
					inputValues.length < Number.parseInt(inputDataset.valMinlengthMin, 10)
				) {
					inputErrors.push(inputDataset.valMinlength);
				}
				// MaxLength Check
				if (
					inputDataset.valLength &&
					inputValues.length > Number.parseInt(inputDataset.valLengthMax, 10)
				) {
					inputErrors.push(inputDataset.valLength);
				} else if (
					inputDataset.valMaxlength &&
					inputValues.length > Number.parseInt(inputDataset.valMaxlengthMax, 10)
				) {
					inputErrors.push(inputDataset.valMaxlength);
				}

				// Range Check
				// This code Range block is generated 100% using Google Gemini Pro
				if (
					inputDataset.valRange &&
					inputDataset.valRangeMin &&
					inputDataset.valRangeMax &&
					inputValues &&
					inputValues !== currentUnselectedValue &&
					String(inputValues).trim() !== ""
				) {
					const numValue = Number.parseFloat(inputValues);
					const min = Number.parseFloat(inputDataset.valRangeMin);
					const max = Number.parseFloat(inputDataset.valRangeMax);
					if (Number.isNaN(numValue) || numValue < min || numValue > max) {
						inputErrors.push(inputDataset.valRange);
					}
				}


                // Check if the field is a date picker
                if (inputDataset.valEqualto && inputDataset.valEqualtoOther) {
                    // Check if the field is a date picker
                    const otherFieldName = inputDataset.valEqualtoOther.replace('*.', '');
                    const otherFieldValue = this.formData[otherFieldName];

                    // Only compare if the current field has a value
                    if (inputValues && inputValues !== otherFieldValue) {
                        inputErrors.push(inputDataset.valEqualto);
                    }
                }


				// Regex Check
				if (inputDataset.valRegex && inputDataset.valRegexPattern) {
					const regex = new RegExp(inputDataset.valRegexPattern);
					if (inputValues && !regex.test(inputValues)) {
						inputErrors.push(inputDataset.valRegex);
					}
				}

				// --- Update UI and errors based on individual checks ---
				if (inputErrors.length > 0) {
					this.errors[fieldName] = inputErrors;
					this.updateValidationUI(fieldName, inputErrors, inputElement);
					isFieldValid = false;
				} else {
					if (this.errors[fieldName]) {
						delete this.errors[fieldName];
						this.updateValidationUI(fieldName, null, inputElement);
					}
					isFieldValid = true;
				}
			}
			return isFieldValid;
		},

		/**
		 * Dockmentation generated by Google Gemini Pro
		 * Validates all fields within the form's data object, excluding any fields listed in `this.fileInputNames`.
		 * It iterates through each field name in `this.formData`, calls `this.extendedValidator` for each non-file field,
		 * and aggregates the results to determine the overall validity of the form.
		 *
		 * @returns {boolean} True if all validated fields in the form are valid according to `extendedValidator`, false otherwise.
		 */
		validateEntireForm() {
			let isFormValid = true;
			// Iterate through each field in formData
			for (const fieldName of Object.keys(this.formData)) {
				// Skip file inputs if they exist in formData but shouldn't be validated here
				if (this.fileInputNames.includes(fieldName)) continue;

				// Validate each field and update the form's validity status
				const isFieldValid = this.extendedValidator(fieldName);
				isFormValid = isFieldValid && isFormValid;
			}
			// Note: The specific birthdate check block is removed as it's handled by groups now.
			return isFormValid;
		},

		/**
		 * Asynchronously validates a specific field after the next DOM update cycle.
		 * Dockmentation generated by Google Gemini Pro
		 * It schedules the `extendedValidator` method to run for the given field name
		 * once the Vue component has finished updating the DOM.
		 *
		 * @param {string} fieldName - The name of the field to be validated.
		 */
		async validateField(fieldName) {
			this.$nextTick(() => {
				this.extendedValidator(fieldName);
			});
		},

		/**
		 * Handles the asynchronous submission of the form associated with the component.
		 * Dockmentation generated by Google Gemini Pro
		 *
		 * Performs the following steps:
		 * 1. Prevents submission if already submitting (`this.isSubmitting`).
		 * 2. Performs client-side validation using `this.validateEntireForm()`.
		 * 3. If client-side validation fails, logs an error, focuses the first invalid field, and stops.
		 * 4. Sets `this.isSubmitting` to true and clears existing `this.errors`.
		 * 5. Resets validation UI for all fields.
		 * 6. Creates a `FormData` object (`payLoad`).
		 * 7. Appends non-file fields from `this.formData` to `payLoad`, skipping empty values.
		 * 8. Appends files from file inputs specified in `this.fileInputNames` to `payLoad`.
		 * 9. Sends the `payLoad` to `this.validationEndpoint` using a POST request via `fetch`.
		 * 10. Includes the `RequestVerificationToken` header using `this.antiforgeryToken`.
		 * 11. Handles the server response:
		 *     - If the response is not OK (e.g., status 400), attempts to parse JSON error data.
		 *     - Updates `this.errors` and the validation UI based on server-returned errors.
		 *     - Displays generic error messages for non-400 errors or unknown validation issues.
		 *     - If the response is OK, parses the JSON result and logs success. (Success handling like redirection might be implemented here).
		 * 12. Catches network or other fetch errors, logs them, and updates the UI with a generic submission error.
		 * 13. Finally, sets `this.isSubmitting` back to false.
		 *
		 * Relies on component properties: `isSubmitting`, `errors`, `formRefName`, `formData`,
		 * `fileInputNames`, `validationEndpoint`, `antiforgeryToken`.
		 * Relies on component methods: `validateEntireForm`, `updateValidationUI`.
		 *
		 * @async
		 * @function submitForm
		 * @returns {Promise<void>} A promise that resolves when the submission process (including handling the response) is complete.
		 */
		async submitForm() {
			if (this.isSubmitting) return;

			// Client-side validation
			if (!this.validateEntireForm()) {
				console.log("Client-side validation failed. Submission stopped.");
				const firstErrorField = Object.keys(this.errors)[0];
				const formElement = this.$refs[this.formRefName];
				// Focus on the first invalid field
				if (firstErrorField && formElement?.elements[firstErrorField]) {
					try {
						formElement.elements[firstErrorField].focus();
					} catch (e) {
						console.warn(`Could not focus on field: ${firstErrorField}`, e);
					}
				}
				return;
			}


		

			// Prepare for submission, clear UI
			this.isSubmitting = true;
			this.errors = {};
			for (const fieldName of Object.keys(this.formData)) {
				// Find the element within the form using querySelector by name attribute
				const inputElement = document.querySelector(`[name="${fieldName}"]`);
				this.updateValidationUI(fieldName, null, inputElement);
			}

			// Prepare payload and getting form data
			const payLoad = new FormData();
			const formElement = this.$refs[this.formRefName];

			// Append standard form data
			for (const key in this.formData) {
				// Skip file inputs here, handle them separately
				if (!this.fileInputNames.includes(key)) {
					const value = this.formData[key];
					if (
						value !== null &&
						value !== undefined &&
						String(value).trim() !== ""
					) {
						payLoad.append(key, value);
					}
				}
			}

			// Append file uploads based on config
			// === File Upload ===
			for (const fileName of this.fileInputNames) {
				const fileInput = formElement.elements[fileName];
				if (fileInput?.files?.length > 0) {
					payLoad.append(fileName, fileInput.files[0]);
				}
			}

			// Submit the form
			try {
				await new Promise((r) => setTimeout(r, 3000));
				const ajaxResponse = await fetch(this.validationEndpoint, {
					// Corrected variable name
					method: "POST",
					headers: {
						RequestVerificationToken: this.antiforgeryToken,
					},
					body: payLoad,
				});

				// Handle response
				if (!ajaxResponse.ok) {
					// Corrected variable name
					if (ajaxResponse.status === 400) {
						const errorData = await ajaxResponse.json();

						// Check if the expected error structure exists
						if (errorData?.errors) {
							// Iterate over the error data
							const serverErrors = errorData.errors;
							const generalErrorMessages = [];

							// Clear previous errors on submission
							this.errors = {};

							// Iterate over the server errors
							for (const key in serverErrors) {
								if (key === "") {
									// If it's a general error (key is empty string), add it to our temporary array
									// Got assisted by Google Gemini Pro
									generalErrorMessages.push(...serverErrors[key]);
								} else {
									// If it's a field-specific error:
									// a) Assign it to the component's errors object under the field name
									this.errors[key] = serverErrors[key];
									// b) Update the UI specifically for that field

									// Find the element within the form using querySelector by name attribute
									const inputElement = document.querySelector(`[name="${key}"]`);
									this.updateValidationUI(key, serverErrors[key], inputElement);
								}
							}

							// If there are general error messages, update the UI for the form
							if (generalErrorMessages.length > 0) {
								this.errors[this.formRefName] = generalErrorMessages;
							}

							console.log(
								"Server-side validation errors:",
								generalErrorMessages,
							);
						} else {
							// If the error structure is not as expected, handle it generically
							this.errors = {};
							this.errors[this.formRefName] = [
								"An unexpected validation error occurred (invalid format).",
							];
							console.error("Unexpected 400 error format:", errorData);
						}
					} else {
						// Handle other non-400 server errors (e.g., 500 Internal Server Error)
						// Default to a generic error message
                        let errorMessage = `Server error: ${ajaxResponse.status} ${ajaxResponse.statusText}`; // Default message

                        // Check if the error is a 401 Unauthorized
                        if (ajaxResponse.status === 401) {
                            try {
                                // Attempt to parse the response body as JSON
								// This is a common format for error messages
                                const errorData = await ajaxResponse.json(); 
 								errorMessage = errorData.message;
                            } catch (e) {
                                // Use a generic 401 message
                                errorMessage = "Login failed: Invalid credentials.";
                            }
                        }
                        // For other errors (like 500), the default errorMessage is already set
                        this.errors = {};
                        this.errors[this.formRefName] = [errorMessage];
					}
				} else {
					// Handle successful response
					const result = await ajaxResponse.json();
					console.log("Form submitted successfully!", result);
					// Dispatch success event or handle success directly
					this.$dispatch("form-success", { response: result });
					 window.location.replace(result.redirectUrl);
				}
			} catch (error) {
				console.error("Form submission error:", error);
				this.updateValidationUI(this.formRefName, [
					"Could not submit form. Network or server error.",
				]);
			} finally {
				this.isSubmitting = false;
			}
		},
	};
}

/**
 * Validates a birth date provided as separate day, month, and year values.
 * Intended for use as a validation callback within ajaxFormValidation groups.
 * Documentation Generated by Google Gemini Pro
 * Code block refactored by Google Gemini Pro
 *
 * Checks if the day, month, and year are valid numbers, form a valid calendar date,
 * and fall within a reasonable range (Year >= 1900 and Year <= current year).
 *
 * @param {Object.<string, string>} values - An object containing the form values, expected to have properties 'BirthDate.Day', 'BirthDate.Month', and 'BirthDate.Year' as strings.
 * @returns {string[] | null} An array containing error messages if validation fails, otherwise null.
 */
function birthDateValidationCallback(values) {
	const dayField = "BirthDate.Day";
	const monthField = "BirthDate.Month";
	const yearField = "BirthDate.Year";
	const dayNumber = Number.parseInt(values[dayField], 10);
	const monthNumber = Number.parseInt(values[monthField], 10);
	const yearNumber = Number.parseInt(values[yearField], 10);
	const currentYear = new Date().getFullYear();

	// Validate the actual date using the Date object.
	if (
		Number.isNaN(dayNumber) ||
		Number.isNaN(monthNumber) ||
		Number.isNaN(yearNumber)
	) {
		return ["Invalid numeric value for Day, Month, or Year."];
	}
	// JavaScript months are 0-indexed (0-11).
	// https://stackoverflow.com/questions/2552483/why-does-the-month-argument-range-from-0-to-11-in-javascripts-date-constructor
	// https://stackoverflow.com/questions/36179804/javascript-date-allows-invalid-data-e-g-feb-30th
	// https://stackoverflow.com/questions/15799514/why-does-javascript-getmonth-count-from-0-and-getdate-count-from-1/68452856
	const dateObj = new Date(yearNumber, monthNumber - 1, dayNumber);
	if (
		dateObj.getFullYear() !== yearNumber ||
		dateObj.getMonth() !== monthNumber - 1 ||
		dateObj.getDate() !== dayNumber ||
		yearNumber < 1900 ||
		yearNumber > currentYear
	) {
		return ["The selected Day, Month, and Year do not form a valid date."];
	}
	return null;
}


/**
 * Validates if the Password and ConfirmPassword fields match.
 * Intended for use as a validation callback within ajaxFormValidation groups.
 *
 * @param {Object.<string, string>} values - An object containing the form values,
 *                                           expected to have 'Password' and 'ConfirmPassword'.
 * @returns {string[] | null} An array containing an error message if passwords don't match, otherwise null.
 */
function passwordComparisonCallback(values) {
    const password = values.Password;
    const confirmPassword = values.ConfirmPassword;

    // Only compare if both fields have some value (basic required validation should be separate)
    if (password && confirmPassword && password !== confirmPassword) {
        // You can customize this message or potentially get it from a data attribute if needed
        return ["The passwords do not match."];
    }

    // Passwords match or one/both are empty (let required validation handle empty cases)
    return null;
}

export {
	formatDatePicker,
	resetDatePickerYear,
	avatarUploader,
	ajaxFormValidation,
	birthDateValidationCallback,
};
