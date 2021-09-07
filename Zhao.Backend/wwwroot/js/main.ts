import './components/hero'
import './components/backTop'
import './components/nav'

const [red, green, blue] = [255, 255, 255]
const elem: HTMLElement = document.querySelector('.changeColor')
const elem2: HTMLElement = document.querySelector('.changeBackground--red')

window.addEventListener('scroll', () => {
    let y = 1 + (window.scrollY || window.pageYOffset) / 150
    y = y < 1 ? 1 : y
    const [r, g, b] = [red / y, green / y, blue / y].map(Math.round)
    if(elem)
        elem.style.backgroundColor = `rgb(${r + 220}, ${g + 300}, ${b + 220})`
    else if(elem2)
        elem2.style.backgroundColor = `rgb(${r + 300}, ${g + 180}, ${b + 180})`
})
