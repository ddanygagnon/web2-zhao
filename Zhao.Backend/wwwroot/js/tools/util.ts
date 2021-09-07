export const hasClass = function(el, className) {
    if (el.classList) return el.classList.contains(className);
    else return !!el.className.match(new RegExp('(\\s|^)' + className + '(\\s|$)'));
};

export const addClass = function(el, className) {
    const classList = className.split(' ');
    if (el.classList) el.classList.add(classList[0]);
    else if (!hasClass(el, classList[0])) el.className += " " + classList[0];
    if (classList.length > 1) addClass(el, classList.slice(1).join(' '));
};

export const removeClass = function(el, className) {
    const classList = className.split(' ');
    if (el.classList) el.classList.remove(classList[0]);
    else if(hasClass(el, classList[0])) {
        var reg = new RegExp('(\\s|^)' + classList[0] + '(\\s|$)');
        el.className=el.className.replace(reg, ' ');
    }
    if (classList.length > 1) removeClass(el, classList.slice(1).join(' '));
};

export const toggleClass = function(el, className, bool) {
    if(bool) addClass(el, className);
    else removeClass(el, className);
};

export const scrollTo = function(final, duration, cb?) {
    let start = window.scrollY || document.documentElement.scrollTop,
        currentTime = null;

    const animateScroll = function (timestamp) {
        if (!currentTime) currentTime = timestamp;
        let progress = timestamp - currentTime;
        if (progress > duration) progress = duration;
        const val = easeInOutQuad(progress, start, final - start, duration);
        window.scrollTo(0, val);
        if (progress < duration) {
            window.requestAnimationFrame(animateScroll);
        } else {
            cb && cb();
        }
    };

    window.requestAnimationFrame(animateScroll);
};

export const easeInOutQuad = function (t, b, c, d) {
    t /= d/2;
    if (t < 1) return c/2*t*t + b;
    t--;
    return -c/2 * (t*(t-2) - 1) + b;
};