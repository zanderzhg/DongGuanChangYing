<?php
namespace app\api\controller;

use app\base\model\Config;
use app\base\model\Member;

use app\base\model\User;
use app\base\model\Record;
use app\base\model\Recordlog;
use app\base\model\SwipeRecord;

use \think\Exception;


class M6 extends Base
{

    /**
     * 获取用户的
     * 
     * 
     * 
     * 
     */
    public function m6GetBooking()
    {
        if($this->request->isPost()){
            // $appid = $this->request->post('appid');
            // $appsecret = $this->request->post('appsecret');
            // $timestamp = $this->request->post('timestamp');
            $token  = $this->request->post('token');
            $code   = $this->request->post('code');
            $phone  = $this->request->post('phone');
            $idcard = $this->request->post('idcard');

            // if(!$appid){
            //     return json_encode((array('status'=>0,'msg'=>'appid不能为空')));
            // }

            // if(!$appsecret){
            //     return json_encode((array('status'=>0,'msg'=>'appsecret不能为空')));
            // }

            // if(!$timestamp){
            //     return json_encode((array('status'=>0,'msg'=>'timestamp不能为空')));
            // }

            if(!$token){
                return json_encode((array('status'=>0,'msg'=>'token不能为空')));
            }

            // if(!$this->checkToken($appid, $appsecret, $timestamp, $token)){
            //     return json_encode((array('status'=>0,'msg'=>'token验证失败')));
            // }

            $member = Member::where([
                    'token' => $token
                ])->find();
            $member = $member ? $member->toArray() : $member;

            if(!$member){
                return json_encode((array('status'=>0,'msg'=>'token未授权')));
            }


            // $key = $code ? 'code' : $phone ? 'phone' : 'idcard';

            // $value = $code ? $code : $phone ? $phone : $idcard;

            // if($value){
            //     return json_encode((array('status'=>0,'msg'=>'key值为空')));
            // }

            // $record = Record::where([
            //     $key => $value
            // ])->find();

            if($code){#查询条件是code
                $records = Record::where([
                        // l.code' => $this->request->param('code'),
                        'a.token'  => $token,
                        'l.url'    => $code,
                        // 'l.isUsed' => 0
                    ])
                    ->field('a.*, r.content, l.url_img')
                    ->alias('a')
                    ->join('refuse r','a.id = r.id','left')
                    ->join('recordlog l', 'a.id = l.id', 'left')
                    ->select();
            } elseif($phone){#查询条件是phone
                $records = Record::where([
                        // 'l.code' => $this->request->param('code'),
                        'a.token'  => $token,
                        'a.phone'  => $phone,
                        // 'l.isUsed' => 0
                    ])
                    ->field('a.*, r.content, l.url_img')
                    ->alias('a')
                    ->join('refuse r','a.id = r.id','left')
                    ->join('recordlog l', 'a.id = l.id', 'left')
                    ->select();
            } elseif($idcard){#查询条件是idcard
                $records = Record::where([
                        // 'l.code' => $this->request->param('code'),
                        'a.token'  => $token,
                        'a.idcard' => $idcard,
                        // 'l.isUsed' => 0
                    ])
                    ->field('a.*, r.content, l.url_img')
                    ->alias('a')
                    ->join('refuse r','a.id = r.id','left')
                    ->join('recordlog l', 'a.id = l.id', 'left')
                    ->select();
            } else {#查询条件只都为空
                return json_encode((array('status'=>0,'msg'=>'key值为空')));
            }

            return json_encode(array('status'=>200,'msg'=>'操作成功！','records'=>$records));
        } else {
            return json_encode((array('status'=>0,'msg'=>'非法请求方式')));
        }  
    }

