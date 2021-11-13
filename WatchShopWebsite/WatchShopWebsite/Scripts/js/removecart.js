$(document).ready(function () {
    getTotal();

    // xóa sản phẩm khỏi giỏ hàng
    $(".removecart").click(function () {
        var model = {};
        //lấy id sản phẩm
        model.Id = $(this).data("productid");
        // lấy số sản phẩm hiện tại trong giỏ hàng
        var getCartCount = $("#CartCountHidden").val();
        $.ajax({
            type: "POST",
            url: "/Cart/Remove/",
            data: JSON.stringify(model),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                $('#CartCount').text(getCartCount - 1);
                $("#product-" + model.Id).remove();
                getTotal();
            },
            error: function () {
                alert("Lỗi trong khi xóa sản phẩm trong giỏ hàng!");
            }
        });
    });

    // tính tổng tiền = giá tiền (/1sp) * số lượng sp
    // nếu có nhiều sp thì cộng dồn tổng tiền của mỗi sp
    function getTotal() {
        var trs = $('#tb-cart tbody').find('tr');
        var total = 0;
        $.each(trs, function (i, e) {
            var tds = $(e).find('td');
            var val = $(tds[tds.length - 2]).children(0).text();
            total += parseInt(formatString(val));
        });

        if (isNaN(total)) {
            $('#total-cart').text("0 VNĐ");
        } else {
            $('#total-cart').text(formatNumber(total) + " VNĐ");
        }
    }
});

function formatNumber(nStr) {
    var groupSeperate = ',';
    var x = formatString(nStr);
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x)) {
        x = x.replace(rgx, '$1' + groupSeperate + '$2');
    }
    return x;
}

function formatString(money) {
    var x = '';
    var arr = money.toString().split(',');
    arr.forEach((str) => {
        x += str;
    });
    return x;
}