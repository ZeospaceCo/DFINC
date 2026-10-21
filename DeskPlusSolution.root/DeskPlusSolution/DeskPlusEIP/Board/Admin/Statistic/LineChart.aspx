<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineChart.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Statistic.LineChart" %>

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
    <link rel='stylesheet' href='/DeskPlusEIP/Html/Js/ChartNwagon/Nwagon.css' type='text/css' />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%= GetResource("StatYearMonth") %></h1>
            </div>
            <div id="layout_srh">
                <ul class="btn_searchImg">
                    <li>
                        <%= GetResource("SearchYear") %> : <asp:DropDownList ID="ddlYear" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                    </li>
                </ul>
                <ul class="btn_searchbox">
                </ul>
            </div>
            <div id="subline" class="mainoutline" style="width:100%;padding:5px;">
                <table cellpadding="0" cellspacing="0" border="0" style="width:99%;"> 
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <!-- 차트 시작 -->                        
                        <td style="padding:2px;">
                            <div id="chartStacked"></div>                        
                        </td>
                        <!-- 차트 종료 -->
                    </tr>
                    <tr><td style="height:10px"></td></tr>
                </table>
            </div>
            <div id="layout_content">
                <!-- 테이블 시작 //-->
                <table class="table_typeA">
                    <caption>
                        <%= GetResource("StatYearMonth") %>
                    </caption>
                    <colgroup>
                        <col width="120px" />
                        <col width="120px" />
                    </colgroup>
                    <tr>
                        <th>
                            <%= GetResource("YearMonth")%>
                        </th>
                        <th>
                            <%= GetResource("WriteCnt")%>
                        </th>
                    </tr>
                    <asp:Literal ID="ltlConent" runat="server"></asp:Literal>
                </table>
                
                <!-- 테이블 끝 //-->
            </div>            
            
        </div>
    </div>
    <br />

    <script>

        var xChartTitle = [<%= ChartTitle %>];
        var chartWidth = 350;
        var chartHeight = 350;
        var xChartTitleLen = xChartTitle.length;
        if(xChartTitleLen > 0) {
            chartWidth = Number(xChartTitleLen * 55) + 150; //막대차트 개당 넓이공간 55px
        }

        var options = {
            'legend': {
                names: xChartTitle,
                hrefs: []
            },
            'dataset': {
                title: 'Playing time per day',
                values: [<%= ChartValue %>],
                colorset: ['#56b4e9'],
                fields: ['등록수']
            },
            'chartDiv': 'chartStacked',
            'chartType': 'column',
            'chartSize': { width: 900, height: 300 },            
            'increment': <%= ChartIncrement %>
        };

        Nwagon.chart(options);

	</script>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script src="/DeskPlusEIP/Util/Component/FusionCharts/FusionCharts.js" type="text/javascript"></script>
	<script type="text/javascript" src='/DeskPlusEIP/Html/Js/ChartNwagon/Nwagon.js'></script>
    <script type="text/javascript">
        $(function () {
            $(document).ready(function () {

                $('#ddlYear').change(function () {

                    ParamArray = {
                        'year': $('#ddlYear').val()
                    };

                    CallEncrypt(CallGetParam());
                    location = "LineChart.aspx?p=" + GetEncryptValue;
                });
            });

        });
    </script>
</body>
</html>