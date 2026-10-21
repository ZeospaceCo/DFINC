<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.MyInfo.Etc.Summary" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("SummaryTit1")%></h1>
                <ul class="btn_right">                
                    <cc3:ClientSmartButton ID="cbtnUser" runat="server" BeginEndTag="li" />
                    <cc3:clientsmartbutton id="cbtnList" runat="server" beginendtag="li"/>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <div class="col-12" ><!-- width 100%(col-12) -->    
                    <!-- Tab //-->
                    <ul id="tab" class="tab">
                        <li><a href="#" id="log"><span><%=GetResource("SummaryTit2")%></span> </a></li>
                        <li><a href="#" id="aut"><span><%=GetResource("SummaryTit3")%></span> </a></li>
                        <li><a href="#" id="sum" class="tab_on"><span><%=GetResource("SummaryTit4")%></span> </a></li>
                    </ul>

                    <!-- Contents //-->
                    <div style="min-height:510px;height:510px;overflow:auto;">

                        <h2><%=GetResource("SummaryTit5")%><span id="btnapp" class="summary"><%=GetResource("SummaryTit6")%></span></h2>
                        <table class="table_typeB">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tbody>
                        <tr>
                            <th><%=GetResource("SummaryTit7")%></th>
                            <td><span id="apping"></span></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("SummaryTit8")%></th>
                            <td><span id="appcom"></span></td>
                        </tr>
                        </tbody>
                        </table>
                
                        <div class="layout_subbox"><h2><%=GetResource("SummaryTit9")%><span id="btnboa" class="summary"><%=GetResource("SummaryTit6")%></span></h2></div>
                        <table class="table_typeB">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tbody>
                        <tr>
                            <th><%=GetResource("SummaryTit10")%></th>
                            <td><span id="boacnt"></span></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("SummaryTit11")%></th>
                            <td><span id="boanm"></span></td>
                        </tr>
                        </tbody>
                        </table>

                        <div class="layout_subbox"><h2><%=GetResource("SummaryTit12")%><span id="btncomm" class="summary"><%=GetResource("SummaryTit6")%></span></h2></div>
                        <table class="table_typeB">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tbody>
                        <tr>
                            <th><%=GetResource("SummaryTit13")%></th>
                            <td><span id="cmmcnt"></span></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("SummaryTit14")%></th>
                            <td><span id="cmmmgr"></span></td>
                        </tr>
                        </tbody>
                        </table>

                        <div class="layout_subbox"><h2><%=GetResource("SummaryTit15")%><span id="btntask" class="summary"><%=GetResource("SummaryTit6")%></span></h2></div>
                        <table class="table_typeB">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tbody>
                        <tr>
                            <th><%=GetResource("SummaryTit16")%></th>
                            <td><span id="taskcnt"></span></td>
                        </tr>
                        </tbody>
                        </table>
                    </div>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";
        var DeptID = "<%=DeptID %>";
        var IsAdmin = "<%=IsAdmin %>";

        var R_WorkPeriod = "<%=GetResource("WorkPeriod")%>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var PreLang = "<%=PreLangReal %>"; //ja
    </script>
    <!-- script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal %>.js"></script//-->
    <!-- script type="text/javascript" src="/DeskPlusEIP/Account/Js/MyInfo/Etc/Summary.js"></script //-->
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        JsCall("/DeskPlusEIP/Account/Js/MyInfo/Etc/Summary.js");
    </script>
</body>
</html>