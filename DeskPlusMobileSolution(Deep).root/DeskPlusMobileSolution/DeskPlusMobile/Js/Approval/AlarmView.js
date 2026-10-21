View = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    setTimeout("View.readyLoad();", 400);

    //전결
    $('#btnApproveA').click(function (e) {
        View.goPage('Approve', 'A');
    });

    //승인
    $('#btnApproveS').click(function (e) {
        View.goPage('Approve', 'S');
    });

    //합의
    $('#btnApproveH').click(function (e) {
        View.goPage('Approve', 'H');
    });

    //합의불가
    $('#btnApproveHR').click(function (e) {
        View.goPage('Approve', 'HR');
    });

    //반려
    $('#btnReturn').click(function (e) {
        View.goPage('Return', 'R');
    });

    //결재자 반려문서 결재취소
    $('#btnApproveSC').click(function (e) {
        View.approve();
    });

    //의견추가
    $('#btnOpinion').click(function (e) {
        View.AddOpinion();
    });

    $('#before').click(function (e) {
        View.goBefore();
    });

    $('#listTitle').click(function (e) {
        View.goBefore();
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
작성일자 : 2019-09-23
작성목적 : 문서 정보 가져오기
***************************************************/
View.readyLoad = function () {

    View.getDataSeq();
    View.getData();
    View.getOpinion();

    View.ModCarboncopyViewingDate();    //참조문서함 확인날짜 수정
}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 결재선 가져오기 (웹서비스 호출)
***************************************************/
View.getDataSeq = function () {

    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetSeqTotal";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID
    };

    var obj = eval("View.succeededSeq");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 결재선 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededSeq = function (data, status) {
    var seqObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var tmpSeq = R_ApprovalInfo;
    var tmpZR = "";

    for (var i = 0; i < seqObj.dsSeq.length; i++) {

        if (seqObj.dsSeq[i].Proxy == "Y") {

            //대결자처리
            tmpSeq += "<div><span class='bg-warning text-black'>" + View.getAppTag(seqObj.dsSeq[i].ApprovalTag) + "</span>";
            tmpSeq += "<span>" + seqObj.dsSeq[i].UserName + "(" + seqObj.dsSeq[i].TitleName + "/" + seqObj.dsSeq[i].DeptName + ")</span></div>";
            tmpSeq += "<div><span class='bg-warning text-black'>" + R_Proxy1 + "</span>";
            tmpSeq += "<span>" + seqObj.dsSeq[i].PUserName + "(" + seqObj.dsSeq[i].PTitleName + "/" + seqObj.dsSeq[i].PDeptName + ")</span></div>";
            tmpSeq += "<div><span style='width:50px;'>&nbsp;</span><span class='btn-sm btn-white'>" + View.getAppType(seqObj.dsSeq[i].HGunbun) + "</span>";
            tmpSeq += "<span>" + seqObj.dsSeq[i].ConvertTime + "</span></div>";

        } else {

            if (seqObj.dsSeq[i].ApprovalTag.indexOf("ZR") < 0) {
                //일반승인처리
                if (View.getAppType(seqObj.dsSeq[i].ApprovalType) == R_NoApprove) {
                    tmpSeq += "<div><span class='bg-secondary text-white'>" + View.getAppTag(seqObj.dsSeq[i].ApprovalTag) + "</span>";
                    tmpSeq += "<span>" + seqObj.dsSeq[i].UserName + "(" + seqObj.dsSeq[i].TitleName + "/" + seqObj.dsSeq[i].DeptName + ")</span>";
                    tmpSeq += "<span class='bg-danger text-white'>" + R_NoApprove + "</span></div>"
                } else {
                    tmpSeq += "<div><span class='bg-warning text-black'>" + View.getAppTag(seqObj.dsSeq[i].ApprovalTag) + "</span>";
                    tmpSeq += "<span>" + seqObj.dsSeq[i].UserName + "(" + seqObj.dsSeq[i].TitleName + "/" + seqObj.dsSeq[i].DeptName + ")</span></div>";
                    tmpSeq += "<div><span style='width:50px;'>&nbsp;</span><span class='btn-sm btn-white'>" + View.getAppType(seqObj.dsSeq[i].HGunbun) + "</span>"
                    tmpSeq += "<span>" + seqObj.dsSeq[i].ConvertTime + "</span></div>";
                }

            } else {
                // 공람처리
                if (View.getAppType(seqObj.dsSeq[i].ApprovalType) == R_NoApprove) {
                    tmpZR += "<div><span class='bg-secondary text-white'>" + View.getAppTag(seqObj.dsSeq[i].ApprovalTag) + "</span>";
                    tmpZR += "<span>" + seqObj.dsSeq[i].UserName + "(" + seqObj.dsSeq[i].TitleName + "/" + seqObj.dsSeq[i].DeptName + ")</span>";
                    tmpZR += "<span class='bg-danger text-white'>" + R_NoApprove + "</span></div>"
                } else {
                    tmpZR += "<div><span class='bg-warning text-black'>" + View.getAppTag(seqObj.dsSeq[i].ApprovalTag) + "</span>";
                    tmpZR += "<span>" + seqObj.dsSeq[i].UserName + "(" + seqObj.dsSeq[i].TitleName + "/" + seqObj.dsSeq[i].DeptName + ")</span></div>";
                    tmpZR += "<div><span style='width:50px;'>&nbsp;</span><span class='btn-sm btn-white'>" + View.getAppType(seqObj.dsSeq[i].HGunbun) + "</span>"
                    tmpZR += "<span>" + seqObj.dsSeq[i].ConvertTime + "</span></div>";
                }
            }

        }
    }

    tmpSeq += tmpZR;

    $("#seq").empty();
    $("#seq").append(tmpSeq);

}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 결재태그 이름
***************************************************/
View.getAppTag = function (val) {
    if (val.toUpperCase() == "ZD0") {
        return R_ZD0;  //기안
    } else {
        switch (val.substring(0, 2)) {
            case "ZD": return R_ZD;
            case "ZH": return R_ZH;
            case "ZA": return R_ZA;
            case "ZR": return R_ZR;
            default: return R_ZD;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 결재상태 이름
***************************************************/
View.getAppType = function (val) {
    switch (val.substring(0, 2)) {
        case "A": return R_Arbitrary;
        case "S": return R_Approve;
        case "R": return R_Return;
        case "L": return R_ReadLaterOn;
        case "H": return R_Confirm;
        case "HR": return R_NoConfirm;
        case "C": return R_ConfirmZR;
        default: return R_NoApprove;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 정보 가져오기 (웹서비스 호출)
***************************************************/
View.getData = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetContentTotal";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID
    };

    var obj1 = eval("View.succeededData");
    var obj2 = eval("View.errorData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncServiceAll(url, data, obj1, obj2);
}
/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 정보 가져오기 (웹서비스 값 받기 - 에러)
***************************************************/
View.errorData = function (data, status) {
    View.goError();
}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 정보 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededData = function (data, status) {

    if (data.d == "") {
        View.goError();
        return;
    }

    var daObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    //서브메뉴 제어
    var tmpIsMobile = daObj.dsDocInfo[0].IsMobile;

    AccTag = daObj.dsDocInfo[0].AccountTag;
    Num2 = daObj.dsDocInfo[0].Number2;
    CDID = daObj.dsDocInfo[0].LastDocID;
    GroupID = daObj.dsDocInfo[0].GroupID;
    IsLegacy = daObj.dsDocInfo[0].IsLegacy;
    CustomFileName = daObj.dsDocInfo[0].CustomFileName;

    $("#subject").empty();
    $("#num3").empty();
    $("#write").empty();

    $("#subject").html(daObj.dsDocInfo[0].Subject);
    $("#num3").text(daObj.dsDocInfo[0].Number3)

    if (tmpIsMobile == "0") {
        $("#sIsmobile").addClass("fas fa-desktop");
    } else {
        $("#sIsmobile").addClass("fas fa-mobile-alt");
    }

    $("#write").text(daObj.dsDocInfo[0].UserName + " " + daObj.dsDocInfo[0].ConvertWriteTime);

    var cnt = daObj.dsDocInfo[0].bodyNoStyle;

    $("#contentHTML").empty();
    $("#contentHTML").append(cnt);

    $('#contentHTML').contents().find('input[type != text]').attr('readonly', "true");
    //$('#contentHTML').contents().find('input[type != text]').attr('disabled', "true");
    $('#contentHTML').contents().find('input').attr('contentEditable', "false");
    $('#contentHTML').contents().find('div').attr('contentEditable', "false");
    $('#contentHTML').contents().find('select').attr('disabled', "true");
    $('#contentHTML').contents().find('input:radio').attr('disabled', "true");
    $('#contentHTML').contents().find('input:checkbox').attr('disabled', "true");

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

    //    View.getAttach();
    View.getDS();
    View.getBase();
}


/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 첨부파일 가져오기 (웹서비스 호출)
***************************************************/
View.getAttach = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetAttachFile";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': CDID
    };

    var obj = eval("View.succeededAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 첨부파일 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededAttach = function (data, status) {
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

        tmpAttachTag += "<div style='word-wrap:break-word;'><a class='linkDiv' onclick=\"View.goAttach('" + filepath + attObj.dsAtt[i].FileGUID + "','" + tmpFileName + "')\">" + attObj.dsAtt[i].FileName + "</a>";
        tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsAtt[i].FileLength) + "byte</span></div>";
    }

    $("#attach").empty();
    $("#attach").append(tmpAttachTag);

}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 첨부파일 읽기 (웹서비스 호출)
***************************************************/
View.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': encodeURIComponent(fileName)
    };

    var obj = eval("View.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
    //fileName.securityText()
}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
View.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        View.goError();
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
작성일자 : 2018-06-21
작성목적 : 문서 스트리밍 Post
***************************************************/
View.goDocStream = function (fileID) {

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
View.getDS = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetDSFile";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': CDID
        , 'TYPE': "APP_ATTACH"
    };
    var obj = eval("View.succeededDS");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-06
작성목적 : 문서 닥스트리밍 첨부파일 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededDS = function (data, status) {
    var attObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var filepath = "";
    var tmpAttachTag = R_Attach;

    for (var i = 0; i < attObj.dsDS.length; i++) {
        if (attObj.dsDS[i].FileID.length > 0) {
            tmpAttachTag += "<div><a class='linkDiv' onclick=\"View.goDocStream('" + attObj.dsDS[i].FileID + "')\">" + attObj.dsDS[i].FileName + "</a>";
            tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsDS[i].FileLength) + "byte</span></div>";
        } else {
            filepath = attObj.dsDS[i].FilePath.replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/")

            var tmpFileName = attObj.dsDS[i].FileName;
            if (tmpFileName.indexOf("'") > -1) {
                //tmpFileName = attObj.dsAtt[i].FileGUID;
                tmpFileName = tmpFileName.split("'").join("");
            }

            tmpAttachTag += "<div><a class='linkDiv' onclick=\"View.goAttach('" + filepath + attObj.dsDS[i].FileGUID + "','" + tmpFileName + "')\">" + attObj.dsDS[i].FileName + "</a>";
            tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsDS[i].FileLength) + "byte</span></div>";
        }

    }

    $("#attach").empty();
    $("#attach").append(tmpAttachTag);

    if (attObj.dsDS.length == 0) {
        View.getAttach();
    }

}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 근거문서 가져오기 (웹서비스 호출)
***************************************************/
View.getBase = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetBaseDoc";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': CDID
    };

    var obj = eval("View.succeededBase");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 문서 근거문서 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededBase = function (data, status) {
    var baseObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var tmpBaseTag = R_BaseDoc;
    for (i = 0; i < baseObj.dsBase.length; i++) {
        if (baseObj.dsBase[i].IsLegacy == "N") {
            tmpBaseTag += "<div><a class='linkDiv' href='#2' id=\"BaseButton" + i + "\" type=\"button\" onclick=\"View.goBase('" + baseObj.dsBase[i].ParentDocumentID + "')\" >" + baseObj.dsBase[i].Name + "</a></div>";
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
작성일자 : 2019-09-23
작성목적 : 문서 근거문서 보기 화면으로 가기 
***************************************************/
View.goBase = function (baseDID) {

    ParamArray = {
        'BaseDID': baseDID
    };

    CallEncrypt(CallGetParam());

    location.href = "BaseView.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 목록 페이지 이동
***************************************************/
View.goBefore = function () {
    ParamArray = {
        'PageNo': PageNo
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-09-24
작성목적 : 각 버튼 액션에 따른 페이지 이동
***************************************************/
View.goPage = function (url, appGun) {

    if (url == "") return;

    var rtnValMyTurn = View.getMyturn();

    if ((rtnValMyTurn == "ERROR") || (rtnValMyTurn == "N")) {
        View.goError();
        return;
    }

    if (SealPath != "") {

        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'DID': DID,
            'Stat': Stat,
            'ATag': ATag,
            'AccTag': AccTag,
            'Num2': Num2,
            'Seq': Seq,
            'CDID': CDID,
            'GroupID': GroupID,
            'AppGun': appGun,
            'TargetUID': TargetUID,
            'TargetDID': TargetDID,
            'Alarm': 'Y'
        };

        CallEncrypt(CallGetParam());

        location.href = url + ".aspx?p=" + GetEncryptValue;
    }
    else {
        NoticeShow("!", R_SignNo);
        return;
    }
}



/**************************************************
작성자   : famo23
작성일자 : 2019-09-23
작성목적 : 에러 발생시에 처리
***************************************************/
View.goError = function () {
    action_popup.alert(R_ChangeDoc);

    ParamArray = {
        'PageNo': PageNo
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2016-12-05
작성목적 : 결재 처리 
***************************************************/
View.approve = function (_approvalType) {

    action_popup.confirm(R_ConfirmCancle, function (res) {
        if (res) {
            if (BtnClickCheck != "") {
                NoticeShow("!", R_ProcessingWait); return;
            }

            LoadingShow(true);

            var rtnValMyTurn = View.getMyturn();

            if ((rtnValMyTurn == "ERROR") || (rtnValMyTurn == "N")) {
                View.goError();
                return;
            }

            BtnClickCheck = "Y";
            View.approveSC();        //결재취소
        }
    })
}


/**************************************************
작성자   : famo23
작성일자 : 2016-12-05
작성목적 : 결재 취소 
***************************************************/
View.approveSC = function () {

    DecisionTag = "SC";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID,
        'Seq': Seq,
        'AccTag': AccTag,
        'ATag': ATag,
        'GroupID': GroupID
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/SetCancel",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            LoadingShow(false);
            if (msg.d == "!NULL") {
                if (View.setLog(1)) {
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else if (msg.d == "OK") {
                if (View.setLog(2)) {
                    NoticeShow("*", R_Complete);

                    ParamArray = {
                        'PageNo': PageNo
                    };

                    CallEncrypt(CallGetParam());

                    location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
                }
                else {
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else {
                if (View.setLog(3)) {
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            }
        },
        error: function (msg) {
            if (View.setLog(4)) {
                BtnClickCheck = "";
                NoticeShow("!", R_ProcessingError);
                LoadingShow(false);
                return;
            }
        }
    });
}


/**************************************************
작성자   : famo23
작성일자 : 2016-12-05
작성목적 : 결재 순서 확인
***************************************************/
View.getMyturn = function () {
    var response = "";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID,
        'ATag': ATag

    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetMyturn",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            response = msg.d;
        },
        error: function (msg) {
            response = "ERROR";
        }
    });

    return response;
}

/**************************************************
작성자   : famo23
작성일자 : 2016-12-05
작성목적 : 로그처리
***************************************************/
View.setLog = function (LogGubun) {
    var response = false;

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID,
        'Seq': Seq,
        'ATag': ATag,
        'Gubun': LogGubun,
        'DecisionTag': DecisionTag
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/SetLog",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            response = true;
        },
        error: function (msg) {
            response = false;
        }
    });

    return response;
}


/**************************************************
작성자   : famo23
작성일자 : 2017-01-13
작성목적 : 의견 추가화면 이동
***************************************************/
View.AddOpinion = function () {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'DID': DID,
        'Stat': Stat,
        'ATag': ATag,
        'AccTag': AccTag,
        'Num2': Num2,
        'Seq': Seq,
        'GroupID': GroupID,
        'TargetUID': TargetUID,
        'TargetDID': TargetDID,
        'Alarm': 'Y'
    };

    CallEncrypt(CallGetParam());

    location.href = "AddOpinion.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성목적 : 참조문서 확인일자 수정 (웹서비스 호출)
***************************************************/
View.ModCarboncopyViewingDate = function () {

    var url = "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/ModCarboncopyViewingDate";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID
    };

    var obj = eval("View.succeededCarboncopyDate");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성목적 : 참조문서 확인일자 수정 (웹서비스 값 받기)
***************************************************/
View.succeededCarboncopyDate = function (data, status) {

}

/**************************************************
작성자   : kkzz52
작성일자 : 2022-01-18
작성목적 : 결재의견 가져오기 (웹서비스 호출)
***************************************************/
View.getOpinion = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetOpinionList";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': CDID
    };

    var obj = eval("View.succeededOpinion");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2022-01-18
작성목적 : 결재의견 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededOpinion = function (data, status) {
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