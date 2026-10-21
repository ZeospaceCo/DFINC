<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TreeAdmin.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Folder.TreeAdmin" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="WebHardTree" Src="../Common/WebHardTree.ascx" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <style>html, body {height:98%}</style>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Webhard.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Webhard.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript">
        var RealFolderID = "0";
        var ParentFolderID = "0";
        var FolderType = "U";  
        var AccordionActive = <%= p_AccActive %>;    

        var PPercentCount = "<%=PPercentCount%>";
        var PPrivateQuota = "<%=PPrivateQuota%>";
        var PUseCount = "<%=PUseCount%>";      
        var PDisPlayUse = "<%=PDisPlayUse%>";      
        var PDisPlayQuota = "<%=PDisPlayQuota%>";      

        var UID = "0";
        var ListTypeUser = "0";
        var R_DeleteFolder = "<%=GetResource("DeleteFolder") %>";
        var R_DeleteSubFolder = "<%=GetResource("DeleteSubFolder") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_DelFavoriteFolder = "<%=GetResource("DelFavoriteFolder") %>";        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Folder/TreeAdmin.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/WebHard/Js/Folder/TreeAdmin.js");
    </script>   
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left"  class="left-menu">
       <ul class="left-menu-container">
         <li>
           <div id="layout_treeboxAcc">
    	        <div class="layout_left_style3 bg-secondary">
                    <div class="center p-3"><span class="h5 text-white"><%=GetResource("PublicFolderLength")%> :</span> <span class="badge badge-dark"> <span id="userTotal"></span> / <span id="userQuota"></span></span></div>
                    <div class="tree_webhard_volume">            
                            <div class="progress">
                              <div id="graphBar" class="progress-bar bg-warning " role="progressbar" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
                            </div>
                        <span class="volume_percent" id="percent"></span>
                    </div>
                </div>
                <div class="accordion">
                    <div class="menu" id="public"><a href="#"><%=GetResource("Folders")%><%=GetResource("ManagerS")%></a></div>
                    <div class="menusub">
                        <uc1:WebHardTree ID="WebHardTreeA" runat="server" UseCategory="A"></uc1:WebHardTree>
                    </div>    
                    <div class="menu" id="publicList"><a href="#"><%=GetResource("Files")%><%=GetResource("ManagerS")%></a></div>
                    <div class="menusub">
                        <uc1:WebHardTree ID="WebHardTreeL" runat="server" UseCategory="L"></uc1:WebHardTree>
                    </div>  
                </div>    
             </div>        
             
             <div id="layout_treebuttom"></div>
            
             <div id="layout_treebutton">
                 <a id="imgUDel" style="cursor: pointer" class="btn btn-sm btn-danger"><%=GetResource("Del")%></a>
                 <a id="imgUMod" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Mod")%></a>
                 <a id="imgUAdd" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Add")%></a>  
             </div> 
        </li>
       </ul>

       
    </div>  
    <div style="display: none">
    <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
    <asp:TextBox ID="tbFID" runat="server" Width="0"></asp:TextBox>
    </div>
    </form>
</body>
</html>
