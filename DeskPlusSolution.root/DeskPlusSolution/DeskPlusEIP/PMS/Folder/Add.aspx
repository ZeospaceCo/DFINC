<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Folder.Add" %>
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
            <h1><asp:Literal ID="ltHeader" runat="server"></asp:Literal></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
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
                    </colgroup>
                    <tbody>
                    <tr>
                        <th><%=GetResource("ParentFolderName")%></th>
                        <td><cc3:ClientSmartTextBox ID="CTBParentName" ReadOnly="true" MaxLength="250" runat="server" CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox></td>          
                    </tr>
                    <tr>
                        <th><%=GetResource("FolderName")%></th>
                        <td><cc3:ClientSmartTextBox ID="CTBName" InputMode="None" MaxLength="250" runat="server" Required="true"  CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox></td>
                    </tr> 
                    <tr>
                        <th><%=GetResource("MultiName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="" /></a></th>
                        <td><cc3:ClientSmartTextBox ID="CTBMultiName" InputMode="None" MaxLength="250" runat="server" Required="true"  CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox></td>
                    </tr>                  
                    </tbody>
                    </table>
                  </div>
             </div>        
        </div>    
    </div>
    </div>    
    <div style="display:none;">
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton>        
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
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var MultiLang = "<%=MultiLang %>";
        var R_InputFolderName = "<%=GetResource("InputFolderName") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Folder/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Folder/Add.js");
    </script>
</body>
</html>
