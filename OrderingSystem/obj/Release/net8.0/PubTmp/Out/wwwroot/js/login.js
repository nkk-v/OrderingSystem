function togglePassword(inputId, iconId) {
    const input = document.getElementById(inputId);
    const icon = document.getElementById(iconId);

    if (!input || !icon) return;

    const isPassword = input.type === "password";
    input.type = isPassword ? "text" : "password";

    // Toggle Bootstrap Icon classes
    icon.classList.remove("bi-eye-slash-fill", "bi-eye-fill");
    icon.classList.add(isPassword ? "bi-eye-fill" : "bi-eye-slash-fill");
}