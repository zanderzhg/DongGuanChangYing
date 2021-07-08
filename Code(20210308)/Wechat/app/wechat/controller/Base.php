<?php
namespace app\wechat\controller;

use \think\Controller;


use app\base\model\Record;
use app\base\model\Member;
use app\base\model\Conference;
use app\base\model\User;
use app\base\model\Config as Con;
use app\base\controller\Base as Bas;




class Base extends Controller
{
	public $config; # 配置参数

	public function _initialize()
	{

		$this->config = new Config($this->request->param('code'), $this->request->param('state'));
        // print_r($this->config);die;
        // if(!$this->config->openid){
        //      $this->redirect(url('we_chat/done',array('message1'=>"获取用户信息失败",'message2'=>"如有需要，请联系工作人员！")));
        // }
	}
    public function conf(){
        $this->config = new Config('', 'gh_760ac6f32334');
    }

	#获取来访事由
    public function getReasons()
    {
    	$reasons = Con::where([
                    'name'  => 'VISIT_REASONS',
                    'token' => $this->config->token
                ])->select();
        return $reasons; 
    }


    #获取当前HTTP协议
    public function getHttpProtocol()
    {
        return $result = $_SERVER['SERVER_PORT'] == 443 ? 'https://' : 'http://';
    }

    #发送模板消息
    public function sendWeixinMessage($data)
    {
        // print_r($data);
        $access_token = $this->getAccessToken();
        $url  = 'https://api.weixin.qq.com/cgi-bin/message/template/send?access_token='. $access_token;
        $data = json_encode($data);
        // print_r($data);die;

        $res  = Bas::getContents($url, $data);
        $result = json_decode($res, true);
        // print_r($data);
        // print_r($result);die;
        if(!empty( $result )) return true;
        return false;
    }

    // 获取access_token
    private function getAccessToken()
    {

        $access_token = Bas::getAccessToken();
        return $access_token;
 //        echo "调试中";
 //        die;
 //        $appid = $this->config->appid;
 //        $secret = $this->config->secret;
 //        $url = 'https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=%s&secret=%s';
 //        $url = sprintf($url,$appid,$secret);

 //        $result = self::getContents($url);
 //        $result = json_decode($result, true); 
	
	// //	print_r($result);
 //        if(isset($result['access_token'])) {
 //            return $result['access_token'];
 //        }
    }

      #curl http请求 整合get和post
    static public function getContents($url, $data='')
    {
        $header = array(
            'Content-Type:application/json;charset=utf-8',
            'Content-Length: ' .strlen($data)
        );
        $curl = curl_init();
        curl_setopt($curl,CURLOPT_URL,$url);
        curl_setopt($curl,CURLOPT_HTTPHEADER,$header);
        curl_setopt($curl,CURLOPT_SSL_VERIFYPEER,FALSE);
        curl_setopt($curl,CURLOPT_SSL_VERIFYHOST,FALSE);
        if(!empty($data))
        {
            curl_setopt($curl,1,1);
            curl_setopt($curl,CURLOPT_POSTFIELDS,$data);
        }
        curl_setopt($curl,CURLOPT_RETURNTRANSFER,1);
        $output = curl_exec($curl);
        curl_close($curl);
        return $output;
    }
	
}

// 配置类
class Config
{
	public $timer = 12; #预约记录默认有效事件
	public $openid; #用户openid
    public $headimgurl; #用户headimgurl用户头像url
	public $token; #公众号token
    public $appid; #公众号appid
    public $secret; #公众号secret
	public $web_url; #公众号网站域名
	public $template_reserve; #发起预约时的模板消息
	public $template_success; #预约成功时的模板消息
	public $tmeplate_failure; #预约失败时的模板消息

	public function __construct($code = '', $state='')
	{
        $state = 'gh_2a814d73e7d4';
		$state = $state ? $state : session('state');
		session('state', $state);
		
		$member_obj = Member::where('wechat_token', $state)->find();
		$member_arr = $member_obj ? $member_obj->toArray() : '';

        if(!$member_arr){#没有找到数据
            return;
        }

		$this->token   = $member_arr['token'];
        $this->appid   = $member_arr['appid'];
        $this->secret  = $member_arr['appsecret'];
		$this->web_url = $member_arr['web_url'];
		$this->template_reserve = $member_arr['template_reserve'];
		$this->template_success = $member_arr['template_success'];
		$this->template_failure = $member_arr['template_failure'];
        $this->address = $member_arr['address'];

		$con = Con::where('token', $this->token)->select();
		$config = [];
		foreach ($con as $val)
		{
			$value = $val->toArray();
			$config[$value['name']] = $value;
		}

		if(isset($congif['TIMER'])) $this->timer = $congfig['TIMER']['value'];

		if($code){
			$this->openid = $this->getOpenid($member_arr, $code);
		} else {
			$this->openid = session('openid');
		}

        $this->headimgurl = $this->getHeadimgurl($member_arr);
	}

	private function getOpenid($member_arr, $code)
	{

        $access_token = Bas::getAccessToken();
        // print_r($access_token);
        // die;
	    $url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={$member_arr['appid']}&secret={$member_arr['appsecret']}&code={$code}&grant_type=authorization_code";
        $code_json   = Bas::getContents($url);
        
        // 解析json数据包
        $returncode = json_decode($code_json, true);
        if(isset($returncode['openid']))
        {
            // 从数组中获取openid，并且避免刷新出现空数组的错误
            $openid = trim($returncode['openid']);
            session('openid', trim($returncode['openid']));
        } else {
            $openid = session('openid');
        }
        return $openid;
	}

    private function getHeadimgurl($member_arr){
        # 获取access_token
       	// return '';
	 // $appid = $this->appid;
        // $secret = $this->secret;
        // $url = 'https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=%s&secret=%s';
        // $url = sprintf($url,$appid,$secret);
        // $result = Bas::getContents($url);
        // $result = json_decode($result, true);
        $access_token = Bas::getAccessToken($member_arr);

        # 获取用户头像
        $url="https://api.weixin.qq.com/cgi-bin/user/info?access_token={$access_token}&openid={$this->openid}&lang=zh_CN";
        $code_json   = Bas::getContents($url);
        // 解析json数据包
        $returncode = json_decode($code_json, true);
        if(isset($returncode['headimgurl']))
        {
            // 从数组中获取access_token，并且避免刷新出现空数组的错误
            $headimgurl = trim($returncode['headimgurl']);
            session('headimgurl', trim($returncode['headimgurl']));
        } else {
            $headimgurl = session('headimgurl');
        }
        
        return $headimgurl;
    }
}
