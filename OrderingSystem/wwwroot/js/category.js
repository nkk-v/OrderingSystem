
    function loadCategoryForm(id) {
        let url = id ? `/Category/Edit/${id}` : `/Category/Create`;
    $('#categoryModalLabel').text(id ? 'Edit Category' : 'Add Category');

    $.get(url, function (data) {
        $('#categoryModalBody').html(data);
            });
        }

    // Optional: reload page or table after form submission
    $(document).on("submit", "#categoryForm", function (e) {
        e.preventDefault();

    var formData = new FormData(this);
    var actionUrl = $(this).attr("action");

    $.ajax({
        type: "POST",
    url: actionUrl,
    data: formData,
    processData: false,
    contentType: false,
    success: function () {
        $('#categoryModal').modal('hide');
    location.reload(); // Replace with a dynamic table refresh if needed
                },
    error: function () {
        alert("An error occurred while saving the category.");
                }
            });
    });


function setDeleteId(Id) {
    document.getElementById("deleteId").value = Id;
}