<?php
namespace app\api\controller;

use \think\Controller;
use app\base\model\Member;
use app\base\model\Config as con;
use app\api\controller\Base as Bas;
class Base extends Controller
{
    public $config; # 配置参数

    public function initializeConfig($token)
    {
        $this->config = new Config($token);
        // if($this->request->param('token') != '')
        // {
        //     print_r($this->request->param());
        //     $this->config = new Config($this->request->param('token'));
        // } else {
        //     print_r(json_encode(array('status'=>0,'msg'=>'授权信息不能为空')));
        //     die;
        // }
    }

    public function sendSMS($phone)
    {
        $flag   = 0; 
        $params = '';//要post的数据 
        // $verify = rand( 1234, 9999 );//获取随机验证码      
        $verify = 'http://wechat.tengenglish.cn/index.php';
        // $verify = '13316182824';
        // $mobile = $_POST['mobile'];
        $mobile = $phone;
        // $mobile = '13316182824';
        //以下信息自己填以下

        // $mobile  ='18814127576';//手机号
        $argv   = array( 
            'name'   => 'tecsun',   //必填参数。用户账号
            'pwd'    => 'C9BAD3187006A98B459099432956',   //必填参数。（web平台：基本资料中的接口密码）
            // 'content'=> 'testmsg',  //必填参数。发送内容（1-500 个汉字）UTF-8编码
            'content'=> '短信验证码为：'.$verify.'，请勿将验证码提供给他人。',   //必填参数。发送内容（1-500 个汉字）UTF-8编码
            'mobile' => $mobile,    //必填参数。手机号码。多个以英文逗号隔开
            'stime'  => '',         //可选参数。发送时间，填写时已填写的时间发送，不填时为当前时间发送
            'sign'   => 'Tecsun',   //必填参数。用户签名。
            'type'   => 'pt',       //必填参数。固定值 pt
            // 'extno'   => $verify     //可选参数，扩展码，用户定义扩展码，只能为数字
        ); 
        foreach ( $argv as $key=>$value )
        { 
            if ( $flag!=0 )
            { 
                $params .= "&"; 
                $flag = 1; 
            } 
            $params .= $key."="; 
            $params .= urlencode($value);// urlencode($value); 
            $flag   = 1; 
        } 
        $url = "http://sms.1xinxi.cn/asmx/smsservice.aspx?".$params; //提交的url地址
        $con = file_get_contents($url);
        // $con = substr( file_get_contents($url), 0, 1 );  //获取信息发送后的状态
        print_r($con);
        die;
        if($con == '0')
        {
            // echo $verify;
        } else {
        }
    }

	static public function getBaseAccessToken($appid, $sectet)
    {
        // $appid = 'wx0b7cc2c472491aba';
        // $sectet = '946c4124bda07f436f82528981204401';
        $url = 'https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=%s&secret=%s';
        $url = sprintf($url,$appid,$sectet);

        $result = self::getContents($url);
        $result = json_decode($result, true); 

        if(isset($result['access_token']))
        {
            return $result['access_token'];
        }
    }

    static public function showReturnCode($code='', $data=[], $msg='')
    {
        $return_data = [
            'code'  => '500',
            'msg'   => '未定义消息',
            'data'  => $code == 200 ? $data : [],
        ];
        if (empty($code)) return $return_data;
        $return_data['code'] = $code;
        if(!empty($msg))
        {
            $return_data['msg'] = $msg;
        }else if (isset(ReturnCode::$return_code[$code])) {
            $return_data['msg'] = ReturnCode::$return_code[$code];
        }
        // return self::JSON($return_data);
        return json_encode($return_data);
    }

    static public function showReturnCodeWithOutData($code='', $msg='')
    {
        return self::showReturnCode($code,[],$msg);
    }

     #发送模板消息
    public function sendWeixinMessage($data)
    {
        // print_r($data);
        $access_token = self::getAccessToken();
        $url  = 'https://api.weixin.qq.com/cgi-bin/message/template/send?access_token='. $access_token;
        $data = json_encode($data);

        $res  = Bas::getContents($url, $data);
        $result = json_decode($res, true);
        // print_r($data);
        // print_r($result);
        if(!empty( $result )) return true;
        return false;
    }

    // 获取access_token
    private function getAccessToken()
    {

        // $appid = 'wxd75a94381f9edb0d';
        // $secret = '0ee7210f3e5fe17f30730db7215cbcdd';
        // $appid = 'wxd9e3d3efc11ace6a';
        // $secret = '68ac90a0c58a137aec04c8efd550639f';
        // $appid = $this->config->appid;
        // $secret = $this->config->secret;
        $url = 'https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=%s&secret=%s';
        $url = sprintf($url,$appid,$secret);

        $result = Bas::getContents($url);
        $result = json_decode($result, true); 

        if(isset($result['access_token']))
        {
            return $result['access_token'];
        }

        // $con = Con::where(['name'=>'ACCESS_TOKEN', 'token'=>$this->config->token])->find();
        // $con = $con ? $con->toArray() : $con;
        // return $con['value'];
    }

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
    public $timer = 2; #预约记录默认有效事件
    public $openid; #用户openid
    public $token; #公众号token
    public $appid; #公众号appid
    public $appsecret; #appsecret
    public $web_url; #公众号网站域名
    public $template_reserve; #发起预约时的模板消息
    public $template_success; #预约成功时的模板消息
    public $tmeplate_failure; #预约失败时的模板消息
    public $template_swipe;   #进出门禁记录的模板消息

    public function __construct($token = '')
    {
        // print_r($token);die;        
        $member_obj = Member::where('wechat_token', $token)->find();
        $member_arr = $member_obj ? $member_obj->toArray() : '';
        
        $this->token   = $member_arr['token'];
        $this->appid   = $member_arr['appid'];
        $this->web_url = $member_arr['web_url'];
        $this->appsecret   = $member_arr['appsecret'];
        $this->template_reserve = $member_arr['template_reserve'];
        $this->template_success = $member_arr['template_success'];
        $this->template_failure = $member_arr['template_failure'];
        $this->template_swipe = trim($member_arr['template_swipe']);

        $con = Con::where('token', $this->token)->select();
        $config = [];
        foreach ($con as $val)
        {
            $value = $val->toArray();
            $config[$value['name']] = $value;
        }

        if(isset($congif['TIMER'])) $this->timer = $congfig['TIMER']['value'];

    }
}
