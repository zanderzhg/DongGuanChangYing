$(function(){

    /*判断是否是首次预约*/
    var status = $.trim($('#status').val());
    if (status == -1) {
        // 预约信息
        $('#form-reserve').css('display','none');
        // 我的信息
        $('#form-message').css('display','block');    
    }

    /*点击按钮——我的信息*/
    $("#btn-message").click(function(){
        $('#form-reserve').css('display','none');
        $('#form-message').css('display','block');
        $('#btn-submit').css('display','none');
        $('#btn-return').css('display','block');
        $('#btn-modify').css('display','block');
    }); 

    /*点击按钮——返回预约*/
    $("#btn-return").click(function() {
        $('#form-message').css('display','none');
        $('#form-reserve').css('display','block');
    });

    /*点击按钮——修改信息*/
    $("#btn-modify").click(function() {
        var sex     = $.trim($('#sex').val()); 
        var url     = window.location.protocol+'//'+ window.location.hostname+"/index.php/wechat/visitor/modify";
        var name    = $.trim($('#name').val());
        var phone   = $.trim($('#phone').val());
        var idcard  = $.trim($('#idcard').val());
        var company = $.trim($('#company').val());
        if(!checkName(name))
        {
            // $('#name').focus();
            return false;
        }
        if(!checkPhone(phone)){
            // $('#phone').focus();
            return false;
        }

        if(idcard == ""){
            swal('身份证不能为空', {
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

        if(company == ""){
            swal('您的单位不能为空', {
                            timer: 2000, 
                            buttons: false,
                        });
            return false;
        }
        
        $.post(
            url, {
                "name":name,
                "phone":phone,
                "idcard":idcard,
                "sex":sex,
                "company":company
            }, function(data,status) {
            
            if(status =='success'){
                if(data == '1'){
                    swal('该手机号与身份证号已经存在', {
                        timer: 2000, 
                        buttons: false,
                    });
                    data = null;
                    return false;
                }else if(data == '2'){
                    swal('该手机号已经存在', {
                        timer: 2000, 
                        buttons: false,
                    });
                    data = null;
                    return false;
                }else if(data == '3'){
                    swal('该身份证号已经存在', {
                        timer: 2000, 
                        buttons: false,
                    });
                    data = null;
                    return false;
                }else if(data == '4'){
                    swal('信息无修改', {
                        timer: 2000, 
                        buttons: false,
                    });
                    data = null;
                    return false;
                }else if(data == '0'){
                    // alert('信息修改成功,马上去预约TA');
                    data = null;
                    location.href="reserve";
                }
            } else {
                swal('信息修改失败，请稍后再试', {
                    timer: 2000, 
                    buttons: false,
                });
                return false;
            }
            location.href="reserve";
        });
    });

    /*点击按钮——提交访客注册信息按钮*/
    $("#btn-submit").click(function() {
        var sex     = $.trim($('#sex').val()); 
        var url     = window.location.protocol+'//'+ window.location.hostname+"/index.php/wechat/visitor/add";
        var name    = $.trim($('#name').val());
        var phone   = $.trim($('#phone').val());
        var idcard  = $.trim($('#idcard').val());
        var company = $.trim($('#company').val());

        if(!checkName(name)) {
            return false;
        } 

        if(!checkPhone(phone)){
            return false;
        }

        if(idcard == ""){
            swal('身份证不能为空', {
                            timer: 2000, 
                            buttons: false,
                        });
            return false;
        }

        if(idcard != ""){
            if(!checkIdcard(idcard)){
                return false;
            }
        }

        if(company == ""){
            swal('您的单位不能为空', {
                            timer: 2000, 
                            buttons: false,
                        });
            return false;
        }

        $.post(
            url, {
                "name":name,
                "phone":phone,
                "idcard":idcard,
                "sex":sex,
                "company":company
            }, function(data,status) {
                // console.log(data);die;
                if(status =='success'){
                    if(data == 'flag1'){
                        swal('该手机号与身份证号已经存在', {
                            timer: 2000, 
                            buttons: false,
                        });
                        data = null;
                        return false;
                    }else if(data == 'flag2'){
                        swal('该手机号已经存在', {
                            timer: 2000, 
                            buttons: false,
                        });
                        data = null;
                        return false;
                    }else if(data == 'flag3'){
                        swal('该身份证号已经存在', {
                            timer: 2000, 
                            buttons: false,
                        });
                        data = null;
                        return false;
                    }else if(data == 'flag4'){
                        swal('信息无修改', {
                            timer: 2000, 
                            buttons: false,
                        });
                        data = null;
                        return false;
                    }else if(data == 'flag0'){
                        // alert('信息修改成功,马上去预约TA');
                        data = null;
                        location.href="reserve";
                    }
                } else {
                    swal('信息修改失败，请稍后再试', {
                        timer: 2000, 
                        buttons: false,
                    });
                    $('#ephone').focus();
                    return false;
                }

                location.href="reserve";
            }
        );
    });

    /**
     * 点击按钮--发起预约
     *
     */
    $('#btn-reserve').click(function() {  
        // alert("1111");

        var id     = $.trim($('#id').val());        
        // var phone  = $.trim($('#phone').val());
        var ephone = $.trim($('#ephone').val());
        var time   = $.trim($('#time').val());
        var number = $.trim($('#number').val());
// alert(id,phone,ephone,time,number);
        if (time == '') {
            $('#time').focus();
            return false;
        }

        if (!checkPhone(ephone)) {
            $('#ephone').focus();
            return false;
        }

        // if (ephone == phone) {
        //     swal('员工手机号不能与你的手机号相同', {
        //         timer: 2000, 
        //         buttons: false,
        //     });
        //     $('#ephone').focus();
        //     return false;
        // }

        $('#form-reserve').submit();
    });

    // 监听员工手机号码输入框的值变化
    $('#ephone').on('input',function() {
        var ephone = $.trim($('#ephone').val());
        if(ephone.length == 11){
            var url   =  window.location.protocol+'//'+ window.location.hostname+"/index.php/getEmpName";
            var phone = $('#ephone').val();
            var token = $('#token').val();
            $.post(
                url, {
                    "phone"     : phone,
                    "token"     : token,
                },function(data,status){
                if(status =='success'){
                    var obj = JSON.parse(data);
                    if(obj.status == 200){
                        $('#ename').val(obj.empName);
                    } else {
                        $('#ename').val(obj.msg);
                    }
                } else {
                    alert("信息修改失败，请稍后再试");
                }
            });
        } else {
            $('#ename').val('');
        }
    })
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
        swal('请输入来宾姓名！', {
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