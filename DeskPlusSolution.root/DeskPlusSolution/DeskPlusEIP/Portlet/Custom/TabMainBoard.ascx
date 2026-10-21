<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabMainBoard.ascx.cs"
    Inherits="Zeospace.DeskPlus.Portlet.Web.Custom.TabMainBoard" EnableViewState="false" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {

        //ContextMenu 생성하기
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';        
        conMenu += '</ul>';
        $("#MenuList").append(conMenu);

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";        

        if ("<%=IsAuth%>" != "True") {
            $("#basicMenu_<%=TabPortletID%>_<%=PortletID%> .del").remove();
        }   
        
        // 포틀릿 설정
        $("#ptConfig_" + portletDom).contextMenu({ menu: 'basicMenu_<%=TabPortletID%>_<%=PortletID%>', leftButton: true }, function (action, el, pos) {
            var pID = el[0].id.replace('ptConfig_', '');
            Main.ContextMenuWork(pID, action, el, pos);

            // 리스트 행 개수 바인딩
            $("#boardListCount_" + portletDom).val($("#hdnTabPageSize_" + portletDom).val());
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });        

        //setTimeout(function () { 
            $("#hdnTabSelID_" + portletDom).val("0");            
            LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("0");
        //}, 1);        

    });
    

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 탭선택시
    ***************************************************/
    function LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(p_gubun)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";        

        $("#selTabMainBoard_" + portletDom + "_" + $("#hdnTabSelID_" + portletDom).val()).attr('class','');     
        $("#selTabMainBoard_" + portletDom + "_" + p_gubun).attr('class','on hovercol');
        $("#hdnTabSelID_" + portletDom).val(p_gubun);        

        switch (p_gubun) {
            case "0":
                ParamArray = {
                    'catID': '30',
                    'recordPerPage': '5'
                };

                CallEncrypt(CallGetParam());

                $('#pliFrame_' + portletDom)[0].src = "/DeskPlusEIP/Portlet/Package/Board/IFramList.aspx?p=" + GetEncryptValue;
                break;
            case "1":
                $('#pliFrame_' + portletDom)[0].src = "http://www.zeuscoltd.com/globalzeus/zkms/Portlet1_PCR_main.php";
                break;
            case "2":
                $('#pliFrame_' + portletDom)[0].src = "http://www.zeuscoltd.com/globalzeus/zkms/Portlet2_PCR_info.php";
                break;
            case "3":
                $('#pliFrame_' + portletDom)[0].src = "http://www.zeuscoltd.com/globalzeus/zkms/Portlet3_INFO_main.php";
                break;
            case "4":
                $('#pliFrame_' + portletDom)[0].src = "http://www.zeuscoltd.com/globalzeus/zkms/Portlet4_INFO_person.php";
                break;
            case "5":
                $('#pliFrame_' + portletDom)[0].src = "http://www.zeuscoltd.com/globalzeus/zkms/Portlet5_PRO_main.php";
                break;
            case "6":
                $('#pliFrame_' + portletDom)[0].src = "http://www.zeuscoltd.com/globalzeus/zkms/Portlet6_QANDA_main.php";
                break;
        }
        
    }


    
</script>

<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.rolling.js"></script>
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">

    <div class="pomailbox bg">

        <div class="mailmenubox ">
            <ul class="mailmenu headtitle">
                <li id="selTabMainBoard_<%=TabPortletID%>_<%=PortletID%>_0" class="on hovercol"><a onclick="LoadSelTab_<%=TabPortletID%>_<%=PortletID%>('0')">공지사항</a></li>
                <li id="selTabMainBoard_<%=TabPortletID%>_<%=PortletID%>_1"><a onclick="LoadSelTab_<%=TabPortletID%>_<%=PortletID%>('1')">PCR이력</a></li>
                <li id="selTabMainBoard_<%=TabPortletID%>_<%=PortletID%>_2"><a onclick="LoadSelTab_<%=TabPortletID%>_<%=PortletID%>('2')">PCR정보</a></li>
                <li id="selTabMainBoard_<%=TabPortletID%>_<%=PortletID%>_3"><a onclick="LoadSelTab_<%=TabPortletID%>_<%=PortletID%>('3')">영업정보</a></li>                
                <li id="selTabMainBoard_<%=TabPortletID%>_<%=PortletID%>_5"><a onclick="LoadSelTab_<%=TabPortletID%>_<%=PortletID%>('5')">개선제안</a></li>
                <li class="poset">
                <a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"><img src="<%=BaseImgPath%>/Package/poset.gif" alt="<%=GetResource("Setting")%>" title="<%=GetResource("Setting")%>" /></a>
                </li>
            </ul><!--div://mailmenu-->            
        </div>

        <div class="maillistbox">
            <iframe id="pliFrame_<%=TabPortletID%>_<%=PortletID%>" name="ifrm" class="pane ui-layout-center" width="100%" height="195px"  frameborder="0" marginheight="0" marginwidth="0" scrolling="auto" src="about:blank" ></iframe>
        </div><!--div://maillistbox-->
    </div><!--div://mailbox-->    
    <div style="display:none">
        <input type="hidden" id="hdnTabSelID_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelID_<%=TabPortletID%>_<%=PortletID%>" />
    </div>
</div>
