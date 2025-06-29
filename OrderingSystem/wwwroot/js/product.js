function loadProductForm(id) {
    let url = id ? `/product/Edit/${id}` : `/product/Create`;
    $('#productModalLabel').text(id ? 'Edit Product' : 'Add Product');

    $.get(url, function (data) {
        $('#productModalBody').html(data);
        initProductFormScripts();
    });

    // Reinitialize form scripts after modal load

    //$('#productModal').on('shown.bs.modal', function () {
    //    if (!AutoNumeric.getAutoNumericElement('#PriceInput')) {
    //        new AutoNumeric('#PriceInput', {
    //            decimalPlaces: 2,
    //            digitGroupSeparator: ',',
    //            decimalCharacter: '.',
    //            unformatOnSubmit: true
    //        });
    //    }
    //});
}

function initProductFormScripts() {
    if (!AutoNumeric.getAutoNumericElement('#variantPriceInput')) {
        new AutoNumeric('#variantPriceInput', {
            decimalPlaces: 2,
            digitGroupSeparator: ',',
            decimalCharacter: '.',
            unformatOnSubmit: true
        });
    }

    let variantIndex = $('#variantList .variant-item').length;

    // Wrap all variant items inside a single box
    if (!$('#variantBox').length) {
        // Move existing variants inside variantList into variantBox
        $('#variantList').wrapInner('<div id="variantBox" class="border rounded p-3 bg-light mb-3"></div>');
    }

    $('#addVariantBtn').off('click').on('click', function () {
        const name = $('#variantNameInput').val().trim();
        const price = $('#variantPriceInput').val().trim();
        const description = $('#variantDescInput').val().trim();

        if (name === '' || price === '') {
            alert('Please enter both name and price.');
            return;
        }

        const html = `
            <div class="variant-item row g-2 align-items-end mb-2">
                <input type="hidden" name="Variants[${variantIndex}].Id" value="0" />
                <div class="col-md-5">
                    <input name="Variants[${variantIndex}].VariantName" class="form-control" value="${name}" readonly />
                </div>
                <div class="col-md-4">
                    <input name="Variants[${variantIndex}].Description" class="form-control" value="${description}" readonly />
                </div>
                <div class="col-md-4">
                    <input name="Variants[${variantIndex}].Price" class="form-control" value="${price}" readonly />
                </div>
                <div class="col-md-3 text-end">
                    <button type="button" class="btn btn-danger btn-sm remove-variant"><i class="bi bi-trash3"></i></button>
                </div>
            </div>
        `;

        $('#variantBox').append(html);
        variantIndex++;

        $('#variantNameInput').val(''); // Clear only name input
        // Do not clear price input
    });

    $(document).off('click', '.remove-variant').on('click', '.remove-variant', function () {
        //$(this).closest('.variant-item').remove();
        //reindexVariants();

        const $variantItem = $(this).closest('.variant-item');
        const variantId = $variantItem.find('input[name$=".Id"]').val();

        if (variantId && parseInt(variantId) !== 0) {
            // Track the removed variant ID
            $('#removedVariantContainer').append(`<input type="hidden" name="RemovedVariantIds" value="${variantId}" />`);
        }

        $variantItem.remove();
        reindexVariants();
    });

    function reindexVariants() {
        $('#variantBox .variant-item').each(function (i, el) {
            $(el).find('input').each(function () {
                const name = $(this).attr('name');
                if (name) {
                    const newName = name.replace(/\Variants\[\d+\]/, `Variants[${i}]`);
                    $(this).attr('name', newName);
                }
            });
        });
        variantIndex = $('#variantBox .variant-item').length;
    }
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
            alert("An error occurred while saving the product.");
        }
    });
});

function setProductDeleteId(Id) {
    document.getElementById("productDeleteId").value = Id;
}
