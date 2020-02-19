var getRandom = function (Max) {
    return Math.ceil(Math.random() * Max) - 1;
};

$(function() {

    $('#HeaderFavorite').click(function () {
        $(this).showDialog("/Favorite/MyFavoritesMenu/", "#PopupDialog", "#CloseMyFavoritesMenu");
        return false;
    });
    $('#header_boards').click(function () {
        $(this).showDialog("/Board/MyBoardsMenu/", "#PopupDialog", "#CloseMyBoardsMenu");
        return false;
    });
    $('#HeaderMyBooks').click(function () {
        $(this).showDialog("/Word/MyBooksMenu/", "#PopupDialog", "#CloseMyBooksMenu");
        return false;
    });
    $('div.header-btn.header-auth').click(function(){ 
        var cid=$(this).attr('id');
        $(this).showDialog("/Account/InitialContext/","#PopupDialog","#CloseInitialContext");
        return false;
    });
//            $('body').click(function (event) {
//                var clicked = $(event.target);
//                if(clicked.is("a#CloseShowCard"))
//                {
//                    window.location.href=$('#curBoardUrl').val();
//                    return false;
//                }
//                if (clicked.is("#PopupDialog") || clicked.parents().is("#PopupDialog")) 
//                {
//                    return false;
//                }
//                else if (clicked.is("#CardAllInformationBlock") || clicked.parents().is("#CardAllInformationBlock")) 
//                {
//                    return true;
//                }
//                else
//                {
//                    if($('#CardAllInformationBlock').length > 0)
//                    {
//                        window.location.href=$('#curBoardUrl').val();
//                        return false;
//                    }
//                    if($('#PopupDialog').length > 0)
//                    {
//                        $("#PopupDialog").hide();
//                    }
//                }
//                return true;
//            });
    $(window).resize(function() {
        adjustSizeToRealWindow();
    });
    adjustSizeToRealWindow();

//    window.setTimeout(function() {
//        var $fakeFrame = $('<iframe style="height:0;width:0;display:none" src="/Content/Images/LogoImg30X30-Blue.png"></iframe>');
//        $('body').append($fakeFrame);
//        $fakeFrame.remove();
//    }, 1500);
});

