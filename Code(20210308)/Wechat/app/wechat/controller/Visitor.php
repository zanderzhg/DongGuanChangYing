<?php
namespace app\wechat\controller;

use app\base\model\User;
use app\base\model\Refuse;
use app\base\model\Record;
use app\base\model\Recordlog;
use app\base\model\Member;
use app\base\model\Conference;
use app\base\model\Config as Con;
use app\base\controller\Base as Bas;




class Visitor extends Base
{
    /**
     *  我要预约
     *  
     *  @param string phone   员工手机号码
     *  @param string number  访客来访人数
     *  @param string reasons 来访事由
     *  @param string time    来访开始时间
     */
    public function reserve()
    {
        #判断是否获取到openid
        $this->config->openid != '' or $this->redirect(url('Text/done',array('message1'=>"服务器开小差了",'message2'=>"请稍后再试！")));
        #查找访客信息
        $visitor = User::where('openid', $this->config->openid)->find();
        $visitor = $visitor ? $visitor->toArray() : $visitor; 
        // print_r($this->config->openid);die;
        #获取来访理由
        $reasons = $this->getReasons();
        // print_r($this->config->token);
        // print_r($reasons);
       	
        // 判断是否是提交预约申请
        if($this->request->isPost()) {
       		$data = [
       			'phone' 	=> $this->request->post('ephone'),
                'area'    => $this->request->post('area'),
                'quyu'    => $this->request->post('quyu'),
       			'reasons' 	=> $this->request->post('reasons'),
                'startTime' => $this->request->post('time'),
                'car_num' => $this->request->post('car_num'),
       		];
            $data['reasons'] = $data['reasons'] ? $data['reasons'] : '';

            #获取被访员工信息
       		$employee = User::where([
                            'phone'  => $data['phone'], 
                            // 'token'  => $this->config->token
                        ])->find() or $this->error('不存在预约对象');
            $employee = $employee ? $employee->toArray() : $employee;

            #判断被访人是否具备被预约的条件
            if($employee['status'] != 1) $this->error('该预约对象已经解绑，不能对其进行预约');
            if($employee['type'] == 0) $this->error('普通访客不能是被预约对象');

            if($employee['token'] != $visitor['token']){
                User::where(['openid' => $visitor['openid']])->update(['token'=>$employee['token']]);
            }

            $this->config->token = $employee['token'];

            #拼接需要添加到数据库的数据
            $addData = [
       			'idcard' 	 => $visitor['idcard'],     #访客身份证号码
       			'vid'		 => $visitor['id'],         #访客id
       			'evid'		 => $employee['id'],        #员工id
       			'company'	 => $visitor['company'],    #访客公司
       			'visittype'  => 0,                      #访问类型 0：访客预约 1：员工邀请
       			'token'	     => $employee['token'],     #token
                'area'     => $data['area'],            #到访厂区
                'quyu'     => $data['quyu'],            #进入区域
                'car_num'     => $data['car_num'],      #车牌号
       			'phone'	     => $visitor['phone'],      #访客手机号码
       			'ephone'     => $employee['phone'],     #员工手机号码
                'vopenid'    => $this->config->openid,  #访客openid
                // 'eopenid'    => $this->config->openid,  #员工openid
                'eopenid'    => $employee['openid'],    #员工openid
       			'reasons'    => $data['reasons'],       #来访事由
       			'name'		 => $visitor['name'],       #访客姓名
       			'ename' 	 => $employee['name'],      #员工姓名
       			// 'address'	 => $employee['address'],   #见面地址
       			'create_time'=> time(),                 #记录生成时间
       		];
            if($data['area']=='生态园'){
                $addData['address'] = '广东省东莞市松山湖生态园新湖路2号';
                $data['areaTag'] = '生态园';
            }else if($data['area']=='松山湖'){
                $addData['address'] = '广东省东莞市松山湖高新技术产业开发区工业西三路六号';
                $data['areaTag'] = '大朗';
            }else if($data['area']=='大朗'){
                $addData['address'] = '广东省东莞市大朗镇水平村象山南路2号象山工业园怡高工业园';
                $data['areaTag'] = '大朗';
            }else if($data['area']=='东莞长盈'){
                $addData['address'] = '广东省东莞市大朗镇美景西路639号';
                $data['areaTag'] = '大朗';
            }else if($data['area'] = '茶山分厂') {
		$addData['address'] = '广东省东莞市松山湖生态园新湖路2号';
                $data['areaTag'] = '生态园';
	    }else{
                $addData['address'] = '大朗镇水平村象山工业园象和路273号';
                $data['areaTag'] = '生态园';
            }
            $addData['start_time'] = strtotime($data['startTime']);#有效开始时间
            $addData['end_time']   = $addData['start_time'] + 60 * 60 * $this->config->timer;#有效截止时间
            $res = Record::create($addData);
       		if($res->id) {
       			// 添加记录成功
                $template_url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={$this->config->appid}&redirect_uri={$this->config->web_url}/index.php/w_v_detail/{$res->id}.html&response_type=code&scope=snsapi_base&state={$this->config->token}#wechat_redirect";
       			
                // 给员工发送模板消息
       			$sendData = [
        			'touser'      => trim($employee['openid']),
                    'template_id' => trim($this->config->template_reserve),
                    'url'         => $template_url,
        			'data' => [
        				'first'     => [
							'value' => '预约申请',
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
							'value' =>  "进入区域：" . $addData['quyu'].
							            "\n预约事由：" . $addData['reasons'],
							'color' => '#173177',
						]
        			]
        		];
                // print_r($sendData);die;
                // print_r($this->sendWeixinMessage($sendData)); die;
        		if($this->sendWeixinMessage($sendData)) {
                    #发起预约成功
                    $this->redirect(url('Text/done',array('message1'=>"已经发送请求",'message2'=>"敬请等待被访员工审批！")));
                }          
            } else {
                 if($this->config->openid == '') $this->redirect(url('Text/done',array('message1'=>"服务器开小差了",'message2'=>"请稍后再试！")));
            }
            #发起预约失败
       		$this->error('预约失败，请稍后再试！');
       	} else {
            #查找最后一次历史信息
            $record  = Record::where([
                        'vopenid'   => $this->config->openid, 
                        // 'vopenid'   => 'gh_1197d26bdb2a', 
                        'visittype' => 0
                    ])->order('id desc')
                    ->find();
            #判断是否已经绑定了访客/员工身份 -1：没有绑定 1：已经绑定
            $visitor['status'] = $visitor == null ? '-1' : 1;
            if($visitor['status'] == -1) {
                $visitor['name']    = '';
                $visitor['phone']   = '';
                $visitor['idcard']  = '';
                $visitor['company'] = '';
                $visitor['token']   = session('token');
            }
            #先前端页面传递参数
            $this->assign('user', $visitor);
            $this->assign('record', $record);
            $this->assign('reasons', $reasons);
            $this->assign('status', $visitor['status']);
       		return $this->fetch();
       	}
    }

