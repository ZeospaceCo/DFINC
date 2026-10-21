<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Admin.Source.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("SourceSetting")%></h1>
            </div>
            <div id="layout_content">
                <div class="row">
                   <div class="col-12 p-0 pt-4"><!-- width 100%(col-12) -->
                        <ul class="btn_right">
                            <cc3:clientsmartbutton id="CbtnMove" runat="server" beginendtag="li" class="btn btn-secondary"/>
                        </ul>

                        <div style="clear: both;" class="gap"></div>

                        <table class="table_typeB">
                            <caption><%=GetResource("Category")%></caption>
                            <colgroup>
                                <col width="100px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th><%=GetResource("IsSource")%></th>
                                    <td>
                                        <span class="aLeft"><cc3:clientsmarttextbox id="ctbParentName" required="true" maxlength="1000" runat="server"></cc3:clientsmarttextbox>&nbsp;</span>
                                        <ul class="btn_left">
                                            <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-sm btn-danger" /></li>
                                            <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-sm btn-secondary" /></li>
                                            <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-sm btn-secondary" /></li>
                                        </ul>
                                    </td>
                                </tr>
                                <tr>
                                    <th><label><%=GetResource("MutiName")%></label>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="add" /></a></th>
                                    <td><cc3:clientsmarttextbox id="ctbMenuName" required="true" maxlength="100" runat="server" cssstyle="width:95% !important;"></cc3:clientsmarttextbox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Active")%></th>
                                    <td><asp:CheckBox ID="chkActive" runat="server" Checked="true" /></td>
                                </tr>
                            </tbody>
                        </table>
                        <div style="clear: both; padding: 10px 0px 0px 0px;"></div>
                        <div class="mid">
                            <div id="phSourceList" style="clear: both;"></div>
                        </div>
                   </div><!-- end col content-area-body-right -->
                </div>
            </div>
        </div>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var SourceID = "";
        var MultiLang = "<%=MultiLang %>";
        var R_DelGubun = "<%=GetResource("ApplyDelete") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_Active = "<%=GetResource("Active") %>";
        var R_NonActive = "<%=GetResource("NonActive") %>";
        var R_UsedYN = "<%=GetResource("UsedYN") %>";
        var R_IsSource = "<%=GetResource("IsSource") %>";
        var R_CheckDeleteList = "<%=GetResource("CheckDeleteList") %>";
        var R_SourceName = "<%=GetResource("SourceName") %>"
        var R_NoListData = "<%=GetResource("NoListData") %>"
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Admin/Source/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Admin/Source/List.js");
    </script>
</body>
</html>