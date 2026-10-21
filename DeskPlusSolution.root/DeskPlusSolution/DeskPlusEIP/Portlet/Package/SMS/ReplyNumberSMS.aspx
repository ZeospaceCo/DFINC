<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReplyNumberSMS.aspx.cs"
    Inherits="Zeospace.DeskPlus.Portlet.Web.Package.SMS.ReplyNumberSMS" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <link href="<%=CssPath%>/DeskPlusBase.css" type="text/css" rel="stylesheet" />
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";

        var AddReplyPhoneNumber = "회신번호를 추가해주세요.";
        var DefaultReplyPhoneNumber = "기본회신번호";
        var SettingDefaultReplyPhoneNumber = "기본회신번호가 설정되었습니다.";
        var NoDelPhoneNumber = "더이상 삭제할 번호가 없습니다.";
        var InputReplyNumber = "회신번호를 입력해주세요.";    
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Portlet/Js/Package/SMS/ReplyNumberSMS.js");
    </script>
</head>
<body>
    <form id="Frm" name="Frm" method="post" runat="server">

  <div class="layer_popup">
    <div class="inside">
        
        <div class="popup_header">
            <h1>회신번호 설정</h1>
            <ul class="btn_right">
                <li><a href="#" id="CbtnClose" title="닫기"><span>닫기</span></a></li>
            </ul>
        </div>        
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="overflow:hidden !Important;">
        <div class="layout_scroll" style="min-height:320px;height:320px;">
        	<!-- 테이블 시작 -->            
            <div>
                <strong>회신번호를 추가/삭제 합니다.</strong><br>
                <br>
                자주 쓰는 회신번호를 등록해 두시면 매번 전송시에 번거로운 회신번호 입력이 필요 없습니다. 고객님의 기본 회신번호를 지정할 수 있습니다.
            </div>
            <div style="min-height:30px;height:30px;"></div>

            <table height="34" cellspacing="0" cellpadding="0" width="400" align="center" border="0">
                <tr>
                    <td align="right" width="132">
                        추가할 회신번호 입력 :&nbsp;
                    </td>
                    <td width="164" height="23">
                        <input class="input_join" onkeypress="onlyNum();" style="ime-mode: disabled" maxlength="3" size="4" name="Req1" id="Req1">-
                        <input class="input_join" onkeypress="onlyNum();" style="ime-mode: disabled" maxlength="4" size="4" name="Req2" id="Req2">-
                        <input class="input_join" onkeypress="onlyNum();member_ReqCHK_enter(event.keyCode);" style="ime-mode: disabled" maxlength="4" size="4" name="Req3" id="Req3">
                    </td>
                    <td width="104">
                        <a href="javascript:member_ReqCHK();">
                            <img src="<%=BaseImgPath %>/SMS/btn_add.gif" border="0" name="rbtn"></a>
                    </td>
                </tr>
            </table>

            <table cellspacing="0" cellpadding="0" width="400" align="center" border="3">
                <tr>
                    <td valign="top" align="right" width="131" rowspan="2">
                        현재 회신번호 목록 :&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td valign="top" colspan="2" height="120" rowspan="2">
                        <asp:ListBox ID="ReqList" runat="server" size="3" ondblclick="member_DelCHK();" Style="width: 149px;
                            height: 120px; font-size: 9pt; border: 1px; border-style: solid 1; color: #333333;">
                        </asp:ListBox>
                    </td>
                    <td width="100" height="18">
                        <a href="javascript:DelReply();"><img src="<%=BaseImgPath %>/SMS/btn_del.gif" border="0"></a>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 4px" valign="top" width="100" height="82">
                        <a href="javascript:member_Req_Set();"><img src="<%=BaseImgPath %>/SMS/btn_set.gif" border="0"></a>&nbsp;<br>
                        &nbsp; <span id="span_req" style="height: 21px"></span>
                    </td>
                </tr>
            </table>
            <!-- 테이블 끝 -->

        </div>
        </div>

    </div>
</div>





    <table cellpadding="0" cellspacing="0" border="0" id="table1">
        <tr>
            <td height="15">
            </td>
        </tr>
        <tr>
            <td style="padding-left: 15px" height="50">
                
            </td>
        </tr>
        <tr>
            <td height="15">
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" align="center" border="0" id="table4">
                    <tr>
                        <td colspan="2" height="140">
                            <table height="230" cellspacing="0" cellpadding="0" border="0" id="table5">
                                <tr>
                                    <td width="5" height="5">
                                        <img src="<%=BaseImgPath %>/SMS/ggbg_ltc.gif">
                                    </td>
                                    <td class="td_gray" width="400">
                                    </td>
                                    <td width="5">
                                        <img src="<%=BaseImgPath %>/SMS/ggbg_rtc.gif">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_gray" colspan="3">
                                        <table height="40" cellspacing="0" cellpadding="0" width="400" align="center" border="0"
                                            id="table6">
                                            <tr>
                                                <td width="5" height="5">
                                                    <img src="<%=BaseImgPath %>/SMS/whitebg_c01.gif">
                                                </td>
                                                <td width="300" bgcolor="#ffffff">
                                                </td>
                                                <td width="5">
                                                    <img src="<%=BaseImgPath %>/SMS/whitebg_c02.gif">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#ffffff" colspan="3" height="30">
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="5" height="5">
                                                    <img src="<%=BaseImgPath %>/SMS/whitebg_c03.gif">
                                                </td>
                                                <td width="470" bgcolor="#ffffff">
                                                </td>
                                                <td width="5">
                                                    <img src="<%=BaseImgPath %>/SMS/whitebg_c04.gif">
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellspacing="0" cellpadding="0" width="400" align="center" border="0" id="table8">
                                            <tr>
                                                <td height="15">
                                                </td>
                                            </tr>
                                        </table>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td width="5" height="5">
                                        <img src="<%=BaseImgPath %>/SMS/ggbg_lbc.gif">
                                    </td>
                                    <td class="td_gray">
                                    </td>
                                    <td width="5">
                                        <img src="<%=BaseImgPath %>/SMS/ggbg_rbc.gif">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" width="500" align="center" border="0" id="table11">
                    <tr>
                        <td height="15">
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" width="500" align="center" border="0" id="table12">
                    <tr>
                        <td align="center">
                            <a href="javascript:self.close()">
                                <img src="<%=BaseImgPath %>/SMS/btn_cancel.gif"></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <iframe id="ifrm" name="ifrm" src="about:blank" frameborder="0" width="0" height="0">
    </iframe>
    </form>
</body>
</html>
