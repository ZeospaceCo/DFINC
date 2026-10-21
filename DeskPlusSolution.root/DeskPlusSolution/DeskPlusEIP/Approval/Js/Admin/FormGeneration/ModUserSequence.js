ModUserSequence = {}

$(function () {
    $(document).ready(function () {

        $("#ulWidgets").sortable();

        //[닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        //[확인]
        $("#cbtnConfirm").click(function (e) {

            e.preventDefault();
            var items = $("#ulWidgets li");
            var sortlist = "";

            var appointerKind = "";
            var deptID = "";
            var deptName = "";
            var userID = "";
            var userName = "";
            var isDept = "";
            var isActive = "";

            var arrsortlist = "";

            for (var i = 0; i < items.length; i++) {
                sortlist = items[i].id
                arrsortlist = sortlist.split("_");

                if (i == 0) {
                    appointerKind = arrsortlist[0];
                    deptID = arrsortlist[1];
                    deptName = arrsortlist[2];
                    userID = arrsortlist[3];
                    userName = arrsortlist[4];
                    isDept = arrsortlist[5];
                    isActive = arrsortlist[6];
                }
                else {
                    appointerKind += "," + arrsortlist[0];
                    deptID += "," + arrsortlist[1];
                    deptName += "," + arrsortlist[2];
                    userID += "," + arrsortlist[3];
                    userName += "," + arrsortlist[4];
                    isDept += "," + arrsortlist[5];
                    isActive += "," + arrsortlist[6];
                }
            }

            ScsModSeq(appointerKind, deptID, deptName, userID, userName, isDept, isActive);
        });


    });
});


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2016-03-22
작성목적 : 항목순서변경
**********************************************************************************************/
function ScsModSeq(_appointerKind, _deptID, _deptName, _userID, _userName, _isDept, _isActive) {
    if (_appointerKind != "") {
        parent.ModSetting.LoadData(_appointerKind, _deptID, _deptName, _userID, _userName, _isDept, _isActive);
    }

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}
