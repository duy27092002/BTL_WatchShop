$(document).ready(function () {

    $(".open-evaluate-modal").click(function () {
        var productId = $(this).attr("data-evaluateProId");

        $("#send-evaluate").on("click", function () {

            var ProdId = productId;
            var CusId = $("#MaKH").val();
            var EvaluateDate = $("#ThoiGianDG").val();
            var Like = $("#LuotThich").val();
            var EvaluateContent = $("#LoiDG").val();
            var Point = $("input[name=Diem]").filter(":checked").val();

            var getEvaluate = {
                MaSP: ProdId,
                MaKH: CusId,
                ThoiGianDG: EvaluateDate,
                LuotThich: Like,
                LoiDG: EvaluateContent,
                Diem: Point,
            };

            if (EvaluateContent == "" || Point == null) {
                swal({
                    title: "Lỗi dữ liệu!",
                    text: "Xin hãy nhập đầy đủ dữ liệu",
                    icon: "warning",
                    buttons: "Đã hiểu",
                });
            } else {
                $.ajax({
                    type: "POST",
                    url: "/Product/Evaluate/",
                    data: getEvaluate,
                    dataType: "json",
                    // có dòng lệnh này sẽ lỗi
                    //contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        if (result.success) {
                            swal("Đánh giá thành công!", "Cám ơn bạn đã đánh giá sản phẩm của chúng tôi", "success");
                            $("#form-evaluate")[0].reset();
                            $("#send-evaluate").remove();
                        }
                    },
                    error: function (result) {
                        if (!result.success) {
                            swal("Đánh giá thất bại!", "Vui lòng thử lại", "error");
                            $("#form-evaluate")[0].reset();
                        }
                    }
                });
            }
        });
    });
});