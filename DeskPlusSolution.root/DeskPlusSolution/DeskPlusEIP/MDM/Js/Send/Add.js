Add = {}

/*******************************************************
작성자: famo23
작성일: 2020-02-26
작성목적: 초기화
********************************************************/
$(function () {
    $(document).ready(function () {

        // 전송 
        $('#cbtnSend').click(function (e) {
            e.preventDefault();
            Add.Send();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        // 조직도 
        $('#cbtnDept').click(function (e) {
            e.preventDefault();
            Add.GetUserDept();
        });

        // 추가 
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Add.GetTarget();
        });

        // 삭제 
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Add.DelTarget();
        });


        $("#tbReceive").autocomplete({
            source: function (request, response) {

                //파라미터값 배열에 담기
                ParamArray = {
                    'txt': escape($("#tbReceive").val())
                };

                CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                data = "{p:'" + GetEncryptValue + "'}";


                $.ajax({
                    url: "/DeskPlusEIP/MDM/Ws/Send/Add.asmx/GetAutoCompleteData",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: "{p:'" + GetEncryptValue + "'}",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.Name,
                                value: item.NameStr
                            }
                        }))
                        //debugger;
                    },
                    error: function (result) {
                        //alert("Error");
                    }
                });
            },
            open: function (event, ui) {
                if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                    $('.ui-autocomplete').off('menufocus hover mouseover mouseenter');
                }
            },
            select: function (event, ui) {
                $('#tbReceive').focus();
            }
        });


    });
});


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-27
작성목적 : 조직도 창 
**********************************************************************************************/
Add.GetUserDept = function () {

    if ($("#hdnCategoryID").val() == "") {
        ParamArray = {
            'callBackEvent': "SetUserDept"
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/MDM/Send/GetUserDept.aspx?p=" + GetEncryptValue;

        WindowObjectReference = PopupWinOpen(url, "", "GetUserDept", "800", "475")
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2020-02-27
작성목적 : 사용자 추가값 가져오기 
***************************************************/
function SetUserDept(rtnVal) {

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {

        var response = unescape(rtnVal);
        var tmpArr = response.split(";");

        var tmpVal = $("#tbReceive").val();

        var gubun = "";
        var oid = "";
        var did = "";
        var tname = "";
        var tmpText = "";

        //구분$OwnerID$DeptID$이름
        for (var i = 0; i < tmpArr.length; i++) {
            var tArr = tmpArr[i].split("$");

            gubun = tArr[0];
            oid = tArr[1];
            did = tArr[2];
            tname = tArr[3];

            if (oid != "") {
                if (gubun == "G") {
                    tmpText = "<@" + did + ">" + tname;
                }
                else {
                    tmpText = "<#" + oid + ">" + tname;
                }

                if ($("#tbReceive").val().indexOf(tmpText) < 0) {
                    tmpVal += (tmpVal != "") ? ("\n" + tmpText + ";") : (tmpText + ";");
                }
            }
        }

        $("#tbReceive").val(tmpVal);
        $("#tbReceive").autogrow();

    }

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-03-03
작성목적 : 알림 발송 (웹서비스 호출)
**********************************************************************************************/
Add.Send = function () {

    if (BtnCheck == "Y") {
        alert(R_Processing);
        return;
    }

    //필수입력
    var validator = new Validator($("#form1"));
    validator.checkRequired("tbxHeader", R_InputHeader, true);
    if (!validator.validate()) return;

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbReceive", R_InputReceive, true);
    if (!validator.validate()) return;

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbContent", R_InputContent, true);
    if (!validator.validate()) return;

    //2번 이상 클릭 불가
    BtnCheck = "Y";

    var url = "/DeskPlusEIP/MDM/Ws/Send/Add.asmx/AddSend";

    ParamArray = {
        'header': escape($("#tbxHeader").val()),
        'receive': escape($("#tbReceive").val()),
        'content': escape($("#tbContent").val()),
        'categoryID': $("#hdnCategoryID").val(),
        'listID': $("#hdnListID").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessSend");

    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-03-03
작성목적 : 알림 발송 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessSend = function (data, status) {
    if (data.d == "TRUE") {
        alert(R_MsgSendAlarm);
        parent.GetList();
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else if (data.d == "FALSE") {
        alert(R_ProcessingError);
        BtnCheck = "N";
        return;
    }
    else {
        var tmpRtn = data.d.split('^');
        if (tmpRtn[0] == "1") {
            alert("'" + tmpRtn[1] + "'" + R_MsgOverlap);
            BtnCheck = "N";
            return;
        }
        else if (tmpRtn[0] == "2") {
            alert("'" + tmpRtn[1] + "'" + R_MsgNoUser);
            BtnCheck = "N";
            return;
        }
        else if (tmpRtn[0] == "3") {
            alert("'" + tmpRtn[1] + "'" + R_MsgKey);
            BtnCheck = "N";
            return;
        }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-11
작성목적 : 게시판 URL 추가 
**********************************************************************************************/
Add.GetTarget = function () {

    var url = "/DeskPlusEIP/MDM/Send/GetTargetURL.aspx";

    WindowObjectReference = PopupWinOpen(url, "", "TargetURL", "860", "690")
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-15
작성목적 : 바로가기 URL 등록
**********************************************************************************************/
function SetTarget(_categoryID, _listID, _title) {

    if (_listID != "undefined" && _listID != "" && _listID != null) {

        $("#hdnCategoryID").val(_categoryID);
        $("#hdnListID").val(_listID);

        var tmpTitle = unescape(_title);

        if (tmpTitle.length > 20) {
            tmpTitle = tmpTitle.substring(0, 18) + "..";
        }

        var valtext = "<a onclick=\"Add.PopupShort('" + _categoryID + "','" + _listID + "');return false;\" style=\"cursor:pointer\">";
        valtext += tmpTitle + "</a>";

        $("#sTargetUrl").html(valtext);

        $("#tbReceive").val(R_MsgBoardAuth);
        $("#tbReceive").attr("readonly", true);
        $("#tbReceive").attr("disabled", true);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2020-06-16
작성목적 : 새창으로 게시판 글 보기
***************************************************/
Add.PopupShort = function (p_categoryID, p_listID) {
    ParamArray = {
        'catID': p_categoryID,
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "PopupShort", "1000", "700", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-16
작성목적 : 게시판 URL 삭제
**********************************************************************************************/
Add.DelTarget = function () {

    if ($("#hdnCategoryID").val() != "") {

        $("#hdnCategoryID").val('');
        $("#hdnListID").val('');
        $("#sTargetUrl").html('');
        $("#tbReceive").val('');
        $("#tbReceive").removeAttr("readonly");
        $("#tbReceive").removeAttr("disabled");
    }
}


