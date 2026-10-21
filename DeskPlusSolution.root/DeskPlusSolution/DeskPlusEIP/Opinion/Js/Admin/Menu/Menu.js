Menu = {}

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

    //예시관리  :exam
    $("#exam").click(function () {
        ParamArray = { 'menuID': 'exam' };
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = "/DeskPlusEIP/Opinion/Admin/Content/UnitList.aspx?p=" + GetEncryptValue;
    });

    //질문관리  :unit
    $("#unit").click(function () {
        ParamArray = { 'menuID': 'unit' };
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = "/DeskPlusEIP/Opinion/Admin/Content/UnitList.aspx?p=" + GetEncryptValue;
    });

    //설문관리(진행중) :qing
    $("#qing").click(function () {
        ParamArray = { 'menuID': 'qing' };
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = "/DeskPlusEIP/Opinion/Admin/Content/QuesList.aspx?p=" + GetEncryptValue;
    });

    //설문관리(마감)  :qcolsing
    $("#qclosing").click(function () {
        ParamArray = { 'menuID': 'qclosing' };
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = "/DeskPlusEIP/Opinion/Admin/Content/QuesList.aspx?p=" + GetEncryptValue;
    });

    //설문통계

});


/*Menu.MovePage = function (_target) {
    var targetURL = "";
    switch (_target) {
        case "Ques":
            targetURL = "../Content/QuesList.aspx";
            break;
        case "Unit":
            targetURL = "../Content/UnitList.aspx";
            break;
        default:
            targetURL = "../Content/QuesList.aspx";
            break;
    }
    parent.frmContent.location.href = targetURL;
}*/