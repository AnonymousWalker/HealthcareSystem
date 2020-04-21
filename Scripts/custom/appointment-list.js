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
});