    public function m6ChangeBooking()
    {
        if($this->request->isPost()){

            $token  = $this->request->post('token');
            $record_id   = $this->request->post('record_id');
            $name   = $this->request->post('name');

            if(!$token){
                return json_encode((array('status'=>0,'msg'=>'token不能为空')));
            }

            if(!$record_id){
                return json_encode((array('status'=>0,'msg'=>'record_id不能为空')));
            }

            if(!$name){
                return json_encode((array('status'=>0,'msg'=>'name不能为空')));
            }

            // if(!$this->checkToken($appid, $appsecret, $timestamp, $token)){
            //     return json_encode((array('status'=>0,'msg'=>'token验证失败')));
            // }

            $member = Member::where([
                    'token' => $token
                ])->find();
            $member = $member ? $member->toArray() : $member;

            if(!$member){
                return json_encode((array('status'=>0,'msg'=>'token未授权')));
            }

            $record =  Record::where([
                    'a.token'  => $token,
                    'a.id'     => $record_id,
                ])
                ->field('a.*, r.content, l.url_img, l.isUsed')
                ->alias('a')
                ->join('refuse r','a.id = r.id','left')
                ->join('recordlog l', 'a.id = l.id', 'left')
                ->find();

            $record = $record ? $record->toArray() : $record;
            if(!$record) {
                return json_encode((array('status'=>0,'msg'=>'数据不存在')));
            } elseif($record['isUsed'] == 1) {
                return json_encode((array('status'=>0,'msg'=>'数据状态已经被改变')));
            } elseif($record['name'] != $name) {
                return json_encode((array('status'=>0,'msg'=>'数据状态信息不对应')));
            }else {
                $up_recordlog = Recordlog::where([
                        'id' => $record_id
                    ])->update([
                        'isUsed' => 1
                    ]);
                if($up_recordlog) {
                    return json_encode((array('status'=>200,'msg'=>'操作成功！')));
                } else {
                    return json_encode((array('status'=>0,'msg'=>'操作失败！')));
                }
            }

        } else {
            return json_encode((array('status'=>0,'msg'=>'非法请求方式')));
        }  
    }

    public function m6GetUser()
    {
        if($this->request->isPost()){

            $token  = $this->request->post('token');
            $e_id   = $this->request->post('e_id');
            $e_phone   = $this->request->post('e_phone');

            if(!$token){
                return json_encode((array('status'=>0,'msg'=>'token不能为空')));
            }

            if(!$e_id){
                return json_encode((array('status'=>0,'msg'=>'e_id不能为空')));
            }

            if(!$e_phone){
                return json_encode((array('status'=>0,'msg'=>'e_phone不能为空')));
            }

            // if(!$this->checkToken($appid, $appsecret, $timestamp, $token)){
            //     return json_encode((array('status'=>0,'msg'=>'token验证失败')));
            // }

            $member = Member::where([
                    'token' => $token
                ])->find();
            $member = $member ? $member->toArray() : $member;

            if(!$member){
                return json_encode((array('status'=>0,'msg'=>'token未授权')));
            }

            $employee = User::where([
                    'id' => $e_id,
                    'token' => $token,
                    'type' => 1
                ])->find();
            $employee = $employee ? $employee->toArray() : $employee;

            if(!$employee){
                return json_encode((array('status'=>0,'msg'=>'人员不存在')));
            } elseif($employee['phone'] != $e_phone) {
                return json_encode((array('status'=>0,'msg'=>'人员信息不对应')));
            } else {
                return json_encode((array('status'=>200,'msg'=>'操作成功！', 'user'=>$employee)));
            }

        } else {
            return json_encode((array('status'=>0,'msg'=>'非法请求方式')));
        }  
    }

    public function m6GUploadVisitingData()
    {
        if($this->request->isPost()){

            $token  = $this->request->post('token');
            $e_id   = $this->request->post('e_id');
            $e_phone   = $this->request->post('e_phone');

            if(!$token){
                return json_encode((array('status'=>0,'msg'=>'token不能为空')));
            }

            if(!$e_id){
                return json_encode((array('status'=>0,'msg'=>'e_id不能为空')));
            }

            if(!$e_phone){
                return json_encode((array('status'=>0,'msg'=>'e_phone不能为空')));
            }

            // if(!$this->checkToken($appid, $appsecret, $timestamp, $token)){
            //     return json_encode((array('status'=>0,'msg'=>'token验证失败')));
            // }

            $member = Member::where([
                    'token' => $token
                ])->find();
            $member = $member ? $member->toArray() : $member;

            if(!$member){
                return json_encode((array('status'=>0,'msg'=>'token未授权')));
            }

            $employee = User::where([
                    'id' => $e_id,
                    'token' => $token,
                    'type' => 1
                ])->find();
            $employee = $employee ? $employee->toArray() : $employee;

            if(!$employee){
                return json_encode((array('status'=>0,'msg'=>'人员不存在')));
            } elseif($employee['phone'] != $e_phone) {
                return json_encode((array('status'=>0,'msg'=>'人员信息不对应')));
            } else {
                return json_encode((array('status'=>200,'msg'=>'操作成功！', 'user'=>$employee)));
            }

        } else {
            return json_encode((array('status'=>0,'msg'=>'非法请求方式')));
        }  
    }


