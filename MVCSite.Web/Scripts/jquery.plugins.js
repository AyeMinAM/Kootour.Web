(function ($) {
    $.fn.textfill = function (options) {
        var fontSize = options.maxFontPixels;
        var ourText = $('span:visible:first', this);
        var maxHeight = $(this).height();
        var maxWidth = $(this).width() - 25;
        var textHeight;
        var textWidth;
        do {
            ourText.css('font-size', fontSize);
            textHeight = ourText.height();
            textWidth = ourText.width();
            fontSize = fontSize - 1;
        } while ((textHeight > maxHeight || textWidth > maxWidth) && fontSize > 3);
        return this;
    };
    $.fn.center = function () {
        this.css("position", "absolute");
        var top = (($(window).height() - this.outerHeight()) / 2) + $(window).scrollTop();
        var left = (($(window).width() - this.outerWidth()) / 2) + $(window).scrollLeft();
        this.css("top", top + "px");
        this.css("left", left + "px");
        return this;
    };

    $.fn.showModalDialog = function ($closeButton) {
        var $dialog = this;
        if ($dialog.length == 0)
            return;
        var $overlay = $('#modal-overlay');

        $overlay.show();
        $dialog.children().show();
        //        $dialog.css('z-index', 255).center();
        $dialog.css('z-index', 255);
        $dialog.show();

        if ($closeButton) {
            $closeButton.click(function () {
                $dialog.hide();
                $overlay.hide();
                return false;
            });
        }
        return this;
    };

    $.fn.showDialog = function (_url, _dlgId, _closeId, _leftOffset, _topOffset, _jsonp) {
        var $dialog = $(_dlgId);
        if ($dialog.length == 0)
            return;

        var windowWidth = $(window).width();
        var windowHeight = $(window).height();
        var offset = null, postLeft = _leftOffset, posTop = _topOffset;
        if (_topOffset == null || _leftOffset == null) {
            offset = $(this).offset();
            postLeft = offset.left, posTop = offset.top + 35;
        }
        var showDialogCallBack = function (data) {
            $dialog.html(data);
            var firstDiv = $dialog.children("div:first");
            var firstDivWidth = firstDiv.width();
            var firstDivHeight = firstDiv.height();
            if (postLeft + firstDivWidth + 10 > windowWidth)
                postLeft = windowWidth - firstDivWidth - 10;

            if (posTop + firstDivHeight + 10 > windowHeight)
                posTop = windowHeight - firstDivHeight - 10;
            if (posTop < 0)
                posTop = 18;
            if (postLeft < 0)
                postLeft = 10;
            firstDiv.css({ left: postLeft, top: posTop });
            $dialog.children().show();
            $('#SecondPopupDialog').hide();
            $dialog.css('z-index', 255);
            $dialog.show();
            if (_closeId != null) {
                $(_closeId).click(function () {
                    var dlg = $(_dlgId);
                    dlg.hide();
                    return false;
                });
            }
            return;
        };
        var url = encodeURI(_url);
        if (_jsonp == null || typeof (_jsonp) == 'undefined' || !_jsonp)
            $.get(url, showDialogCallBack);
        else {
//            $.ajaxSetup({
//                beforeSend: function (xhr) {
//                    xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
//                }
            //            });
            $.ajax({
                dataType: "jsonp",
                url: url,
                //crossDomain: false,
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                },
                success: showDialogCallBack,
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }
        return this;
    };


    $.fn.showDialogInClient = function (_dlgId, _htmlId,_leftOffset, _topOffset) {
        var $dialog = $(_dlgId);
        if ($dialog.length == 0)
            return;
        var windowWidth = $(window).width();
        var windowHeight = $(window).height();
        var offset = null, postLeft = _leftOffset, posTop = _topOffset;
        if (_topOffset == null || _leftOffset == null) {
            offset = $(this).offset();
            postLeft = offset.left, posTop = offset.top + 35;
        }
        $dialog.html($(_htmlId).html());
        var firstDiv = $dialog.children("div:first");
        var firstDivWidth = firstDiv.width();
        var firstDivHeight = firstDiv.width();
        if (postLeft + firstDivWidth > windowWidth)
            postLeft = windowWidth - firstDivWidth - 30;
        if (posTop + firstDivHeight > windowHeight)
            posTop = windowHeight - firstDivHeight - 30;
        firstDiv.css({ left: postLeft, top: posTop });
        $dialog.children().show();
        $dialog.css('z-index', 255);
        $dialog.show();
        return this;
    };

})(jQuery);
