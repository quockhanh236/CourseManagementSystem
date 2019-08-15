function forChange(box) {
    var ind = document.getElementsByName("answers");
    for (var i = 0; i < 4; i++) {
        ind[i].setAttribute('type', box);
    }
}

function changeButtons() {
    var s = document.getElementById('typeSel');
    if (s.selectedIndex == 0) {
        forChange("radio");
    }
    else {
        forChange("checkbox");
    }
}