    /**
     *  查看预约的详细信息
     */
    public function detail()
    {
        $id     = $this->request->param('id');
        // print_r($id);die;
        $openid = $this->config->openid;
        is_numeric($id) or $this->redirect(url('Text/done',array('message1'=>"操作有误！",'message2'=>"请稍后再试！")));
        $record = Record::get($id);
        $record = $record ? $record->toArray() : $record;

        $record = db( 'record' )->alias( 'r' )
                            ->where( [
                                'r.id' => $id
                            ] )
                            // ->order( 'r.id desc' )->join( 'refuse l', 'r.id = l.id')->find();
                            ->order( 'r.id desc' )->find();
        // print_r($record);die;
        $refuse =db('refuse')->alias('r')
                        ->where([
                             'r.id' => $id
                        ])
                        ->order( 'r.id desc' )->find();


        $this->assign('list', $record);
        $this->assign('refuse', $refuse);
        return $this->fetch();
    }
    
    

    // 同意预约
    public function agree($id=0)
    {

        is_numeric($id) or $this->redirect(url('Text/done',array('message1'=>"操作有误",'message2'=>"请稍后再试！")));

        $record = Record::get($id);
        $record = $record ? $record->toArray() : $record;
        $record or $this->redirect(url('Text/done',array('message1'=>"操作有误",'message2'=>"请稍后再试！")));

        // print_r($record);die;
        if($record['status'] != 0) {
            $this->assign('list', $record);
            return $this->fetch('detail');
        }

        $employee = User::get($record['evid']);
        $employee = $employee ? $employee->toArray() : $employee;

        $employee or $this->redirect(url('Text/done',array('message1'=>"操作有误",'message2'=>"请稍后再试！")));
        $this->conf();
        $this->config->token = $employee['token'];

        $data = [
            'id'     => $id,
            'status' => 1,
        ];
        $res = Record::update($data);

        if($res->id) {
            $code   = Bas::getCode($id);
            $url    = $code;
            
            // $urlImg = $this->getHttpProtocol() . $_SERVER['SERVER_NAME']. '/' . createQR( $url );
            $url_img = $this->getHttpProtocol().$_SERVER['SERVER_NAME'].'/' . createQR( $url );
            $data   = [
                'id'      => $id,
                'url'     => $url,
                'url_img' => $url_img,
                'code'    => $code,
                'token'   => $this->config->token
            ];

            $result = Recordlog::insert($data);

            if($result)
            {

                // 添加记录成功
                $template_url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={$this->config->appid}&redirect_uri={$this->config->web_url}/index.php/w_v_record/{$res->id}.html&response_type=code&scope=snsapi_base&state={$this->config->token}#wechat_redirect";
                // 给员工发送模板消息
                $member = Member::where([
                        'token' => trim($this->config->token)
                    ])->find();
                $sendData = [
                    'touser'      => $record['vopenid'],
                    'template_id' => trim($this->config->template_success),
                    'url'         => $template_url,
                    'topcolor' => '#FF0000',
                    'data'     => [
                        'first'     => [
                            'value' => '我发出的预约确认通知',
                            'color' => '#173177',
                        ],
                        'keyword1'  => [
                            'value' => '访客预约',
                            'color' => '#173177',
                        ],
                        'keyword2'  => [
                            'value' => date('Y-m-d H:i',$record['start_time']),
                            'color' => '#173177',
                        ],
                        'keyword3'  => [
                            'value' => $record['address'],
                            'color' => '#173177',
                        ],
                        'remark'    => [
                            'value' =>  "访客姓名：".$record['name'].
                                        "\n来访事由：".$record['reasons'],
                            'color' => '#173177',
                        ],
                    ],
                ];

                $this->sendWeixinMessage($sendData) or $this->redirect(url('Text/done',array('message1'=>"服务器开小差了",'message2'=>"请联系访客重新发起预约！")));

                $this->redirect(url('Text/done',array('message1'=>"审批成功",'message2'=>"敬请等待访客来访！")));
            }
        }

        $this->redirect(url('Text/done',array('message1'=>"服务器开小差了",'message2'=>"请稍后再试！")));;
    }

