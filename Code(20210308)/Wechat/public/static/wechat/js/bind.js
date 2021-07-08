var return_code = '0';
function codeButton() {
    var code = $("#code");
    code.attr("disabled", "disabled");
    sendSMS();
    setTimeout(function() {
        code.css("opacity", "0.8");
    }, 1000)
    var time = 60;
    var set = setInterval(function() {
        code.val(--time + "s");
    }, 1000);
    setTimeout(function() {
        code.attr("disabled", false).val("重新发送");
        clearInterval(set);
    }, 60000);
}
function sendSMS() {
    var mobile = document.getElementById("phone").value;
    var url    = "send_SMS.html";
    
    if (mobile == '') {
        window.alert(" 请输入电话号码！");
        return;
    }
    $.post(url,{"mobile":mobile},function(data,status){
        return_code = data;
        console.log(data);
    });
    // return_code = '1111';
}
// $(function(){
//  var ma="";
//  var that=this;
//  console.log('1111');
//    $.ajax({
//      type:'post', 
//      "contentType": "application/json;charset=utf-8",
//    url:'https://wxapi.liuyunchuan.com/sendSMS',
//    dataType: "json",
//    success:function(data){
//      console.log('接口');
//      that.ma=data
//    }
//    })

// })

$(function() {
    var namejt = false;
    var phonejt = false;
    var majt = false;
    var that=this;
    // $('#name').bind('input propertychange', function() {
    //     var namelen = ($(this).val().length);
    //     if(namelen > 0) {
    //         that.namejt = true
    //         // console.log('名字正确')
    //         if(that.majt && that.phonejt && that.namejt) {
    //                 document.getElementById("bbb").style.display = "inline";
    //                 document.getElementById("ccc").style.display = "none";
    //             } else {
    //                 document.getElementById("bbb").style.display = "none";
    //                 document.getElementById("ccc").style.display = "inline";
    //             }
    //     } else {
    //         that.namejt = false
    //     }
    // })
    $('#phone').bind('input propertychange', function() {
        var phone = ($(this).val());
        if(phone.length == 11) {
            /*
             * 正则判断
             */
            var str = /^1\d{10}$/;
            if(str.test(phone)) {
                that.phonejt = true
                // console.log('手机号码正确')
                if( that.phonejt){
                    document.getElementById("code").style.display = "inline";
                    document.getElementById("aaa").style.display = "none";
                }else{
                    document.getElementById("code").style.display = "none";
                    document.getElementById("aaa").style.display = "inline";
                }
                if(that.majt && that.phonejt && that.namejt) {
                    document.getElementById("bbb").style.display = "inline";
                    document.getElementById("ccc").style.display = "none";
                } else {
                    document.getElementById("bbb").style.display = "none";
                    document.getElementById("ccc").style.display = "inline";
                }

            } else {
                that.phonejt = false

                alert('手机号不正确')

            }
        } else {
            document.getElementById("code").style.display = "none";
                    document.getElementById("aaa").style.display = "inline";
            return false
        }
    })
    $('#ma').bind('input propertychange', function() {
        var ma = ($(this).val());
        //      console.log(namelen);
        if(ma.length == 4) {
            /*
             * 正则判断
             */
            if(ma == return_code) {
                that.majt = true;
                // console.log('验证码正确')
                if(that.majt && that.phonejt && that.namejt) {
                    document.getElementById("bbb").style.display = "inline";
                    document.getElementById("ccc").style.display = "none";
                } else {
                    document.getElementById("bbb").style.display = "none";
                    document.getElementById("ccc").style.display = "inline";
                }
                document.getElementById('bind').submit();
            } else {
                that.majt = false
                alert('验证码不正确')
            }
        } else {
            var majt = false
        }
    })
    $('')
})

// $(function() {
//  $('#submit').click(function() {
//      var d = {};
//      var t = $('form').serializeArray();
//      $.each(t, function() {
//          d[this.name] = this.value;
//      });
//      alert(JSON.stringify(d));
//  });
// });