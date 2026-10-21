Setting = {}

$(function () {
    $(document).ready(function () {
        
        //전체선택
        $('#cbtnAll').click(function (e) {
            e.preventDefault();
            //$('#cbtnAll').hide();
            //$('#cbtnUnAll').show();
            Setting.Checked();
        });

        //전체해제
        $('#cbtnUnAll').click(function (e) {
            e.preventDefault();
            //$('#cbtnUnAll').hide();
            //$('#cbtnAll').show();            
            Setting.UnChecked();
        });

        //협업종류 선택하기
        $('#ddlSetting').change(function () {
            Setting.GetIsTypeList();
        });

        $('#cbtnOk').click(function (e) {
            e.preventDefault();
            Setting.Add();
        })

        Setting.Load();
        Setting.UnCheck(UserItemList);

    })
})

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-08
작성목적 : 사용자 보기싫은 값 가져와 선택 해제 하기
**********************************************************************************************/
Setting.UnCheck = function (list) {
    var listarr = list.split(',');    
    for (var i in listarr) {
        $("#cbxID" + listarr[i]).attr("checked", false);
    }
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-07
작성목적 : 메뉴설정 저장
**********************************************************************************************/
Setting.Add = function () {
    var chklist = Setting.UnCheckedValue();

    var url = "/DeskPlusEIP/Task/Ws/Menu/Setting.asmx/AddSetting";

    ParamArray = {
        'listID': chklist,
        'isType': IsType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Setting.SuccessSetting");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-08
작성목적 : 메뉴설정 저장 완료
**********************************************************************************************/
Setting.SuccessSetting = function (data, status) {

    if (data.d == true) {
        ParamArray = {
            accActive: 1,
            'reLoad': 1
        };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Task/Menu/Tree.aspx?p=" + GetEncryptValue;

        parent.frmLeft.location = url;

        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ErrorProcessing);
    }

}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-26
작성목적 : 드롭다운 선택 시 이벤트
**********************************************************************************************/
Setting.GetIsTypeList = function () {
    IsType = $('#ddlSetting').val();
    
    var url = "/DeskPlusEIP/Task/Ws/Menu/Setting.asmx/GetIsTypeList";

    ParamArray = {
        'isType': IsType
    };

    var obj = eval("Setting.SuccessIsTypeList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
};


Setting.SuccessIsTypeList = function (data, status) {
    
    Setting.Load();
    Setting.UnCheck(data.d);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 리스트 초기화
**********************************************************************************************/
Setting.Load = function () {
    //$('#cbtnUnAll').hide();
    Setting.GetPageList();
    if ($(".r_title") != undefined) { $(".r_title").text(R_Category); }
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoListData); }
}



Setting.GetPageList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Menu/Setting.asmx/GetPageList";
    var obj = eval("Setting.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-06
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Setting.SuccessPageList = function (data, status) {
    
    $('#phSettingList').setTemplateURL('/DeskPlusEIP/Task/Jt/Menu/JTSetting.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phSettingList').processTemplate(data.d);

}


/**************************************************
작성자   : maniaysw
작성일자 : 2015-05-06
작성목적 : 전체 체크박스 선택, 해제
***************************************************/
Setting.Checked = function () {
    
    $(".cbxID").attr("checked", true);
}

Setting.UnChecked = function () {
    $(".cbxID").attr("checked", false);
}



/**************************************************
작성자   : maniaysw
작성일자 : 2015-05-06
작성목적 : 선택안된 값 가져오기
***************************************************/
Setting.UnCheckedValue = function () {
    var allVals = $('.cbxID:not(:checked)').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}