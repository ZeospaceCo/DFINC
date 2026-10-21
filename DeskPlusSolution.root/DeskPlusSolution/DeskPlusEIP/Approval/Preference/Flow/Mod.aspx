<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Preference.Flow.Mod" %>
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
                    <h1><%=GetResource("SeqLineTit")%></h1>
                    <span class="app_caption">* <%=GetResource("SeqLineAddMsg")%></span>
                    <!-- 버튼 //-->
                    <ul class="btn_right">
                        <li><a href="#" onclick="Mod.Confirm()" class="btn btn-secondary"><span><%=GetResource("Btn_Save")%></span></a></li>                      
                        <li><a href="#" onclick="Mod.Close()" class="btn"><span><%=GetResource("Cancel")%></span></a></li>                      
                    </ul>
                </div>

                <div id="layout_content">
                    <div class="row">
                        <div class="col-12 content-area-body">
                            <!-- 이름.기타정보 //-->
                            <table class="table_typeB">
                                <caption></caption>
                                <colgroup>
                                    <col width="100px" />
                                    <col />
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <th class="center"><%=GetResource("SeqLineSubject")%></th>
                                        <td><cc3:ClientSmartTextBox ID="ctbSubject" InputMode="None" runat="server" Required="true" TextMode="SingleLine" Width="95%"></cc3:ClientSmartTextBox></td>
                                    </tr>
                                    <tr>
                                        <th class="center"><%=GetResource("SeqLineMemo")%></th>
                                        <td><cc3:ClientSmartTextBox ID="ctbMemo" InputMode="None" runat="server" Required="true" TextMode="SingleLine" Width="95%"></cc3:ClientSmartTextBox></td>
                                    </tr>
                                </tbody>
                            </table>

                            <div class="gap5"></div>

                            <!-- 일반결재,합의결재,협조결재 //-->
                            <div class="layout_subbox">
                                <h2 style="display:inline;"><%=GetResource("FavTitLineZD")%></h2>
                                <ul class="btn_right">
                                    <li><a onclick="Mod.ModSeq('ZD')" class="btn"><span><%=GetResource("ChangeOrder3")%></span></a></li>
                                    <li><a onclick="Mod.ModTag('ZD')" class="btn"><span><%=GetResource("SeqLineTagZD")%></span></a></li>
                                    <li><a onclick="Mod.ModTag('ZA')" class="btn"><span><%=GetResource("SeqLineTagZA")%></span></a></li>
                                    <li><a onclick="Mod.ModTag('ZH')" class="btn"><span><%=GetResource("SeqLineTagZH")%></span></a></li>
                                </ul>
                            </div>
                    
                            <div id="placeholder_ZD" style="clear: both;"></div>
                    
                            <span class="app_captit">* <%=GetResource("MsgFavSeqInfo0")%></span>

                            <div class="gap5"></div>

                            <!-- 수신결재 //-->
                            <div class="layout_subbox">
                                <h2 style="display:inline;"><%=GetResource("FavTitLineZP")%></h2>
                                <ul class="btn_right">
                                    <li><a onclick="Mod.ModSeq('ZP')" class="btn"><span><%=GetResource("ChangeOrder3")%></span></a></li>
                                    <li><a onclick="Mod.ModTag('ZP')" class="btn"><span><%=GetResource("SeqLineTagZP")%></span></a></li>
                                    <li><a onclick="Mod.ModTag('ZQ')" class="btn"><span><%=GetResource("SeqLineTagZQ")%></span></a></li>
                                </ul>
                            </div>
                    
                            <div id="placeholder_ZP" style="clear: both;"></div>
                    
                            <span class="app_captit">* <%=GetResource("MsgFavSeqInfo1")%></span>

                            <div class="gap5"></div>

                            <!-- 참조결재 //-->
                            <div class="layout_subbox">
                                <h2 style="display:inline;"><%=GetResource("FavTitLineZC")%></h2>
                                <ul class="btn_right">
                                    <li><a onclick="Mod.ModSeq('ZC')" class="btn"><span><%=GetResource("ChangeOrder3")%></span></a></li>
                                    <li><a onclick="Mod.ModTag('ZC')" class="btn"><span><%=GetResource("SeqLineTagZC")%></span></a></li>
                                </ul>
                            </div>
                    
                            <div id="placeholder_ZC" style="clear: both;"></div>
                    
                            <span class="app_captit">* <%=GetResource("MsgFavSeqInfo2")%></span>
                        </div><!-- end col-3 content-area-body-right -->
                
                    </div><!-- end row  -->
                </div>

            </div>
        </div>
        <div style="display:none">
            
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬

        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var FavSeqID = "<%=FavSeqID %>";
        var PreLang = "<%=PreLang%>";

        var R_Number = "<%=GetResource("Number") %>";
        var R_FavTag = "<%=GetResource("FavTag") %>";
        var R_FavSec = "<%=GetResource("FavSec") %>";
        var R_Del = "<%=GetResource("DelItem") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_TitleName = "<%=GetResource("TitleName") %>";
        var R_DutyName = "<%=GetResource("DutyName") %>";
        var R_UserName = "<%=GetResource("Name") %>";
        var R_NoData = "<%=GetResource("NoContent") %>"; 
        var R_Msg_Del = "<%=GetResource("Msg_Del") %>"; 
        var R_Msg_ErrSubj = "<%=GetResource("Msg_ErrSubj") %>";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>";
        var R_MSG_FAIL = "<%=GetResource("MSG_FAIL") %>";
        var R_MSG_AlertIsSub = "<%=GetResource("MSG_AlertIsSub") %>";
        var R_MSG_AlertProcess = "<%=GetResource("MSG_AlertProcess") %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   

        var RowMax = 0;

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Preference/Flow/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Preference/Flow/Mod.js");
    </script>

</body>
</html>