var ListAdmin = {};

$(function () {

    $(document).ready(function () {

        $('#IngLayer').scroll(function () {

            if ($('#IngLayer').scrollTop() > 650) { //최하단의 위치값과 동일여부
                if (isScroll == "") {
                    alert("추가 데이터를 가져 옵니다. \n잠시기다려 주세요.");
                    isScroll = "A";
                    __doPostBack('LBTNProcess', 'fullDataLoad');
                    //alert($('#IngLayer').scrollTop()
                }
                // 나머지 값들 조회 
            }
        });

        //lastPosted(); //스크롤페이징 ajax 호출 스크립트
    });

    $('#spanSubmit').click(function (e) {
        e.preventDefault();
        ListAdmin.Submit();
    });

    $('#spanReject').click(function (e) {
        e.preventDefault();
        ListAdmin.Reject();
    });

    $('#spanBatch').click(function (e) {
        e.preventDefault();
        ListAdmin.ChangeBatch();
    });

    //$(".tdate").datepick($.datepicker.regional[PreLang]);

    // 최초 승인대기항목만 보임
    $('#EndLayer').hide();

    $('.chkNum').onlyNumber();

    $("#SelectedAmount", "#DocumentAmount", "#TotalAmount").attr("style", "text-align:right;");


    $('#templeteNM').text(TempleteName);
    $('#batchNM').text(BatchName);

    //$('.datepick')).datepick($.datepicker.regional[PreLang]);
    //$('.datepick')).keydown(function (e) { return false; });

    //검색입력필드 엔터 이벤트 기술
    $("#ctbSearch").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            ListAdmin.GetPageList();
        }
    });

    //검색
    $('#cbtnSearch').click(function (e) {
        e.preventDefault();
        ListAdmin.GetPageList();
    });

    //검색어입력
    $("#ctbSearch").attr("placeholder", R_PlaceholderText);
});



ListAdmin.SetTab = function (_section) {

    if (_section == "I") {
        $('#IngLayer').show();
        $('#EndLayer').hide();

        $("#tabIng").attr("class", "tab_on");
        $("#tabEnd").attr("class", "");

        $('#spanReject').show();
        $('#spanSubmit').show();

        IsTab = "I";

        $("#DocumentAmount").val("");
        $("#TmpData").val("99999");

        __doPostBack('LBTNProcess', 'LoadDatail');

    } else {
        $('#IngLayer').hide();
        $('#EndLayer').show();

        $("#tabEnd").attr("class", "tab_on");
        $("#tabIng").attr("class", "");

        $('#spanReject').hide();
        $('#spanSubmit').hide();

        IsTab = "C";

        $("#DocumentAmount").val("");
        $("#TmpData").val("99999");

        __doPostBack('LBTNProcess', 'LoadDatail');

    }
}


ListAdmin.ChangeBatch = function () {

    ParamArray = {
        'VendorCD': '',
        'MasterDetailGubun': ''
    };

    CallEncrypt(CallGetParam());
    var url = "../User/Popup/PopBatch.aspx?p=" + GetEncryptValue;
    PopupWin(500, 400, url, "-500", "", true);
}


ListAdmin.BatchBind = function (_tVal, _bVal) {

    TempleteName = _tVal;
    BatchName = _bVal;

    $('#templeteNM').text(TempleteName);
    $('#batchNM').text(BatchName);
}

ListAdmin.GetDetail = function (_val, _exVendorCD, _kind, _amount) {

    SelVoucher = _val;
    SelVendor = _exVendorCD;
    SelCardVendorKind = _kind;

    $("#DocumentAmount").val(_amount);
    $("#TmpData").val(_val);

    __doPostBack('LBTNProcess', 'LoadDatail');
}


ListAdmin.Submit = function () {

    // 체크된 항목 정보 가져오기 
    var CheckVal = ListAdmin.CheckedValue();

    if (TempleteName == "") {
        alert("Batch 가 설정 되어 있지 않습니다.");
        return;
    }



    if (CheckVal != "") {

        if (confirm("선택된 Voucher No [" + CheckVal + "] 에 대해 승인 하시겠습니까?")) {
            $("#TmpData").val(CheckVal + "|" + TempleteName + "|" + BatchName);

            __doPostBack('LBTNProcess', 'Submit');
        }
    }

}


