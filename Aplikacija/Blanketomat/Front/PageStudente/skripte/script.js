var toggleBtn = document.querySelector('.toggle-btn');

toggleBtn.addEventListener("click", function () {
    document.querySelector("#sidebar").classList.toggle("expand");

    
    var icon = toggleBtn.querySelector('i');

    
    if (icon.classList.contains('lni-angle-double-right')) {
        
        icon.classList.remove('lni-angle-double-right');
        icon.classList.add('lni-angle-double-left');
    } else {
        
        icon.classList.remove('lni-angle-double-left');
        icon.classList.add('lni-angle-double-right');
    }
});

//  document.querySelector('.logDugme').addEventListener('click', () => {
//  window.location.href = '../Login/LoginPage.html';
//    });


  