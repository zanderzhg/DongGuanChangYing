<?php
namespace app\wechat\controller;

use app\base\model\User;
use app\base\model\Record;
use app\base\model\Recordlog;
use app\base\model\Member;
use app\base\model\Conference;
use app\base\model\Config as Con;
use app\base\controller\Base as Bas;




class Employee extends Base
{
    /**
     *  员工绑定分流入口
     *  
     */
    public function index()
    {
        $openid    = $this->config->openid;
        $token     = $this->config->token;        
        $employee  = User::where([ 
                      'openid' => $openid, 
                      // 'token'  => $token 
                    ] )->find();
        $employee = $employee ? $employee->toArray() : $employee;
        if($employee) {
            // 绑定了员工或者访客
            if($employee['type'] == '0' || $employee['type'] == 0) {
                // 绑定了访客
                $this->redirect(url('Text/done',array('message1'=>"你不具有该模块权限",'message2'=>"如有需要，请联系工作人员！")));
            } else {
                // 绑定了员工
                // 跳转到员工基本信息的页面
                $this->assign( 'list', $employee );
                return $this->fetch();
              }
        } else {
            // 没有绑定员工也没有绑定访客 跳转到绑定页面
            $this->redirect( 'Employee/bind' );
        }
    }
    #绑定员工信息
    /**
     * 绑定员工信息
     * 
     * @param string phone 员工手机号码
     * 
     * 
     */
    public function bind()
    {
        if($this->request->isPost()) {
            #设置页面编码
            header("Content-Type:text/html;charset:utf-8");
            #拼接数据
            $data = [
                'phone'  => $this->request->post('phone'),
                'openid' => $this->config->openid,
                'status' => 1,
            ];
            #查找是否有该人员信息
            $employee = User::where( [ 
                        'phone' => $data['phone'],
                        // 'token' => $this->config->token,
                        ])->find();
            if(!empty( $employee)) {
                #后台能查找到该手机信息
                #判断时候是员工
                if( $employee['type'] == 1 ) {
                    #判断为员工
                    if($employee['status']==1) {
                        return $this->error('该员工信息已经被绑定，请核实信息或联系管理人员！');
                    }
                    #进行员工绑定
                    $result = User::where('phone', $data['phone'])->update($data);
                    if( $result !== false ) {
                        #员工绑定成功
                        return $this->redirect(url('Text/done',array('message1'=>"绑定成功！")));
                    } else {
                        #员工绑定失败
                        $this->error( '员工绑定失败，请稍后再试。');
                    }
                } else {
                    #判断为访客
                    #跳转到提示页面
                    $this->redirect(url('Text/done',array('message1'=>"你不具有该模块权限",'message2'=>"如有需要，请联系工作人员！")));
                }
            } else {
                #后台查找不到该手机信息
                $this->error( '后台没有该被访者信息，请核实信息是否有误。');
            }
        } else {
          return $this->fetch();
        }
    }

    #解除员工绑定
    public function release()
    {
        #拼接数据
        $data = ['status' => 2, 'openid' => ''];
        #解除绑定
        $result = User::where(['openid' => $this->config->openid])->update($data);
        #清空session
        if($result !== false)
        {
            #解除绑定成功
            session(null);
            return 1;
        } else {
            #解除绑定失败
            return 0;
        }
    }


    #预约信息_全部记录
    public function bookRecord(){
        #获取用户openid
        $token    = $this->config->token;  
        $headimgurl = $this->config->headimgurl;
        // print_r($headimgurl);die;      
        $openid   = $this->config->openid;
        $employee = User::where([ 
                    'openid' => $openid, 
                    // 'token'  => $token 
                  ])->find();
        $employee = $employee ? $employee->toArray() : $employee; 
        if( !empty($employee) )
        {
            #绑定了员工或者访客
            if( $employee['type'] == 0)
            {
                #绑定了访客
                $this->redirect(url('Text/done',array('message1'=>"你不具有该模块权限",'message2'=>"如有需要，请联系工作人员！")));
            } else {
                #绑定了员工 
                #查找员工预约记录
                $record = db( 'record' )->alias( 'r' )
                            ->where( [ 
                                'r.eopenid'=>$this->config->openid, 
                                'r.token'=>$this->config->token, 
                                // 'r.visittype'=>'1' 
                            ] )->order( 'r.id desc' )->limit( 10 )->select();
                $this->assign('list', $record);

                
                $userdetail  = User::where([ 
                      'openid' => $openid, 
                      'token'  => $token 
                    ] )->find();
                $this->assign('userdetail', $userdetail);

                $this->assign('headimgurl', $headimgurl);
                return $this->fetch();
            }
        } else {
            #没有绑定员工也没有绑定访客 跳转到绑定页面
            $this->redirect('Employee/bind');
      }
    }

