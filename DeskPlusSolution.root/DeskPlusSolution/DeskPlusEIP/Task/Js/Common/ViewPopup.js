View = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 초기화
**********************************************************************************************/
$(function () {
    $(document).ready(function () {

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        $('#cbtnPrint_F').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //메인 페이지 이동
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            if (window.opener != null) {
                window.opener.parent.frmTop.Menu.GoParam("TASK");
                window.opener.location.href = "/DeskPlusEIP/Task/Default.aspx";
                self.close();
            }
            else {
                location = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=TASK";
            }
        });

        $('#cbtnList_F').click(function (e) {
            e.preventDefault();
            if (window.opener != null) {
                window.opener.parent.frmTop.Menu.GoParam("TASK");
                window.opener.location.href = "/DeskPlusEIP/Task/Default.aspx";
                self.close();
            }
            else {
                location = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=TASK";
            }
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
작성일자 : 2015-04-27
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
}



