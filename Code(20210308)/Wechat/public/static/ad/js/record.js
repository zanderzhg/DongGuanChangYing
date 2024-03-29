// 后退按钮
$("#btn-back").click( function () {
	window.history.back(-1);
});
// 点击删除选定记录按钮
$("#record-del").click( function () {
	// 获取选定数据
	var res = $('input:checked');
	var data = new Array();
	// 弹出确认框
	if ( confirm('是否删除选定预约记录，删除后数据无法恢复！')){
		if (res.length <= 0)
		{
			alert('请先选择数据');
			return false;
		}
		for(var i=0; i<res.length; i++)
		{
			data[i] = $(res[i]).val();
		}
		$.post('recordDel',{'id':data},function(msg){
			window.location.reload();
		});
	}
});

// 点击清空所有预约记录按钮
$("#record-del-all").click( function () {
	var type = $("#type").val();
	if( confirm("是否清空所有预约记录，清空数据无法恢复！") ){
		$.post('recordDelAll',{'type':type},function(msg){
			// 刷新页面
			window.location.reload();
		});
	}
});	
// 点击导出选定记录
$("#record-export").click( function () {
	var res = $('input:checked');
	var data = new Array();
	if( confirm("是否导出选中预约记录")) {

		if (res.length <= 0)
		{
			alert('请先选择数据');
			return false;
		}	
		for(var i=0; i<res.length; i++)
		{
			data[i] = $(res[i]).val();
		}
		$.post('recordExport',{'id':data},function(msg){
			
			// 下载excel文件
			window.location.href = "/excel/"+msg;
			// 取消全选，关键是去掉样式类
			for(var i=0; i<res.length; i++)
			{
				$(res[i]).parent().removeClass('checked');
				$(res[i]).parent().parent().parent().removeClass('selected');
			}

			// $(".bulk-actions:firsts").hide();

		});
	}
});

// 点击导出所有预约记录
$("#record-export-all").click( function () {

	if( confirm("是否导出所有预约记录")) {
		$.post('recordExportAll',function(msg){
			// 下载excel文件
			window.location.href = "/excel/"+msg;

		});
	}
});
