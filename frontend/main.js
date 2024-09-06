window.addEventListener('DOMContentLoaded', (event) => {
    getVisitCount();
});

const functionApi = 'http://localhost:7071/api/UpdateCount';

const getVisitCount = () => {
    fetch(functionApi)
        .then(response => response.json())
        .then(data => {
            console.log("Website called function API.");
            console.log("Count data received:", data);
            document.getElementById("counter").innerText = data.count; // Display the count
        })
        .catch(error => {
            console.error("Error fetching the count:", error);
        });
};
