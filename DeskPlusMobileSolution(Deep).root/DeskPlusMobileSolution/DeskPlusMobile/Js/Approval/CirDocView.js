CirDocView = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    setTimeout("CirDocView.readyLoad();", 400);

    $('#minus').click(function (e) {
        e.preventDefault();
        CirDocView.minus();
    });

    $('#plus').click(function (e) {
        e.preventDefault();
        CirDocView.plus();
    });

    $('#before').click(function (e) {
        CirDocView.goBefore();
    });

    $('#listTitle').click(function (e) {
        CirDocView.goBefore();
    });

    $('#btnMemo').click(function (e) {
        CirDocView.goMemo();
    });

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
작성일자 : 2015-10-26
작성목적 : 문서 정보 가져오기
***************************************************/
CirDocView.readyLoad = function () {

    CirDocView.getData();

    CirDocView.getOpinion();

    if (BPage == "CirRecv") {
        CirDocView.ModRecvViewingDate();    //받은 회람함 확인날짜 수정
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 문서 정보 가져오기 (웹서비스 호출)
***************************************************/
CirDocView.getData = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetContentCirDoc";

    //파라미터값 배열에 담기
    ParamArray = {
        'CirDocID': CirDocID
    };

    var obj1 = eval("CirDocView.succeededData");
    var obj2 = eval("CirDocView.errorData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncServiceAll(url, data, obj1, obj2);
}
/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 문서 정보 가져오기 (웹서비스 값 받기 - 에러)
***************************************************/
CirDocView.errorData = function (data, status) {
    CirDocView.goError();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 문서 정보 가져오기 (웹서비스 값 받기)
***************************************************/
CirDocView.succeededData = function (data, status) {
    var daObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (daObj.dsDocInfo.length < 1) {
        CirDocView.goError();
        return;
    }

    $("#subject").empty();
    $("#num3").empty();
    $("#write").empty();

    $("#subject").html(daObj.dsDocInfo[0].Subject);
    $("#num3").text(daObj.dsDocInfo[0].Number3);
    $("#write").text(daObj.dsDocInfo[0].UserName + " " + daObj.dsDocInfo[0].ConvertWriteTime + " ~ " + daObj.dsDocInfo[0].ConvertCompleteDate);

    var cnt = daObj.dsDocInfo[0].bodyNoStyle;

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

    //CirDocView.getAttach();
    CirDocView.getDS();
    CirDocView.getBase();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 문서 첨부파일 가져오기 (웹서비스 호출)
***************************************************/
CirDocView.getAttach = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetAttachFileCir";

    //파라미터값 배열에 담기
    ParamArray = {
        'CirDocID': CirDocID
    };

    var obj = eval("CirDocView.succeededAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 문서 첨부파일 가져오기 (웹서비스 값 받기)
***************************************************/
CirDocView.succeededAttach = function (data, status) {
    var attObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var filepath = "";
    var tmpAttachTag = R_Attach;

    for (var i = 0; i < attObj.dsAtt.length; i++) {
        filepath = attObj.dsAtt[i].FilePath.replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/")

        var tmpFileName = attObj.dsAtt[i].FileName;
        if (tmpFileName.indexOf("'") > -1) {
            //tmpFileName = attObj.dsAtt[i].FileGUID;
            tmpFileName = tmpFileName.split("'").join("");
        }

        tmpAttachTag += "<div style='word-wrap:break-word;'><a class='linkDiv' onclick=\"CirDocView.goAttach('" + filepath + attObj.dsAtt[i].FileGUID + "','" + tmpFileName + "')\">" + attObj.dsAtt[i].FileName + "</a>";
        tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsAtt[i].FileLength) + "byte</span></div>";
    }

    $("#attach").empty();
    $("#attach").append(tmpAttachTag);

}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 문서 첨부파일 읽기 (웹서비스 호출)
***************************************************/
CirDocView.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': encodeURIComponent(fileName)
    };

    var obj = eval("CirDocView.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 문서 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
CirDocView.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        CirDocView.goError();
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

            location.href = "/DeskPlusMobile/Common/AttachCirDocView.aspx?p=" + GetEncryptValue;
        }
    }
}


/**************************************************
작성자   : 이중호
작성일자 : 2018-06-21
작성목적 : 문서 스트리밍 Post
***************************************************/
CirDocView.goDocStream = function (fileID) {

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
작성자   : 이중호
작성일자 : 2018-04-06
작성목적 : 문서 닥스트리밍 첨부파일 가져오기 (웹서비스 호출)
***************************************************/
CirDocView.getDS = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetDSFile";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': CirDocID
        , 'TYPE': "APP_CIR"
    };
    var obj = eval("CirDocView.succeededDS");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-06
작성목적 : 문서 첨부파일 가져오기 (웹서비스 값 받기)
***************************************************/
CirDocView.succeededDS = function (data, status) {
    var attObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var filepath = "";
    var tmpAttachTag = R_Attach;

    for (var i = 0; i < attObj.dsDS.length; i++) {
        if (attObj.dsDS[i].FileID.length > 0) {
            tmpAttachTag += "<div><a class='linkDiv' onclick=\"CirDocView.goDocStream('" + attObj.dsDS[i].FileID + "')\">" + attObj.dsDS[i].FileName + "</a>";
            tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsDS[i].FileLength) + "byte</span></div>";
        } else {
            filepath = attObj.dsDS[i].FilePath.replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/")

            var tmpFileName = attObj.dsDS[i].FileName;
            if (tmpFileName.indexOf("'") > -1) {
                //tmpFileName = attObj.dsAtt[i].FileGUID;
                tmpFileName = tmpFileName.split("'").join("");
            }

            tmpAttachTag += "<div><a class='linkDiv' onclick=\"CirDocView.goAttach('" + filepath + attObj.dsDS[i].FileGUID + "','" + tmpFileName + "')\">" + attObj.dsDS[i].FileName + "</a>";
            tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsDS[i].FileLength) + "byte</span></div>";
        }

    }

    $("#attach").empty();
    $("#attach").append(tmpAttachTag);

    if (attObj.dsDS.length == 0) {
        CirDocView.getAttach();
    }

}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 문서 근거문서 가져오기 (웹서비스 호출)
***************************************************/
CirDocView.getBase = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetBaseDocCir";

    //파라미터값 배열에 담기
    ParamArray = {
        'CirDocID': CirDocID
    };

    var obj = eval("CirDocView.succeededBase");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 문서 근거문서 가져오기 (웹서비스 값 받기)
