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
			window.location.href = "/tp5/public/excel/"+msg;
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
			window.location.href = "/tp5/public/excel/"+msg;
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


// 改变以及菜单的类型
$('#menu_type_value').change(function(){
	var menu_type_value =  $("#menu_type_value").val();
	if(menu_type_value == 'MENU'){
		$('#menu_group').show();
		$('#key_group').hide();
		$('#url_group').hide();
	} else if(menu_type_value == 'URL'){
		$('#menu_group').hide();
		$('#key_group').hide();
		$('#url_group').show();
	} else if(menu_type_value == 'KEY'){
		$('#menu_group').hide();
		$('#key_group').show();
		$('#url_group').hide();
	}
});

// 点击一级菜单的提交按钮
$('#menu_add_sub').click(function(){
	var menu_name =  $("#menu_name").val();
	var menu_type_value =  $("#menu_type_value").val();
	var MENU = $("#MENU").val();
	var URL = $("#URL").val();
	var KEY = $("#KEY").val();
// 	alert(menu_name);
// 	alert(menu_type_value);
// 	alert(MENU);
// 	alert(URL);
// 	alert(KEY);
// return false;
	if(menu_name == ''){
		alert('请输入菜单名称');
		return false;
	} else if(menu_type_value == 'MENU'){
		if(MENU == ''){
			alert('请输入选择菜单位置');
			return false;
		}
	} else if(menu_type_value == 'URL'){
		if(URL == ''){
			alert('请填写需要跳转URL地址');
			return false;
		}
	} else if(menu_type_value == 'KEY'){
		if(KEY == ''){
			alert('请填写菜单事件的值');
			return false;
		}
	}
});

// 点击一级菜单详情页里面的提交按钮
$('#menu_det_sub').click(function(){
	var menu_name =  $("#menu_name").val();
	var menu_type_value =  $("#menu_type_value").val();
	var MENU = $("#MENU").val();
	var URL = $("#URL").val();
	var KEY = $("#KEY").val();
	if(menu_name == ''){
		alert('请输入菜单名称');
		return false;
	} else if(menu_type_value == 'MENU'){
		if(MENU == ''){
			alert('请输入选择菜单位置');
			return false;
		}
	} else if(menu_type_value == 'URL'){
		if(URL == ''){
			alert('请填写需要跳转URL地址');
			return false;
		}
	} else if(menu_type_value == 'KEY'){
		if(KEY == ''){
			alert('请填写菜单事件的值');
			return false;
		}
	}
});