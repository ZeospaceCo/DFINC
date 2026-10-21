Print = {}

$(function () {
    $(document).ready(function () {

        // 미리보기
        $('#cbtnPreview').click(function (e) {
            e.preventDefault();
            Print.Preview();
        });

        // 인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            Print.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        // 목록
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            var url = "../Default.aspx";
            parent.location.href = url;
        });

        // 인쇄항목 모두 선택
        $("#cbxAll").attr("checked", "checked");
        $(".cbxID").attr("checked", "checked");


        //본문내용값 저장
        Contents = $('#plContent').html();
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
Print.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();        
    }).get();

    return allVals;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 전체 체크박스
***************************************************/
Print.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 인쇄미리보기
***************************************************/
Print.Preview = function () {
    var repeat = $('#ddlRepeat').val(); // 반복수 
    var fontSize = $('#ddlFontSize').val();  // 폰트사이즈
    var printList = "";
    $('#plContent').html("");           // 초기화   
    $('#plContent').html(Contents);     // 현재 그려진 리스트 대신 로딩 시 저장한 html로 다시 세팅
    var pList = $('#plContent').html();
    var selVal = Print.CheckedValue();  // 선택한 인쇄 항목    

    // 처음에 모든 항목 안보이게 한 후 선택한 인쇄 항목만 보이게 한다.
    if (selVal.length != 0) {
        $('.All').hide();
        for (var i = 0; i < selVal.length; i++) {
            $("." + selVal[i]).show();
        }
    }

    pList = $('#plContent').html();

    // 반복 한 수 만큼 테이블 그리기
    for (var i = 0; i < repeat; i++) {
        printList += pList;
    }

    $('#plContent').html(printList);

    // 폰트 사이즈 적용 
    $('.All').css('fontSize', fontSize);        // 헤더
    $('#toPrint').css('fontSize', fontSize);    // 본문   
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 인쇄
***************************************************/
Print.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}