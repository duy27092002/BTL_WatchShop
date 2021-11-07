// xem thêm hoặc thu gọn nội dung chi tiết sản phẩm
var getHeight = $(".content").css("height");
if (parseInt(getHeight, 10) >= 500) {
    $(".content-box").addClass("readLess");
    $("#btn-read-more").prop('type', 'button');

    $("#btn-read-more").on("click", function () {
        var getValueOfReadMoreBtn = $("#btn-read-more").val();

        if (getValueOfReadMoreBtn == "Thu gọn") {
            $(".content-box").addClass("readLess");
            $("#btn-read-more").val("Xem thêm nội dung");
        } else {
            $(".content-box").removeClass("readLess");
            $("#btn-read-more").val("Thu gọn");
        }
    });
}