<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForEach.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Util.jTemplate.ForEach" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server"><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script src="../../Html/Js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../Html/Js/jQuery/jquery.common.js" type="text/javascript"></script>
    <script src="../../Html/Js/jQuery/jtemplates.js" type="text/javascript"></script>
    <script src="../../Html/Js/jQuery/jquery.list.pager.js" type="text/javascript"></script>

    <script type="text/javascript">
        var _totalItemCount = 0;
        var _currentPage = 1;
        var _recordPerPage = 10;

        $(function () {
            // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
            var loadBoardData = function () {

                //데이터베이스로부터 출력할 데이터를 Ajax로 가져왔다고 가정
                //총 레코드 카운트를 전역변수에 설정.

                //실제 데이터를 화면에 출력한다. 물론, 밑의 줄은 걍 찍어보는 것
                $("#boardList").text(_currentPage + "를 출력했습니다");
                CallService(_currentPage);
                


                // 아래의 코드는 반드시 추가해 주셔야 하는 코드입니다.
                // 그래야, 페이저 구역이 다시 렌더되면서 현재의 레코드 수에 맞게 페이징이 구성됩니다.
                // 인자로는 반드시, 전체 레코드 수와 페이지당 레코드 수를 넣어주셔야 합니다.
                var pageCount = $("#pager").pager.refresh(_totalItemCount, _recordPerPage);
                $("#pageInfo").text(_currentPage + " / " + pageCount);
            }

            // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
            var Pager = {
                pageIndexChanged: function (selectedPage) {
                    _currentPage = selectedPage;
                    loadBoardData();
                }
            }

            $(document).ready(function () {

                _totalItemCount = $("#txtTotalItemCount").val();
                _recordPerPage = $("#txtRecoredPerPage").val();

                // 페이저 초기화
                // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
                $("#pager").pager({
                    pager: "#pager",
                    pagerMode: "image", //text, image 중 선택
                    totalItemCount: _totalItemCount,
                    recordPerPage: _recordPerPage,
                    currentPage: _currentPage,
                    pageIndexChanged: Pager.pageIndexChanged,
                    prev10Text: "<<",
                    next10Text: ">>",
                    prev10ImgSrc: "images/Prev10.gif",
                    next10ImgSrc: "images/next10.gif",
                    selectedPageFontColor: "orange"
                });

                //조회 버튼 클릭 이벤트 연결
                $("#go").click(function () {
                    _currentPage = 1;
                    _totalItemCount = $("#txtTotalItemCount").val();
                    _recordPerPage = $("#txtRecoredPerPage").val();

                    loadBoardData();
                });

                loadBoardData();

            });
        });


        function CallService(page) {
            var url = "MyService.asmx/GetCustomers";
            var data = "{gubun : " + page + "}";
            var successFun = eval("SuccessFun");
            AjaxService(url, data, successFun);
        }

        function CallService1(p_url, p_data, p_success) {            
            $.ajax({
                type: "POST",
                url: p_url,
                data: p_data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: p_success,
                error: Error
            });
        }

        function SuccessFun(data, status) {            
            $('#placeholder').setTemplateURL('JTemplates/ForEachTemplate.htm');
            $('#placeholder').processTemplate(data.d);
        } 

        function Success(data, status) {
            $('#placeholder').setTemplateURL('JTemplates/ForEachTemplate.htm');
            $('#placeholder').processTemplate(data.d);
        }

        function Error(request, status, error) {
            $("#placeholder").html(request.statusText);
        }
    </script>
</head>
<body style="font-family:Verdana; font-size:12px">  
    <form id="form1" runat="server">
        <div>

            <div style="width:700px; text-align:center">
                총 레코드 수 : <input type="text" id="txtTotalItemCount" style="width:50px" value="200000" />, 
                페이지 당 레코드 수 : <input type="text" id="txtRecoredPerPage" style="width:50px" value="10" />
                <input type="button" id="go" value="조회" />
            </div>
            <div>
                <span id="pageInfo"></span>
            </div>
            <br />
            <div id="boardList" style="padding:50px; border:1px solid; width:700px; text-align:center"></div>
            <div id="placeholder" style="clear: both;"></div>
            <br />
            <div style="width:700px; text-align:right">
                <span id="pager"></span>
	        </div>   
        
        

            
            
        </div>
    </form>
</body>
</html>
