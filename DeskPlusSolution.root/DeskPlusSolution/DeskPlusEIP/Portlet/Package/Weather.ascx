<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Weather.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Weather" %>
<script type="text/javascript">
    var weather_tabPortletID = "<%=TabPortletID%>";
    var weather_portletID = "<%=PortletID%>";
    var weather_categoryID = "<%=CategoryID%>";
    var weather_dataURL = "<%=DataURL%>";
    var weather_dataType = "<%=DataType%>";
    var weather_IsAuth = "<%=IsAuth%>";
    var weather_ListCount = "<%=ListCount %>";
    var weather_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var CitySelect = "<%=CitySelect %>"
    
    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 받지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {
         var portletDom = weather_tabPortletID + "_" + weather_portletID;
         //alert(weather_IsMini);

        if (weather_IsAuth != "True") {
            $("#ptConfig_" + portletDom).remove();       
        }

        //최소화 여부
        if (weather_IsMini == "1") {
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

        WeatherLoad(CitySelect);

    });

   function Weather_ContextMenuWork(id, action, el, pos) {
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
                Main.ModMinMaxPortlet(weather_tabPortletID, weather_portletID, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }
        case "max":
            {
                Main.ModMinMaxPortlet(weather_tabPortletID, weather_portletID, "0");
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

    function WeatherLoad(location)
    {    
       var url = "/DeskPlusEIP/Portlet/Ws/Package/Weather.asmx/WeatherList";
        ParamArray = { 'location':location }

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Weather_SuccessList");

        AjaxService(url, data, obj);
    }

    function Weather_SuccessList(data, status)
    {       
        var objList = data.d;
        
        $.each(objList, function (index, objListInfo) {

            var list = objListInfo.hour; 
            var img = objListInfo.weatherImg;
            var tomorrow = objListInfo.tomorrow;
            var todayinfo = objListInfo.todayinfo;
            //alert(list);
            document.getElementById("weatherIcon").src = img;
            document.getElementById("weatherText").innerText = list;
            document.getElementById("weatherToday").innerText = todayinfo;
            document.getElementById("weatherTomo").innerText = tomorrow;
        })
        
    };



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

<!-- 포틀릿(배너) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-weather"> 

        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><%=Title%></span>
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
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                </div>    
            </div>
        </div>
    
        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show" >
            <div class="portlet-contents">
              <div class="content-tabs">

                    <div style="float:right;padding-top:10px;text-align:center;font-weight:normal;">
                        <asp:DropDownList id="ddlWeather" runat="server" onchange="WeatherLoad(this.value)" AutoPostBack="false" CssClass="searchselect form-control"></asp:DropDownList>
                    </div>

                    <table cellpadding="0" cellspacing="0" border="0" style="width:99%;">
                    <tbody>
                    <tr>
                        <td style="text-align:center;padding:3px;"><img id ="weatherIcon"></td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="font-weight:bold;padding-left:5px;"><span id="weatherText" style="font-size:22px;"></span> <%=GetResource("Weather")%></td>
                    </tr>
                    <tr style="height:22px;">
                        <td style="font-weight:bold;padding-left:20px;"><span style="color:#ff7e00;"><%=GetResource("Today")%> :</span><span id="weatherToday" style="padding-left:5px;"></span></td>
                    </tr>
                    <tr style="height:22px;">
                        <td style="font-weight:bold;padding-left:20px;"><span style="color:#ff7e00;"><%=GetResource("Tomorrow")%> :</span><span id="weatherTomo" style="padding-left:5px;"></span></td>
                    </tr>
                    </tbody>
                    </table>

                   

                 </div>
                              
            </div>
        </div>
    
        <div style="display: none">
                <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>
</div>



<!-- 포틀릿(배너) End //-->