ListAdmin.Reject = function () {
    // 체크된 항목 정보 가져오기 
    var CheckVal = ListAdmin.CheckedValue();

    if (CheckVal != "") {
        if (confirm("선택된 Voucher No [" + CheckVal + "] 에 대해 반려 하시겠습니까?")) {
            $("#TmpData").val(CheckVal);

            __doPostBack('LBTNProcess', 'Reject');
        }
    }


}

ListAdmin.CheckedValue = function () {

    var allVals = $('.exCbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


// 결재문서 보기
ListAdmin.AppPopupView = function (p_documentID) {

    ParamArray = {
        'documentID': p_documentID
        , 'srh': ''
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "SHOWDOC", "1024", "700", true)
}


ListAdmin.ExChecked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".exCbxID").attr("checked", true);
    }
    else {
        $(".exCbxID").attr("checked", false);
    }

    ListAdmin.SelectedAmount();
}


ListAdmin.ChangePostingDate = function (_val, _voucherNo) {

    var tmpData = $("#InPostingDate" + _val).val() + "★" + _val + "★" + _voucherNo;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'PostingDate');

}

// 동적 컨트롤 세팅
ListAdmin.PostingDateClick = function (_val, _voucherNo) {
    if (IsTab == "I") {
        $("#LvPostingDate" + _val).hide();
        $("#InPostingDate" + _val).show();
        $("#InPostingDate" + _val).datepick($.datepicker.regional[PreLang]);
    }
}

ListAdmin.ChangeDocumentDate = function (_val, _voucherNo) {

    var tmpData = $("#InDocumentDate" + _val).val() + "★" + _val + "★" + _voucherNo;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'DocumentDate');

}

// 동적 컨트롤 세팅
ListAdmin.DocumentDateClick = function (_val, _voucherNo) {
    if (IsTab == "I") {
        $("#LvDocumentDate" + _val).hide();
        $("#InDocumentDate" + _val).show();
        $("#InDocumentDate" + _val).datepick($.datepicker.regional[PreLang]);
    }
}


ListAdmin.ChangeDueDate = function (_val, _voucherNo) {

    var tmpData = $("#InDueDate" + _val).val() + "★" + _val + "★" + _voucherNo;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'DueDate');

}

// 동적 컨트롤 세팅
ListAdmin.DueDateClick = function (_val, _voucherNo) {
    if (IsTab == "I") {
        $("#LvDueDate" + _val).hide();
        $("#InDueDate" + _val).show();
        $("#InDueDate" + _val).datepick($.datepicker.regional[PreLang]);
    }
}


// 계정과목 관련 클릭
ListAdmin.GLAccountClick = function (_val, _voucherNo) {
    if (IsTab == "I") {
        // _Val = Seq_Kind_LineNo 
        SelVal = _val;
        var kind = _val.split('_')[1];

        if (kind == "E") {
            // 비용라인은 GLAccountPop
            ParamArray = {
                'CardVendorKind': SelCardVendorKind,
                'ItemKey': '',
                'GLAccountID': '',
                'IsAdmin': 'Y'
            };

            CallEncrypt(CallGetParam());
            var url = "../User/Popup/PopGLAccount.aspx?p=" + GetEncryptValue;
            PopupWin(500, 350, url, "-500", "", true);
        }
        else if (kind == "T") {
            // Tax 라인은 Tax 팝업 
            // 카드의 경우 Tax 가 없으니 선택 할 수 없다.
            if (SelCardVendorKind == "V") {
                // 일단 현재로선 계산이 스지 않으니 주석 잡자.. Tax 가 바뀔때에 고려 대상이 너무 많음
                //            ParamArray = {
                //                'CardVendorKind': SelCardVendorKind,
                //                'ItemKey': '',
                //                'GLAccountID': '',
                //                'IsAdmin': 'Y'

                //            };
                //            CallEncrypt(CallGetParam());
                //            var url = "../User/Popup/PopTax.aspx?p=" + GetEncryptValue;
                //            PopupWin(500, 300, url, "-500", "", true);
            }
        }
        else { // V 라인은 벤더 
            ParamArray = {
                'VendorCD': '',
                'MasterDetailGubun': 'D',
                'IsAdmin': 'Y'
            };

            CallEncrypt(CallGetParam());
            var url = "../User/Popup/PopVendor.aspx?p=" + GetEncryptValue;
            PopupWin(500, 500, url, "-500", "", true);
        }
    }

}

// 계정과목 선택시 바인딩
ListAdmin.GLAccountbind = function (_code, _name) {

    var tmpData = _code + "|" + _name + "★" + SelVoucher + "|" + SelVendor + "|" + SelCardVendorKind + "|" + SelVal;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'GLAccount');
}

