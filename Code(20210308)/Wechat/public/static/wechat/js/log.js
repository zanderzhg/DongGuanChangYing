$(function(){
    $('#save').click(function(){ 
    // alert("ddd");return false;    
        var id      = $.trim($('#id').val());        
		var name    = $.trim($('#name').val());
        var phone   = $.trim($('#phone').val());
        var url     = "edit.html";
        // if(!checkname(name)){
        //     $('#name').focus();
        //     return false;
        // }
        // if(!checkphone(phone)){
        //     $('#phone').focus();
        //     return false;
        // }
    
        $.post(url,{"id":id,"name":name,"phone":phone},function(data,status){
            if(status == 'success'){
                if(data == 'flag2'){
                    alert('该手机号已经存在');
                    $('#yourphone').focus();
                    data = null;
                    return false;
                }else if(data == 'flag4'){
                    alert('信息无修改');
                    data = null;
                }else if(data == 'flag0'){
                    alert('信息修改成功');
                    data = null;
                }else {
                    alert('信息无修改');
                    data = null;
                }
            } else {
                alert("信息修改失败，请稍后再试");
            }
        });
    });
    $("#go").click(function(){
        var u = $.trim($('#u').val());
        if(!u)
        {
            $('#u').focus();
            return false;
        }
        
        $('#form-reserve').submit();
    })
});