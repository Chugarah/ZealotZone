/**
 * Sends an asynchronous HTTP request using the Fetch API.
 * Handles common scenarios like setting headers, sending body data,
 * and parsing JSON responses (both success and error).
 * Provides structured success and error objects.
 *
 * @async
 * @function sendAjaxRequest
 * @param {string} url - The URL endpoint for the request.
 * @param {string} method - The HTTP method (e.g., 'GET', 'POST', 'PUT', 'DELETE').
 * @param {object} [headers={}] - An object containing request headers. Defaults to an empty object.
 * @param {any} [body=null] - The request body. For 'GET' or 'HEAD' requests, this is ignored. Can be a string, FormData, Blob, etc. Defaults to null.
 * @returns {Promise<object>} A promise that resolves to an object indicating the outcome.
 *   - On success: `{ success: true, status: number, data: any }` where `data` is the parsed JSON response.
 *   - On server error (e.g., 500, 404): `{ success: false, status: number, message: string }`. Attempts to parse JSON error message.
 *   - On validation error (specifically 400 with an `errors` property in the JSON response): `{ success: false, status: 400, errors: object }`.
 *   - On authorization error (401): `{ success: false, status: 401, message: string }`. Attempts to parse JSON error message.
 *   - On response parsing error: `{ success: false, status: 'parsing_error', message: string, error: Error }`.
 *   - On network/fetch error: `{ success: false, status: 'network_error', message: string, error: Error }`.
 */
async function sendAjaxRequest(url, method, headers = {}, body = null) {
    try {
        // Add a small delay if needed (optional, kept from original code)
        await new Promise((r) => setTimeout(r, 1000)); 

        const options = {
            method: method.toUpperCase(),
            headers: headers,
        };


        if (body && !["GET", "HEAD"].includes(options.method)) {
            options.body = body;
        }

        const response = await fetch(url, options);

        if (!response.ok) {
            let errorData = null;
            let errorMessage = `Server error: ${response.status} ${response.statusText}`;

            try {
                // Try to parse JSON, common for structured errors
                errorData = await response.json();
            } catch (e) {
                // If JSON parsing fails, use the status text
                console.warn("Could not parse error response as JSON.", e);
            }

            // Handle specific error cases
            if (response.status === 400 && errorData?.errors) {
                // Specific handling for validation errors with expected structure
                return { success: false, status: 400, errors: errorData.errors };
            }
            // Handle authorization errors (401)
            if (response.status === 401) {
                errorMessage = errorData?.message || "Authorization failed."; // Use message from JSON if available
                return { success: false, status: 401, message: errorMessage };
            }
            // Handle other errors (e.g., 500, 404, or 400 without specific 'errors' structure)
            errorMessage = errorData?.message || errorMessage; 
            return { success: false, status: response.status, message: errorMessage };
        }
        // Handle successful response
        // Check if the response is JSON
        try {
            const result = await response.json();
            return { success: true, status: response.status, data: result };
            } catch (e) {
                console.error("Could not parse success response as JSON.", e);
                return {
                    success: false,
                    status: "parsing_error",
                    message: "Failed to parse successful response.",
                    error: e,
                };
            }
        
    // Handle network errors (e.g., fetch failed)
    } catch (error) { 
        console.error("AJAX request failed:", error);
        return {
            success: false,
            status: "network_error",
            message: "Could not connect to the server. Network error.",
            error: error,
        };
    }
}

export {
    sendAjaxRequest, // Export the new function
};