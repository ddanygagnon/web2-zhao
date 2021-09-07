export function Parallax(varName: string, speed: number, limit: number = 330) {
    if(window.pageYOffset * 0.3 >= limit) return;
    const $root = document.documentElement;
    const imageScroll = `${window.pageYOffset * speed}px`
    $root.style.setProperty(varName, imageScroll)
}