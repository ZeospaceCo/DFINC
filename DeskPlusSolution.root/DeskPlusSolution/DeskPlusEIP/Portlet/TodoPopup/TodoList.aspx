<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TodoList.aspx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.TodoPopup.TodoList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Package.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Package.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_home.css" />

<style type="text/css">

.gap10 {padding-right:15px;}
.pTWa {height:190px;}
table table td{border:none !important;word-break: break-all;} 
</style>

</head>
<body class="todobody" style="background:none transparent;">
    <form id="form1" runat="server">

    <div class="layer_popup">

            <div class="inside p-4" style="position:relative;">
                
                <!-- 타이틀 -->
                <div style="display:inline-block;width:100%;">
                    <ul class="pobox bg-secondary mb-4 "> 
                        <div class="portlet-header ui-widget-header ui-corner-all" >
                            <li class="title l_PFlist"><span><%=TodayStr %> TO-DO List</span></li>
                            <li class="poset"><i class="fas fa-cog" id="settmenu"></i></li>
                        </div>
                    </ul>
                </div>
                <!-- 메뉴설정 -->
                <div id="settarea" style="display: none;width:100%; padding-bottom:20px; padding-top:0px; border-radius:21px; position:relative;" class="clearfix"> 
                	<ul class="settarea clearfix" >
                        <li style="font-weight:bold;text-align:center;"  class=" bg-secondary text-white col-2 p-2" ><span><%=GetResource("TodoTitle")%></span></li>
                        <li style="background:#f3f3f3;padding:10px;line-height:25px;" title="<%=GetResource("TodoPopupMsg")%>" class="col-8 p-2">
                            <input type="checkbox" class="settitem" id="APP-S" value="APP-S" /><span class="gap10"><%=GetResource("TodoItem01Sett")%></span>
                            <input type="checkbox" class="settitem" id="APP-W" value="APP-W" /><span class="gap10"><%=GetResource("TodoItem02Sett")%></span>
                            <input type="checkbox" class="settitem" id="TAS-S" value="TAS-S" /><span class="gap10"><%=GetResource("TodoItem03Sett")%></span>
                            <input type="checkbox" class="settitem" id="TAS-W" value="TAS-W" /><span class="gap10"><%=GetResource("TodoItem04Sett")%></span>
                            <input type="checkbox" class="settitem" id="PIM-T" value="PIM-T" /><span class="gap10"><%=GetResource("TodoItem05Sett")%></span>
                            <input type="checkbox" class="settitem" id="PIM-S" value="PIM-S" /><span class="gap10"><%=GetResource("TodoItem06Sett")%></span>
                            <input type="checkbox" class="settitem" id="RM-C" value="RM-C" /><span class="gap10"><%=GetResource("TodoItem07Sett")%></span>
                            <input type="checkbox" class="settitem mgr" id="RM-W" value="RM-W" /><span class="mgr"><%=GetResource("TodoItem08Sett")%></span>
                        </li>
                        <li style="text-align:center;" class="col-2 p-2">
                            <span class="listsetbtn"><a id="settsave" class="btn btn-secondary"><%=GetResource("Save")%></a><a id="settclose" class="btn btn-secondary"><%=GetResource("Close")%></a></span>    
                        </li>
					</ul>
                </div>
        
                <div class="layout_scroll" style="min-height:260px;height:395px;display:block;width:100%; float:none">
                    <!-- 테이블 시작 -->
                    <table style="border:0; width:100%;">
                    <caption></caption>
                    <colgroup>
                        <col width="49%"/>
                        <col/>
                        <col width="49%"/>
                    </colgroup>
                    <tbody>
                    <tr>
                        <td><div class="pTWa" id="item_0"></div></td>
                        <td>&nbsp;</td>
                        <td><div class="pTWa" id="item_1"></div></td>
                    </tr>
                    <tr>
                        <td><div class="pTWa" id="item_2"></div></td>
                        <td>&nbsp;</td>
                        <td><div class="pTWa" id="item_3"></div></td>
                    </tr>
                    </tbody>
                    </table>
                </div>
                <div style="display:block;height:20px; "></div>
                <!-- 하위메뉴 -->
                <div class="footarea">
                    <span><a href="javascript:TodoList.ViewChecked()"><%=GetResource("ForWeek")%></a>
                   	<a href="javascript:TodoList.saveCookie()" ><%=GetResource("NotOpen")%></a>
                    <a href="javascript:TodoList.PopupClose()" ><i class="fas fa-times-circle text-secondary" title="<%=GetResource("Close")%>"></i></a></span>
                </div>
        
            </div>

    </div>

</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=UserID %>";
        var TodoMenu = "<%=TodoMenu %>";
        var popid = "<%=PopID %>";
        var IsMgr = "<%=IsMgr%>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var MsgLimitedItem = "<%=GetResource("TodoAlert01")%>";
    </script>
    <!-- script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/TodoPopup/TodoList.js"></script //-->
     <script type="text/javascript">
         JsCall("/DeskPlusEIP/Portlet/Js/TodoPopup/TodoList.js");
    </script>
</body>
</html>