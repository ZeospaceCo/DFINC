Tree = {}

$(function () {
    $(document).ready(function () {

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            overTop: 0,
            active: 0
        });
        
    });
});


Tree.Add = function () {
    
    parent.ChildCommunication(1);
}


function DeptNodeSelect_onNodeSelect(node) {
   /// $("#hdSelDept").val('');
    var selID = node.ID;
    var selValue = node.Value;

    if (selID == "0")
        return;

    
    //$("#hdSelDept").val(selID);
    
      ParamArray = {
        'Key': 'DP',
        'Value':selValue
        };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    
    parent.ChildCommunication(GetEncryptValue);
   // alert( $("#hdSelDept").val());
}