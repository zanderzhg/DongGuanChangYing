window.onload=function(){
var container1=document.getElementById("container");
var headHei=document.getElementById("head").offsetHeight
container1.style.marginTop=headHei+32+"px";
var winHei=document.documentElement.clientHeight;
var winHei=document.documentElement.clientHeight-32;
var a=0;//记录页面滑动的次数
var timer4=null; 
var i=0;//通知设置0,1，确保事件执行顺序

// 页面切换时间
var time = 20000;

//显示时间
setTimeout(showTime(),0)
setInterval(showTime,1000)
function showTime(){
var mydate=new Date();
var myyear=mydate.getFullYear();//年
var mymonth=mydate.getMonth()+1
mymonth=mymonth<10?"0"+mymonth:mymonth;//月
var myday=mydate.getDate();//日
myday=myday<10?"0"+myday:myday;
document.getElementById("year").innerHTML=myyear+"年"+mymonth+"月"+myday+"日";

var weekday=["星期日","星期一","星期二","星期三","星期四","星期五","星期六"];
var myweek=mydate.getDay();
document.getElementById("day").innerHTML=weekday[myweek];

var myhour=mydate.getHours();
myhour=myhour<10?"0"+myhour:myhour;//小时
var myminute=mydate.getMinutes();
myminute=myminute<10?"0"+myminute:myminute;//分钟
var mysecond=mydate.getSeconds();
mysecond=mysecond<10?"0"+mysecond:mysecond;//秒
if(myminute==00||myminute==30)
{
    location.reload();
}

document.getElementById("hour").innerHTML=myhour+":"+myminute+":"+mysecond;
}

//每天四条内容
var aResult=(winHei-headHei-16*6)/6;
var tdElement=document.getElementsByTagName("td");

for(var tdNum=0;tdNum<tdElement.length;tdNum++){
	tdElement[tdNum].style.height=aResult+"px";
}

//设置页面切换时间
var timer1=setInterval(slide,time);	

function slide(){
	// var step=0;
	// var timer2=setInterval(slide1,1);/*设置页面切换的快慢*/
	// function slide1(){//通过"每1毫秒，内容上移10px"来控制页面切换快慢
	// container1.style.marginTop=parseInt(container1.style.marginTop)-10+"px";
	// step=step+10;
	// if(step>winHei-headHei){//当移动距离到达(可视区域距离)时，停止移动.
	// 	clearInterval(timer2);
	// 	}
 //    }
	// a++;//页面滑动一次，a+1
	// var count=Math.ceil(parseInt(container1.offsetHeight)/(parseInt(winHei)-10));
 //    if(a==count)//当内容全部展示完毕后，恢复原状，开始新一轮循环
 //    {
 //    	// container1.style.marginTop=parseInt(winHei)+5+"px";
 //    	// a=0;
 //    	location.reload();
 //    }
}


window.document.onkeydown=function(){//键盘事件
	e=window.event||e;
    switch(e.keyCode){
    case 38: //向上键
      turnUp();
      break;
    case 40: //向下键
      turnDown();
      break;
    default:
      auto();//暂停，无操作后自动滚动
      break;
  }
   	  clearInterval(timer1);
}

window.onmousewheel=function(){//鼠标事件
	e=window.event||e;
	switch(e.wheelDelta){
    case 120: //向上键
      turnUp();
      break;
    case -120: //向下键
      turnDown();
      break;
  }
   	  clearInterval(timer1);
}

window.onmousemove=function(){//鼠标移动时页面停止滚动，无操作5s后自动滚动
	clearInterval(timer1);
	clearInterval(timer4);
	e=window.event||e;
	var mouseX=e.clientX;
	var mouseY=e.clientY;
    setTimeout(isAuto,time);
    function isAuto(){
    	var mouseX1=e.clientX;
    	var mouseY1=e.clientY;
    	if(mouseX==mouseX1&&mouseY==mouseY1){
    		auto();
    	}
    }
}


function turnUp(){//页面向上滚动
	while(i==0){
	i=1;
	if(a==0){
		i=0;//最前面一页。不能再往上翻页了
		return ;
	}
	clearInterval(timer1);//清除原来的定时器
	step1=0;
	var timer3=setInterval(slide2,1);/*设置页面切换的快慢*/
	function slide2(){
	container1.style.marginTop=parseInt(container1.style.marginTop)+10+"px";
	step1=step1+10;
	if(step1>winHei-headHei){
		clearInterval(timer3);
		i=0;
		auto()//确保页面在完成向前翻动后，开市计时，无操作后自动滚动
		}
    }
	a--;
	}
}

function turnDown(){//页面向下滚动
	while(i==0){
		i=1;
	var count=Math.ceil(parseInt(container1.offsetHeight)/(parseInt(winHei)-10));
	if(a==count-1){//最后一页。不能再往下翻页了
	  	i=0;
	  	return ;
	 }
	step1=0;
	var timer3=setInterval(slide2,1);/*设置页面切换的快慢*/
	function slide2(){
	container1.style.marginTop=parseInt(container1.style.marginTop)-10+"px";
	step1=step1+10;
	if(step1>winHei-headHei){
		clearInterval(timer3);
		i=0;
		auto();
		}
    }
	a++;
    }
}
//暂停一段时间后自动轮播
function auto(){
	// clearInterval(timer4);//清除已经存在的定时器
	// timer4=null;
	// setTimeout(timer,2000);
	// setTimeout(timer,time);
	}
function timer(){
	// if(timer4==null){//因为延时的原因，先判断是否已经存在定时，如果没有，则开启定时
	// timer4=setInterval(slide,2000);
	// }
	}
//屏幕进行点击，上滑动，下滑动
var startx, starty;
//获得角度
function getAngle(angx, angy) {
    return Math.atan2(angy, angx) * 180 / Math.PI;
};
 
//根据起点终点返回方向 1向上 2向下 3向左 4向右 0未滑动
function getDirection(startx, starty, endx, endy) {
    var angx = endx - startx;
    var angy = endy - starty;
    var result = 0;
    //如果滑动距离太短
    if (Math.abs(angx) < 2 && Math.abs(angy) < 2) {
        return result;
    }
    var angle = getAngle(angx, angy);
    if (angle >= -135 && angle <= -45) {
        result = 1;
    } else if (angle > 45 && angle < 135) {
        result = 2;
    } else if ((angle >= 135 && angle <= 180) || (angle >= -180 && angle < -135)) {
        result = 3;
    } else if (angle >= -45 && angle <= 45) {
        result = 4;
    }
        return result;
    }
//手指接触屏幕
document.addEventListener("touchstart", function(e) {
    startx = e.touches[0].pageX;
    starty = e.touches[0].pageY;
}, false);
//手指离开屏幕
document.addEventListener("touchend", function(e) {
    var endx, endy;
    endx = e.changedTouches[0].pageX;
    endy = e.changedTouches[0].pageY;
    var direction = getDirection(startx, starty, endx, endy);
    switch (direction) {
        case 0:
            clearInterval(timer1);
			clearInterval(timer4);
    		auto();
            break;
        case 1://向上
            turnDown();
      		break;
        case 2://向下
            turnUp();
      		break;
        /*case 3:
            console.log('3');
            break;
        case 4:
            console.log('4');
            break;
        default:*/
    }
}, false);

}


