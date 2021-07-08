<?php
namespace app\api\controller;

use app\base\model\Config;
use app\base\model\Member;

use app\base\model\User;
use app\base\model\Record;
use app\base\model\Recordlog;
use app\base\model\SwipeRecord;

use \think\Exception;

 /**
     * 访客易接口-云平台
     * 
     * 
     * @date 2018/12/17
     */
class Fky extends Base
{

    /**
     * 检测员工信息是否有新版本
     * POST
     * @param key 
     * @param tecsun 
     * @param version 
     * 
     */
    public function checkEmpVersion()
    {
        if($this->request->isPost()){#属于post请求
            #验证数据
            $key = $this->request->post('key');
            $token = $this->request->post('token');
            $version = $this->request->post('version');

            if(empty($key)) return json_encode(array('status'=>0,'info'=>'缺少key'));
            if(empty($token)) return json_encode(array('status'=>0,'info'=>'缺少token'));
            if(empty($version)) return json_encode(array('status'=>0,'info'=>'缺少version'));

            if($key != 'tecsun') {
                return json_encode(array('status'=>0,'msg'=>'错误的key'));
            }

            $is_member_obj = Member::where([
                    'token' => $token
                ])->find();

            if(!$is_member_obj) return json_encode(array('status'=>0,'info'=>'token未授权'));

            #查询当前版本号
            $this_version_obj = Config::where([
                    'token' => $token,
                    'name'  => 'THIS_VERSION',
                ])->find();

            #判断当前版本与请求版本时候一致，如果一致，放回一致的提示，如果不一致，返回不一致的提示 
            if($this_version_obj){#系统内已经存在版本号
                $this_version_arr = $this_version_obj->toArray();
                
                if($this_version_arr['value'] = $version){#系统当前的版本号与请求的版本号一致
                    return json_encode(array('status'=>200, 'message'=>'No', 'info'=>'系统当前版本号与请求的版本号一致', 'version'=>$this_version_arr['value']));
                } else {#系统当前版本号与请求的版本号不一致
                    return json_encode(array('status'=>200, 'message'=>'Yes', 'info'=>'系统当前版本号与请求的版本号不一致', 'this_version'=>$this_version_arr['value']));
                }          
            } else {#系统内目前还没有存在版本号
                return json_encode(array('status'=>200, 'message'=>'Yes', 'info'=>'系统内目前还没有存在版本号'));
            }
        } else {#不属于post请求
            return json_encode(array('status'=>0,'info'=>'请使用Post请求方式'));
        }        
    }

    /**
     * 推送员工信息
     * POST
     * @param key 
     * @param tecsun 
     * @param version 
     * @param data 
     * 
     */
    public function pushEmployee()
    {
        if($this->request->isPost()){#属于post请求
            #验证数据
            $key = $this->request->post('key');
            $data = $this->request->post('data');
            $token = $this->request->post('token');
            $version = $this->request->post('version');

            if(empty($key)) return json_encode(array('status'=>0,'info'=>'缺少key'));
            if(empty($data)) return json_encode(array('status'=>0,'info'=>'缺少data'));
            if(empty($token)) return json_encode(array('status'=>0,'info'=>'缺少token'));
            if(empty($version)) return json_encode(array('status'=>0,'info'=>'缺少version'));

            if($key != 'tecsun') {
                return json_encode(array('status'=>0,'msg'=>'错误的key'));
            }

            $is_member_obj = Member::where([
                    'token' => $token
                ])->find();

            if(!$is_member_obj) return json_encode(array('status'=>0,'info'=>'token未授权'));

            #更新员工信息
            try {
                foreach ($data as $v) {
                    $is_employee_obj = User::where([
                        'token' => $token,
                        'phone' => $v['strTel'],
                        'type'  => '1',
                    ])->find();

                    if($is_employee_obj){#已经存在员工信息，需要更新
                        $is_employee_arr = $is_employee_obj->toArray();

                        if($v['iStatus'] == 0){#记录状态标记为非删除
                            $upadte_employee_res = User::update([
                                'id'            => $is_employee_arr['id'],
                                'name'          => $v['strName'],#姓名
                                'phone'         => $v['strTel'],#电话
                                'sex'           => $v['strSex'],#性别
                                'department'    => $v['strDept'],#部门
                                'room_number'   => $v['strRoom'],#办公室
                                'office_phone'  => $v['strOfficePhone'],#办公室电话
                                'ext_phone'     => $v['strExtOfficePhone'],#分机号码
                                'idcard'        => $v['strIdCertNo'],#身份证号码
                                'iccard'        => $v['strCardNo'],#ic卡号码
                                'code'        => $v['strCode'],#员工工号
                            ]);
                        } else {#记录状态标记为删除
                            $destry_employee_res = User::destry($is_employee_arr['id']);
                        }                    
                    } else {#目前还没有该员工信息，需要添加
                        $upadte_employee_res = User::insert([
                            'name'          => $v['strName'],#姓名
                            'phone'         => $v['strTel'],#电话
                            'sex'           => $v['strSex'],#性别
                            'department'    => $v['strDept'],#部门
                            'room_number'   => $v['strRoom'],#办公室
                            'office_phone'  => $v['strOfficePhone'],#办公室电话
                            'ext_phone'     => $v['strExtOfficePhone'],#分机号码
                            'idcard'        => $v['strIdCertNo'],#身份证号码
                            'iccard'        => $v['strCardNo'],#ic卡号码
                            'type'          => 1,#人员类型
                            'token'         => $token,
                            'status'        => 2,
                            'code'        => $v['strCode'],#员工工号
                        ]);
                    }
                }
            } catch (Exception $e) {
                return json_encode(array('status'=>0,'info'=>'更新员工信息时出错'));
            }
            
            #查询当前版本号
            $this_version_obj = Config::where([
                    'token' => $token,
                    'name'  => 'THIS_VERSION',
                ])->find();

            #判断当前版本与请求版本时候一致，如果一致，放回一致的提示，如果不一致，返回不一致的提示 
            if($this_version_obj){#系统内已经存在版本号
                $this_version_arr = $this_version_obj->toArray();
                $update_version_res = Config::update([
                        'id'        => $this_version_arr['id'],
                        'value'     => $version,
                    ]);                      
            } else {#系统内目前还没有存在版本号
                $insert_version_res = Config::insert([
                        'value'     => $version,
                        'token'     => $token,
                        'name'      => 'THIS_VERSION',
                        'title'     => '人员信息当前版本号'
                    ]);
            }

            #操作完毕，返回结果
            return json_encode(array('status'=>200,'message'=>'success'));
        } else {#不属于post请求
            return json_encode(array('status'=>0,'info'=>'请使用Post请求方式'));
        }        
    }

