<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.SNS.Web.MyTalk.Menu.Menu" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_SNS.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_SNS.css"/>
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_left" class="left-menu ">
    <div class="left-menu-container">

    <!--div id="layout_treetop">
        <span class="treetop_title"><%=GetResource("MyTalk")%></span>
    </div-->

    <div id="layout_treeboxAcc">

        <div class="accordion">

            <div class="menu" id="OpenUser"><a href="#" onclick="" ><%=GetResource("TalkMenu1")%></a></div>
            <div class="menusub" style="text-align:center;">

            <div>
                <ul class="btn_searchbox" style="width:100% !important; text-align:center;" >
                    <li><asp:TextBox ID="ctbSearch1" ClientIDMode="Predictable" CssClass="searchtext form-control" placeholder="" style="width:98px !important; margin-left:3px;" runat="server"></asp:TextBox></li>
                    <li><cc3:ClientSmartButton ID="CbtnDeptUserSearch1" Class="popupsearchbtn" runat="server" /></li>
                </ul>
            </div>
            <div class="gap"></div>
            
            <div id="phOpenList" style="clear: both;"></div>
            
            <!-- 페이징 시작 //-->
            <div id="Pager3" name="Pager3" class="paging_num_s1">
                <span id="pager2"></span>
                <br />
                <h3><span id="pageInfo1"></span></h3>
                <asp:DropDownList ID="ddlListCount1" runat="server" CssClass="aRight form-control">
                    <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <!-- 페이징 끝 //-->
            <div class="gap"></div><div class="gap"></div>

            </div>

            <div class="menu" id="ShareUser"><a href="#" onclick="" ><%=GetResource("TalkMenu2")%></a></div>
            <div class="menusub" style="text-align:center;">

            <div>
                <ul class="btn_searchbox" style="width:100% !important; text-align:center;" >
                    <li><asp:TextBox ID="ctbSearch" ClientIDMode="Predictable" CssClass="searchtext form-control" placeholder="" style="width:98px !important; margin-left:3px;" runat="server"></asp:TextBox></li>
                    <li><cc3:ClientSmartButton ID="CbtnDeptUserSearch" Class="popupsearchbtn" runat="server" /></li>
                </ul>
            </div>
            <div class="gap"></div>
            
            <div id="phShareList" style="clear: both;"></div>
            
            <!-- 페이징 시작 //-->
            <div id="Pager1" name="Pager1" class="paging_num_s1">
                <span id="pager"></span>
                <br />
                <h3><span id="pageInfo"></span></h3>
                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                    <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <!-- 페이징 끝 //-->
            <div class="gap"></div><div class="gap"></div>

            </div>
    
        </div>
        </div>
    </div>

    <div id="layout_treebuttom"></div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.tree.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var RUserID = "<%=RUserID %>";     
        var AccordionActive = 0;  
        var AccordionCheck = 0;  
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var SearchValue = "<%=SearchValue %>"; //검색어   
        var CurrentPage1 = "<%=CurrentPage1 %>"; //현재페이지
        var TotalItemCount1 = "<%=TotalCount1 %>"; //게시물수
        var RecordPerPage1 = "<%=RecordPerPage1 %>"; //보여줄게시물수
        var SearchValue1 = "<%=SearchValue1 %>"; //검색어   
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";   
        var R_Name = "<%=GetResource("Name") %>";             
        var R_Photo = "<%=GetResource("Photo") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/SNS/Js/MyTalk/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/SNS/Js/MyTalk/Menu/Menu.js");
    </script>
</body>
</html>