    // 拒绝预约
    public function refuse($id=0)
    {
        is_numeric($id) or $this->redirect('Text/err');

        $record = Record::get($id);
        $record = $record ? $record->toArray() : $record;

        
        $record or $this->redirect('Text/err');


        if($record['status'] != 0){
            $this->assign('list, $record');
            return $this->fetch('detail');
        }

        $employee = User::get($record['evid']);
        $employee = $employee ? $employee->toArray() : $employee;

        $employee or $this->redirect(url('Text/done',array('message1'=>"操作有误",'message2'=>"请稍后再试！")));

        $this->conf();
        $this->config->token = $employee['token'];

        if($this->request->isPost()) {
            if(trim($this->request->post('refuseReasons')) == '') $this->error('拒绝理由不能为空');
            $recordData = [
                'id'    => $id,
                'status'=> 2
            ];
            $data = [
                'id'      => $id,
                'evid'    => $record['evid'],
                'time'    => date('Y-m-d H:i:s'),
                'content' => trim($this->request->post('refuseReasons'))
            ];
            $re = Record::update($recordData);


            if(!$re) $this->redirect('Text/err');

            $res = Refuse::insert($data);
            if($res)
            {
                 // 添加记录成功
                $template_url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={$this->config->appid}&redirect_uri={$this->config->web_url}/index.php/w_v_detail/{$id}.html&response_type=code&scope=snsapi_base&state={$this->config->token}#wechat_redirect";
                // 给员工发送模板消息
                $sendData = [
                    'touser'      => $record['vopenid'],
                    'template_id' => trim($this->config->template_failure),
                    'url'         => $template_url,
                    'topcolor' => '#FF0000',
                    'data'     => [
                        'first'     => [
                            'value' => '对方拒绝了您的预约',
                            'color' => '#173177',
                        ],
                        'keyword1'  => [
                            'value' => '访客预约',
                            'color' => '#173177',
                        ],
                        'keyword2'  => [
                            'value' => '访客预约',
                            'color' => '#173177',
                        ],
                        'keyword3'  => [
                            'value' => date('Y-m-d H:i', $record['start_time']),
                            'color' => '#173177',
                        ],
                        'remark'    => [
                            'value' =>  "拒绝原因：".$data['content'],
                            'color' => '#173177',
                        ],
                    ]
                ];
                $this->sendWeixinMessage($sendData) or $this->redirect(url('Text/done',array('message1'=>"服务器开小差了",'message2'=>"请联系访客重新发起预约！")));

                $this->redirect(url('Text/done',array('message1'=>"拒绝成功",'message2'=>"访客将不会获得来访权限")));
            }
        } else {
            $this->assign('id', $id);
            return $this->fetch();
        }
    }

