document.addEventListener('DOMContentLoaded', () => {
  const ime = localStorage.getItem('ime');
  const prezime = localStorage.getItem('prezime');
  const email = localStorage.getItem('email');
  const ImeIPrezime = document.getElementById('imeIprezime-display');
  const mejl = document.getElementById('email-display');
  ImeIPrezime.innerText = ime + ' ' + prezime;
  mejl.innerText = email;
  console.log(ImeIPrezime.innerText);
});