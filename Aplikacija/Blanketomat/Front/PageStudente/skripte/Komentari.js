document.addEventListener('DOMContentLoaded', function () {
    // Toggle display of likes list
    document.querySelectorAll('.likes-toggle').forEach(function (toggle) {
        toggle.addEventListener('click', function () {
            const likesList = this.nextElementSibling;
            if (likesList.style.display === 'none' || !likesList.style.display) {
                likesList.style.display = 'block';
            } else {
                likesList.style.display = 'none';
            }
        });
    });

    // Toggle display of replies
    document.querySelectorAll('.show-replies').forEach(function (toggle) {
        toggle.addEventListener('click', function () {
            const replies = this.nextElementSibling;
            if (replies.style.display === 'none' || !replies.style.display) {
                replies.style.display = 'block';
            } else {
                replies.style.display = 'none';
            }
        });
    });
});