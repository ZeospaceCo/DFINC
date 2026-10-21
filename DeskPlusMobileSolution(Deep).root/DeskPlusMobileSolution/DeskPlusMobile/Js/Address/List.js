List = {}

/**************************************************
작성자   : freeman
작성일자 : 2011-07-09
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    $("span.dy").click(function () {
        $("#addisrc").toggle();
        $("#addirec").toggle();
        if ($("#addisrc").is(":visible")) {
            $("span.dy").html(R_GoDirect + " ▼&nbsp;&nbsp;");
            $("#tbSearchValue").val("");
            $("ul.abt li>a").attr("class", "btbg");
            FirstChar = "";
            FastPath = "0";
        } else {
            $("span.dy").html(R_GoDirect + " ▲&nbsp;&nbsp;");
            $("#tbSearchValue").val("");
            FirstChar = "";
            FastPath = "1";
        }
    });

    if (FastPath == "1") {
        $("span.dy").html(R_GoDirect + " ▲");
        $("#addisrc").hide();
        $("#addirec").show();
    }

    if (FirstChar != "") {
        var tmpArg = "";

        switch (FirstChar) {
            case "ㄱ":
                tmpArg = "f1";
                break;
            case "ㄴ":
                tmpArg = "f2";
                break;
            case "ㄷ":
                tmpArg = "f3";
                break;
            case "ㄹ":
                tmpArg = "f4";
                break;
            case "ㅁ":
                tmpArg = "f5";
                break;
            case "ㅂ":
                tmpArg = "f6";
                break;
            case "ㅅ":
                tmpArg = "f7";
                break;
            case "ㅇ":
                tmpArg = "f8";
                break;
            case "ㅈ":
                tmpArg = "f9";
                break;
            case "ㅊ":
                tmpArg = "f10";
                break;
            case "ㅋ":
                tmpArg = "f11";
                break;
            case "ㅌ":
                tmpArg = "f12";
                break;
            case "ㅍ":
                tmpArg = "f13";
                break;
            case "ㅎ":
                tmpArg = "f14";
                break;
            case "A":
                tmpArg = "f15";
                break;
            case "0":
                tmpArg = "f16";
                break;
            default:
                tmpArg = "f1";
                break;
        }

        $("#" + tmpArg).attr("class", "btbg on");

        if (Gubun == "G") {
            $("div.nv1 ul>li").attr("class", "l1");
            $("div.nv1 ul>li:eq(0)").attr("class", "l1 on");
        }
        else if (Gubun == "C") {
            $("div.nv1 ul>li").attr("class", "l1");
            $("div.nv1 ul>li:eq(1)").attr("class", "l1 on");
        }
        else if (Gubun == "P") {
            $("div.nv1 ul>li").attr("class", "l1");
            $("div.nv1 ul>li:eq(2)").attr("class", "l1 on");
        }

        List.GetList();
    }
    else {
        setTimeout("List.Load(Gubun);", 300);
    }

    $('#gubunG').click(function (e) {
        Gubun = "G";
        List.search();
    });

    $('#gubunC').click(function (e) {
        Gubun = "C";
        List.search();
    });

    $('#gubunP').click(function (e) {
        Gubun = "P";
        List.search();
    });

    $('#search').click(function (e) {
        e.preventDefault();
        List.search();
    });

    $('#pvNum').click(function (e) {
        e.preventDefault();
        List.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        List.nxNum();
    });

    $('#confirm').click(function (e) {
        List.Confirm();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    $("#tbSearchValue").attr("placeholder", R_InputAdd);

});

/**************************************************
작성자   : freeman
작성일자 : 2011-07-14
작성목적 : 주소록 가져오기 (웹서비스 호출)
***************************************************/
List.GetList = function () {
    //LoadingShow(true);

    var url = "/DeskPlusMobile/Address/Ws/GetAddress.asmx/GetAddressList";
    SearchValue = $("#tbSearchValue").val();

    //파라미터값 배열에 담기
    ParamArray = {
        'firstChar': FirstChar,
        'searchValue': SearchValue.securityText(),
        'gubun': Gubun,
        'pageNo': PageNo,
        'pageSize': PageSize
    };

    var obj = eval("List.SucceededAddress");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : freeman
작성일자 : 2011-07-14
작성목적 : 주소록 가져오기 (웹서비스 값 받기)
***************************************************/
List.SucceededAddress = function (data, status) {
    $('#phAddress').setTemplateURL('/DeskPlusMobile/Address/Jt/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phAddress').processTemplate(data.d);

    setTimeout(function () {
        List.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2011-07-14
작성목적 : 주소록 가져오기 
***************************************************/
List.Load = function (arg) {
    //초기화
    $('#phAddress').html("");

    FirstChar = "";

    switch (arg) {
        case "G":
            Gubun = "G";
            $("div.nv1 ul>li").attr("class", "l1");
            $("div.nv1 ul>li:eq(0)").attr("class", "l1 on");
            break;
        case "C":
            Gubun = "C";
            $("div.nv1 ul>li").attr("class", "l1");
            $("div.nv1 ul>li:eq(1)").attr("class", "l1 on");
            break;
        case "P":
            Gubun = "P";
            $("div.nv1 ul>li").attr("class", "l1");
            $("div.nv1 ul>li:eq(2)").attr("class", "l1 on");
            break;
        case "S":
            break;
        default:
            $("ul.abt li>a").attr("class", "btbg");
            $("#" + arg).attr("class", "btbg on");
            FirstChar = $("#" + arg).text();

            if (FirstChar == "A-Z") {
                FirstChar = "A";
            } else if (FirstChar == "0-9") {
                FirstChar = "0";
            }

            PageNo = 1;
            break;
    }

    List.GetList();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-27
작성목적 : 주소록 가져오기 
***************************************************/
List.LoadList = function (arg) {

    if (List.CheckMail() != "") {
        action_popup.confirm(R_ConfirmMail, function (res) {
            if (res) {
                List.Confirm();
            }
            else {

                FirstChar = $("#" + arg).text();

                if (FirstChar == "A-Z") {
                    FirstChar = "A";
                } else if (FirstChar == "0-9") {
                    FirstChar = "0";
                }

                ParamArray = {
                    'gubun': Gubun,
                    'PageNo': 1,
                    'FastPath': FastPath,
                    'FirstChar': FirstChar,
                    'Action': Action,
                    'MailID': MailID,
                    'Code': Code,
                    'Mail_sn': Mail_sn,
                    'Hearder': Hearder,
                    'History': History
                };

                CallEncrypt(CallGetParam());

                location.href = "List.aspx?p=" + GetEncryptValue;
            }
        })
    }
    else {
        FirstChar = $("#" + arg).text();

        if (FirstChar == "A-Z") {
            FirstChar = "A";
        } else if (FirstChar == "0-9") {
            FirstChar = "0";
        }

        ParamArray = {
            'gubun': Gubun,
            'PageNo': 1,
            'FastPath': FastPath,
            'FirstChar': FirstChar,
            'Action': Action,
            'MailID': MailID,
            'Code': Code,
            'Mail_sn': Mail_sn,
            'Hearder': Hearder,
            'History': History
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2011-07-08
작성목적 : 상세정보 보기 
***************************************************/
List.ViewList = function (_gubun, ownerID, mail) {

    if (jQuery.trim(Hearder) != "") {
        if (mail == "") {
            NoticeShow("!", NoMail);
            return;
        }
        else {
            if (FileID != "") {
                ParamArray = {
                    'Mail': mail,
                    'MailID': MailID,
                    'Hearder': Hearder,
                    'FileID': FileID,
                    'SwfUploadGUID': SwfUploadGUID,
                    'FileCheck': FileCheck,
                    'FolderID': FolderID,
                    'PageNo': PageNo,
                    'Order': Order,
                    'ShareGubun': ShareGubun,
                    'PublicGubun': PublicGubun
                };

                CallEncrypt(CallGetParam());

                location.href = "/DeskPlusMobile/WebHard/Send.aspx?p=" + GetEncryptValue;
            }
            else {

                ParamArray = {
                    'Mail': mail,
                    'MailID': MailID,
                    'Action': Action,
                    'Code': Code,
                    'Mail_sn': Mail_sn,
                    'Hearder': Hearder,
                    'History': History,
                    'SwfUploadGUID': SwfUploadGUID,
                    'FileCheck': FileCheck
                };

                CallEncrypt(CallGetParam());

                //location.href = "/DeskPlusMobile/Mail/Send.aspx?p=" + GetEncryptValue;
                location.href = "/DeskPlusMobile/Mail/" + CallPage + ".aspx?p=" + GetEncryptValue;
            }
        }
    }
    else {
        SearchValue = $("#tbSearchValue").val();

        ParamArray = {
            'gubun': _gubun,
            'ownerID': ownerID,
            'PageNo': PageNo,
            'FastPath': FastPath,
            'FirstChar': FirstChar,
            'sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-02-04
작성목적 : 검색
***************************************************/
List.SetSearch = function () {
    List.search();

    return false;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-27
작성목적 : 검색
***************************************************/
List.search = function () {

    if (List.CheckMail() != "") {
        action_popup.confirm(R_ConfirmMail, function (res) {
            if (res) {
                List.Confirm();
            }
            else {
                SearchValue = $("#tbSearchValue").val();

                ParamArray = {
                    'gubun': Gubun,
                    'PageNo': 1,
                    'FastPath': FastPath,
                    'FirstChar': FirstChar,
                    'sv': SearchValue.securityText(),
                    'Action': Action,
                    'MailID': MailID,
                    'Code': Code,
                    'Mail_sn': Mail_sn,
                    'Hearder': Hearder,
                    'History': History

                };

                CallEncrypt(CallGetParam());

                location.href = "List.aspx?p=" + GetEncryptValue;
            }
        })
    }
    else {
        SearchValue = $("#tbSearchValue").val();

        ParamArray = {
            'gubun': Gubun,
            'PageNo': 1,
            'FastPath': FastPath,
            'FirstChar': FirstChar,
            'sv': SearchValue.securityText(),
            'Action': Action,
            'MailID': MailID,
            'Code': Code,
            'Mail_sn': Mail_sn,
            'Hearder': Hearder,
            'History': History

        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-01
작성목적 : 확인버튼
***************************************************/
List.Confirm = function () {
    var tmpValue = "";
    var tmpCheck = "";
    for (var i = 0; i < $("input:checkbox").length; i++) {
        if ($("input:checkbox")[i].checked == true) {
            tmpCheck = "Y";

            var arrVal = $("input:checkbox")[i].value.split('_');
            if (arrVal[1] != "") {
                tmpValue += arrVal[1] + ";";
            }
        }
    }

    if (tmpValue == "") {
        if (tmpCheck == "Y") {
            NoticeShow("!", R_NoMail);
            return;
        }
        else {
            NoticeShow("!", R_NoSelList);
            return;
        }
    }

    ParamArray = {
        'Mail': tmpValue,
        'MailID': MailID,
        'Action': Action,
        'Code': Code,
        'Mail_sn': Mail_sn,
        'Hearder': Hearder,
        'History': History
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Mail/Send.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-27
작성목적 : 이전 페이지 이동
***************************************************/
List.pvNum = function () {
    if (parseInt(PageNo) > 1) {

        if (List.CheckMail() != "") {
            action_popup.confirm(R_ConfirmMail, function (res) {
                if (res) {
                    List.Confirm();
                }
                else {
                    PageNo = parseInt(PageNo) - 1;

                    ParamArray = {
                        'gubun': Gubun,
                        'PageNo': PageNo,
                        'FastPath': FastPath,
                        'FirstChar': FirstChar,
                        'sv': SearchValue.securityText(),
                        'Action': Action,
                        'MailID': MailID,
                        'Code': Code,
                        'Mail_sn': Mail_sn,
                        'Hearder': Hearder,
                        'History': History
                    };

                    CallEncrypt(CallGetParam());

                    location.href = "List.aspx?p=" + GetEncryptValue;
                }
            })
        }
        else {

            PageNo = parseInt(PageNo) - 1;

            ParamArray = {
                'gubun': Gubun,
                'PageNo': PageNo,
                'FastPath': FastPath,
                'FirstChar': FirstChar,
                'sv': SearchValue.securityText(),
                'Action': Action,
                'MailID': MailID,
                'Code': Code,
                'Mail_sn': Mail_sn,
                'Hearder': Hearder,
                'History': History
            };

            CallEncrypt(CallGetParam());

            location.href = "List.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-27
작성목적 : 다음 페이지 이동
***************************************************/
List.nxNum = function () {
    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {

        if (List.CheckMail() != "") {
            action_popup.confirm(R_ConfirmMail, function (res) {
                if (res) {
                    List.Confirm();
                }
                else {
                    PageNo = parseInt(PageNo) + 1;

                    ParamArray = {
                        'gubun': Gubun,
                        'PageNo': PageNo,
                        'FastPath': FastPath,
                        'FirstChar': FirstChar,
                        'sv': SearchValue.securityText(),
                        'Action': Action,
                        'MailID': MailID,
                        'Code': Code,
                        'Mail_sn': Mail_sn,
                        'Hearder': Hearder,
                        'History': History
                    };

                    CallEncrypt(CallGetParam());

                    location.href = "List.aspx?p=" + GetEncryptValue;
                }
            })
        }
        else {
            PageNo = parseInt(PageNo) + 1;

            ParamArray = {
                'gubun': Gubun,
                'PageNo': PageNo,
                'FastPath': FastPath,
                'FirstChar': FirstChar,
                'sv': SearchValue.securityText(),
                'Action': Action,
                'MailID': MailID,
                'Code': Code,
                'Mail_sn': Mail_sn,
                'Hearder': Hearder,
                'History': History
            };

            CallEncrypt(CallGetParam());

            location.href = "List.aspx?p=" + GetEncryptValue;
        }
    }
}

List.CheckMail = function () {

    var tmpValue = "";
    for (var i = 0; i < $("input:checkbox").length; i++) {
        if ($("input:checkbox")[i].checked == true) {

            var arrVal = $("input:checkbox")[i].value.split('_');
            if (arrVal[1] != "") {
                tmpValue = arrVal[1];
                break;
            }
        }
    }

    return tmpValue;
}