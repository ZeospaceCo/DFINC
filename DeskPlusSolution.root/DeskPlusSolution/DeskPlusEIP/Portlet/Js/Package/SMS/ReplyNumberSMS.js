
$(document).ready(function () {
    $("#Req1").focus();

    //닫기
    $("#CbtnClose").click(function (e) {
        e.preventDefault();
        self.close();
    });
});


//함수명	: onlyNum
//설명		: 숫자만 입력 가능 (0~9)
//ex		: ONKEYPRESS="onlyNum();"
function onlyNum(){
	var keyCode = event.keyCode ? event.keyCode : 
	event.which ? event.which : event.charCode;
	if((keyCode>47 && keyCode<58)) {
		event.returnValue = true;
		} else {
		event.returnValue = false;
		}
}


//함수명	: autoTab
//설명		: 자동 포커스 이동
//ex		: ONKEYUP="autoTab(this,nextobj,size);"
function autoTab(obj1,obj2,ss){
	if (obj1.value.trim().length>=parseInt(ss,10)) {
		obj2.focus();
	}
}

String.prototype.trim = function()
{
	return this.replace(/(^\s*)|(\s*$)/g, "");
}

function member_ReqCHK_enter(aaa){
	if (aaa=='13') {
		member_ReqCHK();
	}
}

//회신번호추가
function member_ReqCHK() {

	var req1 = $("#Req1").val();
	var req2 = $("#Req2").val();
	var req3 = $("#Req3").val();	    
	    
	if (req1 == '') {
	    alert(InputReplyNumber);
	    $("#Req1").focus();
	    return;
	} 
        
    if (req2 == '') {
	    alert(InputReplyNumber);
	    $("#Req2").focus();
	    return;
	}

	if (req3 == '') {
	    alert(InputReplyNumber);
	    $("#Req3").focus();
	    return;
	}

	AddReply(req1, req2, req3);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 회람글 추가하기
**********************************************************************************************/
function AddReply(p_req1, p_req2, p_req3) {

	ParamArray = {
	    'req1': p_req1,
	    'req2': p_req2,
	    'req3': p_req3
	};

	CallEncrypt(CallGetParam())

	var data = "{p:'" + GetEncryptValue + "'}";
	var obj = eval("SuccessAjaxAddReply");
	var url = "/DeskPlusEIP/Portlet/Ws/Package/SMS/ReplyNumberSMS.asmx/Add";

	AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 회람글 추가하기
**********************************************************************************************/
function SuccessAjaxAddReply(data, status) {

	alert("처리 되었습니다.");

	var response = data.d;

	var req1 = $("#Req1").val();
	var req2 = $("#Req2").val();
	var req3 = $("#Req3").val();

	var req = req1 + '-' + req2 + '-' + req3;	    

	document.getElementById("ReqList").options[document.getElementById("ReqList").options.length] = new Option(req, req + "," + response);
	$("#Req1").val("");
	$("#Req2").val("");
	$("#Req3").val("");
	$("#Req1").focus();
}


//기본값 설정
function member_Req_Set() {
    var f = document.Frm;
    var idx = "";
    var ReqNum = "";

    if (document.getElementById("ReqList").options.length == 0) {
        alert(AddReplyPhoneNumber);
    } else {
        if (document.getElementById("ReqList").options.selectedIndex == -1) {
            idx = document.getElementById("ReqList").options.length - 1;
            ReqNum = document.getElementById("ReqList").options[idx].value;
            document.getElementById("ReqList").options[idx].selected = true;
        } else {
            idx = document.getElementById("ReqList").selectedIndex;
            ReqNum = document.getElementById("ReqList").options[idx].value;
        }

        var reqNumStr = ReqNum.split(",");
        ModSettingDefault(reqNumStr[1],reqNumStr[0]);
            
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 회람글 추가하기
**********************************************************************************************/
function ModSettingDefault(p_id, p_phoneNumber) {
        
    ParamArray = {
        'id': p_id,
        'phoneNumber': p_phoneNumber
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("SuccessModSettingDefault");
    var url = "/DeskPlusEIP/Portlet/Ws/Package/SMS/ReplyNumberSMS.asmx/ModBase";

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 회람글 추가하기
**********************************************************************************************/
function SuccessModSettingDefault(data, status) {

    alert("처리 되었습니다.");

    var response = data.d;

    //document.getElementById.span_req.innerHTML = DefaultReplyPhoneNumber + " : <font color=blue>" + reqNumStr[0] + "</font>";
}


//내 정보수정에서 회신번호삭제
function DelReply() {

    var Selindex;
    var Selval;

    if (document.getElementById("ReqList").options.length == 0) {
        alert(NoDelPhoneNumber);
        return;
    }

    if (document.getElementById("ReqList").selectedIndex == -1) {
        //첫번째부터 삭제한다.
        Selindex = document.getElementById("ReqList").options.length - 1;
        //선택번호를 리스트에서 삭제한다.
    }
    else {
        Selindex = document.all("ReqList").selectedIndex;
    }

        

    //선택번호를 리스트에서 삭제한다.
    Selval = document.getElementById("ReqList").options[Selindex].value;


    var selValStr = Selval.split(",");


    ParamArray = {
        'id': selValStr[1]
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("SuccessDelReply");
    var url = "/DeskPlusEIP/Portlet/Ws/Package/SMS/ReplyNumberSMS.asmx/Del";

    AjaxService(url, data, obj);

    document.getElementById("ReqList").options[Selindex] = null;    
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 회람글 추가하기
**********************************************************************************************/
function SuccessDelReply(data, status) {

    if (data.d == true) {
        alert("처리 되었습니다.");
    }
    
}