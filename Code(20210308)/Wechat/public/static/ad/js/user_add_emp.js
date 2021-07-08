// 后退按钮
$("#btn-back").click( function () {
	window.history.back(-1);
});

// 提交按钮
$("#btn-sub").click( function () {
	var name   = $("#name").val();
	var phone  = $("#phone").val();
	var idcard = $("#idcard").val();

	// swal("Hello world!", {
	// 	timer: 2000, 
	//   	buttons: false,
	// });

	// swal("干得漂亮！", "你点击了按钮！","success")

	// swal({ 
	//   title: "自动关闭弹窗！", 
	//   text: "2秒后自动关闭。", 
	//   timer: 2000, 
	//   buttons:false
	//   // showConfirmButton: false 
	// });

 //    swal({ 
	//   title: "Error!", 
	//   text: "姓名不能为空",
	//   type: "error", 
	//   confirmButtonText: "Cool" 
	// });
	// alert('6666');
	// return false;
	// 暂时不对数据判断
});