    /**
     * 根据手机号码获取员工信息
     * 
     * @param string phone 员工手机号码
     * @param string token token
     * 
     * @date 2018/7/28
     */
    public function getEmpName()
    {
        $phone = $this->request->param('phone');
        $token = $this->request->param('token');

        if(!$phone)
        {
            return json_encode((array('status'=>0,'msg'=>'手机号码为空')));
        }
        if(!$token)
        {
            return json_encode((array('status'=>0,'msg'=>'token为空')));
        }
        try{
            $user = User::where([
                    'phone' => $phone,
                    'token' => $token,
                    'type'  => 1, # 人员的类型 0:普通访客 1:员工 -1:未绑定的员工
                ])->find();
        } catch (Exception $e){
            return json_encode((array('status'=>0,'msg'=>$e)));
        }
        if($user)
        {
            $user = $user->toArray();
            if($user['type'] == 0)
            {
                return json_encode(array('status'=>201,'msg'=>'普通访客不能最为访问对象'));
            } else if($user['status'] == 2){
                return json_encode(array('status'=>202,'msg'=>'被访员工未进行身份绑定'));
            } else{
                return json_encode(array('status'=>200,'empName'=>$user['name']));
            }
        } else {
            return json_encode(array('status'=>0,'msg'=>'找不到被访员工'));
        }
    }

    // 获取多条预约信息 包括访客和员工
    public function getRecords()
    {
        // $user = User::where(['openid'=>’'oq9n05U0u5u_g_KCJpqQ7Ku7h5gY'])->find();
        $user = User::where([
                'openid' => $this->request->param('openid'),
                'token'  => $this->request->param('token'),
            ])->find();
        $user = $user ? $user->toArray() : $user;
        if($user['type']==1){
             $records = Record::where([
                    'openid' => $this->request->param('openid'),
                    'a.token'=> $this->request->param('token'),
                ])
                ->field('a.*, r.content, l.url_img')
                ->alias('a')
                ->join('refuse r','a.id = r.id','left')
                ->join('recordlog l', 'a.id = l.id', 'left')
                ->select();
        } else if($user['type']==2){
            $records = Record::where([
                    'eopenid' => $this->request->param('openid'),
                    'token'   => $this->request->param('token'),
                ])
                ->field('a.*, r.content, l.url_img')
                ->alias('a')
                ->join('refuse r','a.id = r.id','left')
                ->join('recordlog l', 'a.id = l.id', 'left')
                ->select();
        } else{
            return json_encode(array('status'=>0,'msg'=>'操作前请先绑定身份！'));
        }
        // $records = Record::where(['a.openid'=>'oq9n05U0u5u_g_KCJpqQ7Ku7h5gY'])
        
        foreach ($records as $key => $value) {
            // $value = $value ? $value->toArray() : $value;
            $records[$key]['start_time'] = date('Y-m-d H:i', $records[$key]['start_time']);
            // print_r($value->name);
        }

        return json_encode(array('status'=>200,'msg'=>'操作成功！','records'=>$records));
    }

    // 根据二维码获取预约记录
    public function getRecordByCode()
    {
        $code = $this->request->param('code');
        // print_r($code);
        // $code = '3250006980';
        $records = Record::where([
                    // 'l.code' => $this->request->param('code'),
                    'l.url' => $code,
                ])
                ->field('a.*, r.content, l.url_img')
                ->alias('a')
                ->join('refuse r','a.id = r.id','left')
                ->join('recordlog l', 'a.id = l.id', 'left')
                ->select();
        // echo Record::
        return json_encode(array('status'=>200,'msg'=>'操作成功！','records'=>$records));
    }

    // 根据访客手机号码获取预约记录
    public function getRecordByPhone()
    {
        $phone = $this->request->param('phone');
        $records = Record::where([
                    // 'l.code' => $this->request->param('code'),
                    'phone' => $phone,
                    'l.isUsed' => 0
                ])
                ->field('a.*, r.content, l.url_img')
                ->alias('a')
                ->join('refuse r','a.id = r.id','left')
                ->join('recordlog l', 'a.id = l.id', 'left')
                ->select();
        // echo Record::
        return json_encode(array('status'=>200,'msg'=>'操作成功！','records'=>$records));
    }
    
    // 验证登记之后通知微信端盖预约已登记
    public function channgRecordStatus()
    {
        $id = $this->request->param('id');
        $res = Recordlog::update([
                    'id'    => 'id',
                    'isUsed' => '1'
                ]);

        return json_encode(array('status'=>200,'msg'=>'操作成功！','id'=>$id));
    }


