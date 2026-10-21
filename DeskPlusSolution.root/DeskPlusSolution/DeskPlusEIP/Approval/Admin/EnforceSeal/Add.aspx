<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.EnforceSeal.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="layout_body">
            <div class="layout_full">
                <div id="layout_header">
                    <!-- 페이지타이틀 //-->
                    <h1><%=GetResource("EnforceStampAdd")%></h1>
                    <span class="app_caption">* <%=GetResource("EnforceMsg02")%></span>
                    <!-- 버튼 //-->
                    <ul class="btn_right">
                        <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                        <cc3:ClientSmartButton ID="CbtnCancel" runat="server" BeginEndTag="li" />
                    </ul>
                </div>

                <div id="layout_content">
                    <div class="row">
                        <div class="col-12 content-area-body pt-0 mt-4"><!-- width 100%(col-12) -->    
                            <!-- 이름.기타정보 //-->
                            <table class="table_typeB">
                            <caption><%=GetResource("EnforceMsg03")%></caption>
                            <colgroup>
                                <col width="120px" />
                                <col width="120px" />
				                <col />
                                <col width="120px" />
                                <col />
                            </colgroup>
			                <tbody>
                                <tr>
                                    <td rowspan="9">
                                        <ul class="app_space">
                                            <li><div id="stampImg" class="acc_sealimg"><%=StampImage %></div></li>
                                            <li class="block">
                                                <ul class="btn_smallL">
                                                    <li><a href="#" id="stampAdd" class="btn btn-sm" ><%=GetResource("Add")%></a></li>
                                                    <li><a href="#" id="stampDel" class="btn btn-sm" ><%=GetResource("Del")%></a></li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </td>
				                    <th><%=GetResource("EnforceSubTit01")%></th>
					                <td colspan="3">
                                        <asp:DropDownList ID="ddlDept" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                                        <span id="spMsg" class="app_subtxt" style="display:">* <%=GetResource("EnforceMsg00")%></span>
                                    </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceSubTit02")%></th>
					                <td colspan="3">
                                        <cc3:ClientSmartTextBox ID="ctbExpFirstBase" runat="server" InputMode="None" CssStyle="width:40% !important;"></cc3:ClientSmartTextBox>
                                        <cc3:ClientSmartTextBox ID="ctbExpMidBase" runat="server" InputMode="None" CssStyle="width:35% !important; padding-right:5px;" Required="true"></cc3:ClientSmartTextBox>
                                        <cc3:ClientSmartTextBox ID="ctbExpEndBase" runat="server" InputMode="None" CssStyle="width:20% !important;" Required="true"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceSubTit02")%>(<%=GetResource("MultiLang")%>) <a id="exp"><img src="/DeskPlusEIP/Design/C/Img/Icon/ico_add.gif" alt="" /></a></th>
					                <td colspan="3">
                                        <cc3:ClientSmartTextBox ID="tbExpFirstMulti" runat="server" InputMode="None" CssStyle="width:40% !important;" class="searchtext"></cc3:ClientSmartTextBox>
                                        <cc3:ClientSmartTextBox ID="tbExpMidMulti" runat="server" InputMode="None" CssStyle="width:35% !important;" class="searchtext"></cc3:ClientSmartTextBox>
                                        <cc3:ClientSmartTextBox ID="tbExpEndMulti" runat="server" InputMode="None" CssStyle="width:20% !important;" class="searchtext"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("ZipCode")%> <a id="zipcode"><img src="/DeskPlusEIP/Design/C/Img/Icon/ico_add.gif" alt="" /></a></th>
                                    <td colspan="3">
                                        <cc3:ClientSmartTextBox ID="ctbZipCode" runat="server" InputMode="None" CssStyle="width:60px !important;" Required="true"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
				                    <th><%=GetResource("EnforceSubTit03")%></th>
					                <td colspan="3">
                                        <cc3:ClientSmartTextBox ID="ctbAddressFirstBase" runat="server" InputMode="None" CssStyle="width:45% !important;" Required="true"></cc3:ClientSmartTextBox>
                                        <cc3:ClientSmartTextBox ID="ctbAddressEndBase" runat="server" InputMode="None" CssStyle="width:45% !important;" Required="true"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceSubTit03")%>(<%=GetResource("MultiLang")%>) <a id="address"><img src="/DeskPlusEIP/Design/C/Img/Icon/ico_add.gif" alt="" /></a></th>
					                <td colspan="3">
                                        <cc3:ClientSmartTextBox ID="tbAddressFirstMulti" runat="server" InputMode="None" CssStyle="width:45% !important;" class="searchtext"></cc3:ClientSmartTextBox>
                                        <cc3:ClientSmartTextBox ID="tbAddressEndMulti" runat="server" InputMode="None" CssStyle="width:45% !important;" class="searchtext"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
				                    <th><%=GetResource("EnforceSubTit04")%></th>
					                <td>
                                        <cc3:ClientSmartTextBox ID="ctbPhoneBase" runat="server" InputMode="None" CssStyle="width:120px !important;" Required="true"></cc3:ClientSmartTextBox>&nbsp;
                                        <span class="app_subtxt">(ex)02-1234-5678</span>
                                    </td>
                                    <th><%=GetResource("EnforceSubTit04")%>(<%=GetResource("MultiLang")%>) <a id="phone"><img src="/DeskPlusEIP/Design/C/Img/Icon/ico_add.gif" alt="" /></a></th>
					                <td>
                                        <cc3:ClientSmartTextBox ID="ctbPhoneMulti" runat="server" InputMode="None" CssStyle="width:98% !important;" class="searchtext"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
				                    <th><%=GetResource("EnforceSubTit05")%></th>
					                <td>
                                        <cc3:ClientSmartTextBox ID="ctbFaxBase" runat="server" InputMode="None" CssStyle="width:120px !important;" Required="true"></cc3:ClientSmartTextBox>&nbsp;
                                        <span class="app_subtxt">(ex)02-1234-5678</span>
                                    </td>
                                    <th><%=GetResource("EnforceSubTit05")%>(<%=GetResource("MultiLang")%>) <a id="fax"><img src="/DeskPlusEIP/Design/C/Img/Icon/ico_add.gif" alt="" /></a></th>
					                <td>
                                        <cc3:ClientSmartTextBox ID="ctbFaxMulti" runat="server" InputMode="None" CssStyle="width:98% !important;" class="searchtext"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
				                    <th><%=GetResource("EnforceSubTit06")%></th>
					                <td colspan="3">
                                        <cc3:ClientSmartTextBox ID="ctbHomepage" runat="server" InputMode="None" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                            </tbody>
			                </table> 
                        </div><!-- end col content-area-body-right -->
                    </div><!-- end row  -->
                </div>

            </div>
        </div>
        <div style="display:none">
            <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
        </div>


