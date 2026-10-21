Add = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //포틀릿 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Add.Add();
        });

        //목록으로
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        //RSS2.0 Data 확인
        $('#cbtnDataCheck').click(function (e) {
            e.preventDefault();
            Add.CheckData();
        });

        //링크 URL 확인
        $('#cbtnLinkURLCheck').click(function (e) {
            e.preventDefault();
            Add.LinkURLCheck();
        });

        //ui형식
        $('#ddlUIType').change(function () {
            //탬플릿일 경우
            if ($('#ddlUIType').val() == "T") {
                $("#ddlUITemplate").show();
                $("#ctbUIURL").hide();
            }
            else {
                $("#ddlUITemplate").hide();
                $("#ctbUIURL").show();
            }
        });

        //link설정
        $('#ddlLinkType').change(function () {
            //하단프레임경우
            if ($('#ddlLinkType').val() == "C") {
                $("#ddlPopupWidth").hide();
                $("#ctbPopupWidth").hide();
                $("#splitChar").hide();
                $("#ddlPopupHeight").hide();
                $("#ctbPopupHeight").hide();
            }
            else {
                $("#ddlPopupWidth").show();
                $("#ctbPopupWidth").show();
                $("#splitChar").show();
                $("#ddlPopupHeight").show();
                $("#ctbPopupHeight").show();
            }
        });

        //가로
        $('#ddlPopupWidth').change(function () {
            if ($('#ddlPopupWidth').val() == "0") {
                $("#ctbPopupWidth").hide();
            }
            else {
                $("#ctbPopupWidth").show();
                $("#ctbPopupWidth").focus();
            }
        });

        //세로
        $('#ddlPopupHeight').change(function () {
            if ($('#ddlPopupHeight').val() == "0") {
                $("#ctbPopupHeight").hide();
            }
            else {
                $("#ctbPopupHeight").show();
                $("#ctbPopupHeight").focus();
            }
        });


        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Add.AttachSwfFile();
        });

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiLang").val(), "GetMultiName", MultiLang);
        });

        $('#ctbMultiLang').attr("disabled", true);

        Add.LoadData();

    });
});


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 초기화
**********************************************************************************************/
Add.LoadData = function () {
    $('#ctbPortletName').focus();

    //탬플릿일 경우
    if ($('#ddlUIType').val() == "T") {
        $("#ddlUITemplate").show();
        $("#ctbUIURL").hide();
    }
    else {
        $("#ddlUITemplate").hide();
        $("#ctbUIURL").show();
    }

    //하단프레임경우
    if ($('#ddlLinkType').val() == "C") {
        $("#ddlPopupWidth").hide();
        $("#ctbPopupWidth").hide();
        $("#splitChar").hide();
        $("#ddlPopupHeight").hide();
        $("#ctbPopupHeight").hide();
    }
    else {
        $("#ddlPopupWidth").show();
        $("#ctbPopupWidth").show();
        $("#splitChar").show();
        $("#ddlPopupHeight").show();
        $("#ctbPopupHeight").show();

        if ($('#ddlPopupWidth').val() == "0") {
            $("#ctbPopupWidth").hide();
        }

        if ($('#ddlPopupHeight').val() == "0") {
            $("#ctbPopupHeight").hide();
        }
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 포틀릿 추가
**********************************************************************************************/
Add.Add = function () {
    $('#ctbMultiLang').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbPortletName", R_InputPortletName, true);
    //validator.checkRequired("ctbPortletCode", "포틀릿코드를 입력해 주세요.", true);
    //validator.checkRequired("ctnDataURL", "Data 형식 값을 입력해 주세요.", true);
    //validator.checkRequired("ctbLinkURL", "Link URL 값을 입력해 주세요.", true);
    if ($('#ddlDataType').val() != "E") {
        validator.checkRequired("ctnDataURL", R_Format, true);
    }

    if (!validator.validate()) return;

    //탬플릿이 아닐 경우
    if ($('#ddlUIType').val() != "T") {
        if ($("#ctbUIURL").val().trimText() == "") {
            alert(R_InputUIFormatURLValue);
            $("#ctbUIURL").focus();
            return;
        }
    }



    //link 설정    
    /*
    if ($('#ddlLinkType').val() != "C") {

    if ($("#ddlPopupWidth").val() == "1") {
    if ($("#ctbPopupWidth").val().trimText() == "") {
    alert("가로 창 크기값을 입력해 주세요.");
    $("#ctbPopupWidth").focus();
    return;
    }
    }

    if ($("#ddlPopupHeight").val() == "1") {
    if ($("#ctbPopupHeight").val().trimText() == "") {
    alert("세로 창 크기값을 입력해 주세요.");
    $("#ctbPopupHeight").focus();
    return;
    }
    }
    }
    */

//    Add.GetCountCode();

//    if (CountCode == 1) {
//        alert("포트릿코드가 중복되었습니다.");
//        $("#ctbPortletCode").focus();
//        return;
//    }

    __doPostBack('lbtnRegister', '');
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 목록으로
**********************************************************************************************/
Add.Cancel = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-12.26
작성목적 : RSS2.0 Data 확인
**********************************************************************************************/
Add.CheckData = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctnDataURL", R_InputDataFormat, true);
    if (!validator.validate()) return;

    var url = $("#ctnDataURL").val();
    window.open(url, "", "");
    //var WindowObjectReference = PopupWinOpen(url, "", "", "1024", "768")
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : link url 확인
**********************************************************************************************/
Add.LinkURLCheck = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbLinkURL", R_InputLinkURLValue, true);
    if (!validator.validate()) return;

    var url = $("#ctbLinkURL").val();
    var WindowObjectReference = PopupWinOpen(url, "", "", "1024", "768")
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 이미지 첨부파일 추가
***************************************************/
Add.AttachSwfFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'fileCount': '1',
        'moduleName': 'EIP',
        'callBackEvent': "Add.GetAttachFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 사진 이미지 넣기
**********************************************************************************************/
Add.GetAttachFileList = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessAttachFile");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Add.SuccessAttachFile = function (data, status) {

    for (var count in data.d) {
        var photoPath = data.d[count].FilePath;
        $("#photoImg")[0].innerHTML = "<img src='" + photoPath + "' width='120' height='60' />";
        $("#tbPhotoNEW").val(data.d[count].FileName);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbMultiLang").val(multiNames);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : PortletCode 로 카운트 가져오기 (중복검열)
**********************************************************************************************/
Add.GetCountCode = function () {
    
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbPortletCode", R_InputPortletCode, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/Add.asmx/GetCountByPortletCode";

    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID,
        'portletCode': $("#ctbPortletCode").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessGetCountPortletCode");

    AjaxService(url, data, obj)
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시, PortletCode 로 카운트 가져오기
**********************************************************************************************/
Add.SuccessGetCountPortletCode = function (data, status) {
    CountCode = data.d;
}