// 벤더 바인딩 저장
ListAdmin.VendorBind = function (_venCD, _venNM, _postingG) {
    var tmpData = _venCD + "|" + _venNM + "|" + _postingG + "★" + SelVoucher + "|" + SelVendor + "|" + SelCardVendorKind + "|" + SelVal;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'Vendor');
}


ListAdmin.PostingGroupClick = function (_val, _voucherNo) {
    if (IsTab == "I") {
        // _Val = Seq_Kind_LineNo 
        SelVal = _val;
        var kind = _val.split('_')[1];

        if (kind == "V") { // 벤더라인만.
            ParamArray = {
                'VendorCD': '',
                'MasterDetailGubun': '',
                'IsAdmin': 'Y'
            };

            CallEncrypt(CallGetParam());
            var url = "../User/Popup/PopPostingGroup.aspx?p=" + GetEncryptValue;
            PopupWin(300, 500, url, "-500", "", true);

        }
    }
}


// 벤더 바인딩 저장
ListAdmin.PostingGroupBind = function (_val) {
    var tmpData = _val + "|" + "" + "★" + SelVoucher + "|" + SelVendor + "|" + SelCardVendorKind + "|" + SelVal;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'PostingGroup');
}


ListAdmin.DesClick = function (_val, _voucherNo) {
    if (IsTab == "I") {
        // _Val = Seq_Kind_LineNo 
        SelVal = _val;
        var kind = _val.split('_')[1];

        // input 컨트롤 활성화 
        $("#LvDes" + _val).hide();
        $("#InDes" + _val).show();

        $("#InDes" + _val).keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListAdmin.UpdateDes(_val);
            }
        });
    }

}

ListAdmin.UpdateDes = function (_val) {

    var Data = $("#InDes" + _val).val();

    var tmpData = Data + "|" + "" + "★" + SelVoucher + "|" + SelVendor + "|" + SelCardVendorKind + "|" + SelVal;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'Des');
}


ListAdmin.DebitAmountClick = function (_val, _voucherNo) {
    if (IsTab == "I") {
        // _Val = Seq_Kind_LineNo 
        SelVal = _val;
        var kind = _val.split('_')[1];

        // input 컨트롤 활성화 
        $("#LvDebit" + _val).hide();
        $("#InDebit" + _val).show();
        $("#InDebit" + _val).onlyNumber();

        $("#InDebit" + _val).keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListAdmin.UpdateDebit(_val);
            }
        });
    }
}

ListAdmin.CreditAmountClick = function (_val, _voucherNo) {
    // _Val = Seq_Kind_LineNo 
    SelVal = _val;
    var kind = _val.split('_')[1];

    // input 컨트롤 활성화 
    $("#LvCredit" + _val).hide();
    $("#InCredit" + _val).show();
    $("#InCredit" + _val).onlyNumber();

    $("#InCredit" + _val).keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            ListAdmin.UpdateCredit(_val);
        }
    });
}


ListAdmin.UpdateDebit = function (_val) {

    var Data = $("#InDebit" + _val).val();

    var tmpData = Data + "|" + "" + "★" + SelVoucher + "|" + SelVendor + "|" + SelCardVendorKind + "|" + SelVal;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'Amount');
}


ListAdmin.UpdateCredit = function (_val) {

    var Data = $("#InCredit" + _val).val();

    var tmpData = Data + "|" + "" + "★" + SelVoucher + "|" + SelVendor + "|" + SelCardVendorKind + "|" + SelVal;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'Amount');
}



ListAdmin.BankClick = function (_val, _voucherNo) {
    // _Val = Seq_Kind_LineNo 
    if (IsTab == "I") {
        SelVal = _val;
        var kind = _val.split('_')[1];

        if (kind == "V") { // 벤더라인만.
            ParamArray = {
                'VendorCD': SelVendor,
                'MasterDetailGubun': '',
                'IsAdmin': 'Y'
            };

            CallEncrypt(CallGetParam());
            var url = "../User/Popup/PopBankAccount.aspx?p=" + GetEncryptValue;
            PopupWin(500, 200, url, "-500", "", true);

        }
    }
}


ListAdmin.BankBind = function (_code, _bankAccountName, _bankClearingID) {
    var tmpData = _code + "|" + _bankAccountName + "|" + _bankClearingID + "★" + SelVoucher + "|" + SelVendor + "|" + SelCardVendorKind + "|" + SelVal;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'Bank');
}


