function showCustomAlert(message) {
    const alertBox = document.getElementById("alertBox");
    const alertMessage = document.getElementById("alertMessage");

    alertMessage.textContent = message;
    alertBox.style.display = "block";
    alertBox.classList.add("fade-in");

    setTimeout(() => {
        alertBox.classList.remove("fade-in");
    }, 500);
}

function hideAlert() {
    const alertBox = document.getElementById("alertBox");
    alertBox.style.display = "none";
}