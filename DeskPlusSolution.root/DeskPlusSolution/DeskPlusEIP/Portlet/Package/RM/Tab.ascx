<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.RM.Tab"  EnableViewState="false" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var R_NoContent = "<%=GetResource("NoContents") %>";

    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnTabPageSize_" + portletDom).val("<%=PageSize %>");
        $("#rmListCount_" + portletDom).val("<%=PageSize %>");

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
            $("#rmListCount_" + portletDom).val($("#hdnTabPageSize_" + portletDom).val());
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

	        parent.frmTop.Menu.GoParam("Resource"); 
            document.location.href = "/DeskPlusEIP/RM/Default.aspx";            
        });

        // 서브메뉴 - 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            //페이지 사이즈 할당하기
            tabBPageSize = $("#rmListCount_" + portletDom).val();
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
       

        $("#hdnTabPageNo_" + portletDom).val("1");

         //setTimeout(function () {
		        var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
		 //   }, 1);

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
    작성자: famo23
    작성일: 2017-02-07
    작성목적 : 탭선택시
    ***************************************************/
    function LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        $("#selrmtab_" + portletDom + "_" + $("#hdnTabSelID_" + portletDom).val()).attr('class','btn btn-sm');//모든 tab        
        $("#selrmtab_" + portletDom + "_" + p_portletID).attr('class','btn btn-sm btn-secondary'); //선택한 tab     

        // 첫 페이지로 세팅
        $("#hdnTabPageNo_" + portletDom).val("1"); 
        $("#hdnTabSelID_" + portletDom).val(p_portletID); 

        //선택 탭 파라미터 정의
        LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>(p_portletID);
        //선택 탭 총 카운트
        LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();
        //선택 탭 리스트
        LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
    }

    /**************************************************
    작성자: famo23
    작성일: 2017-02-07
    작성목적 : 선택한 탭별 파리미터 설정
    ***************************************************/
    function LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {    
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";        
        var selPortletID = $("#hdnTabSelID_" + portletDom).val();

        var paramValueList = $("#hdnTabParamValue_" + portletDom).val();
        var urlValue = $("#hdnUrlValue_" + portletDom).val();

        if(paramValueList != "")
        {
            $("#hdnTabSelJTList_" + portletDom).val("");

            var paramValueListStr = paramValueList.split(",");
            for (var i = 0; i < paramValueListStr.length; i++) {
                var portletID = paramValueListStr[i].split(":")[0];
                
                if(portletID == selPortletID)
                {
                    var paramValue = paramValueListStr[i].split(":")[1];  
                    $("#hdnTabSelParamValue_" + portletDom).val(paramValue);       
                    var paramValueStr = paramValue.split("^");                    
                    
                    for (var j = 0; j < paramValueStr.length; j++) {
                        var keyValue = paramValueStr[j].split("=");
                        if(keyValue[0] == "tabID")
                        {
                           //탭 카테고리값
                            $("#hdnTabCategoryID_" + portletDom).val(keyValue[1]);
                        }
                        else if(keyValue[0] == "jtListUrl")
                        {
                           //jt html 경로
                            $("#hdnTabSelJTList_" + portletDom).val(keyValue[1]);
                        }
                    }                                 
                }
            }            
        }     
        
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
    작성자: famo23
    작성일: 2017-02-08
    작성목적 : 총카운트 가져오기
    ***************************************************/
    function LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var gubun = $("#hdnTabCategoryID_" + portletDom).val();

        var urlPage = "";
        switch (gubun)
        {
            case "Usable1":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/RM/List.asmx/GetUsable1Cnt";   //승인완료
                break;
            case "Usable0":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/RM/List.asmx/GetUsable0Cnt";   //승인대기
                break;           
        }

        ParamArray = {            
            'gubun': gubun
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
                $("#hdnTabTotCount_" + portletDom).val(data.d);
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });
    }


    /**************************************************
    작성자: famo23
    작성일: 2017-02-08
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadTabList_<%=TabPortletID%>_<%=PortletID%>() {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var urlPage = $("#hdnTabSelWS_" + portletDom).val(); 

        ParamArray = {
            'recordPerPage': $("#hdnTabPageSize_" + portletDom).val(),     // 리스트 행 개수
            'pageSizePageNo': $("#hdnTabPageNo_" + portletDom).val(),      // 페이지Nuumber
            'totalItemCount': $("#hdnTabTotCount_" + portletDom).val()     // 전체 페이지 카운트
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
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });
        
    }       

    /**************************************************
    작성자: famo23
    작성일: 2017-02-08
    작성목적 : 포틀릿 페이징(arg; next(이후), before(이전))
    ***************************************************/
    function TabBList_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var rmList_PageSize = $("#hdnTabPageSize_" + portletDom).val();        // 리스트 행 개수
        var rmList_Count = $("#hdnTabPageNo_" + portletDom).val();              // 페이지Nuumber
        var rmList_TotalCount = $("#hdnTabTotCount_" + portletDom).val();    // 전체 페이지 카운트   
        
        // MAX페이지 가져오기
        var rmList_MaxCount = 1;
        if (rmList_TotalCount > 0) {

            // 몫
            var mok = parseInt((rmList_TotalCount / rmList_PageSize));
            // 나머지
            var balance = rmList_TotalCount % rmList_PageSize;

            if (balance != 0) {
                rmList_MaxCount = mok + 1;                
            }
            else {
                rmList_MaxCount = mok;
            }
        }        

        if (arg == "next") {
            rmList_Count = parseInt(rmList_Count) + 1;
        }
        else if (arg == "before") {
            rmList_Count = parseInt(rmList_Count) - 1;
        }
        
        // 1페이지에서 이전을 누를 때
        if (rmList_Count < 1) {
            rmList_Count = 1;
            $("#hdnTabPageNo_" + portletDom).val(rmList_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (rmList_Count > rmList_MaxCount) {            
            rmList_Count = rmList_MaxCount;
            $("#hdnTabPageNo_" + portletDom).val(rmList_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnTabPageNo_" + portletDom).val(rmList_Count);

        //setTimeout(function () {
            //선택 탭 리스트
            LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
        //}, 1);
    } 

    /**************************************************
    작성목적 : 자원관리 상세보기
    ***************************************************/
    function TabRMMain_PopupMyViewList(p_reservationID) { 

        ParamArray = { 'reservationID': p_reservationID };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/RM/Content/ViewPopup.aspx?p=" + GetEncryptValue;
        window.open(url, "RM" + p_reservationID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");  
    }  
    
</script>
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-reservation">

        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
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
                <div class="col col-lg portlet-setting-select">
                    <div class="form-group">
                        <select id="rmListCount_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
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
                    <div style="padding-top: 50px; text-align: center;">
                        <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                    </div>
                </div>      

                <!-- 이전/이후 //-->
                <nav aria-label="Page navigation example">
                    <ul class="pagination pagination-sm justify-content ptl_more">
                    <li class="page-item"><a class="page-link" tabindex="-1" onclick="TabBList_Change_<%=TabPortletID%>_<%=PortletID%>('before')"><i class="fas fa-chevron-left"></i></a></li>
                    <li class="page-item"><a class="page-link" onclick="TabBList_Change_<%=TabPortletID%>_<%=PortletID%>('next')"><i class="fas fa-chevron-right"></i></a></li>
                    </ul>
                </nav>
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
            <input type="hidden" id="hdnTabSelJTList_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelJTList_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" value="<%=TabPortletFirst %>" />
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>
</div>
<!-- 포틀릿(리스트:일반형) End //-->
