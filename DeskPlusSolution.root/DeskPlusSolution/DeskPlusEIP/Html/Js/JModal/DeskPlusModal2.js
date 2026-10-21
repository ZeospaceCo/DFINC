/***************************/
//@Author: Adrian "yEnS" Mato Gondelle
//@website: www.yensdesign.com
//@email: yensamg@gmail.com
//@license: Feel free to use it, but keep this credits please!					
/***************************/

//SETTING UP OUR POPUP
//0 means disabled; 1 means enabled;
var popupStatus = 0;

var IE = document.all ? true : false;
//if (!IE) document.addEventListener("mousemove", getMouseXY, false);
//document.onmousemove = getMouseXY; //폼에 XY값 
var tempX = 0; //초기 좌표값 0 
var tempY = 0; //초기 좌표값 0 


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-24
작성목적 : DIV를 Scroll 과 상관없이 중앙에 띄우기
**********************************************************************************************/
(function ($) {
    $.fn.extend({
        center: function (options) {
            var options = $.extend({ // Default values 
                inside: window, // element, center into window 
                transition: 0, // millisecond, transition time 
                minX: 0, // pixel, minimum left element value 
                minY: 0, // pixel, minimum top element value 
                withScrolling: true, // booleen, take care of the scrollbar (scrollTop) 
                vertical: true, // booleen, center vertical 
                horizontal: true // booleen, center horizontal 
            }, options);
            return this.each(function () {
                var props = { position: 'absolute' };

                if (options.vertical) {
                    var top = ($(options.inside).height() - $(this).outerHeight(true)) / 2;
                    if (options.withScrolling) top += $(options.inside).scrollTop() || 0;
                    top = (top > options.minY ? top : options.minY);
                    $.extend(props, { top: top + 'px' });
                }

                if (options.horizontal) {
                    var left = ($(options.inside).width() - $(this).outerWidth(true)) / 2;
                    if (options.withScrolling) left += $(options.inside).scrollLeft() || 0;
                    left = (left > options.minX ? left : options.minX);
                    $.extend(props, { left: left + 'px' });
                }

                if (options.transition > 0) {
                    //$(this).animate(props, options.transition);

                }
                else {
                    $(this).css(props);
                }

                return $(this);
            });
        }
    });
})(jQuery);


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 초기화
***************************************************/
jQuery(document).ready(function () {

    /*
    //LOADING POPUP
    //Click the button event!
    jQuery("#button").click(function() {
    //centering with css
    centerPopup();
    //load popup
    loadPopup();
    });
    */

    //보이지 않게 초기화 한다 
    jQuery("#zeoModalBG").fadeOut("slow");
    jQuery("#zeoModalContact").fadeOut("slow");

    //CLOSING POPUP
    //Click the x event!
    jQuery("#zeoModalContactClose").click(function () {
        //모달창 fadeout 으로 천천히 감추기
        disablePopup();
    });
    //Click out event!
    jQuery("#zeoModalBG").click(function () {
        //모달창 fadeout 으로 천천히 감추기
        //disablePopup();
    });
    //Press Escape event!
    jQuery(document).keypress(function (e) {
        //esc 키와 팝업이 활성회 되어 있을경우
        if (e.keyCode == 27 && popupStatus == 1) {
            //모달창 fadeout 으로 천천히 감추기
            disablePopup();

        }
    });


});


/**************************************************
작성자   : freeman
작성일자 : 2011-11-22
작성목적 : 모달창 띄우기 사용(가로,세로,호출경로,x값,y값)
w - 넓이
h - 높이
url - 페이지 URL
mouseX - 창을 띄울 left 위치  (isModal이 true 이면 적용이 않됨)
mouseY - 창을 띄울 top 위치  (isModal이 true 이면 적용이 않됨)
isModal - true : 모달창으로 띄워지며 D&D 불가 / 부모 페이지 선택 불가
false : 모달리스로 띄워지며 D&D 지원 / 부모 페이지 선택 가능

#### 정책에 따라 무조건 가운데 모달창 간다.(2013.03.26 shimpark & freeman)
***************************************************/
function PopupWin(w, h, url, mouseX, mouseY, isModal) {

    //정책에 따라 무조건 가운데 모달창 간다.(2013.03.26 shimpark & freeman)
    isModal = true;

    //모달 폼 사이즈 설정하기
    jQuery("#zeoModalContact").css({ "width": w, "height": h });
    jQuery("#zeoModalDrag").css("width", w);

    //호출할 페이지의 iframe 사이즈 설정하기
    jQuery("#zeoModalIfrm").css({ "width": w, "height": h });


    if (isModal) {
        mouseX = "";
        mouseY = "";
    }

    //모달창 설정한 값의 좌표값 설정하기
    centerPopup(mouseX, mouseY, isModal);

    //숨겨놓은 모달창 fadin 으로 천천히 보여주기
    loadPopup();

    if (isModal) {  //모달 페이지
        $("#zeoModalDrag").hide();
        $("#zeoModalBG").css({ "opacity": "0.6" });
        $("#zeoModalBG").show();
    } else {
        $("#zeoModalDrag").show();
    }

    //호출할 페이지 post 로 경로 호출하기
    jQuery("#zeoModalForm").attr({ "action": url, "method": "post" }).submit();

    //모달일때 스크롤과 상관없이 가운데에 표시
    if (isModal) {

        $('#zeoModalContact').center();
        $(window).bind('resize', function () {
            $('#zeoModalContact').center({ transition: 300 });
        });
    }
}