    #预约凭证
    public function records()
    {
        #获取用户openid
        $token    = $this->config->token;
        $headimgurl = $this->config->headimgurl;      
        $openid   = $this->config->openid;
        $employee = User::where([ 
                    'openid' => $openid, 
                    // 'token'  => $token 
                  ])->find();
        $employee = $employee ? $employee->toArray() : $employee; 
        if( !empty($employee) )
        {
            #绑定了员工或者访客
            if( $employee['type'] == 0)
            {
                #绑定了访客
                $this->redirect(url('Text/done',array('message1'=>"你不具有该模块权限",'message2'=>"如有需要，请联系工作人员！")));
            } else {
                #绑定了员工 
                #查找员工预约记录
                $record = db( 'record' )->alias( 'r' )
                            ->where( [
                             'r.eopenid'=>$this->config->openid, 
                             'r.token'=>$this->config->token, 
                            ] )
                            ->order( 'r.id desc' )->limit( 10 )->join( 'recordlog l', 'r.id = l.id')->select();
                $this->assign('list', $record);
                $this->assign('headimgurl', $headimgurl);
                return $this->fetch();
            }
        } else {
            #没有绑定员工也没有绑定访客 跳转到绑定页面
            $this->redirect('Employee/bind');
      }
    }

    #根据id获取预约凭证
    public function recordid()
    {
        $id = $this->request->param('id');
        #获取用户openid
        $token    = $this->config->token;        
        $openid   = $this->config->openid;
        $employee = User::where([ 
                    'openid' => $openid, 
                    // 'token'  => $token 
                  ])->find();
        $employee = $employee ? $employee->toArray() : $employee; 
        if( !empty($employee) )
        {
            #绑定了员工或者访客
            if( $employee['type'] == 0)
            {
                #绑定了访客
                $this->redirect(url('Text/done',array('message1'=>"你不具有该模块权限",'message2'=>"如有需要，请联系工作人员！")));
            } else {
                #绑定了员工 
                #查找员工预约记录
                $record = db( 'record' )->alias( 'r' )
                            ->where( [
                             'r.eopenid'=>$this->config->openid, 
                             'r.token'=>$this->config->token,
                             'r.id' => $id
                            ] )
                            ->find();
                $this->assign('listid', $record);
                return $this->fetch();
            }
        } else {
            #没有绑定员工也没有绑定访客 跳转到绑定页面
            $this->redirect('Employee/bind');
      }
    }
  
    #预约凭证（未回复待审核记录0）
    public function bookrecordun()
    {
        #获取用户openid
        $token    = $this->config->token;  
        $headimgurl = $this->config->headimgurl;      
        $openid   = $this->config->openid;
        $employee = User::where([ 
                    'openid' => $openid, 
                    // 'token'  => $token 
                  ])->find();
        $employee = $employee ? $employee->toArray() : $employee; 
        if( !empty($employee) )
        {
            #绑定了员工或者访客
            if( $employee['type'] == 0)
            {
                #绑定了访客
                $this->redirect(url('Text/done',array('message1'=>"你不具有该模块权限",'message2'=>"如有需要，请联系工作人员！")));
            } else {
                #绑定了员工 
                #查找员工预约记录
                $record = db( 'record' )->alias( 'r' )
                            ->where( [
                             'r.eopenid'=>$this->config->openid, 
                             'r.token'=>$this->config->token, 
                             'r.visittype'=>'0',
                             'r.status'=>0 
                            ] )
                            ->order( 'r.id desc' )->limit( 10 )->select();
                $this->assign('listun', $record);

                $userdetail  = User::where([ 
                      'openid' => $openid, 
                      'token'  => $token 
                    ] )->find();
                $this->assign('userdetail', $userdetail);
                $this->assign('headimgurl', $headimgurl);
                return $this->fetch();
            }
        } else {
            #没有绑定员工也没有绑定访客 跳转到绑定页面
            $this->redirect('Employee/bind');
      }
    }

