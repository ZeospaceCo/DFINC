List = {}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //List.LoadData("");
            List.GoPageLoad();
        }
    }

    $(document).ready(function () {

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });

        //회사 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //라이센스 관리
        $('#cbtnLic').click(function (e) {
            e.preventDefault();
            List.License();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.PageSize();
        });

        //활성,비활성화
        $('#cbtnActive').click(function (e) {
            e.preventDefault();
            List.Active();
        });

        //라이센스 저장
        $('#cbtnModLicense').click(function (e) {
            e.preventDefault();
            List.ModLicense();
        });

        
        

        List.GetLicenseCount();
        List.LoadData("");
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
List.PageSize = function () {
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');   

    ParamArray = {
        'companyID': 0,
        'recordPerPage': $('#ddlListCount').val(),        
        'order': Order
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = iPath + "?p=" + GetEncryptValue;
}



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadAccountData = function () {
        List.GetPageList();

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadData("");
            }
        }

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });
    }

    //var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    //$('#loading').html(loadingPath);

    setTimeout(function () {
        loadAccountData();
    }, 100);

    
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 회사 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Company/List.asmx/GetCompanyPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'activeCompany': '0',
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 회사 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Company/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);    

    List.Resource();    
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

List.Resource = function () {
    $("#r_companyname")[0].innerHTML = R_CompanyName;
    $("#r_ownername")[0].innerHTML = R_DelegatorName;
    $("#r_phone")[0].innerHTML = R_Telephone;
    $("#r_fax")[0].innerHTML = R_Fax;
    $("#r_userCount")[0].innerHTML = R_UserCount;
    $("#r_active")[0].innerHTML = R_Active;
    $("#r_licensecnt")[0].innerHTML = R_LicenseCount;
    

    $('.r_activeYes').text(R_Yes);
    $('.r_activeNo').text(R_No);
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-24
작성목적 : 라이센스 개수
**********************************************************************************************/
List.GetLicenseCount = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Company/List.asmx/GetLicenseCount";
    var obj = eval("List.SuccessGetLicenseCount");
    AjaxService(url, "", obj);
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 성공시  라이센스 개수
**********************************************************************************************/
List.SuccessGetLicenseCount = function (data, status) {    
    $("#licenseCnt").text(data.d);
}




/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 정렬
**********************************************************************************************/
List.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;    

    //List.LoadData("");
    List.GoPageLoad();
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 추가
**********************************************************************************************/
List.Add = function () {
    ParamArray = {
        'companyID': '0',
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());

    location = "Add.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 수정
**********************************************************************************************/
List.Mod = function (params) {
    ParamArray = {
        'companyID': params,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());
        
    location = "Add.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-04-14
작성목적 : 라이센스
**********************************************************************************************/
List.License = function () {
    var url = "../License/List.aspx";
    document.location.href = url;
}



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 전체 체크박스
***************************************************/
List.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}



/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 회사 활성화 및 비활성화
***************************************************/
List.Active = function () {

    var selUserList = List.CheckedValue();

    if (selUserList != "") {

        ParamArray = {
            'comList': selUserList
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Company/List.asmx/ModCompanyActive";
        var obj = eval("List.SuccessActive");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    }
    else {
        alert(R_SearchRequestSelect);
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공 시, 회사 활성화 및 비활성화
**********************************************************************************************/
List.SuccessActive = function (data, status) {
    if (data.d == true) {
        List.LoadData("");
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
List.GetLicenseValue = function () {
    var allVals = $('.cbxLicenseCnt').map(function () {
        return this.id.replace("License", "") +"_" + $(this).val();
    }).get();

    return allVals;
}


/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 회사별 라이센스 카운트 저장
***************************************************/
List.ModLicense = function () {

    var alList = List.GetLicenseValue();    
    if (alList != "") {

        ParamArray = {
            'licenseList': alList
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/EIP/Ws/Admin/Company/List.asmx/ModLicense";
        var obj = eval("List.SuccessModLicense");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    }
    else {
        alert(R_SearchRequestSelect);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공 시, 회사별 라이센스 카운트 저장
**********************************************************************************************/
List.SuccessModLicense = function (data, status) {
    
    if (data.d >= 0) {
        alert(R_SUCCESS);
        List.LoadData("");
    }
    else if (data.d == -1) {
        alert(R_ExcessLicenseCnt);
        List.LoadData("");
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'companyID': '0',
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}


