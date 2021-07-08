<?php
namespace app\wechat\controller;

use think\Controller;

class WeChat extends Controller
{
	/**
	 * 微信服务器发送转发数据到服务器的入口
	 * 
	 * 
	 * 
	 */
	public function index()
	{
		$WeChatApi = new WeChatApi;
	}

	/**
	 * 显示手机端的提示信息
	 * 
	 * 
	 * 
	 */
	public function done()
	{
		$message1 = $this->request->param('message1');
		$message2 = $this->request->param('message2');
		$message1 = $message1 ? $message1 : '';
		$message2 = $message2 ? $message2 : '';
		
        $this->assign('message1', $message1);
        $this->assign('message2', $message2);
        return $this->fetch();
	}
}