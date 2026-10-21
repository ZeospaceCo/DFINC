<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IfrmNaver.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.IfrmNaver" %>
<script type="text/javascript">

    var iFrmNaver_Gubun_<%=TabPortletID%>_<%=PortletID%> = "0";

    $(document).ready(function () {

        //ContextMenu 생성하기
        var conMenu = '';
        $("#MenuList").append(conMenu);
        

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        if ("<%=IsAuth%>" != "True") {
            $("#basicMenu_<%=TabPortletID%>_<%=PortletID%> .del").remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {                
            $("#ptContent_" + portletDom).fadeOut("slow");
            $("#contentHeader_" + portletDom).attr('style', '');
        }

        // 포틀릿 설정
        $("#ptConfig_" + portletDom).contextMenu({ menu: 'basicMenu_<%=TabPortletID%>_<%=PortletID%>', leftButton: true }, function (action, el, pos) {

            var pID = el[0].id.replace('ptConfig_', '');
            Main.ContextMenuWork(pID, action, el, pos);
            
            switch (action) {
                
                case "min":
                {                    
                    $("#contentHeader_" + portletDom).attr('style', '');  
                    break;
                }
                case "max":
                {
                    $("#contentHeader_" + portletDom).attr('style', 'height:200px;');                    
                    break;
                }
            }

        });

        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                LoadKMS_<%=TabPortletID%>_<%=PortletID%>(iFrmNaver_Gubun_<%=TabPortletID%>_<%=PortletID%>);  

                var loadingPath = "<img src='<%=ImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);
                   
        });


        setTimeout(function () {
            LoadKMS_<%=TabPortletID%>_<%=PortletID%>("0");
        }, 1);  


        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        conMenu += '</ul>';

        $("#MenuList").append(conMenu);
    });
    

    function LoadKMS_<%=TabPortletID%>_<%=PortletID%>(p_gubun) {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        iFrmNaver_Gubun_<%=TabPortletID%>_<%=PortletID%> = p_gubun;

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
<!-- 포틀릿(리스트:일반형) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">    
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'" onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'" > 
        <div class="portlet-header">
        <li class="title headtitle">
            <a onclick="LoadKMS_<%=TabPortletID%>_<%=PortletID%>('0')"><%=Title%></a>
            | <a onclick="LoadKMS_<%=TabPortletID%>_<%=PortletID%>('1')"><span class="time">PCR이력</span></a> 
            | <a onclick="LoadKMS_<%=TabPortletID%>_<%=PortletID%>('2')">PCR정보</a>  
            | <a onclick="LoadKMS_<%=TabPortletID%>_<%=PortletID%>('3')">영업정보</a>  
            | <a onclick="LoadKMS_<%=TabPortletID%>_<%=PortletID%>('4')">인맥정보</a>  
            | <a onclick="LoadKMS_<%=TabPortletID%>_<%=PortletID%>('5')">개선제안</a>  
            <div style="display:none">| <a onclick="LoadKMS_<%=TabPortletID%>_<%=PortletID%>('6')">QandA</a> </div>
        </li>
        </div>
        <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset">
            <a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a>
        </li>
        <div id="contentHeader_<%=TabPortletID%>_<%=PortletID%>" style="height:200px;">        
            <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
                <iframe id="pliFrame_<%=TabPortletID%>_<%=PortletID%>" name="ifrm" class="pane ui-layout-center" width="100%" height="165px"  frameborder="0" marginheight="0" marginwidth="0" scrolling="auto" src="about:blank" ></iframe>                
            </div>
        </div> 
    </ul>
</div>
                
<!-- 포틀릿(리스트:일반형) End //-->