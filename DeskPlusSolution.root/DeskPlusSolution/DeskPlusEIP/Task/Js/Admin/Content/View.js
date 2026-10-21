View = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-22
작성목적 : 초기화
**********************************************************************************************/
$(function () {
    $(document).ready(function () {

        //파일백업
        $('#cbtnfileBackUp').click(function (e) {
            e.preventDefault();
            View.FileBackUp();
        });

        $('#cbtnfileBackUp_F').click(function (e) {
            e.preventDefault();
            View.FileBackUp();
        });

        //조회현황 클릭
        $('#cbtnReadList').click(function (e) {
            e.preventDefault();
            View.ReadList();
        });

        $('#cbtnReadList_F').click(function (e) {
            e.preventDefault();
            View.ReadList();
        });

        //전달확인 클릭
        $('#cbtnForwardList').click(function (e) {
            e.preventDefault();
            View.ForwardList();
        });

        $('#cbtnForwardList_F').click(function (e) {
            e.preventDefault();
            View.ForwardList();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        $('#cbtnPrint_F').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });


        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            if (window.confirm(R_ApplyDelete)) {
                //2번 이상 클릭 불가
                $("#cbtnDel").attr('onclick', '').unbind('click');
                __doPostBack('LBTNProcess', "DEL");
            }
        });

        $('#cbtnDel_F').click(function (e) {
            e.preventDefault();
            if (window.confirm(R_ApplyDelete)) {
                //2번 이상 클릭 불가
                $("#cbtnDel").attr('onclick', '').unbind('click');
                __doPostBack('LBTNProcess', "DEL");
            }
        });

        //리스트
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            View.GoList();
        });

        $('#cbtnList_F').click(function (e) {
            e.preventDefault();
            View.GoList();
        });

     
        //상세보기 열기
        $('#cbtnDetailOpen').click(function (e) {
            e.preventDefault();
            $(".detail").show();
            $('#cbtnDetailClose').show();
            $('#cbtnDetailOpen').hide();

            if (IsBase == "") {
                $(".detailEtc").show();
            }
        });

        //상세보기 닫기
        $('#cbtnDetailClose').click(function (e) {
            e.preventDefault();
            $(".detail").hide();
            $(".detailEtc").hide();
            $('#cbtnDetailClose').hide();
            $('#cbtnDetailOpen').show();

        });      

        //맨밑으로
        $('#cbtnDown').click(function (e) {
            var pos = document.body.scrollHeight;
            $("html, body").animate({ scrollTop: pos }, 100);

        });


        $('#cbtnDetailOpen').hide();

    });
});

/**************************************************
작성자   : famo23
작성일자 : 2015-04-22
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-22
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
}


/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 파일 백업
***************************************************/
View.FileBackUp = function () {

    ParamArray = {
        'listID': ListID
    };
    CallEncrypt(CallGetParam());

    $("#ifrm")[0].src = "/DeskPlusEIP/Task/Common/CreateZipFile.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 조회 현황
***************************************************/
View.ReadList = function () {

    ParamArray = {
        'listID': ListID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Common/ReadList.aspx?p=" + GetEncryptValue;
    PopupWin(500, 390, url, "", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 전달 현황
***************************************************/
View.ForwardList = function () {

    ParamArray = {
        'listID': ListID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Common/ForwardList.aspx?p=" + GetEncryptValue;
    PopupWin(800, 390, url, "", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 목록가기
***************************************************/
View.GoList = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isGubun': IsGubun
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}