<div style="display:none;">

    <asp:TextBox ID="tbFileName" runat="server" Width="0"></asp:TextBox>

    <asp:TextBox ID="tbExpFirstMultiHidden" runat="server" Width="0" Text=""></asp:TextBox>
    <asp:TextBox ID="tbExpMidMultiHidden" runat="server" Width="0" Text=""></asp:TextBox>
    <asp:TextBox ID="tbExpEndMultiHidden" runat="server" Width="0" Text=""></asp:TextBox>
    <asp:TextBox ID="tbAddressFirstMultiHidden" runat="server" Width="0" Text=""></asp:TextBox>
    <asp:TextBox ID="tbAddressEndMultiHidden" runat="server" Width="0" Text=""></asp:TextBox>
    <asp:TextBox ID="tbPhoneMultiHidden" runat="server" Width="0" Text=""></asp:TextBox>
    <asp:TextBox ID="tbFaxMultiHidden" runat="server" Width="0" Text=""></asp:TextBox>

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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.jpg;*.jpeg;*.png;*.gif";
        var FileTypeDesc = "Images (*.jpg *.jpeg *.png *.gif)";

        var CurrentPage = "<%=CurrentPage%>";
        var RecordPerPage = "<%=RecordPerPage%>";
        var SearchField = "<%=SearchField%>";
        var SearchValue = "<%=SearchValue%>";
        var SearchTerm = "<%=SearchTerm%>";
        var SearchTermValue = "<%=SearchTermValue%>";
        var Order = "<%=Order%>";

        var R_MSG_SPECIALCHAR = "<%=GetResource("MSG_Secure") %>";              //"해당 특수문자는 보안취약성 공격에 사용되므로 제한됩니다.";
        var R_MSG_EnforceMsg01 = "<%=GetResource("EnforceMsg01")%>";              //이미 등록된 계열사입니다. 
        var R_MSG_EnforceMsg04 = "<%=GetResource("EnforceMsg04")%>";              //계열사정보를 입력하세요.
        var R_MSG_EnforceMsg05 = "<%=GetResource("EnforceMsg05")%>";              //표현명을 입력하세요
        var R_MSG_EnforceMsg06 = "<%=GetResource("EnforceMsg06")%>";              //주소를 입력하세요
        var R_MSG_EnforceMsg07 = "<%=GetResource("EnforceMsg07")%>";              //전화번호를 입력하세요
        var R_MSG_EnforceMsg08 = "<%=GetResource("EnforceMsg08")%>";              //팩스번호를 입력하세요

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/EnforceSeal/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/EnforceSeal/Add.js");
    </script>
</body>
</html>
