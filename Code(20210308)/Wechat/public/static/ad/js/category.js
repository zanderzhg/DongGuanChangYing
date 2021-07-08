// 后退按钮
$("#btn-back").click( function () {
	window.history.back(-1);
});
// 点击添加类型按钮
$("#btn-sub").click( function () {
	// 获取选定数据
	var category = $.trim($('#category').val());
	var value = $.trim($('#value').val());
	if(category=='')
	{
		alert('类型名称不能为空！');
		$('#category').focus();
		return false;
	}
	if(value=='')
	{
		alert('类型值不能为空！');
		$('#value').focus();
		return false;
	}
	return true;
});
// 点击删除选定记录按钮
$("#category-del").click( function () {
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
		$.post('categoryDel',{'id':data},function(msg){
			window.location.reload();
		});
	}
});

// 点击清空所有预约记录按钮
$("#category-del-all").click( function () {
	if( confirm("是否清空所有部门，清空数据无法恢复！") ){
		$.post('categoryDelAll',function(msg){
			// 刷新页面
			window.location.reload();
		});
	}
});	
