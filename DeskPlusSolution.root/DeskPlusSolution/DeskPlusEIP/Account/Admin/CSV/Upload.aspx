<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.CSV.Upload" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var WindowObjectReference; //새창용

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_SelectOutLookFormat = "외부메일 형식을 선택 해 주세요";
    </script>
    <script type="text/javascript">
        InOutStep1 = {}

        $(function () {
            $(document).ready(function () {
                //확인
                $('#cbtnGetCSV').click(function (e) {
                    e.preventDefault();
                    InOutStep1.Save();
                });

            });
        });


        /**************************************************
        작성자   : 심재운
        작성일자 : 2011-04-11
        작성목적 : CSV 주소록 가져오기
        ***************************************************/
        InOutStep1.Save = function () {
            var gubun = "";

            if ($("#Career").is(":checked")) {
                gubun = $("#Career").val();
            }

            if ($("#Family").is(":checked")) {
                gubun = $("#Family").val();
            }

            if ($("#Scholar").is(":checked")) {
                gubun = $("#Scholar").val();
            }

            if ($("#Reward").is(":checked")) {
                gubun = $("#Reward").val();
            }


            if (gubun == "") {
                alert("선택해 주세요.");
                return;
            }

            $("#tbTypes").val(gubun);

            __doPostBack("lbtnSumbit", "");
        }

        /**************************************************
        작성자   : 심재운
        작성일자 : 2011-04-11
        작성목적 : 구글 주소록 페이지 새창띄우기
        ***************************************************/
        InOutStep1.GoogleContact = function () {
            //    var urlpath = "/DeskPlusEIP/Address/Outlook/InGoogleStep1.aspx";
            //    WindowObjectReference = window.open(urlpath, "GoogleContact", "menubar=no,location=no,resizable=no,scrollbars=yes,status=no");

            var url = "/DeskPlusEIP/Address/Outlook/InGoogleStep1.aspx";
            var param = "";
            WindowObjectReference = PopupWinOpen(url, param, "GoogleContact", "800", "600")
        }
    </script>  
</head>
<body>
    <form id="form1" runat="server">
    
    <div class="outmail">
        <p class="outmail_descrip">휴대폰에서 내보낸 CSV(쉼표로 분리)파일이나 직접 작성한 엑셀파일을 가져와 주소록에 추가할 수 있습니다.<br />
            한메일 주소록을 아웃룩, 익스프레스용 CSV파일 또는 엑셀 파일로 다운받고 싶으신 경우는 <em class="ft_blue">주소록 내보내기</em>를 이용하시면 됩니다.<br />
            ※ 파일의 주소 개수가 1000개 이상인 경우 속도가 느려질 수 있으며, 1000개씩 파일을 나눠서 가져오기 해 주세요. </p>
        <ul class="outmail_location">
            <li><a class="over">01 불러오기</a></li>
            <li>02 미리보기</li>
            <li class="last-child">03 저장하기</li>
        </ul>
        <div class="outmail_select"> 
    	    <span class="btnWrap"> 
                    <span class="btn_address"><input id="Career" name="Career" type="radio" value="Career" /><a href="#"> 경력</a></span>
                    <span class="btn_address"><input id="Family" name="Family" type="radio" value="Family" /><a href="#"> 가족</a></span>
                    <span class="btn_address"><input id="Scholar" name="Scholar" type="radio" value="Scholar" /><a href="#"> 학력</a> </span>
                    <span class="btn_address last-child"><input id="Reward" name="Reward" type="radio" value="Reward" /><a href="#"> 상벌</a> </span>
            </span>
            <div class="addfile">
        	    <span class="btnWrap">
                    <span class="list_add_style4">
                        <label for="fromfile">CSV 가져오기</label>
                        <asp:FileUpload ID="fuFile" runat="server" />
                        <a href="#" id="cbtnGetCSV" class="btn btn-secondary">가져오기</a> 
                    </span>
                    <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
                </span>
            </div>        
        </div>
    </div>

    <div style="display:none">
        <asp:TextBox ID="tbTypes" runat="server" Width="0"></asp:TextBox>
        <asp:LinkButton ID="lbtnSumbit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    </div>
    </form>
</body>
</html>