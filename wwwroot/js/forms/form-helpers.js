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

export { formatDatePicker, resetDatePickerYear };
