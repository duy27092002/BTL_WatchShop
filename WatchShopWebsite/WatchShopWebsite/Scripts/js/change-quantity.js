$(document).ready(function () {
	$("#button-minus").on("click", function () {
		let quantity = $("#quantity").val();
		quantity--;
		if (quantity < 1) { quantity = 1 };
		$("#quantity").val(quantity);
    });

	$("#button-plus").on("click", function () {
		let quantity = $("#quantity").val();
		let max = $("#SoLuong").val();
		quantity++;
		if (quantity > max) { quantity = max };
		$("#quantity").val(quantity);
	});
});