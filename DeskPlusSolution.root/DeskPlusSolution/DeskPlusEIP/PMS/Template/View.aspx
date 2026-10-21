<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Template.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
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


<div id="layout_body">
<div class="layout_full">

    <div id="layout_header">
        <h1><%=GetResource("Manage_Template")%></h1>
        <ul class="btn_right">
            <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary" /></li>    
            <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-secondary" /></li>    
            <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger" /></li>    
            <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server" /></li>  
        </ul>
    </div>
    <div id="layout_content">
        <div class="row">
			<div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                <table class="table_typeB">
                <caption></caption>
                <colgroup>
                    <col width="80px" />
                    <col />
                    <col width="80px" />
                    <col />
                </colgroup>
                <tbody>
                <tr>
                    <th><asp:Literal ID="ltlName" runat="server"></asp:Literal></th>
                    <td><asp:HyperLink ID="hlName" runat="server"></asp:HyperLink></td>          
                    <th><asp:Literal ID="ltlWriteDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblWriteDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></th>
                    <td colspan="3"><asp:Label ID="lblTitle" runat="server"></asp:Label></td> 
                </tr>
                </tbody>
            </table>

            <div class="content_box"  style="overflow:auto;">
            <table cellspacing="0" style="min-height:300px;width:100%;">
                <tr>
                    <td valign="top">
                       <asp:Panel ID="pnContent" runat="server" Width="100%" Height="100%" ></asp:Panel>
                    </td>
                </tr>
            </table>
            </div>
          </div>
        </div>
    </div>    
</div>
</div>
    <div style="display: none;">
        <asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton>
    </div>
    </form>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
		var ProjectID           = "<%=ProjectID %>";
        var MenuID              = "<%=MenuID %>";  
        var MenuType            = "<%=MenuType %>";    
        var RqProjectID = "<%=RqProjectID %>";  
        var pageListUrl         = "<%=PageListUrl %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var TemplateID           = "<%=TemplateID %>";
        var ImagePath = "<%=BaseImgPath %>";        
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var PreLang = "<%=PreLang%>";
		var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Template/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Template/View.js");
    </script> 
</body>
</html>
