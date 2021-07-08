<?php
// +----------------------------------------------------------------------
// | ThinkPHP [ WE CAN DO IT JUST THINK ]
// +----------------------------------------------------------------------
// | Copyright (c) 2006~2016 http://thinkphp.cn All rights reserved.
// +----------------------------------------------------------------------
// | Licensed ( http://www.apache.org/licenses/LICENSE-2.0 )
// +----------------------------------------------------------------------
// | Author: liu21st <liu21st@gmail.com>
// +----------------------------------------------------------------------
use think\Route;
Route::get([
	
]);
Route::post([
	
]);
Route::rule([
    'getEmpName/:token/:phone'  => 'api/Api/getEmpName',
  
    'getEmpName'  => 'api/Api/getEmpName',
    'duanxin'  => 'api/Api/duanxin',


    'w_v_detail/:id'  => 'wechat/visitor/detail',
    'w_v_record/:id'  => 'wechat/visitor/record',

    'getRecordByCode'  => 'api/Api/getRecordByCode',
    'getRecordByPhone'  => 'api/Api/getRecordByPhone',
    'channgRecordStatus'  => 'api/Api/channgRecordStatus',
    'receiveReserve'  => 'api/Api/receiveReserve',

    'wechatapi'     => 'wechat/we_chat/index', #微信公众号后台服务器配置URL路由地址

    
    'pushMessage'   => 'api/fky/pushMessage',
    'card'   => 'api/T/card',
    'addTestEmployee'   => 'api/T/addTestEmployee',

]);
return [
    '__pattern__' => [
        'name' => '\w+',
    ],
    '[hello]'     => [
        ':id'   => ['index/hello', ['method' => 'get'], ['id' => '\d+']],
        ':name' => ['index/hello', ['method' => 'post']],
    ],

];
