<?php
namespace app\base\controller;

use \think\Controller;
use app\base\model\Config as Con;
use app\base\model\Member;

class Base extends Controller
{

    /**
     * curl 请求 http 接口
     */
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


    /**
     * 获取当前HTTP协议
     */
    static public function getHttpProtocol()
    {
        return $result = $_SERVER['SERVER_PORT'] == 443 ? 'https://' : 'http://';
    }

    /**
     * 加密函数
     * @param str 需要加密的字符串
     * @param key 加密秘钥
     */
    static private function des_encrypt($str, $key)
    {
      $block = mcrypt_get_block_size('des', 'ecb');
      $pad   = $block - (strlen($str) % $block);
      $str  .= str_repeat(chr($pad), $pad);
      return mcrypt_encrypt(MCRYPT_DES, $key, $str, MCRYPT_MODE_ECB);
    }

    /**
     * 解密函数
     * @param str 需要解密的字符串
     * @param key 解密秘钥
     */
    static private function des_decrypt($str, $key)
    {
        $str   = mcrypt_decrypt(MCRYPT_DES, $key, $str, MCRYPT_MODE_ECB);
        $len   = strlen($str);
        $block = mcrypt_get_block_size('des', 'ecb');
        $pad   = ord($str[$len - 1]);
        return substr($str, 0, $len - $pad);
    }

    /**
     * 生成门禁授权序列号
     * @param id 自增记录的id
     */
    static public function getCode($id)
    {
        $code = '3';
        $rand1 = rand( 10, 99 );
        $rand2 = rand( 10, 99 );
        $code .= $rand1;
        if($id > 99999)
        {
            $id%=100000;
        }
        for( $i=strlen($id); $i<5; $i++ )
        {
            $id = '0'.$id;
        }
        $code .= $id;
        $code .= $rand2;
        return $code;
    }

    static private function getAT($appid,$secret)
    {
        $url = 'https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=%s&secret=%s';
        $url = sprintf($url,$appid,$secret);

        $result = self::getContents($url);
        // print_r($result);
        // die;
        $result = json_decode($result, true); 

        if(isset($result['access_token'])) {
            return $result['access_token'];
        }
    }

    static public function getAccessToken()
    {
        // $member = $member ? $member->toArray() : $member;
        $config = Con::where([
            'token' => 'gh_2a814d73e7d4',
            'name' =>'ACCESS_TOKEN'
        ])->find();

        $member_obj = Member::where('wechat_token', 'gh_2a814d73e7d4')->find();
        $member = $member_obj ? $member_obj->toArray() : '';

        // print_r($config);

        if ($config) {
            $config = $config->toArray();
             // print_r($config);die;
            if (time() - $config['create_time'] > 7000) {
                $accessToken = self::getAT($member['appid'], $member['appsecret']);
                $res = Con::where([
                        'name' => 'ACCESS_TOKEN',
                        'token'=> $member['token']
                    ])->update([
                        'value' => $accessToken,
                        'create_time' => time()
                    ]);
            } else {
                $accessToken = $config['value'];
            }

        } else {
            $accessToken = self::getAT($member['appid'], $member['appsecret']);
            $res = Con::insert([
                'name' => 'ACCESS_TOKEN',
                'title' => 'access_token',
                'token' => $member['token'],
                'value' =>$accessToken,
                'create_time'=>time()
            ]);
        }
        return $accessToken;
    }
}