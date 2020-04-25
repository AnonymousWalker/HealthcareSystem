$(document).ready(function () {
    $(".cancel-ap-btn").click(function () {
        var $btn = $(this);
        var appointmentId = parseInt($btn.data("appointmentid"));
        var urlString = $("#CancelURL").val();

        $.ajax({
            url: urlString,
            type: "get",
            data: { appointmentId },
            success: function (isCancelled) {
                if (isCancelled) {
                    $btn.closest("tr").remove();
                }
            }
        });
    });

    $("#dr-ap-table > tbody > tr").click(function () {
        var urlString = $("#UpdateServiceTreatmentURL").val();
        var appointmentId = $(this).data("appointmentid");
        if (urlString != "" && appointmentId != undefined) {
            window.location.href = urlString + "?appointmentId=" + appointmentId;
        }
    });

    $("#med-record-table > tbody > tr").click(function () {
        var urlString = $("#EditMedicalRecordURL").val();
        var recordId = $(this).data("recordid");
        if (recordId != "" && urlString != "") {
            window.location.href = urlString + "?recordId=" + recordId;
        }
    });
});