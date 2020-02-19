var magnifyRatio = 1000000;
var ctrPageNo = 51666;
var OnClickCloseMapMarkerPop = function () {
    document.getElementById('webPageIframe').contentWindow.getPointPos();
    $("#ModalPopWindowOverlay").hide();
    $("#ModalPopDetail").html("");
    $("#ModalPopDetail").hide();
    var pa = $('#_m_m_a_').html();
    var pd = $('#_m_m_d_').val();
    var px = $('#_m_m_x_').val();
    var py = $('#_m_m_y_').val();
    $.post("/Home/ChangePosition/" + pd, { address: pa, x: px, y: py }, function (data) {
        var dlg = $("#PopupDialog");
        dlg.html(data).css("display:block;");
        dlg.showModalDialog($('#CloseCallResultMenu'));
    });
};
var getMapZoomLevel = function (distance) {
    distance = 1000 * distance;
    if (distance < 3000) {
        return 13;
    } else if (distance < 7000) {
        return 12;
    } else if (distance < 15000) {
        return 11;
    }
    else if (distance < 30000) {
        return 10;
    }
    else {
        return 9;
    }
}
var switchBizCircle = function (control) {
    var pid = $(control).attr("pid");
    var currentClass = $(control).attr("class");
    var nextClass = currentClass == "SwitchBizCircle Hide" ? "SwitchBizCircle Show" : "SwitchBizCircle Hide";
    var kidId = "HotPlace" + pid;
    $('.HotSpaceKids').hide();
    $('.SwitchBizCircle').attr("class", "SwitchBizCircle Hide");
    $(control).attr("class", nextClass);
    if (nextClass == "SwitchBizCircle Show") {
        $('#' + kidId).show();
    }
}
function GetSingleBizDataUrl() {
    var guid = $('#_m_m_b_i_').val();
    var px = $('#_m_m_x_').val();
    var py = $('#_m_m_y_').val();
    var cne = $('#_m_m_cne_').val();
    var target = "http://wellyours.com/Biz/" + guid + "?city=" + cne + "&cx=" + px + "&cy=" + py;
    return target;
}
function CallAjaxWithJsonp(url, successFun) {
    $.ajax({
        dataType: "jsonp",
        url: url,
        headers: {
            "X-Requested-With": "XMLHttpRequest"
        },
        success: successFun,
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

var GetSingleBizData = function () {
    var url = GetSingleBizDataUrl();
    var successFun = function (data) {
        var box = $("#SingleBizBox");
        box.html(data);
        $('iframe#mapPointsIframe').attr("src", "/Home/GaodePoints/");
    };
    CallAjaxWithJsonp(url, successFun);
};
var GetMajorCityData = function () {
    var url = "http://wellyours.com/MajorCity/";
    var successFun = function (data) {
        var box = $("#MajorCityBox");
        box.html(data);
    };
    CallAjaxWithJsonp(url, successFun);
};
var GetHotPlaceData = function () {
    var cne = $('#_m_m_cne_').val();
    var url = "http://wellyours.com/HotPlace/" + cne;
    var successFun = function (data) {
        var box = $("#HotPlaceBox");
        box.html(data);
    };
    CallAjaxWithJsonp(url, successFun);
};