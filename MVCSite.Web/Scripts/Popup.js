
function checkBrowser() {
//    var userAgent=navigator.userAgent.toLowerCase();
//    var is_opera=((userAgent.indexOf("opera")!=-1)||(typeof (window.opera)!="undefined"));
//    var is_saf=((userAgent.indexOf("applewebkit")!=-1)||(navigator.vendor=="Apple Computer, Inc."));
//    var is_webtv=(userAgent.indexOf("webtv")!=-1);
//    var is_ie=((userAgent.indexOf("msie")!=-1)&&(!is_opera)&&(!is_saf)&&(!is_webtv));
//    var is_ie4=((is_ie)&&(userAgent.indexOf("msie 4.")!=-1));
//    var is_ie7=((is_ie)&&(userAgent.indexOf("msie 7.")!=-1));
//    var is_ie8 = ((is_ie) && (userAgent.indexOf("msie 8.") != -1));
//    var is_ie8 = ((is_ie) && (userAgent.indexOf("msie 8.") != -1));
//    var is_ie8 = ((is_ie) && (userAgent.indexOf("msie 8.") != -1));
//    var is_ps3=(userAgent.indexOf("playstation 3")!=-1);
//    var is_moz=((navigator.product=="Gecko")&&(!is_saf));
//    var is_kon=(userAgent.indexOf("konqueror")!=-1);
//    var is_ns=((userAgent.indexOf("compatible")==-1)&&(userAgent.indexOf("mozilla")!=-1)&&(!is_opera)&&(!is_webtv)&&(!is_saf));
//    var is_ns4=((is_ns)&&(parseInt(navigator.appVersion)==4));
//    var is_mac=(userAgent.indexOf("mac")!=-1);
//    if(is_ie7)
//    {
//        alert("Your browser is not supported!");
    //    }
    if ($.browser.msie) {
        if (/*$.browser.version == '6.0' || $.browser.version == '7.0' || */$.browser.version == '8.0' || $.browser.version == '9.0' 
        || $.browser.version == '10.0' || $.browser.version == '11.0') {
            return true;
        }
    }
    else if ($.browser.mozilla) {
        return true;
    }
    var is_chrome = /chrome/.test( navigator.userAgent.toLowerCase() );
    if (is_chrome) {
        return true;
    }
    return false;

}
//if (!checkBrowser()) {
//    location.href = "/Static/BrowserWarning/";
//}

function getActivities() {
    RefreshBoardActivities();
    window.setTimeout("getActivities()", 60000);
};
function adjustHeightToRealWindow() {
    var totalH = $(window).height();
    var headH = $('div#header').height();
    var boardHeadH = $('div#board-header').height();
    var boardH = totalH - headH - boardHeadH - 20;
    if (boardH < 50)
        boardH = 50;
    var rightTopH = $('div#BoardRightColumnTop').height();
    var actH = totalH - headH - boardHeadH - rightTopH - 30;
    if (actH < 50)
        actH = 50;
    //var listH = totalH - headH - boardHeadH - 103;
    var listH = totalH - headH - boardHeadH - 70;
    $('div.BoardListContainer').css("max-height", listH-4);
    $('div.list-card-area').css("max-height", listH);
    
    $('div#board div:first').css("height", boardH);
    $('div#BoardActivityListNoHeader').css("max-height", actH);
//    $('div#FavCardAllInformationBlock').css("height", totalH - 20);
//    $('iframe#webPageIframe').css("height", totalH - 205);

};
function adjustModalPopToRealWindow() {
    var totalW = $(window).width();
    var elementW = 0;
    var elementLeftW = 0;
    var totalH = $(window).height();
    var elementH = 0;
    var elementTopH = 0;
    $('div#CardAllInformationBlock,div#ModalPopDetail').each(function (index) {
        if ($(this) != null) {
            elementW = $(this).width();
            elementLeftW = Math.round((totalW - elementW) / 2);
            $(this).css("left", elementLeftW);
            elementH = $(this).height();
            elementTopH = Math.round((totalH - elementH) / 2);
            $(this).css("top", elementTopH);
        }
    });
};
function adjustWidthToRealWindow() {

};


