document.addEventListener("DOMContentLoaded", function () {
    const variantModal = document.getElementById("variantModal");
    const variantForm = document.getElementById("variantForm");
    const variantOptions = document.getElementById("variantOptions");
    const productIdInput = document.getElementById("productIdInput");

    // Attach click events to all variant buttons
    document.querySelectorAll('.view-variants-btn').forEach(btn => {
        btn.addEventListener('click', async function () {
            const productId = this.dataset.productId;

            try {
                const response = await fetch(`/Menu/GetProductVariants?productId=${productId}`);
                const variants = await response.json();

                productIdInput.value = productId;
                variantOptions.innerHTML = "";

                variants.forEach(variant => {
                    const html = `
                            <div class="card border-warning h-100">
                            <div class="card-header border-0 bg-transparent">
                                <div class="form-check">
                                <input class="form-check-input" type="radio" name="variantId" id="variant${variant.id}" value="${variant.id}" style="vertical-align: middle" required>
                                </div>
                            </div>
                                <div class="card-body text-center">
                                        <label class="form-check-label ms-2 card-title" for="variant${variant.id}">
                                         ${variant.variantName}
                                        </label>
                                    <div class="fw-bold fs-5 card-text">
                                        ₱${variant.price.toFixed(2)}
                                    </div>
                                </div>
                            </div>
                        `;
                    variantOptions.insertAdjacentHTML('beforeend', html);
                });


                const modal = new bootstrap.Modal(variantModal);
                modal.show();
            } catch (error) {
                console.error("Failed to load variants:", error);
            }
        });
    });

    variantForm.addEventListener("submit", function (e) {
        e.preventDefault();

        const formData = new FormData(this);

        fetch('/Cart/AddToCart', {
            method: 'POST',
            body: formData
        })
            .then(res => res.json())
            .then(response => {
                if (response.success) {
                    $('#variantModal').modal('hide');

                    fetch('/Cart/GetCartCount')
                        .then(res => res.text())
                        .then(html => {
                            document.getElementById("cart-count").innerHTML = html;
                        });
                } else if (response.redirectUrl) {
                    window.location.href = response.redirectUrl;
                } 
            })
            .catch(err => {
                console.error("Error adding to cart:", err);
                toastr.error("Something went wrong.");
            });
    });
});
