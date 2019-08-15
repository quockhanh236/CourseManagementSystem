
var isGecko = navigator.userAgent.toLowerCase().indexOf("gecko") != -1;
var iframe = (isGecko) ? document.getElementById("frameId") : frames["frameId"];
var iWin = (isGecko) ? iframe.contentWindow : iframe.window;
var iDoc = (isGecko) ? iframe.contentDocument : iframe.document;
var iHTML = "<html><head></head><body style='background-color: white;font-size: 14pt;'></body></html>";
var isLoadedFromDisk = false;
var isImage = false;
var loadedFile = "";
var isFilledB;
var isFilledI;
var isFilledU;
iDoc.open();
iDoc.write(iHTML);
iDoc.close();
var isChangeSize = false;
iDoc.designMode = "on";
iDoc.body.onclick = fillUI;
iDoc.body.onkeyup = fillUI;
var isSettingsVideo = false;
var tempSelection = null;
//document.getElementById('frameIdJs').contentDocument.body.onpaste = pasteJs;
function rgbToHex(r, g, b) {
    return componentToHex(r) + componentToHex(g) + componentToHex(b);
}



function componentToHex(c) {
    c = Number(c);
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}

function changeB() {
    if (document.getElementById("bold").innerHTML == "B") {
        changeBoldButton(true);
    }
    else changeBoldButton(false);
}

function changeI() {
    if (document.getElementById("ital").innerHTML == "I") {
        changeItalicButton(true);
    }
    else changeItalicButton(false);
}

function changeU() {
    if (document.getElementById("under").innerHTML == "U") {
        changeUnderlineButton(true);
    }
    else changeUnderlineButton(false);
}

function fillButtons(node) {
    if (node != null) {
        isFilledB = isFilledI = isFilledU = false;
        while (node.tagName != "BODY") {
            if (node.tagName == "B") {
                changeBoldButton(true);
                isFilledB = true;
            }
            if (node.tagName == "I") {
                changeItalicButton(true);
                isFilledI = true;
            }
            if (node.tagName == "U") {
                changeUnderlineButton(true);
                isFilledU = true;
            }
            node = node.parentNode;
        }
        if (!isFilledB) {
            changeBoldButton(false);
            isFilledB = false;
        }
        if (!isFilledI) {
            changeItalicButton(false);
            isFilledI = false;
        }
        if (!isFilledU) {
            changeUnderlineButton(false);
            isFilledU = false;
        }
    }
}

function fillColors(fontColor, fontBackgroundColor) {
    while (fontColor.tagName != "HTML") {
        if (fontColor.color != null && fontColor.color != "") {
            var colorText = fontColor.color;
            colorText = colorText.slice(1, colorText.length);
            document.getElementById('fontColor').color.fromString(colorText);
            break;
        }
        else if (fontColor.style.getPropertyValue("color") != null) {
            if (fontColor.style.getPropertyValue("color") != "black") {
                var expr = /(\d*),\s(\d*),\s(\d*)/i;
                var myArray = fontColor.style.getPropertyValue("color").match(expr);
                document.getElementById('fontColor').color.fromString(rgbToHex(myArray[1], myArray[2], myArray[3]));
                break;
            }
        }
        else {
            fontColor = fontColor.parentNode;
        }
        document.getElementById('fontColor').color.fromString('000000');
    }
    while (fontBackgroundColor.tagName != "HTML") {
        if (fontBackgroundColor.style.getPropertyValue("background-color") != null) {
            if (fontBackgroundColor.style.getPropertyValue("background-color") != "white") {
                expr = /(\d*),\s(\d*),\s(\d*)/i;
                myArray = fontBackgroundColor.style.getPropertyValue("background-color").match(expr);
                document.getElementById('bgColor').color.fromString(rgbToHex(myArray[1], myArray[2], myArray[3]));
                break;
            }
            else {
                document.getElementById('bgColor').color.fromString('FFFFFF');
                break;
            }
        }
        else {
            fontBackgroundColor = fontBackgroundColor.parentNode;
        }
    }
}

