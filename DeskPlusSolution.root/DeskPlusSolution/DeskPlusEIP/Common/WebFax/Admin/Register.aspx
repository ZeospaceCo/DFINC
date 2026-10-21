<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.WebFax.Admin.Register" %>


<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var FaxID = "<%=FaxID %>";

        var SearchKey = "<%=SearchKey %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지        
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>";
        var Gubun = "<%=Gubun %>";
        var MultiLang = "<%=MultiLang %>";

        var R_FaxReallyDel = "<%=GetResource("FaxReallyDel") %>";
        var R_FaxRegisterFaxGroupName = "<%=GetResource("FaxRegisterFaxGroupName") %>";

    </script>
    <script type="text/javascript">
        JsCall("../../Js/WebFax/Admin/Register.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("List")%></h1>

                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" Class="btn btn-danger" />
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
				  <div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->
                        <table class="table_typeB">
                            <caption>
                                <%=GetResource("List")%></caption>
                            <colgroup>
                                <col width="80px" />
                                <col />
                            </colgroup>
                            <tbody>                        
                                <tr>
                                    <th><%=GetResource("FaxGroupName")%></th>
                                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbName"  Required="true" Style="width: 95% !important;" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th>다국어&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" /></a></th>
                                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbMuliLang" Style="width: 95% !important;" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("FaxNumber")%></th>
                                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbFaxNumber" Style="width: 95% !important;" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                </tr> 
                                <tr>
                                    <th><%=GetResource("FaxBranchName")%>&nbsp;<a id="DeptLink"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" /></a></th>
                                    <td><asp:DropDownList ID="ddlBranch" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                                </tr>                       
                                <tr>
                                    <th><%=GetResource("FaxIsActive")%></th>
                                    <td><asp:DropDownList ID="ddlActive" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                                </tr>
                            </tbody>
                        </table>   
                      </div>
                    </div>
            </div>
        </div>
    </div>
    <div style="display: none;">        
        <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
</body>
</html>
