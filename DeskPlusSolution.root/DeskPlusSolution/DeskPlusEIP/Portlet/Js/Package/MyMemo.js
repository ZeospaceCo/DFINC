$(document).ready(function () {

    var portletDom = mymemo_tabPortletID + "_" + mymemo_portletID;

    if (mymemo_IsAuth != "True") {
        $("#ptConfig_" + portletDom).remove();   
    }

    //최소화 여부
    if (mymemo_IsMini == "1") {
        $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
        $("#ptContent_" + portletDom).fadeOut("slow");
    }

    //포틀릿 최소화/최대화
    $("#ptSlide_" + portletDom).on("click", function (e) {
        if ($("#hdnTabSlide_" + portletDom).val() == "1") {
            //최대화
            Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
            $("#ptContent_" + portletDom).fadeIn("slow");
            $("#hdnTabSlide_" + portletDom).val("0");
        }
        else {
            //최소화
            Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
            $("#ptContent_" + portletDom).fadeOut("slow");
            $("#hdnTabSlide_" + portletDom).val("1");
        }
    });


    //포틀릿 설정
    $("#ptConfig_" + portletDom).on("click", function (e) {
        // 리스트 행 개수 바인딩
        $("#boardListCount_" + portletDom).val($("#hdnTabPageSize_" + portletDom).val());
        $("#ptListConfig_" + portletDom).slideToggle(600);
    });

    // 포틀릿 삭제
    $("#cbtnPDel_" + portletDom).click(function (e) {
        e.preventDefault();

        Main.DelTabPortlet(portletDom);
        if (PortletAuth == "True") {
            $("#portlet_" + portletDom).fadeOut("slow");
        }
    });

    /*
    // 환경 설정 저장
    $('#cbtnMemoConfirm').click(function (e) {
    e.preventDefault();

    MyMemo_EnvSettingAdd();
    });

    // 환경설정 닫기
    $('#cbtnMemoClose').click(function (e) {
    e.preventDefault();

    $("#ptMyMemoConfig").slideToggle(600);
    }); 
    */

    // 환경설정 닫기 (메모저장)
    $('#MyMemotxtInput').focusout(function (e) {
        e.preventDefault();
        MyMemo_Add();
    });

    // 저장 버튼
    $('#cbtnMemoSave').click(function (e) {
        e.preventDefault();
        MyMemo_Add();
    });

    MyMemo_LoadMemo();

    $("#MyMemotxtInput").autogrow();
});


/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 메모 정보 바인딩
***************************************************/
function MyMemo_LoadMemo() {

    var url = "/DeskPlusEIP/Portlet/Ws/Package/MyMemo.asmx/GetMemoInfo";

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyMemo_ListSuccess");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 메모 정보 바인딩성공
***************************************************/
function MyMemo_ListSuccess(data, status) {
    
    if (data.d.MemoID != 0) {
        if (data.d.Title == null) {
            alert(R_ProcessingError);
            return;
        }
        else if (data.d.BackColor == null) {
            alert(R_ProcessingError);
            return;
        }
        else if (data.d.Color == null) {
            alert(R_ProcessingError);
            return;
        }
        else if (data.d.Memo == null) {
            alert(R_ProcessingError);
            return;
        }

        var title = data.d.Title
                            .replace(/[＆]/gi, '&').replace(/[＂]/gi, '"')
                            .replace(/[＇]/gi, '\'').replace(/[（]/gi, '(')
                            .replace(/[）]/gi, ')').replace(/[＋]/gi, '+')
                            .replace(/[※]/gi, '§').replace(/[＝]/gi, '=');

        $("#myMemoTitle").text(title != '' ? title : mymemo_portletName);
        $("#ddlBackColor").val(data.d.BackColor);
        $("#ddlFontColor").val(data.d.Color);

        mymemo_OldMyMemo = data.d.Memo;

        mymemo_OldMyMemo = mymemo_OldMyMemo
                            .replace(/[＆]/gi, '&').replace(/[＂]/gi, '"')
                            .replace(/[＇]/gi, '\'').replace(/[（]/gi, '(')
                            .replace(/[）]/gi, ')').replace(/[＋]/gi, '+')
                            .replace(/[※]/gi, '§').replace(/[＝]/gi, '=');

        $("#MyMemotxtInput").val(mymemo_OldMyMemo);
        

        //$("#MyMemotxtInput").css("background-color", data.d.BackColor);
        //$("#MyMemotxtInput").css("color", data.d.Color);
        
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 메모저장
***************************************************/
function MyMemo_Add() {

    mymemo_MyMemo = $("#MyMemotxtInput").val();

    if (mymemo_OldMyMemo == mymemo_MyMemo) 
        return;

    var url = "/DeskPlusEIP/Portlet/Ws/Package/MyMemo.asmx/MyMemoAdd";

    // 내용에 & 포함시 & 이후의 문자는 잘림 
    // 따라서 & 문자를 다른 특수 문자로 치환함
    //mymemo_MyMemo = mymemo_MyMemo.replace(/&/g, '┢');

    mymemo_MyMemo = mymemo_MyMemo
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋')
        .replace(/[§]/gi, '※').replace(/[=]/gi, '＝');    
    
    ParamArray = {
        'myMemo': mymemo_MyMemo
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyMemo_Success");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 성공
**********************************************************************************************/
function MyMemo_Success(data, status) {
    if (data.d)
    {
        //alert("메모가 저장되었습니다.");
        mymemo_OldMyMemo = $("#MyMemotxtInput").val();
    }
    else
    {
        alert(R_ProcessingError);
    }
    return;
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 환경설정 저장(제목, 배경색, 글자색)
**********************************************************************************************/
function MyMemo_EnvSettingAdd() {
    mymemo_MyMemoTitle = $("#tbTitle").val() == "" ? mymemo_portletName : $("#tbTitle").val();
    mymemo_BackColor = $("#ddlBackColor").val();
    mymemo_FontColor = $("#ddlFontColor").val();

    var url = "/DeskPlusEIP/Portlet/Ws/Package/MyMemo.asmx/EnvSettingAdd";
    // & 가 포함 될 경우 & 이하의 내용이 잘리기 때문에
    // 특수문자 ＆로 치환
    //mymemo_MyMemoTitle = mymemo_MyMemoTitle.replace(/&/g, '┢')

    mymemo_MyMemoTitle = mymemo_MyMemoTitle
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋')
        .replace(/[§]/gi, '※').replace(/[=]/gi, '＝');

    ParamArray = {
        'myMemoTitle': mymemo_MyMemoTitle,
        'fontColor': mymemo_FontColor,
        'backColor': mymemo_BackColor       
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyMemo_EnvSettingSuccess");

    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 환경설정 저장 성공
**********************************************************************************************/
function MyMemo_EnvSettingSuccess(data, status) {

    if (data.d) {
        alert(R_PortletInfoSave);

        mymemo_MyMemoTitle = mymemo_MyMemoTitle
                            .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
                            .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
                            .replace(/[)]/gi, '）').replace(/[+]/gi, '＋')
                            .replace(/[§]/gi, '※').replace(/[=]/gi, '＝');

        $("#myMemoTitle").text(mymemo_MyMemoTitle);
        //$("#MyMemotxtInput").css("background-color", mymemo_BackColor);
        //$("#MyMemotxtInput").css("color", mymemo_FontColor);
    }
    else {
        alert(R_ProcessingError);
    }
    return;
}