function fillFontInfo(font, fontName) {
    var str;
    while (font.tagName != "HTML") {
        str = font.style.getPropertyValue("font-size");
        if (str != null) {
            document.getElementById("fontSizeInput").value = str.slice(0, str.length - 2);
            break;
        }
        else {
            document.getElementById("fontSizeInput").value = 14;
        }
        font = font.parentNode;
    }
    while (fontName.tagName != "HTML") {
        if (fontName.face != null && fontName.face != "") {
            document.getElementById("fonts").value = fontName.face;
            break;
        }
        else if (fontName.style.getPropertyValue("font-family") != null) {
            var fontstr = fontName.style.getPropertyValue("font-family");
            fontstr = fontstr.slice(1, fontstr.length - 1);
            document.getElementById("fonts").value = fontstr;
            break;
        }
        else {
            document.getElementById("fonts").value = "Times New Roman";
        }
        fontName = fontName.parentNode;
    }
}

function fillUI() {
    if (isChangeSize) { // Обязательно должно стоять в самом вверху!!!!
        var t = document.getElementById("fontSizeInput").value;
        var selectedFont = iWin.getSelection().focusNode.parentNode;
        selectedFont.style.fontSize = t + 'pt';
        isChangeSize = false;
    }
    var selectedElement = iWin.getSelection().focusNode.parentNode;
    var fontColor, fontBackgroundColor, font, fontName, position, node;
    fontColor = fontBackgroundColor = font = fontName = position = node = selectedElement;
    fillColors(fontColor, fontBackgroundColor);
    fillFontInfo(font, fontName);
    fillButtons(node);
    fillPosition(position);
    frameFitting();
}

function fillPosition(position) {
    if (position.tagName == "BODY") {
        iWin.focus();
        return;
    }
    while (position.tagName != "HTML") {
        if (position.style.getPropertyValue("text-align") != null) {
            if (position.style.getPropertyValue("text-align") == "right") {
                document.getElementById("center").src = "/../../resources/CreateLecture/res/Center.png";
                document.getElementById("left").src = "/../../resources/CreateLecture/res/Left.png";
                document.getElementById("right").src = "/../../resources/CreateLecture/res/RightB.png";
                document.getElementById("inwidth").src = "/../../resources/CreateLecture/res/InWidth.png";
                break;
            }
            else
                if (position.style.getPropertyValue("text-align") == "center") {
                    document.getElementById("center").src = "/../../resources/CreateLecture/res/CenterB.png";
                    document.getElementById("left").src = "/../../resources/CreateLecture/res/Left.png";
                    document.getElementById("right").src = "/../../resources/CreateLecture/res/Right.png";
                    document.getElementById("inwidth").src = "/../../resources/CreateLecture/res/InWidth.png";
                    break;
                }
                else
                    if (position.style.getPropertyValue("text-align") == "justify") {
                        document.getElementById("center").src = "/../../resources/CreateLecture/res/Center.png";
                        document.getElementById("left").src = "/../../resources/CreateLecture/res/Left.png";
                        document.getElementById("right").src = "/../../resources/CreateLecture/res/Right.png";
                        document.getElementById("inwidth").src = "/../../resources/CreateLecture/res/InWidthB.png";
                        break;
                    }
                    else
                        if (position.style.getPropertyValue("text-align") == "left") {
                            document.getElementById("center").src = "/../../resources/CreateLecture/res/Center.png";
                            document.getElementById("left").src = "/../../resources/CreateLecture/res/LeftB.png";
                            document.getElementById("right").src = "/../../resources/CreateLecture/res/Right.png";
                            document.getElementById("inwidth").src = "/../../resources/CreateLecture/res/InWidth.png";
                            break;
                        }
        }
        else {
            document.getElementById("center").src = "/../../resources/CreateLecture/res/Center.png";
            document.getElementById("left").src = "/../../resources/CreateLecture/res/LeftB.png";
            document.getElementById("right").src = "/../../resources/CreateLecture/res/Right.png";
            document.getElementById("inwidth").src = "/../../resources/CreateLecture/res/InWidth.png";
        }
        position = position.parentNode;
    }
}

