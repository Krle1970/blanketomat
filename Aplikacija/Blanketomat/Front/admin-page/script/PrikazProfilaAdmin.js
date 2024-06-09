document.addEventListener('DOMContentLoaded', () => {
  const email = localStorage.getItem('email');
  const imejl = document.getElementById('email-display');
  imejl.innerText = email;
});