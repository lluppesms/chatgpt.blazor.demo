
function focusOnInputField(input) {
    if (input) {
        let element = document.getElementById(input);
        if (element) {
            element.focus();
        }
    }
}
function scrollToBottomOfDiv(input) {
    if (input) {
        let element = document.getElementById(input);
        if (element) {
          element.scrollTop = element.scrollHeight;
        }
    }
}

function syncHeaderTitle() {
    let element = document.getElementById("headerPageTitle");
    if (element) {
        element.innerHTML = document.title;
    }
}
function setHeaderTitle(title) {
    if (title) {
        let element = document.getElementById("headerPageTitle");
        if (element) {
            element.innerHTML = title;
        }
    }
}