function setBold() {
    iWin.document.execCommand("bold", null, "");
    if (getSelectedText() == "") {
        changeB();
    }
    else {
        fillUI();
    }
    iWin.focus();
}

function setItal() {
    iWin.document.execCommand("italic", null, "");
    if (getSelectedText() == "") {
        changeI();
    }
    else {
        fillUI();
    }
    iWin.focus();
}

function setUnderline() {
    iWin.document.execCommand("underline", null, "");
    if (getSelectedText() == "") {
        changeU();
    }
    else {
        fillUI();
    }
    iWin.focus();
}

function setSize() {
    var t = document.getElementById("fontSizeInput").value;

    //iWin.document.execCommand('formatblock', false, 'p');
    if (getSelectedText() != "") {
        iWin.document.execCommand("FontSize", null, 1);
        var anchorNode = iWin.getSelection().anchorNode.parentNode;
        var focusNode = iWin.getSelection().focusNode.parentNode;
        anchorNode.style.fontSize = t + 'pt';
        focusNode.style.fontSize = t + 'pt';
    }
    else {
        var selectedElement = iWin.getSelection().focusNode.parentNode;
        var str = selectedElement.style.getPropertyValue("font-size");
        if (str != null)
            if (str.slice(0, str.length - 2) != document.getElementById("fontSizeInput").value) {
                iWin.document.execCommand("FontSize", null, 1);
                isChangeSize = true;
            }
    }
    iWin.focus();

}

function getSelectedText() {
    var text = "";
    if (iWin.getSelection) {
        text = iWin.getSelection();
    }
    else if (iDoc.getSelection) {
        text = iDoc.getSelection();
    }
    else if (iDoc.selection) {
        text = iDoc.selection.createRange().text;
    }
    return text;
}

function setLink() {
    var url = prompt("Input URL:", "http://");
    if (!url) return;
    iWin.focus();
    iWin.document.execCommand("CreateLink", null, url);
}

function setLeft() {
    iWin.document.execCommand("JustifyLeft", null, "");
    iWin.focus();
    fillPosition(iWin.getSelection().focusNode);
}

function setRight() {
    iWin.document.execCommand("JustifyRight", null, "");
    iWin.focus();
    fillPosition(iWin.getSelection().focusNode);
}

function setCenter() {
    iWin.document.execCommand("JustifyCenter", null, "");
    iWin.focus();
    fillPosition(iWin.getSelection().focusNode);
}

function setInWidth() {
    iWin.document.execCommand("JustifyFull", null, "");
    iWin.focus();
    fillPosition(iWin.getSelection().focusNode);
}

function setFont() {
    var font = document.getElementById("fonts").value;
    iWin.document.execCommand("FontName", null, font);
    iWin.focus();
}

function init() {
    document.getElementById("fontSizeInput").value = 14;
    document.getElementById('fontColor').color.fromString('000000');
    document.getElementById("center").src = "/../../resources/CreateLecture/res/Center.png";
    document.getElementById("left").src = "/../../resources/CreateLecture/res/LeftB.png";
    document.getElementById("right").src = "/../../resources/CreateLecture/res/Right.png";
    document.getElementById("inwidth").src = "/../../resources/CreateLecture/res/InWidth.png";
    iWin.focus();
}

function changeBoldButton(bool) {
    if (bool) {
        document.getElementById("bold").innerHTML = "<b>B</b>";
    }
    else {
        document.getElementById("bold").innerHTML = "B";
    }
}

