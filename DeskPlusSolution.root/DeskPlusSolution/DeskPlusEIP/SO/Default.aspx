<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.SO.Web.Default" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="css/animate.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
<div id="wrapper">

    <nav class="navbar-default navbar-static-side" role="navigation">
        <div class="sidebar-collapse">
            <ul class="nav metismenu" id="side-menu">
                <li class="nav-header">
                    <div class="dropdown profile-element">
                            <a data-toggle="dropdown" class="dropdown-toggle" href="#">
                            <span class="clear"> <span class="block m-t-xs"> <strong class="font-bold">이현수</strong>
                             </span> <span class="text-muted text-xs block">개발본부 이사 <b class="caret"></b></span> </span> </a>
                            <ul class="dropdown-menu animated fadeInRight m-t-xs">
                                <li><a href="#">Logout</a></li>
                            </ul>
                    </div>
                    <div class="logo-element">
                        DP+
                    </div>
                </li>
                <li class="active">
                    <a href="index.html"><i class="fa fa-th-large"></i> <span class="nav-label">사무실 환경(조명/온도)</span></a>
                </li>
                <li>
                    <a href="minor.html"><i class="fa fa-th-large"></i> <span class="nav-label">사무실 보안(출입문/감지)</span> </a>
                </li>
                <li>
                    <a href="minor.html"><i class="fa fa-th-large"></i> <span class="nav-label">개인 근태</span> </a>
                </li>
            </ul>

        </div>
    </nav>

    <div id="page-wrapper" class="gray-bg">
        <div class="row border-bottom">
            <nav class="navbar navbar-static-top white-bg" role="navigation" style="margin-bottom: 0">
                <div class="navbar-header">
                    <a class="navbar-minimalize minimalize-styl-2 btn btn-primary " href="#"><i class="fa fa-bars"></i> </a>
                    <form role="search" class="navbar-form-custom" method="post" action="#">
                        <div class="form-group">
                            <input type="text" placeholder="Search for something..." class="form-control" name="top-search" id="top-search">
                        </div>
                    </form>
                </div>
                <ul class="nav navbar-top-links navbar-right">
                    <li>
                        <a href="#">
                            <i class="fa fa-sign-out"></i> Log out
                        </a>
                    </li>
                </ul>

            </nav>
        </div>
        <div class="wrapper wrapper-content animated fadeInRight">
            <div class="row">
                <div class="col-lg-12">
                    <div class="text-center m-t-lg">
                        <h1>
                            SMART OFFICE
                        </h1>
                        <small>
                            어떤 정보로 구성 해야 할까? 사무실의 위치별 온도/ 전원 사용 유무 / 
                        </small>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-3">
                    <div class="widget style1 lazur-bg">
                        <div class="row">
                            <div class="col-xs-4">
                                <i class="fa fa-bar-chart fa-5x"></i>
                            </div>
                            <div class="col-xs-8 text-right">
                                <span> 온도 </span>
                                <h2 class="font-bold">22C</h2>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3">
                    <div class="widget style1 lazur-bg">
                        <div class="row">
                            <div class="col-xs-4">
                                <i class="fa fa-lightbulb-o fa-5x"></i>
                            </div>
                            <div class="col-xs-8 text-right">
                                <span> 조명 </span>
                                <h2 class="font-bold">ON</h2>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6">
                    <div class="ibox float-e-margins">
                        <div class="ibox-title">
                            <h5>온도 변화
                                <small></small>
                            </h5>
                            <div ibox-tools>
                                <div style="float: left; margin-left: 30px;">
                                    <select id="ddlType">
                                        <option value="0">전체</option>
                                        <option value="1">출입문</option>
                                        <option value="2">기술본부</option>                                        
                                    </select>
                                    <input type="text" id="selDate" size="12" class="clsDate" />
                                    <input id="btn_line_chart" type="button" value="Show" /></div>
                                </div>
                            </div>
                        </div>
                        <div class="ibox-content">
                            <div>
                                <canvas id="lineChart" height="140"></canvas>
                            </div>
                            <div style="text-align:center;float:left;margin-left:40px;width:70px;height:25px;background-color:rgba(26,179,148,0.5);border-color:rgba(220,220,220,1);">기술본부</div>
                            <div style="text-align:center;float:left;margin-left:40px;width:70px;height:25px;background-color:rgba(220,220,220,0.5);">출입문</div> 
                        </div>
                    </div>
                </div>
            </div>
    </div>
    <div class="footer">
            <div class="pull-right">
                10GB of <strong>250GB</strong> Free.
            </div>
            <div>
                <strong>Copyright</strong> Example Company &copy; 2014-2015
            </div>
        </div>

