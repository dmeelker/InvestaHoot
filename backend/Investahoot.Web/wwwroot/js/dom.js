export function Dom(element) {
    element.style.display = '';
}

export function hide(element) {
    element.style.display = 'none';
}
export function setVisible(element, visible) {
    element.style.display = visible ? '' : 'none';
}