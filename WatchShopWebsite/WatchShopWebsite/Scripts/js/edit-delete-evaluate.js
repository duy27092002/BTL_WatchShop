$(document).ready(function () {
    $(".edit-evaluate").click(function () {
        var getEvaluateId = $(this).attr("data-evaluateId");

        $.ajax({
            type: "POST",
            url: "/Product/EditEvaluate/",
            data: {
                evaluateId: getEvaluateId
            },
            dataType: "json",
            success: function (result) {},
            error: function (result) {
                if (!result.success) {
                    swal("Đã có lỗi xảy ra!", "Vui lòng thử lại", "error");
                }
            }
        });
    });

    $(".delete-evaluate").click(function () {
        var getEvaluateId = $(this).attr("data-evaluateId");

        $.ajax({
            type: "POST",
            url: "/Product/EditAndDeleteEvaluate/",
            data: {
                evaluateId: getEvaluateId
            },
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    swal("Xóa thành công!", "", "success");
                }
            },
            error: function (result) {
                if (!result.success) {
                    swal("Xóa thất bại!", "Vui lòng thử lại", "error");
                }
            }
        });
    });
});