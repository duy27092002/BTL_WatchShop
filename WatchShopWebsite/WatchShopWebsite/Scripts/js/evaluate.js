$(document).ready(function () {
    // Nhiệm vụ 1: Load danh sách bài viết đánh giá bằng hàm LoadEvaluate() với mặc định ban đầu là load các bài viết của page = 1
    // Nhiệm vụ 2: Bắt sự kiện click vào các trang để lấy page, sau đó load các bài đánh giá thuộc page đó
    var prodId = $("#MaSP").val();

    LoadEvaluate(parseInt(prodId, 10), 1);

    $("#pagination-evaluate").on("click", "a", function (e) {
        e.preventDefault();
        page = $(this).attr("data-page");
        LoadEvaluate(parseInt(prodId, 10), parseInt(page, 10));
    });
});

function LoadEvaluate(prodId, page) {
    $.ajax({
        url: "/Product/PaginationEvaluate",
        type: "GET",
        data: {
            id: prodId,
            page: page,
        },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                $("#evaluate-box").empty();
                if (data.evaluateCount <= 0) {
                    let html = '<div class="d-flex justify-content-center"><b>Chưa có đánh giá nào!</b></div>';
                    $("#evaluate-box").append(html);
                } else {
                    $.each(data.evaluateRecord, function (key, val) {
                        let html = '<div class="row p-3">';
                        html += '<div class="col-12 col-sm-12 col-md-5 col-lg-5 mb-3">';

                        let getAvatar = val.Avatar.replace("~", "../../..");

                        html += '<p><img src="' + getAvatar + '" class=" rounded-circle" style="width:50px" /><span class="ml-3"><b>' + val.TenKH + '</b></span></p></div>';
                        html += '<div class="col-12 col-sm-12 col-md-7 col-lg-7 mb-3">';
                        html += '<p class="mb-0">';

                        for (let i = 0; i < val.Diem; i++) {
                            html += '<span class="fa fa-star checked"></span>';
                        }

                        for (let j = 0; j < 5 - val.Diem; j++) {
                            html += '<span class="fa fa-star not-checked"></span>';
                        }

                        let status = val.Diem == 5 ? "Cực kỳ hài lòng" :
                            val.Diem == 4 ? "Hài lòng" :
                                val.Diem == 3 ? "Bình thường" :
                                    val.Diem == 2 ? "Không hài lòng" : "Rất không hài lòng";

                        html += '<span class="ml-3"><b>' + status + '</b></span>';
                        html += '</p><p class="text-success mb-3"><i class="fas fa-check-circle"></i> Đã mua hàng</p>';
                        html += val.LoiDG;

                        html += '<p class="text-muted mb-0"><small>Đã đánh giá vào ngày: ' + val.ThoiGianDG + '</small></p>';

                        if (data.sessionCusId != 0) {
                            if (data.sessionCusId == val.MaKH) {
                                html += '<p class="mb-0">';
                                html += '<small><a class="text-info edit-evaluate" href="" data-evaluateId="' + val.MaDG + '" data-toggle="modal" data-target="#editEvaluateModal">Chỉnh sửa</a></small> |';
                                html += '<small><a class="text-info delete-evaluate" href="" data-evaluateId="' + val.MaDG + '">Xóa</a></small></p>';
                            }
                            html += '<button type="button" id="like-btn-' + val.MaDH + '" class="btn btn-outline-warning mt-3 like-btn" data-likeBtn="like-btn-' + val.MaDH + '" data-prodId="' + val.MaSP + '" data-orderId="' + val.MaDH + '" data-cusId="' + val.MaKH + '">';
                            html += '<span class="icon-' + val.MaDH + '"><i class="far fa-thumbs-up"></i></span> Hữu ích(<span class="count-' + val.MaDH + '">' + val.LuotThich + '</span>)';
                            html += '</button> <button type="button" class="btn btn-outline-info mt-3 comment-btn">Bình luận</button>';
                        } else {
                            html += '<a class="btn btn-outline-warning mt-3" href="/Customer/Login"><i class="far fa-thumbs-up"></i> Hữu ích (' + val.LuotThich + ')</a> ';
                            html += '<a class="btn btn-outline-info mt-3" href="/Customer/Login">Bình luận</a>';
                        }

                        html += '</div></div>';

                        $("#evaluate-box").append(html);
                    });

                    // phân trang
                    if (data.pageCount > 1) {
                        $("#pagination-evaluate").empty();

                        let li = '';
                        if (data.currentPage > 3) {
                            li += '<li class="page-item"><a class="page-link" data-page="1" href="">Trang đầu</a></li>';
                        }
                        else if (data.currentPage > 1) {
                            let prevPage = data.currentPage - 1;
                            li += '<li class="page-item"><a class="page-link" data-page="' + prevPage + '" href="">«</a></li>';
                        }

                        for (var i = 1; i <= data.pageCount; i++) {
                            if (i != data.currentPage) {
                                if (i > data.currentPage - 3 && i < data.currentPage + 3) {
                                    li += '<li class="page-item"><a class="page-link" data-page="' + i + '" href="">' + i + '</a></li>';
                                }
                            }
                            else {
                                li += '<li class="active page-item"><a class="page-link" data-page="' + i + '" href="">' + i + '</a></li>';
                            }
                        }

                        if (data.currentPage <= data.pageCount - 1) {
                            let nextPage = data.currentPage + 1;
                            li += '<li class="page-item"><a class="page-link" data-page="' + nextPage + '" href="">»</a></li>';
                        }
                        else if (data.currentPage < data.pageCount - 3) {
                            li += '<li class="page-item"><a class="page-link" data-page="' + data.pageCount + '" href="">Trang cuối</a></li>';
                        }

                        $("#pagination-evaluate").append(li);
                    }
                }
            }
        },
        error: function (data) {}
    });
}