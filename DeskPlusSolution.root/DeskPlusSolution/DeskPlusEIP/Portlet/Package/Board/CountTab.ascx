<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountTab.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Board.CountTab" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnTabPageSize_" + portletDom).val("<%=PageSize %>");
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

        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                // 첫 페이지로 세팅
                $("#hdnTabPageNo_" + portletDom).val("1");
                //LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);
        });

        //바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();
            var paramValue = $("#hdnTabSelParamValue_" + portletDom).val();
            if(paramValue != ""){
                //menuCatID=1^catID=57^dcparm=BOARD1
                var paramValueStr = paramValue.split("^");
                var menuCatID = "";
                var catID = "";
                var dcparm = "";

                for(var i=0;i<paramValueStr.length;i++)
                {                
                    switch(paramValueStr[i].split("=")[0])
                    {
                        case "menuCatID" :
                            menuCatID = paramValueStr[i].split("=")[1];
                            break;
                        case "catID" :
                            catID = paramValueStr[i].split("=")[1];
                            break;
                        case "dcparm" :
                            dcparm = paramValueStr[i].split("=")[1];
                            break;
                    }
                }
                
                ParamArray = {
                    'menuCatID': menuCatID,
                    'catID': catID,
                    'dcparm': dcparm
                };

                CallEncrypt(CallGetParam());            
                location = "/DeskPlusEIP/Board/Default.aspx?p=" + GetEncryptValue;
            }
        });

        // 환경설정 저장
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


        // 환경설정 닫기
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

        $("#hdnTabPageNo_" + portletDom).val("1");

         //setTimeout(function () {
		        var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
		   // }, 1);

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
        
        LoadTabCategory_<%=TabPortletID%>_<%=PortletID%>(p_portletID);

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
        LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>();
        //선택 탭 총 카운트
        //LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();
        //선택 탭 리스트
        LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
        
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 탭별 게시판의 파라미터값 설정하기
    ***************************************************/
    function LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>()
    {
    
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";        
        var selPortletID = $("#hdnTabSelID_" + portletDom).val();
        //포틀릿아이디:menuCatID=47,~
        var paramValueList = $("#hdnTabParamValue_" + portletDom).val();

        if(paramValueList != "")
        {
            //menuCatID=1^catID=57,~
            var paramValueListStr = paramValueList.split(",");
            
            for (var i = 0; i < paramValueListStr.length; i++) {
                var portletID = paramValueListStr[i].split(":")[0];
                
                if(portletID == selPortletID)
                {
                    //menuCatID=1^catID=57
                    var paramValue = paramValueListStr[i].split(":")[1];                      
                    $("#hdnTabSelParamValue_" + portletDom).val(paramValue);                  
                }
            }            
        }        
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 카테고리값 설정하기
    ***************************************************/
    function LoadTabCategory_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //포틀릿아이디:menuCatID=47,~
        var paramValueList = $("#hdnTabParamValue_" + portletDom).val();  
        
        //포틀릿아이디:페이지      
        var urlValue = $("#hdnUrlValue_" + portletDom).val();
        
        if(paramValueList != "")
        {
            //menuCatID=1^catID=57,~
            var paramValueListStr = paramValueList.split(",");
            for (var i = 0; i < paramValueListStr.length; i++) {
                var portletID = paramValueListStr[i].split(":")[0];
                
                if(portletID == p_portletID)
                {
                    //menuCatID=1^catID=57
                    var paramValue = paramValueListStr[i].split(":")[1];  
                    
                    //menuCatID=1                  
                    var paramValueStr = paramValue.split("^");                    
                    
                    for (var j = 0; j < paramValueStr.length; j++) {
                    
                        var keyValue = paramValueStr[j].split("=");
                        if(keyValue[0] == "StatusID")
                        {
                           //게시판 카테고리값
                            $("#hdnTabCategoryID_" + portletDom).val(keyValue[1]);
                            break;
                        }
                    }
                }
            }            
        }
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadTabList_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";  
        var gubun = $("#hdnTabCategoryID_" + portletDom).val();
        
        var urlPage = "";      
        switch (gubun)
        {
            case "ShortRep":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/List.asmx/GetRepCountList";   //최다댓글자
                break;
            case "Registrant":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/List.asmx/GetBoardTopRegister";   //최다등록자
                break;
            case "CheckWriting":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/List.asmx/GetBoardTopReader";   //최다조회글
                break;
            case "Comment":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/List.asmx/GetLargestComment";   //최다댓글게시글
                break;
            default :
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/List.asmx/GetLargestRegistrant";   //
                break;
        }
        ParamArray = {

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
                var jtUrl = "";
                switch (gubun)
                {
                    case "ShortRep":
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Board/JTRepCountList.htm?seq=" + Guids();
                        break;
                    case "Registrant":
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Board/JTTopRegister.htm?seq=" + Guids();
                        break;
                    case "CheckWriting":
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Board/JTTopReader.htm?seq=" + Guids();
                        break;
                    case "Comment":
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Board/JTLargestComment.htm?seq=" + Guids();
                        break;
                    default :
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Board/JTLargestRegistrant.htm?seq=" + Guids();
                        break;
                }

                $('#plList_' + portletDom).setTemplateURL(jtUrl, null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d); 

                if($(".NoListData") != undefined) {                    
                    $(".NoListData").text(R_NoContent);
                }

                if ($(".TitleIcon") != undefined) { $(".TitleIcon").attr('src', ImagePath + '/main/ico_bullet2.gif'); }
                if ($(".PtlType3Left") != undefined) { $(".PtlType3Left").attr('src', ImagePath + '/Main/ptl_laytype3_left.gif'); }
                if ($(".IconNoImg") != undefined) { $(".IconNoImg").attr('src', ImagePath + '/Icon/ico_noimg2.gif'); }
                if ($(".PtlType3Right") != undefined) { $(".PtlType3Right").attr('src', ImagePath + '/Main/ptl_laytype3_right.gif'); }   
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });

        
    }

    
</script>

<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice">    

        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></span>
                <span class="caption-helper"></span>
            </div>
            <!-- 탭 //-->
            <div class="nav-tabs">                      
                <%=TabPortletTitle %>
            </div>

            <div class="actions">
                <a href="" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-setting-icon"><i class="fas fa-cog"></i></a>
                <a href="" id="ptSlide_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-roll-icon"><i class="fas fa-chevron-down"></i></a>
            </div>
        </div>
    
       <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" class="portlet-setting" style="display:none;">
            <!-- 환경설정 //-->             
            <div class="row">
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
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
            <input type="hidden" id="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" value="<%=TabPortletFirst %>" />
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>
 
</div>
<!-- 포틀릿(리스트:일반형) End //-->