    #预约信息_已通过记录（status=1）
    public function bookrecordy()
    {
        #获取用户openid
        $token    = $this->config->token;  
        $headimgurl = $this->config->headimgurl;      
        $openid   = $this->config->openid;
        $employee = User::where([ 
                    'openid' => $openid, 
                    // 'token'  => $token 
                  ])->find();
        $employee = $employee ? $employee->toArray() : $employee; 
        if( !empty($employee) )
        {
            #绑定了员工或者访客
            if( $employee['type'] == 0)
            {
                #绑定了访客
                $this->redirect(url('Text/done',array('message1'=>"你不具有该模块权限",'message2'=>"如有需要，请联系工作人员！")));
            } else {
                #绑定了员工 
                #查找员工预约记录
                $record = db( 'record' )->alias( 'r' )
                            ->where( [
                             'r.eopenid'=>$this->config->openid, 
                             'r.token'=>$this->config->token, 
                             'r.status'=>1 
                            ] )
                            ->order( 'r.id desc' )->limit( 10 )->join( 'recordlog l', 'r.id = l.id')->select();
                $this->assign('listy', $record);

                 $userdetail  = User::where([ 
                      'openid' => $openid, 
                      'token'  => $token 
                    ] )->find();
                $this->assign('userdetail', $userdetail);
                $this->assign('headimgurl', $headimgurl);
                return $this->fetch();
            }
        } else {
            #没有绑定员工也没有绑定访客 跳转到绑定页面
            $this->redirect('Employee/bind');
      }
    }

    #预约信息_已拒绝记录(status=2)
    public function bookrecordn()
    {
        #获取用户openid
        $token      = $this->config->token;
        $headimgurl = $this->config->headimgurl;
        $openid     = $this->config->openid;
        $employee   = User::where([ 
                    'openid' => $openid, 
                    'token'  => $token 
                  ])->find();
        $employee = $employee ? $employee->toArray() : $employee; 
        if( !empty($employee) )
        {
            #绑定了员工或者访客
            if( $employee['type'] == 0)
            {
                #绑定了访客
                $this->redirect(url('Text/done',array('message1'=>"你不具有该模块权限",'message2'=>"如有需要，请联系工作人员！")));
            } else {
                #绑定了员工 
                #查找员工预约记录
                $record = db( 'record' )->alias( 'r' )
                            ->where( [
                             'r.eopenid'=>$this->config->openid, 
                             'r.token'=>$this->config->token, 
                             'r.visittype'=>'0',
                             'r.status'=>'2'
                            ] )
                            ->order( 'r.id desc' )->limit( 10 )->join( 'refuse l', 'r.id = l.id')->select();
                $this->assign('listn', $record);

                 $userdetail  = User::where([ 
                      'openid' => $openid, 
                      'token'  => $token 
                    ] )->find();
                $this->assign('userdetail', $userdetail);
                $this->assign('headimgurl', $headimgurl);
                return $this->fetch();
            }
        } else {
            #没有绑定员工也没有绑定访客 跳转到绑定页面
            $this->redirect('Employee/bind');
      }
    }

