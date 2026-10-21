var AddMaster = {};

$(function () {


    // 벤더 저장
    $('#ExpenseSave').click(function (e) {
        e.preventDefault();
        AddMaster.ExpenseSave();
    });

    // 포스팅 Date 달력
    $("#ctbPostingDate").datepick($.datepicker.regional[PreLang]);
    $("#ctbPostingDate").keydown(function (e) { return false; });

    $("#ctbDocumentDateAdd").datepick($.datepicker.regional[PreLang]);
    $("#ctbDocumentDateAdd").keydown(function (e) { return false; });

    $("#ctbDueDateAdd").datepick($.datepicker.regional[PreLang]);
    $("#ctbDueDateAdd").keydown(function (e) { return false; });


    $("#CardPerioFr").datepick($.datepicker.regional[PreLang]);
    $("#CardPerioFr").keydown(function (e) { return false; });

    $("#CardPerioTo").datepick($.datepicker.regional[PreLang]);
    $("#CardPerioTo").keydown(function (e) { return false; });

    //기간 바뀔때 이벤트 정의
    $('#CardPerioFr').change(function (e) {
        e.preventDefault();
        AddMaster.LoadCardPeriod();
    });

    //기간 바뀔때 이벤트 정의
    $('#CardPerioTo').change(function (e) {
        e.preventDefault();
        AddMaster.LoadCardPeriod();
    });

    // 벤더-금액입력 필드 정렬처리
    $("#ctbAmountAdd, #ctbTaxAmountAdd").attr("style", "text-align:right;width:90%;");
    $("#SelectedAmount", "#DocumentAmount", "#TotalAmount").attr("style", "text-align:right;");

    // 최초 법인카드만 보임
    $('#ExpenseLayer').hide();


    // 전자결재 호출 
    $('#approvalSubmit').click(function (e) {
        e.preventDefault();
        AddMaster.ApprovalSubmit();
    });

    $('#draft').click(function (e) {
        e.preventDefault();
        AddMaster.Draft();
    });


    $('#ExpenseNew').click(function (e) {
        e.preventDefault();
        AddMaster.ExpenseNew();
    });

    $('#ExpenseUpdate').click(function (e) {
        e.preventDefault();
        AddMaster.ExpenseUpdate();
    });

    $('#ExpenseReset').click(function (e) {
        e.preventDefault();
        AddMaster.ExpenseReset();
    });

    //postingDate 바뀔때 DueDate 바뀌도록 Event 정의
    $('#ctbPostingDate').change(function (e) {
        e.preventDefault();
        AddMaster.ChangePostingDate();
    });


    //벤더에서 DocumentDate 바뀔때 DueDate 바뀌도록 Event 정의
    $('#ctbDocumentDateAdd').change(function (e) {
        e.preventDefault();
        AddMaster.ChangeDocumentDate();
    });

    //숫자만 입력체크
    $('.chkNum').onlyNumber();      //Common.js에 구현


    //최초기초데이터값 체크  
    if (ExUseValidation == "1") {
        alert("비용정산 이용을 위한 시스템코드정보가 그룹웨어에 등록되어 있지 않거나\nNAV에 사용자 Vendor 혹은 BankAccount 가 등록되어 있지 않습니다.\n관리자에게 문의해주세요.");
        history.back();
    }

    if (ExUseValidation == "2") {
        if (confirm("법인카드 비용정산 이용을 위한 카드정보가 등록되어 있지 않습니다.\n등록하시겠습니까?")) {
            alert("등록 후에는 해당 페이지를 새로 고침 해주세요.");
            AddMaster.CardInfo();
        }
        else {
            $('#ExpenseLayer').show();
            $('#CardLayer').hide();
            $('#CardDate').hide();
            $('#CardPeriodSh').hide();
            $('#CardLayerbtn').hide();
            $('#CardLayerbtn1').hide();

            $("#tabCard").attr("class", "");
            $("#tabVendor").attr("class", "tab_on");

            // 카드레이어 이용 불가
            $('#tabCard').attr("disabled", "true");

            CardExpenseKind = "V";
        }

    }

    // 카드이용 내역이 없는 경우 초기값을 벤더 세팅
    if (ExUseValidation == "3") {
        $('#ExpenseLayer').show();
        $('#CardLayer').hide();
        $('#CardDate').hide();
        $('#CardPeriodSh').hide();
        $('#CardLayerbtn').hide();
        $('#CardLayerbtn1').hide();

        $("#tabCard").attr("class", "");
        $("#tabVendor").attr("class", "tab_on");

        CardExpenseKind = "V";
    }


    $("#ctbAmountAdd").keyup(function (e) {
        AddMaster.AmountAddKeyDown();
    });


    $("#ctbAmountAdd").focusout(function (e) {
        AddMaster.AmountAddfocusout();
    });


    __addEventListener(document, "click", AddMaster.click);


});




AddMaster.UpLayer = function () {
    $("#modDes").hide();
    $("#spanUpLayer").hide();
    $("#spanDownLayer").show();
}


AddMaster.DownLayer = function () {
    $("#modDes").show();
    $("#spanUpLayer").show();
    $("#spanDownLayer").hide();
}


AddMaster.AmountAddfocusout = function () {
    var _taxCD = $("#ctbTaxCodeAdd").val();
    if (_taxCD == "01") {
        // 부가세 금액 자동 계산
        AddMaster.TaxAmount();
    }

    if (_taxCD == "05") {

        // 부가세 카드 금액 자동 계산
        AddMaster.TaxAmount05();
    }

    if (_taxCD == "06") {

        // 부가세 카드 금액 자동 계산
        AddMaster.TaxAmount06();
    }

    if (_taxCD == "00") {
        // 부가세 금액 자동 계산
        AddMaster.TaxAmount00();
    }
    
}


AddMaster.AnotherCard = function () {
    __doPostBack('LBTNProcess', 'AnotherCard');
}


AddMaster.ChangePostingDate = function () {
    var cuDate = $("#ctbPostingDate").val();

    DueDateTemp = AjaxGetDueDate(PaymentTerm, cuDate);

    //$("#ctbDueDateAdd").val(DueDateTemp); // 벤터측 DueDate 는 DocumentDate 가 바뀔때 바꾸어 준다.

    DocumentNo = "";
   // $("#modDes").hide();

    // 포스팅 Date 가 바뀌면 새로운 VoucherNo
}


