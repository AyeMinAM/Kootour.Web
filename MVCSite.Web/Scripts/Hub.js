var boardHub = $.connection.boardHub;
var RefreshBoardList = function (listId) {
    var url = '/Board/GetList/' + listId;
    $.getJSON(url, function (data) {
        $('div[id=' + data.lid + ']').html(data.html);
    });
};
var RefreshBoardListContent = function (boardId) {
    var url = '/Board/ListContent/' + boardId;
    $.get(url, function (data) {
        $('#BoardListContent').html(data);
    });
};

var RefreshCardAttachments = function (cardId) {
    var url = '/Card/AttachmentList/' + cardId;
    $.get(url, function (data) {
        $('#AttachmentList').html(data);
    });
};
boardHub.updateList = function (listId) {
    RefreshBoardList(listId);
    RefreshBoardActivities();
};
boardHub.updateListContent = function (boardId) {
    RefreshBoardListContent(boardId);
    RefreshBoardActivities();
};
boardHub.updateCardAttachments = function (cardId) {
    RefreshCardAttachments(cardId);
    RefreshBoardActivities();
    RefreshCardActivities();
};

boardHub.moveCard = function (boardId) {
    RefreshBoardListContent(boardId);
    RefreshBoardActivities();
};

boardHub.updateFavListContent = function (favId) {
    var url = '/Favorite/ListContent/' + favId;
    $.get(url, function (data) {
        $('#FavoriteListContent').html(data);
    });
};

boardHub.wordSetJson = function (index, json, nextMax) {
    $('#curMaxInSet').val(nextMax);
    var djson = s_2_c(json);
    if (index == 1)
        $('#curJsonWord1').val(djson);
    else
        $('#curJsonWord2').val(djson);
};