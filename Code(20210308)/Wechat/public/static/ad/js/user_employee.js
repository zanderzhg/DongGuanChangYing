// 后退按钮
$("#btn-back").click( function () {
	window.history.back(-1);
});
function employeeAdd()
{
	window.location.href='User/employeeAdd';
}

// 提交按钮
$("#btn-sub").click( function () {
	var name = $("#name").val();
	var phone = $("#phone").val();
	var idcard = $("#idcard").val();

	// 暂时不对数据判断
});
// 删除多个访客
// 有待完善
$("#employee-del").click( function () {
	var res = $('input:checked');
	var data = new Array();
	if (confirm("是否删除选中员工信息，继续操作将会删除员工信息以及门禁权限，且操作不可逆。")) 
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
		$.post('employeeDel',{'id':data},function(msg){
			window.location.reload();
		});
	}
});
// 删除所有员工
$("#employee-delete-all").click( function() {
	if( confirm("是否清空所有员工信息") ){
		$.post('employeeDeleteAll',function(msg){
			// 刷新页面
			window.location.reload();
		});
	}
});
// 导出所有员工
$("#employee-exportall").click( function () {
	if (confirm("是否导出所有员工信息")) 
	{
		$.post('employeeExportAll',function(msg){
			window.location.href = "/excel/"+msg;
		});
	}
});

$("#employee-out").click( function () {
	if (confirm("是否下载导入模板")) 
	{
		$.post('employeeout',function(msg){
			window.location.href = "/excel/"+msg;
		});
	}
});

/*导出选中员工*/
$("#employee-export").click( function () {
	var res = $('input:checked');
	var data = new Array();

	/*弹出提示框*/
	if (confirm("是否导出选中员工信息"))
	{
		// alert("44");
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
		$.post('employeeExport',{'id':data},function(msg){
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

/*批量录入员工信息*/
$("#employee-import").click( function (){
	// 弹出选择框
	$('#login').modal('show');
});

// 导入员工信息
$("#employee-import-submit").click( function () {
	var excelData = $("#excelData").val();
	if( !excelData )
	{
	alert('请先选择数据');return false;
	}
});