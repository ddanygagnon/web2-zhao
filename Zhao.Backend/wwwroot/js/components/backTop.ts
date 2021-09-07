import {scrollTo as scrollerTo, removeClass, addClass} from '../tools/util'

(() => {
    let backTop = document.getElementsByClassName('js-back-top')[0],
        offset = 300, // browser window scroll (in pixels) after which the "back to top" link is shown
        offsetOpacity = 1200, //browser window scroll (in pixels) after which the "back to top" link opacity is reduced
        scrollDuration = 700,
        scrolling = false;

    if( backTop ) {
        window.addEventListener("scroll", () => {
            if( !scrolling ) {
                scrolling = true;
                (!window.requestAnimationFrame) ? setTimeout(checkBackToTop, 250) : window.requestAnimationFrame(checkBackToTop);
            }
        });

        backTop.addEventListener('click', function(event) {
            event.preventDefault();
            (!window.requestAnimationFrame) ? window.scrollTo(0, 0) : scrollerTo(0, scrollDuration);
        });
    }

    function checkBackToTop() {
        const windowTop = window.scrollY || document.documentElement.scrollTop;
        ( windowTop > offset ) ? addClass(backTop, 'back-top--is-visible') : removeClass(backTop, 'back-top--is-visible back-top--fade-out');
        ( windowTop > offsetOpacity ) && addClass(backTop, 'back-top--fade-out');
        scrolling = false;
    }
})();