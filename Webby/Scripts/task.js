var running = false;
function startTask() {
    running = true;
    if (running) {
        $('#btnStartProcess').fadeOut('slow');
        $('#btnStopProcess').fadeIn('slow');
        if (Confirmation("START")) {
            $.ajax({
                url: '/Home/Run',
                type: 'GET',
                success: function (res) {
                    alert(res);
                }
            });
        }
    }
}
function Confirmation(process) {
    var r = confirm("Are you sure to " + process + " ?");
    return (r == true)
}
function cancelTask() {
    if (Confirmation("CANCEL")) {
        $.ajax({
            url: '/Home/Cancel',
            type: 'GET',
            success: function (res) {
                alert(res);
                $('#progressBarParagraph').text('');
                $("fieldset").css({ "display": "none" });
                $("#runningService").css({ "display": "none" });
                running = false;
                $('#mod-progress').css('display', 'none');
                $("#mod-progress fieldset").children().css({ "display": "none" });
                $("#mod-progress,#modal-dialog").children().css({ "display": "none" });
                $(".text-center").children().css({ "display": "none" });
                $('#btnStartProcess').fadeIn('slow');
                $('#btnStopProcess').fadeOut('slow');
            }
        });
    }
}

function ProgressBarModal(showHide) {
    if (showHide === 'show') {
        $('#btnStartProcess').fadeOut('fast');
        $('#btnStopProcess').fadeIn('fast');
        $("#runningService").css({ "display": "block" });
        $("#mod-progress").css({ "display": "inline-block" });
        $("#mod-progress fieldset").children().css({ "display": "inline-block" });
        $("#mod-progress,#modal-dialog").children().css({ "display": "inline-block" });
        $(".text-center").children().css({ "display": "block" });
        if (arguments.length >= 2) {
            $('#progressBarParagraph').text(arguments[1]);
        } else {
            $('#progressBarParagraph').text('U tijeku...');
        }
        window.progressBarActive = true;
    } else {
        $('#btnStopProcess').fadeOut('fast');
        $('#btnStartProcess').fadeIn('fast');
        $("fieldset").css({ "display": "none" });
        $("#runningService").css({ "display": "none" });
        running = false;
        $('#mod-progress').css('display', 'none');
        $("#mod-progress fieldset").children().css({ "display": "none" });
        $("#mod-progress,#modal-dialog").children().css({ "display": "none" });
        $(".text-center").children().css({ "display": "none" });
        window.progressBarActive = false;
    }
}
$(function () {
    var progress = $.connection.progressHub;
    console.log(progress);
    progress.client.addProgress = function (message, percentage) {
        ProgressBarModal("show", message + " " + percentage);
        $('#ProgressMessage').width(percentage);
        if (percentage.toString() == "000%") {
            ProgressBarModal("");
        }
    };
    $.connection.hub.start().done(function () {
        var connectionId = $.connection.hub.id;
        console.log(connectionId);
    });
});