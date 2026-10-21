<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Folder.Tree" %>
<%@ Register Src="ProjectTree.ascx" TagName="ProjectTree" TagPrefix="uc1" %>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var AccordionActive = <%=AccActive %>;
        var RealFolderID = "0";
        var AccessAuth = "<%=AccessAuth%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Folder/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Folder/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
	<div id="layout_left" class="left-menu">
         <ul class="left-menu-container">
            <li>
                <div id="layout_treeboxAcc">
                    <div class="accordion">
                        <div class="menu"><a href="#" onclick=""><%=GetResource("Folder")%></a></div>
                        <div class="menusub">
                            <uc1:ProjectTree id="UCProjectTree1" UseIcon="true" runat="server"></uc1:ProjectTree>
                        </div>
                        <div class="menu"><a href="#" id="CompletePRJ"><%=GetResource("Module_Node4")%></a></div>
                    </div>    
                </div>
        
                <div id="layout_treebuttom"></div>
                
                <div id="layout_treebutton" style="display:none">
                    <a id="imgDel" style="cursor: pointer" class="btn btn-sm btn-danger"><%=GetResource("FileDel")%></a>
                    <a id="imgMod" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Mod")%></a>
                    <a id="imgAdd" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Add")%></a>                      
                </div>
            </li>
          </ul>
    </div>
    </form>
</body>
</html>