***************************************************/
CirDocView.succeededBase = function (data, status) {
    var baseObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var tmpBaseTag = R_BaseDoc;
    for (i = 0; i < baseObj.dsBase.length; i++) {

        if (baseObj.dsBase[i].IsLegacy == "N") {
            tmpBaseTag += "<div><a class='linkDiv' id=\"BaseButton" + i + "\" type=\"button\" onclick=\"CirDocView.goBase('" + baseObj.dsBase[i].BaseDocumentID + "')\" >" + baseObj.dsBase[i].Name + "</a></div>";
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
작성일자 : 2015-10-26
작성목적 : 문서 근거문서 보기 화면으로 가기 
***************************************************/
CirDocView.goBase = function (baseDID) {

    ParamArray = {
        'BaseDID': baseDID
    };

    CallEncrypt(CallGetParam());

    location.href = "BaseView.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 이전 글 가기
***************************************************/
CirDocView.minus = function () {
    CirDocView.getBeforeNext('Top');
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 다음 글 가기
***************************************************/
CirDocView.plus = function () {
    CirDocView.getBeforeNext('Down');
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 이전 , 다음 글 가기 (웹서비스 호출)
***************************************************/
CirDocView.getBeforeNext = function (gubun) {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetBeforeNextCirDoc";

    //파라미터값 배열에 담기
    ParamArray = {
        'BPage': BPage,
        'Gubun': gubun,
        'Sv': SearchValue.securityText(),
        'CirDocID': CirDocID,
        'CirUserID': CirUserID
    };

    var obj = eval("CirDocView.succeededBeforeNext");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 이전 , 다음 글 가기 (웹서비스 값 받기)
***************************************************/
CirDocView.succeededBeforeNext = function (data, status) {
    if (data.d != "0") {
        var tmpVal = data.d;
        var arrVal = tmpVal.split("^");
        if (arrVal[0] != "0") {

            ParamArray = {
                'BPage': BPage,
                'PageNo': PageNo,
                'Sv': SearchValue.securityText(),
                'CirDocID': arrVal[0],
                'CirUserID': arrVal[1]
            };

            CallEncrypt(CallGetParam());
            document.location.href = "CirDocView.aspx?p=" + GetEncryptValue;
        }
        else {
            action_popup.alert(R_NoList);
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 목록 페이지 이동
***************************************************/
CirDocView.goBefore = function () {
    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "CirDocList.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 에러 발생시에 처리
***************************************************/
CirDocView.goError = function () {
    action_popup.alert(R_ReLoadDoc);
    location.href = "Folder.aspx";
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 확인일자 수정 (웹서비스 호출)
***************************************************/
CirDocView.ModRecvViewingDate = function () {

    var url = "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/ModRecvViewingDate";

    //파라미터값 배열에 담기
    ParamArray = {
        'CirDocID': CirDocID
    };

    var obj = eval("CirDocView.succeededViewingDate");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 확인일자 수정 (웹서비스 값 받기)
***************************************************/
CirDocView.succeededViewingDate = function (data, status) {

}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-26
작성목적 : 의견보기
***************************************************/
CirDocView.goMemo = function () {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText(),
        'CirDocID': CirDocID
    };

    CallEncrypt(CallGetParam());

    location.href = "Memo.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : kkzz52
작성일자 : 2022-01-18
작성목적 : 결재의견 가져오기 (웹서비스 호출)
***************************************************/
CirDocView.getOpinion = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetOpinionList";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': CDID
    };

    var obj = eval("CirDocView.succeededOpinion");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2022-01-18
작성목적 : 결재의견 가져오기 (웹서비스 값 받기)
***************************************************/
CirDocView.succeededOpinion = function (data, status) {
    var obj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (obj.dsShort.length == 0) {
        $("#btnOpinionView").hide();
    }
    else {
        $("#btnOpinionView").show();
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