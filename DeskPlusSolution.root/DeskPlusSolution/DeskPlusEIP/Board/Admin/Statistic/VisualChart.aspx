<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualChart.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Statistic.VisualChart" %>

<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link href="/DeskPlusEIP/Html/Js/Chart/VisualChart/basic.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link href="/DeskPlusEIP/Html/Js/Chart/VisualChart/visualize.css" type="text/css" rel="stylesheet" />
    <link href="/DeskPlusEIP/Html/Js/Chart/VisualChart/visualize-dark.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%= GetResource("StatYearMonth") %></h1>
            <!-- 버튼 //-->
            <ul class="btn_left">
            </ul>
            <ul class="btn_right">                
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchImg">                
                <li><%= GetResource("SearchYear") %> : <select name="yearpicker" id="yearpicker"></select></li>
            </ul>

            <ul class="btn_searchbox">
                
            </ul>
        </div>

        <div id="layout_content">
            <table>
	            <caption><%= GetResource("StatYearMonth") %></caption>
	            <thead>
		            <tr>
			            <td></td>
			            <th scope="col"><%= GetResource("WriteCnt")%></th>
		            </tr>
	            </thead>
                <tbody>
                    <asp:Literal ID="ltlConent" runat="server"></asp:Literal>
                </tbody>
           </table>                  
            
            <!-- 테이블 끝 //-->
        </div>
    </div>
    </div> 
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
	<script type="text/javascript" src="/DeskPlusEIP/Html/Js/Chart/VisualChart/enhance.js"></script>	
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/Chart/VisualChart/excanvas.js"></script>	
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/Chart/VisualChart/visualize.jQuery.js"></script>	
    <script type="text/javascript">
        var YY = "<%=YY %>";
        $(function () {
            $(document).ready(function () {
                $('table').visualize({ type: 'bar', width: '900px' });

                $('#yearpicker').append($('<option />').val("").html("--"));

                for (i = new Date().getFullYear(); i > 2000; i--) {
                    $('#yearpicker').append($('<option />').val(i).html(i));
                }

                $('#yearpicker').val(YY);

                $("#yearpicker").change(function (e) {
                    e.preventDefault();
                    if ($("#yearpicker").val() == "") {
                        return;
                    }

                    ParamArray = {
                        'yy': $("#yearpicker").val()
                    };

                    CallEncrypt(CallGetParam());

                    //이동하기    
                    location = "visualchart.aspx?p=" + GetEncryptValue;
                });
            });
        });
    </script>
</body>
</html>
