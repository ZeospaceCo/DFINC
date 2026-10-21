BaseView = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    setTimeout("BaseView.getChild();", 400);

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    if (window.history && window.history.pushState) {
        window.history.replaceState(null, null, '');
        $(window).on('popstate', function (event) {
            LoadingShow(false);
        });
    }
});

/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 문서 가져오기 (웹서비스 호출)
***************************************************/
BaseView.getChild = function (action) {
    BaseView.getContent();
    //    BaseView.getAttach();
    BaseView.getDS();
    BaseView.getBase();
    BaseView.getOpinion();
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 문서 내용 가져오기 (웹서비스 호출)
***************************************************/
BaseView.getContent = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetContentHTMLDoc";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': BaseDID
    };

    var obj = eval("BaseView.succeededContent");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 문서 정보 가져오기 (웹서비스 값 받기)
***************************************************/
BaseView.succeededContent = function (data, status) {

    var conObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (conObj.dsCont.length < 1) {
        BaseView.goError();
        return;
    }

    //var cnt = conObj.dsCont[0].ContentHTML;
    var cnt = conObj.dsCont[0].bodyNoStyle;

    $("#contentHTML").empty();
    $("#contentHTML").append(cnt);

    $('#contentHTML').contents().find('input[type != text]').attr('readonly', "true");
    //$('#contentHTML').contents().find('input[type != text]').attr('disabled', "true");
    $('#contentHTML').contents().find('input').attr('contentEditable', "false");
    $('#contentHTML').contents().find('div').attr('contentEditable', "false");
    $('#contentHTML').contents().find('select').attr('disabled', "true");
    $('#contentHTML').contents().find('input:radio').attr('disabled', "true");

    var elmnt = document.getElementById("contentHTML");
    var tmpWidth = elmnt.scrollWidth + 20;
    var tmpHight = elmnt.scrollHeight + 20;
    var TmpSize = $(window).width() / tmpWidth;

    if (TmpSize < 1) {

        $("#contentHTML").css({
            '-ms-zoom': +TmpSize,
            '-moz-transform': 'scale(' + TmpSize + ')',
            '-moz-transform-origin': '0px 0',
            '-o-transform': 'scale(' + TmpSize + ')',
            '-o-transform-origin': '0 0',
            '-webkit-transform': 'scale(' + TmpSize + ')',
            '-webkit-transform-origin': '0 0',
            '-ms-transform': 'scale(' + TmpSize + ')',
            '-ms-transform-origin': '0 0'
        });

        $("#contentHTML").height(tmpHight * TmpSize);
    }

}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 문서 첨부파일 가져오기 (웹서비스 호출)
***************************************************/
BaseView.getAttach = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetAttachFile";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': BaseDID
    };

    var obj = eval("BaseView.succeededAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 문서 첨부파일 가져오기 (웹서비스 값 받기)
***************************************************/
BaseView.succeededAttach = function (data, status) {
    var attObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var filepath = "";
    var tmpAttachTag = R_Attach;

    for (var i = 0; i < attObj.dsAtt.length; i++) {
        filepath = attObj.dsAtt[i].FilePath.replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/")

        var tmpFileName = attObj.dsAtt[i].FileName;

        if (tmpFileName.indexOf("'") > -1) {
            tmpFileName = tmpFileName.split("'").join("");
        }

        tmpAttachTag += "<div style='word-wrap:break-word;'><a class='linkDiv' onclick=\"BaseView.goAttach('" + filepath + attObj.dsAtt[i].FileGUID + "','" + tmpFileName + "')\">" + attObj.dsAtt[i].FileName + "</a>";
        tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsAtt[i].FileLength) + "byte</span></div>";
    }

    $("#attach").empty();
    $("#attach").append(tmpAttachTag);

}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 문서 첨부파일 읽기 (웹서비스 호출)
***************************************************/
BaseView.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': encodeURIComponent(fileName)
    };

    var obj = eval("BaseView.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : 이중호
작성일자 : 2018-06-21
작성목적 : 문서 스트리밍 Post
***************************************************/
BaseView.goDocStream = function (fileID) {

    var url = "http://sp.deskplus.co.kr:9979/docviewer.melita";

    var form = document.createElement("form");
    var parm = new Array();
    var input = new Array();

    form.action = url;
    //    form.target = "_blank";
    form.method = "post";

    parm.push(['FILE_ID', fileID]);
    parm.push(['client', 'mobile_web']);

    for (var i = 0; i < parm.length; i++) {
        input[i] = document.createElement("input");
        input[i].setAttribute("type", "hidden");
        input[i].setAttribute('name', parm[i][0]);
        input[i].setAttribute("value", parm[i][1]);
        form.appendChild(input[i]);
    }

    document.body.appendChild(form);
    form.submit();
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 문서 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
BaseView.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        BaseView.goError();
        return;
    }
    else if (data.d == "FAIL") {
        NoticeShow("!", R_NoDownload);
        return;
    }
    else {
        var tmpAttachName = data.d;
        var lastidx = tmpAttachName.lastIndexOf(".");
        var tmpFiletype = tmpAttachName.substring(lastidx + 1, tmpAttachName.length);

        if (tmpFiletype.toLowerCase() != "mht") {
            if (MobileType == "ios") {

                ParamArray = {
                    'companyID': CompanyID,
                    'userID': UserID,
                    'attachName': encodeURIComponent(tmpAttachName)
                };

                CallEncrypt(CallGetParam());

                var url = HttpType + MobileDOMAIN + "/DeskPlusMobile/Common/AttachUrl.aspx?p=" + GetEncryptValue;
                //window.webkit.messageHandlers.open_safari.postMessage(url);
                openSafari(url);
            }
            else {
                location.href = "/DeskPlusMobile/FileData/" + CompanyID + "/users/" + UserID + "/Spool/Mobile/" + encodeURIComponent(tmpAttachName);
            }
        }
        else {
            ParamArray = {
                'AttachName': encodeURIComponent(tmpAttachName)
            };

            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusMobile/Common/AttachView.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-06
작성목적 : 문서 닥스트리밍 첨부파일 가져오기 (웹서비스 호출)
***************************************************/
BaseView.getDS = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetDSFile";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': BaseDID
        , 'TYPE': "APP_ATTACH"
    };
    var obj = eval("BaseView.succeededDS");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-06
작성목적 : 문서 닥스트리밍 첨부파일 가져오기 (웹서비스 값 받기)
***************************************************/
BaseView.succeededDS = function (data, status) {
    var attObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var filepath = "";
    var tmpAttachTag = R_Attach;

    for (var i = 0; i < attObj.dsDS.length; i++) {
        if (attObj.dsDS[i].FileID.length > 0) {
            tmpAttachTag += "<div><a class='linkDiv' onclick=\"BaseView.goDocStream('" + attObj.dsDS[i].FileID + "')\">" + attObj.dsDS[i].FileName + "</a>";
            tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsDS[i].FileLength) + "byte</span></div>";
        } else {
            filepath = attObj.dsDS[i].FilePath.replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/")

            var tmpFileName = attObj.dsDS[i].FileName;
            if (tmpFileName.indexOf("'") > -1) {
                //tmpFileName = attObj.dsAtt[i].FileGUID;
                tmpFileName = tmpFileName.split("'").join("");
            }

            tmpAttachTag += "<div><a class='linkDiv' onclick=\"BaseView.goAttach('" + filepath + attObj.dsDS[i].FileGUID + "','" + tmpFileName + "')\">" + attObj.dsDS[i].FileName + "</a>";
            tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsDS[i].FileLength) + "byte</span></div>";
        }

    }

    $("#attach").empty();
    $("#attach").append(tmpAttachTag);

    if (attObj.dsDS.length == 0) {
        BaseView.getAttach();
    }

}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 문서 근거문서 가져오기 (웹서비스 호출)
***************************************************/
BaseView.getBase = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetBaseDoc";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': BaseDID
    };

    var obj = eval("BaseView.succeededBase");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 문서 근거문서 가져오기 (웹서비스 값 받기)