AddMaster.ChangeDocumentDate = function () {
    var cuDate = $("#ctbDocumentDateAdd").val();

    if (cuDate != "") {
        var tempDate = AjaxGetDueDate(ExPaymentTerm, cuDate);

        $("#ctbDueDateAdd").val(tempDate); // 벤터측 DueDate 는 DocumentDate 가 바뀔때 바꾸어 준다.
    }

    // 포스팅 Date 가 바뀌면 새로운 VoucherNo
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 법인카드 등록
***************************************************/
AddMaster.CardInfo = function () {
    ParamArray = {
        'companyID': '1',
        'userID': UserID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Account/MyInfo/CardInfo.aspx?p=" + GetEncryptValue;

    PopupWin(650, 300, url, "", "", true);
}




AddMaster.Draft = function () {
    var CheckVal = AddMaster.ExpenseReportCheckedValue();
    var IsCard = "";

    if (CheckVal != "") {
        // 카드 내역이 포함 되어 있는지 체크 
        $.each(CheckVal, function (i, o) {
            // 공급가액 빠지는 부분 케어 해준다. 
            if ($("#Type" + o).text() == "Card") {
                IsCard = "Y";
            }
        });
    }

    ParamArray = {
        'companyID': '1',
        'userID': UserID,
        'CheckVal': CheckVal,
        'IsCard': IsCard
    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopDraftList.aspx?p=" + GetEncryptValue;

    PopupWin(450, 400, url, "", "", true);
}


// 벤더에서 New
AddMaster.ExpenseNew = function () {

    $("#ctbVendorCDEx").val("");
    $("#ctbVendorNMEx").val("");
    //$("#ctbDocumentDateAdd").val("");
    //$("#ctbDueDateAdd").val("");
    //$("#ctvPostingGroupAdd").val("");
    $("#ctbAmountAdd").val("");
    $("#ctbTaxCodeAdd").val("00");
    $("#ctbTaxNameAdd").val("해당없음");
    $("#ctbTaxAmountAdd").val("");
    $("#ctbAmountAdd1").val("");
    $("#ctbGLAccountNMAdd").val("");
    $("#ctbGLAccountCDAdd").val("");
    $("#ctvBankAccountCDEx").val("");
    $("#ctbDescriptionAdd").val("");
    $("#ctbDescription").val("");
    $("#ctbDocumentDateAdd").val("");
    $("#ctbDueDateAdd").val("");
    $("#ctvPostingGroupAdd").val("");
    


    // 상단 초기화 리셋 및 저장 후 처리
    $("#ctvDepartMent").val($("#ctbDepartMentOld").val());
    $("#ctvDepartMentCD").val($("#ctbDepartMentOldCD").val());
    $("#ctvEmployee").val($("#ctbEmployeeOld").val());
    $("#ctvEmployeeCD").val($("#ctbEmployeeOldCD").val());
    DepartMentDimensionCD = $("#ctbDepartMentOldCD").val();
    EmpCD = $("#ctbEmployeeOldCD").val();

    ProductGroup = "";
    ProductGroupNM = "";
    DocumentNoEx = ""; // _voucherNo;
    ExVendorCD = "";
    ExBankAccountCD = "";
    ExBankAccountNo = "";
    ExBankClearingID = "";
    //ExVendorPostingGroup = "";

    // Update 버튼 감추기 
    $("#ExpenseUpdate").hide();
    //$("#modDes").hide();
    $("#ExpenseSave").show();

}



// 벤더에서 Reset
AddMaster.ExpenseReset = function () {

    $("#ctbAmountAdd1").val("");
    $("#ctbTaxAmountAdd").val("");
    $("#ctbGLAccountCDAdd").val("");
    $("#ctbGLAccountNMAdd").val("");
    $("#ctbAmountAdd").val("");
    $("#ctbTaxNameAdd").val("해당없음");
    $("#ctbDescription").val("");
    $("#ctbTaxCodeAdd").val("00");
    $("#ctbDescriptionAdd").val("");

    $("#ctbDocumentDateAdd").val("");
    $("#ctbDueDateAdd").val("");
    ProductGroup = "";
    ProductGroupNM = "";

    // Update 버튼 감추기 
    $("#ExpenseUpdate").hide();
    //$("#modDes").hide();
    $("#ExpenseSave").show();

}

// 벤더에서 Update
AddMaster.ExpenseUpdate = function () {
    if ($("#ctbVendorCDEx").val() == "") {
        alert("Vendor 를 입력해 주세요.");
        return;
    }

    if ($("#ctbDocumentDateAdd").val() == "") {
        alert("DocumentDate 를 입력해 주세요.");
        return;
    }

    if ($("#ctbDueDateAdd").val() == "") {
        alert("DueDate 를 입력해 주세요.");
        return;
    }

    if ($("#ctbAmountAdd").val() == "") {
        alert("금액을 입력해 주세요.");
        return;
    }

    if ($("#ctbGLAccountCDAdd").val() == "") {
        alert("G/L Account 를 입력해 주세요.");
        return;
    }

    if ($("#ctbDescriptionAdd").val() == "") {
        alert("비용 Description 을 입력해 주세요.");
        return;
    }

    if ($("#ctvPostingGroupAdd").val() == "") {
        alert("Posting Group 을 입력 해 주세요.");
        return;
    }

    // 체크된 항목 정보 가져오기 
    var tmpVal = "";
    var detailData = "";
    var headVal = "";
    // 체크항목 가져오기 

    var VoucherNo = DocumentNoEx;                              // 바우처넘버
    var CardExKind = "V"; // CardExpenseKind;                        // 카드벤더 구분
    var vendorCD = $("#ctbVendorCD").val();                  // 벤더코드 
    var DocumentDate = $("#ctbDocumentDateAdd").val();           // 
    var PostingDate = $("#ctbPostingDate").val();            // postingDate
    var DueDate = $("#ctbDueDateAdd").val();                // 
    var VendorDescription = $("#ctbDescription").val();      // VendorDescription  

    var iPostingGroup = $("#ctvPostingGroupAdd").val();

    // 100 byte 체크 
    var checkByte;
    checkByte = AddMaster.ByteCheck(VendorDescription);

    if (checkByte > 100) {
        alert("Vendor Description 을 100 Byte 이내로 입력해주세요.");
        return;
    }

    checkByte = AddMaster.ByteCheck($("#ctbDescriptionAdd").val());

    if (checkByte > 100) {
        alert("비용 Description 을 100 Byte 이내로 입력해주세요.");
        return;
    }

    

    // 공급가액 빠지는 부분 케어 해준다. 
    //if ($("#ctbAmountAdd1").val() == "") {
    //    $("#ctbAmountAdd1").val($("#ctbAmountAdd").val());
    //}

    // 합계 혹은 부가세 변경시 공급가액을 재 계산 해준다. 
    AddMaster.SetAmount();


    headVal = VoucherNo + "|" + CardExKind + "|" + vendorCD + "|" + DocumentDate + "|" + PostingDate + "|" + DueDate + "|" + VendorDescription + "|";

    detailData = $("#ctbDescriptionAdd").val() + "|" + $("#ctbAmountAdd1").val() + "|" + $("#ctbTaxAmountAdd").val() + "|" + $("#ctbAmountAdd").val()
                + "|" + DepartMentDimensionCD + "|" + EmpCD + "|" + vendorCD + "|" + ProductGroup + "|" + DepartMentDimensionCD + "|" + EmpCD + "|" + ExVendorCD + "|" + ProductGroup
                + "|" + $("#ctbVendorNM").val() + "|" + $("#ctbVendorNMEx").val() + "|" + $("#ctbGLAccountCDAdd").val() + "|" + $("#ctbGLAccountNMAdd").val()
                + "|" + $("#ctbTaxCodeAdd").val() + "|" + $("#ctbTaxNameAdd").val() + "|" + ExBankAccountCD + "|" + ExBankAccountNo + "|" + ExBankClearingID
                + "|" + iPostingGroup + "|" + BankAccountCD + "|" + BankAccountNo + "|" + BankClearingID + "|" + "" + "|" + "" + "|" + "0" + "|" + "T"
                + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + ""
                + "|" + "" + "|" + ProductGroupNM + "|" + $("#ctbGLAccountNMAdd").val() + "|" + $("#ctvDepartMent").val() + "|" + $("#ctvEmployee").val() + "★" + SelectedSeq;
    // ExDescription | 공급가액(계산된값) | 부가세값 | 합계금액(요청금액) | Dimension1Code(부서코드) | Dimension2Code(사번) | Dimension3Code(벤더) 
    //   | Dimension4Code(Product Group) | ExDimension1Code(부서코드) | ExDimension2Code(사번) | ExDimension3Code(벤더) | ExDimension4Code(Product Group)
    //  | VendorNM | ExVendorNM | GLAccount | GLAccountNM | TaxCode | TaxCodeNM | ExBankAccountCode | ExBankAccountNo | ExBankAccountCearing
    //  | ExPostingGroup | BankAcountCode | BankAccountNo | BankAcountCearingID | PONo | CardApprovalNo | DocumentID | Status 
    //  | etc1 | etc2 | etc3 | etc4 | etc5 | etc6 | etc7 | etc8 | etc9 | etc10
    tmpVal = headVal + detailData;

    $("#TmpData").val(tmpVal);

/* 하단 내용 그대로 초기화 하지 않음
    // 초기화
//    $("#ctbVendorCDEx").val("");
//    $("#ctbVendorNMEx").val("");
//    //$("#ctbDocumentDateAdd").val("");
//    //$("#ctbDueDateAdd").val("");
//    //$("#ctvPostingGroupAdd").val("");
//    $("#ctbAmountAdd").val("");
//    $("#ctbTaxCodeAdd").val("00");
//    $("#ctbTaxNameAdd").val("해당없음");
//    $("#ctbTaxAmountAdd").val("");
//    $("#ctbAmountAdd1").val("");
//    $("#ctbGLAccountNMAdd").val("");
//    $("#ctbGLAccountCDAdd").val("");
//    $("#ctvBankAccountCDEx").val("");
//    $("#ctbDescriptionAdd").val("");
//    $("#ctbDescription").val("");
//    $("#ctbDocumentDateAdd").val("");
//    $("#ctbDueDateAdd").val("");
//    $("#ctvPostingGroupAdd").val("");



//    // 상단 초기화 리셋 및 저장 후 처리
//    $("#ctvDepartMent").val($("#ctbDepartMentOld").val());
//    $("#ctvDepartMentCD").val($("#ctbDepartMentOldCD").val());
//    $("#ctvEmployee").val($("#ctbEmployeeOld").val());
//    $("#ctvEmployeeCD").val($("#ctbEmployeeOldCD").val());
//    DepartMentDimensionCD = $("#ctbDepartMentOldCD").val();
//    EmpCD = $("#ctbEmployeeOldCD").val();

//    ProductGroup = "";
//    DocumentNoEx = ""; // _voucherNo;
//    ExVendorCD = "";
//    ExBankAccountCD = "";
//    ExBankAccountNo = "";
//    ExBankClearingID = "";
//    //ExVendorPostingGroup = "";

//    // Update 버튼 감추기 
//    $("#ExpenseUpdate").hide();
//    $("#ExpenseSave").show();
*/


    __doPostBack('LBTNProcess', 'AddExpenseUpdate');

}



// 카드에서 Update
AddMaster.CardUpdate = function () {
    if (DocumentNo == "") {
        alert("리스트에서 수정하실 카드 항목을 선택 해 주세요.");
        return;
    }

    var DepartMent = $("#ctvDepartMent").val();
    var DepartMentCD = $("#ctvDepartMentCD").val();
    var Employee = $("#ctvEmployee").val();
    var EmployeeCD = $("#ctvEmployeeCD").val();
    var VendorDescription = $("#ctbDescription").val();      // VendorDescription  
    // 100 byte 체크 
    var checkByte;
    checkByte = AddMaster.ByteCheck(VendorDescription);

    if (checkByte > 100) {
        alert("Vendor Description 을 100 Byte 이내로 입력해주세요.");
        return;
    }

    var tmpVal = DepartMent + "|" + DepartMentCD + "|" + Employee + "|" + EmployeeCD + "|" + VendorDescription + "★" + SelectedSeq;

    $("#TmpData").val(tmpVal);

    __doPostBack('LBTNProcess', 'AddCardUpdate');

}



// Update 취소
AddMaster.ExpenseCancel = function () {
    // 버튼 레이어 정의 
//    $("#ExpenseUpdate").hide();
//    $("#ExpenseCancel").hide();
//    $("#ExpenseSave").show();
//    $("#ExpenseNew").show();

//    $("#ctbDocumentDateAdd").val("");
//    $("#ctbAmountAdd").val("");
//    $("#ctbGLAccountNMAdd").val("");
//    $("#ctbDueDateAdd").val("");
//    $("#ctbTaxAmountAdd").val("");
//    $("#ctbTaxNameAdd").val("");
//    ProductGroup = "";
//    ProductGroupNM = "";

}

AddMaster.SetTab = function (_section) {
    if (_section == "C") {
        // 카드 보임 , 벤더 안보임
        $('#ExpenseLayer').hide();
        $('#CardLayer').show();
        $('#CardDate').show();
        $('#CardLayerbtn').show();
        $('#CardLayerbtn1').show();

        $("#tabCard").attr("class", "tab_on");
        $("#tabVendor").attr("class", "");

        CardExpenseKind = "C";

    } else {
        // 벤더보임 , 카드 안보임
        $('#ExpenseLayer').show();
        $('#CardLayer').hide();
        $('#CardDate').hide();
        $('#CardPeriodSh').hide();
        $('#CardLayerbtn').hide();
        $('#CardLayerbtn1').hide();

        $("#tabCard").attr("class", "");
        $("#tabVendor").attr("class", "tab_on");

        CardExpenseKind = "V";

    }

    //$("#modDes").hide();
}


/**************************************************
작성자   : seo
작성목적 : 팝업레이어정의
***************************************************/
AddMaster.PopupCallVendor = function (_val) {
    ParamArray = {
        'VendorCD': VendorCD,
        'MasterDetailGubun': _val
    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopVendor.aspx?p=" + GetEncryptValue;
    PopupWin(600, 570, url, "-500", "", true);
}


// 특정 계정과목일때 호출 해주는 ProductGroup
AddMaster.PopupProductGroup = function (_val) {

    ParamArray = {
        'CardVendorKind': 'C',
        'ItemKey': _val,
        'GLAccountID': ''
    };

    CallEncrypt(CallGetParam());

    var url = "Popup/PopProductGroup.aspx?p=" + GetEncryptValue;

    objPopup = PopupModalWinOpen(url, 500, 350);

    var rtnVal = objPopup;

    //ProductGroup = rtnVal.split('|')[0];
    if (objPopup != undefined) {
        $("#ProductGroup" + rtnVal.split('|')[1]).val(rtnVal.split('|')[0]);
        $("#Etc7_" + rtnVal.split('|')[1]).val(rtnVal.split('|')[2]);
    }

}


AddMaster.PopupProductGroupEx = function () {
    ParamArray = {
        'CardVendorKind': 'V',
        'ItemKey': '',
        'GLAccountID': ''
    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopProductGroup.aspx?p=" + GetEncryptValue;

    objPopup = PopupModalWinOpen(url, 500, 350);

    //PopupWin(700, 500, url, "-500", "", true);

    if (objPopup != undefined) {
        ProductGroup = objPopup.split('|')[0];
        ProductGroupNM = objPopup.split('|')[2];
    }

}

// 카드에서 계정과목 선택시 선택바인딩
AddMaster.ProductGroupCardbind = function (_code, _name, key) {

    $("#ProductGroup" + key).text(_code);
}


AddMaster.PopupCallBankAccount = function (_val) {
    var tempVendroCD;
    if (_val == "M") {
        tempVendroCD = VendorCD;
    }
    else {
        tempVendroCD = ExVendorCD;
    }

    ParamArray = {
        'VendorCD': tempVendroCD,
        'MasterDetailGubun': _val
    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopBankAccount.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "", true);
}


AddMaster.PostingGroup = function (_val) {
    var tempVendroCD;
    if (_val == "M") {
        tempVendroCD = VendorCD;
    }
    else {
        tempVendroCD = ExVendorCD;
    }

    ParamArray = {
        'VendorCD': tempVendroCD,
        'MasterDetailGubun': _val
    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopPostingGroup.aspx?p=" + GetEncryptValue;
    PopupWin(300, 500, url, "-500", "", true);

}


AddMaster.PopupCallDepartMent = function (_val) {

    var tempVendroCD;
    if (_val == "M") {
        tempVendroCD = VendorCD;
    }
    else {
        tempVendroCD = ExVendorCD;
    }

    ParamArray = {
        'VendorCD': tempVendroCD,
        'MasterDetailGubun': _val
    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopDepartMent.aspx?p=" + GetEncryptValue;
    PopupWin(400, 500, url, "-500", "", true);

}


AddMaster.PopupCallEmployee = function (_val) {

    var tempVendroCD;
    if (_val == "M") {
        tempVendroCD = VendorCD;
    }
    else {
        tempVendroCD = ExVendorCD;
    }

    ParamArray = {
        'VendorCD': tempVendroCD,
        'MasterDetailGubun': _val
    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopEmployee.aspx?p=" + GetEncryptValue;
    PopupWin(400, 500, url, "-500", "", true);

}


// 벤더에서 G/L Account 팝업
AddMaster.PopupCallGLAccount = function (_val) {

    // Tax 가 선택 되면 자동으로 하나의 GLAccountID 만을 조회 한다.
    ParamArray = {
        'CardVendorKind' : 'V',
        'ItemKey': '',
        'GLAccountID': '',
        'ProductGroupCD': ProductGroup,
        'ProductGroupNM': ProductGroupNM
    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopGLAccount.aspx?p=" + GetEncryptValue;
    PopupWin(500, 390, url, "-500", "", true);
}


// 카드에서 G/L Account 팝업
AddMaster.PopGLAccountItem = function (_val) {
    $("#CardLayerbtn").show();
    // Tax 가 선택 되면 자동으로 하나의 GLAccountID 만을 조회 한다.
    ParamArray = {
        'CardVendorKind' : 'C',
        'ItemKey': _val,
        'GLAccountID' : '',
        'ProductGroupCD' : $("#ProductGroup" + _val).val(),
        'ProductGroupNM': $("#Etc7_" + _val).val()
    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopGLAccount.aspx?p=" + GetEncryptValue;
    PopupWin(500, 400, url, "-500", "", true);
}


// 카드에서 Tax 팝업
AddMaster.PopTaxItem = function (_val) {
    // Tax 가 선택 되면 자동으로 하나의 GLAccountID 만을 조회 한다.
    ParamArray = {
        'CardVendorKind': 'C',
        'ItemKey': _val,
        'GLAccountID': ''
    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopTax.aspx?p=" + GetEncryptValue;
    PopupWin(500, 300, url, "-500", "", true);
}


// 벤더에서 Tax 팝업
AddMaster.PopTaxVendor = function (_val) {
    // Tax 선택전 금액이 입력 되어 있어야 한다.
    if ($("#ctbAmountAdd").val() == "") {
        alert("금액을 먼저 입력해 주세요.");
        return;
    }
    // Tax 가 선택 되면 자동으로 하나의 GLAccountID 만을 조회 한다.
    ParamArray = {
        'CardVendorKind': 'V',
        'ItemKey': '',
        'GLAccountID': ''

    };

    CallEncrypt(CallGetParam());
    var url = "Popup/PopTax.aspx?p=" + GetEncryptValue;
    PopupWin(500, 300, url, "-500", "", true);
}


AddMaster.VendorBind = function (_venCD, _venNM, _postingG, _payMent) {
    VendorCD = _venCD;
    VendorNM = _venNM;
    VendorPostingGroup = _postingG;
    PaymentTerm = _payMent;
    //    DepartMentDimensionCD = _departCD;
    //    BankAccountCD = _baccCD;
    //    BankAccountNo = _baccNO;
    //    BankClearingID = _bclID;
    //    ExVendorCD = _venCD;
    //    ExVendorNM = _venNM;
    //    ExVendorPostingGroup = _postingG;
    //    ExDepartMentDimensionCD = _departCD;
    //    ExBankAccountCD = _baccCD;
    //    ExBankAccountNo = _baccNO;
    //    ExBankClearingID = _bclID;

    // Vendor 에서 가져올 값 : _venCD, _venNM, _postingG


    $("#ctbVendorCD").val(_venCD);
    $("#ctbVendorNM").val(_venNM);
    $("#ctvPostingGroup").val(_postingG);
    //    $("#ctvBankAccountCD").val(_baccCD);
    //    $("#ctbVendorCDEx").val(_venCD);
    //    $("#ctbVendorNMEx").val(_venNM);
    //    $("#ctvBankAccountCDEx").val(_baccCD);
    //    // postring Group 컨트롤 세팅

    //    $("#ctvPostingGroupAdd").val(_postingG);


    // 또한 기존 그룹 ID (바우처넘버) 를 재 생성 할 수 있도록 초기화 해준다. 
    DocumentNo = "";

    // 그외 별도로 가져올 값 : AjaxCall 이용함 _departCD, _baccCD, _baccNO, _bclID
    var etcVal = AjaxGetVendorEtc(_venCD);

    var arrEtcVal = etcVal.split('|');

    //DepartMentDimensionCD = arrEtcVal[1];
    BankAccountCD = arrEtcVal[0];
    BankAccountNo = arrEtcVal[3];
    BankClearingID = arrEtcVal[2];

    $("#ctvBankAccountCD").val(arrEtcVal[0]);
    //$("#ctvDepartMentCD").val(arrEtcVal[1]); // ctbDepartMentAddCD
    //$("#ctvDepartMent").val(arrEtcVal[1]);


    // 벤더가 바뀌게 되면 해당 벤더 정보를 가지고 기초 데이터 로딩 후 카드 데이터를 비교 및 로딩 해준다. 
    var AnotherCardExist = AjaxAnotherCardInfo(_venCD);

    if (TargetEtcCardNo.indexOf(AnotherCardExist) > -1) {
        // 다른 카드를 불러 올지 여부 판단
        // 버튼을 보여지게 하는게 아닌 해당 카드로 로딩 해준다. 

        $("#TmpData").val(AnotherCardExist);
        AddMaster.AnotherCard();

        //$('#AnotherCard').show();
    }
    else {
        $("#TmpData").val("XXXX");
        AddMaster.AnotherCard();
    }


}


AddMaster.VendorBindEx = function (_venCD, _venNM, _postingG, _payMent) {

    if (_venCD.indexOf('E') > -1) {
        // 해당 사용자의 시스템 코드를 확인 해온다.
        var exsiteSysCode = AjaxGetSysCode(_venCD);

        if (exsiteSysCode == "!") {
            alert("해당 사용자의 시스템 코드가 그룹웨어에 등록 되어 있지 않습니다.\n관리자에게 문의 해 주세요.");
            return;
        }
        ExVendorPostingGroup = "AP-EMP";
    }
    else {
        ExVendorPostingGroup = _postingG;
    }

    ExVendorCD = _venCD;
    ExVendorNM = _venNM;

    ExPaymentTerm = _payMent;
    //    ExDepartMentDimensionCD = _departCD;
    //    ExBankAccountCD = _baccCD;
    //    ExBankAccountNo = _baccNO;
    //    ExBankClearingID = _bclID;

    $("#ctbVendorCDEx").val(_venCD);
    $("#ctbVendorNMEx").val(_venNM);
    //    $("#ctvBankAccountCDEx").val(_baccCD);

    // postring Group 컨트롤 세팅
    if (_venCD.indexOf('E') > -1)
        $("#ctvPostingGroupAdd").val("AP-EMP");
    else
        $("#ctvPostingGroupAdd").val(_postingG);

    // 벤더가 바뀌게 되면 카드정보를 새로 로딩 해준다. 
    // 또한 기존 그룹 ID (바우처넘버) 를 재 생성 할 수 있도록 초기화 해준다. 
    DocumentNoEx = "";
    ProductGroup = "";
    ProductGroupNM = "";

    // 그외 별도로 가져올 값 : AjaxCall 이용함 _departCD, _baccCD, _baccNO, _bclID
    var etcVal = AjaxGetVendorEtc(_venCD);

    var arrEtcVal = etcVal.split('|');
    //ExDepartMentDimensionCD = arrEtcVal[1];
    ExBankAccountCD = arrEtcVal[0];
    ExBankAccountNo = arrEtcVal[3];
    ExBankClearingID = arrEtcVal[2];

    $("#ctvBankAccountCDEx").val(arrEtcVal[0]);
    //$("#ctvDepartMentCD").val(arrEtcVal[1]); // ctbDepartMentAddCD
    //$("#ctvDepartMent").val(arrEtcVal[1]);



}


// 벤더에서 계정과목 선택시 바인딩
AddMaster.GLAccountbind = function (_code, _name, _productCD, _productNM) {

    $("#ctbGLAccountCDAdd").val(_code);
    $("#ctbGLAccountNMAdd").val(_name);

    ProductGroup = _productCD;
    ProductGroupNM = _productNM;

    // 해당 계정 과목이 ProductGroup 필수 값인지 확인 하고 필수 값이라면 해당 레이어를 띄운다.
    if (_productCD == "") {
        if (ProductGroupStandard.indexOf(_code) > -1) {
            //alert("Product Group 을 선택 해주세요.");
            AddMaster.PopupProductGroupEx();
        }
    }
}

// 카드에서 계정과목 선택시 선택바인딩
AddMaster.GLAccountCardbind = function (_code, _name, key, _productCD, _productNM) {

    $("#cardGL" + key).text(_name);
    $("#GLAccountCardCD" + key).val(_code);

    $("#ProductGroup" + key).val(_productCD);
    $("#Etc7_" + key).val(_productNM);

    // 해당 계정 과목이 ProductGroup 필수 값인지 확인 하고 필수 값이라면 해당 레이어를 띄운다.
    if (_productCD == "") {
        if (ProductGroupStandard.indexOf(_code) > -1) {
            //alert("Product Group 을 선택 해주세요.");
            AddMaster.PopupProductGroup(key);
        }
    }

    //cardGL71346905 | 5584200007262925 | 01
}




// 벤더에서 계정과목 선택시 바인딩
AddMaster.ProductGroupbind = function (_code, _name) {

    ProductGroup = _code;

}

// 카드에서 계정과목 선택시 선택바인딩
AddMaster.ProductGroupCardbind = function (_code, _name, key) {

    $("#ProductGroup" + key).text(_code);

}



AddMaster.AmountAddKeyDown = function () {

    var _taxCD = $("#ctbTaxCodeAdd").val();

    if (_taxCD == "01") {
        // 부가세 금액 자동 계산
        AddMaster.TaxAmount();
    }

//    if (_taxCD == "05") {

//        // 부가세 카드 금액 자동 계산
//        AddMaster.TaxAmount05();
//    }

//    if (_taxCD == "06") {

//        // 부가세 카드 금액 자동 계산
//        AddMaster.TaxAmount06();
//    }

    if (_taxCD == "00") {
        // 부가세 금액 자동 계산
        AddMaster.TaxAmount00();
    }
}


// 벤더에서 Tax 선택시 바인딩
AddMaster.TaxBind = function (_taxCD, _taxName, _taxGLAccount) {

    $("#ctbTaxNameAdd").val(_taxName);
    $("#ctbTaxCodeAdd").val(_taxCD);

    if (_taxCD == "01") {
        // 부가세 금액 자동 계산
        AddMaster.TaxAmount();
    }

    if (_taxCD == "05") {

        // 부가세 카드 금액 자동 계산
        // 세액 초기화 

        AddMaster.TaxAmount05();
    }

    if (_taxCD == "06") {

        // 부가세 카드 금액 자동 계산
        // 세액 초기화

        AddMaster.TaxAmount06();
    }

    if (_taxCD == "00") {
        // 부가세 금액 자동 계산
        AddMaster.TaxAmount00();
    }

}

// 카드에서 Tax 선택시 선택바인딩
AddMaster.TaxCardBind = function (_taxCD, _taxName, _taxGLAccount, key) {


    $("#cardTax" + key).text(_taxName);
    $("#TaxCardCD" + key).val(_taxCD);

    if (_taxCD == "02") {

        // 부가세 카드 금액 자동 계산
        AddMaster.TaxAmountCard(key);
    }
}



// BankAccount Pop 에서 바인딩
AddMaster.BankAccountBind = function (_code, _bankAccountName, _bankClearingID) {

    BankAccountCD = _code;
    BankClearingID = _bankClearingID;
    BankAccountNo = _bankAccountName;

    $("#ctvBankAccountCD").val(_code);

    ExBankAccountCD = _code;
    ExBankClearingID = _bankClearingID;
    ExBankAccountNo = _bankAccountName;

    $("#ctvBankAccountCDEx").val(_code);


    // 벤더가 바뀌게 되면 카드정보를 새로 로딩 해준다. 
    // 또한 기존 그룹 ID (바우처넘버) 를 재 생성 할 수 있도록 초기화 해준다. 

}

// BankAccount Pop 에서 바인딩
AddMaster.BankAccountBindEx = function (_code, _bankAccountName, _bankClearingID) {

    ExBankAccountCD = _code;
    ExBankClearingID = _bankClearingID;
    ExBankAccountNo = _bankAccountName;

    $("#ctvBankAccountCDEx").val(_code);


    // 벤더가 바뀌게 되면 카드정보를 새로 로딩 해준다. 
    // 또한 기존 그룹 ID (바우처넘버) 를 재 생성 할 수 있도록 초기화 해준다. 

}



// BankAccount Pop 에서 바인딩
AddMaster.PostingGroupBind = function (_PostringGroup) {

    VendorPostingGroup = _PostringGroup;
    ExVendorPostingGroup = _PostringGroup;

    $("#ctvPostingGroup").val(_PostringGroup);
    //$("#ctvPostingGroupAdd").val(_PostringGroup);


}

// BankAccount Pop 에서 바인딩
AddMaster.PostingGroupBindEx = function (_PostringGroup) {

    VendorPostingGroup = _PostringGroup;
    ExVendorPostingGroup = _PostringGroup;

    $("#ctvPostingGroupAdd").val(_PostringGroup);


}



// EMP Pop에서 바인딩
AddMaster.PopEmployeeBind = function (_code, _name) {

    EmpCD = _code;

    $("#ctvEmployeeCD").val(_code);
    $("#ctvEmployee").val(_name);


}

// Emp Pop 에서 바인딩
AddMaster.PopEmployeeBindEx = function (_code, _name) {
   // ExEmpCD = _code;

   // $("#ctbEmployeeAddCD").val(_code);
   // $("#ctbEmployeeAdd").val(_code);


}


// Department Pop에서 바인딩
AddMaster.DepartMentBind = function (_code, _name) {
    DepartMentDimensionCD = _code;
    ExDepartMentDimensionCD = _code;

    $("#ctvDepartMentCD").val(_code);
    $("#ctvDepartMent").val(_name);


}

// Department Pop 에서 바인딩
AddMaster.DepartMentBindEx = function (_code, _name) {
    ExDepartMentDimensionCD = _code;

    $("#ctbDepartMentAddCD").val(_code);
    $("#ctbDepartMentAdd").val(_name);


}


// 비용 관련 전체 체크
AddMaster.ExChecked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".exCbxID").attr("checked", true);
    }
    else {
        $(".exCbxID").attr("checked", false);
    }

    // 체크된 내역에 대한 Selected Amount 를 구해준다. 

    // 체크된 항목 정보 가져오기 
    var CheckVal = AddMaster.ExpenseReportCheckedValue();
    var selAmount1 = 0;

    $.each(CheckVal, function (i, o) {
        selAmount1 += parseInt($("#CreditExLevel" + o).text().replace(/,/gi, ""));
        // 부가세가 0 이나 "" 이 아닌경우 공급가액을 세로 세팅 해준다.
    });

    $("#SelectedAmount").val(AddMaster.FormatNumber(selAmount1, 3));

}

// 카드 관련 전체 체크
AddMaster.CardChecked = function () {

    if ($("#cbxAllCard").is(":checked")) {
        $(".cardCbxID").attr("checked", true);
    }
    else {
        $(".cardCbxID").attr("checked", false);
    }
}

// 바우처 단위 체크
AddMaster.VoucherCheck = function (_checkID, _voucherNo, _seq) {

    if ($("#" + _checkID).is(":checked")) {
        // 동일 바우처 넘버에 대한 체크

        $.each($(".exCbxID"), function (i, o) {
            if ($("#VoucherNo" + o.value).text() == _voucherNo) {
                $("#Excheckbox" + o.value).attr("checked", true);
            }
        });

        // VoucherNum 전역변수를 세팅해준다. 
        //DocumentNo = _voucherNo;

    }
    else {
        $.each($(".exCbxID"), function (i, o) {
            if ($("#VoucherNo" + o.value).text() == _voucherNo) {
                $("#Excheckbox" + o.value).attr("checked", false);
            }
        });
        //DocumentNo = ""; // clear
    }

    // 체크된 내역에 대한 Selected Amount 를 구해준다. 

    // 체크된 항목 정보 가져오기 
    var CheckVal = AddMaster.ExpenseReportCheckedValue();
    var selAmount1 = 0;

    $.each(CheckVal, function (i, o) {
        selAmount1 += parseInt($("#CreditExLevel" + o).text().replace(/,/gi, ""));
        // 부가세가 0 이나 "" 이 아닌경우 공급가액을 세로 세팅 해준다.
    });

    $("#SelectedAmount").val(AddMaster.FormatNumber(selAmount1, 3));

}



// 벤더 저장
AddMaster.ExpenseSave = function () {
    // 벨리데이션 체크

    //if ($("#ctbDescription").val() == "") {
    //    alert("상단 Description 을 입력해 주세요.");
    //    return;
    //}

    if ($("#ctbVendorCDEx").val() == "") {
        alert("Vendor 를 입력해 주세요.");
        return;
    }

    if ($("#ctbDocumentDateAdd").val() == "") {
        alert("DocumentDate 를 입력해 주세요.");
        return;
    }

    if ($("#ctbDueDateAdd").val() == "") {
        alert("DueDate 를 입력해 주세요.");
        return;
    }

    if ($("#ctbAmountAdd").val() == "") {
        alert("금액을 입력해 주세요.");
        return;
    }

    if ($("#ctbGLAccountCDAdd").val() == "") {
        alert("G/L Account 를 입력해 주세요.");
        return;
    }

    if ($("#ctbDescriptionAdd").val() == "") {
        alert("비용 Description 을 입력해 주세요.");
        return;
    }

    if ($("#ctbDescription").val() == "") {
        $("#ctbDescription").val($("#ctbDescriptionAdd").val());
    }


    if ($("#ctvPostingGroupAdd").val() == "") {
        alert("Posting Group 을 입력 해 주세요.");
        return;
    }

    if (DocumentNoEx == "")
        DocumentNoEx = GetVoucherNo();


    // 체크된 항목 정보 가져오기 
    var tmpVal = "";
    var detailData = "";
    var headVal = "";
    // 체크항목 가져오기 

    var VoucherNo = DocumentNoEx;                              // 바우처넘버
    var CardExKind = "V"; // CardExpenseKind;                        // 카드벤더 구분
    var vendorCD = $("#ctbVendorCD").val();                  // 벤더코드 
    var DocumentDate = $("#ctbDocumentDateAdd").val();           // 
    var PostingDate = $("#ctbPostingDate").val();            // postingDate
    var DueDate = $("#ctbDueDateAdd").val();                // 
    var VendorDescription = $("#ctbDescription").val();      // VendorDescription  
    var iPostingGroup = $("#ctvPostingGroupAdd").val();

    // 공급가액 빠지는 부분 케어 해준다. 
    //if ($("#ctbAmountAdd1").val() == "") {
    //    $("#ctbAmountAdd1").val($("#ctbAmountAdd").val());
    //}

    // 100 byte 체크 
    var checkByte;
    checkByte = AddMaster.ByteCheck(VendorDescription);

    if (checkByte > 100) {
        alert("Vendor Description 을 100 Byte 이내로 입력해주세요.");
        return;
    }

    checkByte = AddMaster.ByteCheck($("#ctbDescriptionAdd").val());

    if (checkByte > 100) {
        alert("비용 Description 을 100 Byte 이내로 입력해주세요.");
        return;
    }

    AddMaster.SetAmount();

    headVal = VoucherNo + "|" + CardExKind + "|" + vendorCD + "|" + DocumentDate + "|" + PostingDate + "|" + DueDate + "|" + VendorDescription + "|";

    detailData = $("#ctbDescriptionAdd").val() + "|" + $("#ctbAmountAdd1").val() + "|" + $("#ctbTaxAmountAdd").val() + "|" + $("#ctbAmountAdd").val()
                + "|" + DepartMentDimensionCD + "|" + EmpCD + "|" + vendorCD + "|" + ProductGroup + "|" + DepartMentDimensionCD + "|" + EmpCD + "|" + ExVendorCD + "|" + ProductGroup
                + "|" + $("#ctbVendorNM").val() + "|" + $("#ctbVendorNMEx").val() + "|" + $("#ctbGLAccountCDAdd").val() + "|" + $("#ctbGLAccountNMAdd").val()
                + "|" + $("#ctbTaxCodeAdd").val() + "|" + $("#ctbTaxNameAdd").val() + "|" + ExBankAccountCD + "|" + ExBankAccountNo + "|" + ExBankClearingID
                + "|" + iPostingGroup + "|" + BankAccountCD + "|" + BankAccountNo + "|" + BankClearingID + "|" + "" + "|" + "" + "|" + "0" + "|" + "T"
                + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + ""
                + "|" + "" + "|" + ProductGroupNM + "|" + $("#ctbGLAccountNMAdd").val() + "|" + $("#ctvDepartMent").val() + "|" + $("#ctvEmployee").val() + "★";
    // ExDescription | 공급가액(계산된값) | 부가세값 | 합계금액(요청금액) | Dimension1Code(부서코드) | Dimension2Code(사번) | Dimension3Code(벤더) 
    //   | Dimension4Code(Product Group) | ExDimension1Code(부서코드) | ExDimension2Code(사번) | ExDimension3Code(벤더) | ExDimension4Code(Product Group)
    //  | VendorNM | ExVendorNM | GLAccount | GLAccountNM | TaxCode | TaxCodeNM | ExBankAccountCode | ExBankAccountNo | ExBankAccountCearing
    //  | ExPostingGroup | BankAcountCode | BankAccountNo | BankAcountCearingID | PONo | CardApprovalNo | DocumentID | Status 
    //  | etc1 | etc2 | etc3 | etc4 | etc5 | etc6 | etc7 | etc8 | etc9 | etc10
    tmpVal = headVal + detailData;

    $("#TmpData").val(tmpVal);


    // 보내기 submit 전에 주요데이터들에 대한 초기화 해준다.
    $("#ctbAmountAdd1").val("");
    $("#ctbTaxAmountAdd").val("");
    $("#ctbGLAccountCDAdd").val("");
    $("#ctbGLAccountNMAdd").val("");
    $("#ctbAmountAdd").val("");
    $("#ctbTaxNameAdd").val("해당없음");
    $("#ctbDescription").val("");
    $("#ctbDocumentDateAdd").val("");
    $("#ctbDueDateAdd").val("");
    $("#ctbTaxCodeAdd").val("00");
    $("#ctbDescriptionAdd").val("");


    ProductGroup = "";
    ProductGroupNM = "";

    //카드의 경우 새로 시작 되도록 만들어준다.
    DocumentNo = "";

    // Update 필드에 대한 초기화를 해준다. 
    $("#ExpenseUpdate").hide();
    //$("#modDes").hide();

    // 상단 초기화 리셋 및 저장 후 처리
    $("#ctvDepartMent").val($("#ctbDepartMentOld").val());
    $("#ctvDepartMentCD").val($("#ctbDepartMentOldCD").val());
    $("#ctvEmployee").val($("#ctbEmployeeOld").val());
    $("#ctvEmployeeCD").val($("#ctbEmployeeOldCD").val());
    DepartMentDimensionCD = $("#ctbDepartMentOldCD").val();
    EmpCD = $("#ctbEmployeeOldCD").val();

    __doPostBack('LBTNProcess', 'AddExpense');

}

AddMaster.CardSave = function () {

    // 100 byte 체크 
    var checkByte;

    // 체크된 항목 정보 가져오기 
    var CheckVal = AddMaster.CardCheckedValue();

    var CheckValidation = "";
    var CheckDes = "";
    var CheckAmountDesc = "";
    var amount1 = 0;
    var amount2 = 0;

    $.each(CheckVal, function (i, o) {

        if (CheckValidation != "N") {
            if ($("#GLAccountCardCD" + o).val() == "") {
                CheckValidation = "N";
            }
        }

        if (CheckDes != "N") {
            if ($("#DescriptionCard" + o).val() == "") {
                CheckDes = "N";
            }
        }

        checkByte = AddMaster.ByteCheck($("#DescriptionCard" + o).val());

        // 신청금액이 승인금액을 넘어서면 안된다.
        amount1 = parseInt($("#approve_AMT" + o).text().replace(/,/gi, ""));
        amount2 = parseInt($("#CardReqAMT" + o).val().replace(/,/gi, ""));

        if (CheckAmountDesc != "N") {
            if (amount1 < amount2) {
                CheckAmountDesc = "N";
            }
        }

        // 부가세가 0 이나 "" 이 아닌경우 공급가액을 세로 세팅 해준다.

    });


    if (CheckValidation == "N") {
        alert("G/L Account 를 입력해 주세요.");
        return;
    }

    if (CheckDes == "N") {
        alert("Description 을 입력해주세요");
        return;
    }

    if (CheckAmountDesc == "N") {
        alert("신청 금액은 승인금액을 초과 할 수 없습니다.");
        return;
    }

    if (checkByte > 100) {
        alert("카드 Description 을 100 Byte 이내로 입력해주세요.");
        return;
    }


    if (DocumentNo == "")
        DocumentNo = GetVoucherNo();


    var tmpVal = "";
    var detailData = "";
    var headVal = "";
    // 체크항목 가져오기 

    var VoucherNo = DocumentNo;                              // 바우처넘버
    var CardExKind = "C"; // CardExpenseKind;                        // 카드벤더 구분
    var vendorCD = $("#ctbVendorCD").val();                  // 벤더코드 
    var DocumentDate = $("#ctbPostingDate").val();           // 일단 postingDate
    var PostingDate = $("#ctbPostingDate").val();            // postingDate
    var DueDate = DueDateTemp; // $("#ctbPostingDate").val();                // postingDate
    var VendorDescription = $("#ctbDescription").val();      // VendorDescription  

    checkByte = AddMaster.ByteCheck(VendorDescription);

    if (checkByte > 100) {
        alert("Vendor Description 을 100 Byte 이내로 입력해주세요.");
        return;
    }


    if (CheckVal != "") {

        if (VendorDescription == "") {
            $.each(CheckVal, function (i, o) {
                if (VendorDescription == "") {
                    VendorDescription = $("#DescriptionCard" + o).val();
                }
            });
        }



        $.each(CheckVal, function (i, o) {
            // 공급가액 빠지는 부분 케어 해준다. 
            //if ($("#CardAMT" + o).val() == "") {
            $("#CardAMT" + o).val($("#CardReqAMT" + o).val());
            //}


            headVal = VoucherNo + "|" + CardExKind + "|" + vendorCD + "|" + $("#approveDate" + o).text() + "|" + PostingDate + "|" + DueDate + "|" + VendorDescription + "|"; // documentdate 는 승인일

            detailData = $("#DescriptionCard" + o).val() + "|" + $("#CardAMT" + o).val() + "|" + $("#CardTaxAMT" + o).val() + "|" + $("#CardReqAMT" + o).val()
                       + "|" + DepartMentDimensionCD + "|" + EmpCD + "|" + vendorCD + "|" + $("#ProductGroup" + o).val() + "|" + DepartMentDimensionCD + "|" + EmpCD + "|" + vendorCD + "|" + $("#ProductGroup" + o).val()
                       + "|" + $("#ctbVendorNM").val() + "|" + $("#ctbVendorNM").val() + "|" + $("#GLAccountCardCD" + o).val() + "|" + $("#cardGL" + o).text()
                       + "|" + $("#TaxCardCD" + o).val() + "|" + $("#cardTax" + o).text() + "|" + BankAccountCD + "|" + BankAccountNo + "|" + BankClearingID
                       + "|" + VendorPostingGroup + "|" + BankAccountCD + "|" + BankAccountNo + "|" + BankClearingID + "|" + "" + "|" + o + "|" + "0" + "|" + "T"
                       + "|" + $("#Etc1_" + o).val() + "|" + $("#Etc2_" + o).val() + "|" + $("#Etc3_" + o).val() + "|" + $("#Etc4_" + o).val() + "|" + $("#Etc5_" + o).val()
                       + "|" + $("#Etc6_" + o).val() + "|" + $("#Etc7_" + o).val() + "|" + $("#cardGL" + o).text() + "|" + $("#ctvDepartMent").val() + "|" + $("#ctvEmployee").val() + "★";
            // ExDescription | 공급가액(계산된값) | 부가세값 | 합계금액(요청금액) | Dimension1Code(부서코드) | Dimension2Code(사번) | Dimension3Code(벤더) 
            //   | Dimension4Code(Product Group) | ExDimension1Code(부서코드) | ExDimension2Code(사번) | ExDimension3Code(벤더) | ExDimension4Code(Product Group)
            //  | VendorNM | ExVendorNM | GLAccount | GLAccountNM | TaxCode | TaxCodeNM | ExBankAccountCode | ExBankAccountNo | ExBankAccountCearing
            //  | ExPostingGroup | BankAcountCode | BankAccountNo | BankAcountCearingID | PONo | CardApprovalNo | DocumentID | Status 
            //  | etc1 | etc2 | etc3 | etc4 | etc5 | etc6 | etc7 | etc8 | etc9 | etc10

            tmpVal += headVal + detailData;

        });
    }


    // 중복 입력 
    //if ($("#TmpData").val() == tmpVal) {
    //   alert("기존 자료 입력 중이오니 잠시 기다려 주십시오.");
    //    return;
    //}

    $("#TmpData").val(tmpVal);

    $("#CardLayerbtn").hide();

    // 상단 초기화 리셋 및 저장 후 처리
    $("#ctbDescription").val("");
    $("#ctvDepartMent").val($("#ctbDepartMentOldCD").val());
    $("#ctvDepartMentCD").val($("#ctbDepartMentOld").val());
    $("#ctvEmployee").val($("#ctbEmployeeOld").val());
    $("#ctvEmployeeCD").val($("#ctbEmployeeOldCD").val());
    DepartMentDimensionCD = $("#ctbDepartMentOldCD").val();
    EmpCD = $("#ctbEmployeeOldCD").val();


    __doPostBack('LBTNProcess', 'AddCard');
}


AddMaster.CardCheckedValue = function () {

    var allVals = $('.cardCbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}



// 벤더 Span 클릭시 Input 박스 로드
AddMaster.ExVendorClick = function (_seq) {

    $("#VendorSpanExVal" + _seq).hide();
    $("#VendorSpanExInput" + _seq).show();
}


// 카드 Span 클릭시 Input 박스 로드
AddMaster.CreditClick = function (_seq) {

    $("#CreditExLevel" + _seq).hide();
    $("#CreditExInput" + _seq).show();
}


// 차변2 Span 클릭시 Input 박스 로드
AddMaster.Debit2Click = function (_seq) {

    $("#Debit2ExLevel" + _seq).hide();
    $("#Debit2ExInput" + _seq).show();
}



//숫자 자릿수 콤마찍기
AddMaster.FormatNumber = function (price, num) {

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


// 부가세 카드 자동 계산
AddMaster.TaxAmountCard = function (key) {

    // 해당키의 승인금액을 가져온다. 
    var total = 0;
    var Ch1 = 0; 		//공급가액
    var Ch2 = 0; 		//부가세
    total = parseInt($("#approve_AMT" + key).text().replace(/,/gi, ""));

    if (!isNaN(total)) {
        Ch1 = total / 1.1;
        Ch2 = total - Ch1;

        Ch1 = Ch1.toFixed();
        Ch2 = Ch2.toFixed();

        $("#CardAMT" + key).val(AddMaster.FormatNumber(Ch1, 3));
        $("#CardTaxAMT" + key).val(AddMaster.FormatNumber(Ch2, 3));

    }
}


// 부가세 비용 자동 계산
AddMaster.TaxAmount = function () {

    // 해당키의 승인금액을 가져온다. 
    var total = 0;
    var Ch1 = 0; 		//공급가액
    var Ch2 = 0; 		//부가세
    total = parseInt($("#ctbAmountAdd").val().replace(/,/gi, ""));

    if (!isNaN(total)) {

        Ch1 = total / 1.1;
        Ch2 = total - Ch1;


        Ch1 = Ch1.toFixed();
        Ch2 = Ch2.toFixed();


        $("#ctbAmountAdd1").val(AddMaster.FormatNumber(Ch1, 3));
        $("#ctbTaxAmountAdd").val(AddMaster.FormatNumber(Ch2, 3));

    }
}

// 부가세 비용 자동 계산
AddMaster.TaxAmount05 = function () {

    // 해당키의 승인금액을 가져온다. 
    // 세액초기화 
    $("#ctbTaxAmountAdd").val("");
    var total = 0;
    var gyungbi = 0;
    var ghase = 0;
    var so = 0;
    var ju = 0;
    var De = 0; 		//대변
    var Ch2 = 0; 		//부가세
    total = parseInt($("#ctbAmountAdd").val().replace(/,/gi, ""));

    if (!isNaN(total)) {

        gyungbi = total * 0.8;
        ghase = total - gyungbi;
        so = ghase * 0.2;
        ju = so * 0.1;
        Ch2 = so + ju;
        De = total - Ch2;

        $("#ctbAmountAdd1").val(AddMaster.FormatNumber(total, 3));
        $("#ctbTaxAmountAdd").val(AddMaster.FormatNumber(Ch2, 3));
        $("#ctbAmountAdd").val(AddMaster.FormatNumber(De, 3));

    }
}

// 부가세 비용 자동 계산
AddMaster.TaxAmount06 = function () {

    // 해당키의 승인금액을 가져온다. 
    $("#ctbTaxAmountAdd").val("");
    var total = 0;
    var gyungbi = 0;
    var ghase = 0;
    var so = 0;
    var ju = 0;
    var De = 0; 		//대변
    var Ch2 = 0; 		//부가세
    total = parseInt($("#ctbAmountAdd").val().replace(/,/gi, ""));

    if (!isNaN(total)) {

        gyungbi = total * 0.8;
        ghase = total - gyungbi;
        so = ghase * 0.2;
        ju = so * 0.1;
        Ch2 = so + ju;
        De = total + Ch2;


        $("#ctbAmountAdd1").val(AddMaster.FormatNumber(total, 3));
        $("#ctbTaxAmountAdd").val(AddMaster.FormatNumber(Ch2, 3));
        $("#ctbAmountAdd").val(AddMaster.FormatNumber(De, 3));

    }
}



// 해당없음 일때 다시 부가세 초기화 
AddMaster.TaxAmount00 = function () {

    $("#ctbAmountAdd1").val("");
    $("#ctbTaxAmountAdd").val("");

}



// 합계과 부가세가 변경 된 금액으로 정해 지면 공급가액을 새로 정해준다.
AddMaster.SetAmount = function () {

    var total = 0;
    var Ch1 = 0; 		//공급가액
    var Ch2 = 0; 		//부가세

    total = parseInt($("#ctbAmountAdd").val().replace(/,/gi, ""));
    if ($("#ctbTaxAmountAdd").val() != "") {
        Ch2 = parseInt($("#ctbTaxAmountAdd").val().replace(/,/gi, ""));
    }

    if (!isNaN(total)) {

        Ch1 = total - Ch2;
        Ch1 = Ch1.toFixed();
        $("#ctbAmountAdd1").val(AddMaster.FormatNumber(Ch1, 3));

    }
}


AddMaster.DelItem = function (_seq, _cardseq) {

    //삭제시 documentno 초기화 
    DocumentNo = "";
    DocumentNoEx = "";
    ProductGroup = "";
    ProductGroupNM = "";
    //$("#modDes").hide();

    IsDel = "Y";

    $("#TmpData").val(_seq + "★" + _cardseq);


    __doPostBack('LBTNProcess', 'DelItem');
}




AddMaster.AllDelItem = function (_seq, _cardseq) {

    if (confirm("모든 항목을 삭제 하시겠습니까?")) {
        DocumentNo = "";
        DocumentNoEx = "";
        ProductGroup = "";
        ProductGroupNM = "";
        //$("#modDes").hide();
        __doPostBack('LBTNProcess', 'AllDelItem');
    }
}


AddMaster.ApprovalSubmit = function () {
    // 벨리데이션 체크 체크된 항목의 Vendor 가 16개까지만 됨

    //var CheckVal = AddMaster.VoucherNoColor();

    // 체크된 항목 정보 가져오기 
    var CheckVal = AddMaster.ExpenseReportCheckedValue();

    if (CheckVal != "") {

        // VendorCode 는 16가지만 허용
        var selVendor = "";
        $.each(CheckVal, function (i, o) {
            if (selVendor.indexOf($("#ExVendorNM" + o).val()) < 0) {
                selVendor += "|" + $("#ExVendorNM" + o).val();
            }
        });

        if (selVendor.split('|').length > 17) {
            alert("한번에 제출 가능한 Vendor 수를 초과 하였습니다.\n한번에 제출 가능한 Vendor 수는 16개입니다.");
            return;
        }

        ParamArray = {
            'listID': 'LEGACY',
            'nickname': 'ExpenseReport',
            'legacykey': Guids() + LegacyKey.substring(8, LegacyKey.length - 1) + Guids(), // 저장을 먼저 하고 키값을 가지고 할지 아닐지는 난중에 생각 
            'parametc': CheckVal         // parametc 우선 이걸로 넘겨준다.
        };


        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Approval/FormFlow/Default.aspx?p=" + GetEncryptValue;
        WindowObjectReference = window.open(url, "ExpensDoc_" + Guids(), "width=1300px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no")
        //PopupWin(600, 485, url, "-500", "", true);
    }


}


AddMaster.ExpenseReportCheckedValue = function () {

    var allVals = $('.exCbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


AddMaster.VoucherNoColor = function () {

    var allVals = $('.exVoucher').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


AddMaster.RowClick = function (_voucherNo, _seq, _Type, _amount) {

    if (IsDel == "Y") {
        IsDel = "";
        return;
    }

    if (_Type == "Vendor") {
        // 상단에 뿌려준다.
        $("#ctbVendorCDEx").val($("#ExDimension3Code" + _seq).text());
        $("#ctbVendorNMEx").val($("#ExVendorNM" + _seq).val());
        $("#ctbDocumentDateAdd").val($("#SpanDocumentDate" + _seq).text());
        $("#ctbDueDateAdd").val($("#SpanDueDate" + _seq).text());
        $("#ctvPostingGroupAdd").val($("#ExPostingGroup" + _seq).text());
        $("#ctbAmountAdd").val($("#CreditExLevel" + _seq).text());
        $("#ctbTaxCodeAdd").val($("#TaxCode" + _seq).text());
        $("#ctbTaxNameAdd").val($("#SpanTaxCodeNM" + _seq).text());
        $("#ctbTaxAmountAdd").val($("#Debit2ExLevel" + _seq).text());
        $("#ctbAmountAdd1").val($("#DebitAmount1" + _seq).text());
        $("#ctbGLAccountNMAdd").val($("#SpanGLAccountNM" + _seq).text());
        $("#ctbGLAccountCDAdd").val($("#GLAccount" + _seq).text());
        $("#ctvBankAccountCDEx").val($("#ExBankAccountCode" + _seq).text());
        $("#ctbDescriptionAdd").val($("#ExDescription" + _seq).text());
        $("#ctbDescription").val($("#Description" + _seq).text());

        $("#ctvDepartMent").val($("#DepartMentNM" + _seq).text());
        $("#ctvDepartMentCD").val($("#ExDimension1Code" + _seq).text());
        $("#ctvEmployee").val($("#EmpNM" + _seq).text());
        $("#ctvEmployeeCD").val($("#ExDimension2Code" + _seq).text());


        ProductGroup = $("#ExDimension4Code" + _seq).text();
        DepartMentDimensionCD = $("#ExDimension1Code" + _seq).text();
        EmpCD = $("#ExDimension2Code" + _seq).text();
        ExVendorCD = $("#ExDimension3Code" + _seq).text();
        ExVendorNM = $("#ExVendorNM" + _seq).val();
        //        ExDepartMentDimensionCD =
        //        ExVendorCD =
        ExBankAccountCD = $("#ExBankAccountCode" + _seq).text();
        ExBankAccountNo = $("#ExBankAccountNo" + _seq).text();
        ExBankClearingID = $("#ExBankClearingID" + _seq).text();
        ExVendorPostingGroup = $("#ExPostingGroup" + _seq).text();

        DocumentNoEx = _voucherNo;

        SelectedSeq = _seq;

        // 버튼 레이어 정의 
        // seo 임시 $("#ExpenseUpdate").show();
        $("#ExpenseReset").show();
        $("#ExpenseSave").hide();
        $("#ExpenseNew").show();

        // Vendor 섹션을 잡아준다.
        // 벤더보임 , 카드 안보임
        $('#ExpenseLayer').show();
        $('#CardLayer').hide();
        $('#CardDate').hide();
        $('#CardPeriodSh').hide();
        $('#CardLayerbtn').hide();
        $('#CardLayerbtn1').hide();

        $("#tabCard").attr("class", "");
        $("#tabVendor").attr("class", "tab_on");

        CardExpenseKind = "V";


    }
    else {
        // 카드 보임 , 벤더 안보임
        $('#ExpenseLayer').hide();
        $('#CardLayer').show();
        $('#CardDate').show();
        $('#CardLayerbtn').show();
        $('#CardLayerbtn1').show();

        $("#tabCard").attr("class", "tab_on");
        $("#tabVendor").attr("class", "");

        CardExpenseKind = "C";

        $("#ctvDepartMent").val($("#DepartMentNM" + _seq).text());
        $("#ctvDepartMentCD").val($("#ExDimension1Code" + _seq).text());
        $("#ctvEmployee").val($("#EmpNM" + _seq).text());
        $("#ctvEmployeeCD").val($("#ExDimension2Code" + _seq).text());

        $("#ctbDescription").val($("#Description" + _seq).text());

        DocumentNo = _voucherNo;

        SelectedSeq = _seq;
    }


    var total = 0;
    $.each($(".exCbxID"), function (i, o) {
        if ($("#VoucherNo" + o.value).text() == _voucherNo) {
            total += parseInt($("#CreditExLevel" + o.value).text().replace(/,/gi, ""));
        }
    });

    $("#DocumentAmount").val(AddMaster.FormatNumber(total, 3));

    //$("#modDes").show();


}



AddMaster.CardDate = function (e) {
    $("#CardLayerbtn").show();
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    var oItem = oContextMenuCtrl.addItem("최근승인내역(3일)", AddMaster.ViewCardDate, "");
    oItem.txt.setAttribute("arg", "3D", 0); // 

    oItem = oContextMenuCtrl.addItem("최근승인내역(1주일)", AddMaster.ViewCardDate, "");
    oItem.txt.setAttribute("arg", "7D", 0); // 

    oItem = oContextMenuCtrl.addItem("최근승인내역(1개월)", AddMaster.ViewCardDate, "");
    oItem.txt.setAttribute("arg", "1M", 0); // 

    oItem = oContextMenuCtrl.addItem("최근승인내역(3개월)", AddMaster.ViewCardDate, "");
    oItem.txt.setAttribute("arg", "3M", 0); // 

    oItem = oContextMenuCtrl.addItem("기간입력", AddMaster.ViewCardDate, "");
    oItem.txt.setAttribute("arg", "0", 0); // 

    //oContextMenuCtrl.clearItem();
    oContextMenuCtrl.show(e);
}

AddMaster.ViewCardDate = function (e) {

    var evt = new __event(e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        if (arg == "0") {
            // 직접 입력 레이어 오픈
            $("#CardPeriodSh").show();
        }
        else {
            $("#TmpData").val(arg);
            $("#CardPeriodSh").hide();
            $("#CardPerioFr").val("");
            $("#CardPerioTo").val("");
            __doPostBack('LBTNProcess', 'CardLoad');
        }
    }
}


// 카드 데이터에 대한 기간 검색
AddMaster.LoadCardPeriod = function () {

    var from = $("#CardPerioFr").val();
    var to = $("#CardPerioTo").val();

    if (from != "" && to != "") {
        $("#TmpData").val(from + "★" + to);

        __doPostBack('LBTNProcess', 'CardPeriod');
    }
}

var oContextMenuCtrl = null;
AddMaster.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");

    switch (action) {
        case "card": AddMaster.CardDate(e); break; // 조회클릭 
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }

    AddMaster.TotalAmount();
}

AddMaster.ByteCheck = function (value) {
    var strlen = 0;
    var str;
    var len = value.length;
    var i;
    for (i = 0; i < len; i++) {
        str = value.substr(i, 1);
        if (AddMaster.isKorean(str)) {
            strlen = strlen + 2;
        } else {
            strlen = strlen + 1;
        }
    }
    return strlen;
}

AddMaster.isKorean = function (value) {
    var numUnicode = value.charCodeAt(0);
    if (44031 <= numUnicode && numUnicode <= 55203 || 12593 <= numUnicode && numUnicode <= 12643) return true;
    return false;
}


AddMaster.TotalAmount = function () {

    var selAmount1 = 0;

    $.each($(".exCbxID"), function (i, o) {
    
         selAmount1 += parseInt($("#CreditExLevel" + o.value).text().replace(/,/gi, ""));
    });

    $("#TotalAmount").val(AddMaster.FormatNumber(selAmount1, 3));
}


AddMaster.ExReload = function () {

    __doPostBack('LBTNProcess', 'ExReload');
}


AddMaster.UpItem = function (_seq, _voucherNo) {
    // 현재 시퀀스와 비교해 상위 시퀀스 

    var currentSeq = _seq;
    var currentVoucher = _voucherNo;
    var beforeSeqTemp;
    var beforeVoucherTemp;
    var beforeSeq;
    var beforeVoucher;
    var countTemp = 0;
    var count = 0;

    $.each($(".exCbxID"), function (i, o) {
        countTemp = countTemp + 1;
        if (currentSeq == $("#Excheckbox" + o.value).val()) {
            beforeSeq = beforeSeqTemp;
            beforeVoucher = beforeVoucherTemp;
            count = countTemp;
        }
        else {
            beforeSeqTemp = $("#Excheckbox" + o.value).val();
            beforeVoucherTemp = $("#VoucherNo" + o.value).text();
        }
    });

    if (count < 2) {
        alert("더이상 위로 이동 할 수 없습니다.");
        return;
    }


    if (currentVoucher != beforeVoucher) {
        alert("동일한 VoucherNo 그룹만 순서 변경을 할 수 있습니다.");
        return;
    }

    // 순서변경 
    $("#TmpData").val(currentSeq + "★" + beforeVoucher);

    __doPostBack('LBTNProcess', 'ExpenseUp');

}

AddMaster.DownItem = function (_seq, _voucherNo) {

}


// 비용 voucherNo 별로 비용 분기 해준다
AddMaster.ColorLayer = function () {

//    var currentSeq = 0;
//    var currentVoucher = "";
//    var beforeSeqTemp;
//    var beforeVoucherTemp;
//    var beforeSeq;
//    var beforeVoucher;
//    var countTemp = 0;
//    var count = 0;

//    alert(1);
//    $.each($(".exVoucher"), function (i, o) {
//        //countTemp = countTemp + 1;
//        //if (currentSeq == $("#Excheckbox" + o.value).val()) {
//        //    beforeSeq = beforeSeqTemp;
//        //    beforeVoucher = beforeVoucherTemp;
//        //    count = countTemp;
//        //}
//        //else {
//        //    beforeSeqTemp = $("#Excheckbox" + o.value).val();
//        //    beforeVoucherTemp = $("#VoucherNo" + o.value).text();
//        //}
//        //alert(o.value);
//    });
}











