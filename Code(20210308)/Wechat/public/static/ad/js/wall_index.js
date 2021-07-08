<script>
	function getMessages()
	{
		// lastId = lastId + 4;
		$.ajax({
			url : 'message.html?v=' + (new Date()/1),
			// data : {'lastId':lastId},
			success : function(data){

				var obj = jQuery.parseJSON(data)
	
				$.each(obj, function(i, item) {
        			// console.log(item.content);
			        var msgtxt = item.content;
			        // 获取随机颜色
			        var colortxt = getReandomColor();  
			        // 获取随机高度
			        var topPos = generateMixed(3);  
			        console.log( msgtxt );
			        if (topPos > 300)  
			        {  
			            topPos = 50;  
			        }  
			        var newtxt = '<p style="top:'+topPos+'px; color:'+colortxt+'">'+msgtxt+'</p>';
			        // console.log( newtxt );  
			        $(".ctxt").prepend(newtxt);  
			        var addTextW = $(".ctxt").find("p").width();  
			        $(".ctxt p").animate({left: '-'+addTextW+"px"}, 15000,function(){  
			            $(this).hide();  
			        });    			                      
        			            			                                  			                                  			                      
        		});
    		}
    	});
			

	setTimeout( "getMessages()", 2000 );
	}
	getMessages();
	//随机获取颜色值  
    function getReandomColor(){  
        return '#'+(function(h){  
        return new Array(7-h.length).join("0")+h  
        })((Math.random()*0x1000000<<0).toString(16))  
    }  
  
	//生成随机数据。n表示位数    
    var jschars = ['0','1','2','3','4','5','6','7','8','9'];    
    function generateMixed(n) {    
        var res = "";    
        for(var i = 0; i < n ; i ++) {    
            var id = Math.ceil(Math.random()*9);    
            res += jschars[id];    
        }    
        return res;    
    }   
</script>