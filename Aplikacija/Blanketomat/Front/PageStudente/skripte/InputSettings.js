document.addEventListener('DOMContentLoaded', function() {
    const inputs = document.querySelectorAll('input[type="text"], input[type="search"], input[type="email"], input[type="url"], input[type="tel"], input[type="number"], input[type="password"]');
    inputs.forEach(input => {
        input.setAttribute('autocomplete', 'off');
    });
});