var _ContentResultOK = "OK";
var _ContentResultFailed = "FAILED";
var myScroll;
var pageSize = 3;
var maxTalkId = 0,selectedId=0;
var pageUrl = "", popupUrl = "", prevPageUrl = "", nextPageUrl = "", lastPageUrl = "";
var pageNo = 0;
var scroll_in_progress = false, isLogging = true, isNextPageDown = true, isLogBrowserMsg = true, isRecording = false;

function loaded() {
//    myScroll = new IScroll('#wrapper', {
//        scrollbars: true,
//        mouseWheel: true,
//        interactiveScrollbars: true,
//        shrinkScrollbars: 'scale',
//        fadeScrollbars: true
//    });
//    myScroll = new IScroll('#wrapper', { mouseWheel: true, hScrollbar: true, vScrollbar: true, scrollbars: true });
//    myScroll.scrollTo(0, myScroll.maxScrollY, 0);
    //    myScroll = new IScroll('#wrapper', { mouseWheel: true, hScrollbar: true, vScrollbar: true });
    load_content();
//    setTimeout(function () {
//        load_content();
//    }, 1000);


}
document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);

//$(window).load(function () {
//    logMessage("$(window).load ENTERED!");
//    var container = document.getElementById("wrapper");
//    if (container != null) {
//        loaded();
//    }
//});
$(function () {
    logMessage("DOM READY ENTERED!");
    var container = document.getElementById("wrapper");
    if (container != null) {
        loaded();
    }
    $(window).resize(function () {
        calculateScrollerHeight();
    });
    calculateScrollerHeight();

});

