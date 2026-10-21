DocView = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    setTimeout("DocView.readyLoad();", 400);

    $('#minus').click(function (e) {
        e.preventDefault();
        DocView.minus();
    });

    $('#plus').click(function (e) {
        e.preventDefault();
        DocView.plus();
    });

    $('#before').click(function (e) {
        DocView.goBefore();
    });

    $('#listTitle').click(function (e) {
        DocView.goBefore();
    });

    $('#btnMemo').click(function (e) {
        DocView.goMemo();
    });

    $('#btnOpinion').click(function (e) {
        DocView.AddOpinion();
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
작성일자 : 2015-10-21
작성목적 : 문서 정보 가져오기
***************************************************/
DocView.readyLoad = function () {

    DocView.getDataSeq();
    DocView.getData();
    DocView.getOpinion();

    if (BPage == "Carboncopy") {
        DocView.ModCarboncopyViewingDate();    //참조문서함 확인날짜 수정
    }
    else if (BPage == "Forward") {
        DocView.ModForwardViewingDate();    //전달문서함 확인날짜 수정
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 결재선 가져오기 (웹서비스 호출)
***************************************************/
DocView.getDataSeq = function () {

    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetSeqBackUp";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID
    };

    var obj = eval("DocView.succeededSeq");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 결재선 가져오기 (웹서비스 값 받기)
***************************************************/
DocView.succeededSeq = function (data, status) {
    var seqObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (seqObj.dsSeq.length == 0) {
        DocView.goError();
        return;
    }

    var tmpSeq = R_ApprovalInfo;
    var tmpZR = "";

    for (var i = 0; i < seqObj.dsSeq.length; i++) {

        if (seqObj.dsSeq[i].Proxy == "Y") {

            //대결자처리
            tmpSeq += "<div><span class='bg-warning text-black'>" + DocView.getAppTag(seqObj.dsSeq[i].ApprovalTag) + "</span>";
            tmpSeq += "<span>" + seqObj.dsSeq[i].UserName + "(" + seqObj.dsSeq[i].TitleName + "/" + seqObj.dsSeq[i].DeptName + ")</span></div>";
            tmpSeq += "<div><span class='bg-warning text-black'>" + R_Proxy1 + "</span>";
            tmpSeq += "<span>" + seqObj.dsSeq[i].PUserName + "(" + seqObj.dsSeq[i].PTitleName + "/" + seqObj.dsSeq[i].PDeptName + ")</span></div>";
            tmpSeq += "<div><span style='width:50px;'>&nbsp;</span><span class='btn-sm btn-white'>" + DocView.getAppType(seqObj.dsSeq[i].HGunbun) + "</span>";
            tmpSeq += "<span>" + seqObj.dsSeq[i].ConvertTime + "</span></div>";

        } else {

            if (seqObj.dsSeq[i].ApprovalTag.indexOf("ZR") < 0) {
                // 일반승인처리
                if (DocView.getAppType(seqObj.dsSeq[i].ApprovalType) == R_NoApprove) {
                    tmpSeq += "<div><span class='bg-secondary text-white'>" + DocView.getAppTag(seqObj.dsSeq[i].ApprovalTag) + "</span>";
                    tmpSeq += "<span>" + seqObj.dsSeq[i].UserName + "(" + seqObj.dsSeq[i].TitleName + "/" + seqObj.dsSeq[i].DeptName + ")</span>";
                    tmpSeq += "<span class='bg-danger text-white'>" + R_NoApprove + "</span></div>"
                } else {
                    tmpSeq += "<div><span class='bg-warning text-black'>" + DocView.getAppTag(seqObj.dsSeq[i].ApprovalTag) + "</span>";
                    tmpSeq += "<span>" + seqObj.dsSeq[i].UserName + "(" + seqObj.dsSeq[i].TitleName + "/" + seqObj.dsSeq[i].DeptName + ")</span></div>";
                    tmpSeq += "<div><span style='width:50px;'>&nbsp;</span><span class='btn-sm btn-white'>" + DocView.getAppType(seqObj.dsSeq[i].HGunbun) + "</span>"
                    tmpSeq += "<span>" + seqObj.dsSeq[i].ConvertTime + "</span></div>";
                }

            } else {
                // 공람처리
                if (DocView.getAppType(seqObj.dsSeq[i].ApprovalType) == R_NoApprove) {
                    tmpZR += "<div><span class='bg-secondary text-white'>" + DocView.getAppTag(seqObj.dsSeq[i].ApprovalTag) + "</span>";
                    tmpZR += "<span>" + seqObj.dsSeq[i].UserName + "(" + seqObj.dsSeq[i].TitleName + "/" + seqObj.dsSeq[i].DeptName + ")</span>";
                    tmpZR += "<span class='bg-danger text-white'>" + R_NoApprove + "</span></div>"
                } else {
                    tmpZR += "<div><span class='bg-warning text-black'>" + DocView.getAppTag(seqObj.dsSeq[i].ApprovalTag) + "</span>";
                    tmpZR += "<span>" + seqObj.dsSeq[i].UserName + "(" + seqObj.dsSeq[i].TitleName + "/" + seqObj.dsSeq[i].DeptName + ")</span></div>";
                    tmpZR += "<div><span style='width:50px;'>&nbsp;</span><span class='btn-sm btn-white'>" + DocView.getAppType(seqObj.dsSeq[i].HGunbun) + "</span>"
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
작성일자 : 2015-10-21
작성목적 : 결재태그 이름
***************************************************/
DocView.getAppTag = function (val) {
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
작성일자 : 2015-10-21
작성목적 : 결재상태 이름
***************************************************/
DocView.getAppType = function (val) {
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
작성일자 : 2015-10-21
작성목적 : 문서 정보 가져오기 (웹서비스 호출)
***************************************************/
DocView.getData = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetContentDoc";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID
    };

    var obj1 = eval("DocView.succeededData");
    var obj2 = eval("DocView.errorData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncServiceAll(url, data, obj1, obj2);
}
/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 정보 가져오기 (웹서비스 값 받기 - 에러)
***************************************************/
DocView.errorData = function (data, status) {
    DocView.goError();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 정보 가져오기 (웹서비스 값 받기)
***************************************************/
DocView.succeededData = function (data, status) {
    var daObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (daObj.dsDocInfo.length < 1) {
        DocView.goError();
        return;
    }

    AccTag = daObj.dsDocInfo[0].AccountTag;
    GroupID = daObj.dsDocInfo[0].GroupID;

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

    //    DocView.getAttach();
    DocView.getDS();
    DocView.getBase();
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 첨부파일 가져오기 (웹서비스 호출)
***************************************************/
DocView.getAttach = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetAttachFile";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID
    };

    var obj = eval("DocView.succeededAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 첨부파일 가져오기 (웹서비스 값 받기)
***************************************************/
DocView.succeededAttach = function (data, status) {
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

        tmpAttachTag += "<div style='word-wrap:break-word;'><a class='linkDiv' onclick=\"DocView.goAttach('" + filepath + attObj.dsAtt[i].FileGUID + "','" + tmpFileName + "')\">" + attObj.dsAtt[i].FileName + "</a>";
        tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsAtt[i].FileLength) + "byte</span></div>";
    }

    $("#attach").empty();
    $("#attach").append(tmpAttachTag);

}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 첨부파일 읽기 (웹서비스 호출)
***************************************************/
DocView.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': encodeURIComponent(fileName)
    };

    var obj = eval("DocView.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-06
작성목적 : 문서 닥스트리밍 첨부파일 가져오기 (웹서비스 호출)
***************************************************/
DocView.getDS = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetDSFile";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID
        , 'TYPE': "APP_ATTACH"
    };
    var obj = eval("DocView.succeededDS");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-06
작성목적 : 문서 첨부파일 가져오기 (웹서비스 값 받기)
***************************************************/
DocView.succeededDS = function (data, status) {
    var attObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var filepath = "";
    var tmpAttachTag = R_Attach;

    for (var i = 0; i < attObj.dsDS.length; i++) {
        if (attObj.dsDS[i].FileID.length > 0) {
            tmpAttachTag += "<div><a class='linkDiv' onclick=\"DocView.goDocStream('" + attObj.dsDS[i].FileID + "')\">" + attObj.dsDS[i].FileName + "</a>";
            tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsDS[i].FileLength) + "byte</span></div>";
        } else {
            filepath = attObj.dsDS[i].FilePath.replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/")

            var tmpFileName = attObj.dsDS[i].FileName;
            if (tmpFileName.indexOf("'") > -1) {
                //tmpFileName = attObj.dsAtt[i].FileGUID;
                tmpFileName = tmpFileName.split("'").join("");
            }

            tmpAttachTag += "<div><a class='linkDiv' onclick=\"DocView.goAttach('" + filepath + attObj.dsDS[i].FileGUID + "','" + tmpFileName + "')\">" + attObj.dsDS[i].FileName + "</a>";
            tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsDS[i].FileLength) + "byte</span></div>";
        }

    }

    $("#attach").empty();
    $("#attach").append(tmpAttachTag);

    if (attObj.dsDS.length == 0) {
        DocView.getAttach();
    }

}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
DocView.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        DocView.goError();
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

            location.href = "/DeskPlusMobile/Common/AttachDocView.aspx?p=" + GetEncryptValue;
        }
    }
}



/**************************************************
작성자   : 이중호
작성일자 : 2018-06-21
작성목적 : 문서 스트리밍 Post
***************************************************/
DocView.goDocStream = function (fileID) {

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
작성일자 : 2015-10-21
작성목적 : 문서 근거문서 가져오기 (웹서비스 호출)
***************************************************/
DocView.getBase = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetBaseDoc";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID
    };

    var obj = eval("DocView.succeededBase");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 근거문서 가져오기 (웹서비스 값 받기)
***************************************************/
DocView.succeededBase = function (data, status) {
    var baseObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var tmpBaseTag = R_BaseDoc;
    for (i = 0; i < baseObj.dsBase.length; i++) {
        if (baseObj.dsBase[i].IsLegacy == "N") {
            tmpBaseTag += "<div><a class='linkDiv' id=\"BaseButton" + i + "\" type=\"button\" onclick=\"DocView.goBase('" + baseObj.dsBase[i].ParentDocumentID + "')\" >" + baseObj.dsBase[i].Name + "</a></div>";
        }
        else {
            tmpBaseTag += "<div><a class='linkDiv' href=\"" + baseObj.dsBase[i].Path + "\">" + baseObj.dsBase[i].Name + "</a></div>";
        }
    }

    $("#base").empty();
    $("#base").append(tmpBaseTag);

}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 근거문서 보기 화면으로 가기 
***************************************************/
DocView.goBase = function (baseDID) {

    ParamArray = {
        'BaseDID': baseDID
    };

    CallEncrypt(CallGetParam());

    location.href = "BaseView.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 이전 글 가기
***************************************************/
DocView.minus = function () {
    DocView.getBeforeNext('Top');
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 다음 글 가기
***************************************************/
DocView.plus = function () {
    DocView.getBeforeNext('Down');
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-22
작성목적 : 이전 , 다음 글 가기 (웹서비스 호출)
***************************************************/
DocView.getBeforeNext = function (gubun) {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetBeforeNextDoc";

    //파라미터값 배열에 담기
    ParamArray = {
        'BPage': BPage,
        'DID': DID,
        'Gubun': gubun,
        'Sv': SearchValue.securityText(),
        'TargetDeptID': TargetDeptID,
        'TargetTitleID': TargetTitleID,
        'KeyID': KeyID,
        'Key2ID': Key2ID
    };

    var obj = eval("DocView.succeededBeforeNext");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-22
작성목적 : 이전 , 다음 글 가기 (웹서비스 값 받기)
***************************************************/
DocView.succeededBeforeNext = function (data, status) {
    if (data.d != "0") {
        var tmpVal = data.d;
        var arrVal = tmpVal.split("^");
        if (arrVal[0] != "0") {

            ParamArray = {
                'BPage': BPage,
                'PageNo': PageNo,
                'DID': arrVal[0],
                'Sv': SearchValue.securityText(),
                'KeyID': arrVal[1],
                'key2ID': arrVal[2]
            };

            CallEncrypt(CallGetParam());
            document.location.href = "DocView.aspx?p=" + GetEncryptValue;
        }
        else {
            action_popup.alert(R_NoList);
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-22
작성목적 : 목록 페이지 이동
***************************************************/
DocView.goBefore = function () {
    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText(),
        'TargetDeptID': TargetDeptID,
        'TargetTitleID': TargetTitleID
    };

    CallEncrypt(CallGetParam());

    if (BPage == "Public") {
        location.href = "DeptList.aspx?p=" + GetEncryptValue;
    }
    else if (BPage == "Carboncopy") {
        location.href = "CarboncopyList.aspx?p=" + GetEncryptValue;
    }
    else {
        location.href = "DocList.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-22
작성목적 : 에러 발생시에 처리
***************************************************/
DocView.goError = function () {
    action_popup.alert(R_ReLoadDoc);
    location.href = "Folder.aspx";
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 참조문서 확인일자 수정 (웹서비스 호출)
***************************************************/
DocView.ModCarboncopyViewingDate = function () {

    var url = "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/ModCarboncopyViewingDate";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID
    };

    var obj = eval("DocView.succeededCarboncopyDate");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 참조문서 확인일자 수정 (웹서비스 값 받기)
***************************************************/
DocView.succeededCarboncopyDate = function (data, status) {

}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 전달문서 확인일자 수정 (웹서비스 호출)
***************************************************/
DocView.ModForwardViewingDate = function () {

    var url = "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/ModForwardViewingDate";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DID
    };

    var obj = eval("DocView.succeededViewingDate");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 전달문서 확인일자 수정 (웹서비스 값 받기)
***************************************************/
DocView.succeededViewingDate = function (data, status) {

}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 전달문서 의견보기
***************************************************/
DocView.goMemo = function () {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'DID': DID,
        'Sv': SearchValue.securityText(),
        'KeyID': KeyID,
        'key2ID': Key2ID
    };

    CallEncrypt(CallGetParam());

    location.href = "Memo.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2017-01-17
작성목적 : 의견 추가화면 이동
***************************************************/
DocView.AddOpinion = function () {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'DID': DID,
        'Sv': SearchValue.securityText(),
        'TargetDeptID': TargetDeptID,
        'TargetTitleID': TargetTitleID,
        'KeyID': KeyID,
        'key2ID': Key2ID,
        'AccTag': AccTag,
        'GroupID': GroupID
    };

    CallEncrypt(CallGetParam());

    location.href = "DocOpinion.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : kkzz52
작성일자 : 2022-01-18
작성목적 : 결재의견 가져오기 (웹서비스 호출)
***************************************************/
DocView.getOpinion = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetOpinionList";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': CDID
    };

    var obj = eval("DocView.succeededOpinion");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2022-01-18
작성목적 : 결재의견 가져오기 (웹서비스 값 받기)
***************************************************/
DocView.succeededOpinion = function (data, status) {
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