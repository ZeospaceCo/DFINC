var SendFax = {}


/**************************************************
초기화
***************************************************/
$(function () {

    $(document).ready(function () {

        //주소록 추가
        $('#cbtnAddress').click(function (e) {
            e.preventDefault();
            SendFax.CallAddress();
        });

        //추가하기
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            SendFax.AddUser();
        });

        //팩스 전송
        $('#cbtnSendFax').click(function (e) {
            e.preventDefault();
            SendFax.SendFax();
        });


        $("#cbtnSubject").focus();
        $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '370px', 'height': '370px' });

    });
});




/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 팩스 발송
***************************************************/
SendFax.SendFax = function () {

    var faxUser = "";

    if (AuthArray.length > 0) {

        for (var i = 0; i < AuthArray.length; i++) {
            var comName = AuthArray[i].compName;
            var userName = AuthArray[i].userName;
            var faxNum = AuthArray[i].faxNum;

            faxUser += comName + "^" + userName + "^" + faxNum + "$";
        }

        $("#hdnFaxUser").val(faxUser);
    }

    if (faxUser != "") {
        if (window.confirm(R_FaxIsSendFaxRecv)) {
            __doPostBack('lbtnRegister', '');
        }
    }
    else {
        alert(R_FaxInputFaxRecvUser);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 주소록 호출
***************************************************/
SendFax.CallAddress = function () {
    var url = "/DeskPlusEIP/Common/WebFax/AddressBook.aspx";
    PopupWin(700, 590, url, "-500", "", true);
}

var AuthArray = []; //db 에서 권한 값을 권한배열에 담는다.
var CompareArray = []; //넘어온 권한값 배열에 할당하기(G$1$영업부,영업부)
var DelArray = []; //삭제할 값 배열에 담기.

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 입력 추가
***************************************************/
SendFax.AddUser = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbFaxNumber", R_FaxInputFaxNumber, true);
    if (!validator.validate()) return;

    var comName = $("#ctbCompanyName").val();
    var userName = $("#ctbName").val();
    var faxNum = $("#ctbFaxNumber").val();

    SendFax.CompareArrayVal(faxNum, comName, userName);
}


//==================================================================    
// 팩스번호 가져오기 //이름|팩스$이름|팩스$
//==================================================================
function GetFaxUser(p_fax) {

    var rtnVal = p_fax;

    if (rtnVal) {
        if (rtnVal.indexOf("$") > -1) {
            var wholeSplit = rtnVal.split("$");
            for (var i = 0; i < wholeSplit.length; i++) {
                //이름|팩스
                var valueSplit = wholeSplit[i].split("|");
                var userName = valueSplit[0]; //이름
                var faxNumber = valueSplit[1]; //팩스번호

                if (faxNumber != "" && faxNumber != undefined) SendFax.CompareArrayVal(faxNumber, userName, userName);
            }
        }
    }
}




/**************************************************
작성자   : shimpark
작성목적 : 권한 대상자 할당하기
***************************************************/
SendFax.AddAuthArray = function (faxnum, compName, userName) {

    AuthArray.push({
        'faxNum': faxnum,
        'compName': escape(compName),
        'userName': escape(userName)
    });

}


/**************************************************
작성자   : shimpark
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
SendFax.DelAuthRow = function (index, faxnum) {

    var row = $("#boardAuth" + index);

    for (var i = 0; i < AuthArray.length; i++) {

        if (AuthArray[i].faxNum == faxnum) {

            AuthArray.splice(i, 1);

            row.fadeOut("slow", function () {
                row.remove();
            });

            break;
        }
    }
}


/**************************************************
작성자   : shimpark
작성목적 : 비교 처리하며, 권한 할당
***************************************************/
SendFax.CompareArrayVal = function (faxnum, compName, userName) {

    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (AuthArray.length > 0) {
        for (var i = 0; i < AuthArray.length; i++) {

            if (AuthArray[i].faxNum == faxnum) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우                
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            SendFax.AddAuthArray(faxnum, compName, userName);
            SendFax.AuthRowHtml(faxnum, compName, userName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        SendFax.AddAuthArray(faxnum, compName, userName);
        SendFax.AuthRowHtml(faxnum, compName, userName);
    }

    return response;
}



/**************************************************
작성자   : shimpark
작성목적 : 추가 table row 생성하고 권한값 AuthArray[] 배열에 할당하기
***************************************************/
SendFax.AuthRowHtml = function (faxnum, compName, userName) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    $("#contentTable").append(
		"<tr id='boardAuth" + RowMax + "'>"
			+ "<td class='center'>"
			+ unescape(compName)
			+ "</td>"
            + "<td class='center'>"
			+ unescape(userName)
			+ "</td>"
            + "<td class='center'>"
			+ faxnum
			+ "</td>"
			+ "<td><a onclick=\"SendFax.DelAuthRow(" + RowMax + ",'" + faxnum + "')\"><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
		+ "</tr>"
	);
}

