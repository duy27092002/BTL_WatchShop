// phân trang
$(document).ready(function () {
    // Nhiệm vụ 1: Load danh sách bài viết đánh giá bằng hàm LoadEvaluate() với mặc định ban đầu là load các bài viết của page = 1
    // Nhiệm vụ 2: Bắt sự kiện click vào các trang để lấy page, sau đó load các bài đánh giá thuộc page đó
    var prodId = $("#MaSP").val();

    // mặc định load trang đầu tiên
    LoadEvaluate(parseInt(prodId, 10), 1);

    // load các bài đánh giá theo trang
    $("#pagination-evaluate").on("click", "a", function (e) {
        e.preventDefault();
        page = $(this).attr("data-page");
        LoadEvaluate(parseInt(prodId, 10), parseInt(page, 10));
    });
});

// load danh sách bài đánh giá
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

                        html += '<div class="evaluateContent-' + val.MaDG + '">' + val.LoiDG + '</div>';
                        html += '<a href="javascript:void(0);" class="text-info readMore-' + val.MaDG + '" onclick="readMore(this)">Xem thêm</a>';
                        html += '<p class="text-muted mb-0"><small>Đã đánh giá vào ngày: ' + data.evaluateDate + '</small></p>';

                        if (data.sessionCusId != 0) {
                            if (data.sessionCusId == val.MaKH) {
                                html += '<p class="mb-0">';
                                html += '<small><a class="text-info edit-evaluate" href="" data-evaluateId="' + val.MaDG + '" data-prodId="' + val.MaSP + '" data-toggle="modal" data-target="#editEvaluateModal">Chỉnh sửa</a></small> |';
                                html += '<small><a class="text-info delete-evaluate" href="javascript:void(0);" data-evaluateId="' + val.MaDG + '" data-prodId="' + val.MaSP + '"> Xóa</a></small></p>';
                            }
                            html += '<button type="button" id="like-btn-' + val.MaDG + '" class="btn btn-outline-warning mt-3 like-btn" data-evlId="' + val.MaDG + '" data-clicked="false">';
                            html += '<span class="icon-' + val.MaDG + '"><i class="far fa-thumbs-up"></i></span> Hữu ích(<span class="count-' + val.MaDG + '">' + val.LuotThich + '</span>)';
                            html += '</button> <button type="button" class="btn btn-outline-info mt-3 comment-btn">Bình luận</button>';
                        } else {
                            html += '<a class="btn btn-outline-warning mt-3" href="/Customer/Login"><i class="far fa-thumbs-up"></i> Hữu ích (' + val.LuotThich + ')</a> ';
                            html += '<a class="btn btn-outline-info mt-3" href="/Customer/Login">Bình luận</a>';
                        }

                        html += '</div></div>';

                        $("#evaluate-box").append(html);

                        // sự kiện xem thêm nội dung hoặc thu gọn nội dung bài đánh giá
                        let noOfCharac = 200; // giới hạn ký tự cho lời đánh giá
                        let contents = document.querySelectorAll(".evaluateContent-" + val.MaDG); // lấy nội dung đánh giá
                        contents.forEach(content => {
                            // nếu nội dung đánh giá k vượt quá giới hạn cho phép
                            if (content.textContent.length < noOfCharac) {
                                // ẩn link "xem thêm"
                                content.nextElementSibling.style.display = "none";
                            }
                            else {
                                // biến chứa nội dung đánh giá đã bị giới hạn
                                let displayText = content.textContent.slice(0, noOfCharac);
                                // biến chứa 100% nội dung đánh giá
                                let moreText = content.textContent.slice(noOfCharac);
                                content.innerHTML = displayText + '<span class="dots">...</span><span class="hide more">' + moreText + '</span>';
                            }
                        });

                        // sự kiện tăng giảm hữu ích cho bài đánh giá
                        //$("#like-btn-" + val.MaDG).on("click", function () {
                        //    let clicked = $(this).attr("data-clicked");
                        //    let isTrueSet = (clicked === 'true'); // trả ra false
                        //    let likes = document.querySelector(".count-" + val.MaDG);
                        //    let likeIcon = document.querySelector(".icon-" + val.MaDG);

                        //    // nếu ấn thích (clicked = true)
                        //    if (!isTrueSet) {
                        //        clicked = true;
                        //        likeIcon.innerHTML = '<i class="fas fa-thumbs-up"></i>';
                        //        $.ajax({
                        //            type: "POST",
                        //            url: "/Product/UpdateLike",
                        //            data: {
                        //                EvlId: val.MaDG,
                        //                Clicked: clicked
                        //            },
                        //            dataType: "json",
                        //            success: function (result) {
                        //                if (result.success) {
                        //                    likes.textContent++;
                        //                    $("#like-btn-" + val.MaDG).data("clicked", true); // chưa thành công
                        //                }
                        //            },
                        //            error: function (result) {
                        //                if (!result.success) {
                        //                    swal("Lỗi!", "Vui lòng thử lại", "error");
                        //                }
                        //            }
                        //        });
                        //    } else { // nếu bỏ thích (clicked = false)
                        //        clicked = false;
                        //        likeIcon.innerHTML = '<i class="far fa-thumbs-up"></i>';
                        //        $.ajax({
                        //            type: "POST",
                        //            url: "/Product/UpdateLike",
                        //            data: {
                        //                EvlId: val.MaDG,
                        //                Clicked: clicked
                        //            },
                        //            dataType: "json",
                        //            success: function (result) {
                        //                if (result.success) {
                        //                    likes.textContent--;
                        //                    $("#like-btn-" + val.MaDG).data("clicked", false);
                        //                }
                        //            },
                        //            error: function (result) {
                        //                if (!result.success) {
                        //                    swal("Lỗi!", "Vui lòng thử lại", "error");
                        //                }
                        //            }
                        //        });
                        //    }
                        //});
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
        error: function (data) { }
    });
}