    #邀请访客
    public function invite()
    {
        #获取员工信息
        $token    = $this->config->token;        
        $openid   = $this->config->openid;
        $employee = User::where(['openid'=>$openid])->find();
        #判断是否已经绑定
        ($employee) or $this->redirect('bind');
        #判断是否是访客
        ($employee['type'] == 1) or $this->redirect(url('Text/done',array('message1'=>"你不具有该模块权限",'message2'=>"如有需要，请联系工作人员！")));
        $this->config->token = $employee['token'];
        if($this->request->isPost()) {
            #拼接预约记录数据
            $data['start_time']  = strtotime($this->request->post('time'));
            $data['end_time']    = $data['start_time'] + 4*60*60;
            $data['name']        = $this->request->post('name');
            $data['phone']       = $this->request->post('phone');
            $data['reasons']     = $this->request->post('reasons');
            $data['car_num']     = $this->request->post('car_num');
	    $data['area']        = $this->request->post('area');
            $data['quyu']        = $this->request->post('quyu');
            $data['evid']        = $employee['id'];
            $data['ephone']      = $employee['phone'];
            $data['ename']       = $employee['name'];
            $data['token']       = $employee['token'];
            $data['eopenid']     = $employee['openid'];
            $data['idcard']       =$this->request->post('idcard');
            $data['visittype']   = 1;
            $data['status']      = 1;
            $data['create_time'] = time();
            $data['reasons']     = $data['reasons'] != '' ? $data['reasons'] : ' ';
            
            #判断访客号码与员工号码是否一致
            if ($data['phone'] == $data['ephone']) $this->error('访客的手机号码与你的手机号码相同');
            if($this->request->post('area')=='生态园'){
                $data['address'] = '广东省东莞市松山湖生态园新湖路2号';
                $data['areaTag'] = '生态园';
            }else if($this->request->post('area')=='松山湖'){
                $data['address'] = '广东省东莞市松山湖高新技术产业开发区工业西三路六号';
                $data['areaTag'] = '大朗';
            }else if($this->request->post('area')=='大朗'){
                $data['address'] = '广东省东莞市大朗镇水平村象山南路2号象山工业园怡高工业园';
                $data['areaTag'] = '大朗';
            }else if($this->request->post('area')=='东莞长盈'){
                $data['address'] = '广东省东莞市大朗镇美景西路639号';
                $data['areaTag'] = '大朗';
            }else if($this->request->post('area')=='茶山分厂'){
                $data['address'] = '广东省东莞市松山湖生态园新湖路2号';
                $data['areaTag'] = '生态园';
            }else {
                $data['address'] = '大朗镇水平村象山工业园象和路273号';
                $data['areaTag'] = '生态园';
            }
            #插入预约数据
            $res = Record::create($data, true);
            if($res->id)
            {
                #插入预约记录成功
                #获取二维码单号
                $code    = Bas::getCode($res->id);
                $url     = $code;
                #生成二维码照片
                $url_img = $this->getHttpProtocol().$_SERVER['SERVER_NAME'].'/' . createQR( $url );
                #拼接二维码记录数据
                $data = [
                    'id'      => $res->id,
                    'url'     => $url,
                    'url_img' => $url_img,
                    'code'    => $code,
                    'token'   => $data['token'],
                ];
                #插入二维码记录
                if(Recordlog::create($data, true)->id)
                {
                    
                    #邀请成功
                    $this->redirect( 'Text/reservation', array('id' => $res->id ));
                } else {
                    #操作失败
                    $this->redirect(url('Text/done',array('message1'=>"操作失败",'message2'=>"如有需要，请联系工作人员！")));
                }
            } else {
                // print_r("666611");die;
                #插入预约记录失败
                $this->redirect(url('Text/done',array('message1'=>"操作失败",'message2'=>"如有需要，请联系工作人员！")));
            }
        } else {
            #获取上一次预约记录
            $record = User::where([
                          'openid' => $openid, 
                          'type'   => 1
                        ])->order('id desc')
                          ->find();
            #把数据装换为数组
            $record = $record ? $record->toArray() : $record;
            #获取来访事由
            $reasons = $this->getReasons($token);
            $this->assign('list',   $employee);
            $this->assign('record', $record);
            $this->assign('reasons',$reasons);
            return $this->fetch();
        }
    }


    // 发送短信
    public function send_SMS()
    {
        // return '1234';
        $flag   = 0; 
        $params = '';//要post的数据 
        $verify = rand( 1234, 9999 );//获取随机验证码      

        $mobile = $_POST['mobile'];
        //以下信息自己填以下

        // $mobile  ='18814127576';//手机号
        $argv   = array( 
            'name'   => 'tecsun',   //必填参数。用户账号
            'pwd'    => '8F78108F9BFF8A7993DA928465E6',   //必填参数。（web平台：基本资料中的接口密码）
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
        $con = substr( file_get_contents($url), 0, 1 );  //获取信息发送后的状态
        
        if($con == '0')
        {
            echo $verify;
        } else {
        }
    }

}
