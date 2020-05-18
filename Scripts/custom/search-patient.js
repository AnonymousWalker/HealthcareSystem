$(document).ready(function () {

    $("#search-btn").click(function () {
        var firstName = $("#fname-input").val();
        var lastName = $("#lname-input").val();
        var phone = $("#phone-input").val();
        var urlString = $("#SearchURL").val();

        $.ajax({
            url: urlString,
            type: 'post',
            data: { firstName, phone, lastName },
            success: function (result) {
                $("#patient-table").html(result);
            }
        });
    });

    

    $("#patient-table").on("click","tbody > tr",function () {
        var patientId = $(this).data("patientid");
        var action = $("#ActionType").val();
        if (action != "") {
            var urlString;
            var role = $("#Role").val();
            if (action == "make-apt") {
                urlString = $("#MakeAppointmentURL").val();
                window.location.href = urlString + "?patientid=" + patientId;
            }
            else if (action == "view" && role != "Staff") {
                urlString = $("#PatientMedicalRecordURL").val();
                window.location.href = urlString + "?patientid=" + patientId;
            }
            else if (action == "input" && (role == "Doctor" || role == "Nurse")) {
                urlString = $("#InputRecordURL").val();
                window.location.href = urlString + "?patientid=" + patientId;
            }
        }
    });

});


