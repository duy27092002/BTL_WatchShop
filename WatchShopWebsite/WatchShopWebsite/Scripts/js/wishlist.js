$(document).ready(function () {
    // thêm sản phẩm vào danh sách yêu thích
    $("#addtowishlist").click(function () {
        var productInfo = {};
        //lấy id sản phẩm
        productInfo.Id = $("#productid").val();
        $.ajax({
            type: "POST",
            url: "/Wishlist/AddToWishlist/",
            data: JSON.stringify(productInfo),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                alert("Đã thêm vào danh sách yêu thích!");
            },
            error: function () {
                alert("Đã có lỗi xảy ra trong khi thêm vào danh sách yêu thích!");
            }
        });
        return false;
    });

    // xóa sản phẩm khỏi danh sách yêu thích
    $("#removewishlist").click(function () {
        var info = {};
        //lấy id sản phẩm
        info.Id = $("#productId").val();
        $.ajax({
            type: "POST",
            url: "/Wishlist/RemoveWishlist/",
            data: JSON.stringify(info),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                $("#prod-" + info.Id).remove();
                alert("Đã xóa thành công!");
            },
            error: function () {
                alert("Đã có lỗi xảy ra trong quá trình xóa!");
            }
        });
        return false;
    });
});
