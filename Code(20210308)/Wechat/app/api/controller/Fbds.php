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
     * 访客易接口-富邦大厦
     * 
     * 
     * @date 2019/2/20
     */
class Fbds extends Base
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
    
}
