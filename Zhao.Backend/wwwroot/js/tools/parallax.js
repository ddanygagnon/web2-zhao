export function Parallax(varName, speed, limit) {
    if (limit === void 0) { limit = 330; }
    if (window.pageYOffset * 0.3 >= limit)
        return;
    var $root = document.documentElement;
    var imageScroll = window.pageYOffset * speed + "px";
    $root.style.setProperty(varName, imageScroll);
}
