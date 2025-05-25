// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const searchInput = document.getElementById("searchBox");
if (searchInput) {
    searchInput.addEventListener("keyup", function () {
        const query = this.value.toLowerCase();
        const rows = document.querySelectorAll("#usersTable tbody tr");

        rows.forEach(row => {
            const email = row.children[0].innerText.toLowerCase();
            row.style.display = email.includes(query) ? "" : "none";
        });
    });
}
