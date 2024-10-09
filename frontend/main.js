window.addEventListener('DOMContentLoaded', (event) => {
    getVisitCount();
});

const functionAPIUrl = 'https://getresumecounterrk1.azurewebsites.net/api/UpdateCount?code=jiU09M8F0MBKMpANZ3piTAPeI1AM0hRErfzZmbgLk6LKAzFuEtgSYw%3D%3D'
const localFunctionApi = 'http://localhost:7071/api/UpdateCount';

const getVisitCount = () => {
    fetch(functionAPIUrl)
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
