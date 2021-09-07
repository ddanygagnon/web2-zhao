import {mediaQuery} from "../tools/media";
import {Parallax} from "../tools/parallax";

const heroImage = () => {
    const varName = '--hero-image-y'
    mediaQuery('sm') ?
        Parallax(varName, 0.3) :
        Parallax(varName, 0)
}

const heroTitle = () => {
    const varName = '--hero-title-y'
    mediaQuery('sm') ?
        Parallax(varName, -0.5) :
        Parallax(varName, 0.3)
}

const heroParralax = () => {
    heroImage()
    heroTitle()
}

window.addEventListener('scroll', heroParralax)