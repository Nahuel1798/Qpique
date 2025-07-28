// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Animación pulse al hacer click en dropdown toggle (botón que abre menú)
document.querySelectorAll('.nav-link.dropdown-toggle').forEach(btn => {
    btn.addEventListener('click', e => {
        btn.classList.add('animate__animated', 'animate__pulse');
        setTimeout(() => {
            btn.classList.remove('animate__animated', 'animate__pulse');
        }, 800);
    });
});