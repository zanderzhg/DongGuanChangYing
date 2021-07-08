$(function(){
  $('#invite').click(function(){  
    // alert("!");
    var id    = $.trim($('#id').val());        
    var name  = $.trim($('#name').val());
    var phone = $.trim($('#phone').val());
    var time  = $.trim($('#time').val());
    var idcard= $.trim($('#idcard').val()); 
    var reasons= $.trim($('#reasons').val());
    var end_time = $.trim($('#end_time').val());
    // if(end_time == '')
    // {
    //   $('#end_time').focus();
    //   return false;
    // }
    if(!checkName(name)){
      $('#name').focus();
      return false;
    }

    if(!checkPhone(phone)){
      $('#phone').focus();
      return false;
    }

    if(idcard == ''){
      swal('身份证号不能为空！', {
            timer: 2000, 
            buttons: false,
        });
      return false;
    }

    if(idcard != ""){
            if(!checkIdcard(idcard)){
                // $("#idcard").focus();
                return false;
            }
        }

    if(time == '')
    {
      $('#time').focus();
      return false;
    }


    $('#form-invite').submit();
      
  });
});


// 验证手机号码的格式
function checkPhone(phone){
    var reg = /^[1][345789][0-9]{9}$/;
    
    if(phone==""){
        swal('手机号码不能为空！', {
            timer: 2000, 
            buttons: false,
        });
        return false;

    }else if(!reg.test(phone)){
        swal('手机号码格式有误！', {
            timer: 2000, 
            buttons: false,
        });
        return false;

    } else {
        return true;
    }
}

// 检查访客姓名位数是否符合要求 1-9位
function checkName(name){
    var i = name.length;
    
    if(i == 0) {
        swal('请输入姓名！', {
            timer: 2000, 
            buttons: false,
        });
        return false;
    } else if(i > 10) {
        swal('名称不能超过10个字！', {
            timer: 2000, 
            buttons: false,
        });
        return false;
    } else {
        return true;
    }
}

// 验证身份证号码的格式
function checkIdcard(id) {
     // 1 "验证通过!", 0 //校验不通过
        var format = /^(([1][1-5])|([2][1-3])|([3][1-7])|([4][1-6])|([5][0-4])|([6][1-5])|([7][1])|([8][1-2]))\d{4}(([1][9]\d{2})|([2]\d{3}))(([0][1-9])|([1][0-2]))(([0][1-9])|([1-2][0-9])|([3][0-1]))\d{3}[0-9xX]$/;
        //号码规则校验
        if(!format.test(id)){
            swal('身份证号码格式有误！', {
                timer: 2000, 
                buttons: false,
            });
            return false;
        }
        //区位码校验
        //出生年月日校验   前正则限制起始年份为1900;
        var year = id.substr(6,4),//身份证年
            month = id.substr(10,2),//身份证月
            date = id.substr(12,2),//身份证日
            time = Date.parse(month+'-'+date+'-'+year),//身份证日期时间戳date
            now_time = Date.parse(new Date()),//当前时间戳
            dates = (new Date(year,month,0)).getDate();//身份证当月天数
        if(time>now_time||date>dates){
            swal('身份证号码格式有误！', {
                timer: 2000, 
                buttons: false,
            });
            return false;
        }
        //校验码判断
        var c = new Array(7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2);   //系数
        var b = new Array('1','0','X','9','8','7','6','5','4','3','2');  //校验码对照表
        var id_array = id.split("");
        var sum = 0;
        for(var k=0;k<17;k++){
            sum+=parseInt(id_array[k])*parseInt(c[k]);
        }
        if(id_array[17].toUpperCase() != b[sum%11].toUpperCase()){
            swal('身份证号码格式有误！', {
                timer: 2000, 
                buttons: false,
            });
            return false;
        }
        return true;
}