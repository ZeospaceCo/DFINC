<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Admin.Folder.Add" %>
<%@ Register Src="../../Module/Folder.ascx" TagName="FolderTree" TagPrefix="uc1" %>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var MultiLang = "<%=MultiLang %>";
        var FOLDER_Register_MSGName = "<%=GetResource("FOLDER_Register_MSGName")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Admin/Folder/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Admin/Folder/Add.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <h1><%=GetResource("Module_Folder_Node1")%></h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li><cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="CbtnMod" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="CbtnDel" runat="server" Class="btn btn-danger"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:330px;">
                <table class="table_typeB">
			    <caption></caption>
			    <colgroup>
                    <col width="80px;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("FolderName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                        <td>
                        <cc3:ClientSmartTextBox ID="tbxFolderName" runat="server" CssStyle="width:100px !important;" TitleAtt=""></cc3:ClientSmartTextBox>
                        <cc3:ClientSmartTextBox ID="CTBMultiName" runat="server" CssStyle="width:200px !important;" TitleAtt=""></cc3:ClientSmartTextBox>
                        </td>
                    </tr>
                </tbody>
                </table>

                <div class="layout_scroll" style="height:280px;margin:10px 25px 15px 1px;padding:10px;outline:1px solid rgba(0,0,0,.1);" >
                    <uc1:FolderTree ID="FolderTree1" runat="server" UseIcon="true"></uc1:FolderTree>
                </div>
                
            </div>                
        </div>
    </div>   
    <div style="display: none">
        <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
        <asp:TextBox ID="tbxFolderID" runat="server" Width="0"></asp:TextBox>
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
</body>
</html>
