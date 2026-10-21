<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Main.Menu" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CommunityPath%>/Community.css" />    
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_left" class="left-menu">    
    <ul class="left-menu-container" style="width:230px;padding-left:10px">
      <li id="layout_treeboxAcc">
        <ul>
            <li id="layout_treetop">
                <span class="treetop_title"><asp:Literal ID="lbCommunityName" runat="server"></asp:Literal></span>
            </li>
        </ul>
        <div class="cm_userType1">
            <div class="box01">
                <asp:datalist id="dlTopMenu" runat="server">
				    <HeaderTemplate>
                        <ul><li><img src="<%=BaseImgPath%>/Community/cm_cat_home.gif" alt="" /><a id="aContent"><span class="tit"><%=GetResource("Home")%></span></a></ul>
				    </HeaderTemplate>
				    <ItemTemplate>
                        <ul><%# MakeMenu(DataBinder.Eval(Container.DataItem,"MenuID").ToString(),DataBinder.Eval(Container.DataItem,"Type").ToString(),
                                        DataBinder.Eval(Container.DataItem, "NameBase").ToString(), DataBinder.Eval(Container.DataItem, "NameKo").ToString(),
                                        DataBinder.Eval(Container.DataItem,"NameEn").ToString(), DataBinder.Eval(Container.DataItem,"NameJa").ToString(),
                                        DataBinder.Eval(Container.DataItem,"NameFr").ToString(), DataBinder.Eval(Container.DataItem,"NameZhcn").ToString(),
                                        DataBinder.Eval(Container.DataItem,"NameZhtw").ToString())%></ul>
				    </ItemTemplate>
			    </asp:datalist>
            </div>
            <div class="box02" id="dvUser" style="display:none">
                <ul>
                    <li><img src="<%=BaseImgPath%>/Community/cm_cat_search.gif" alt="" /><a id="aSearch1"><span class="tit"><%=GetResource("SearchUserInfo")%></span></a></li>                
                    <li><img src="<%=BaseImgPath%>/Community/cm_cat_board.gif" alt="" /><a id="aMain1"><span class="tit"><%=GetResource("MainCommunity")%></span></a></li>               
                    <li><img src="<%=BaseImgPath%>/Community/cm_cat_home.gif" alt="" /><a class="aClipboard"><%=GetResource("CopyClipBoard")%></a></li>
                </ul>
            </div>
            <div class="box02" id="dvJoin" style="display:none">
                <ul>                
                    <li><img src="<%=BaseImgPath%>/Community/cm_cat_data.gif" alt="" /><a id="aJoin"><span class="tit"><%=GetResource("Join")%></span></a></li>
                    <li><img src="<%=BaseImgPath%>/Community/cm_cat_board.gif" alt="" /><a id="aMain2"><span class="tit"><%=GetResource("MainCommunity")%></span></a></li>    
                    <li><img src="<%=BaseImgPath%>/Community/cm_cat_home.gif" alt="" /><a class="aClipboard"><%=GetResource("CopyClipBoard")%></a></li>        
                </ul>
            </div>
            <div class="box02" id="dvAdmin" style="display:none">
                <ul>
                    <li><img src="<%=BaseImgPath%>/Community/cm_cat_search.gif" alt="" /><a id="aSearch2"><span class="tit"><%=GetResource("SearchUserInfo")%></span></a></li> 
                    <li><img src="<%=BaseImgPath%>/Community/cm_cat_data.gif" alt="" /><a id="aAdmin"><span class="tit"><%=GetResource("AdminMenu")%></span></a></li>
                    <li><img src="<%=BaseImgPath%>/Community/cm_cat_board.gif" alt="" /><a id="aMain3"><span class="tit"><%=GetResource("MainCommunity")%></span></a></li>
                    <li><img src="<%=BaseImgPath%>/Community/cm_cat_home.gif" alt="" /><a class="aClipboard"><%=GetResource("CopyClipBoard")%></a></li>
                </ul>
            </div>        
        </div>
        <div id="layout_treebuttom"></div>       
       </li>
       <li style="border:0px">
         <div class="cm_left_favA">
            <asp:DropDownList ID="ddlMyCommunity" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
            <span id="ShortCutURL" style="display:none;"><asp:Label title="" ID="lbGoUrl" runat="server"></asp:Label></span>  
            <input type="text" id="tbGoUrl" style="opacity: 0;" />          
         </div> 
       </li>
      </ul>  
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <!--script type="text/javascript" src='/DeskPlusEIP/Html/Js/JClip/jquery.zclip.js'></script-->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>    
    <script type="text/javascript">        
        var CommunityID = "<%=CommunityID %>";
        var Grade = "<%=Grade %>";
        var AdminCheck = "<%=AdminCheck %>";
        var R_NoReadAuth = "<%=GetResource("NoReadAuth") %>";
        var R_ClipBoard = "<%=GetResource("ClipBoard") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    </script>
    <script type='text/javascript' src='/DeskPlusEIP/Community/Js/Site/Main/Menu.js'></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Main/Menu.js");
    </script>
</body>
</html>
