$(function () {
    //alert("aaa");
    //var myConnection = $.connection('/echo');
    //var messenger = $.connection.messengerHub;

    //var connection = $.hubConnection();
    //var messenger = connection.createHubProxy('messengerHub');

    //alert(contosoChatHubProxy);

    var messenger = $.connection.messenger // generate the client-side hub proxy { Initialized to Exposed Hub }

    messenger.logging = true;

    $.connection.hub.error(function (error) {
        alert('SignalR error: ' + error)
    });

    //alert("messenger : " + messenger);

    function init() {
        //alert("1");
        messenger.server.addToGroup("sourcing");
        return messenger.server.getAllMessages().done(function (message) {
            // Process Message Indivudally if Necessary
        });
    }

    messenger.begin = function () {
        alert("test");
        $.jGrowl.defaults.animateClose = { width: 'hide' };
        $.jGrowl("Messenging System Started", { life: 5000 });
    };

    messenger.client.add = function (message) {
        $.jGrowl(message.Content, { header: message.Title, sticky: true });
    };


    // Start the Connection
    $.connection.hub.start(function () {
        init().done(function () {
            //alert("start");
            messenger.begin();

        });
    });

});

function sendMessage() {
    var messenger = $.connection.messenger

//    messenger.server.getAllMessages().done(function (message) {
//        // Process Message Indivudally if Necessary
//        alert(message);
    //    });


    messenger.server.broadCastMessage({ Title: "test", Content: "aaaa", Duration: "500" }, "sourcing").done(function () {
        //alert('Invocation of BroadCastMessage succeeded');
    }).fail(function (error) {
        alert('Invocation of BroadCastMessage failed. Error: ' + error);
    });
}