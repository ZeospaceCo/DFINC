<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalendarDialog.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Common.CalendarDialog" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div style="min-height:368px;">
            <div class="introcontentblock bottomcontent" >
                <ul class="mobliecontent" >
                    <li class="poptitle text-primary">
                        <span>
                            <i class="fas dropbtn "></i><%=GetResource("Calendar")%>
                        </span>
                        <div class="float-right date text-dark">
                            <button type="button" class="btn btn-xs btn-primary" id="btStarDate" style="display:none">시작일달력선택</button>
                            <button type="button" class="btn btn-xs btn-primary" id="btEndDate">종료일달력선택</button>
                            <button type="button" class="btn btn-xs btn-primary" id="btConfirm"><%=GetResource("ConfirmZR")%></button>
                            <button type="button" class="btn btn-xs btn-primary" id="btClose"><%=GetResource("Close")%></button>
                        </div>
                    </li> 
                </ul>    
                <ul class="mobilecontent di2">
                    <li class="contentblock">   
                        <div class="container-fluid" style="text-align:center">   
                            <span id="selectStart"></span> 
                            &nbsp;~
                            <span id="selectEnd"></span>                           
                         </div> 
                    </li>
                </ul> 
                <div id="dStart">
                <ul class="mobilecontent di2">
                    <li>   
                        <div class="chh" style="text-align:left;">   
                            &nbsp;&nbsp;&nbsp;시작일 : &nbsp;&nbsp;
                            <a href="#" class="bt5 bt5pvm" title="이전달" id="pvMonthS">
                                <div><span class="hc2"><%=GetResource("BeforeMonth")%></span></div>
                            </a>
                            <asp:DropDownList ID="ddlStartYear" runat="server"></asp:DropDownList>&nbsp;- 
                            <asp:DropDownList ID="ddlStartMonth" runat="server"></asp:DropDownList>                          
                            <a href="#" class="bt5 bt5nxm" title="다음달" id="nxMonthS">
                                <div><span class="hc2"><%=GetResource("NextMonth")%></span></div>
                            </a>                      
                        </div>
                    </li>
                </ul>      
                <ul class="mobilecontent di2">
                    <li>
                        <div id="phCalStart"></div>
                    </li>
                </ul>    
                </div>
                <div id="dEnd" style="display:none">
                <ul class="mobilecontent di2">
                    <li>   
                        <div class="chh" style="text-align:left;">   
                            &nbsp;&nbsp;&nbsp;종료일 : &nbsp;&nbsp;
                            <a href="#" class="bt5 bt5pvm" title="이전달" id="pvMonthE">
                                <div><span class="hc2"><%=GetResource("BeforeMonth")%></span></div>
                            </a>
                            <asp:DropDownList ID="ddlEndYear" runat="server"></asp:DropDownList>&nbsp;-
                            <asp:DropDownList ID="ddlEndMonth" runat="server"></asp:DropDownList>
                            <span id="timeListE"></span>                                
                            <a href="#" class="bt5 bt5nxm" title="다음달" id="nxMonthE">
                                <div><span class="hc2"><%=GetResource("NextMonth")%></span></div>
                            </a>
                        </div>
                    </li>
                </ul>
                <ul class="mobilecontent di2">
                    <li>
                        <div id="phCalEnd"></div>
                    </li>
                </ul>
                </div>      
             </div>
        </div>
    </div>
</form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID %>";
        var NowYear = "<%=System.DateTime.Now.Year.ToString()%>";
        var NowMonth = "<%=System.DateTime.Now.Month.ToString()%>";
        var NowDay = "<%=System.DateTime.Now.Day.ToString()%>";
        var NowDate = "";
        var StarDate = "";
        var EndDate = "";
        var StartCalMonth = "";
        var EndCalMonth = "";
        var PreLang = "ko";
        var SelectGubun = "";
        var TmpS_ClassName = "";
        var TmpS_Choice = "";
        var TmpE_ClassName = "";
        var TmpE_Choice = "";
   
        var R_Sun = "<%=GetResource("Sun") %>";     
        var R_Mon = "<%=GetResource("Mon") %>";     
        var R_Tue = "<%=GetResource("Tue") %>";     
        var R_Wed = "<%=GetResource("Wed") %>";     
        var R_Thu = "<%=GetResource("Thu") %>";     
        var R_Fri = "<%=GetResource("Fri") %>";     
        var R_Sat = "<%=GetResource("Sat") %>";   

    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Common/CalendarDialog.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Common/CalendarDialog.js");
    </script>
</body>
</html>
