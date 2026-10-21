<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NoticeBanner.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Board.NoticeBanner" %>
<script type="text/javascript">
    var Banner_tabPortletID = "<%=TabPortletID%>";
    var Banner_portletID = "<%=PortletID%>";
    var Banner_categoryID = "<%=CategoryID%>";
    var Banner_dataURL = "<%=DataURL%>";
    var Banner_dataType = "<%=DataType%>";
    var Banner_IsAuth = "<%=IsAuth%>";
    var Banner_ListCount = "<%=ListCount %>";
    var Banner_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 받지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 카테고리 아이디
		$("#hdnBoardAlbumCategoryID_" + portletDom).val("<%=BoardCategoryID %>");

        //삭제
        $("#del_<%=TabPortletID%>_<%=PortletID%>").on("click", function (e) {
            e.preventDefault();

            Main.DelTabPortlet("<%=TabPortletID%>_<%=PortletID%>");
            $("#portlet_<%=TabPortletID%>_<%=PortletID%>").fadeOut("slow");
        });

        //아이콘 감추기
        $("#div_<%=TabPortletID%>_<%=PortletID%>").hide();

        LoadBoardAlbumList_<%=TabPortletID%>_<%=PortletID%>();
        //setTimeout(function () { LoadBoardAlbumList_<%=TabPortletID%>_<%=PortletID%>(); }, 1);
    });

    // 마우스 오버
    function MOver_<%=TabPortletID%>_<%=PortletID%>()
    {
        $("#div_<%=TabPortletID%>_<%=PortletID%>").show();
        //관리자가 아니면 - 관리자 경우 활성화
        if (Banner_IsAuth != "True") {
            $("#div_<%=TabPortletID%>_<%=PortletID%>").hide();
        }
    }
    // 마우스 아웃
    function MOut_<%=TabPortletID%>_<%=PortletID%>()
    {
        $("#div_<%=TabPortletID%>_<%=PortletID%>").hide();
    }
    // 데이타 가져오기
    function LoadBoardAlbumList_<%=TabPortletID%>_<%=PortletID%>() {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

		//게시판 리스트
		var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/Album.asmx/GetBoardListNotice";    //공지로 지정된 글만 가져옴 (TOP 5개)
		
		ParamArray = {
		    'pageSize': 5,          // 리스트 행 개수
		    'categoryID': $("#hdnBoardAlbumCategoryID_" + portletDom).val(),  // 카테고리 아이디
            'gubun': 1              //이미지게시판 구분값
		};

        CallEncrypt(CallGetParam());
		var paramData = "{p:'" + GetEncryptValue + "'}";
		
		$.ajax({
                type: "POST",
		        url: urlPage,
		        async: true,
		        data: paramData,
		        contentType: "application/json; charset=utf-8",
		        dataType: "json",
		        success: function (data) {

			        $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTNoticeBanner.htm?seq=" + Guids(), null, { filter_data: false });
			        $('#plList_' + portletDom).processTemplate(data.d);

                    //alert($("#kkk").height())
                    if (data.d.length > 0) {                                    //alert($("#plList_" + portletDom + " .imageSlider .imageList").length)
			            $("#plList_" + portletDom + " .clsimg").jCarouselLite({

                            vertical: false, 
                            //pauseOnHover:true,        /* 마우스오버시스크롤멈춤*/
                            //btnPrev: ".flowmenu .left",
			                //btnNext: ".flowmenu .right",
			                visible: 1,
                            start: 0, 
                            scroll: 1, 
                            circular:true,      /*반복여부*/
                            auto : 10000,   /*10000*/
			                speed: 2000,    /*2000*/

                            btnGo: $(".flowmenu a"),
                            afterEnd: function(o) {                                 //A태그 클릭효과                
                                var idx = $("img", o).attr("idx");              
                                $(".flowmenu a").removeClass("flowactive");
                                $(".flowmenu ." + idx).addClass("flowactive"); 
                            }
			            });
                    }

			        if($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
			        if ($(".IconNoImg") != undefined) { $(".IconNoImg").attr('src', ImagePath + '/Icon/ico_noimg2.gif'); }
		        },
		        error: function (data, status, err) { alert(R_ErrorConnection + data); }
	    });
    }

</script>

<!-- 포틀릿(배너) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">

    <!-- 사이즈 : 높이고정 //-->
    <div class="portlet-free" style="overflow:hidden;height:228px;" align="center"  onMouseOver="MOver_<%=TabPortletID%>_<%=PortletID%>()" onMouseOut="MOut_<%=TabPortletID%>_<%=PortletID%>()">
        <!-- 슬라이드 이미지 //-->
        <div id="plList_<%=TabPortletID%>_<%=PortletID%>">
            <div style="padding-top: 50px; text-align: center;">
                <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
            </div>
        </div>
        
        <!-- 삭제 //-->
        <div class="flowdel" id="div_<%=TabPortletID%>_<%=PortletID%>">
            <a id="del_<%=TabPortletID%>_<%=PortletID%>"><img title="" alt=""  src="<%=BaseImgPath%>/Icon/ContextMenu/del.png" /></a>
        </div>
        
    </div>
<div style="display: none">
    <input type="hidden" id="hdnBoardAlbumCategoryID_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardAlbumCategoryID_<%=TabPortletID%>_<%=PortletID%>" />
</div>
</div>
<!-- 포틀릿(배너) End //-->