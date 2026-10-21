
$(function () {

    $(document).ready(function () {

        //[닫기]
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            self.close();
        });

        //사업부 초기데이터 Load
        loadCategory();

        //사업부(회사) 기본 선택 기능 추가
        document.all("categoryID1").value = "1";
        selectItem(1);

    });
});

function loadCategory() {
    //사업부 초기데이터 Load
    var selectCtl = document.all("categoryID1");
    var objOptionItem = document.createElement("option");

    var selectCtlLen = selectCtl.length
    for (i = 0; i <= selectCtlLen; i++) {
        selectCtl.remove(0);
    }

    arrCategoryID = categoryID.split("∫");
    arrCategoryName = categoryName.split("∫");
    arrCourse = course.split("∫");
    arrRemark = remark.split("∫");

    for (i = 0; i < arrCategoryID.length - 1; i++) {
        var selectCtl = document.all("categoryID1");
        var objOptionItem = document.createElement("option");

        objOptionItem.value = arrCategoryID[i];
        objOptionItem.text = arrCategoryName[i];
        objOptionItem.course = arrCourse[i];
        objOptionItem.remark = arrRemark[i];

        selectCtl.add(objOptionItem, selectCtl.length + 1);
    }
}

function selectItem(gubun) {
    var parentCategoryID = "";

    switch (gubun) {

        case 1:
            //사업부 선택
            if (document.all("categoryID1").value != "") {
                removeItem(3);
                removeItem(4);
                removeTarget();
                parentCategoryID = document.all("categoryID1").value;
            }
            break;

        case 2:
            //대분류 선택
            if (document.all("categoryID2").value != "") {
                removeItem(4);
                removeTarget();
                parentCategoryID = document.all("categoryID2").value;
            }
            break;

        case 3:
            //중분류 선택
            if (document.all("categoryID3").value != "") {
                removeTarget();
                parentCategoryID = document.all("categoryID3").value;
            }
            break;

        case 4:
            //소분류 & 전결사항 선택
            if (document.all("categoryID4").value != "") {

                document.all("selTarget").value = $.trim(document.all("categoryID4").options[document.all("categoryID4").selectedIndex].text);
                document.all("selItem").value = $.trim(document.all("categoryID4").options[document.all("categoryID4").selectedIndex].remark);
                categoryID = document.all("categoryID4").value;
            }
            break;
    }

    if (gubun == 4) {
        //소분류 & 전결사항 선택시 전결권자 선택 및 비고 표시
        if (categoryID != "") {
            rtnVal = decisionTargetLoad(categoryID, companyID);
            arrRtnVal = rtnVal.split("∬");
            targetID = arrRtnVal[0];
            targetName = arrRtnVal[1];

            var selectCtl = document.all("targetID");
            var objOptionItem = document.createElement("option");

            selectCtlLen = selectCtl.length
            for (i = 0; i <= selectCtlLen; i++) {
                selectCtl.remove(0);
            }

            objOptionItem.value = targetID;
            objOptionItem.text = targetName;

            selectCtl.add(objOptionItem, selectCtl.length + 1);
            document.all("targetID")[0].selected = true;

        }
    }
    else {

        //하위 분류 리스트 가져오기
        if (parentCategoryID != "" && document.all("categoryID" + gubun).value != "") {
            rtnVal = decisionListLoad(gubun, parentCategoryID, companyID);

            arrRtnVal = rtnVal.split("∬");
            categoryID = arrRtnVal[0];
            categoryName = arrRtnVal[1];
            course = arrRtnVal[2];
            remark = arrRtnVal[3];

            var selectCtl = document.all("categoryID" + parseInt(gubun + 1));
            var objOptionItem = document.createElement("option");

            var selectCtlLen = selectCtl.length
            for (i = 0; i <= selectCtlLen; i++) {
                selectCtl.remove(0);
            }

            arrCategoryID = categoryID.split("∫");
            arrCategoryName = categoryName.split("∫");
            arrCourse = course.split("∫");
            arrRemark = remark.split("∫");

            for (i = 0; i < arrCategoryID.length - 1; i++) {
                var selectCtl = document.all("categoryID" + parseInt(gubun + 1));
                var objOptionItem = document.createElement("option");

                objOptionItem.value = arrCategoryID[i];
                objOptionItem.text = arrCategoryName[i];
                objOptionItem.course = arrCourse[i];
                objOptionItem.remark = arrRemark[i];

                selectCtl.add(objOptionItem, selectCtl.length + 1);
            }
        }
    }
}

function removeItem(gubun) {
    //SelectBox Item삭제
    var selectCtl = document.all("categoryID" + gubun);
    var objOptionItem = document.createElement("option");

    var selectCtlLen = selectCtl.length;

    for (i = 0; i <= selectCtlLen; i++) {
        selectCtl.remove(0);
    }

}

function removeTarget() {

    var selectCtl = document.all("targetID");
    var objOptionItem = document.createElement("option");

    var selectCtlLen = selectCtl.length
    for (i = 0; i <= selectCtlLen; i++) {
        selectCtl.remove(0);
    }

    objOptionItem.value = "0";
    objOptionItem.text = "규정없음";

    selectCtl.add(objOptionItem, selectCtl.length + 1);

    document.all("targetID")[0].selected = true;

}

function sendit() {

    if (document.all("categoryID1").value == "") {
        alert("사업부를 선택하세요.");
        return;
    }

    if (document.all("categoryID2").value == "") {
        alert("대분류를 선택하세요.");
        return;
    }

    if (document.all("categoryID3").value == "") {
        alert("중분류를 선택하세요.");
        return;
    }

    if (document.all("categoryID3").value == "") {
        alert("소분류 & 전결사항을 선택하세요.");
        return;
    }

    if (document.all("targetID").value == "0") {
        alert("전결권자가 없습니다.");
        return;
    }


    if (opener.DOC.all("DTARGET")) {
        //전결규정 근거
        dTarget = document.all("categoryID1")[document.all("categoryID1").selectedIndex].course + "-" + document.all("categoryID2")[document.all("categoryID2").selectedIndex].course + "-" + document.all("categoryID3")[document.all("categoryID3").selectedIndex].course + "-" + document.all("categoryID4")[document.all("categoryID4").selectedIndex].course;
        //전결권자
        dCourse = document.all("targetID")[0].text;
        //전결규정 적용
        opener.setDecision(dTarget, dCourse, targetNameList);
    }

    window.close();
}