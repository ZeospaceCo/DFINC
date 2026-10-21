<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Talk.Tab"  EnableViewState="false" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnTabPageSize_" + portletDom).val("<%=PageSize %>");
        $("#boardListCount_" + portletDom).val("<%=PageSize %>");

        var tabBPageSize = $("#hdnTabPageSize_" + portletDom).val();
        

        if ("<%=IsAuth%>" != "True") {
            $("#cbtnPDel_" + portletDom).remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
            $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
            $("#ptContent_" + portletDom).fadeOut("slow");   
        }

        //포틀릿 최소화/최대화
        $("#ptSlide_" + portletDom).on("click", function (e) {
            if($("#hdnTabSlide_" + portletDom).val() == "1")
            {
                //최대화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
                $("#ptContent_" + portletDom).fadeIn("slow");     
                $("#hdnTabSlide_" + portletDom).val("0"); 
            }
            else{
                //최소화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
                $("#ptContent_" + portletDom).fadeOut("slow");    
                $("#hdnTabSlide_" + portletDom).val("1");
            }            
        });

        
        //포틀릿 설정
        $("#ptConfig_" + portletDom).on("click", function (e) {
            // 리스트 행 개수 바인딩
            $("#boardListCount_" + portletDom).val($("#hdnTabPageSize_" + portletDom).val());
            $("#ptListConfig_" + portletDom).slideToggle(600);      
         });

        // 포틀릿 삭제
        $("#cbtnPDel_" + portletDom).click(function (e) {
            e.preventDefault();

            Main.DelTabPortlet(portletDom);
            if (PortletAuth == "True") {
                $("#portlet_" + portletDom).fadeOut("slow");
            }
        });
            

        //바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();            
            //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";

	        parent.frmTop.Menu.GoParam("SNSWorkTalk"); 
            document.location.href = "/DeskPlusEIP/SNS/WorkTalk/Default.aspx";
        });

        // 서브메뉴 - 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            //페이지 사이즈 할당하기
            tabBPageSize = $("#boardListCount_" + portletDom).val();
            $("#hdnTabPageSize_" + portletDom).val(tabBPageSize);

            var tabFirstlID = $("#hdnTabFirst_" + portletDom).val();//초기값 탭 값
            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", tabBPageSize, tabFirstlID);

            // 첫 페이지로 세팅
            $("#hdnTabPageNo_" + portletDom).val("1");            
            LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();            
            LoadTabList_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });


        // 서브메뉴 - 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 서브메뉴 - 초기탭 설정
        $("#cbtnFirst_" + portletDom).click(function (e) {
            e.preventDefault();

            var tabPageSize = $("#hdnTabPageSize_" + portletDom).val();
            var tabSelID = $("#hdnTabSelID_" + portletDom).val();//선택한 탭
            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", tabPageSize, tabSelID);

            $("#hdnTabFirst_" + portletDom).val(tabSelID);//초기탭 값 변경하기
                        
            $("#ptListConfigBG_" + portletDom).show();
            $("#ptListConfig_" + portletDom).hide();
        });

        //$("#ptListConfig_" + portletDom).hide();

        $("#hdnTabPageNo_" + portletDom).val("1");

        //setTimeout(function () {
		        var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
		  //  }, 1);

        /*
	    if ($.browser.msie) {
		    var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
		    LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
	    }
	    else
	    {
		    setTimeout(function () {
		        var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
		    }, 1);
	    }
        */
    });

    
    

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 탭선택시
    ***************************************************/
    function LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        $("#selboardtab_" + portletDom + "_" + $("#hdnTabSelID_" + portletDom).val()).attr('class','btn btn-sm');//모든 tab        
        $("#selboardtab_" + portletDom + "_" + p_portletID).attr('class','btn btn-sm btn-secondary'); //선택한 tab  
        // 첫 페이지로 세팅
        $("#hdnTabPageNo_" + portletDom).val("1"); 
        $("#hdnTabSelID_" + portletDom).val(p_portletID); 

        // 앨범형태이면 하단의 페이징이 보이지 않아야함
        if($("#hdnTabUrl_" + portletDom).val() == "3")
        {
            $(".ptl_more").hide();
        }
        else
        {
            $(".ptl_more").show();
        }

        //선택 탭 파라미터 정의
        LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>(p_portletID);
        //선택 탭 총 카운트
        LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();
        //선택 탭 리스트
        LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 선택한 탭별 파리미터 설정
    ***************************************************/
    function LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {    
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //203:categoryID=1^pageSize=1,204:listcount=5
        var paramValueList = $("#hdnTabParamValue_" + portletDom).val();  
        
        //203:웹서비스경로,204:웹서비스경로
        var urlValue = $("#hdnUrlValue_" + portletDom).val();

        //카테고리값 초기화
        $("#hdnTabCategoryID_" + portletDom).val("");
        
        if(paramValueList != "")
        {
            //초기화
            $("#hdnTabCategoryID_" + portletDom).val("");
            $("#hdnTabUrl_" + portletDom).val("");
            $("#hdnTabSelModule_" + portletDom).val("");
            //$("#hdnTabPageSize_" + portletDom).val("");
            $("#hdnTabSelJTList_" + portletDom).val("");

            var paramValueListStr = paramValueList.split(",");
            //203:categoryID=1
            for (var i = 0; i < paramValueListStr.length; i++) {
                var portletID = paramValueListStr[i].split(":")[0];
                
                if(portletID == p_portletID)
                {
                    //catID=2^listType=1
                    var paramValue = paramValueListStr[i].split(":")[1];  

                    $("#hdnTabSelParamValue_" + portletDom).val(paramValue);
                    
                    //menuCatID=1                  
                    var paramValueStr = paramValue.split("^");                    
                    
                    for (var j = 0; j < paramValueStr.length; j++) {
                        var keyValue = paramValueStr[j].split("=");
                        if(keyValue[0] == "catID")
                        {
                           //게시판 카테고리값
                            $("#hdnTabCategoryID_" + portletDom).val(keyValue[1]);
                        } else if(keyValue[0] == "listType")
                        {
                           //list 형태 :리스트(1);블로그(2);갤러리(3)
                            $("#hdnTabUrl_" + portletDom).val(keyValue[1]);
                        } else if(keyValue[0] == "module")
                        {
                           //module (board, mail.....)
                            $("#hdnTabSelModule_" + portletDom).val(keyValue[1]);
                        } else if(keyValue[0] == "jtListUrl")
                        {
                           //jt html 경로
                            $("#hdnTabSelJTList_" + portletDom).val(keyValue[1]);
                        }
                    }
                }
            }            
        }
        
        //203:웹서비스경로,204:웹서비스경로
        if(urlValue != "")
        {
            var urlValueStr = urlValue.split(",");
            for(var i = 0; i < urlValueStr.length; i++)
            {
                var portletID = urlValueStr[i].split(":")[0];

                if(portletID == p_portletID)
                {
                    //웹서비스경로값 설정
                    $("#hdnTabSelWS_" + portletDom).val( urlValueStr[i].split(":")[1]);
                }
            }
        }        
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 총카운트 가져오기
    ***************************************************/
    function LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        
        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/TabMain.asmx/GetPageTotCount";

        ParamArray = {            
            'catID': $("#hdnTabCategoryID_" + portletDom).val(),
            'module': $("#hdnTabSelModule_" + portletDom).val()
        };

        //alert($("#hdnTabSelModule_" + portletDom).val());

        CallEncrypt(CallGetParam());
        var paramData = "{p:'" + GetEncryptValue + "'}";
        
        $.ajax({
            type: "POST",
            url: urlPage,
            async: false,
            data: paramData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#hdnTabTotCount_" + portletDom).val(data.d)
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadTabList_<%=TabPortletID%>_<%=PortletID%>() {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var module = $("#hdnTabSelModule_" + portletDom).val();
        var urlPage = $("#hdnTabSelWS_" + portletDom).val();        
        
        ParamArray = {
            'pageSize': $("#hdnTabPageSize_" + portletDom).val(),     // 리스트 행 개수
            'pageSizeNo': $("#hdnTabPageNo_" + portletDom).val(),      // 페이지Nuumber
            'totalCount': $("#hdnTabTotCount_" + portletDom).val(), // 전체 페이지 카운트
            'categoryID': $("#hdnTabCategoryID_" + portletDom).val()
        };
        
        CallEncrypt(CallGetParam());
        var paramData = "{p:'" + GetEncryptValue + "'}";
        
        $.ajax({
            type: "POST",
            url: urlPage,
            async: false,
            data: paramData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                var jtListUrl =  $("#hdnTabSelJTList_" + portletDom).val(); //jtList 경로 반환                   
               
                $('#plList_' + portletDom).setTemplateURL(jtListUrl + "?seq=" + Guids(), null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d);
                
                if($(".NoListData") != undefined) {                    
                    $(".NoListData").text(R_NoContent);
                }
                
                if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
                if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });

        
    }
    

    /**************************************************
    작성자   : 
    작성일자 : 2011-05-10
    작성목적 : 모달창으로 게시판 댓글리스트 보기
    ***************************************************/
    function TabWorkMain_PopupBoardViewShort(p_categoryID, p_listID) {

        ParamArray = {
            'categoryID': p_categoryID,
            'listID': p_listID
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/SNS/Common/BoardTalk.aspx?p=" + GetEncryptValue;
        PopupWin(700, 520, url, "", "", true);
    }


    /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 새창으로 게시물 보기
    ***************************************************/
    function TabWorkMain_PopupBoardViewList (p_categoryID, p_listID) {

        ParamArray = {
            'catID': p_categoryID,
            'categoryID': p_categoryID,
            'listID': p_listID,
            'currentPage': 1,
            'searchGroup': '',
            'searchField': '',
            'searchValue': '',
            'order': ''
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx";
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "PopupBoardViewList", "1200", "800")
    }

    /**************************************************
    작성자   : famo23
    작성일자 : 2012-12-13
    작성목적 : 일정보기
    ***************************************************/
    function TabWorkMain_PopupScheduleViewList(p_companyID, p_categoryID, p_listID) {

        ParamArray = {
            'CompanyID': p_companyID,
            'ScheduleID': p_listID,
            'DisplayID': p_categoryID            
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Pim/Schedule/ViewPopup.aspx";
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "PopupScheduleViewList", "1200", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    }

    /**************************************************
    작성자   : famo23
    작성일자 : 2012-12-14
    작성목적 : 할일보기
    ***************************************************/
    function TabWorkMain_PopupToDoViewList(p_listID) {

        ParamArray = {
            'TodoID': p_listID       
        };

        CallEncrypt(CallGetParam());
        
        var url = "/DeskPlusEIP/Pim/ToDo/ViewPopup.aspx";
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "PopupToDoViewList", "1200", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    }

    

    /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 모달창 일정댓글 보기
    ***************************************************/
    function TabWorkMain_PopupScheduleShort(p_scheduleID) { 

        ParamArray = { 
            'scheduleID': p_scheduleID 
        }; 
 
        CallEncrypt(CallGetParam());
        
       // var url = "/DeskPlusEIP/Pim/Schedule/ShortReplyList.aspx?p=" + GetEncryptValue;
       // var param = "p=" + GetEncryptValue;
       // PopupWin(700, 500, url, "", "", true);

        var url = "/DeskPlusEIP/SNS/Common/ScheduleTalk.aspx?p=" + GetEncryptValue;
        var param = "p=" + GetEncryptValue;
        PopupWin(700, 520, url, "", "", true);
    }

    /**************************************************
    작성자   : shimpark
    작성일자 : 2011-04-11
    작성목적 : 모달창 할일댓글 보기
    ***************************************************/
    function TabWorkMain_PopupToDoShort(p_toDoID) { 

        ParamArray = { 
            'toDoID': p_toDoID 
        }; 
 
        CallEncrypt(CallGetParam());
        
        //var url = "/DeskPlusEIP/Pim/Todo/ShortReplyList.aspx?p=" + GetEncryptValue; 
        //var param = "p=" + GetEncryptValue;
        //PopupWin(700, 500, url, "", "", true);

        var url = "/DeskPlusEIP/SNS/Common/TodoTalk.aspx?p=" + GetEncryptValue;
        var param = "p=" + GetEncryptValue;
        PopupWin(700, 520, url, "", "", true);
    }

    /**************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서보기(결재진행중/완료)
    ***************************************************/
    function TabWorkMain_PopupAppViewList(p_docID) { //alert(p_docID);

        ParamArray = { 'documentID': p_docID };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx";
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "SHOWDOC", "1000", "700")
    }
    /**************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서보기(결재진행중/완료)
    ***************************************************/
    function TabWorkMain_PopupAppShort(p_docID, p_isComp, p_groupID, p_accountTag) { //alert(p_docID + "," + p_isComp);

        ParamArray = {
            'docID': p_docID
            ,'groupID' : p_groupID
            ,'accountTag' : p_accountTag
            ,'isComp' : p_isComp
         };
        CallEncrypt(CallGetParam());

        //var url = "/DeskPlusEIP/Approval/Common/Popup/ShortReplyList.aspx?p=" + GetEncryptValue; 
        //PopupWin(700, 500, url, "", "", true);

        var url = "/DeskPlusEIP/SNS/Common/ApprovalTalk.aspx?p=" + GetEncryptValue;
        PopupWin(700, 520, url, "", "", true);
    }
    
    
</script>
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice">

       <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
                <!-- 탭 //-->
                <div class="nav-tabs">                      
                    <%=TabPortletTitle %>
                </div>
                <span class="caption-helper"></span>
            </div>
            <div class="actions">
                <a href="" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-setting-icon"><i class="fas fa-cog"></i></a>
                <a href="" id="ptSlide_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-roll-icon"><i class="fas fa-chevron-down"></i></a>
            </div>
        </div>
        
        <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" class="portlet-setting" style="display:none;">
            <!-- 환경설정 //-->             
            <div class="row">
                <div class="col col-lg portlet-setting-select">
                    <div class="form-group">
                        <select id="boardListCount_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
                            <option title="5" value="5">5 <%=GetResource("ListRowCnt")%></option>
                            <option title="10" value="10">10 <%=GetResource("ListRowCnt")%></option>
                            <option title="20" value="20">20 <%=GetResource("ListRowCnt")%></option>
                            <option title="30" value="30">30 <%=GetResource("ListRowCnt")%></option>
                        </select>
                    </div>
                </div>
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("Save")%></span></a>
                    </span>
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnFirst_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("FirstTabSetting")%></span></a>
                    </span>
                </div>    
            </div>
        </div>

         <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">
            <div class="portlet-contents">
                <!-- 내용 //-->
                <div class="content-tabs" id="plList_<%=TabPortletID%>_<%=PortletID%>">
                    <div style="padding-top:50px;text-align:center;">
                        <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                    </div>
                </div>
            </div>
        </div>     
                 
        <div style="display: none">
            <input type="hidden" id="hdnTabPageNo_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabPageNo_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabCategoryID_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabCategoryID_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabTotCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabTotCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabPageSize_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabParamValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=ParamValueList %>" />
            <input type="hidden" id="hdnUrlValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnUrlValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=UrlValueList %>" />
            <input type="hidden" id="hdnTabUrl_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabUrl_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabSelID_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelID_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabSelParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelParamValue_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabSelWS_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelWS_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabSelModule_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelModule_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabSelJTList_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelJTList_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" value="<%=TabPortletFirst %>" />
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>

</div>
<!-- 포틀릿(리스트:일반형) End //-->