    // 接受访客平板发送过来的登记信息
    public function receiveReserve()
    {
        if($this->request->isPost())
        {
            $data = [
                'phone'     => $this->request->post('phone'),
                'idcard'    => $this->request->post('idcard'),
                'name'      => $this->request->post('name'),
                'ephone'    => $this->request->post('ephone'),
                'number'    => $this->request->post('number'),
                'reasons'   => $this->request->post('reasons'),
                'startTime' => $this->request->post('start_time'),
            ];
            $data['reasons'] = $data['reasons'] ? $data['reasons'] : '';

            #获取被访员工信息
            $employee = User::where([
                            'phone'  => $data['ephone'], 
                            // 'token'  => $this->config->token
                        ])->find() ;
            // or return json_encode(array('status'=>0,'msg'=>'不能存在预约对象'));
            $employee = $employee ? $employee->toArray() : $employee;

            if(!$employee)
            {
                return json_encode(array('status'=>0,'msg'=>'不存在预约对象'));
            }
            #判断被访人是否具备被预约的条件
            if($employee['status'] != 1) return json_encode(array('status'=>0,'msg'=>'该预约对象已经解绑，不能对其进行预约'));
            if($employee['type'] == 0) return json_encode(array('status'=>0,'msg'=>'普通访客不能是被预约对象'));
            
            #拼接需要添加到数据库的数据
            $addData = [
                'idcard'     => $data['idcard'],     #访客身份证号码
                'vid'        => '0',         #访客id
                'evid'       => $employee['id'],        #员工id
                'company'    => '',    #访客公司
                'visittype'  => 0,                      #访问类型 0：访客预约 1：员工邀请
                'token'      => $employee['token'],     #token
                'number'     => $data['number'],        #来访人数
                'phone'      => $data['phone'],      #访客手机号码
                'ephone'     => $employee['phone'],     #员工手机号码
                'vopenid'    => '',  #访客openid
                'eopenid'    => $employee['openid'],  #员工openid
                'reasons'    => $data['reasons'],       #来访事由
                'name'       => $data['name'],       #访客姓名
                'ename'      => $employee['name'],      #员工姓名
                'address'    => $employee['address'],   #见面地址
                'create_time'=> time(),                 #记录生成时间
            ];
            $addData['start_time'] = strtotime($data['startTime']);#有效开始时间
            $addData['end_time']   = $addData['start_time'] + 60 * 60 * 2;#有效截止时间
            // $addData['end_time']   = $addData['start_time'] + 60 * 60 * $this->config->timer;#有效截止时间
            $res = Record::create($addData);
            if($res->id)
            {
                // 添加记录成功
                $template_url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxd75a94381f9edb0d&redirect_uri=http://wechat.tengenglish.cn/index.php/w_v_detail/{$res->id}.html&response_type=code&scope=snsapi_base&state=gh_4aefa5d2caa8#wechat_redirect";
                // $template_url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={$this->config->appid}&redirect_uri={$this->config->web_url}/index.php/w_v_detail/{$res->id}.html&response_type=code&scope=snsapi_base&state={$this->config->token}#wechat_redirect";
                // $template_url = '';
                // 给员工发送模板消息
                $sendData = [
                    'touser'      => trim($employee['openid']),
                    'template_id' => trim('9pVmswp_nFUHtE-yiEAKEvecUU2r2VFGHBgqmt-wwcU'),
                    // 'template_id' => trim($this->config->template_reserve),
                    'url'         => $template_url,
                    'data' => [
                        'first'     => [
                            'value' => '现场预约申请',
                            'color' => '#173177',
                        ],
                        'keyword1'  => [
                            'value' => $addData['name'],
                            'color' => '#173177',
                        ],
                        'keyword2'  => [
                            'value' => $addData['phone'],
                            'color' => '#173177',
                        ],
                        'keyword3'  => [
                            'value' => date('Y-m-d H:i', $addData['start_time']),
                            'color' => '#173177',
                        ],
                        'remark'    => [
                            'value' =>  "来访人数：" . $addData['number'].
                                        "\n预约事由：" . $addData['reasons'],
                            'color' => '#173177',
                        ]
                    ]
                ];
                // print_r($sendData);die;
                // print_r($this->sendWeixinMessage($sendData)); die;
                if($this->sendWeixinMessage($sendData))
                {
                    #发起预约成功
                    // $this->redirect(url('Text/done',array('message1'=>"已经发送请求",'message2'=>"敬请等待被访员工审批！")));
                    return json_encode(array('status'=>200,'msg'=>'已经发送请求, 敬请等待被访员工审批！'));
                }          
            } else {
                 // if($this->config->openid == '') $this->redirect(url('Text/done',array('message1'=>"服务器开小差了",'message2'=>"请稍后再试！")));
                return json_encode(array('status'=>0,'msg'=>'预约失败'));
            }
            #发起预约失败
            // $this->error('预约失败，请稍后再试！');
            return json_encode(array('status'=>0,'msg'=>'预约失败'));
        }
    }



}
