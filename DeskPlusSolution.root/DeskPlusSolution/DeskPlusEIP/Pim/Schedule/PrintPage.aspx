<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintPage.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.PrintPage" EnableViewState="false" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PimFullCalendar.css"  /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PimFullCalendar.css"  />                   
</head>
<body>
    <form id="form1" runat="server">
         <table width="100%" border="0">
        <tbody>
        <tr>
        <td align="center"><b><span style="cursor:pointer;"><asp:Label ID="lblDate" runat="server" Text="" ></asp:Label></span></b></td>
        </tr></tbody>
        </table>
		<div id="doPrint">
			<div id='calendar' style="float:left;padding:0px;border:0px solid red;"></div>
		</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>              
    <script type="text/javascript">         
        //full calendar 기본 변수
        var scrollfocus=5; 
        var DefaultView='<%=DefaultView %>';
        var calendarheight = 0;
        var setfirstHour= 0;
        //요일
        var week = <%=Week %>;
        var weeklong = <%=WeekLong %>;
        //달
        var monthname = <%=monthname %>;
        var shortmonthname = <%=ShortMonthname %>;        
        var UserID  = <%=UserID %>;
        var date = '<%=Date %>';
        var Opentype = '<%=Opentype %>';
        var Year = "<%=Year %>";
        var Month = "<%=Month %>";         
        var Day = "<%=Day %>";
        var  DayKind = "<%=DayKind %>";
       
        $(function () {
            $(document).ready(function () {

                 //카렌다 로딩
       

            GetEvent();
           

           });
        });

        function GetEvent()
        {
            
            var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetScheduleAllKind";
            var scope = 'N';
            var  Type = "I";           
            //alert(Opentype);
            ParamArray = {
                'StartDate': date,
                'Type': Type,
                'OpenType': Opentype,
                'DayKind': DayKind,
                'CurrentUserID': UserID,
                'scope': 'N'

            }
            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("SuccessPrint");            
            AjaxService(url, data, obj);                        
        }

        function SuccessPrint(data, status)
        {               
            $('#calendar').fullCalendar({
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                loading: function (bool) {
               
                },
                events: data.d
            });                        

           var movedate = new Date(Year, Month-1, Day);
           
           var d = movedate.getDate();
           var m = movedate.getMonth();
           var y = movedate.getFullYear();
           

            $('#calendar').fullCalendar('gotoDate', y, m, d);
            
            PrintAction();
        }

        function PrintAction()
        {            
            
            //window.print();
            //self.close();

            setTimeout("window.print();", 600);
            setTimeout("self.close();", 1000);
          

        }
     </script>
     <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/FullCalendar/fullcalendar-print.js"></script>  
</body>
</html>
