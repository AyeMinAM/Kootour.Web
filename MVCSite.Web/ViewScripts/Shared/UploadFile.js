function initializeUploadFileInput() {
    if ($("#uploadInputArea").attr("iv")!=1)
        $("#uploadInputArea").hide();
    $(".ui-input-btn ").hide();        
    $("#chooseFile").click(function (e) {
        e.preventDefault();
        $("input[type=file]").trigger("click");
    });
    $("input[type=file]").change(function () {
        var file = $("input[type=file]")[0].files[0];
        $("#preview").empty();
        displayAsImage3(file, "preview");

        $info = $("#info");
        $info.empty();
//        if (file && file.name) {
//            $info.append("<li>文件名称:<span>" + file.name + "</span></li>");
//        }
//        //        if (file && file.type) {
//        //            $info.append("<li>size:<span>" + file.type + " bytes</span></li>");
//        //        }
//        if (file && file.size) {
//            $info.append("<li>文件大小:<span>" + file.size + " bytes</span></li>");
//        }
        //        if (file && file.lastModifiedDate) {
        //            $info.append("<li>lastModifiedDate:<span>" + file.lastModifiedDate + " bytes</span></li>");
        //        }
        $info.listview("refresh");
        $("#uploadInputArea").show();
        $(".ui-input-btn").show();
        $("textarea").css("min-height", 60);
    });
};
//$('#page').on('pageinit', initializeUploadFileInput);

function displayAsImage3(file, containerid) {
    if (typeof FileReader !== "undefined") {
        var container = document.getElementById(containerid),
			    img = document.createElement("img"),
			    reader;
        container.appendChild(img);
        reader = new FileReader();
        reader.onload = (function (theImg) {
            return function (evt) {
                theImg.src = evt.target.result;
            };
        } (img));
        reader.readAsDataURL(file);
    }
}
var onSubmitUploadedFile = function () {
    showLoadingMsg();
    return true;
};

