document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const rolSelect = document.querySelector("select[name='id']");
    const alerta = document.getElementById("alerta-rol");

    form.addEventListener("submit", function (e) {
        if (!rolSelect.value) {
            e.preventDefault();
            alerta.classList.remove("d-none");
            alerta.classList.add("show");
        }
    });
});