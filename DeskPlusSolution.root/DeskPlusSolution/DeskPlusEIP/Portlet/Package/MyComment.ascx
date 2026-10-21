<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyComment.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.MyComment" %>
<script type="text/javascript">
    var MyComment_Count = 1;    
    var MyComment_TotalCount = "<%=TotalCount %>";
    var MyComment_PageSize = "<%=PageSize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var UserID = "<%=CurrentUserInfo.UserID%>";

    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnMyCommentPageSize_" + portletDom).val("<%=PageSize %>");
        var MyComment_PageSize = $("#hdnMyCommentPageSize_" + portletDom).val();

        // 리스트 전체 카운트
        //$("#hdnMyCommentTotalCount_" + portletDom).val("<%=TotalCount %>");        
        $("#hdnMyCommentTotalCount_" + portletDom).val(MyComment_PageCount<%=TabPortletID%>_<%=PortletID%>()); 

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
            $("#MyCommentCount_" + portletDom).val($("#hdnMyCommentPageSize_" + portletDom).val());
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
        
        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {
            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                // 첫 페이지로 세팅
                $("#hdnMyCommentCount_" + portletDom).val("1");
                LoadMyComment_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);            
        });

        //상세바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();            
	        parent.frmTop.Menu.GoParam("SNSMyTalk"); 
           document.location.href = "/DeskPlusEIP/SNS/MyTalk/Default.aspx";

        });        

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            MyComment_PageSize = $("#MyCommentCount_" + portletDom).val();
            $("#hdnMyCommentPageSize_" + portletDom).val(MyComment_PageSize);

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", MyComment_PageSize);

            // 첫 페이지로 세팅
            $("#hdnMyCommentCount_" + portletDom).val("1");
            LoadMyComment_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });        

       // $("#ptListConfig_" + portletDom).hide();

        $("#hdnMyCommentCount_" + portletDom).val("1");

        //setTimeout(function () {
		        LoadMyComment_<%=TabPortletID%>_<%=PortletID%>();
		 //   }, 1);

        /*
        if ($.browser.msie) {
		    LoadMyComment_<%=TabPortletID%>_<%=PortletID%>();
        }
        else
        {
	        setTimeout(function () {
		        LoadMyComment_<%=TabPortletID%>_<%=PortletID%>();
		    }, 1);
        }
        */

    });


    /**************************************************
    작성자: shine
    작성일:2012-02-07
    작성목적 : 오늘 새 댓글 카운트 가져오기
    ***************************************************/
    function MyComment_TodayCount<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/MyComment.asmx/GetTodayCount";
        ParamArray = {
            'writeDateYMD': <%=Today %>
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
                //$("#hdnMyCommentVal_" + portletDom).text(data.d);   
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });

    }

    /**************************************************
    작성자: shine
    작성일:2012-02-07
    작성목적 : 페이지 카운트 가져오기
    ***************************************************/
    function MyComment_PageCount<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        //var urlPage = "/DeskPlusEIP/Pim/Ws/ShortReply/ShortReplyList.asmx/GetCount";
        var urlPage = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/GetListCount";

        ParamArray = {
            'rUserID': UserID,
            'searchTerm': "",
            'searchTermValue': "",
            'searchField': "",
            'searchValue': ""
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
                $("#hdnMyCommentTotalCount_" + portletDom).val(data.d);
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });

        // 새댓글 카운트
        //MyComment_TodayCount<%=TabPortletID%>_<%=PortletID%>();
    }
    /**************************************************
    작성자: freeman
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadMyComment_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        
        // 전체 카운트 가져오기
        MyComment_PageCount<%=TabPortletID%>_<%=PortletID%>();

        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/MyComment.asmx/GetMyComment";
        
        ParamArray = {
            'pageSize': $("#hdnMyCommentPageSize_" + portletDom).val(),     // 리스트 행 개수
            'pageSizeNo': $("#hdnMyCommentCount_" + portletDom).val(),      // 페이지Nuumber    
            'totalCount': $("#hdnMyCommentTotalCount_" + portletDom).val()  // 전체 페이지 카운트        
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
                $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/JTMyCommentList.htm?seq=" + Guids(), null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d);

                if($(".NoListData") != undefined) {$(".NoListData").text(R_NoContent);}
                if($(".TitleIcon") != undefined) {$(".TitleIcon").attr('src', ImagePath + '/main/ico_bullet2.gif');}
                if($(".BtnNew") != undefined) {$(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif');}            
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });
    }

    /**************************************************
    작성자: freeman
    작성일:2012-02-07
    작성목적 : 포틀릿 페이징(arg; next(이후), before(이전))
    ***************************************************/
    function MyComment_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 전체 카운트 가져오기
        MyComment_PageCount<%=TabPortletID%>_<%=PortletID%>();

        var MyComment_PageSize = $("#hdnMyCommentPageSize_" + portletDom).val();        // 리스트 행 개수
        var MyComment_Count = $("#hdnMyCommentCount_" + portletDom).val();              // 페이지Nuumber
        var MyComment_TotalCount = $("#hdnMyCommentTotalCount_" + portletDom).val();    // 전체 페이지 카운트   

       
        // MAX페이지 가져오기
        var MyComment_MaxCount = 1;
        if (MyComment_TotalCount > 0) {

            // 몫
            var mok = parseInt((MyComment_TotalCount / MyComment_PageSize));
            // 나머지
            var balance = MyComment_TotalCount % MyComment_PageSize;

            if (balance != 0) {
                MyComment_MaxCount = mok + 1;                
            }
            else {
                MyComment_MaxCount = mok;
            }
        }        

        if (arg == "next") {
            MyComment_Count = parseInt(MyComment_Count) + 1;
        }
        else if (arg == "before") {
            MyComment_Count = parseInt(MyComment_Count) - 1;
        }
        
        // 1페이지에서 이전을 누를 때
        if (MyComment_Count < 1) {
            MyComment_Count = 1;
            $("#hdnMyCommentCount_" + portletDom).val(MyComment_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (MyComment_Count > MyComment_MaxCount) {            
            MyComment_Count = MyComment_MaxCount;
            $("#hdnMyCommentCount_" + portletDom).val(MyComment_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnMyCommentCount_" + portletDom).val(MyComment_Count);

        LoadMyComment_<%=TabPortletID%>_<%=PortletID%>();
    }
    
    /**********************************************************************************************
    작성자   : famo23  
    작성일자 : 2013-06-19
    작성목적 : 사용자 클릭시에
    **********************************************************************************************/
    function MyComment_PopupCommentList(p_userID){

        ParamArray = {
            'rUserID': p_userID
        };

        CallEncrypt(CallGetParam());

//        var url = "/DeskPlusEIP/SNS/Common/MyTalkGroup.aspx?p=" + GetEncryptValue;
//        PopupWin(700, 665, url, "", "", true);

        //document.location.href = "/DeskPlusEIP/SNS/MyTalk/Default.aspx?p=" + GetEncryptValue;

        var url = "/DeskPlusEIP/SNS/Common/MyTalkGroup.aspx";
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "ShortReplyList", "1200", "800");



        //var url = "/DeskPlusEIP/Pim/ShortReply/ShortReplyList.aspx";
        //var param = "p=" + GetEncryptValue;
        //WindowObjectReference = PopupWinOpen(url, param, "ShortReplyList", "800", "500");
    }

    /**********************************************************************************************
    작성자   : famo23
    작성일자 : 2013-06-19
    작성목적 : 마이톡 내용이나, 댓글 클릭시에
    **********************************************************************************************/
    function MyComment_PopupCommentGroup(p_groupID){

        ParamArray = {
            'groupID': p_groupID
        };

        CallEncrypt(CallGetParam());

        //var url = "/DeskPlusEIP/SNS/Common/MyTalk.aspx";
        //var param = "p=" + GetEncryptValue;
        //WindowObjectReference = PopupWinOpen(url, param, "GroupMyTalkList", "700", "520");

        var url = "/DeskPlusEIP/SNS/Common/MyTalk.aspx?p=" + GetEncryptValue;
        PopupWin(700, 520, url, "", "", true);
    }

    /**********************************************************************************************
    작성자   : shine  
    작성일자 : 2011-11-24
    작성목적 : 댓글 삭제
    **********************************************************************************************/
    function MyCommnet_DelReple(myTalkID, groupID){   
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        if (confirm(R_ConfirmDelete)) {

            if (myTalkID == groupID) {

                ParamArray = {
                    'groupID': groupID
                };

                CallEncrypt(CallGetParam())
                var paramData = "{p:'" + GetEncryptValue + "'}";
                        
                var urlPage = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/DelGroupMyTalk";

                $.ajax({
                    type: "POST",
                    url: urlPage,
                    async: false,
                    data: paramData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/JTMyCommentList.htm?seq=" + Guids(), null, { filter_data: false });
                        $('#plList_' + portletDom).processTemplate(data.d);

                        if($(".NoListData") != undefined) {$(".NoListData").text(R_NoContent);}                    
                    }
                    ,
                    error: function (data, status, err) {
                        alert(R_ErrorConnection + data);
                    }
                });
            }
            else{
                ParamArray = {
                    'myTalkID': myTalkID
                };

                CallEncrypt(CallGetParam())
                var paramData = "{p:'" + GetEncryptValue + "'}";
                        
                var urlPage = "/DeskPlusEIP/SNS/Ws/MyTalk/Content/List.asmx/DelMyTalk";

                $.ajax({
                    type: "POST",
                    url: urlPage,
                    async: false,
                    data: paramData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/JTMyCommentList.htm?seq=" + Guids(), null, { filter_data: false });
                        $('#plList_' + portletDom).processTemplate(data.d);

                        if($(".NoListData") != undefined) {$(".NoListData").text(R_NoContent);}     
                    }
                    ,
                    error: function (data, status, err) {
                        alert(R_ErrorConnection + data);
                    }
                });
            }
        }

        $("#hdnMyCommentCount_" + portletDom).val("1");
        LoadMyComment_<%=TabPortletID%>_<%=PortletID%>();
    }
    
