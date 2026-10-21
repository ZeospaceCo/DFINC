
Main = {}

var current_Column_ID = "";
var portlet_Item_Name = "";
var LayoutColArr = [];

$(function () {
    $(document).ready(function () {

        //레이아웃 로드하기

        //************************************************
        //Main.GetTabPortletLayout(); //서버에서 처리함

        //layout 의 dom 추출하기
        LayoutColArr = $('.column').map(function () {
            return this.id;
        }).get();
        //************************************************

        //탭 토글 기능
        $(".mainbtn").toggle(
            function () {
                $(".mainset").fadeIn('slow');
            },
            function () {
                $(".mainset").hide();
            }
        );



        //탭 토글 기능

        setTimeout(function () {

            Main.SortPortlet(); //포틀릿정렬            

            // 메인팝업포틀릿
            if (MainPopupBoardList != "") {
                // 메인팝업-게시판
                var boardCookie = Main.getCookie(MainPopupBoardList.split(',')[1]);
                if (boardCookie == "") {
                    PopupWin(PopupWidth, PopupHeight, MainPopupBoardList.split(',')[0], "", "", true);
                }
            } else {
                // 메인팝업-Todo
                if (TodoPopup != "") {
                    var todoCookie = Main.getCookie(TodoPopup.split(',')[1]);   //'띄우지않음' 쿠키값 확인
                    if (todoCookie == "") {
                        PopupWin("1000", "520", TodoPopup.split(',')[0], "", "", true);
                    }
                }
            }

            //Popup 
            $('.ptPreview').on('click', function (e) {
                e.preventDefault();
                var url = "/DeskPlusEIP/EIP/Portal/Popup.aspx";
                PopupWin(800, 600, url, "", "", true);
            });

            //개인탭설정
            $("#ConfigTabUser").click(function (e) {
                e.preventDefault();
                var url = "/DeskPlusEIP/EIP/Portal/GetTabUser.aspx";
                PopupWin(700, 375, url, "", "", true);
            });

            //포틀릿추가
            $("#AddTabPortlet").click(function (e) {
                e.preventDefault();

                var layoutCols = LayoutColArr.join(",");

                ParamArray = {
                    'tabID': TabID,
                    'firstColumn': LayoutColArr[0],
                    'isAdmin': IsAdmin,
                    'tabLayout': TabLayout,
                    'layoutCols': layoutCols
                };

                CallEncrypt(CallGetParam());
                var url = "/DeskPlusEIP/EIP/Portal/GetPortlet.aspx?p=" + GetEncryptValue;
                PopupWin(700, 570, url, "", "", true);
            });

            //스킨설정
            $("#ConfigTabSkin").click(function (e) {
                e.preventDefault();
                ParamArray = {
                    'tabID': TabID
                };

                CallEncrypt(CallGetParam());
                var url = "/DeskPlusEIP/EIP/Portal/GetSkin.aspx?p=" + GetEncryptValue;
                PopupWin(600, 290, url, "", "", true);
            });

            //공용탭 관리
            $("#ConfigTabAdmin").click(function (e) {
                e.preventDefault();
                var url = "/DeskPlusEIP/EIP/Admin/Portal/GetTab.aspx";
                PopupWin(700, 575, url, "", "", true);
            });

            //시작탭 설정
            $("#ConfigTabMain").click(function (e) {
                e.preventDefault();
                Main.ModStartTab();
            });

            //화면저장 
            $("#TabPortletOrder").click(function (e) {
                e.preventDefault();
                Main.ModLayoutPortlet();
            });


            //포틀릿 관리
            $("#ConfigPortletUser").click(function (e) {
                e.preventDefault();
                var url = "/DeskPlusEIP/EIP/Portal/AddPortletUser.aspx";
                PopupWin(700, 570, url, "", "", true);
            });
        }, 1);

    });
});

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 레이아웃 호출하기
**********************************************************************************************/
Main.GetTabPortletLayout = function () {
    var layoutPath = "/DeskPlusEIP/EIP/Jt/Portal/Layout/JTLayout" + TabLayout + ".htm?seq=" + Guids();
    $('#placeholderLayout').setTemplateURL(layoutPath, null, { filter_data: false });
    $('#placeholderLayout').processTemplate(null);

    //layout 의 dom 추출하기
    LayoutColArr = $('.column').map(function () {
        return this.id;
    }).get();    
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 탭별 포틀릿 가져오기
**********************************************************************************************/
Main.TabPortletList = function () {
    
    var url = "/DeskPlusEIP/EIP/Ws/Portal/Main.asmx/GetTabPortlet";

    //파라미터값 배열에 담기
    ParamArray = {
        'tabID': TabID
    };

    var obj = eval("Main.SuccessTabPortletList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 탭별 포틀릿 가져오기
**********************************************************************************************/
Main.SuccessTabPortletList = function (data, status) {

    if (data.d == null) return;
    var objList = data.d;

    setTimeout(function () {
        $.each(objList, function (index, TabPortletInfo) {
            //alert(TabPortletInfo.NameBase);
            //Main.LoadPortlet("#" + TabPortletInfo.ColumnID, TabPortletInfo.UIURL, TabPortletInfo.TabPortletID);

        
                //Portlet에서 ID로 다시 Query 하는것보다 기본적인것은 여기서 넘겨주고 추가 적인 사항에 대해서만 포틀릿에서 Query를 함
                Main.LoadPortlet("#" + TabPortletInfo.ColumnID,
                    TabPortletInfo.TabPortletID,
                    TabPortletInfo.PortletID,
                    TabPortletInfo.CategoryID,
                    TabPortletInfo.NameBase,
                    TabPortletInfo.NameKo,
                    TabPortletInfo.NameEn,
                    TabPortletInfo.NameJa,
                    TabPortletInfo.NameFr,
                    TabPortletInfo.NameZhcn,
                    TabPortletInfo.NameZhtw,
                    TabPortletInfo.PortletCode,
                    TabPortletInfo.PortletType,
                    TabPortletInfo.UIType,
                    TabPortletInfo.UITemplate,
                    TabPortletInfo.UIURL,
                    TabPortletInfo.DataType,
                    TabPortletInfo.DataURL,
                    TabPortletInfo.LinkType,
                    TabPortletInfo.LinkPopupSize,
                    TabPortletInfo.LinkURL,
                    TabPortletInfo.IsPesonalization,
                    TabPortletInfo.ActivePortlet,
                    TabPortletInfo.SendMethod,
                    TabPortletInfo.Description,
                    TabPortletInfo.PhotoName,
                    TabPortletInfo.IsAdmin,
                    TabPortletInfo.UserID,
                    TabPortletInfo.IsMinimize,
                    TabPortletInfo.PageSize,
                    TabPortletInfo.ParamValue
                );        
        });
    }, 1);
}


/**************************************************
작성자: freeman
작성일:2012-02-13
작성목적 : 포틀릿 로드
***************************************************/
Main.LoadPortlet = function (column_ID,
        p_TabPortletID, p_PortletID, p_CategoryID, p_NameBase, p_NameKo, p_NameEn, p_NameJa, p_NameFr, p_NameZhcn, p_NameZhtw, p_PortletCode,
        p_PortletType, p_UIType, p_UITemplate, p_UIURL, p_DataType, p_DataURL, p_LinkType, p_LinkPopupSize, p_LinkURL,
        p_IsPesonalization, p_ActivePortlet, p_SendMethod, p_Description, p_PhotoName, p_IsAdmin, p_UserID, p_IsMinimize, p_PageSize, p_ParamValue) {

    var count = 0;
    var url = "/DeskPlusEIP/EIP/Ws/Portal/Main.asmx/LoadPortlet";
    /*
    ParamArray = {
        'tabPortletID': p_TabPortletID,
        'portletID': p_PortletID,
        'categoryID': p_CategoryID,
        'nameBase': p_NameBase,
        'nameKo': p_NameKo,
        'nameEn': p_NameEn,
        'nameJa': p_NameJa,
        'nameFr': p_NameFr,
        'nameZhcn': p_NameZhcn,
        'nameZhtw': p_NameZhtw,        
        'uiTemplate': p_UITemplate,
        'dataType': p_DataType,
        'dataURL': p_DataURL,
        'linkPopupSize': p_LinkPopupSize,
        'linkURL': p_LinkURL,
        'userID': p_UserID,
        'isMinimize': p_IsMinimize,
        'pageSize': p_PageSize,
        'paramValue': p_ParamValue
    };
    */

    
    ParamArray = {
        'tabPortletID': p_TabPortletID,
        'portletID': p_PortletID,
        'categoryID': p_CategoryID,
        'nameBase': p_NameBase,
        'nameKo': p_NameKo,
        'nameEn': p_NameEn,
        'nameJa': p_NameJa,
        'nameFr': p_NameFr,
        'nameZhcn': p_NameZhcn,
        'nameZhtw': p_NameZhtw,
        'portletCode': p_PortletCode,
        'portletType': p_PortletType,
        'uiType': p_UIType,
        'uiTemplate': p_UITemplate,
        'uiURL': p_UIURL,
        'dataType': p_DataType,
        'dataURL': p_DataURL,
        'linkType': p_LinkType,
        'linkPopupSize': p_LinkPopupSize,
        'linkURL': p_LinkURL,
        'isPesonalization': p_IsPesonalization,
        'activePortlet': p_ActivePortlet,
        'sendMethod': p_SendMethod,
        'description': p_Description,
        'photoName': p_PhotoName,
        'isAdmin': p_IsAdmin,
        'userID': p_UserID,
        'isMinimize': p_IsMinimize,
        'pageSize': p_PageSize,
        'paramValue': p_ParamValue
    };
    



    //alert(tabPortletID + " : " + uiURL + " : " + categoryID + " : " + portletID + " : " + nameBase + " : " + dataType + " : " + dataURL);
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    jQuery.ajax({
        type: "POST",  //GET 
        url: url,
        async: false,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            jQuery(column_ID).append(response.d);
        },
        error: function () {
            /*
            if (count == 0) {
            //alert("Load ERROR");
            setTimeout(function () {
            Main.LoadPortlet(column_ID, portletPath, portletID);
            }, 100);
            }
            else {
            jQuery(column_ID).append('Load ERROR');
            }

            count += 1;
            */
        }
    });
}




/**************************************************
작성자: freeman
작성일:2012-02-13
작성목적 : 포틀릿 정렬
***************************************************/
Main.SortPortlet = function () {

    //포틀릿 정렬
    $(".column").sortable({
        connectWith: ".column"
    });



    $(".portlet").addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
		    .find(".portlet-header")
			    .addClass("ui-widget-header ui-corner-all")
			    .end()
		    .find(".portlet-content");

    //$(".column").disableSelection();

    $('.column').sortable({
        cursor: 'move',
        forcePlaceholderSize: true,
        opacity: 0.6,

        start: function (event, ui) {
            current_Column_ID = $(event.target).attr("id");
            portlet_Item_Name = $(event.target).find(".portlet").attr("id");
            //$(event.target).find(".portlet").attr("style","cursor:move");            
        },

        receive: function (event, ui) {
            //alert(current_Column_ID + " : " + portlet_Item_Name);
            //alert(" receive : " + current_Column_ID + " : " + portlet_Item_Name + " : " + event.target.id + " : " + $(event.target).find(".portlet").attr("id") + " : " + ui.item.index());
            //PortletPlaceManager(event.target.id,$(ui.sender).attr("id"),$(event.target).find(".portlet").attr("id") ,ui.item.index(),0);
        },

        change: function (event, ui) {
            //alert(" change : " + event.target.id + " : " + $(event.target).find(".portlet").attr("id") + " : " + ui.item.index());
        },

        update: function (event, ui) {
            //alert(current_Column_ID + " : " + portlet_Item_Name);
            //alert(" update : " + current_Column_ID + " : " + portlet_Item_Name + " : " + event.target.id + " : " + $(event.target).find(".portlet").attr("id") + " : " + ui.item.index());
        }

    });

    // 전사메인의 권한이 없으면 이동 불가 처리 해야 함
    //$(".column").sortable("disable"); 
//    $(".portlet .portlet-header").css("cursor", "move");
//    if (IsAdmin == "1") {
//        if (PortletAuth == "True") {
//            //$(".portlet-header").css("cursor", "move");
//            $(".column").sortable("enable");
//        } else {
            $(".portlet-header").css("cursor", "default");
            $(".column").sortable("disable");
//        }
//    } else {
//        //$(".portlet-header").css("cursor", "move");
//        $(".column").sortable("enable");
//    }
}


/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 상태저장
***************************************************/
Main.PortletPlaceManager = function (column_ID, sender_Column_ID, portlet_ID, row_No, is_Drop) {

}


/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Context Menu
***************************************************/
Main.ContextMenuWork = function (id, action, el, pos) {

    switch (action) {
        case "del":
            {
                Main.DelTabPortlet(id);
                $("#portlet_" + id).fadeOut("slow");
                break;
            }
        case "min":
            {
                Main.ModMinMaxPortlet(id.split("_")[0], id.split("_")[1], "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }
        case "max":
            {
                Main.ModMinMaxPortlet(id.split("_")[0], id.split("_")[1], "0");
                $("#ptContent_" + id).fadeIn("slow");
                break;
            }
        case "info":
            {                
                $("#ptListConfig_" + id).slideToggle(600);
                break;
            }
    }
}

/**************************************************
작성자: shimpark
작성일:2012-02-07
작성목적 : 포틀릿 이동 / 포틀릿 순서 저장하기
***************************************************/
Main.ModLayoutPortlet = function () {

    if (IsPortletMove) {
        IsPortletMove = false;

        portletSuccess = true;

        //alert(LayoutColArr.join(","));    

        jQuery.map(LayoutColArr, function (layout, i) {
            var portletList = $("#" + layout + " .portlet").map(function () {
                return this.id.replace("portlet_", "").split("_")[1];
            }).get().join(",");

            if (portletList != "") {
                Main.ModOrderPortlet(layout, portletList);
            }
        });

        if (portletSuccess) {
            alert(R_SaveChangePortletOrder);
        } else {
            alert(R_ProcessingError);
        }
        $(".column").enableSelection();
        $(".portlet-header").css("cursor", "default");
        $(".column").sortable("disable").sortable("refresh");

        $("#TabPortletOrder span").html(R_PortletMove);
    } else {
        IsPortletMove = true;
        $(".column").disableSelection();
        $(".portlet-header").css("cursor", "move");
        $(".column").sortable("enable").sortable( "refresh" );

        $("#TabPortletOrder span").html(R_SaveTabPortletOrder);
        
    }
}


/**************************************************
작성자: shimpark
작성일:2012-02-07
작성목적 : 시작탭 설정
***************************************************/
Main.ModStartTab = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Portal/Main.asmx/ModBaseTab";

    //파라미터값 배열에 담기
    ParamArray = {
        'tabID': TabID
    };

    var obj = eval("Main.SuccessModStartTab");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 시작탭 설정
**********************************************************************************************/
Main.SuccessModStartTab = function (data, status) {
    if (data.d == true) {
        alert(R_ConfirmApply);
    }
    else {
        alert(R_ProcessingError);
    }
}


/**************************************************
작성자: shimpark
작성일:2012-02-07
작성목적 : 포틀릿 순서 저장하기
***************************************************/
Main.ModOrderPortlet = function (p_layout, p_portletList) {
    //alert(p_portletList);
    var url = "/DeskPlusEIP/EIP/Ws/Portal/Main.asmx/ModOrderPortlet";

    //파라미터값 배열에 담기
    ParamArray = {
        'tabID': TabID,
        'columnID': p_layout,
        'portletList': p_portletList
    };

    var obj = eval("Main.SuccessModOrderPortlet");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 포틀릿 순서 저장하기
**********************************************************************************************/
Main.SuccessModOrderPortlet = function (data, status) {
    if (data.d == false) {
        portletSuccess = false;        
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 탭이동하기
**********************************************************************************************/
Main.GoTab = function (p_tabID) {
    ParamArray = {
        'tabID': p_tabID
    };

    CallEncrypt(CallGetParam());
    location = "Main.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자: freeman
작성일:2012-02-21
작성목적 : 포틀릿 가져오기 (RSS / SQL 일 경우)
pID =>  tabPortletID_portletID     (ex) 72_41
***************************************************/
Main.GetPortletListData = function (pID, firstListCount, lastListCount, dataType, dataURL, uiTemplate, templatePage) {
    var pageUrl;
    var jtPage = (templatePage == "") ? "/DeskPlusEIP/Portlet/Jt/Template/JTList.htm" : templatePage;
    switch (uiTemplate) {
        case "L":
            jtPage = "/DeskPlusEIP/Portlet/Jt/Template/JTList.htm";
            break;
        case "B":
            jtPage = "/DeskPlusEIP/Portlet/Jt/Template/JTBlog.htm";
            break;
        case "A":
            jtPage = "/DeskPlusEIP/Portlet/Jt/Template/JTAlbum.htm";
            break;
    }

    if (dataType == "R") //RSS 일경우
    {
        if (dataURL.indexOf("imbc.com") != -1) {
            pageUrl = "/DeskPlusEIP/EIP/Ws/Portal/Main.asmx/GetRss20";
        }
        else {
            pageUrl = "/DeskPlusEIP/EIP/Ws/Portal/Main.asmx/GetRss";
        }

        ParamArray = {
            'firstListCount': firstListCount,
            'lastListCount': lastListCount,
            'dataURL': dataURL
        };
        
        CallEncrypt(CallGetParam());
        var prarmData = "{p:'" + GetEncryptValue + "'}";

        jQuery.ajax({
            type: "POST",  //GET 
            url: pageUrl,
            async: true,
            data: prarmData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                if (data != null) {
                    $('#plList_' + pID).setTemplateURL(jtPage + "?seq=" + Guids(), null, { filter_data: false });
                    $('#plList_' + pID).processTemplate(data.d);

                    if ($(".NoListData") != undefined) {
                        $(".NoListData").text(R_NoContent);
                    }

                    if ($(".ptlType3Left") != undefined) { $(".ptlType3Left").attr('src', ImagePath + '/Main/ptl_laytype3_left.gif'); }
                    if ($(".IconNoImg") != undefined) { $(".IconNoImg").attr('src', ImagePath + '/Icon/ico_noimg.gif'); }
                    if ($(".PtlType3Right") != undefined) { $(".PtlType3Right").attr('src', ImagePath + '/Main/ptl_laytype3_right.gif'); }
                    

                    if ($(".IconBullet") != undefined) { $(".IconBullet").attr('src', ImagePath + '/main/ico_bullet2.gif'); }

                    if (uiTemplate == "A") {
                        //alert(Math.floor($('#plList_' + pID).width() / 160) + " : " + Math.ceil($('#plList_' + pID).width() / 160));
                        //alert('#plList_' + pID + " .imageSlider .imageList");
                        $("#plList_" + pID + " .imageSlider .imageList").jCarouselLite({
                            btnNext: "#plList_" + pID + " .imageSlider .right",
                            btnPrev: "#plList_" + pID + " .imageSlider .left",
                            visible: Math.floor($("#plList_" + pID).width() / 160),
                            speed: 400
                        });
                    }


                } else {
                    $('#plList_' + pID).val("error : Problem in loading RSS");
                }
            },
            error: function () {
                $('#plList_' + pID).val("error : Problem in loading RSS");
            }
        });
    } else {  //SP 호출일 경우
        url = "/DeskPlusEIP/EIP/Ws/Portal/Main.asmx/GetSP";
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 탭의 포틀릿 삭제
          tabPortletID =>  tabPortletID_portletID     (ex) 72_41
**********************************************************************************************/
Main.DelTabPortlet = function (pID) {
    
    if (pID == "") return;

    var url = "/DeskPlusEIP/EIP/Ws/Portal/Main.asmx/DelTabPortlet";

    ParamArray = {
        'tabPortletID': pID.split("_")[0]
    };

    var obj = eval("Main.SuccessDelPortlet");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 포틀릿 삭제 성공
**********************************************************************************************/
Main.SuccessDelPortlet = function (data, status) {
    if (data.d == false) {
        
    }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 탭별 개인들의 포틀릿의 최소,최대화 정보 저장하기
***************************************************/
Main.ModMinMaxPortlet = function (p_tabPortletID, p_portletID, p_isMinimize) {
    
    var url = "/DeskPlusEIP/EIP/Ws/Portal/Main.asmx/AddTabPortletUser";

    ParamArray = {
        'tabPortletID': p_tabPortletID,
        'tabID': TabID,
        'portletID': p_portletID,
        'isMinimize': p_isMinimize
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Main.SuccessModMinMaxPortlet");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-18
작성목적 : 성공시 ,  탭별 개인들의 포틀릿의 최소,최대화 정보 저장하기
**********************************************************************************************/
Main.SuccessModMinMaxPortlet = function (data, status) {
    if (data.d == true) {

    }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 포틀릿별 개인들의 설정정보 저장하기
***************************************************/
Main.ModTabPortletUser = function (p_tabPortletID, p_portletID, p_pageSize, p_etcParam) {

    var url = "/DeskPlusEIP/EIP/Ws/Portal/Main.asmx/ModTabPortletUser";

    ParamArray = {        
        'tabPortletID': p_tabPortletID,
        'tabID': TabID,
        'portletID': p_portletID,
        'pageSize': p_pageSize,
        'etcParam': p_etcParam
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Main.SuccessModTabPortletUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-18
작성목적 : 성공시 ,  포틀릿별 개인들의 설정정보 저장하기
**********************************************************************************************/
Main.SuccessModTabPortletUser = function (data, status) {
    if (data.d == true) {
        
    }
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-05-08
작성목적 : 포틀릿 내용보기 공통 팝업
**********************************************************************************************/
Main.PortletPopup = function (url) {
    PopupWin(700, 560, url, "", "", true);
}

/**********************************************************************************************
작성자   : shipark
작성일자 : 2012-05-08
작성목적 : 쿠키정보 가져오기
**********************************************************************************************/
Main.getCookie = function (cName) {
    cName = cName + '=';
    var cookieData = document.cookie;
    var start = cookieData.indexOf(cName);
    var cValue = '';
    if (start != -1) {
        start += cName.length;
        var end = cookieData.indexOf(';', start);
        if (end == -1) end = cookieData.length;
        cValue = cookieData.substring(start, end);
    }
    return unescape(cValue);
}