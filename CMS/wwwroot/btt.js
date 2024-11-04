$(document).ready(function () {
    /* Back to Top button */
    const bttbutton = document.getElementById("back-to-top-btn");

    // Only add scroll event if the button exists
    if (bttbutton) {
        // When the user scrolls down 50px from the top of the document, show the button
        window.onscroll = function () { scrollFunction() };

        function scrollFunction() {
            if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
                bttbutton.style.display = "block";
            } else {
                bttbutton.style.display = "none";
            }
        };

        // Back to Top button click event
        $('.btt-link').click(function (e) {
            window.scrollTo({
                top: 0,
                left: 0,
                behavior: 'smooth'
            });
        });
    }
});