function changeItalicButton(bool) {
    if (bool) {
        document.getElementById("ital").innerHTML = "<b>I</b>";
    }
    else {
        document.getElementById("ital").innerHTML = "I";
    }
}

function changeUnderlineButton(bool) {
    if (bool) {
        document.getElementById("under").innerHTML = "<b>U</b>";
    }
    else {
        document.getElementById("under").innerHTML = "U";
    }
}

function setFontColor(color) {
    iWin.document.execCommand("ForeColor", null, color);
}

function setFontBGColor(color) {
    iWin.document.execCommand("BackColor", null, color);
}
function openSettings(isVideo) {
    isLoadedFromDisk = false;
    document.getElementById('cover').style.display = '';
    tempSelection = iWin.getSelection();
    isSettingsVideo = isVideo;
    if (isVideo) {
        document.getElementById('inputSizeWidth').value = 640;
        document.getElementById('inputSizeHeigth').value = 390;
    }
    else {
        document.getElementById('inputSizeWidth').value = 100;
        document.getElementById('inputSizeHeigth').value = 100;
    }
    document.getElementById('settingsForm').style.left = document.getElementById('insertPictureButton').offsetLeft + 'px';
    document.getElementById('settingsForm').style.top = document.getElementById('insertPictureButton').offsetTop
    + document.getElementById('insertPictureButton').offsetHeight + 5 + 'px';
    document.getElementById('settingsForm').style.display = '';
    document.getElementById('urlInput').value = "";
    document.getElementById('urlInput').focus();
}

function changeInputSize() {
    var url = document.getElementById('urlInput').value;
    var image = new Image();
    image.src = url;
    image.onload = function () {
        document.getElementById('inputSizeWidth').value = image.width;
        document.getElementById('inputSizeHeigth').value = image.height;
    }
}

function insertPicture(selection, urlImage, width, height, float) {
    //var fileurl = document.getElementById("insertPicture").value;
    //iWin.document.execCommand("InsertImage", null, fileurl);
    //alert(document.getElementById('insertPictureButton').offsetLeft);
    var img = document.createElement('img');
    img.setAttribute("src", urlImage);
    img.setAttribute("width", width);
    img.setAttribute("height", height);
    img.setAttribute("onclick", "openSettings(false, false)");
    img.style.float = float;
    var selectedElement = null;
    if (selection.focusNode.tagName == "BODY")
        selectedElement = selection.focusNode;
    else
        selectedElement = selection.focusNode.parentNode;
    selectedElement.appendChild(img);
}

function insertVideo(selection, urlVideo, width, height, float) {
    var url = urlVideo;
    if (-1 < url.indexOf('youtube.')) {
        var iFrame = document.createElement('iframe');
        var temp = '';
        for (var i = url.length - 1; url[i] != '=' && url[i] != '/'; i--) {
            temp += url[i];
        }
        url = '';
        for (var i = temp.length - 1; i >= 0; i--) {
            url += temp[i];
        }
        var src = '//www.youtube.com/embed/' + url;
        iFrame.setAttribute("width", width);
        iFrame.setAttribute("height", height);
        iFrame.setAttribute("src", src);
        iFrame.setAttribute("frameborder", 0);
        iFrame.setAttribute("allowfullscreen", '');
        iFrame.style.float = float;
        var selectedElement = null;
        if (selection.focusNode.tagName == "BODY")
            selectedElement = selection.focusNode;
        else
            selectedElement = selection.focusNode.parentNode;
        selectedElement.appendChild(iFrame);
        var Doc = (isGecko) ? iFrame.contentDocument : iFrame.document;
        Doc.designMode = "off";
        void 0;
        Doc.close();
    }
    else {
        var selectedElement = null;
        if (tempSelection.focusNode.tagName == "BODY")
            selectedElement = tempSelection.focusNode;
        else
            selectedElement = tempSelection.focusNode.parentNode;
        var temp1 = document.createElement('br');
        selectedElement.appendChild(temp1);
        var Video = document.createElement('video');
        Video.setAttribute("width", document.getElementById('inputSizeWidth').value);
        Video.setAttribute("src", url);
        Video.setAttribute("height", document.getElementById('inputSizeHeigth').value);
        Video.setAttribute("controls", "");
        Video.style.float = float;
        selectedElement.appendChild(Video);
        var Doc = (isGecko) ? Video.contentDocument : Video.document;
        Video.designMode = "off";
        selectedElement.appendChild(temp1);
    }
}

