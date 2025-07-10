
document.addEventListener("DOMContentLoaded", function () {
   
    const addressInput = document.getElementById("address");
    const currentAddress = document.getElementById("currentAddress");
    const toggleButton = document.getElementById("toogleAddressBtn");
    const deliveryFeeElement = document.getElementById("deliveryFee");
    const currentOption = document.getElementById("useCurrent");
    const manualOption = document.getElementById("enterManual")
    const paymentBtn = document.getElementById("proceedToPayment");
    const autocompleteList = document.getElementById("autocomplete-list");
    const itemCount = parseInt(document.getElementById("itemCount").value) || 0;
    
    let duration = 0;

    if (currentOption.checked) {

        toggleButton.disabled = true;
        getLocation();
    };


    let selectedCoords = null;


    currentOption.addEventListener("change", () => {
        if (currentOption.checked) {
            toggleButton.disabled = true;
            addressInput.value = "";
            addressInput.setAttribute("readonly", true);
            toggleButton.innerText = "Change";
            //paymentBtn.disabled = true;
            getLocation();
        }
    });

    manualOption.addEventListener("change", () => {
        if (manualOption.checked) {
            currentAddress.innerText = "";
            document.getElementById("hiddenCurrentAddress").value = "";
            addressInput.removeAttribute("readonly");
            toggleButton.innerText = "Save";
            toggleButton.disabled = false;
            addressInput.focus();
            //paymentBtn.disabled = true;
            updateDeliveryFeeAndTotal(0);
        }
    });


    function getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(success, handleGeoError);
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
        //paymentBtn.disabled = true;
        updateDeliveryFeeAndTotal(0);
    }

    // Toggle readonly and handle address change
    toggleButton.addEventListener("click", async function () {
        const isReadOnly = addressInput.hasAttribute("readonly");

        if (isReadOnly) {
            addressInput.removeAttribute("readonly");
            addressInput.focus();
            toggleButton.innerText = "Save";
            //paymentBtn.disabled = true;
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
                        //paymentBtn.disabled = false;
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
                body: JSON.stringify({ address: address, itemCount: itemCount })
            });

            if (!response.ok) {
                const errorText = await response.text();
                //alert(errorText.includes("15km") ? errorText : "Invalid address or route.");
                alert(errorText);
                paymentBtn.disabled = true;
                toggleButton.disabled = false
                document.getElementById("eta").innerText = "";
                return null;
            }

            const data = await response.json();
            duration = data.totalMinutes;
            updateDeliveryFeeAndTotal(data.deliveryFee);

            if (document.getElementById("deliverLater")?.checked) {
                suggestNextAvailableSchedule(data.totalMinutes);
                setupTimePicker(data.totalMinutes);
            }

            paymentBtn.disabled = false;
            return data.deliveryFee;
        } catch (error) {
            paymentBtn.disabled = true;
            document.getElementById("eta").innerText = "";
            alert("Failed to calculate delivery fee. Try a more specific or nearby address.");
           
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


    function suggestNextAvailableSchedule(totalMinutes) {
        const scheduleDateInput = document.getElementById("scheduleDate");
        const scheduleTimeSelect = document.getElementById("scheduleTime");

        const selectedDateStr = scheduleDateInput.value;
        if (!selectedDateStr) return;

        const deliveryDate = new Date(selectedDateStr);
        const now = new Date();
        const prepTime = new Date(now.getTime() + totalMinutes * 60 * 1000);

        const deliveryStartHour = 10;
        const deliveryEndHour = 17;
        const slotInterval = 30; // minutes

        scheduleTimeSelect.innerHTML = "";

        const optionDefault = document.createElement("option");
        optionDefault.text = "Select Time";
        optionDefault.disabled = true;
        optionDefault.selected = true;
        scheduleTimeSelect.appendChild(optionDefault);

        document.getElementById("hiddenScheduleStart").value = "";
        document.getElementById("hiddenScheduleEnd").value = "";

        for (let hour = deliveryStartHour; hour < deliveryEndHour; hour++) {
            for (let minute = 0; minute < 60; minute += slotInterval) {
                const slot = new Date(deliveryDate);
                slot.setHours(hour, minute, 0, 0);

                // If selected date is today, skip times before prep time
                if (deliveryDate.toDateString() === now.toDateString() && slot < prepTime) {
                    continue;
                }

                const end = new Date(slot.getTime() + slotInterval * 60 * 1000);
                const slotLabel = `${slot.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: true })} - ${end.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: true })}`;

                const option = document.createElement("option");
                option.value = slotLabel;
                option.text = slotLabel;
                scheduleTimeSelect.appendChild(option);
            }
        }
    }

    function formatDateTimeLocal(date) {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        return `${year}-${month}-${day}T${hours}:${minutes}`;
    }

    document.getElementById("scheduleTime").addEventListener("change", function () {
        const selected = this.value;
        const parts = selected.split(" - ");
        const scheduleDateStr = document.getElementById("scheduleDate").value; // Example: July 5, 2025

        if (parts.length === 2) {
            const startTimeStr = `${scheduleDateStr} ${parts[0]}`;
            const endTimeStr = `${scheduleDateStr} ${parts[1]}`;

            const start = new Date(startTimeStr);
            const end = new Date(endTimeStr);

            document.getElementById("hiddenScheduleStart").value = formatDateTimeLocal(start);
            document.getElementById("hiddenScheduleEnd").value = formatDateTimeLocal(end);
        }

        if (addressInput.value.length > 0 && document.getElementById("scheduleDate").value.length > 0
            && document.getElementById("scheduleTime").value.length > 0) {

            paymentBtn.disabled = false;
        }
    });


    document.getElementById("scheduleDate").addEventListener("change", () => {
        //if (document.getElementById("deliverLater").checked) {
           
        //}
        suggestNextAvailableSchedule(duration);
    });


    function setupTimePicker(prepMinutes) {
        const now = new Date();
        const prepTime = new Date(now.getTime() + prepMinutes * 60 * 1000);
        const today = new Date();

        let minTimeStr = "10:00";
        if (prepTime.toDateString() === today.toDateString()) {
            minTimeStr = `${String(prepTime.getHours()).padStart(2, "0")}:${String(prepTime.getMinutes()).padStart(2, "0")}`;
        }

        //flatpickr("#scheduleTime", {
        //    enableTime: true,
        //    noCalendar: true,
        //    dateFormat: "h:i K",
        //    time_24hr: false,
        //    minTime: minTimeStr,
        //    maxTime: "17:00"
        //});
    }

    // Flatpickr Date Only
    flatpickr("#scheduleDate", {
        minDate: "today",
        dateFormat: "F j, Y"
    });

    // Delivery option toggle
    //$('input[name="deliveryOption"]').on('change', function () {
    //    if ($('#deliverLater').is(':checked')) {
    //        $('.schedule-options').slideDown();
    //        suggestNextAvailableSchedule(duration);
    //        setupTimePicker(duration);
    //    }
    //    //else {
    //    //    $('.schedule-options').slideUp();
    //    //    $('#scheduleDate').val('');
    //    //    $('#scheduleTime').val('');
    //    //}
    //});

    //if ($('#deliverLater').is(':checked')) {
    //    $('.schedule-options').show();
    //    suggestNextAvailableSchedule(duration);
    //    setupTimePicker(duration);
    //}
    $('.schedule-options').show();
    suggestNextAvailableSchedule(duration);
    setupTimePicker(duration);

    document.getElementById("proceedToPayment").addEventListener("click", function (e) {
        const scheduleTime = document.getElementById("hiddenScheduleStart").value;
        const scheduleDate = document.getElementById("scheduleDate").value;
        const manualAddress = document.getElementById("address").value;
        const currentAddress = document.getElementById("currentAddress").value;

        if (!scheduleTime || !scheduleDate) {
            alert("Please select a schedule date and time.");
            e.preventDefault();
        } else if (!manualAddress && !currentAddress) {
            alert("Please select or enter address.");
            e.preventDefault();
        }
    });
   
});



