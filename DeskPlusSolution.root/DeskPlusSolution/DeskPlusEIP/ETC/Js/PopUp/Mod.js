Mod = {}

/**************************************************
작성자   : shine
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //게시물 수정
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Mod.Write();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Mod.Cancel();
        });

        // 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Mod.Del();
        })

        //해상도 라디오버튼 클릭
        $("input[type=radio][name=rdopixel]").click(function () {
            Mod.SelectPixel();
        });

        if ($('#ctbWidth').val() == "400") {
            $("input:radio[name='rdopixel']:radio[value='400']").attr("checked", true)
        }
        else if ($('#ctbWidth').val() == "520") {
            $("input:radio[name='rdopixel']:radio[value='520']").attr("checked", true)
        }
        else if ($('#ctbWidth').val() == "800") {
            $("input:radio[name='rdopixel']:radio[value='800']").attr("checked", true)
        }

        xfe.setHtmlValue($("#tbHiddenMime_contents").val());
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "Mod.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "ModN.aspx?" + ClientQueryString;
                }
            });
        }
        */

        $('#startDate').focus(function (e) {
            $('#tdDate').css('height', '220px');

        });

        $('#endDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        $(".clsDate").keydown(function (e) { return false; });

        // 숫자만 넣기
        $('.num_only').css('imeMode', 'disabled').keypress(function (event) {
            if (event.which && (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }

        }).keyup(function () {
            if ($(this).val() != null && $(this).val() != '') {
                $(this).val($(this).val().replace(/[^0-9]/g, ''));
            }
        });

    });
});

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 게시물 수정
***************************************************/
Mod.Write = function () {
    //에디터 내용 히든박스에 저장
    //ByFreeman 20120106 NonActiveX Test
    if (NonActiveX == "T") {
        $("#tbHiddenHTML_contents").val(xfe.getHtmlValue());
        $("#tbHiddenMime_contents").val("");
        $("#tbHiddenText_contents").val(xfe.getTextValue());
    } else {
        if (!EditorSecurity(form1.twe)) return;

        var strHtml = form1.twe.HtmlValue;
        if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
            strHtml = strHtml.replace(".js\"", "\"");
            strHtml = strHtml.replace('=\"xss:expression', "");

            form1.twe.HtmlValue = strHtml;

            alert(R_DoubtStcipt);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
        $("#tbHiddenText_contents").val(form1.twe.TextValue);
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_Add_InputTitle, true);
    if (!validator.validate()) return;

    var getStartDate = $("#startDate").val();
    var getEndDate = $("#endDate").val();

    if (getStartDate == "" || getEndDate == "") {
        alert(R_ApplyRegDate);
        return false;
    }
    else {

        var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

        if (dtResponse == false) {
            alert(R_StartDateEndDate);
            return false;
        }
    }

    var contents = $("#tbHiddenText_contents").val();
    var contentHtml = $("#tbHiddenHTML_contents").val();
    if (contentHtml.indexOf("<IMG") > -1) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert(R_Add_InputContent);
        return;
    }

    if (TrySaveCnt == 0) {
        __doPostBack('lbtnWrite', 'Modify');
        TrySaveCnt += 1;
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 게시물 수정 취소시 
***************************************************/
Mod.Cancel = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 게시물 삭제 
***************************************************/
Mod.Del = function () {

    if (window.confirm(R_ApplyDelete)) {
        __doPostBack("lbtnWrite", "Del");
    }
}

//사용자 해상도 선택시
Mod.SelectPixel = function () {
    var checkValue = $("input[type=radio][name=rdopixel]:checked").val();
    if (checkValue == "400") {
        $("#ctbWidth").val("400");
        $("#ctbHeight").val("600");
    }
    else if (checkValue == "520") {
        $("#ctbWidth").val("520");
        $("#ctbHeight").val("600");
    }
    else if (checkValue == "800") {
        $("#ctbWidth").val("800");
        $("#ctbHeight").val("600");
    }
};