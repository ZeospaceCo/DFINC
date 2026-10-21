UnitAdd = {}

$(function () {
    $(document).ready(function () {

        //이벤트
        $("#ddlType").change(function (e) { e.preventDefault(); UnitAdd.ShowPoint(); });

        //항목입력
        $("#txtItem").keydown(function (event) {if (event.keyCode == 13) { UnitAdd.AddItem();} });

        //[확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); UnitAdd.Add(); });

        //[삭제]
        $('#cbtnDel').click(function (e) { e.preventDefault(); UnitAdd.Del(); });

        //[취소]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); UnitAdd.Cancel(); });

        //[항목순서]
        $('#cbtnSeq').click(function (e) { e.preventDefault(); UnitAdd.ModSeq(); });

        //[질문생성]
        $('#cbtnUnitAdd').click(function (e) { e.preventDefault(); UnitAdd.AddUnit(); });

        $('#txtItem').focus(function (e) { e.preventDefault(); if ($('#txtItem').val() == Txt_TitUnitAdd) $('#txtItem').val(''); });

        UnitAdd.Init();

        if (UnitID != 0) {
            setTimeout(function () { UnitAdd.LoadItem(); }, 200);
        }

    });
});

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 초기함수
***************************************************/
UnitAdd.Init = function () {

    // [예시선택] / 설명부분
    if (Section == "0") {
        $("#spUnitTxt").show(); $("#spExamTxt").hide();
    } else {
        $("#spUnitTxt").hide(); $("#spExamTxt").show();
    }

    UnitAdd.ShowPoint();    // 점수
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 수정시, 데이타가져오기.
***************************************************/
UnitAdd.LoadItem = function () {
    if ($("#tbHiddenUnits").val() != "") {
        var arr = $("#tbHiddenUnits").val().split(";");
        var guid = "";
        for (var i = 0; i < arr.length; i++) {
            guid = Guids();
            UnitAdd.CreateItem(guid);
            $("#ddlSelItem_" + guid).val(arr[i].split("$")[0]);
            $("#txtItem_" + guid).val(arr[i].split("$")[1]);
            $("#tbUrl_" + guid).val(arr[i].split("$")[2]);

            if ($("#tbUrl_" + guid).val() != "") {
                $("#iImg_" + guid).removeAttr("style");
            }
        }
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 형식-가졈형의 경우, 최대값입력
***************************************************/
UnitAdd.ShowPoint = function () {
    //단일가점+다중가점형은 최대값입력
    if ($("#ddlType").val() == "C" || $("#ddlType").val() == "D") {
        $("#spPoint").show();
    } else {
        $("#spPoint").hide();
    }

    //단일가점형+주관식=항목입력불가
    if ($("#ddlType").val() == "C" || $("#ddlType").val() == "E") {
        $("#divItem").hide(); $(".divTarget").hide();   
    } else {
        $("#divItem").show(); $(".divTarget").show();
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 항목생성 & item처리
***************************************************/
UnitAdd.SetItem = function (_obj, _guid) {
    //item처리
    jQuery.each(_obj.childNodes[0].childNodes, function () {
        if (this.getAttribute("class") == "iImg") {
            this.id = "iImg_" + _guid;
            this.style.display = ($("#tbUrl").val() == "") ? "none" : "";     //파일이 있는경우만 이미지 버튼이 보임
            if (this.childNodes[0].tagName.toUpperCase() == "A") { this.childNodes[0].href = "javascript:UnitAdd.ImgItem('tbUrl_" + _guid + "')"; }
        }
        if (this.getAttribute("class") == "iDel") {
            this.style.display = "";
            if (this.childNodes[0].tagName.toUpperCase() == "A") { this.childNodes[0].href = "javascript:UnitAdd.DelItem('" + _guid + "')"; }
        }
        if (this.getAttribute("class") == "iAdd") { this.style.display = "none"; }
        if (this.getAttribute("class") == "iUpload") { this.style.display = "none"; }
    });
}
UnitAdd.CreateItem = function (_guid) {
    //항목생성
    var obj = $("#divItem").clone();
    obj[0].id += "_" + _guid;
    obj.addClass("divItem");    //grouping(항목리스트)

    jQuery.each(obj[0].childNodes, function () {
        if (this.childNodes[0] != null) {
            if (this.childNodes[0].id != "") { this.childNodes[0].id += "_" + _guid; }
            if (this.getAttribute("class") == "iBtn") { UnitAdd.SetItem(this, _guid); }
            if (this.childNodes[0].tagName.toUpperCase() == "SELECT") {
                this.childNodes[0].selectedIndex = $("#ddlSelItem")[0].selectedIndex; 
            }
        }
    });
    $(".divTarget").append(obj);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [항목추가]
***************************************************/
UnitAdd.AddItem = function () {

    if ($('#txtItem').val() == "") { alert(Msg_ErrSubTit); $('#txtItem')[0].focus(); return; }

	//Sript체크

    var guid = Guids();
    UnitAdd.CreateItem(guid);
    $('#txtItem').val(''); $('#tbUrl').val('');
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [항목삭제]
***************************************************/
UnitAdd.DelItem = function (_guid) {
    //이미지가 있으면 이미지, 삭제
    if ($("#tbUrl_" + _guid).val() != "") {
        ParamArray = {
            'fileName': $("#tbUrl_" + _guid).val(),
            'swfUploadGUID': SwfUploadGUID
        };
        CallEncrypt(CallGetParam())
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("UnitAdd.DelItemScs");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";
        
        AjaxService(url, data, obj);
    }
    $("#divItem_" + _guid).remove();
}
UnitAdd.DelItemScs = function (_guid) {
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [항목이미지추가]
***************************************************/
UnitAdd.UploadAdd = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'fileCount': '1',
        'moduleName': 'OPINION',
        'callBackEvent': "UnitAdd.GetSpoolFileNm"
    };
    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510");
}
UnitAdd.GetSpoolFileNm = function () { //방금 등록한 파일정보가져오기
    ParamArray = { 'swfUploadGUID': SwfUploadGUID };
    CallEncrypt(CallGetParam())
    
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("UnitAdd.GetSpoolFileNmLink");
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/UnitAdd.asmx/GetSpoolFileNm";
    AjaxService(url, data, obj);
}
UnitAdd.GetSpoolFileNmLink = function (data, status) {
    if (data.d != "!NULL" && data.d != "") {
        $('#tbUrl').val(unescape(data.d));    //이미지등록함. (경로)

        $('#divItemFn')[0].style.display = "";
        $('#spfn').text(unescape(data.d));
        $('#delfn').click(function (e) { e.preventDefault(); UnitAdd.DelSpoolPath(data.d); });
    } else {
        $('#divItemFn')[0].style.display = "none";
        $('#spfn').text("");
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 임시파일 삭제 처리
***************************************************/
UnitAdd.DelSpoolPath = function (fileName) {
    if (window.confirm(R_Delete)) {
        ParamArray = { 'fileName': fileName, 'swfUploadGUID': SwfUploadGUID };
        CallEncrypt(CallGetParam())
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("UnitAdd.DelSpoolPathSus");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";
        AjaxService(url, data, obj);
    }
}
UnitAdd.DelSpoolPathSus = function (data, status) {
    if (data.d != "!NULL") {
        $('#divItemFn')[0].style.display = "none";
        $('#spfn').text("");
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [항목이미지보기]
***************************************************/
UnitAdd.ImgItem = function (_obj) {
    var fNm = $("#" + _obj).val();
    ParamArray = {
        'filePath': SwfUploadPath + fNm,        //파일명포함전체경로.
        'fileName': fNm
    };
    CallEncrypt(CallGetParam());
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [확인]
***************************************************/
UnitAdd.Add = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbTitle", Msg_ErrTitle, true);
    //validator.checkSpecialChar("ctbTitle", "제목을 바르게 입력하세요", true);  //특수문자체크 ( [~!@\#$%^&*\()\-=+_'] )

    // 단일가점형(C) / 다중가점형(D)
    if ($("#ddlType").val() == "C" || $("#ddlType").val() == "D") {
        validator.checkIsNumber("tbPoint", Msg_ErrNum, true);
    }
    if (!validator.validate()) return;

    //Sript체크

    // 단일가점형(C) / 다중가점형(D)
    if ($("#ddlType").val() == "C" || $("#ddlType").val() == "D") {
        if (parseInt($("#tbPoint").val()) < 1) {
            alert(Msg_ErrMaxNum); $("#tbPoint").focus(); return;
        }
    }

    // 단일선택(A) / 다중선택(B) / 다중가점(D)
    $("#tbHiddenUnits").val("");
    if ($("#ddlType").val() != "C" && $("#ddlType").val() != "E") {
        if ($(".divItem").length < 1) {
            alert(Msg_ErrItem); $("#ddlSelItem").focus(); return;
        }
        jQuery.each($(".divItem"), function () {
            if (this != null) {
//alert($("#txtItem_" + this.id.split("_")[1]).outerHTML)
//alert($("#tbUrl_" + this.id.split("_")[1]).text())
                //  객관식1$항목내용1$이미지1URL;주관식2$항목내용2$이미지2URL;
                var objVal = $("#ddlSelItem_" + this.id.split("_")[1]).val();
                objVal += "$" + $("#txtItem_" + this.id.split("_")[1]).val();
                objVal += "$" + $("#tbUrl_" + this.id.split("_")[1]).val();
                $("#tbHiddenUnits").val($("#tbHiddenUnits").val() + objVal + ";");
            }
        });
    }

    __doPostBack('lbtnSubmit', "ADD");
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [삭제]
***************************************************/
UnitAdd.Del = function () {

    if (window.confirm(Msg_Del)) {
        __doPostBack('lbtnSubmit', "DEL");
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [목록]
***************************************************/
UnitAdd.Cancel = function () {
    ParamArray = {
        'menuID': MenuID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
    };
    CallEncrypt(CallGetParam());
    location.href = "UnitList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [항목순서]
***************************************************/
UnitAdd.ModSeq = function () {
    ParamArray = { 'unitID': UnitID };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var url = "UnitSeq.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "", "", true);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [목록]
***************************************************/
UnitAdd.Reload = function () {
    ParamArray = {
        'menuID': MenuID
        , 'section': Section
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'unitID': UnitID
    };
    CallEncrypt(CallGetParam());
    location.href = "UnitAdd.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [질문생성]
***************************************************/
UnitAdd.AddUnit = function () {
    ParamArray = {
        'menuID': MenuID
        , 'section': Section
        , 'totCnt': TotCnt
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'unitID': UnitID
        , 'expID': UnitID
    };

    CallEncrypt(CallGetParam());
    location.href = "UnitAdd.aspx?p=" + GetEncryptValue;
}