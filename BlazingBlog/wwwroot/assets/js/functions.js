//jQuery( document ).ready(function() {

//    $(window).scroll(function(){
//    $('.topnav').toggleClass('scrollednav py-0', $(this).scrollTop() > 50);
//    });
    
//});


document.addEventListener("DOMContentLoaded", _ => {
    const topNav = document.getElementsByClassName("topnav")[0];
    if (topNav) {
        window.onscroll = () => {
            if (window.scrollY > 50) {
                topNav.classList.add("scrollednav", "py-0")
            }
            else {
                topNav.classList.remove("scrollednav", "py-0")
            }
        }
    }
})

function ToggleMenu(e){
    e.target.classList.toggle("collapsed");
    const navbarWrapper = document.getElementById("top-navbar-menu-wrapper");
    navbarWrapper.classList.toggle("show");
}