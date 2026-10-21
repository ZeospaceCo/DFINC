<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Preference.Flow.Add" %>
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
                    <span class="app_caption">* <%=GetResource("FlowMsg01")%></span>
                    <!-- 버튼 //-->
                    <ul class="btn_right">
                        <li><a href="#" onclick="Add.Confirm()" class="btn btn-secondary"><span><%=GetResource("Btn_Save")%></span></a></li>                      
                        <li><a href="#" onclick="Add.Close()" class="btn"><span><%=GetResource("Cancel")%></span></a></li>                      
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
                        </div><!-- end col-12 content-area-body -->
                    </div><!-- end row  -->
                </div>

            </div>
        </div>
        <div style="display:none">
            <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
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
        var PreLang = "<%=PreLang%>";

        var R_Msg_ErrSubj = "<%=GetResource("Msg_ErrSubj") %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   

        var RowMax = 0;

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Preference/Flow/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Preference/Flow/Add.js");
    </script>

</body>
</html>