function applySettings() {
    var float;
    if (document.getElementsByClassName('floatRadioButton')[0].checked) {
        float = document.getElementsByClassName('floatRadioButton')[0].value;
    } else if (document.getElementsByClassName('floatRadioButton')[1].checked) {
        float = document.getElementsByClassName('floatRadioButton')[1].value;
    } else {
        float = document.getElementsByClassName('floatRadioButton')[2].value;
    }
    if (document.getElementById('urlInput').value != "" && !isLoadedFromDisk) {
        if (isSettingsVideo) {
            insertVideo(tempSelection,
                        document.getElementById('urlInput').value,
                        document.getElementById('inputSizeWidth').value,
                        document.getElementById('inputSizeHeigth').value,
                        float);
        } else {
            insertPicture(tempSelection,
                        document.getElementById('urlInput').value,
                        document.getElementById('inputSizeWidth').value,
                        document.getElementById('inputSizeHeigth').value,
                        float);
        }
    }
    else
        if (isLoadedFromDisk) {
            var file = loadedFile;
            if (isSettingsVideo) {
                var selectedElement = null;
                if (tempSelection.focusNode.tagName == "BODY")
                    selectedElement = tempSelection.focusNode;
                else
                    selectedElement = tempSelection.focusNode.parentNode;
                var temp1 = document.createElement('br');
                selectedElement.appendChild(temp1);
                var Video = document.createElement('video');
                Video.setAttribute("width", document.getElementById('inputSizeWidth').value);
                Video.setAttribute("height", document.getElementById('inputSizeHeigth').value);
                Video.setAttribute("src", "../resources/Media/" + file);
                Video.setAttribute("controls", "");
                Video.style.float = float;

                selectedElement.appendChild(Video);
                var Doc = (isGecko) ? Video.contentDocument : Video.document;
                Video.designMode = "off";
                selectedElement.appendChild(temp1);
            }
            else {
                var Picture = document.createElement('img');
                Picture.setAttribute("width", document.getElementById('inputSizeWidth').value);
                Picture.setAttribute("height", document.getElementById('inputSizeHeigth').value);
                Picture.setAttribute("src", "../resources/Media/" + file);
                Picture.style.float = float;
                var selectedElement = null;
                if (tempSelection.focusNode.tagName == "BODY")
                    selectedElement = tempSelection.focusNode;
                else
                    selectedElement = tempSelection.focusNode.parentNode;
                selectedElement.appendChild(Picture);
                var Doc = (isGecko) ? Picture.contentDocument : Picture.document;
                Picture.designMode = "off";
            }
        }
    document.getElementById('settingsForm').style.display = 'none';
    document.getElementById('cover').style.display = 'none';
}

function loadContent() {
    document.getElementById("selectFile").click();
}

function loadOnServer() {
    var file = document.getElementById("selectFile").value;
    var temp = '';
    for (var i = file.length - 1; file[i] != '\\'; i--) {
        temp += file[i];
    }
    file = '';
    for (var i = temp.length - 1; i >= 0; i--) {
        file += temp[i];
    }
    var type = document.getElementById("selectFile").files[0].type;
    var image = false;
    if (!isSettingsVideo) {
        if (-1 < type.indexOf('image')) {
            document.getElementById("SubmitSingle").click();
            image = true;
            isLoadedFromDisk = true;
        }
        else {
            alert("Was required the image file!");
        }
    }
    if (isSettingsVideo) {
        if (-1 < type.indexOf('video')) {
            document.getElementById("SubmitSingle").click();
            image = false;
            isLoadedFromDisk = true;
        }
        else {
            alert("Was required the video file!");
        }
    }
    isImage = image;
    if (document.getElementById("selectFile").value != "" && isLoadedFromDisk) {
        loadedFile = file;
        document.getElementById("selectFile").value = "";
    }
    else {
        document.getElementById("selectFile").value = "";
    }
    getSizeOfImage();
}

