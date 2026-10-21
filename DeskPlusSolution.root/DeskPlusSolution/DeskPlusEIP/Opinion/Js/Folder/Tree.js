Tree = {};

$(function () {

    $(".accordion").accordion({
        fillSpace: true,
        animated: 'slide'
        //event: "mouseover",
    });

    $("#accordionResizer").resizable({
        minHeight: 0,
        resize: function () { $("#accordion").accordion("resize"); }
    });

    $('#IngVote').click(function (e) { e.preventDefault(); Tree.GoList('', 'IngVote'); });          //참여리스트
    $('#IngVoteNon').click(function (e) { e.preventDefault(); Tree.GoList('', 'IngVoteNon'); });    //미참여

    $('#CloseVote').click(function (e) { e.preventDefault(); Tree.GoList('', 'CloseVote'); });          //참여리스트
    $('#CloseVoteNon').click(function (e) { e.preventDefault(); Tree.GoList('', 'CloseVoteNon'); });    //미참여
                                                                                                    //
    $('#fav').click(function (e) { e.preventDefault(); Tree.GoList('fav', 'fav'); });               //즐겨찾기  :fav

});


Tree.GoList = function (_tpage, _menuID) {

    var url = "";
    var tmpfn = (_tpage.toUpperCase() == "FAV") ? "Fav" : "";

    switch (ListType) {
        case "1":
            url = "/DeskPlusEIP/Opinion/Content/" + tmpfn + "List.aspx"; break;
        case "2":
            url = "/DeskPlusEIP/Opinion/Content/" + tmpfn + "ListBlog.aspx"; break;
        case "3":
            url = "/DeskPlusEIP/Opinion/Content/" + tmpfn + "ListGallary.aspx"; break;
        default: 
            url = "/DeskPlusEIP/Opinion/Content/" + tmpfn + "List.aspx"; break;
    }

    ParamArray = { 'menuID': _menuID };
    CallEncrypt(CallGetParam());
    parent.frmContent.location.href = url+"?p=" + GetEncryptValue;
}