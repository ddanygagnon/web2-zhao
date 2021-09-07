type mediaQuerieSizes = 'xs' | 'sm' | 'md' | 'lg' | 'xl'

const mediaQueries = {
    xs: 32,
    sm: 48,
    md: 64,
    lg: 80,
    xl: 90
}

export const mediaQuery = (size: mediaQuerieSizes): boolean => {
    return window.matchMedia(`(min-width: ${mediaQueries[size]}rem)`).matches;
}