function adjustSizeToRealWindow() {
    adjustHeightToRealWindow();
    adjustWidthToRealWindow();
    return;
}
var refreshPopupDialogWithResponse = function (response) {
    var $dialog = $('#PopupDialog');
    var firstDiv = $dialog.children("div:first");
    var offset = firstDiv.offset();
    $dialog.html(response);
    firstDiv = $dialog.children("div:first");
    firstDiv.css({ left: offset.left, top: offset.top });
    firstDiv.show();
};
var closePopupDialog = function () {
    $('#PopupDialog').hide();
};
function AjaxPostFormCallBackOKRedirect(response) {
    if (response.result) {
        $('#PopupDialog').hide();
        $('#modal-overlay').hide();
        location.href = response.html;
    }
    else {
        var $dialog = $('#PopupDialog');
        var firstDiv = $dialog.children("div:first");
        var offset = firstDiv.offset();
        $dialog.html(response.html);
        firstDiv = $dialog.children("div:first");
        firstDiv.css({ left: offset.left, top: offset.top });
    }
};


function AjaxPostForm(btnClicked,successCallBack) {
    var $form = $(btnClicked).parents('form');
    var successFun = function (response) {
        if (response == "OK") {
            $('#PopupDialog').hide();
            $('#modal-overlay').hide();
        }
        else {
            var $dialog = $('#PopupDialog');
            var firstDiv = $dialog.children("div:first");
            var offset = firstDiv.offset();
            $dialog.html(response);
            firstDiv = $dialog.children("div:first");
            firstDiv.css({ left: offset.left, top: offset.top });
        }
    };
    if (successCallBack != null && typeof (successCallBack) != "undefined") { 
        successFun= successCallBack;   
    }
    $.ajax({
        type: "POST",
        url: $form.attr('action'),
        data: $form.serialize(),
        error: function (xhr, status, error) {
            //do something about the error
            alert("Post Error!");
        },
        success: successFun
    });
    return false;
}

function RefreshBoardActivities() {
    var bid = $('#curBoardID').val();
    var url = '/Board/ActivityGetList/' + bid;
    $.get(url, function (data) {
        var totalH = $(window).height();
        var headH = $('div#header').height();
        var boardHeadH = $('div#board-header').height();
        var rightTopH = $('div#BoardRightColumnTop').height();
        var actH = totalH - headH - boardHeadH - rightTopH - 30;
        if (actH < 50)
            actH = 50;
        $('#BoardActivityList').html(data);
        $('div#BoardActivityListNoHeader').css("max-height", actH);
        return false;
    });
    adjustHeightToRealWindow();

    //Added by Robert
    window.setTimeout(function () {
        var $fakeFrame = $('<iframe style="height:0;width:0;display:none" src="/Content/Images/LogoImg30X30-Blue.png"></iframe>');
        $('body').append($fakeFrame);
        $fakeFrame.remove();
    }, 1500);
    return false;
}
function RefreshCardActivities() {
    var cid = $('#curCardID').val();
    var url = '/Card/ActivityList/' + cid;
    $.get(url, function (data) {
        $('#CardActivityList').html(data);
        return false;
    });
    return false;
}

var clientRedirect;
$(function () {

//    $('#PopupDialogForm').submit(function () {
//        $('#SubmitEmail').attr('disabled', 'disabled');
//        var options = {
//            dataType: 'json',
//            success: function (data) {
//                if (data.redirect && clientRedirect != null)    // some link that required authentication has been clicked and signed in sucessfully (and it's not a signup attempt)
//                    window.location.href = decodeURIComponent(clientRedirect);
//                else if (data.redirect)                         // sign in link has been clicked and signed in sucessfully 
//                    window.location.href = data.redirect;
//                else {                                          // validation error occured
//                    $("#PopupDialog").replaceWith(data.html);
//                    $("#PopupDialog").showModalDialog();
//                }
//            },
//            error: function () {
//                $("#PopupDialog").hide();
//                $('#SubmitEmail').removeAttr('disabled');
//                error('Sorry, internal error, please try again later');
//            }
//        };
//        $(this).ajaxSubmit(options);
//        return false;
//    });

    $("#modal-overlay").click(function () {
        $("#PopupDialog").hide();
        $('#modal-overlay').hide();
    });

    $('#ReceiveNewsletter').click(function () {
        $('#PopupDialog').showModalDialog($('#PopupDialog #closeLink'));
        clientRedirect = window.location.href;
        return false;
    });




});