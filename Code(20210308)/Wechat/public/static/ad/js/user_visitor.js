// 后退按钮
$("#btn-back").click( function () {
	window.history.back(-1);
});
function visitorAdd()
{
	window.location.href='User/visitorAdd';
}

// 提交按钮
$("#btn-sub").click( function () {
	var name   = $("#name").val();
	var phone  = $("#phone").val();
	var idcard = $("#idcard").val();

	// 暂时不对数据判断
});
// 删除多个访客
// 有待完善
$("#visitor-del").click( function () {
	var res = $('input:checked');
	var data = new Array();
	if (confirm("是否删除选中访客信息")) 
	{
		if (res.length <= 0)
		{
			alert('请先选择数据');
			return false;
		}		

		for(var i=0; i<res.length; i++)
		{
			data[i] = $(res[i]).val();
		}
		$.post('visitorDel',{'id':data},function(msg){
			window.location.reload();
		});
	}
});
// 删除所有访客
$("#visitor-delete-all").click( function() {
	if( confirm("是否清空所有访客信息") ){
		$.post('visitorDeleteAll',function(msg){
			// 刷新页面
			window.location.reload();
		});
	}
});
// 导出所有访客
$("#visitor-exportall").click( function () {
	if (confirm("是否导出所有访客信息")) 
	{
		$.post('visitorExportAll',function(msg){
			window.location.href = "/excel/"+msg;
		});
	}
});

/*导出选中访客*/
$("#visitor-export").click( function () {
	var res = $('input:checked');
	var data = new Array();

	/*弹出提示框*/
	if (confirm("是否导出选中访客信息"))
	{
		// 没有选择数据
		if(res.length <= 0)
		{
			alert('请先选择数据');
			return false;
		}
		// 循环获取选中的id
		for(var i=0; i<res.length; i++)
		{
			data[i] = $(res[i]).val();
		}
		// 异步请求
		$.post('visitorExport',{'id':data},function(msg){
			// 下载excel文件
			window.location.href = "/excel/"+msg;
			// 取消全选，关键是去掉样式类
			for(var i=0; i<res.length; i++)
			{
				$(res[i]).parent().removeClass('checked');
				$(res[i]).parent().parent().parent().removeClass('selected');
			}
		});
	}
});

/*批量录入访客信息*/
$("#visitor-import").click( function (){
	// 弹出选择框
	$('#login').modal('show');
});

/*点击查询按钮*/
$("#check")