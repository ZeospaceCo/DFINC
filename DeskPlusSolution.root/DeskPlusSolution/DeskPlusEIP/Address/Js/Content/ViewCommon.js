ViewCommon = {}

$(function () {
    $(document).ready(function () {

        $('input').placeholder(); //watertextbox        

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            ViewCommon.Confirm();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            ViewCommon.Cancel();
        });     

        if (PhotoName != "") {
            $("#photoImg")[0].innerHTML = "<img src='" + PhotoName + "' width='52' height='52' alt='' title='' />";
        }

        $('#ctbEnglishName').attr("disabled", true);
        $('#ctbCompanyMulti').attr("disabled", true);
        $('#ctbDepartmentMulti').attr("disabled", true);
        $('#ctbTitleMulti').attr("disabled", true);
    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 주소 추가 취소 
***************************************************/
ViewCommon.Cancel = function () {    
    ParamArray = {
        'categoryID': CatID,
        'types': Types,
        'rTypes': RTypes,
        'currentPage': CurrentPage,
        'searchOption': SearchOption,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'searchFilter': SearchFilter,
        'order': Order
    };

    var referurl = document.referrer;

    if (RTypes == "IG") {
        if (referurl.indexOf("ListBlogImpo.aspx") != -1) {
            GoPage = "/DeskPlusEIP/Address/Content/ListBlogImpo.aspx";
        }
        else if (referurl.indexOf("ListImpo.aspx") != -1) {
            GoPage = "/DeskPlusEIP/Address/Content/ListImpo.aspx";
        }
    }
    else {
        if (referurl.indexOf("ListBlog.aspx") != -1) {
            GoPage = "/DeskPlusEIP/Address/Content/ListBlog.aspx";
        }
        else if (referurl.indexOf("List.aspx") != -1) {
            GoPage = "/DeskPlusEIP/Address/Content/List.aspx";
        }
    }

    CallEncrypt(CallGetParam());
    location.href = GoPage + "?p=" + GetEncryptValue;
}

