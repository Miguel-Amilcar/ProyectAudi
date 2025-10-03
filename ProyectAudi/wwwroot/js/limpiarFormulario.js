document.addEventListener("DOMContentLoaded", function () {
    const formularios = document.querySelectorAll("form");

    formularios.forEach(form => {
        form.addEventListener("submit", function () {
            setTimeout(() => {
                form.reset();
            }, 100);
        });
    });
});

