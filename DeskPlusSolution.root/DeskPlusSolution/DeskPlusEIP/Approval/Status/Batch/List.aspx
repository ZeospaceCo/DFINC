<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Status.Batch.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
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
        var DocID = "<%=DocID%>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalItemCount %>"; //게시물수 //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬              

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";

        var PageUrl = "<%= Request.Url.ToString() %>";
        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Approval/Js/Status/Batch/List.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div id="layout_body">
        <div class="layout_full">

            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1>일괄결재문서</h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnAdmin" runat="server" Text="관리자" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnDefault" runat="server" Text="초기메뉴로 설정" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnBachDel" runat="server" class="btn btn-secondary" Text="완결문서일괄삭제" /></li>
                </ul>
            </div>

            <div id="layout_srh">
                <ul class="btn_searchbox">
                    <li>
                        <select name="ddlSearchField" id="ddlSearchField" class="searchselect form-control">
	                        <option value="ManyName">이름</option>
	                        <option value="Mail">이메일</option>
	                        <option value="DeptManyName">부서명</option>
	                        <option value="MobilePhone">휴대폰</option>
                        </select>
                    </li>
                    <li><input name="ctbSearch" type="text" id="ctbSearch" class="searchtext" placeholder="" Required="False" class="text" style="width:200px;" /></li>
                    <li><a href="#" class="searchbtn" title="" class="btn">검색</a></li>
                </ul>
            </div>

            <div id="layout_content">

                <!-- 테이블 시작 //-->
                

                <table class="table_typeA">
                <caption>상신한 문서</caption>
                <colgroup>
                    <col />
                    <col />
                    <col />
                    <col />
                    <col />
                    <col />
                </colgroup>
                <thead>
                    <tr>
                        <th>문서양식</th>
                        <th><a href="#">결재상황</a></th>
                        <th><a href="#">제목</a></th>
                        <th>첨부파일</th>
                        <th><a href="#">상신자</a></th>
                        <th><a href="#">다음결재자</a></th>
                        <th><a href="#">상신일</a></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>기안</td>
                        <td><a href="" title="클릭 시, 결재상황SEQ를 조회할 수 있는 페이지가 뜸." onclick="List.DocFlow()">반려</a></td>
                        <td><a href="" title="문서보기/수정후재상신/후열처리 등의 서브메뉴">MS Certified Partner 재등록 사용료 지급.....</a></td>
                        <td><i class="fas fa-arrow-alt-circle-down" style="cursor:pointer"></i></td>
                        <td>길동홍</td>
                        <td>홍상사</td>
                        <td>2012-01-01</td>
                    </tr>
                    <tr>
                        <td colspan="7">내용이 없습니다</td>
                    </tr>
                </tbody>
                </table>

                
                <!-- 테이블 끝 //-->

                <!-- 페이징 시작 //-->
                <div class="paging_num_s1"> 
                    <h5>1 / 20</h5>
	                <span id="pager">
    	            <span id="pager_prev10"><a href="#">이전</a></span>
                        <em> 1 </em>
                        <a href="#"> 2 </a>
                        <a href="#"> 3 </a>
                        <a href="#"> 4 </a>
                        <a href="#"> 5 </a>
                        <a href="#"> 6 </a>
                        <a href="#"> 7 </a>
                        <a href="#"> 8 </a>
                    <span id="pager_next10"><a href="#">다음</a></span></span> 
                    <select class="aRight form-control">
                    <option>10</option><option>20</option><option>30</option>
                    </select>
                </div>
                <!-- 페이징 끝 //-->

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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
</body>
</html>
