<?php
namespace app\wechat\controller;

use app\base\controller\Base as Bas;
use app\base\model\Member;
use app\base\model\ContentReply;

class WeChatApi
{
	public function __construct()
	{
		if(!isset($_GET['echostr'])){
			return $this->responseMsg();
		} else {
			return $this->valid();
		}
	}

	/**
	 * 验证请求来源是否属于微信服务后台
	 * 
	 * @param nonce
	 * @param echostr
	 * @param signature
	 * @param timestamp
	 * 
	 */
	public function valid()
	{
		$nonce 		 = $_GET['nonce'];
		$echostr     = $_GET['echostr'];
		$signature   = $_GET['signature'];
		$timestamp   = $_GET['timestamp'];
		$server_name = $_SERVER['SERVER_NAME'];
		$web_url     = $server_name;
		
		$member_obj  = Member::where([
				'web_url' => [ 'like', '%'.$server_name]
			])->select();
		
		if($member_obj){
			foreach ($member_obj as $v) {
				$member_arr  = $v->toArray();

				$token   = trim($member_arr['token']);
				$tmp_arr = array($token, $timestamp, $nonce);
				sort($tmp_arr);
				$tmp_str = implode($tmp_arr);
				$tmp_str = sha1($tmp_str);
			
				if($tmp_str == $signature){
					echo $echostr;
					exit;
				} else {
					// return false;
				}
			}

		} else {
			return false;
		}
		
	}

	public function responseMsg()
	{
		header("Content-type:text/html;charset=utf-8");

		$post_str = isset($GLOBALS['HTTP_RAW_POST_DATA']) ? $GLOBALS['HTTP_RAW_POST_DATA'] : '';
		
		if(empty($post_str)){
			echo "请把此地址填写到微信公众平台后台URL地址";
			exit;
		} else {
			$post_obj = simplexml_load_string($post_str, 'SimpleXMLElement', LIBXML_NOCDATA);
			$rx_type  = trim($post_obj->MsgType);
			$token    = trim($post_obj->ToUserName);
			// $token = 'gh_1197d26bdb2a';
			$member_obj = Member::where('wechat_token', $token)->find();
			$member_arr = $member_obj ? $member_obj->toArray() : '';
			switch ($rx_type)
			{
				case 'event':
					$result = $this->receiveEvent($post_obj, $member_arr);
					break;
				case 'text':
					$result = $this->receiveText($post_obj ,$member_arr);
					break;
				case 'image':
					$result = $this->receiveImage($post_obj);
					break;
				case 'location':
					$result = $this->receiveLocation($post_obj);
					break;
				case 'video':
					$result = $this->receiveVideo($post_obj);
					break;
				case 'link':
					$result = $this->receiveLink($post_obj);
					break;
				default:
					$result = "暂时不支持该事件类型：" . $rx_type;
					break;
			}
			echo $result;
		}
	}

	// 接收事件信息
	/**
	 * 接收事件信息
	 * @param string post_obj->Event subscribe 		关注
	 * @param string post_obj->Event unsubscribe 	取消关注
	 * @param string post_obj->Event SCAN 			扫描
	 * @param string post_obj->Event CLICK 			点击 CLICK 类型菜单 
	 * @param string post_obj->Event VIEW 			点击 VIEW 类型菜单 
	 * 
	 */
	private function receiveEvent($post_obj, $member_arr)
	{
		// $content = '11';
		// $content = $member_arr['id'];
		// return $this->transmitText($post_obj, $content);
		switch ($post_obj->Event) {
			case 'subscribe':
				$content = "欢迎关注我们公众号";
				break;
			case 'unsubscribe':
				$content = "取消关注";
				break;
			case 'SCAN':
				$content = "扫描对象：" . $post_obj->FromUserName;
				break;
			case 'CLICK':
			// $content = '111';
				$content = $this->receiveEvenceClick($post_obj, $member_arr);
				break;
			case 'VIEW':
				$content = "跳转链接：" . $post_obj->EventKey;
				break;
			// case 'MASSSENDJOBFINISH':
			// 	$content = "消息ID：".$object->MsgID."，结果：".$object->Status."，粉丝数：".$object->TotalCount."，过滤：".$object->FilterCount."，发送成功：".$object->SentCount."，发送失败：".$object->ErrorCount;
			// 	break;
			default:
				// $content = "未定义事件类型: " . $post_obj->Event;
				break;
		}
		if(is_array($content)) {
			if(isset($content[0])) {
				return $this->transmitNews($post_obj, $content);
			} 
		} else {
			return $this->transmitText($post_obj, $content);
		}
	}

