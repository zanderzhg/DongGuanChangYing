<?php
namespace app\api\controller;

use app\base\model\Config;
use app\base\model\Member;

use app\base\model\User;
use app\base\model\Record;
use app\base\model\Recordlog;
use app\base\model\SwipeRecord;

use \think\Exception;


class T extends Base
{

   public function card()
    {
        set_time_limit(0);
        $member = Member::where(['token' => 'cyy'])->find();
        $member = $member->toArray();

        for ($i=0; $i < 1000; $i++) { 
            
            $user = $this->getUser($member);
            $eid = 80000;
            foreach ($user as $k => $v) {
                $v = $v->toArray();
                // print_r($v);
                $data = [];
                #拼接预约记录数据
                $data['start_time']  = date('Y-m-d 0:0:5', time());
                $data['end_time']    = date('Y-m-d 23:59:59', time());
                $data['name']        = '测试'.$i;
                $data['phone']       = $v['phone'];
                $data['reasons']     = '内部员工通行';
                $data['number']      = '1';
                $data['evid']        = $v['id'];
                $data['ecompany']    = $v['company'];
                $data['ephone']      = $v['phone'];
                $data['ename']       = $v['name'];
                $data['address']     = $v['address'];
                $data['token']       = 'cyy';
                $data['eopenid']     = $v['openid'];
                $data['visittype']   = 1;//内部员工通行
                $data['status']      = 1;
                // $data['create_time'] = date('Y-m-d H:i:s');
                $data['reasons']     = $data['reasons'] != '' ? $data['reasons'] : ' ';
                $data['start_time'] = strtotime($data['start_time']);
                $data['end_time']   = strtotime($data['end_time']);
                $data['create_time'] = time();

                 #插入预约数据
                $res = Record::create($data, true);
                
                if($res->id) {
                    #插入预约记录成功
                    #获取二维码单号
                    $code    = $this->getCode($res->id);
                    $url     = $code;
                    #生成二维码照片
                    $url_img = $this->getHttpProtocol() . $_SERVER['SERVER_NAME'].'/' . createQR( $url );
                    #拼接二维码记录数据
                    $logData = [
                        'id'      => $res->id,
                        'url'     => $url,
                        'url_img' => $url_img,
                        'code'    => $code,
                        'token'   => $data['token'],
                    ];
                    #插入二维码记录
                    if(Recordlog::create($logData, true)->id) {
                        #邀请成功
                        // $cardData = [
                        //     // 'eid'        => 0,
                        //     // 'eid'        => $v['id'],
                        //     'code'       => $logData['code'],
                        //     'token'      => $member['token'],
                        //     'openid'     => $v['openid'],
                        //     'url_img'    => $logData['url_img'],
                        //     'start_time' => $data['start_time'],
                        //     'end_time'   => $data['end_time'],
                        //     'create_time'=> $data['create_time'],
                        //     'eid' => $eid,
                        // ];
                        // // print_r($cardData);

                        // $card_data_res = Card::where(['eid'=>$eid])->find();
                        // // // print_r($eid);die;
                        // if($card_data_res){#更新二维码
                        //     $card_data_up_res = Card::where(['eid'=>$eid])->update($cardData);
                        // } else {#新增二维码
                        //     if(Card::create($cardData)->id) {
                        //     }
                        //     // $eid++;
                        // }
                        // $eid++;
                        
                    }
                }
            }
        }
    }


    // 获取员工信息
    public function getUser($member)
    {
        $user = User::where([
                'token' => $member['token'],
                'type' => '1',
            ])
        // ->limit(20)
        ->limit(1)
        // ->limit(999,1000)
        // ->limit(1999,1000)
        // ->limit(2999,1000)
        // ->limit(3999,1000)
        // ->limit(4999,1000)
        // ->limit(5999,1000)
        // ->limit(6999,1000)
        ->select();
        return $user;
    }

     /**
     * 生成门禁授权序列号
     * @param id 自增记录的id
     */
    public function getCode($id)
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

    #获取当前HTTP协议
    public function getHttpProtocol()
    {
        return $result = $_SERVER['SERVER_PORT'] == 443 ? 'https://' : 'http://';
    
    }

    // 添加3000个员工
    public function addTestEmployee()
    {
        // 添加500个访客
        // $all_employee_data = [];
        // for ($i=0; $i < 3000; $i++) { 
        //     $visitor_data = [
        //         'department_id' => '1',
        //         'employee_name' => '员工人'.$i,
        //         'employee_phone' => 13316188888+$i,
        //         'employee_gender' => '0',
        //         'employee_iccard' => 3201000+$i,
        //         'employee_no' => 45454+$i,
        //         'create_time' => date('Y-m-d H:i:s'),
        //     ];
        //     // $res = Visitor::insert($visitor_data);
        //     $all_employee_data[] = $visitor_data;
        // }

        // $res = Employee::insertAll($all_employee_data);

        $all_employee_data = [];
        for ($i=0; $i < 7000; $i++) { 
            $employee_data = [
                'name' => '员工'.$i,
                'phone' => 13314128888+$i,
                'sex' => '0',
                'openid' => '',
                'type' => 1,
                'token' => 'cyy',
                'company' => '德生',
                // 'position' => '开发仔',
                'create_time' => time(),
            ];
            // $all_employee_data[] = $employee_data;
            $res = User::insert($employee_data);
        }

        // $res = User::insertAll($all_employee_data);
    }




}