</div>

<!-- Mainly scripts -->
<script src="js/jquery-2.1.1.js"></script>
<script src="js/bootstrap.min.js"></script>
<script src="js/plugins/metisMenu/jquery.metisMenu.js"></script>
<script src="js/plugins/slimscroll/jquery.slimscroll.min.js"></script>

<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>  
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>  
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
<!-- Custom and plugin javascript -->
<script src="js/inspinia.js"></script>
<script src="js/plugins/pace/pace.min.js"></script>

<!-- ChartJS-->
<script src="js/plugins/chartJs/Chart.min.js"></script>

<script type="text/javascript">
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {
        $("#selDate").datepick($.datepicker.regional[PreLang]);

        $("#btn_line_chart").on('click', function (){
            var ddlType = $("#ddlType").val();
            var selDate = $("#selDate").val();
            //alert(ddlType + " : " + selDate);
            var data = JSON.stringify({
                _thingsID: ddlType,
                _selDate: selDate
            });

            GetChartData(data);
            
        });

        var curDate = new Date();
        var curDateYMD = curDate.getFullYear() + '-' + ('0' + (curDate.getMonth()+1)).slice(-2) + '-' + ('0' + curDate.getDate()).slice(-2);

        var data = JSON.stringify({
            _thingsID: 0,
            _selDate: curDateYMD  
        });
        $("#selDate").val(curDateYMD);
        GetChartData(data);

    })

    function GetChartData(data) {
        $.ajax({
            type: "POST",
            url: "/DeskPlusEIP/SO/Ws/Things/Default.asmx/GetTemChart",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                GetChart(response);
            },
            error: function () {
                /*
                if (count == 0) {
                //alert("Load ERROR");
                setTimeout(function () {
                Main.LoadPortlet(column_ID, portletPath, portletID);
                }, 100);
                }
                else {
                jQuery(column_ID).append('Load ERROR');
                }

                count += 1;
                */
            }

        });
    }

    function GetChart(response)
    {
        var cData = response.d;
        var chartLabels = cData[0];
        var listMu = cData[1];
        var listMo = cData[2];

        var lineData = {
            labels: chartLabels,
            datasets: [
                {
                    label: "출입문",
                    fillColor: "rgba(220,220,220,0.5)",
                    strokeColor: "rgba(220,220,220,1)",
                    pointColor: "rgba(220,220,220,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(220,220,220,1)",
                    data: listMu
                },
                {
                    label: "기술본부",
                    fillColor: "rgba(26,179,148,0.5)",
                    strokeColor: "rgba(26,179,148,0.7)",
                    pointColor: "rgba(26,179,148,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(26,179,148,1)",
                    data: listMo
                }
            ]
        };

        var lineOptions = {
            scaleShowGridLines: true,
            scaleGridLineColor: "rgba(0,0,0,.05)",
            scaleGridLineWidth: 1,
            bezierCurve: true,
            bezierCurveTension: 0.4,
            pointDot: true,
            pointDotRadius: 4,
            pointDotStrokeWidth: 1,
            pointHitDetectionRadius: 20,
            datasetStroke: true,
            datasetStrokeWidth: 2,
            datasetFill: true,
            responsive: true,

        };

        var ctx = document.getElementById("lineChart").getContext("2d");
        var myNewChart = new Chart(ctx).Line(lineData, lineOptions);
    }
</script>

</body>

</html>