	/**
	 * 处理单击 CLICK 菜单事件
	 * 
	 * 
	 * 
	 */
	private function receiveEvenceClick($post_obj, $member_arr)
	{
		$token = trim($post_obj->ToUserName);
		$appid   = $member_arr['appid'];
		// 	$web_url = trim($member_arr['web_url']);
		// $content = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/visitor/reserve&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect";
		// return $appid;
		if(!$member_arr)
		{
			$content = '请联系管理员进行微信预约系统注册';
		} else {
			$appid   = $member_arr['appid'];
			$web_url = trim($member_arr['web_url']);
			switch ($post_obj->EventKey) {
				case 'FANGKE':
				case 'WOSHIFANGKE':
					// $content = "<a href='http://mp.weixin.qq.com/s/Tn9YR0tQz7rZv-CHwZsqEA'>访客操作指引</a>\n\n";
					// $content .= "<a href='"."https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/visitor/records&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect"."'>预约凭证</a>\n\n";
					// $content .= "<a href='"."https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/visitor/reserve&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect"."'>我要预约</a>";
					
					$content   = array();
					$content[] = array(
						"Description" => "微信预约操作指引",
						"Title"  => "我是访客·操作指引",
						"PicUrl" => $web_url."/static/wechat/images/thumbnail/main2.jpg",
						"Url"    => "https://mp.weixin.qq.com/s/G0UusIZ-Mwi7rJmmbzyV9w",
						);
					$content[] = array(
						"Description" => "预约凭证",
						"Title"  => "预约凭证",
						"PicUrl" => $web_url."/static/wechat/images/thumbnail/yuyuepingzheng1.jpg",
						"Url"    => "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/visitor/records&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect",
						);
					$content[] = array(
						"Description" => "我要预约",
						"Title"  => "我要预约",
						"PicUrl" => $web_url."/static/wechat/images/thumbnail/woyaoyuyue1.jpg",
						"Url"    => "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/visitor/reserve&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect",
						);
					
					return $content;
					break;
				case 'YUANGONG':
				case 'WOSHIYUANGONG':
					// $content = "<a href='http://mp.weixin.qq.com/s/5P5gXYXQairW3HuPu-_fFg'>员工操作指引</a>\n\n";
					// $content .= "<a href='"."https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/employee/index&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect"."'>员工绑定</a>\n\n";
					// $content .= "<a href='"."https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/employee/records&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect"."'>预约凭证</a>\n\n";
					// $content .= "<a href='"."https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/employee/invite&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect"."'>邀请访客</a>";

							
					$content   = array();
					$content[] = array(
						"Description" => "微信预约操作指引",
						"Title"  => "我是员工·操作指引",
						"PicUrl" => $web_url."/static/wechat/images/thumbnail/main2.jpg",
						"Url"    => "https://mp.weixin.qq.com/s/fHwTdT45R2XBPP4PBaYXcQ",
						);
					$content[] = array(
						"Description" => "员工绑定",
						"Title"  => "员工绑定",
						"PicUrl" => $web_url."/static/wechat/images/thumbnail/yuangongbangding1.jpg",
						"Url"    => "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/employee/index&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect",
						);
					$content[] = array(	
						"Description" => "预约凭证",
						"Title"  => "预约凭证",
						"PicUrl" => $web_url."/static/wechat/images/thumbnail/yuyuepingzheng1.jpg",
						"Url"    => "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/employee/records&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect",
						);
					$content[] = array(
						"Description" => "邀请访客",
						"Title"  => "邀请访客",
						"PicUrl" => $web_url."/static/wechat/images/thumbnail/yaoqingfangke1.jpg",
						"Url"    => "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/employee/invite&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect",
						);
					$content[] = array(
						"Description" => "预约信息",
						"Title"  => "预约信息",
						"PicUrl" => $web_url."/static/wechat/images/thumbnail/yuyuexinxi1.jpg",
						"Url"    => "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" . $appid . "&redirect_uri=" . $web_url . "/index.php/wechat/employee/bookRecord&response_type=code&scope=snsapi_base&state=" . $token . "#wechat_redirect",
						);
					return $content;
					break;
				default:
					$content = "点击菜单: " . $post_obj->EventKey;
					return $content;
					break;
			}
		}
	}