</script>

<!-- 포틀릿(리스트:일반형) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice">

        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
                <span class="caption-helper"><a class="ptReload" id="ptReload_<%=TabPortletID%>_<%=PortletID%>"><img src="/DeskPlusEIP/Design/C/Img/Package/ico_ref.gif" alt="Reload" /></a></span>
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
                        <select id="MyCommentCount_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
                            <option title="3" value="3">3<%=GetResource("ListRowCnt")%></option>
                            <option title="10" value="10">10<%=GetResource("ListRowCnt")%></option>
                            <option title="20" value="20">20<%=GetResource("ListRowCnt")%></option>
                            <option title="30" value="30">30<%=GetResource("ListRowCnt")%></option>
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
                </div>    
            </div>
        </div>

        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">           
             <div class="portlet-contents">
                <div class="nav-tabs">                                   
                </div>

                <!-- 내용 //-->
                <div class="content-tabs" id="plList_<%=TabPortletID%>_<%=PortletID%>">
                    <div style="padding-top:50px;text-align:center;">
                        <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                    </div>
                </div>
                <!-- 이전/이후 //-->
                <nav aria-label="Page navigation example">
                    <ul class="pagination pagination-sm justify-content ptl_more">
                    <li class="page-item"><a class="page-link" tabindex="-1" onclick="MyComment_Change_<%=TabPortletID%>_<%=PortletID%>('before')"><i class="fas fa-chevron-left"></i></a></li>
                    <li class="page-item"><a class="page-link" onclick="MyComment_Change_<%=TabPortletID%>_<%=PortletID%>('next')"><i class="fas fa-chevron-right"></i></a></li>
                    </ul>
                </nav>

            </div>
        </div>  

        <div style="display: none">
            <input type="hidden" id="hdnMyCommentCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnMyCommentCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnMyCommentTotalCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnMyCommentTotalCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnMyCommentPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnMyCommentPageSize_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnMyCommentParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnMyCommentParamValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=ParamValue %>" />
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>
</div>