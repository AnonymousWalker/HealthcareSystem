var doctorId = 0;
$(document).ready(function () {
    $("table.redTable > tbody > tr").click(function () {
        var urlString = $("#EditSalaryURL").val();
        doctorId = parseInt($(this).data("doctorid"));
        $.ajax({
            url: urlString,
            type: 'get',
            data: { doctorId },
            success: function (modal) {
                $("#modal-ajax").html(modal);
                $("#editModal").modal('toggle');
            }
        });
    });

    $("#modal-ajax").on("click", "#submit-btn", function (e) {
        e.preventDefault();
        var urlString = $("#EditSalaryURL").val();
        var salary = $("#Salary").val();
        if (salary == "" || parseInt(salary) < 0) {
            alert("Invalid input value!");
            return;
        }
        $.ajax({
            url: urlString,
            type: 'post',
            data: { DoctorId: doctorId, Salary: salary },
            success: function (isSuccess) {
                if (isSuccess) {
                    alert("Success");
                    $("#editModal").modal('toggle');
                }
                else {
                    alert("An error has occurred, please try again later.");
                }
            }
        });
    });
});

