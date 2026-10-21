<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthList.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Content.AuthList" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <h1>
                    <%=GetResource("AuthManage")%></h1>
                <ul class="btn_right">
                    <li>
                        <div id="loading">
                        </div>
                    </li>
                    <cc3:ClientSmartButton ID="CbtnMod" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                </ul>
                <!--<ul class="tab">
                    <li><a href="#" class="tab_on" id="tabDivInfo"><span>
                        <%=GetResource("Auth")%></span></a></li>
                </ul>-->
            </div>
            <div id="layout_content"><!-- 컨텐츠 하단영역 -->
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    
                    
                <div id="divAuth">
                    <!-- 권한설정 start-->
                    <div class="acc_auth_left">
                        <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px;overflow:hidden">
                            <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true"  UseCheckBoxGroup="false" UseCheckBoxMember="true" UseIcon="true" />
                        </div>
                    </div>
                    <div class="acc_auth_middle">
                        <ul>
                            <li>
                                <cc3:ClientSmartButton ID="CbtnAdd" runat="server"  class="btn btn-secondary"></cc3:ClientSmartButton>
                            </li>
                        </ul>
                    </div>
                    <div class="acc_auth_right">
                        <table class="table_typeB" id="contentTable">
                            <caption>
                                <%=GetResource("Auth")%></caption>
                            <colgroup>
                                <col />
                                <col width="150px" />
                                <col width="80px" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th class="center"><%=GetResource("Naming")%></th>
                                    <th class="center"><%=GetResource("Auth")%></th>
                                    <th class="center"><%=GetResource("Del")%></th>
                                </tr>
                                <%=AuthTable%>
                            </tbody>
                        </table>
                    </div>
                    <!-- 권한설정 end-->
                </div>
                        
                        
                        
                    </div><!-- end col content-area-body-right -->
                </div><!-- end row  -->
            </div><!-- end layout_content  -->
    
        </div><!-- end layout_full  -->
    </div><!-- end layout_body  -->
    <div style="display: none">
        <input name="tvSelectedID" type="text" id="tvSelectedID" />
        <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag">
            <p>
            </p>
        </div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post">
        </form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank">
        </iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var AuthArrayList = "<%=AuthArrayList %>";
        var RowMax = <%=MaxRowMax %>;
        var R_SettingAuthority = "<%=GetResource("SettingAuthority") %>";
        var R_ShareUser = "<%=GetResource("ShareUser") %>";
        var R_ReportUser = "<%=GetResource("ReportUser") %>";
        var R_CheckAuth = "<%=GetResource("CheckAuth") %>";
        var R_AuthOnlyOne = "<%=GetResource("AuthOnlyOne") %>";
        var R_Processing = "<%=GetResource("Processing") %>";
        var R_Del = "<%=GetResource("Del") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Content/AuthList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Report/Js/Content/AuthList.js");
    </script>
</body>
</html>