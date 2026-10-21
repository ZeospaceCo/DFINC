<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TreeModReceive.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.Menu.TreeModReceive" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" /> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.hashtable.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var DocumentID = "<%=DocumentID%>";
        var GroupID = "<%=GroupID %>";              //문서ID
        var Sequence = "<%=Sequence %>";            //결재순서
        var ApprovalTag = "<%=ApprovalTag %>";      //결재태그
        var AccountTag = "<%=AccountTag %>";        //결재그룹
        var TargetUserID = "<%=TargetUserID %>";    //원결재자 ID
        var TargetDeptID = "<%=TargetDeptID %>";    //원결재자의 부서ID
        var FolderStatusID = "<%=FolderStatusID %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //정렬
        var ProxyUserID = "<%=ProxyUserID %>";
        
        var R_Msg_ErrSelForm = "<%=GetResource("Msg_ErrSelForm") %>";
        var R_Msg_ErrUserInfo = "<%=GetResource("Msg_ErrDefaultInfo") %>";
        var R_Msg_ErrSelUnit = "<%=GetResource("Msg_ErrSelUnit") %>"; 
        var R_Msg_ErrReCancel= "<%=GetResource("Msg_ErrReCancel") %>";
        var R_DeptSet = "<%=GetResource("DeptSet") %>"; 
        var R_MSG_SubDept = "<%=GetResource("MSG_SubDept") %>"; 
        var R_TitSub = "<%=GetResource("TitSub") %>"; 
        var R_Msg_ErrOpinion = "<%=GetResource("Msg_ErrOpinion") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Menu/Common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Menu/TreeModReceive.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Menu/Common.js");
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Menu/TreeModReceive.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_left" class="left-menu">
    <div class="left-menu-container">
        <!--div id="layout_treetop">
            <span class="treetop_title"><%=GetResource("MakeDocMod")%></span>
        </div-->

        <div id="layout_treeboxAcc">

            <div class="accordion">
                <div class="menu left-main-menu"><a href="#" onclick="" >1.<%=GetResource("SelApprover")%></a></div>
                <div class="menusub">
                    <input type="text" id="dummy" style="display: none;" />
                    <zeoAppDeptUserTree:UCAppDeptUserTreeScript id="UCDeptUserTreeScript1" runat="server" />
                </div>
                <div class="menu left-main-menu"><a href="#" onclick="" >2.<%=GetResource("Opinion2")%></a> (<span class="charsRemaining"></span> <%=GetResource("InChar")%>)</div>
                <div class="menusub">
                    <asp:TextBox ID="tbOpinion" runat="server" TextMode="MultiLine" Width="93%" Height="96%" MaxLength="500"></asp:TextBox>
                </div>
            </div>
            <div id="layout_treebuttom"></div>
        </div>    
    </div>
</div>
    </form>
</body>
</html>
