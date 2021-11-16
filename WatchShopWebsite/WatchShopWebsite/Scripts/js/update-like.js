$(document).ready(function () {

    var clicked = false;
    $(".like-btn").click(function () {
        var orderId = $(this).attr("data-orderId");
        var cusId = $(this).attr("data-cusId");
        var prodId = $(this).attr("data-prodId");

        var likes = document.querySelector(".count-" + orderId);
        var likeIcon = document.querySelector(".icon-" + orderId);
        var likeBtn = $(this).attr("data-likeBtn");

        // nếu ấn thích (clicked = true)
        if (!clicked) {
            //clicked = true;
            likeIcon.innerHTML = '<i class="fas fa-thumbs-up"></i>';
            $.ajax({
                type: "POST",
                url: "/Product/UpdateLike/",
                data: {
                    OrderId: orderId,
                    CusId: cusId,
                    ProdId: prodId,
                    Clicked: true
                },
                dataType: "json",
                success: function (result) {
                    if (result.success) {
                        likes.textContent++;
                        clicked = true;
                    }
                },
                error: function (result) {
                    if (!result.success) {
                        swal("Lỗi!", "Vui lòng thử lại", "error");
                    }
                }
            });
        } else { // nếu bỏ thích (clicked = false)
            //clicked = false;
            likeIcon.innerHTML = '<i class="far fa-thumbs-up"></i>';
            $.ajax({
                type: "POST",
                url: "/Product/UpdateLike/",
                data: {
                    OrderId: orderId,
                    CusId: cusId,
                    ProdId: prodId,
                    Clicked: false
                },
                dataType: "json",
                success: function (result) {
                    if (result.success) {
                        likes.textContent--;
                        clicked = false;
                    }
                },
                error: function (result) {
                    if (!result.success) {
                        swal("Lỗi!", "Vui lòng thử lại", "error");
                    }
                }
            });
        }
    });
});