	// 接收文本消息
	private function receiveText($post_obj, $member_arr)
	{
		$content = trim($post_obj->Content);
		$content_return = '';
		// $content_return = '很抱歉，没有明白您的意思。';
		// return $this->transmitText($post_obj, $member_arr['appid']);
		$token   = $member_arr['token'];
		$appid   = $member_arr['appid'];
		$web_url = $member_arr['web_url'];

		$content_reply = ContentReply::where(['content'=>$content, 'token'=>$token])->find();
		$content_reply = $content_reply ? $content_reply->toArray() : $content_reply;
		// return $this->transmitText($post_obj, $content_reply['reply']);

		if(!$content_reply)
		{			
			// $content_reply = ContentReply::where(['content'=>['like'=>%.$content.%], 'token'=>$token])->find();
			// $content_reply = $content_reply ? $content_reply->toArray() : $content_reply;
			// if(!$content_reply)
			// {
				return $this->transmitText($post_obj, $content_return);
			// }
		}
		switch ($content_reply['type']) {
			case 'TEXT':
				return $this->transmitText($post_obj, $content_reply['reply']);
				// return $this->transmitText($post_obj, $content_reply['content_reply']);
				break;
			case 'IMAGE':
				return $this->transmitImage($post_obj, $content_reply['reply']);
				// return $this->transmitImage($post_obj, $content_reply['mediaid_reply']);
				break;
			default:
				return $this->transmitText($post_obj, $content_return);
				break;
		}
		
	}

	// 接收图片消息
	private function receiveImage($object)
	{
		// 获取图片ID
		$imageArray['MediaId'] = trim($object->MediaId);
		// 回复图片消息
		return $this->transmitImage($object, $imageArray);
	}

	// 回复文本消息
	private function transmitText($object, $content)
	{
		$itemTpl = "<MsgType><![CDATA[text]]></MsgType>
					<Content><![CDATA[%s]]></Content>";
		$item_str = sprintf( $itemTpl, $content );
		return $this->transmitBase($object, $item_str);
	}

	// 回复图片消息
	private function transmitImage($object, $imageArray)
	{
		$itemTpl = "<MsgType><![CDATA[image]]></MsgType>
					<Image>
					<MediaId><![CDATA[%s]]></MediaId>
					</Image>";
		$item_str = sprintf($itemTpl, $imageArray['MediaId']);
		
		return $this->transmitBase($object, $item_str);
	}

	// 回复语音消息
	private function transmitVoice($object, $voiceArray)
	{
		$itemTpl = "<MsgType><![CDATA[voice]]></MsgType>
					<Voice>
					<MediaId><![CDATA[%s]]></MediaId>
					</Voice>";
		$item_str = sprintf($itemTpl, $voiceArray['MediaId']);

		return $this->transmitBase($object, $item_str);
	}

	// 回复视频消息
	private function transmitVideo($object, $videoArray)
	{
		$itemTpl = "<MsgType><![CDATA[video]]></MsgType>
					<Video>
						<MediaId><![CDATA[%s]]></MediaId>
						<ThumbMediaId><![CDATA[%s]]></ThumbMediaId>
						<Title><![CDATA[%s]]></Title>
						<Description><![CDATA[%s]]></Description>
					</Video>";
		$item_str = sprintf($itemTpl, $videoArray['MediaId'], $videoArray['ThumbMediaId'], $videoArray['Title'], $videoArray['Description']);

		return $this->transmitBase( $object, $item_str);
	}

	// 回复图文消息
	private function transmitNews($object, $newsArray)
	{
		if(!is_array($newsArray))
		{
			return;
		}
		$itemTpl = "<item>
					<Title><![CDATA[%s]]></Title>
					<Description><![CDATA[%s]]></Description>
					<PicUrl><![CDATA[%s]]></PicUrl>
					<Url><![CDATA[%s]]></Url>
				</item>";
		$item_str = "";
		foreach ($newsArray as $item)
		{
			$item_str .= sprintf($itemTpl, $item['Title'], $item['Description'], $item['PicUrl'], $item['Url']);
		}
		$xmlTpl = "<xml>
					<ToUserName><![CDATA[%s]]></ToUserName>
					<FromUserName><![CDATA[%s]]></FromUserName>
					<CreateTime>%s</CreateTime>
					<MsgType><![CDATA[news]]></MsgType>
					<ArticleCount>%s</ArticleCount>
					<Articles>
					$item_str</Articles>
					</xml>";
		return sprintf($xmlTpl, $object->FromUserName, $object->ToUserName, time(), count($newsArray));
	}

	// 回复多客服消息
	private function transmitService($object)
	{
		$xmlTpl = "<xml>
				<ToUserName><![CDATA[%s]]></ToUserName>
				<FromUserName><![CDATA[%s]]></FromUserName>
				<CreateTime>%s</CreateTime>
				<MsgType><![CDATA[transfer_customer_service]]></MsgType>
				</xml>";
		$result = sprintf($xmlTpl, $object->FromUserName, $object->ToUserName, time());
		return $result;
	}

	// 回复消息共同方法
	private function transmitBase($object, $item_str)
	{
		$xmlTpl = "<xml>
					<ToUserName><![CDATA[%s]]></ToUserName>
					<FromUserName><![CDATA[%s]]></FromUserName>
					<CreateTime>%s</CreateTime>
					$item_str
					</xml>";
		$result = sprintf($xmlTpl, $object->FromUserName, $object->ToUserName, time());
		return $result;
	}
}