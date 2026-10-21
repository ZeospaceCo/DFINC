<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Content.Main" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
   <form id="form1" runat="server">
<div id="layout_body">

    <div class="layout_full">
    
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("MainState")%></h1>
        </div>


        <div id="layout_content">
            <div id="subtit" class="mainsub"> 
                * <asp:DropDownList id="ddlYear" runat="server"></asp:DropDownList> <%=GetResource("Year")%> <%=GetResource("MainState")%>
                <div style="float:right;color:White">
                    <span class="cw_i"><%=GetResource("Progress")%></span>
                    <span class="cw_re"><%=GetResource("Delay")%></span>
                    <span class="cw_e"><%=GetResource("Complete")%></span>
                </div>
            </div>

            <div id="subline" class="mainoutline" style="width:100%;padding:5px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width:99%;"> 
             <tr style="height:370px;" valign="top">
                <td id="line" style="padding:2px;">
                    <div id="dvChart" style="overflow-x:auto;padding-bottom:10px;"></div>
                    
                </td>
                <td style="width:10px;border-left:2px dashed #D3D3D3;">&nbsp;</td>
                <td style="width:380px;">
                    <div class="DivScroll" style="overflow:auto;height: 370px;">
                    <asp:Repeater ID="rpList" runat="server">
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" border="1" style="width:100%;" class="summary">
                        <colgroup>
                            <col />
                            <col width="65px" />
                            <col width="65px" />
                            <col width="65px" />
                            <col width="65px" />
                        </colgroup>
                        <tbody>
                        <tr>
                            <th class="center"><%=GetResource("IsGubun")%></th>
                            <th class="T" title="<%=GetResource("ListType0")%>"><%=GetResource("ListType0")%></th>
                            <th class="I" title="<%=GetResource("StandBy")%>+<%=GetResource("Receipt")%>+<%=GetResource("Processing")%>"><%=GetResource("Progress")%></th>
                            <th class="D" title="<%=GetResource("Delay")%>"><%=GetResource("Delay")%></th>
                            <th class="C" title="<%=GetResource("Complete")%>+<%=GetResource("Reject")%>"><%=GetResource("Complete")%></th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <th class="center" title="<%=GetResource("Category")%>"><a onclick="Main.GoList('<%# DataBinder.Eval(Container.DataItem, "ItemID").ToString()%>')">
                                <%# GetItemName(DataBinder.Eval(Container.DataItem, "ItemNameBase").ToString(),
                                                DataBinder.Eval(Container.DataItem, "ItemNameKo").ToString(),
                                                DataBinder.Eval(Container.DataItem, "ItemNameEn").ToString(),
                                                DataBinder.Eval(Container.DataItem, "ItemNameJa").ToString(),
                                                DataBinder.Eval(Container.DataItem, "ItemNameFr").ToString(),
                                                DataBinder.Eval(Container.DataItem, "ItemNameZhcn").ToString(),
                                                DataBinder.Eval(Container.DataItem, "ItemNameZhtw").ToString())%></a></th>
                            <td class="T" title="<%=GetResource("ListType0")%>"><%# DataBinder.Eval(Container.DataItem, "Tot").ToString()%></td>
                            <td class="I" title="<%=GetResource("StandBy")%>+<%=GetResource("Receipt")%>+<%=GetResource("Processing")%>"><%# DataBinder.Eval(Container.DataItem, "I").ToString()%> (<%# DataBinder.Eval(Container.DataItem, "BeforeI").ToString()%>)</td>                                                        
                            <td class="D" title="<%=GetResource("Delay")%>"><%# DataBinder.Eval(Container.DataItem, "D").ToString()%> (<%# DataBinder.Eval(Container.DataItem, "BeforeD").ToString()%>)</td>
                            <td class="C" title="<%=GetResource("Complete")%>+<%=GetResource("Reject")%>"><%# DataBinder.Eval(Container.DataItem, "C").ToString()%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <th class="sum"><%=GetResource("Sum")%></th>
                            <th class="sum"><span id="spTot"></span></th>                            
                            <th class="sum"><span id="spI"></span> (<span id="spBeforeI"></span>)</th>
                            <th class="sum"><span id="spD"></span> (<span id="spBeforeD"></span>)</th>
                            <th class="sum"><span id="spC"></span></th>
                        </tr>                    
                        </tbody>
                        </table>
                    </FooterTemplate>
                    </asp:Repeater>
                    </div>
                </td>
             </tr>
            </table>
            </div>
        </div>
        
    </div>
</div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>  
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/        
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  
        var R_TotalCnt = "<%=GetResource("TotalCnt") %>";
        var Graph1 = "<%=Graph1%>";
        var Graph2 = "<%=Graph2%>";
        var CntTot = "<%=CntTot%>";
        var CntI = "<%=CntI%>";
        var CntD = "<%=CntD%>";
        var CntC = "<%=CntC%>";
        var CntBeforeI = "<%=CntBeforeI%>";
        var CntBeforeD = "<%=CntBeforeD%>";
        var TopCnt = "<%=TopCnt%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Content/Main.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Task/Js/Content/Main.js");
    </script>
</body>
</html>