***************************************************/
BaseView.succeededBase = function (data, status) {
    var baseObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var tmpBaseTag = R_BaseDoc;

    for (i = 0; i < baseObj.dsBase.length; i++) {
        if (baseObj.dsBase[i].IsLegacy == "N") {
            tmpBaseTag += "<div><a class='linkDiv' href='#2' id=\"BaseButton" + i + "\" type=\"button\" onclick=\"BaseView.goBase('" + baseObj.dsBase[i].ParentDocumentID + "')\" >" + baseObj.dsBase[i].Name + "</a></div>";
        }
        else {
            tmpBaseTag += "<div class='dva'><a class='linkDiv' href=\"" + baseObj.dsBase[i].Path + "\">" + baseObj.dsBase[i].Name + "</a></div>";
        }
    }

    $("#base").empty();
    $("#base").append(tmpBaseTag);

}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-01
작성목적 : 문서 근거문서 보기 화면으로 가기 
***************************************************/
BaseView.goBase = function (baseDID) {

    ParamArray = {
        'BaseDID': baseDID
    };

    CallEncrypt(CallGetParam());

    location.href = "BaseView.aspx?p=" + GetEncryptValue;
}



/**************************************************
작성자   : famo23
작성일자 : 2011-07-19
작성목적 : 에러 발생시에 처리
***************************************************/
BaseView.goError = function () {
    action_popup.alert(R_ReLoadDoc);
    location.href = "Folder.aspx";
}

