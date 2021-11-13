$(document).ready(function () {
    // thêm sản phẩm vào danh sách yêu thích
    $(".addtowishlist").click(function () {
        var productInfo = {};
        //lấy id sản phẩm
        productInfo.Id = $(this).data("productid");
        
        $.ajax({
            type: "POST",
            url: "/Wishlist/AddToWishlist/",
            data: JSON.stringify(productInfo),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                swal("Đã lưu thành công!", "", "success");
            },
            error: function () {
                swal("Đã có lỗi xảy ra!", "", "error");
            }
        });
        return false;
    });

    // xóa sản phẩm khỏi danh sách yêu thích
    $(".removewishlist").click(function () {
        var info = {};
        //lấy id sản phẩm
        info.Id = $(this).data("productid");

        $.ajax({
            type: "POST",
            url: "/Wishlist/RemoveWishlist/",
            data: JSON.stringify(info),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                $("#prod-" + info.Id).remove();
                swal("Đã xóa thành công!", "", "success");
            },
            error: function () {
                swal("Đã có lỗi xảy ra!", "", "error");
            }
        });
        return false;
    });
});