    /**
     * 注册账号 发送平台版本的普通账号到微信预约系统
     * POST
     * @param key 
     * @param tecsun 
     * @param version 
     * @param data 
     * 
     */



    // 给被访人推送访客来访或或访客离开的消息
    public function pushMessage()
    {
        $token = $this->request->post('token');
        $e_phone = $this->request->post('e_phone');
        $v_name = $this->request->post('v_name');
        $v_phone = $this->request->post('v_phone');
        $message = $this->request->post('message');

        if(empty($token)) return json_encode(array('status'=>0,'info'=>'缺少token'));

        #检查其他参数是否合法 接受者的手机号码，访客姓名，访客手机号码，访客来访时间
        if(!isset($e_phone)) return json_encode(array('status'=>0, 'info'=>'被访者手机号码不能为空'));
        if(!isset($v_name))  return json_encode(array('status'=>0, 'info'=>'访客姓名不能为空'));
        if(!isset($v_phone)) return json_encode(array('status'=>0, 'info'=>'访客手机号码不能为空'));
        if(!isset($message)) return json_encode(array('status'=>0, 'info'=>'备注信息不能为空'));

        $member = Member::where(['token'=>$token])->find();
        $member = $member ? $member->toArray() : $member;
        if(!$member){
            return json_encode(array('status'=>201, 'info'=>'token未授权'));
        }


        #查找被访者是否已经绑定了预约助手
        $employee = User::where(['phone' => $e_phone,'token'=> $token])->find();
        $employee = $employee ? $employee->toArray() : $employee;;
        if(!$employee) return json_encode(array('status'=>201, 'info'=>'被访者未绑定接受消息的公众号'));
        
        $this->initializeConfig($member['wechat_token']);
       
            // 给员工发送模板消息
            $template_data = [
                'touser'      => $employee['openid'],
                'template_id' => 'D8iTG0JgSzlA9vpyVkAtwJM8__zmUkqqn6vNem63DiM',
                'url'         => '',
                'data' => [
                    'first'     => [
                        'value' => $message,
                        'color' => '#173177',
                    ],
                    'keyword1'  => [
                        'value' => $v_name,
                        'color' => '#173177',
                    ],
                    'keyword2'  => [
                        'value' => date('Y-m-d H:i:s', time()),
                        'color' => '#173177',
                    ],
                    'remark'  => [
                        'value' => '请知晓',
                        'color' => '#173177',
                    ],
                ]
            ];
        
        // print_r($template_data);
        $send_template_res = $this->sendTemplate($template_data);
        return json_encode(array('status'=>200, 'info'=>'消息发送成功'));
    }

    public function sendTemplate($data)
    {
        $access_token = self::getBaseAccessToken($this->config->appid, $this->config->appsecret);
        // print_r($data);
        // die;
        $url  = 'https://api.weixin.qq.com/cgi-bin/message/template/send?access_token='. $access_token;
        $data = json_encode($data);

        $res  = $this->getContents($url, $data);
        $result = json_decode($res, true);
        // print_r($result);
    }
    
    // 获取Access_token
    public function getAccessToken()
    {
        // $member = Member::get(1000);
        // $member = $member ? $member->toArray() : $member;
        $config = Config::where([
                'name'   => 'ACCESS_TOKEN',
                'token'  => $this->config->token
            ])->find();
        if($config)
        {
            $config = $config->toArray();
            if(time() - $config['create_time'] > 7000)
            {
                $accessToken = self::getBaseAccessToken($this->config->appid, $this->config->appsecret);
                $res = Config::where([
                    'name'        => 'ACCESS_TOKEN',
                    'token'       => $this->config->token
                ])->update([
                    'value'       => $accessToken,
                    'create_time' => time()
                ]);
            } else {
                $accessToken = $config['value'];
            }
        } else {
            $accessToken = self::getBaseAccessToken($this->config->appid, $this->config->appsecret);
            $res = Config::insert([
                    'name'        => 'ACCESS_TOKEN',
                    'title'       => 'access_token',
                    'token'       => $this->config->token,
                    'value'       => $accessToken,
                    'create_time' => time()
            ]);

        }
        return $accessToken;
    }
}
