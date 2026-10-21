GetSign = {}

var image;
var originalSrc;
var iter = 0;
var points = null;
var clickStatus = false;
var addPoint = 0;
/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //[확인]
        $('#cbtnSubmit').click(function (e) { e.preventDefault(); GetSign.Confirm(); });

        //[닫기]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); GetSign.Close(); });


        //[지우기]
        $('#cbtnClear').click(function (e) { e.preventDefault(); GetSign.Clear(); });




        image = document.getElementById("SignImage");
        originalSrc = $('#SignImage').attr("src");

        $('#drawingSignImage').mousedown(function (e) { clickStatus = true; GetSign.StartStroke(); });
        $('#drawingSignImage').mouseup(function (e) { clickStatus = false; GetSign.EndStroke(); });
        $('#drawingSignImage').mouseout(function (e) { clickStatus = false; GetSign.EndStroke(); });
        $('#drawingSignImage').mousemove(function (e) { GetSign.AddPoints(); });

        //아이패드 터치 이벤트 처리 ByFreeman 20120621
        $('#drawingSignImage').live("touchstart", function (e) { clickStatus = true; GetSign.StartStroke(); });
        $('#drawingSignImage').live("touchend", function (e) { clickStatus = false; GetSign.EndStroke(); e.ontouchmove = null; e.ontouchend = null; });
        $('#drawingSignImage').live("touchmove", function (e) {
            e.preventDefault();
            var touch = e.originalEvent.touches[0] || e.originalEvent.changedTouches[0];
            var elm = $(this).offset();
            var x = touch.pageX - elm.left;
            var y = touch.pageY - elm.top;
            if (x < $(this).width() && x > 0) {
                if (y < $(this).height() && y > 0) {
                    //alert(touch.pageY + ' ' + touch.pageX);

                    if (!clickStatus) return;

                    if (points) {
                        var point = { X: x, Y: y };
                        points.push(point);

                        if (points.length == 3) {
                            GetSign.EndStroke();
                            points = new Array();
                            points.push(point);
                        }

                        window.event.returnValue = false;
                    }
                }
            }
        });

    });
});



/**************************************************
작성자   : freeman
작성일자 : 2012.06.20 
작성목적 : [확인] 사인 저장
***************************************************/
GetSign.Confirm = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/GetSign.asmx/Save";

    if (addPoint < 30) {
        alert(R_MSG_GETSIGN);
        return;
    }

    jQuery.ajax({
        type: "POST",  //GET 
        url: url,
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response.d == "!NULL") {
                alert(R_MSG_FAIL);
                if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
            } else {
                alert(R_MSG_SUCCESS);
                if (parent.SignConfirm) parent.SignConfirm(response.d);
                if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
            }
        },
        error: function () {
            alert(R_MSG_FAIL);
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        }
    });

    points = null;
    window.event.returnValue = false;

}

/**************************************************
작성자   : freeman
작성일자 : 2012.06.20
작성목적 : [취소]
***************************************************/
GetSign.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}


/**************************************************
작성자   : freeman
작성일자 : 2012.06.20
작성목적 : 사인 다시하기
***************************************************/
GetSign.Clear = function () {
    addPoint = 0;
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/GetSign.asmx/Clear";

    jQuery.ajax({
        type: "POST",  //GET 
        url: url,
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var tmpImage = new Image(200, 200);
            tmpImage.src = originalSrc + "?" + iter++;
            tmpImage.onload = function () {
                image.src = tmpImage.src;
            }
        },
        error: function () {

        }
    });

    points = null;
    window.event.returnValue = false;
}

/**************************************************
작성자   : freeman
작성일자 : 2012.06.20
작성목적 : 드로잉 시작
***************************************************/
GetSign.StartStroke = function () {
    points = new Array();
    window.event.returnValue = false;
}

/**************************************************
작성자   : freeman
작성일자 : 2012.06.20
작성목적 : 드로잉 저장
***************************************************/
GetSign.EndStroke = function () {
    if (!points || points.length < 2) return true;

    //테스트 코드
    //$("#pnTest").append("X:" + window.event.offsetX + " , Y:" + window.event.offsetY + " ");
    //points.push({ X: window.event.offsetX, Y: window.event.offsetY });
    addPoint += points.length;
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/GetSign.asmx/Draw";

    // 암호화가 필요 없는 내용이여서 하지 않음
    //    ParamArray = {
    //        'point': points.join(',')
    //    };

    //    CallEncrypt(CallGetParam());
    //    data = "{p:" + GetEncryptValue + "}";
    data = "{p:'" + JSON.stringify(points) + "'}";

    jQuery.ajax({
        type: "POST",  //GET 
        url: url,
        async: false,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var tmpImage = new Image(200, 200);
            tmpImage.src = originalSrc + "?" + iter++;
            tmpImage.onload = function () {
                image.src = tmpImage.src;
            }
        },
        error: function () {

        }
    });

    points = null;
    window.event.returnValue = false;
}

/**************************************************
작성자   : freeman
작성일자 : 2012.06.20
작성목적 : 포인트 추가
***************************************************/
GetSign.AddPoints = function () {

    if (!clickStatus) return;

    if (points) {
        var point = { X: parseInt(window.event.offsetX), Y: parseInt(window.event.offsetY) };
        points.push(point);

        if (points.length == 3) {
            GetSign.EndStroke();
            points = new Array();
            points.push(point);
        }

        window.event.returnValue = false;
    }
}