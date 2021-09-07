import { mediaQuery } from "../tools/media";
var openBtn = document.getElementById('zhaoNavOpen');
var closeBtn = document.getElementById('zhaoNavClose');
openBtn.addEventListener('click', OpenNav);
closeBtn.addEventListener('click', CloseNav);
function OpenNav() {
    var nav = document.getElementById('zhaoNav');
    nav.style.width = "100%";
}
function CloseNav() {
    var nav = document.getElementById('zhaoNav');
    nav.style.width = "0";
}
window.addEventListener('resize', function () {
    var nav = document.getElementById('zhaoNav');
    if (mediaQuery('sm'))
        nav.style.width = "100%";
    if (!mediaQuery('sm'))
        nav.style.width = "0";
});
