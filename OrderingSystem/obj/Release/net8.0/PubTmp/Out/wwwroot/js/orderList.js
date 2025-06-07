document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".view-items-btn").forEach(button => {
        button.addEventListener("click", async function () {
            const orderId = this.dataset.orderId;
            const orderNum = this.dataset.orderNum;
            const listElement = document.getElementById("orderItemsList");
            const orderNumberText = document.getElementById("orderNumberText");

            orderNumberText.textContent = orderNum;
            listElement.innerHTML = `<li class="list-group-item text-muted">Loading items...</li>`;

            try {
                const response = await fetch(`/Order/GetOrderItems?orderId=${orderId}`);
                const items = await response.json();

                if (!items.length) {
                    listElement.innerHTML = `<li class="list-group-item text-danger">No items found for this order.</li>`;
                    return;
                }

                listElement.innerHTML = "";

                items.forEach(item => {
                    const li = document.createElement("li");
                    li.className = "list-group-item d-flex justify-content-between align-items-center";
                    li.innerHTML = `
                            <span>${item.quantity} × ${item.productName}${item.variantName ? ` (${item.variantName})` : ''}</span>
                            <span>₱ ${(item.price * item.quantity).toFixed(2)}</span>
                        `;
                    listElement.appendChild(li);
                });
            } catch (err) {
                listElement.innerHTML = `<li class="list-group-item text-danger">Failed to load order items.</li>`;
                console.error(err);
            }
        });
    });
});