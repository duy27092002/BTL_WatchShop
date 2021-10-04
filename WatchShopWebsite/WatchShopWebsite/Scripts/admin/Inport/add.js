$(document).ready(function () {
    // ấn nút thêm sản phẩm ở thêm mới hóa đơn nhập
    $("#btn-add-items").click(function () {

        getProducts(getIds(), getCategoryId());

        $("#modal-data").modal('toggle');
    });

    // nút hủy ở modal để thoát ra
    $("#btn-close").click(function () {
        $("#modal-data").modal('toggle');
    });

    // dropdown thay đổi với giá trị tương ứng theo mã danh mục
    $('#select-category').change(function () {
        getProducts(getIds(), getCategoryId());
    });

    // nút thêm trong modal khi chọn xong sản phẩm
    $('#btn-modal-accept').click(function () {
        var cbs = $('#tb-add-items tbody').find('.cb');
        var html = '';
        $.each(cbs, function (i, e) {
            if ($(e).is(':checked')) {
                var tds = $(e).parent().parent().find('td');
                html += '<tr>' +
                    '<td>' + $(tds[0]).text() + '</td>' +
                    '<td>' + $(tds[1]).text() + '</td>' +
                    '<td>' + $('#select-category option:selected').text() + '</td>' +
                    '<td>' + $(tds[2]).text() + '</td>' +
                    '<td> <input class = "form-control num text-center" /></td>' +
                    '<td> <input class = "form-control num text-center" /></td>' +
                    '<td> <input class = "form-control num text-center bg-white" readonly /></td>' +
                    '<td style = "width:1px;"><button class = "btn btn-sm btn-danger btn-delete">Xóa</button></td>' +
                    '</tr>'
            }
        });

        $('#tb-items').append(html);

        getProducts(getIds(), getCategoryId());
    });

    // xóa sp không mong muốn
    $('body').on('click', '.btn-delete', function () {
        $(this).parent().parent().remove();
        getTotal();
    });

    $('body').on('keyup', '.num', function () {
        var total = 0;

        $(this).val(formatNumber(formatString($(this).val())));
        var tds = $($(this).parent().parent()).find('td');
        var value = parseInt(formatString($(tds[tds.length - 3]).children(0).val())) *
            parseInt(formatString($(tds[tds.length - 4]).children(0).val()))
        $(tds[tds.length - 2]).children(0).val(formatNumber(value));

        getTotal();
    });

    // nút tạo hóa đơn nhập
    $('#btn-submit').click(function () {
        // tạo ra đối tượng hóa đơn nhập
        var info = {
            ThoiGianNhap: $('#ThoiGianNhap').val(),
            MaNCC: $('#MaNCC').val(),
            MaNV: $('#MaNV').val()
        };

        var details = [];

        // tìm ra các dữ liệu trong bảng chi tiết
        var trs = $('#tb-items tbody').find('tr');

        // tạo ra danh sách đối tượng chi tiết hóa đơn nhập
        $.each(trs, function (i, e) {
            // tìm tất cả các cột dữ liệu trong bảng
            var tds = $(e).find('td');

            var detail = {
                MaSP: $(tds[0]).text(),
                SoLuong: formatString($($(tds[4]).children(0)).val()),
                Gia: formatString($($(tds[5]).children(0)).val())
            };

            details.push(detail);
        });

        add(info, details);
    });

    // nút cập nhật hóa đơn nhập
    $('#btn-update').click(function () {
        // tạo ra đối tượng hóa đơn nhập
        var info = {
            ThoiGianNhap: $('#ThoiGianNhap').val(),
            MaNCC: $('#MaNCC').val(),
            MaNV: $('#MaNV').val()
        };

        var details = [];

        // tìm ra các dữ liệu trong bảng chi tiết
        var trs = $('#tb-items tbody').find('tr');

        // tạo ra danh sách đối tượng chi tiết hóa đơn nhập
        $.each(trs, function (i, e) {
            // tìm tất cả các cột dữ liệu trong bảng
            var tds = $(e).find('td');

            var detail = {
                MaSP: $(tds[0]).text(),
                SoLuong: formatString($($(tds[4]).children(0)).val()),
                Gia: formatString($($(tds[5]).children(0)).val())
            };

            details.push(detail);
        });

        update(info, details);
    });
});

// lấy mã đồng hồ
function getIds() {
    var trs = $('#tb-items tbody').find('tr');
    var arr = [];
    $.each(trs, function (i, e) {
        var val = parseInt($($(e).find('td')[0]).text());
        arr.push(val);
    });
    return arr;
}

// lấy mã danh mục để truyền vào getProducts() với vai trò là madm
function getCategoryId() {
    return $('#select-category').val();
}

// lấy mã (ids), tên đồng hồ, tên nhà sản xuất theo mã danh mục (sử dụng ajax)
function getProducts(ids, madm) {
    $.ajax({
        contentType: 'application/json; charset = utf-8',
        url: "/Products/GetAnother",
        type: "Get",
        dataType: "json",
        data: {
            json: JSON.stringify(ids),
            madm: madm
        },
        success: function (result) {
            var data = result.data;
            var html = '';
            $.each(data, function (i, e) {
                html += '<tr>' +
                    '<td>' + e.MaSP + '</td>' +
                    '<td>' + e.TenSP + '</td>' +
                    '<td>' + e.NSX + '</td>' +
                    '<td class = "text-center"> <input type = "checkbox" class = "cb" /></td>'
                '<tr>';
            });
            $('#tb-add-items tbody').empty().append(html);
        },
        error: function (data) {
            console.log("error");
        }
    });
}

// định dạng số (ví dụ: 1000 => 1,000)
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

// tính tổng tiền = giá tiền (/1sp) * số lượng sp
// nếu có nhiều sp thì cộng dồn tổng tiền của mỗi sp
function getTotal() {
    var trs = $('#tb-items tbody').find('tr');
    var total = 0;
    $.each(trs, function (i, e) {
        var tds = $(e).find('td');
        var val = $(tds[tds.length - 2]).children(0).val();
        total += parseInt(formatString(val));
    });
    $('#total').text("Tổng Tiền: " + formatNumber(total));
}

function add(info, details) {
    $.ajax({
        url: "/Inports/CreateConfirmed/",
        type: "POST",
        dataType: "json",
        data: {
            info: info,
            json: JSON.stringify(details)
        },
        success: function (result) {
            window.location = "/Admin/Inports/Index";
        },
        error: function (data) {
            console.log("error");
        }
    });
}

function update(info, details) {
    $.ajax({
        url: "/Inports/EditConfirmed/",
        type: "POST",
        dataType: "json",
        data: {
            info: info,
            json: JSON.stringify(details)
        },
        success: function (result) {
            window.location = "/Admin/Inports/Index";
        },
        error: function (data) {
            console.log("error");
        }
    });
}