function getSizeOfImage() {
    if (!isImage || !isLoadedFromDisk) return;
    var url = "../resources/Media/" + loadedFile;
    //var xmlhttp = getXmlHttp();
    //while (xmlhttp.status != 200) {
    //    xmlhttp.open('GET', url, false);
    //    xmlhttp.send(null);
    //}
    var image = new Image();
    image.src = url;
    image.onload = function () {
        document.getElementById('inputSizeWidth').value = image.width;
        document.getElementById('inputSizeHeigth').value = image.height;
    }
}

function getXmlHttp() {
    var xmlhttp;
    try {
        xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
        try {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (E) {
            xmlhttp = false;
        }
    }
    if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
        xmlhttp = new XMLHttpRequest();
    }
    return xmlhttp;
}

function closeSettings() {
    document.getElementById('regDiv').style.display = 'none';
    document.getElementById('settingsForm').style.display = 'none';
    document.getElementById('cover').style.display = 'none';
    document.getElementById('preview').style.display = 'none';
}

function create() {
    document.getElementById("inputText").value = iframe.contentWindow.document.body.innerHTML;
    document.getElementById("inputName").focus();
    document.getElementById('cover').style.display = '';
    document.getElementById('regDiv').style.display = '';
}
function htmlEditor() {

    if (document.getElementById('modeButton').innerHTML == "HTML") {
        document.getElementById('frameId').style.display = 'none';
        document.getElementById('textAreaJs').style.display = '';
        document.getElementById('modeButton').innerHTML = "Editor";
        var temp = document.getElementsByClassName('noJs');
        for (var i = 0; i < temp.length; i++) {
            temp[i].disabled = true;
        }
        document.getElementById('fontColor').disabled = true;
        document.getElementById('bgColor').disabled = true;

        var content = iframe.contentDocument.body.innerHTML;
        var readableContent = "";
        for (var i = 0, j = 0; i < content.length; i++, j++) {
            if (content[i] == '>') {
                readableContent += content[i]
                readableContent += '\n';
            } else if (content[i] == '<' && i != 0 && content[i - 1] != '>' && content[i - 1] != '\n') {
                readableContent += '\n';
                readableContent += content[i]
            } else {
                readableContent += content[i]
            }
        }
        document.getElementById('textAreaJs').value = readableContent;
    }
    else if (document.getElementById('modeButton').innerHTML == "Editor") {
        document.getElementById('frameId').style.display = '';
        document.getElementById('textAreaJs').style.display = 'none';
        document.getElementById('modeButton').innerHTML = "HTML";

        var temp = document.getElementsByClassName('noJs');
        for (var i = 0; i < temp.length; i++) {
            temp[i].disabled = false;
        }
        document.getElementById('fontColor').disabled = false;
        document.getElementById('bgColor').disabled = false;
        var content = document.getElementById('textAreaJs').value;
        content = content.replace(/(\n(\r)?)/g, ' ');
        iframe.contentDocument.body.innerHTML = noBreak;
    }
}
function preview() {
    document.getElementById('cover').style.display = '';
    //document.write("hello");
    document.getElementById('preview').style.display = '';
    if (document.getElementById('modeButton').innerHTML == "HTML") {
        document.getElementById('previewFrame').contentDocument.body.innerHTML = iframe.contentDocument.body.innerHTML;

    } else {
        document.getElementById('previewFrame').contentDocument.body.innerHTML = document.getElementById('textAreaJs').value;
    }
}