var popCardModalDialog = function (data) {
    $("#PopupDialog").hide();
    var dlg = $("#CardAllInformationBlock");
    dlg.html(data);
    $("#CardWindowOverlay").show();
    dlg.show();
};
var ClosePopUpDialog = function () {
    $("#AllInformationBlock").hide();
    $("#CardWindowOverlay").hide();
};
var OnClickClosePopUp = function () {
    ClosePopUpDialog();
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
var refreshScroller = function (data) {
    lastPageUrl = "";
    if (data.Result) {
        if (data.Prev != null)
            prevPageUrl = data.Prev;
        if (data.Next != null)
            nextPageUrl = data.Next;
        if (data.MaxId != null) {
            maxTalkId = data.MaxId;
        }
        if (data.MaxId != null) {
            maxTalkId = data.MaxId;
        }
        $('#scrollerContent').html(data.Html);
        $("#scroller ul").listview();
        $("#scroller form").trigger('create');
        if (myScroll != null)
            myScroll.refresh();
        if (nextPageUrl != "") {
            if (isNextPageDown)
                $('#wrapper .pullUp').show();
            else
                $('#wrapper .pullDown').show();
        }
        else {
            if (isNextPageDown)
                $('#wrapper .pullUp').hide();
            else
                $('#wrapper .pullDown').hide();
            if (typeof(onReachScrollerEnd) != 'undefined') {
                onReachScrollerEnd();
            }
        }
        calculateScrollerHeight();
    }
    else if (data.Result == false) {
        redirectToInternalErrorPage();
    }
    else {
        redirectToLogonPage();
    }
};
var appendToScrollerEnd = function (data) {
    logMessage("appendToScrollerEnd");
    if (data.Result) {
        if (data.Prev != null)
            prevPageUrl = data.Prev;
        if (data.Next != null)
            nextPageUrl = data.Next;
        if (data.MaxId != null) {
            maxTalkId = data.MaxId;
        }
        var subContent = document.getElementById("scrollerSubContent");
        if (subContent != null) {
            $('#scrollerSubContent').append(data.Html);
        }
        else {
            $('#scrollerContent').append(data.Html);
        }
        $("#scroller ul").listview();
        if (myScroll != null) {
            myScroll.refresh();
            if (maxTalkId > 0) {
                myScroll.scrollTo(0, myScroll.maxScrollY, 200);
            }
        }
    }
    else {
        redirectToInternalErrorPage();
    }
};
var appendToScrollerHead = function (data) {
    logMessage("appendToScrollerHead");
    if (data.Result) {
        if (data.Prev != null)
            prevPageUrl = data.Prev;
        if (data.Next != null)
            nextPageUrl = data.Next;
        if (data.MaxId != null) {
            maxTalkId = data.MaxId;
        }
        var subContent = document.getElementById("scrollerSubContent");
        if (subContent != null) {
            $('#scrollerSubContent').prepend(data.Html);
        }
        else {
            $('#scrollerContent').prepend(data.Html);
        }
        $("#scroller ul").listview();
        if (myScroll != null)
            myScroll.refresh();
    }
    else {
        redirectToInternalErrorPage();
    }
};
var OnPlayVoice = function (ap) {
    window.location.href = ap;
};
var OnPlayPeerVoice = function (ap, ti, ci) {
    OnPlayVoice(ap);
    var url = "/Communicate/ReadTalk/" + ti + "?ci=" + ci;
    $.post(url);
};
var load_content = function (refresh, next_page) {
    logMessage("load_content() refresh=" + refresh + " next_page=" + next_page);
    if (!refresh) {
        // Loading the initial content
        //        $('#wrapper > #scroller > ul').append('<li>Pretty row initial content</li>');
        //        $('#wrapper > #scroller > ul').append('<li>Pretty row initial content</li>');
        //        $('#wrapper > #scroller > ul').append('<li>Pretty row initial content</li>');
        //        $('#wrapper > #scroller > ul').append('<li>Pretty row initial content</li>');
        //        $('#wrapper > #scroller > ul').append('<li>Pretty row initial content</li>');
        //        $('#wrapper > #scroller > ul').append('<li>Pretty row initial content</li>');
        //        $('#wrapper > #scroller > ul').append('<li>Pretty row initial content</li>');
        //        $('#wrapper > #scroller > ul').append('<li>Pretty row initial content</li>');
        //        $('#wrapper > #scroller > ul').append('<li>Pretty row initial content</li>');
        //        $('#wrapper > #scroller > ul').append('<li>Pretty row initial content</li>');
    } else if (refresh && !next_page) {

    } else if (refresh && next_page) {
        // Loading the next-page content and refreshing
        //        alert("nextPageUrl=" + nextPageUrl);
        if (nextPageUrl != "") {
            logMessage("nextPageUrl=" + nextPageUrl);
            if (nextPageUrl != lastPageUrl) {
                if (isNextPageDown)
                    postData(nextPageUrl, null, appendToScrollerEnd);
                else
                    postData(nextPageUrl, null, appendToScrollerHead);
                lastPageUrl = nextPageUrl;
            }
            else {
                logMessage("REFRESH IGNORED AS nextPageUrl == lastPageUrl=" + nextPageUrl);            
            }
        }
        else {
            if (isNextPageDown)
                $('#wrapper .pullUp').hide();
            else
                $('#wrapper .pullDown').hide();
            if (typeof (onReachScrollerEnd) != 'undefined') {
                onReachScrollerEnd();
            }
        }
    }

    if (refresh) {
        if (myScroll != null)
            myScroll.refresh();
        pullActionCallback();

    } else {

        if (myScroll) {
            myScroll.destroy();
            $(myScroll.scroller).attr('style', ''); // Required since the styles applied by IScroll might conflict with transitions of parent layers.
            myScroll = null;
        }
        trigger_myScroll();

    }
};

function pullDownAction() {
    logMessage("pullDownAction() ");
    if (isNextPageDown)
        return;
    load_content('refresh', true);
//    $('#wrapper > #scroller > ul').data('page', 1);

    // Since "topOffset" is not supported with iscroll-5
    $('#wrapper > .scroller').css({ top: 0 });

}
function pullUpAction(callback) {
    logMessage("pullUpAction() ");
    if (!isNextPageDown)
        return;

//    if ($('#wrapper > #scroller > ul').data('page')) {
//        var next_page = parseInt($('#wrapper > #scroller > ul').data('page'), 10) + 1;
//    } else {
//        var next_page = 2;
    //    }
    load_content('refresh', true);
//    $('#wrapper > #scroller > ul').data('page', next_page);

    if (callback) {
        callback();
    }
}
function pullActionCallback() {
    if (pullDownEl && pullDownEl.className.match('loading')) {

        pullDownEl.className = 'pullDown';
        pullDownEl.querySelector('.pullDownLabel').innerHTML = $('#wrapper').attr("ds");

        myScroll.scrollTo(0, parseInt(pullUpOffset) * (-1), 200);

    } else if (pullUpEl && pullUpEl.className.match('loading')) {
        pullUpEl.className = 'pullUp';
        pullUpEl.querySelector('.pullUpLabel').innerHTML = $('#wrapper').attr("us");
    }
}

var pullActionDetect = {
    count: 0,
    limit: 10,
    check: function (count) {
        if (count) {
            pullActionDetect.count = 0;
        }
        // Detects whether the momentum has stopped, and if it has reached the end - 200px of the scroller - it trigger the pullUpAction
        setTimeout(function () {
            logMessage("myScroll.y=" + myScroll.y + " myScroll.minScrollY=" + myScroll.minScrollY + " myScroll.maxScrollY=" + myScroll.maxScrollY);
            if (myScroll.y == 0 && myScroll.maxScrollY == 0) {//the first scroll action
                if (isNextPageDown)
                    pullUpAction();
                else
                    pullDownAction();
            }
            else if (myScroll.y >= 10 && pullDownEl && !pullDownEl.className.match('loading')) {
                $('.pullDown').addClass('loading').html('<span class="pullDownIcon">&nbsp;</span><span class="pullDownLabel">Loading...</span>');
                pullDownAction();
            }
            else if (myScroll.y <= (myScroll.maxScrollY + 200)) {
                if (isNextPageDown && pullUpEl && !pullUpEl.className.match('loading')) {
                    $('.pullUp').addClass('loading').html('<span class="pullUpIcon">&nbsp;</span><span class="pullUpLabel">Loading...</span>');
                    pullUpAction();
                }
                //                else if (!isNextPageDown && pullDownEl && !pullDownEl.className.match('loading')) {
                //                    $('.pullDown').addClass('loading').html('<span class="pullDownIcon">&nbsp;</span><span class="pullDownLabel">Loading...</span>');
                //                    pullDownAction();                
                //                }
            } else if (pullActionDetect.count < pullActionDetect.limit) {
                pullActionDetect.check();
                pullActionDetect.count++;
            }
            else {
                pullActionDetect.check(0);
                pullActionCallback();
                //                load_content();
            }
        }, 200);
    }
}

function trigger_myScroll(offset) {
    pullDownEl = document.querySelector('#wrapper .pullDown');
    if (pullDownEl) {
        pullDownOffset = pullDownEl.offsetHeight;
    } else {
        pullDownOffset = 0;
    }
    pullUpEl = document.querySelector('#wrapper .pullUp');
    if (pullUpEl) {
        pullUpOffset = pullUpEl.offsetHeight;
    } else {
        pullUpOffset = 0;
    }

    if ($('#wrapper ul > li').length < pageSize) {
        // If we have only 1 page of result - we hide the pullup and pulldown indicators.
        $('#wrapper .pullDown').hide();
//        $('#wrapper .pullUp span').hide();
        offset = 0;
    } else if (!offset) {
        // If we have more than 1 page of results and offset is not manually defined - we set it to be the pullUpOffset.
        offset = pullUpOffset;
    }

    myScroll = new IScroll('#wrapper', {
        probeType: 1,
        tap: true,
        click: false,
        preventDefaultException: { tagName: /.*/ },
        mouseWheel: true,
        scrollbars: true,
//        fadeScrollbars: true,
//        interactiveScrollbars: false, 
        keyBindings: false,
        deceleration: 0.0002,
        startY: (parseInt(offset) * (-1))
    });

    myScroll.on('scrollStart', function () {
        scroll_in_progress = true;
    });
    myScroll.on('scroll', function () {

        scroll_in_progress = true;

        if ($('#wrapper ul > li').length >= pageSize) {
            if (this.y >= 5 && pullDownEl && !pullDownEl.className.match('flip')) {
                pullDownEl.className = 'pullDown flip';
                pullDownEl.querySelector('.pullDownLabel').innerHTML = 'Release to refresh';
                this.minScrollY = 0;
            } else if (this.y <= 5 && pullDownEl && pullDownEl.className.match('flip')) {
                pullDownEl.className = 'pullDown';
                pullDownEl.querySelector('.pullDownLabel').innerHTML = 'Pull down to refresh';
                this.minScrollY = -pullDownOffset;
            }

            console.log(this.y);
            pullActionDetect.check(0);

        }
    });
    myScroll.on('scrollEnd', function () {
        //        console.log('scroll ended');
        setTimeout(function () {
            scroll_in_progress = false;
        }, 100);
        if ($('#wrapper ul > li').length >= pageSize) {
//            if (pullDownEl && pullDownEl.className.match('flip')) {
//                pullDownEl.className = 'pullDown loading';
//                pullDownEl.querySelector('.pullDownLabel').innerHTML = 'Loading...';
//                pullDownAction();
//            }
//            // We let the momentum scroll finish, and if reached the end - loading the next page
//            pullActionDetect.check(0);
        }
        else {
            logMessage("$('#wrapper ul > li').length =" + $('#wrapper ul > li').length + " pageSize=" + pageSize);        
        }
    });
//    myScroll.scrollTo(0, myScroll.maxScrollY, 0);
    // In order to prevent seeing the "pull down to refresh" before the iScoll is trigger - the wrapper is located at left:-9999px and returned to left:0 after the iScoll is initiated
    setTimeout(function () {
        $('#wrapper').css({ left: 0 });
    }, 100);
}

function AjaxPostForm(btnClicked, successCallBack) {
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
        successFun = successCallBack;
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
var bindPressTalkEvent = function () {
    //    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
    //        $("a#PressTalk").on("vmouseover", function () {
    //            $("a#PressTalk").addClass("ui-btn-active");
    //            window.location.href = $('a#PressTalkTriggerStart').attr('href');
    //            return false;
    //        })
    //        .on("vmouseup", function () {
    //            $("a#PressTalk").removeClass("ui-btn-active");
    //            popupRecordedAudioInfoDialog();
    //            window.location.href = $('a#PressTalkTriggerStop').attr('href');
    //            return false;
    //        });
    //        $('a#PressTalk').on('vmousemove', function (e) {
    //            e.preventDefault();
    //        });
    //        $('a#PressTalk').on('vmousedown', function (e) {
    //            e.preventDefault();
    //        });
    //        $('a#PressTalk').on('taphold', function (e) {
    //            e.preventDefault();
    //        });
    //    }
    //    else {
    //        $("a#PressTalk").mousedown(function () {
    //            $("a#PressTalk").addClass("ui-btn-active");
    //            window.location.href = $('a#PressTalkTriggerStart').attr('href');
    //            return false;

    //        })
    //        .mouseup(function () {
    //            $("a#PressTalk").removeClass("ui-btn-active");
    //            popupRecordedAudioInfoDialog();
    //            window.location.href = $('a#PressTalkTriggerStop').attr('href');
    //            return false;
    //        });
    //    }
    $("a#PressTalk").bind("tap", function (event) {
        if (!isRecording) {
            $("a#PressTalk").addClass("ui-btn-active");
            $("a#PressTalk").html($("a#PressTalk").attr("et"));
            window.location.href = $('a#PressTalkTriggerStart').attr('href');
            setTimeout(function () {
                if ($("a#PressTalk").hasClass("ui-btn-active")) {
                    doStopRecording();
                }
            }, 60000);
            isRecording = !isRecording;
        }
        else {
            doStopRecording();
        }
        return false;
    });
};
var doStopRecording = function (data) {
    $("a#PressTalk").removeClass("ui-btn-active");
    $("a#PressTalk").html($("a#PressTalk").attr("bt"));
    popupRecordedAudioInfoDialog();
    window.location.href = $('a#PressTalkTriggerStop').attr('href');
    isRecording = !isRecording;
};

var popupModalDialog = function (data) {
    //                popCardModalDialog(data);
    //                adjustModalPopToRealWindow();

    $('#popupDialog').html(data);
    // then trigger the create event to make jQM markup the insert html properly and attach the correct events etc.
    //        $('#popupDialog').trigger('create');
    $("#popupDialog").popup("open");
};
var popupModalDialogJson = function (response) {
    if (response.Result) {
        popupModalDialog(response.Html);
        $("#popupDialog ul").listview();
    }
    else {
        popupModalDialog($('#ActionResultFailedHint').html());
    }
}
var popupRecordedAudioInfoDialog = function () {
    var container = document.getElementById("RecordedAudioInfo");
    if (container == null) {
        return;
    }
    popupModalDialog($("#RecordedAudioInfo").html());
    setTimeout(function () {
        $("#PlayRecordedFile").trigger("click");
    }, 300);
}
var logMessage = function (msg) {
//    if (isLogBrowserMsg) {
//        window.location.href = $("#PageBody").attr("lbmp").replace("LOGMSG", encodeURIComponent(msg))+"";        
//        return;
//    }
    if (isLogging === false)
        return;
    var m;
    if (typeof (window.console) === "undefined") {
        return;
    }
    m = "[" + new Date().toTimeString() + "] VJiaoshi: " + msg;
    if (window.console.debug) {
        window.console.debug(m);
    } else if (window.console.log) {
        window.console.log(m);
    }
};

var refreshScrolerToLastPart = function () {
    if (myScroll == null || typeof (myScroll) === "undefined")
        return;
    myScroll.refresh();
    //        myScroll.scrollTo(0, -500, true);
//            myScroll.scrollToElement('li#lastTalkPos', 100);
    myScroll.scrollTo(0, myScroll.maxScrollY, 0);
};
function calculateScrollerHeight() {
    var totalH = $(window).height();
    var headH = $('div#PageHead').height();
    logMessage("headH =" + headH);
//    if (headH > 0)
//        headH = headH + 15;
    var footerH = 0;
    if (!$("div.footer").is(':hidden')) {
        footerH = $('div.footer').height()+30;
    }
    logMessage("footerH =" + footerH);
    var scollerH = totalH - headH - footerH ;
    $('div#wrapper').css("height", scollerH);
};
var redirectToLogonPage = function () {
    window.location.href = "/Account/LogOn?returnUrl=/Communicate/";
}
var redirectToInternalErrorPage = function () {
    window.location.href = "/Static/MInternalError";
}
var showLoadingMsg = function () {
    var $this = $("#LoadingMsg"),
        theme = $this.jqmData("theme") || $.mobile.loader.prototype.options.theme,
        msgText = $this.jqmData("msgtext") || $.mobile.loader.prototype.options.text,
        textVisible = $this.jqmData("textvisible") || $.mobile.loader.prototype.options.textVisible,
        textonly = !!$this.jqmData("textonly");
    html = $this.jqmData("html") || "";
    $.mobile.loading("show", {
        text: msgText,
        textVisible: textVisible,
        theme: theme,
        textonly: textonly,
        html: html
    });
};
var hideLoadingMsg = function () {
    $.mobile.loading("hide");
    $("#LoadingMsg").hide();
};

var postData = function (url, queryData, callback) {
    showLoadingMsg();
    $.post(url, queryData,
        function (data) {
            callback(data);
            setTimeout(function () {
                hideLoadingMsg();
            }, 8000);
            hideLoadingMsg();
        }
    );
};
