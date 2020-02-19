function StartEvent(first) {
    StopEvent();
    ////Add event listener
    if (document.addEventListener) {
        document.addEventListener("click", click_handler, true);
        document.addEventListener("mouseover", mouseover_handler, true);
        document.addEventListener("mouseout", mouseout_handler, true);
    } //good browsers
    else if (document.attachEvent) {
        document.attachEvent("onclick", click_handler);
        document.attachEvent("mouseover", mouseover_handler);
        document.attachEvent("mouseout", mouseout_handler);

    } //<=IE7
    if (first) {
        var hintLabel = parent.document.getElementById("DownloadingHint");
        hintLabel.style.display = "none";
    }
}
function StopEvent() {
    ////Add event listener
    if (document.addEventListener) {
        document.removeEventListener("click", click_handler, true);
        document.removeEventListener("mouseover", mouseover_handler, true);
        document.removeEventListener("mouseout", mouseout_handler, true);
    } //good browsers
    else if (document.attachEvent) {
        document.detachEvent("onclick", click_handler);
        document.detachEvent("mouseover", mouseover_handler);
        document.detachEvent("mouseout", mouseout_handler);
    } //<=IE7
}
function StopToSetEvent() {
    ////Add event listener
    if (document.addEventListener) {
        document.removeEventListener("click", click_handler, true);
        document.removeEventListener("mouseover", mouseover_handler, true);
        document.removeEventListener("mouseout", mouseout_handler, true);
        document.addEventListener("click", clickopen_handler, true);
    } //good browsers
    else if (document.attachEvent) {
        document.detachEvent("onclick", click_handler);
        document.detachEvent("mouseover", mouseover_handler);
        document.detachEvent("mouseout", mouseout_handler);
        document.attachEvent("onclick", clickopen_handler);
    } //<=IE7
}

function encode64(input) {
    input = escape(input);
    var output = "";
    var chr1, chr2, chr3 = "";
    var enc1, enc2, enc3, enc4 = "";
    var i = 0;
    do {
        chr1 = input.charCodeAt(i++);
        chr2 = input.charCodeAt(i++);
        chr3 = input.charCodeAt(i++);
        enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;
        if (isNaN(chr2)) {
            enc3 = enc4 = 64;
        } else if (isNaN(chr3)) {
            enc4 = 64;
        }
        output = output +
           keyStr.charAt(enc1) +
           keyStr.charAt(enc2) +
           keyStr.charAt(enc3) +
           keyStr.charAt(enc4);
        chr1 = chr2 = chr3 = "";
        enc1 = enc2 = enc3 = enc4 = "";
    } while (i < input.length);
    return output;
}

function clickopen_handler(e) {
    if (!e) { var e = window.event; }
    var el_clicked = e.srcElement || e.target;
    if (el_clicked.tagName == "A" || (el_clicked.tagName == "IMG" && el_clicked.parentNode.tagName == "A")) {
        //Make sure the user dosen't go anywhere
        if (e.preventDefault) e.preventDefault();
        else e.returnValue = false;
        //Load link via get_page.php
        parent.document.getElementById("webPageIframe").src = "/FavCard/Download/?url=" + el_clicked.href;
        parent.document.getElementById("OpenUrlEditor").innerHTML = el_clicked.href;
        parent.document.getElementById("_U_S_L_").value = el_clicked.href;
    }
}
function mouseover_handler(e) {
    if (!e) { var e = window.event; }
    var el_clicked = e.srcElement || e.target;
    //var element = document.getElementById(e.target);
    el_clicked.style.border = "1px solid blue";
}
function mouseout_handler(e) {
    if (!e) { var e = window.event; }
    var el_clicked = e.srcElement || e.target;
    el_clicked.style.border = "";
}
/*********/
var keyStr = "C6HlgsnA3Bz2FLOPbcW7ZaXSYUeVdfhiKEjmopIJqrktDGuvxMNy0145w8QRT9+/=";

setTimeout(StartEvent(true), 2000);
//StartEvent(true);

function click_handler(e) {
    if (!e) { var e = window.event; }
    var el_clicked = e.srcElement || e.target;
    if (e.preventDefault) e.preventDefault();
    else    e.returnValue = false;
    var matched = [];
    var dir = "parentNode";
    var cur = el_clicked;
    while (cur && cur.nodeType !== 9) {
        if (cur.nodeType === 1) {
            matched.push(cur);
        }
        cur = cur[dir];
    }
    StopToSetEvent();
    var xpath = '/';
    var path = matched;
    for (var i = path.length - 1; i >= 0; i--) {
        var nd = path[i].nodeName.toLowerCase();
        xpath += '/';
        if (nd != 'html' && nd != 'body') {
            xpath += nd;
            if (path[i].id != '') {
                xpath += '[@id=\'' + path[i].id + '\']';
            }
            else {
                var nodeIndex = 0;
                // Traverse it's children
                var ndn = path[i].nodeName.toLowerCase();
                for (var elem = path[i + 1].firstChild; elem; elem = elem.nextSibling) {
                    if (ndn == elem.nodeName.toLowerCase())
                        nodeIndex = nodeIndex + 1;
                    if (elem === path[i])
                        break;
                }
                //var i = Array.prototype.indexOf.call(e.childNodes, someChildEl);
                xpath += '[' + nodeIndex + ']';

            }
            //            if (path[i].className != '')
            //                xpath += '[@class=\'' + path[i].className + '\']';
        }
        else
        { xpath += nd; }
    }
    xpath = encode64(xpath);
    var result_text = parent.document.getElementById("_U_S_C2_");
    result_text.value = xpath;
    result_text = parent.document.getElementById("_U_S_C_");
    result_text.value = el_clicked.innerText || el_clicked.textContent;
    var result_text2 = parent.document.getElementById("_U_S_C_2_");
    result_text2.innerHTML = el_clicked.innerText || el_clicked.textContent;
    var result_text3 = parent.document.getElementById("_U_S_C3_");
    result_text3.value =encodeURIComponent( el_clicked.innerHTML || el_clicked.htmlContent);
    var selectButton = parent.document.getElementById("SelectContentAgain");
    selectButton.style.display = "block";
    return false;
}

