AddSimple = {}

$(function () {
    $(document).ready(function () {

        $('#txtDate').hide();

        //상세
        $('#CbtnMod').click(function (e) {
            e.preventDefault();
            if (UserID == CurrentUserID)
                AddSimple.ViewPage(); //AddSimple.Mod();
            else
                AddSimple.ViewPage();
        });

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            if (UserID == CurrentUserID)
                AddSimple.SetPara();
            else
                if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
            //parent.ModalHide();
        });

        $("#txtTitle").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                if (UserID == CurrentUserID)
                    AddSimple.SetPara();
                else
                    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
            }
        });


        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();

            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        ///ddlDate
        //일정선택
        $("#ddlDate").change(function (e) {
            e.preventDefault();
            AddSimple.DateSetting();
        });

        $("#txtDate").datepick($.datepicker.regional[PreLang]);
    });
});


/********************************
작성자:김광일
작성일: 
용도:일정정하기
********************************/
AddSimple.DateSetting = function () {

    var scope = $("#ddlDate").val();

    if (scope == 0) {           //선택안함
        $('#txtDate').hide();
        self.resizeTo(450, 230);
    }
    else if (scope == 1) {  //내일할일
        $('#txtDate').hide();
        self.resizeTo(450, 230);
    }
    else if (scope == 2) {  //특정일까지 할일
        $('#txtDate').show();
        self.resizeTo(450, 300);
        
    }
    else if (scope == 3) { //특정일에할일
        $('#txtDate').show();
        self.resizeTo(450, 230);
        
    }
    else if (scope == 4) {  //내일로연기     
        $('#txtDate').hide();
        self.resizeTo(450, 230);
    }
    else if (scope == 5) {  // 특정일로연기     
        $('#txtDate').show();
        self.resizeTo(450, 300);
    }
}



/********************************
    작성자:김광일
    작성일: 2011-9-6
    용도:상세페이지이동
********************************/
AddSimple.Confirm = function () {

    if ($("#txtTitle").val() == "") {    
        alert(Add_InputTitle);
        return;
    }

    var _scope = $("#ddlDate").val();
    var chk = 0;
    if (_scope == 2) {  //특정일까지 할일
        chk = 1;
    }
    else if (_scope == 3) { //특정일에할일
        chk = 1;
    }
    else if (_scope == 4) {  //내일로연기     
        chk = 0;
    }
    else if (_scope == 5) {  //특정일로연기     
        chk = 1;
    }

    if (chk != 0) {
        if ($("#txtDate").val() == "") {
            alert(Add_InputDate);
            return;
        }
    }
    
    __doPostBack('lbtnWrite', "");
}


/********************************
    작성자:김광일
    작성일: 2011-9-6
    용도:상세페이지이동
********************************/
AddSimple.Mod = function () {

    parent.ListDay.MoveMod(TodoID);
    
}

/********************************
작성자:김광일
작성일: 2011-9-6
용도:파라미터세팅
********************************/
AddSimple.SetPara = function () {

    var validator = new Validator($("#form1"));
    //제목    
    validator.checkRequired("txtTitle", Add_InputTitle, true);
    if (!validator.validate()) return;
    
    SCOPE = $("#ddlDate").val();

    var chk = 0;


    if (SCOPE == 2) {  //특정일까지 할일
        chk = 1;
    }
    else if (SCOPE == 3) { //특정일에할일
        chk = 1;
    }
    else if (SCOPE == 4) {  //내일로연기     
        chk = 0;
    }
    else if (SCOPE == 5) {  //특정일로연기     
        chk = 1;
    }

    if (chk != 0) {
        if ($("#txtDate").val() == "") {
            alert(Add_InputDate);
            return;
        }
    }

    Title = $("#txtTitle").val();
    DateInfo = $("#txtDate").val();
    Priority = $("#ddltodo").val();
    Status = $("#ddlPriorty").val();
    t_Opentype = $("#ddlShare").val();

    AddSimple.ModTodo();
}

/********************************
작성자:김광일
작성일: 2011-9-6
용도:수정
********************************/
AddSimple.ModTodo = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Todo.asmx/ModTodo";
    Title = replaceAll(Title, "+", "＋");
    ParamArray = {
        'CompnayID': CompnayID,
        'TodoID': TodoID,
        'Title': Title.replace(/&/g, '＆'),
        'SCOPE': SCOPE,
        'DateInfo': DateInfo,
        'Priority': Priority,
        'Status': Status,
        'OpenType': t_Opentype
    };
    

    var obj = eval("AddSimple.SuccessMod");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

function replaceAll(str, searchStr, replaceStr) {
    return str.split(searchStr).join(replaceStr);
}

AddSimple.SuccessMod = function (data, status) {
    parent.ListDay.GetTodoList();
    
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}



AddSimple.ViewPage = function () {
    parent.ListDay.MoveTodoView(TodoID);
}