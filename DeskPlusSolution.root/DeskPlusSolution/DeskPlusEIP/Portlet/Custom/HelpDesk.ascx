<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpDesk.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Custom.HelpDesk" EnableViewState="false" %>
<script type="text/javascript">
    var HelpDesk_Count = 1;    
    var HelpDesk_TotalCount = "<%=TotalCount %>";
    var HelpDesk_PageSize = "<%=PageSize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {

        //ContextMenu 생성하기
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        conMenu += '<li class="info"><a href="#info"><%=GetResource("PortletSetting")%></a></li>';
        conMenu += '</ul>';
        $("#MenuList").append(conMenu);


        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnHelpDeskPageSize_" + portletDom).val("<%=PageSize %>");
        var helpDesk_PageSize = $("#hdnHelpDeskPageSize_" + portletDom).val();

        // 리스트 전체 카운트
        $("#hdnHelpDeskTotalCount_" + portletDom).val("<%=TotalCount %>");        

        if ("<%=IsAuth%>" != "True") {
            $("#basicMenu_<%=TabPortletID%>_<%=PortletID%> .del").remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
            $("#ptContent_" + portletDom).fadeOut("slow");
        }
        
        // 포틀릿 설정
        $("#ptConfig_" + portletDom).contextMenu({ menu: 'basicMenu_<%=TabPortletID%>_<%=PortletID%>', leftButton: true }, function (action, el, pos) {        
            
            var pID = el[0].id.replace('ptConfig_', '');
            Main.ContextMenuWork(pID, action, el, pos);
            
            // 리스트 행 개수 바인딩
            $("#HelpDeskCount_" + portletDom).val($("#hdnHelpDeskPageSize_" + portletDom).val());
        });

        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                // 첫 페이지로 세팅
                $("#hdnHelpDeskCount_" + portletDom).val("1");
                LoadHelpDesk_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);            
        });

        //상세바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();            
                        
            var paramValue = $("#hdnHelpDeskParamValue_" + portletDom).val();            
            if(paramValue != ""){
                //menuCatID=1^catID=57^dcparm=BOARD1
                var paramValueStr = paramValue.split("^");

                var id = "";
                var pass = "";
                var go = "";

                for(var i=0;i<paramValueStr.length;i++)
                {
                    switch(paramValueStr[i].split("$")[0])
                    {   
                        case "id" :
                            id = paramValueStr[i].split("$")[1];
                            break;
                        case "pass" :
                            pass = paramValueStr[i].split("$")[1];
                            break;
                        case "go" :
                            go = paramValueStr[i].split("$")[1];
                            break;
                    }
                }

                CallEncrypt(CallGetParam());      
                location = "/DeskPlusEIP/Portlet/Package/HelpDeskFrame.asp?id="+ id +"&pass="+ pass +"&go="+ go;
            }
        });        

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            helpDesk_PageSize = $("#HelpDeskCount_" + portletDom).val();
            $("#hdnHelpDeskPageSize_" + portletDom).val(helpDesk_PageSize);

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", helpDesk_PageSize);

            // 첫 페이지로 세팅
            $("#hdnHelpDeskCount_" + portletDom).val("1");
            LoadHelpDesk_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });        

       // $("#ptListConfig_" + portletDom).hide();

        $("#hdnHelpDeskCount_" + portletDom).val("1");

        //setTimeout(function () {
            LoadHelpDesk_<%=TabPortletID%>_<%=PortletID%>();
        //}, 1);

    });

    /**************************************************
    작성자: freeman
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadHelpDesk_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Custom/HelpDesk.asmx/GetHelpDesk";
        
        ParamArray = {
            'pageSize': $("#hdnHelpDeskPageSize_" + portletDom).val(),     // 리스트 행 개수
            'pageSizeNo': $("#hdnHelpDeskCount_" + portletDom).val()      // 페이지Nuumber            
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
                $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Custom/JTHelpDeskList.htm?seq=" + Guids(), null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d);

                if($(".NoListData") != undefined) {$(".NoListData").text(R_NoContent);}
                if($(".TitleIcon") != undefined) {$(".TitleIcon").attr('src', ImagePath + '/main/ico_bullet2.gif');}

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
    function HelpDesk_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var helpDesk_PageSize = $("#hdnHelpDeskPageSize_" + portletDom).val();        // 리스트 행 개수
        var helpDesk_Count = $("#hdnHelpDeskCount_" + portletDom).val();              // 페이지Nuumber
        var helpDesk_TotalCount = $("#hdnHelpDeskTotalCount_" + portletDom).val();    // 전체 페이지 카운트   
        
        // MAX페이지 가져오기
        var helpDesk_MaxCount = 1;
        if (helpDesk_TotalCount > 0) {

            // 몫
            var mok = parseInt((helpDesk_TotalCount / helpDesk_PageSize));
            // 나머지
            var balance = helpDesk_TotalCount % helpDesk_PageSize;

            if (balance != 0) {
                helpDesk_MaxCount = mok + 1;                
            }
            else {
                helpDesk_MaxCount = mok;
            }
        }        

        if (arg == "next") {
            helpDesk_Count = parseInt(helpDesk_Count) + 1;
        }
        else if (arg == "before") {
            helpDesk_Count = parseInt(helpDesk_Count) - 1;
        }
        
        // 1페이지에서 이전을 누를 때
        if (helpDesk_Count < 1) {
            helpDesk_Count = 1;
            $("#hdnHelpDeskCount_" + portletDom).val(helpDesk_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (helpDesk_Count > helpDesk_MaxCount) {            
            helpDesk_Count = helpDesk_MaxCount;
            $("#hdnHelpDeskCount_" + portletDom).val(helpDesk_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnHelpDeskCount_" + portletDom).val(helpDesk_Count);

        LoadHelpDesk_<%=TabPortletID%>_<%=PortletID%>();
    }

    
</script>
<!-- 포틀릿(리스트:일반형) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'"
        onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'">
        <div class="portlet-header"><li class="title headtitle"><a class="ptReload" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"> <%=Title%></a></li></div>
        <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset"><a class="callContextMenu"
            id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a></li>
        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
            <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" style="display: none;">
                <li class="poboxlistset"><span class="listsetbox">
                    <select id="HelpDeskCount_<%=TabPortletID%>_<%=PortletID%>">
                        <option title="5" value="5">5
                            <%=GetResource("ListRowCnt")%></option>
                        <option title="10" value="10">10
                            <%=GetResource("ListRowCnt")%></option>
                        <option title="20" value="20">20
                            <%=GetResource("ListRowCnt")%></option>
                        <option title="30" value="30">30
                            <%=GetResource("ListRowCnt")%></option>
                    </select>
                </span><span class="listsetbtn"><a id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>">
                    <%=GetResource("Save")%></a></span>
                    <!--span class="listsetbtn"><a id="cbtnClose_<%=TabPortletID%>_<%=PortletID%>" title="<%=GetResource("Close")%>"><%=GetResource("Close")%></a></span-->
                </li>
            </div>
            <div id="plList_<%=TabPortletID%>_<%=PortletID%>" style="min-height: 125px;">
                <div style="padding-top: 50px; text-align: center;">
                    <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                </div>
            </div>
            <li>
                <ul class="ptl_more">
                    <a onclick="HelpDesk_Change_<%=TabPortletID%>_<%=PortletID%>('before')">
                        <img src="<%=BaseImgPath%>/Main/ico_b.gif" alt="<%=GetResource("Prev")%>" title="<%=GetResource("Prev")%>" /></a>
                    <a onclick="HelpDesk_Change_<%=TabPortletID%>_<%=PortletID%>('next')">
                        <img src="<%=BaseImgPath%>/Main/ico_a.gif" alt="<%=GetResource("Next")%>" title="<%=GetResource("Next")%>" /></a>
                </ul>
            </li>
            <div style="display: none">
                <input type="hidden" id="hdnHelpDeskCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnHelpDeskCount_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnHelpDeskCategoryID_<%=TabPortletID%>_<%=PortletID%>"   name="hdnHelpDeskCategoryID_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnHelpDeskTotalCount_<%=TabPortletID%>_<%=PortletID%>"   name="hdnHelpDeskTotalCount_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnHelpDeskPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnHelpDeskPageSize_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnHelpDeskParamValue_<%=TabPortletID%>_<%=PortletID%>"   name="hdnHelpDeskParamValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=ParamValue %>" />
            </div>
        </div>
    </ul>
</div>