<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnalogClock.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Clock.AnalogClock" %>

<script type="text/javascript">
    var analogClock_tabPortletID = "<%=TabPortletID%>";
    var analogClock_portletID = "<%=PortletID%>";
    var analogClock_IsAuth = "<%=IsAuth%>";
    var analogClock_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";    

    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 받지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {
        var portletDom = analogClock_tabPortletID + "_" + analogClock_portletID;

        if (analogClock_IsAuth != "True") {
            $("#basicMenu_" + portletDom + " .del").remove();
        }

        //최소화 여부
        if (analogClock_IsMini == "1") {
            $("#ptContent_" + portletDom).fadeOut("slow");
        }

        $("#ptConfig_" + portletDom).contextMenu({ menu: "basicMenu_" + portletDom, leftButton: true }, function (action, el, pos) {

        var pID = el[0].id.replace('ptConfig_', '');
            AnalogClock_ContextMenuWork(pID, action, el, pos);
        });

        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        conMenu += '</ul>';

        $("#MenuList").append(conMenu);

        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();
            Main.Popup();
        });

        var clockHtml = "<div id='clock_Contry_{0}' class='clock_container'>";                
        clockHtml += "<div class='clockHolder'>";
	    clockHtml += "<div class='rotatingWrapper'><img alt='' class='hour' src='/DeskPlusEIP/Portlet/Package/Clock/images/clock_hour.png' /></div>";
	    clockHtml += "<div class='rotatingWrapper'><img alt='' class='mins' src='/DeskPlusEIP/Portlet/Package/Clock/images/clock_min.png' /></div>";
	    clockHtml += "<div class='rotatingWrapper'><img alt='' class='sec' src='/DeskPlusEIP/Portlet/Package/Clock/images/clock_sec.png' /></div>";
	    clockHtml += "<img alt='' class='clock' src='/DeskPlusEIP/Portlet/Package/Clock/images/clock_face.png' />";
        clockHtml += "</div>"; 
        clockHtml += "<div class='digital' style='padding-top:5px;color:#ff7e00;'>";
	    clockHtml += "<span class='hr'></span><span class='minute'></span> <span class='period'></span>";
        clockHtml += "</div>";
        clockHtml += "</div>";

        var defContry = $("#ddlCountry_" + portletDom).val();        
        $("#clockRander_" + portletDom)[0].innerHTML = clockHtml.replace("{0}",defContry);
        $('#clock_Contry_' + defContry).jClocksGMT({ offset: '+9', hour24: false });

        $("#ddlCountry_" + portletDom).change(function (e) {  
         
            e.preventDefault();         

            var contry = $("#ddlCountry_" + portletDom).val();            
            var diffTime = "0";

            switch(contry) {
                case "Newyork" :
                    diffTime = "-5";
                break;
                case "Sanfrancisco" :
                    diffTime = "-8";
                break;
                case "Hawaii" :
                    diffTime = "-10";
                break;
                case "London" :
                    diffTime = "0";
                break;
                case "Roma" :
                    diffTime = "+1";
                break;
                case "Bangkok" :
                    diffTime = "+7";
                break;
                case "Singapore" :
                    diffTime = "+8";
                break;
                case "Seoul" :
                    diffTime = "+9";
                break;
                case "Sydney" :
                    diffTime = "+11";
                break;
            }

            $("#clockRander_" + portletDom)[0].innerHTML = clockHtml.replace("{0}",contry);
            $('#clock_Contry_' + contry).jClocksGMT({ offset: diffTime, hour24: false });
        });
        
    });

    function AnalogClock_ContextMenuWork(id, action, el, pos) {
        switch (action) {
            case "del":
                {
                    Main.DelTabPortlet(id);
                    if (PortletAuth == "True") {
                        $("#portlet_" + id).fadeOut("slow");
                    }
                    break;
                }
            case "min":
                {
                    Main.ModMinMaxPortlet(analogClock_tabPortletID, analogClock_portletID, "1");
                    $("#ptContent_" + id).fadeOut("slow");
                    break;
                }
            case "max":
                {
                    Main.ModMinMaxPortlet(analogClock_tabPortletID, analogClock_portletID, "0");
                    $("#ptContent_" + id).fadeIn("slow");
                    break;
                }
            case "info":
                {
                    alert(R_NoSettingInfo);
                    break;
                }
        }
    }

    function MOver_<%=TabPortletID%>_<%=PortletID%>()
    {
        $("#div_<%=TabPortletID%>_<%=PortletID%>").show();
        //관리자가 아니면 - 관리자 경우 활성화
        if (Banner_IsAuth != "True") {
            $("#div_<%=TabPortletID%>_<%=PortletID%>").hide();
        }
    }

    function MOut_<%=TabPortletID%>_<%=PortletID%>()
    {
        $("#div_<%=TabPortletID%>_<%=PortletID%>").hide();
    }


</script>
<link rel="stylesheet" href="/DeskPlusEIP/Portlet/Package/Clock/css/jClocksGMT.css">
<script type="text/jscript" src="/DeskPlusEIP/Portlet/Package/Clock/js/jClocksGMT.js"></script>
<script type="text/jscript" src="/DeskPlusEIP/Portlet/Package/Clock/js/jquery.rotate.js"></script>
<!-- 포틀릿(배너) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'"  onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'">

        <div class="portlet-header">
            <!-- 상단 타이틀 //-->
            <li class="title headtitle"><a class="ptReload" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></li>
            <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset"><a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a></li>
        </div>
        
        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
            <!-- 내용 //-->
            <div style="min-height: 150px;text-align:center;">

                <div id="clockRander_<%=TabPortletID%>_<%=PortletID%>" style="display:inline-block;"></div>
                <div style="min-height: 6px;"></div>                
			    <select id="ddlCountry_<%=TabPortletID%>_<%=PortletID%>" name="ddlCountry_<%=TabPortletID%>_<%=PortletID%>">
				    <option value="Seoul" selected><%=GetResource("AnalogClock_Korea") %>,<%=GetResource("AnalogClock_Japan")%></option>
				    <option value="Singapore"><%=GetResource("AnalogClock_Singapore")%>,<%=GetResource("AnalogClock_HongKong")%></option>
				    <option value="London"><%=GetResource("AnalogClock_England")%></option>
				    <option value="Roma"><%=GetResource("AnalogClock_Italy")%></option>
				    <option value="Bangkok"><%=GetResource("AnalogClock_Thailand")%></option>
				    <option value="Sydney"><%=GetResource("AnalogClock_Australia")%></option>
				    <option value="Hawaii"><%=GetResource("AnalogClock_Hawaii")%></option>
				    <option value="Sanfrancisco"><%=GetResource("AnalogClock_SanFrancisco")%></option>
				    <option value="Newyork"><%=GetResource("AnalogClock_NewYork")%></option>
			    </select>
            </div>
        </div>    
    </ul>
</div>

            