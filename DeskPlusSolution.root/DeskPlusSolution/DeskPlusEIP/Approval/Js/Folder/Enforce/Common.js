Common = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
    });
});


Common.InitEnforce = function () {

    ifrmObj = ifrmForm.document;

    Common.SetObj("EXPFIRST", $("#tbExpFirst").val());  //상단내용-EXPFIRST
    Common.SetObj("EXPMID", $("#tbExpMid").val());      //하단내용-EXPMID
    Common.SetObj("EXPEND", $("#tbExpEnd").val());      //하단내용-EXPEND
    Common.SetObj("ZIPCODE", $("#tbZipCode").val());    //우편번호-ZIPCODE
    Common.SetObj("ADDRESSFIRST", $("#tbAddressFirst").val());  //주소1-ADDRESSFIRST
    Common.SetObj("ADDRESSEND", $("#tbAddressEnd").val());      //주소2-ADDRESSEND
    Common.SetObj("PHONE", $("#tbPhone").val());                //전화-PHONE
    Common.SetObj("FAX", $("#tbFax").val());                    //팩스-FAX
    Common.SetObj("HOMEPAGE", $("#tbHomepage").val());          //Homepage-HOMEPAGE
    Common.SetObj("DOCNUMBER", $("#tbDocNumber").val());        //문서번호-DOCNUMBER
    Common.SetObj("ENFORCEDATE", $("#tbEnforceDate").val());    //시행일자-ENFORCEDATE
    Common.SetObj("WRITEDATE", $("#tbWriteTime").val());        //상신일자-WRITEDATE
    Common.SetObj("COMPLETEDATE", $("#tbCompleteDate").val());  //완결일자-COMPLETEDATE
    Common.SetObj("RECEIVE", $("#tbReceive").val());            //수신처-RECEIVE
    Common.SetObj("REFERENCE", $("#tbReference").val());        //참조처-REFERENCE
    Common.SetObj("SUBJECT", $("#tbSubject").val());            //제목-SUBJECT
    Common.SetObjHTML("CONTENT", $("#htaEditorHTML").val());    //내용-CONTENT

    $("#DIVEXP", ifrmObj).attr("style", "position:relative;z-index:100;");
    Common.SetStamp(false);  //직인처리
}

Common.SetObj = function (_id, _val) {
    if (_id == "" || _val == "") return;

    if ($("#" + _id, ifrmObj).length > 0) {
        $("#" + _id, ifrmObj).text(_val);
    }
}
Common.SetObjHTML = function (_id, _val) {
    if (_id == "" || _val == "") return;
    
    if ($("#" + _id, ifrmObj).length > 0) {
        $("#" + _id, ifrmObj).html(_val);
    }
}

/**************************************************
작성목적 : 직인처리
***************************************************/
Common.SetStamp = function (_isview) {

    var stamp = $("#DIVSTAMP", ifrmObj);
    var last = $("#EXPEND", ifrmObj);

    if (StampPath == "") return;
    if (stamp.length == 0 || last.length == 0) return;

    var sobj = "<img src='" + StampPath + "' style='width:60px;height:60px'>";
    stamp.html(sobj);

    var objh = (stamp.height() / 3) - (last.height / 2);
    var objw = last.width();

    var objv = (isview) ? "" : "hidden";

    var sval = "position:relative;opacity: 0.5; filter: alpha(opacity=50);top:" + objh + "px;right:" + objw + "px;visibility:" + objv + ";";
    stamp.attr("style", sval);
}