/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 모달창 띄우기 사용(가로,세로,호출경로,x값,y값)
***************************************************/
function ModalDesign(w, h, url, mouseX, mouseY) {
    //모달 폼 사이즈 설정하기
    jQuery("#zeoModalContact").css({
        "width": w,
        "height": h
    });

    //호출할 페이지의 iframe 사이즈 설정하기
    jQuery("#zeoModalIfrm").css({
        "width": w,
        "height": h
    });

    //모달창 설정한 값의 좌표값 설정하기
    centerPopup(mouseX, mouseY, false);

    //숨겨놓은 모달창 fadin 으로 천천히 보여주기
    loadPopup();

    //호출할 페이지 post 로 경로 호출하기
    jQuery("#zeoModalForm").attr({
        "action": url,
        "method": "post"
    }).submit();
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 모달창 자동적으로 가운데 띄우기 사용(가로,세로,호출경로,x값,y값)
***************************************************/
function ModalAutoDesign(w, h, url, mouseX, mouseY) {
    //모달 폼 사이즈 설정하기
    jQuery("#zeoModalContact").css({
        "width": w,
        "height": h
    });

    //호출할 페이지의 iframe 사이즈 설정하기
    jQuery("#zeoModalIfrm").css({
        "width": w,
        "height": h
    });

    //모달창 설정한 값의 좌표값 설정하기
    centerPopup(mouseX, mouseY, true);

    //숨겨놓은 모달창 fadin 으로 천천히 보여주기
    loadPopup();

    //호출할 페이지 post 로 경로 호출하기
    jQuery("#zeoModalForm").attr({
        "action": url,
        "method": "post"
    }).submit();
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 모달창 설정한 값의 좌표값 설정하기
#### 정책에 따라 무조건 가운데 모달창 간다.(2013.03.26 shimpark & freeman)
***************************************************/
function centerPopup(mouseX, mouseY, auto) {

    //alert(mouseX + "," + mouseY);

    //정책에 따라 무조건 가운데 모달창 간다.(2013.03.26 shimpark & freeman)
    auto = true;
    mouseX = "";
    mouseY = "";

    //request data for centering
    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var popupHeight = jQuery("#zeoModalContact").height();
    var popupWidth = jQuery("#zeoModalContact").width();

    //LAYOUT x,y 좌표 가운데 auto 용
    var controlHeight = windowHeight / 2 - popupHeight / 2;
    var controlWidth = windowWidth / 2 - popupWidth / 2;

    if (auto == false) {
        //마우스에서 클릭한 위치의 x,y 좌표값
        controlHeight = tempY + 15;
        controlWidth = tempX + 15;

    } else {
        jQuery("#zeoModalBG").css({ "width": $(document).width(), "height": $(document).height() });
    }

    //인위적으로 x 좌표값을 넣으면 클릭한 위치의 좌표와 함께 합산한다.
    if (mouseX != "") {
        controlWidth = controlWidth + Number(mouseX);
    }

    if (mouseY != "") {
        controlHeight = controlHeight + Number(mouseY);
    }

    jQuery("#zeoModalContact").css({
        "position": "absolute",
        "top": controlHeight,
        "left": controlWidth
    });

    //only need force for IE6

    //    jQuery("#zeoModalBG").css({
    //        "height": windowHeight
    //    });

}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 숨겨놓은 모달창 fadin 으로 천천히 보여주기
***************************************************/
function loadPopup() {

    var frame = jQuery('#zeoModalIfrm').get(0);
    var frameDoc = frame.contentDocument || frame.contentWindow.document;
    $('body', frameDoc).html("");


    //loads popup only if it is disabled
    if (popupStatus == 0) {
        //        jQuery("#zeoModalBG").css({
        //            "opacity": "0.7"
        //        });
        jQuery("#zeoModalContact").fadeIn("slow");
        popupStatus = 1;
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 모달창 fadeout 으로 천천히 감추기
***************************************************/
function disablePopup() {
    //disables popup only if it is enabled
    if (popupStatus == 1) {
        jQuery("#zeoModalBG").fadeOut("slow");
        jQuery("#zeoModalContact").fadeOut("slow");
        popupStatus = 0;

        // 해당 함수가 존재 한다면 호출 해준다. Checkbox 그룹에 대한 활성화 처리
        if (typeof (window["CheckEnabled"]) == "function") {
            CheckEnabled(false);
        }

        // 해당 함수가 존재 한다면 호출 해준다. Checkbox 그룹에 대한 활성화 처리
        if (typeof (window["PopupWinReturnFunc"]) == "function") {
            PopupWinReturnFunc();
        }
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 이벤트 발생한 x, y 좌표 값 설정하기
***************************************************/
function getMouseXY(e) {
    try {

        // Detect mouse position
        var _innerW = null;
        var d = {}, x, y;
        if (self.innerHeight) {
            d.pageYOffset = self.pageYOffset;
            d.pageXOffset = self.pageXOffset;
            d.innerHeight = self.innerHeight;
            //d.innerWidth = self.innerWidth;  For.양식등록때, object로인해 수정 (By.withsuki)
            d.innerWidth = (_innerW != null) ? _innerW : self.innerWidth;
        } else if (document.documentElement &&
								document.documentElement.clientHeight) {
            d.pageYOffset = document.documentElement.scrollTop;
            d.pageXOffset = document.documentElement.scrollLeft;
            d.innerHeight = document.documentElement.clientHeight;
            d.innerWidth = (_innerW != null) ? _innerW : document.documentElement.clientWidth;
        } else if (document.body) {
            d.pageYOffset = document.body.scrollTop;
            d.pageXOffset = document.body.scrollLeft;
            d.innerHeight = document.body.clientHeight;
            d.innerWidth = (_innerW != null) ? _innerW : document.body.clientWidth;
        }

        (e.pageX) ? x = e.pageX : x = e.clientX + d.scrollLeft;
        (e.pageY) ? y = e.pageY : x = e.clientY + d.scrollTop;

        if (d.innerWidth < eval(x + 130)) {
            x -= 130;
        }

        tempX = x;
        tempY = y;

        if (tempX < 0) { tempX = 0; } //초기값보다 적을때 0 
        if (tempY < 0) { tempY = 0; } //초기값보다 적을때 0 
        var xValue = tempX; //x축으로 움직인 만큼의 x값 
        var yValue = tempY; //y축으로 움직인 만큼의 y값
    } catch (e) { }
    return true;
}




/**************************************************
작성자   : 김광일
작성일자 : 2011-04-11
작성목적 : 
***************************************************/
function FixModalDesign(w, h, url, mouseX, mouseY) {

    jQuery("#zeoModalContact").css({
        "width": w,
        "height": h
    });

    jQuery("#zeoModalIfrm").css({
        "width": w,
        "height": h
    });
    tempX = 10;
    tempY = 10;
    centerPopupFix(mouseX, mouseY);
    loadPopup();

    $("#zeoModalDrag").hide();
    $("#zeoModalBG").css({ "opacity": "0.6" });
    $("#zeoModalBG").show();

    jQuery("#zeoModalForm").attr({
        "action": url,
        "method": "post"
    }).submit();
}

/**************************************************
작성자   : freeman
작성일자 : 2017-06-14
작성목적 : 모달창 설정한 값의 좌표값 설정하기
***************************************************/
function centerPopupFix(mouseX, mouseY) {

    //request data for centering
    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var popupHeight = jQuery("#zeoModalContact").height();
    var popupWidth = jQuery("#zeoModalContact").width();

    //LAYOUT x,y 좌표 가운데 auto 용
    var controlHeight = windowHeight / 2 - popupHeight / 2;
    var controlWidth = windowWidth / 2 - popupWidth / 2;

    jQuery("#zeoModalBG").css({ "width": $(document).width(), "height": $(document).height() });


    //인위적으로 x 좌표값을 넣으면 클릭한 위치의 좌표와 함께 합산한다.
    if (mouseX != "") {
        controlWidth = controlWidth + Number(mouseX);
    }

    if (mouseY != "") {
        controlHeight = Number(mouseY);
    }

    jQuery("#zeoModalContact").css({
        "position": "absolute",
        "top": controlHeight,
        "left": controlWidth
    });

}