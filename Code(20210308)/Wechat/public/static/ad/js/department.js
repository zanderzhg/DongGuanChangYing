// 后退按钮
$("#btn-back").click( function () {
	window.history.back(-1);
});
// 点击删除选定记录按钮
$("#department-del").click( function () {
	// 获取选定数据
	var res = $('input:checked');
	var data = new Array();
	// 弹出确认框
	if ( confirm('是否删除选定部门，删除后数据无法恢复！')){
		if (res.length <= 0)
		{
			alert('请先选择数据');
			return false;
		}
		for(var i=0; i<res.length; i++)
		{
			data[i] = $(res[i]).val();
		}
		$.post('departmentDel',{'id':data},function(msg){
			window.location.reload();
		});
	}
});

// 点击清空所有预约记录按钮
$("#department-del-all").click( function () {
	if( confirm("是否清空所有部门，清空数据无法恢复！") ){
		$.post('departmentDelAll',function(msg){
			// 刷新页面
			window.location.reload();
		});
	}
});	
