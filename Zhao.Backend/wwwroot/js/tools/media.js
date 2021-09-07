var mediaQueries = {
    xs: 32,
    sm: 48,
    md: 64,
    lg: 80,
    xl: 90
};
export var mediaQuery = function (size) {
    return window.matchMedia("(min-width: " + mediaQueries[size] + "rem)").matches;
};
