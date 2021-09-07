import {mediaQuery} from "../tools/media";

let openBtn = document.getElementById('zhaoNavOpen')
let closeBtn = document.getElementById('zhaoNavClose')

openBtn.addEventListener('click', OpenNav)
closeBtn.addEventListener('click', CloseNav)

function OpenNav() {
    let nav = document.getElementById('zhaoNav')
    nav.style.width = "100%"
}

function CloseNav() {
    let nav = document.getElementById('zhaoNav')
    nav.style.width = "0"
}

window.addEventListener('resize', () => {
    let nav = document.getElementById('zhaoNav')
    if(mediaQuery('sm')) nav.style.width = "100%"
    if(!mediaQuery('sm')) nav.style.width = "0"
})