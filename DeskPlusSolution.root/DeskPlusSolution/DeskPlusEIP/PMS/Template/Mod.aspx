<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Template.Mod" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <h1><%=GetResource("Add_Templete")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
				  <div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->
                        <table class="table_typeB">
                            <caption>
                            </caption>
                            <colgroup>
                                <col width="100px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbTitle" runat="Server" Width="97%"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div id="tabs" class="layout_subbox">
                            <ul id="tab" class="tab">
                                <li><a id="ActiveX" class="tab_on"><span>ActiveX</span> </a></li>
                                <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                            </ul>
                            <div class="divEditor">
                                <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <input type="hidden" id="contents" name="contents" value="<%: Contents %>" />
        <asp:TextBox ID="tbHiddenSelectedContent" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" TextMode="MultiLine" Width="0" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Width="0" runat="server"></asp:TextBox>

        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"
            style="display: none" rows="1" cols="20"></textarea>
    </div>
    <!-- ==================================================================================== -->
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/	
        var projectID           = "<%=ProjectID %>";
        var menuID              = "<%=MenuID %>";  
        var menuType            = "<%=MenuType %>";  
        var pageListUrl         = "<%=PageListUrl %>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var TemplateID           = "<%=TemplateID %>";
        var R_Add_InputTitle = "<%=GetResource("Add_InputTitle") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var ImagePath = "<%=BaseImgPath %>";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Template/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Template/Mod.js");
    </script>
</body>
</html>
