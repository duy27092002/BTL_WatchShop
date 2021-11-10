$(document).ready(function () {
    $("#send-evaluate").on("click", function () {
        var ProdId = $("#MaSP").val();
        var CusId = $("#MaKH").val();
        var EvaluateDate = $("#ThoiGianDG").val();
        var Like = $("#LuotThich").val();
        var EvaluateContent = $("#LoiDG").val();
        var Point = $("input[name = Diem]:checked", "#form-evaluate").val();
        debugger;

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
                        swal("Đánh giá thành công!", "", "success");
                        $("#form-evaluate")[0].reset();  
                    }                
                },
                error: function (result) {
                    if (!result.success) {
                        swal("Đã có lỗi xảy ra!", "", "error");
                        $("#form-evaluate")[0].reset();
                    }
                }
            });
        }
    });
});