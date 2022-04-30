export function show(element) {
    element.style.display = '';
}

export function hide(element) {
    element.style.display = 'none';
}

export function hideAll(elements) {
    for(const element of elements)
        hide(element);
}

export function setVisible(element, visible) {
    element.style.display = visible ? '' : 'none';
}

export function clear(element) {
    element.innerHTML = '';
}
