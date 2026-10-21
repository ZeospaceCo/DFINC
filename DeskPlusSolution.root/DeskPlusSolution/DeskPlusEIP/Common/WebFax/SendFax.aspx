<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendFax.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.WebFax.SendFax" %>


<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
   <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
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
        var RowMax = 0;
        var OwnerID = "<%=OwnerID %>";
        var Gubun = "<%=Gubun %>";

        var R_FaxIsSendFaxRecv = "<%=GetResource("FaxIsSendFaxRecv") %>";
        var R_FaxInputFaxRecvUser = "<%=GetResource("FaxInputFaxRecvUser") %>";
        var R_FaxInputFaxNumber = "<%=GetResource("FaxInputFaxNumber") %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

    </script>
    <script type="text/javascript">
        JsCall("../Js/WebFax/SendFax.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <!-- 페이지타이틀 //-->
                <h1>팩스 수신자 추가</h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnSendFax" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content"  style="height: 615px;">
                <table class="table_typeB">
                    <caption>
                        팩스 내용</caption>
                    <colgroup>
                        <col width="80px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                제목
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="cbtnSubject" Required="true" Width="98%"
                                    InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>                
                <div class="layout_subbox">
                    <h2>팩스 수신자 추가</h2>
                    <ul class="btn_right">                
                        <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" />
                    </ul>
                </div>
                <table class="table_typeB">
                    <caption>
                        팩스 수신자 추가</caption>
                    <colgroup>
                        <col width="80px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                회사명
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbCompanyName" Required="true" Width="98%"
                                    InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                담당자
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbName" InputMode="None" class="searchtext"
                                    Width="98%"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                팩스번호
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbFaxNumber" Width="50%" InputMode="None"
                                    class="searchtext"></cc3:ClientSmartTextBox> (ex 국내: 02-2108-3080 / 중국 : 86-10-1234-4567)
                            </td>
                        </tr>
                    </tbody>
                </table>

                <div class="layout_subbox">                    
                    <h2>팩스 수신자 리스트 (<span style="color:Red"> * 수신자 명단이 없으면 FAX 전송 안됩니다.</span>)</h2>
                    <ul class="btn_right">                
                        <cc3:ClientSmartButton ID="cbtnAddress" runat="server" BeginEndTag="li" />
                    </ul>
                </div>

                
                                
                <div id="scrollDiv">
                <table class="table_typeA" id="contentTable">
                    <caption>
                        추가</caption>
                    <colgroup>
                        <col />
                        <col />
                        <col />
                        <col width="80px" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th class="center">
                                회사명 
                            </th>
                            <th class="center">
                                담당자 
                            </th>
                            <th class="center">
                                팩스번호
                            </th>
                            <th class="center">
                                삭제
                            </th>
                        </tr>                        
                    </tbody>
                </table>                                
                       
                </div>
                         
            </div>            
        </div>
    </div>
    <div style="display: none;">
        <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
        <asp:TextBox ID="hdnFaxUser" runat="server" Width="0"></asp:TextBox>
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
</body>
</html>