    /**
     *  获取单条预约凭证
     */
    public function record()
    {
        $id = $this->request->param('id');
        // print_r($id);die;
        is_numeric($id) or $this->redirect(url('Text/done',array('message1'=>"操作有误",'message2'=>"请稍后再试！")));

        $record    = Record::get($id);
        $recordlog = Recordlog::where('id',$id)->find();
        $record    = $record ? $record->toArray() : $record;
        $recordlog = $recordlog ? $recordlog->toArray() : $recordlog;
        $this->assign('record', $record);
        $this->assign('recordlog', $recordlog);
        return $this->fetch();
    }

    /**
     *  获取最近10条预约凭证
     *  
     */
    public function records()
    {
        $headimgurl = $this->config->headimgurl;
        $record = db( 'record' )->alias( 'r' )
        ->where( [ 
            'r.vopenid'=>$this->config->openid, 
            'r.token'=>$this->config->token,
            'r.visittype'=>'0' 
        ] )->order( 'r.id desc' )->limit( 10 )->join( 'recordlog l', 'r.id = l.id')->select();
        // print_r($record);
        $this->assign('list', $record);
        $this->assign('headimgurl', $headimgurl);
        return $this->fetch();
    }

    /**
     * 修改访客信息
     * 
     * @param strinng openid  访客openid
     * @param strinng name    访客姓名
     * @param strinng phone   访客手机号码
     * @param strinng idcard  访客身份证号码
     * @param strinng sex     访客性别 2:未选择 1:男 0:女
     * @param strinng company 访客公司
     * 
     * @return status 0：修改成功，1：身份证号码和手机号码都已经存在，2：手机号码已经存在，3：身份证号码已经存在，4：信息没有修改
     * @date 2018/7/28
     */
    public function modify()
    {
        if($this->request->isPost()) {
            $data = [
                'name'      => $this->request->post('name'),
                'phone'     => $this->request->post('phone'),
                'idcard'    => strtoupper($this->request->post('idcard')),
                'sex'       => $this->request->post('sex'),
                'company'   => $this->request->post('company'),
                'openid'    => $this->config->openid,
            ];
            if($data['idcard'] != '') {
                $infoPhone = User::where([
                                'phone'     => $data['phone'],
                                'token'     => $this->config->token,
                                'openid'    => ['neq', $data['openid']]
                            ])->find();

                $infoIdcard = User::where([
                                'idcard'    => $data['idcard'],
                                'token'     => $this->config->token,
                                'openid'    => ['neq', $data['openid']]
                            ])->find();
                $infoPhone  = $infoPhone  ? $infoPhone->toArray()  : $infoPhone;
                $infoIdcard = $infoIdcard ? $infoIdcard->toArray() : $infoIdcard;
                if($infoPhone && $infoIdcard)
                {
                    #手机号码和身份证号码已经存在
                    return 1;
                }
                if($infoPhone)
                {
                    #手机号码已经存在
                    return 2;
                }
                if($infoIdcard)
                {
                    #身份证号码已经存在
                    return 3;
                }
            } else {
                $infoPhone = User::where([
                                'phone'     => $data['phone'],
                                'token'     => $this->config->token,
                                'openid'    => ['neq', $data['openid']]
                            ])->find();
                if($infoPhone)
                {
                    #手机号码已经存在
                    return 2;
                }
            }
            if(User::where(['openid' => $data['openid']])->update($data)) {
                #修改访客信息成功
                return 0;
            } else {
                #修改访客信息失败
                return 4;
            }
        }
    }