ListAdmin.DepartMentClick = function (_val, _voucherNo) {
    if (IsTab == "I") {
        // _Val = Seq_Kind_LineNo 
        SelVal = _val;
        var kind = _val.split('_')[1];

        ParamArray = {
            'VendorCD': '',
            'MasterDetailGubun': '',
            'IsAdmin': 'Y'
        };

        CallEncrypt(CallGetParam());
        var url = "../User/Popup/PopDepartMent.aspx?p=" + GetEncryptValue;
        PopupWin(400, 500, url, "-500", "", true);
    }
}

ListAdmin.EmployeeClick = function (_val, _voucherNo) {
    // _Val = Seq_Kind_LineNo 
    if (IsTab == "I") {
        SelVal = _val;
        var kind = _val.split('_')[1];

        ParamArray = {
            'VendorCD': '',
            'MasterDetailGubun': '',
            'IsAdmin': 'Y'
        };

        CallEncrypt(CallGetParam());
        var url = "../User/Popup/PopEmployee.aspx?p=" + GetEncryptValue;
        PopupWin(400, 500, url, "-500", "", true);
    }
}

// Department Pop에서 바인딩
ListAdmin.DepartMentBind = function (_code, _name) {

    var tmpData = _code + "|" + _name + "★" + SelVoucher + "|" + SelVendor + "|" + SelCardVendorKind + "|" + SelVal;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'Department');


}

ListAdmin.EmployeeBind = function (_code, _name) {

    var tmpData = _code + "|" + _name + "★" + SelVoucher + "|" + SelVendor + "|" + SelCardVendorKind + "|" + SelVal;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'Employee');

}


ListAdmin.VendorClick = function (_val, _voucherNo) {
    if (IsTab == "I") {
        // _Val = Seq_Kind_LineNo 
        SelVal = _val;
        var kind = _val.split('_')[1];

        ParamArray = {
            'VendorCD': '',
            'MasterDetailGubun': 'D',
            'IsAdmin': 'Y'
        };

        CallEncrypt(CallGetParam());
        var url = "../User/Popup/PopVendor.aspx?p=" + GetEncryptValue;
        PopupWin(500, 500, url, "-500", "", true);
    }
}

ListAdmin.ProductGroupClick = function (_val, _voucherNo) {
    // _Val = Seq_Kind_LineNo 
    if (IsTab == "I") {
        SelVal = _val;
        var kind = _val.split('_')[1];

        if (kind != "V") { // 벤더라인만.

            ParamArray = {
                'CardVendorKind': 'C',
                'ItemKey': _val,
                'GLAccountID': '',
                'IsAdmin': 'Y'
            };

            CallEncrypt(CallGetParam());

            var url = "../User/Popup/PopProductGroup.aspx?p=" + GetEncryptValue;

            PopupWin(500, 350, url, "-500", "", true);
        }
    }

}

ListAdmin.PopProductGroupBind = function (_code, _name) {

    var tmpData = _code + "|" + _name + "★" + SelVoucher + "|" + SelVendor + "|" + SelCardVendorKind + "|" + SelVal;

    $("#TmpData").val(tmpData);

    __doPostBack('LBTNProcess', 'ProductGroup');

}


ListAdmin.SelectedAmount = function () {
    // 체크된 항목 정보 가져오기 
    var CheckVal = ListAdmin.CheckedValue();
    var selAmount1 = 0;

    $.each(CheckVal, function (i, o) {
        selAmount1 += parseInt($("#CreditExLevel" + o).text().replace(/,/gi, ""));
        // 부가세가 0 이나 "" 이 아닌경우 공급가액을 세로 세팅 해준다.
    });

    $("#SelectedAmount").val(ListAdmin.FormatNumber(selAmount1, 3));
}


//숫자 자릿수 콤마찍기
ListAdmin.FormatNumber = function (price, num) {
    var str = new Array();
    minus = "";
    if (price < 0) {
        minus = "-";
        price = price * -1;
    }
    price = String(price);

    for (var i = 1; i <= price.length; i++) {
        if (i % num) str[price.length - i] = price.charAt(price.length - i);
        else str[price.length - i] = ',' + price.charAt(price.length - i);
    }
    return minus + str.join('').replace(/^,/, '');
}


/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ListAdmin.GetPageList = function () {

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();

    if (SearchValue != "") {
        var tmpData = SearchField + "★" + SearchValue;


        $("#TmpData").val(tmpData);

        __doPostBack('LBTNProcess', 'Search');
    }

}



