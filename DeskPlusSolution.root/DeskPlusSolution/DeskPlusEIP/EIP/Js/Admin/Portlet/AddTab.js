AddTab = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //포틀릿 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            AddTab.Add();
        });

        //목록으로
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            AddTab.Cancel();
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            AddTab.AttachSwfFile();
        });

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiLang").val(), "GetMultiName", MultiLang);
        });


        $('#ctbMultiLang').attr("disabled", true);
        $('#ctbPortletName').focus();
    });
});

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : PortletCode 로 카운트 가져오기 (중복검열)
**********************************************************************************************/
AddTab.GetCountCode = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbPortletCode", R_InputPortletCode, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/AddTab.asmx/GetCountByPortletCode";
    
    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID,
        'portletCode': $("#ctbPortletCode").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddTab.SuccessGetCountPortletCode");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시, PortletCode 로 카운트 가져오기
**********************************************************************************************/
AddTab.SuccessGetCountPortletCode = function (data, status) {
    CountCode = data.d;
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 포틀릿 추가
**********************************************************************************************/
AddTab.Add = function () {
    $('#ctbMultiLang').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbPortletName", R_InputPortletName, true);
    //validator.checkRequired("ctbPortletCode", R_InputPortletCode, true);
    //validator.checkRequired("ctbUIURL", "link 경로 값을 입력해 주세요.", true);
    if (!validator.validate()) return;

//    AddTab.GetCountCode();

//    if (CountCode == 1) {
//        alert(R_DuplicatePortletCode);
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
AddTab.Cancel = function () {
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


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 이미지 첨부파일 추가
***************************************************/
AddTab.AttachSwfFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'fileCount': '1',
        'moduleName': 'EIP',
        'callBackEvent': "AddTab.GetAttachFileList"
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
AddTab.GetAttachFileList = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddTab.SuccessAttachFile");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
AddTab.SuccessAttachFile = function (data, status) {

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