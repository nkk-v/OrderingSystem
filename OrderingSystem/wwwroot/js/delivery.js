
document.addEventListener("DOMContentLoaded", function () {
   
    const addressInput = document.getElementById("address");
    const currentAddress = document.getElementById("currentAddress");
    const toggleButton = document.getElementById("toogleAddressBtn");
    const deliveryFeeElement = document.getElementById("deliveryFee");
    const currentOption = document.getElementById("useCurrent");
    const manualOption = document.getElementById("enterManual")
    const paymentBtn = document.getElementById("proceedToPayment");
    const autocompleteList = document.getElementById("autocomplete-list");

    if (currentOption.checked) {
        getLocation();
        toggleButton.disabled = true;
        paymentBtn.disabled = true;
    };


    let selectedCoords = null;


    currentOption.addEventListener("change", () => {
        if (currentOption.checked) {
            toggleButton.disabled = true;
            addressInput.value = "";
            addressInput.setAttribute("readonly", true);
            toggleButton.innerText = "Change";
            paymentBtn.disabled = true;
            getLocation();
        }
    });

    manualOption.addEventListener("change", () => {
        if (manualOption.checked) {
            currentAddress.innerText = "";
            toggleButton.disabled = false;
            addressInput.focus();
            paymentBtn.disabled = true;
            updateDeliveryFeeAndTotal(0);
        }
    });


    function getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(success, handleGeoError, {
                timeout: 10000,
                maximumAge: 60000
            });
        }
        else {
            alert("Geolocation is not supported by your browser");
        }
    }


    async function success(position) {
        const { latitude, longitude } = position.coords;

        const url = `/api/delivery/reverse-geocode?lat=${latitude}&lon=${longitude}`;

        try {
            const res = await fetch(url);
            const data = await res.json();

            if (data.address) {

                document.getElementById("hiddenCurrentAddress").value = data.address;
                currentAddress.innerText = data.address;
                const fee = await fetchDeliveryFee(data.address);
                if (fee != null) updateDeliveryFeeAndTotal(fee);
                paymentBtn.disabled = false;
            }
        } catch {
            alert("Unable to fetch address. Please try manually.");
        }

    }

    function handleGeoError(error) {
        const message = {
            1: "Permission denied. Please enter your address manually.",
            2: "Location unavailable.",
            3: "Request timed out.",
        }[error.code] || "Unknown error occurred.";

        alert(message);
        manualOption.checked = true;
        addressInput.value = "";
        addressInput.focus();
        addressInput.removeAttribute("readonly");
        toggleButton.innerText = "Save";
        toggleButton.disabled = false;
        paymentBtn.disabled = true;
        updateDeliveryFeeAndTotal(0);
    }

    // Toggle readonly and handle address change
    toggleButton.addEventListener("click", async function () {
        const isReadOnly = addressInput.hasAttribute("readonly");

        if (isReadOnly) {
            addressInput.removeAttribute("readonly");
            addressInput.focus();
            toggleButton.innerText = "Save";
            paymentBtn.disabled = true;
        } else {
            if (addressInput.value.trim() === "") {
                alert("Address is required.");
                addressInput.focus();
                return;
            }

            addressInput.setAttribute("readonly", true);
            toggleButton.innerText = "Change";
            toggleButton.disabled = true;

            const address = addressInput.value;

            const fee = await fetchDeliveryFee(address)
            if (fee != null) {
                updateDeliveryFeeAndTotal(fee);
            }

            toggleButton.disabled = false;
            paymentBtn.disabled = false;
        }
    });

    // Autocomplete while typing
    addressInput.addEventListener("input", async () => {
        const query = addressInput.value;
        if (query.length < 3) {
            autocompleteList.innerHTML = "";
            return;
        }

        const url = `/api/delivery/autocomplete?query=${encodeURIComponent(query)}`;

        try {
            const res = await fetch(url);
            const data = await res.json();

            autocompleteList.innerHTML = "";

            if (data.features && data.features.length > 0) {
                data.features.forEach((feature) => {
                    const item = document.createElement("li");
                    item.className = "list-group-item list-group-item-action";
                    item.textContent = feature.properties.label;

                    item.addEventListener("click", async () => {
                        addressInput.value = feature.properties.label;
                        selectedCoords = {
                            lon: feature.geometry.coordinates[0],
                            lat: feature.geometry.coordinates[1]
                        };
                        autocompleteList.innerHTML = "";

                        // Show spinner before fetching fee
                        document.getElementById("spinner").style.display = "flex";

                        const fee = await fetchDeliveryFee(addressInput.value); 

                        // Hide spinner after fee is fetched
                        document.getElementById("spinner").style.display = "none";

                        if (fee != null) {
                            updateDeliveryFeeAndTotal(fee);
                        }

                        toggleButton.innerText = "Change";
                        addressInput.setAttribute("readonly", true);
                        paymentBtn.disabled = false;
                    });

                    autocompleteList.appendChild(item);
                });
            }
        } catch (error) {
            console.error("Autocomplete failed:", error);
        }
    });

    // Clear autocomplete when clicking outside
    document.addEventListener("click", function (e) {
        if (e.target !== addressInput) {
            autocompleteList.innerHTML = "";
        }
    });

    async function fetchDeliveryFee(address) {
        try {
            document.getElementById("spinner").style.display = "flex";

            const response = await fetch("/api/delivery/calculate-fee", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ address: address })
            });

            if (!response.ok) throw new Error("Invalid address or route");

            const data = await response.json();
            updateDeliveryFeeAndTotal(data.fee);

            return data.fee;
        } catch (error) {
            alert("Failed to calculate delivery fee. Try a more specific or nearby address.");
            console.error(error);
            return null;
        } finally {
            document.getElementById("spinner").style.display = "none";
            document.getElementById("deliveryFee").scrollIntoView({ behavior: "smooth" });
        }
    }
   
    function updateDeliveryFeeAndTotal(fee) {
        const subtotalElement = document.getElementById("subtotal");
        const totalElement = document.getElementById("total");

        if (deliveryFeeElement && subtotalElement && totalElement) {
            deliveryFeeElement.innerText = fee.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });

            const subtotal = parseFloat(subtotalElement.innerText.replace(/[,]/g, ""));
            const total = subtotal + fee;

            totalElement.innerText = total.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });

            document.getElementById("subtotalInput").value = subtotal.toFixed(2);
            document.getElementById("deliveryInput").value = fee.toFixed(2);
            document.getElementById("totalAmountInput").value = total.toFixed(2);
        }

    }

   
});



