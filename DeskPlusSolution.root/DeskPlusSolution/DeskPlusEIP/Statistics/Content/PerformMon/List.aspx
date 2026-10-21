<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Statistics.Web.Content.PerformMon.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Statistic.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Statistic.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <style>
    canvas{ 
         -moz-user-select: none; 
         -webkit-user-select: none; 
         -ms-user-select: none; 
    } 
    </style> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/js/Chart/chartJs/Chart.min.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";
        var SelTab = "<%=SelMachineName %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Statistics/Js/Content/PerformMon/List.js"></script>
</head>
<body>
    <form id="form1" runat="server">

<div id="layout_body" class="write">
    <div class="layout_one">

        <!-- 상단 = 타이블 + 버튼 //-->
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("PerformMonTitle")%></h1>
            <span class="app_subtxt" style="display:none;"> * 최근 7일간 평균 사용량 </span>
            <!-- 버튼 //-->            
            <div class="gap5"style="display:none;"></div>
            <table class="table_typeB" style="display:none;">
            <colgroup>
                <col width="130px" />
                <col />
                <col width="140px" />
                <col />
                <col width="130px" />
                <col />
                <col width="140px" />
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th>CPU</th>
                    <td class="txt_point"><asp:Label ID="lblPerformMonTotal"  runat="server"></asp:Label></td>
                    <th>Memory</th>
                    <td class="txt_point"><asp:Label ID="lblPerformMonFailTotal"  runat="server"></asp:Label></td>
                    <th>Disk</th>
                    <td class="txt_point"><asp:Label ID="lblMPerformMonTotal"  runat="server"></asp:Label></td>
                    <th>Network</th>
                    <td class="txt_point"><asp:Label ID="lblMPerformMonFailTotal"  runat="server"></asp:Label></td>
                </tr>
            </tbody>
            </table>

        </div>
        <!-- 상단 = 타이블 + 버튼 //-->        
        <div id="layout_content">
        
        <!-- 탭 //-->
        <div class="stat_tab">

             <div  class="layout_srh clearfix">
                <ul class="tab" style="margin-left:30px !important">
                    <%= TabMenu%>
                </ul>
            </div>

        </div>
        <!-- 탭 //-->

        <!-- Contents //-->
        <div class="stat_cont">
            <div style="float:left;width:50%;text-align:center;">
                <div>
                    <canvas id="lineChart" style="max-width: 600px; max-height: 400px;min-height:400px;"></canvas>
                    <div style="text-align:center;float:left;margin-left:140px;height:25px;">CPU 사용량이 70% 이상은 과부하</div>
                </div>
            </div>
            <div style="float:left;padding-left:30px;text-align:center;">
                <div>
                    <canvas id="barChart" style="max-width: 600px; max-height: 400px;min-height:400px;">></canvas>
                </div>
                <div style="text-align:center;float:left;margin-left:140px;height:25px;">사용할 수 있는 여유 공간 GByte [<span id="diskMbytes" style="width:200px; font-weight:bold;"></span>]</div>
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
</body>
</html>