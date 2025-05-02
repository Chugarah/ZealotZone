// Define the list of CSS color variable names to use for the gradient stops
const cssColorVariables = [
	"--color-alpha-accent",
	"--color-alpha-green",
	"--color-alpha-orange",
	"--color-alpha-red",
	"--color-alpha-yellow",
	"--color-alpha-blue-1",
	"--color-alpha-blue-dark",
	"--color-alpha-purple",
	"--color-alpha-dark-green", // Include dark variants if desired
	"--color-alpha-dark-orange",
	"--color-alpha-dark-yellow",
	"--color-alpha-dark-red",
	"--color-alpha-dark-purple",
	"--color-alpha-dark-accent",
];

/**
 * Applies a random linear gradient using CSS variables to elements
 * with the class 'gradient-generator'.
 *  Inspired by:
 * https://stackoverflow.com/questions/74169636/get-random-gradient-background-color
 * @returns {void}
 */
function applyRandomCssGradient() {
	const gradientGenerators = document.querySelectorAll(".element-gradient-random");

	if (!gradientGenerators) {
		console.warn("No elements found to apply gradient to.");
		return;
	}


	// Apply a random gradient to each element
	for (const element of gradientGenerators) {
        // Select two distinct random color variable names
        const color1 = cssColorVariables[Math.floor(Math.random() * cssColorVariables.length)];
        let color2 = cssColorVariables[Math.floor(Math.random() * cssColorVariables.length)];


        // We need to ensure no two colors are the same
        while (color1 === color2) {
			// If the two colors are the same, select a new color for the second variable
            color2 = cssColorVariables[Math.floor(Math.random() * cssColorVariables.length)];
        }	

        // Set the CSS variables on the element. The ::before pseudo-element uses these.
        element.style.setProperty('--gradient-color-1', `var(${color1})`);
        element.style.setProperty('--gradient-color-2', `var(${color2})`);
    }
}

// Export the function so it can be imported and used elsewhere (e.g., in site.js)
export { applyRandomCssGradient };