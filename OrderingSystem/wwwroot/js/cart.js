
$(document).ready(function () {
    $(".qty").each(function () {
        const input = $(this);
        const container = input.closest(".quantity-controls");
        const minusBtn = container.find(".minus");

        if (parseInt(input.val()) <= 1) {
            minusBtn.prop("disabled", true);
        } else {
            minusBtn.prop("disabled", false);
        }
    });

    $('.plus, .minus').on('click', function () {
        const button = $(this);
        const container = button.closest('.quantity-controls');
        const input = container.find('.qty');
        let quantity = parseInt(input.val());
        const id = container.data('id');
        const price = parseFloat(container.data('price'));

        if (button.hasClass('plus')) {
            quantity++;
        } else if (button.hasClass('minus') && quantity > 1) {
            quantity--;
        }

        //$('#loader').fadeIn();
        // Show spinner before fetching fee
        document.getElementById("spinner").style.display = "flex";

        $.ajax({
            url: '/Cart/UpdateQuantity',
            type: 'POST',
            data: {
                cartItemId: id,
                quantity: quantity
            },
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function () {
                input.val(quantity);
                const subtotal = price * quantity;

                const formattedSubtotal = subtotal.toLocaleString('en-PH', {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                });
                $(`#subtotal-${id}`).text(`₱${formattedSubtotal}`);
                updateCartTotal();
                // Disable minus if quantity is 1, otherwise enable
                const minusBtn = container.find(".minus");
                minusBtn.prop("disabled", quantity <= 1);
            },
            error: function () {
                alert('Failed to update quantity.');
            },
            complete: function () {

                //$('#loader').fadeOut();
                // Hide spinner after fee is fetched
                document.getElementById("spinner").style.display = "none";
            }
        });
    });

    function updateCartTotal() {
        let total = 0;

        $("[id^='subtotal-']").each(function () {
            const value = parseFloat($(this).text().replace(/[₱,]/g, '')) || 0;
            total += value;
        });


        const formatted = total.toLocaleString('en-PH', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });


        $("#overall-total").text(`${formatted}`);
    }
});

