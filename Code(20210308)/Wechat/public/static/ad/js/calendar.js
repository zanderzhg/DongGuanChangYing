// 后退按钮
$("#btn-back").click( function () {
	window.history.back(-1);
});
/*批量录入排期信息*/
$("#paiqi-import").click( function (){
	// 弹出选择框
	$('#login').modal('show');
});

// 导入排期信息
$("#paiqi-import-submit").click( function () {
	var excelData = $("#excelData").val();
	if( !excelData )
	{
		alert('请先选择数据');return false;
	}
});

// 导出排期信息
$("#paiqi-export").click( function () {
	// if(comfirm('是否导出本月排期信息'))
	// {
		$.post('calendarExport',function(msg){
			window.location.href = '/excel/' + msg;
		});
	// }
});