import { mediaQuery } from "../tools/media";
import { Parallax } from "../tools/parallax";
var heroImage = function () {
    var varName = '--hero-image-y';
    mediaQuery('sm') ?
        Parallax(varName, 0.3) :
        Parallax(varName, 0);
};
var heroTitle = function () {
    var varName = '--hero-title-y';
    mediaQuery('sm') ?
        Parallax(varName, -0.5) :
        Parallax(varName, 0.3);
};
var heroParralax = function () {
    heroImage();
    heroTitle();
};
window.addEventListener('scroll', heroParralax);
