AddTab = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //탭추가
        $('#cbtnAddTab').click(function (e) {
            e.preventDefault();

            location = "AddTab.aspx";
        });

        //탭순서
        $('#cbtnOrderTab').click(function (e) {
            e.preventDefault();
            AddTab.OrderTab();
        });        

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            AddTab.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();

            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //탭현황
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            location = "/DeskPlusEIP/EIP/Admin/Portal/GetTab.aspx";
        });        

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiNamePopup($("#ctbMultiName").val(), "GetMultiName", MultiLang);
        });

        //레이아웃 버튼 클릭 이벤트
        $("[name=rbLayout]").change(function () {
            for (var i = 1; i <= 7; i++) {
                $("#imglayout" + i).attr("src", ImagePath + "/Layout/" + i + ".gif");
            }

            var selID = $('input[name=rbLayout]:checked').val();
            $("#imglayout" + selID).attr("src", ImagePath + "/Layout/" + selID + "_on.gif");
        });

        $('#ctbMultiName').attr("disabled", true);
        $('#ctbBaseName').focus();
    });
});



/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 탭순서변경
***************************************************/
AddTab.OrderTab = function () {
    var url = "ModTabOrder.aspx";
    WindowObjectReference = PopupWinOpen(url, "", "ModTabOrder", "500", "400")
}


/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 확인
***************************************************/
AddTab.Confirm = function () {

    $('#ctbMultiName').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbBaseName", R_InputTabName, true);
    if (!validator.validate()) return;

    var layout = $("input[name=rbLayout]:checked").val();
    if (layout < 1) {
        alert(R_SettingLayout);
        return;
    }

    __doPostBack('lbtnWrite', '');

}



/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbMultiName").val(multiNames);
}