// tạo bài đánh giá mới
$(document).ready(function () {
    $(".open-evaluate-modal").click(function () {
        var productId = $(this).attr("data-evaluateProId");
        var orderId = $(this).attr("data-evaluateOrderId");

        $("#send-evaluate").on("click", function () {

            var OrderId = orderId;
            var ProdId = productId;
            var CusId = $("#MaKH").val();
            var EvaluateDate = $("#ThoiGianDG").val();
            var Like = $("#LuotThich").val();
            var EvaluateContent = $("#LoiDG").val();
            var Point = $("input[name=Diem]").filter(":checked").val();

            var getEvaluate = {
                MaDH: OrderId,
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
                    url: "/Product/Evaluate",
                    data: getEvaluate,
                    dataType: "json",
                    success: function (result) {
                        if (result.error) {
                            swal("Thông báo!", "Bạn đã đánh giá sản phẩm này rồi", "info");
                        } else if (result.success) {
                            swal("Đánh giá thành công!", "Cám ơn bạn đã đánh giá sản phẩm của chúng tôi", "success");
                        }
                        $("#form-evaluate")[0].reset();
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

// chỉnh sửa bài đánh giá
// hiển thị dữ liệu cần chỉnh sửa
$(document).on("click", ".edit-evaluate", function () {
    var getEvaluateId = $(this).attr("data-evaluateId");

    $.ajax({
        type: "POST",
        url: "/Product/EditEvaluate",
        data: {
            evaluateId: getEvaluateId
        },
        dataType: "json",
        success: function (result) {
            if (result.success) {
                $("#e-LoiDG").val(result.content);
                $('#diem-' + result.point).attr('checked', 'checked');
                $("#e-MaDG").val(result.evlId);
                $("#e-MaSP").val(result.productId);
            }
        },
        error: function (result) {
            if (!result.success) {
                swal("Đã có lỗi xảy ra!", "Vui lòng thử lại", "error");
            }
        }
    });
});

// sự kiện lưu sau khi chỉnh sửa bài đánh giá
$(document).on("click", "#update-evaluate", function () {
    var evaluateId = $("#e-MaDG").val();
    var evaluateDate = $("#e-ThoiGianDG").val();
    var evaluateContent = $("#e-LoiDG").val();
    var point = $("input[name=e-Diem]").filter(":checked").val();
    var prodId = $("#e-MaSP").val();

    if (evaluateContent.length == 0 || point == null) {
        swal({
            title: "Lỗi dữ liệu!",
            text: "Xin hãy nhập đầy đủ dữ liệu",
            icon: "warning",
            buttons: "Đã hiểu",
        });
    } else {
        $.ajax({
            type: "POST",
            url: "/Product/UpdateEvaluate",
            data: {
                evaluateId: evaluateId,
                evaluateDate: evaluateDate,
                evaluateContent: evaluateContent,
                point: point
            },
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    swal("Sửa thành công!", "", "success");
                    LoadEvaluate(parseInt(prodId, 10), 1);
                }
            },
            error: function (result) {
                if (!result.success) {
                    swal("Đã có lỗi xảy ra!", "Vui lòng thử lại", "error");
                }
            }
        });
    }
});

// sự kiện xóa bài đánh giá
$(document).on("click", ".delete-evaluate", function () {
    var getEvaluateId = $(this).attr("data-evaluateId");
    var prodId = $(this).attr("data-prodId");

    $.ajax({
        type: "POST",
        url: "/Product/DeleteEvaluate",
        data: {
            evaluateId: getEvaluateId
        },
        dataType: "json",
        success: function (result) {
            if (result.success) {
                swal("Xóa thành công!", "", "success");
                LoadEvaluate(parseInt(prodId, 10), 1);
            }
        },
        error: function (result) {
            if (!result.success) {
                swal("Đã có lỗi xảy ra!", "Vui lòng thử lại", "error");
            }
        }
    });
});

// sự kiện xem thêm hoặc thu gọn nội dung bình luận
function readMore(elm) {
    let post = elm.parentElement;
    post.querySelector(".dots").classList.toggle("hide");
    post.querySelector(".more").classList.toggle("hide");
    elm.textContent == "Thu gọn" ? elm.textContent = "Xem thêm" : elm.textContent = "Thu gọn";
}