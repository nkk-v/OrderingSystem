// Flatpickr date picker
flatpickr("#scheduleDate", {
    minDate: "today",
    dateFormat: "F j, Y"
});

// Flatpickr time picker
flatpickr("#scheduleTime", {
    enableTime: true,
    noCalendar: true,
    dateFormat: "h:i K", // 12-hour format with AM/PM
    time_24hr: false
});

// Show/hide delivery schedule options
$(document).ready(function () {
    $('input[name="deliveryOption"]').on('change', function () {
        if ($('#deliverLater').is(':checked')) {
            $('.schedule-options').slideDown();
        } else {
            $('.schedule-options').slideUp();

            $('#scheduleDate').val('');
            $('#scheduleTime').val('');
        }
    });

    // Trigger the initial state on page load
    if ($('#deliverLater').is(':checked')) {
        $('.schedule-options').show();
    }
});