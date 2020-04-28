$(document).ready(function () {
    $(".cancel-ap-btn").click(function () {
        var $btn = $(this);
        var appointmentId = parseInt($btn.data("appointmentid"));
        var patientId = $("#PatientId").val();
        var urlString = $("#CancelURL").val();

        $.ajax({
            url: urlString,
            type: "post",
            data: { appointmentId, patientId },
            success: function (isCancelled) {
                if (isCancelled== "True") {
                    $btn.closest("tr").remove();
                } else {
                    alert("You cannot cancel this appointment at the moment!");
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