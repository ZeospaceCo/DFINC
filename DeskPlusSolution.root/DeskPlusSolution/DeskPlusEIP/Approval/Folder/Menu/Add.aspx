<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Menu.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>
    <form id="form1" runat="server">


<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1>
                <span><%=GetResource("AddFolder")%></span>
            </h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><a href="" title="" class="btn"><%=GetResource("EditUpFolder")%></a></li>
                <li><a href="" title="" class="btn"><%=GetResource("Orders")%></a></li>
                <li><a href="javascript:history.back()" title="" class="btn btn-secondary"><%=GetResource("Confirm")%></a></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
                <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <table class="table_typeB">
                    <caption><%=GetResource("DocInfo")%></caption>
			        <colgroup>
			            <col width="80px" />
				        <col />
                        <col width="80px" />
				        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th><%=GetResource("UpFolder")%></th>
                            <td><%=GetResource("UpperFolderName")%></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("FolderName")%></th>
                            <td><input /></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("MultiLang")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" /></a></th>
                            <td><input /></td>
                        </tr>
                    </tbody>
                    </table>
                </div><!-- end col-3 content-area-body-right -->
            </div><!-- end row  -->    
        </div>

    </div>
</div>




    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Menu/Add.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Menu/Add.js");
    </script>

</body>
</html>