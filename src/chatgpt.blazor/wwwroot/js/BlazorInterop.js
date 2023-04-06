
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
