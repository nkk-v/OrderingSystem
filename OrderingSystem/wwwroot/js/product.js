function loadProductForm(id) {
    let url = id ? `/product/Edit/${id}` : `/product/Create`;
    $('#productModalLabel').text(id ? 'Edit Product' : 'Add Product');

    $.get(url, function (data) {
        $('#productModalBody').html(data);
    });

    $('#productModal').on('shown.bs.modal', function () {
        if (!AutoNumeric.getAutoNumericElement('#PriceInput')) {
            new AutoNumeric('#PriceInput', {
                decimalPlaces: 2,
                digitGroupSeparator: ',',
                decimalCharacter: '.',
                unformatOnSubmit: true
            });
        }
    });
}

// Optional: reload page or table after form submission
$(document).on("submit", "#productForm", function (e) {
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
            $('#productModal').modal('hide');
            location.reload(); // Replace with a dynamic table refresh if needed
        },
        error: function () {
            alert("An error occurred while saving the category.");
        }
    });
});

function setProductDeleteId(Id) {
    document.getElementById("productDeleteId").value = Id;
}