    /**
     * 添加访客信息
     * 
     * @param strinng openid  访客openid
     * @param strinng name    访客姓名
     * @param strinng phone   访客手机号码
     * @param strinng idcard  访客身份证号码
     * @param strinng sex     访客性别 2:未选择 1:男 0:女
     * @param strinng company 访客公司
     * 
     * @return status 0：修改成功，1：身份证号码和手机号码都已经存在，2：手机号码已经存在，3：身份证号码已经存在，4：信息没有修改
     * @date 2018/7/28
     */
    public function add()
    {
        if($this->request->isPost())
        {
            $data = [
                'status'      => 1,     #记录的状态 1：已绑定，2：未绑定
                'type'        => 0,     #人员类型   0：访客，1：员工
                'openid'      => $this->config->openid,
                'token'       => $this->config->token,
                'name'        => $this->request->post('name'),
                'phone'       => $this->request->post('phone'),
                'idcard'      => strtoupper($this->request->post('idcard')),
                'sex'         => $this->request->post('sex'),
                'company'     => $this->request->post('company'),
                'create_time' => time(),
            ];
            if($data['idcard'] != '') {
                $infoPhone = User::where([
                                'phone'     => $data['phone'],
                                'token'     => $this->config->token,
                                'openid'    => ['neq', $data['openid']]
                            ])->find();

                $infoIdcard = User::where([
                                'idcard'    => $data['idcard'],
                                'token'     => $this->config->token,
                                'openid'    => ['neq', $data['openid']]
                            ])->find();
                $infoPhone  = $infoPhone  ? $infoPhone->toArray()  : $infoPhone;
                $infoIdcard = $infoIdcard ? $infoIdcard->toArray() : $infoIdcard;
                if($infoPhone && $infoIdcard) {
                    #手机号码和身份证号码已经存在
                    return json(array('status'=>1));
                }
                if($infoPhone) {
                    #手机号码已经存在
                    return json(array('status'=>2));
                }
                if($infoIdcard) {
                    #身份证号码已经存在
                    return json(array('status'=>3));
                }
            } else {
                $infoPhone = User::where([
                                'phone'     => $data['phone'],
                                'token'     => $this->config->token,
                                'openid'    => ['eq', $data['openid']]
                            ])->find();
                if($infoPhone) {
                    #手机号码已经存在
                    return json(array('status'=>2));
                }
            }
            if(User::insert($data)) {
                #添加访客信息成功
                return json(array('status'=>0));
            } else {
                #添加访客信息失败
                return json(array('status'=>4));
            }
        }
    }	
}