/**************************************************
작성자   : kkzz52
작성일자 : 2022-01-18
작성목적 : 결재의견 가져오기 (웹서비스 호출)
***************************************************/
BaseView.getOpinion = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetOpinionList";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': CDID
    };

    var obj = eval("BaseView.succeededOpinion");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2022-01-18
작성목적 : 결재의견 가져오기 (웹서비스 값 받기)
***************************************************/
BaseView.succeededOpinion = function (data, status) {
    var obj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (obj.dsShort.length == 0) {
        $("#menu").hide();
    }
    else {
        $("#menu").show();
    }

    var tmpOpinionTag = "";
    for (i = 0; i < obj.dsShort.length; i++) {
        if (i == 0) {
            tmpOpinionTag += "<div class='modal_div'>";
        }
        else {
            tmpOpinionTag += "<div class='modal_div' style='border-top: 1px solid #dcdcdc'>";
        }
        tmpOpinionTag += "<div class='title'>";
        tmpOpinionTag += "<table width='100%' cellpadding='0' cellspacing='0' style='font-family: Nanum Gothic; font-size: 1.0em;'>";
        tmpOpinionTag += "<tr>";
        tmpOpinionTag += "<td>";
        tmpOpinionTag += "<div class='divBest'>";
        tmpOpinionTag += "<span class='repleName'>" + obj.dsShort[i].UserName + "(" + obj.dsShort[i].TitleName + "/" + obj.dsShort[i].DeptName + ")";
        if (obj.dsShort[i].Section == "2") {
            tmpOpinionTag += " <b>(*)</b>";
        }
        tmpOpinionTag += "</span>";
        tmpOpinionTag += "</div>";
        tmpOpinionTag += "<div style='width: 100%'>";
        tmpOpinionTag += "<table cellpadding='0' cellspacing='0' style='font-family: Nanum Gothic; font-size: 1.2em;'>";
        tmpOpinionTag += "<tr>";
        tmpOpinionTag += "<td>";
        tmpOpinionTag += obj.dsShort[i].Opinion;
        tmpOpinionTag += "</td>";
        tmpOpinionTag += "</tr>";
        tmpOpinionTag += "</table>";
        tmpOpinionTag += "</div>";
        tmpOpinionTag += "<div class='reple_Box'>";
        tmpOpinionTag += "<span class='reple_Date'>";
        tmpOpinionTag += obj.dsShort[i].ConvertTime;
        tmpOpinionTag += "</span>";
        tmpOpinionTag += "</div>";
        tmpOpinionTag += "</td>";
        tmpOpinionTag += "</tr>";
        tmpOpinionTag += "</table>";
        tmpOpinionTag += "</div>";
        tmpOpinionTag += "</div>";
    }

    if (obj.dsShort.length > 0) {
        // 모달 팝업 띄울 시 발생하는 이벤트 (이벤트명 : show.bs.modal)
        $('#modalopinion').on('show.bs.modal', function (e) {
            // 모달 팝업에 데이터 집어넣기
            var modal = $(this);
            modal.find('.modal-body').html(tmpOpinionTag);
        })

        // 모달 팝업의 열림이 끝나고 발생하는 이벤트
        $('#modalopinion').on('shown.bs.modal', function (e) {
            var modal = $(this);
            if (modal.find('.modal-body').height() > 400) {
                modal.find('.modal-body').css("height", "400px");
                modal.find('.modal-body').css("overflow", "auto");
            }
        })

        $('#modalopinion').modal('show');
    }

}