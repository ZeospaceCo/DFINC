function setCookie(name, value, expiredays) {
    var todayDate = new Date();
    var year = todayDate.getFullYear();
    var month = todayDate.getMonth();
    var day = todayDate.getDate() + 1 + expiredays;
    todayDate = new Date(year, month, day);
    document.cookie = name + "=" + escape(value) + "; path=/; expires=" + todayDate.toGMTString() + ";"
}

function saveCookie() {
    setCookie(popid, "yes", 0);
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    //self.close();
}

function movePop() {
    var url = "";
    if (url != "") {
        opener.location.href = url;
    }
    self.close();
}

function ViewChecked() {
    setCookie(popid, "yes", 7);
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    //self.close();
}

function PopupClose() {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}