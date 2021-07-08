// 后退按钮
$("#btn-back").click( function () {
	window.history.back(-1);
});
// 点击删除选定记录按钮
$("#account-del").click( function () {
	// 获取选定数据
	var res = $('input:checked');
	var data = new Array();
	// 弹出确认框
	if ( confirm('是否删除选定管理员账号，删除后数据无法恢复！')){
		if (res.length <= 0)
		{
			alert('请先选择数据');
			return false;
		}
		for(var i=0; i<res.length; i++)
		{
			data[i] = $(res[i]).val();
		}
		$.post('accountDel',{'id':data},function(msg){
			window.location.reload();
		});
	}
});

// 点击清空所有预约记录按钮
$("#account-del-all").click( function () {
	if( confirm("是否清空所有管理员账号，清空数据无法恢复！") ){
		$.post('accountDelAll',function(msg){
			// 刷新页面
			window.location.reload();
		});
	}
});	

// 点击添加管理员信息按钮
$("#btn-add").click( function () {
	var username = $('#username').val();
	var password = $('#password').val();
	var remark   = $('#remark').val();
	if(username=='')
	{
		$('#username').focus();
		return false;
	}
	if(password=='')
	{
		$('#password').focus();
		return false;
	}
	if(remark=='')
	{
		$('#remark').focus();
		return false;
	}
});
// 点击添加管理员信息按钮
$("#btn-mod").click( function () {
	var username = $('#username').val();
	var password = $('#password').val();
	var remark   = $('#remark').val();
	if(username=='')
	{
		$('#username').focus();
		return false;
	}
	if(password=='')
	{
		$('#password').focus();
		return false;
	}
	if(remark=='')
	{
		$('#remark').focus();
		return false;
	}
});