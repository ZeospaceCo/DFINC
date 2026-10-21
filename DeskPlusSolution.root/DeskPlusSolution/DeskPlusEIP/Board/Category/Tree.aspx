<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Category.Tree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="BoardTreeScript" Src="../Common/BoardTreeScript.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript">
        var Dcparm = "<%= Dcparm %>";
        var MenuCatID = "<%= MenuCatID %>";
        var CatID = "<%= CatID %>";                
        var AccordionActive = <%=AccActive %>;     
        var FavoriteID = "0";
        var InterestedUserID = "0";
        var OptionUseDept = "<%= OptionUseDept %>";
        var MyAuthority = "<%= MyAuthority %>";
        var OptionListType = "<%= OptionListType %>";
        var OptionUseView = "<%= OptionUseView %>";
        var Types = "BC";
        var ListTypeUser = "0";
        var UCBoardTreeScript_tvBoardTreeScript = "UCBoardTreeScript2_tvBoardTreeScript";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_NoAuthority = "<%=GetResource("NoAuthority") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Category/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Category/Tree.js");
    </script>  
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu "> 
    	<ul class="left-menu-container">
        	<li>
                <!--div id="layout_treetop">
                    <span id="folderName" class="treetop_title"><%=GetResource("Board")%></span>
                </div-->
        
                <div id="layout_treeboxAcc">
                    <div class="accordion">
                        <div class="menu"><a href="#" onclick="" ><%=GetResource("Folder")%></a></div>
                        <div class="menusub">
                            <uc1:BoardTreeScript ID="UCBoardTreeScript1" runat="server" UseIcon="true" 
                            UseWriteAuthority="false" UseReadAuthority="true" ShowTitleNoAuth="true"
                            UseFolder="true" UseMyTemplate="true" UseMyFavorite="false" SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete">
                            </uc1:BoardTreeScript>
                        </div>
        
                        <div class="menu"><a href="#" onclick="" ><%=GetResource("InterestedPerson")%></a></div>
                        <div class="menusub" style="display:none;">
                            <uc1:BoardTreeScript ID="UCBoardTreeScript2" runat="server" UseIcon="true" 
                            UseWriteAuthority="false" UseReadAuthority="true" 
                            UseFolder="false" UseMyTemplate="false" UseMyFavorite="false" UseInterested="true"
                            SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete">
                            </uc1:BoardTreeScript>
                        </div> 
        
                        <div class="menu"><a href="#" onclick="" ><%=GetResource("FavoriteFolder")%></a></div>
                        <div class="menusub" style="display:none;">
                            <uc1:BoardTreeScript ID="UCBoardTreeScript3" runat="server" UseIcon="true" 
                            UseWriteAuthority="false" UseReadAuthority="true" 
                            UseFolder="false" UseMyTemplate="false" UseMyFavorite="true" 
                            SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete">
                            </uc1:BoardTreeScript>
                        </div> 
                        <div class="menu"><a id="impo" href="#"><%=GetResource("Important")%></a></div>                
                    </div>
                    
                </div>
        
                <div id="layout_treebuttom"></div>
            
                <div id="layout_treebutton">
                    <a id="imgDel" style="cursor: pointer" class="btn btn-sm btn-danger"><%=GetResource("Del")%></a>
                    <a id="imgMod" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Modify")%></a>
                    <a id="imgAdd" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Add")%></a>  
                </div>
    		</li>
    	</ul>
            
    </div>    
    <div style="display: none">
        <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
        <asp:TextBox ID="tbCID" runat="server" Width="0"></asp:TextBox>
        <input type="text" id="hiddenCatID" value="<%= MenuCatID %>" />
    </div>      
  
    </form>

</body>
</html>
