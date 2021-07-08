<?php
// +----------------------------------------------------------------------
// | ThinkPHP [ WE CAN DO IT JUST THINK ]
// +----------------------------------------------------------------------
// | Copyright (c) 2006-2016 http://thinkphp.cn All rights reserved.
// +----------------------------------------------------------------------
// | Licensed ( http://www.apache.org/licenses/LICENSE-2.0 )
// +----------------------------------------------------------------------
// | Author: 流年 <liu21st@gmail.com>
// +----------------------------------------------------------------------

// 应用公共文件
/**
 * 生成二维码
 * @param  string $url URL地址
 * @return string      二维码文件名
 */
function createQR( $url )
{
    // 引入库文件
    $res = import('Ercode.Phpqrcode');
    // var_dump($res);die;
    $QRcode   = new \QRcode();
    // 生成文件名
    $fileName = './QRcode/' . md5( $url ) . '.png';
    // 纠错级别： L、M、Q、H
    $errorCorrectionLevel = 'L';
    // 二维码的大小：1到10
    $matrixPointSize = 10;
    $result =  $QRcode::png( $url, $fileName, $errorCorrectionLevel, $matrixPointSize, 2);
    // var_dump($fileName);
    // var_dump($result);die;
    return $fileName;
}

function isMobile()
{ 
    if (isset ($_SERVER['HTTP_X_WAP_PROFILE'])) {
        return true;
    } 

    if (isset ($_SERVER['HTTP_VIA'])) { 
        return stristr($_SERVER['HTTP_VIA'], "wap") ? true : false;
    } 

    if (isset ($_SERVER['HTTP_USER_AGENT'])) {
        $clientkeywords = array ('nokia',
            'sony',
            'ericsson',
            'mot',
            'samsung',
            'htc',
            'sgh',
            'lg',
            'sharp',
            'sie-',
            'philips',
            'panasonic',
            'alcatel',
            'lenovo',
            'iphone',
            'ipod',
            'blackberry',
            'meizu',
            'android',
            'netfront',
            'symbian',
            'ucweb',
            'windowsce',
            'palm',
            'operamini',
            'operamobi',
            'openwave',
            'nexusone',
            'cldc',
            'midp',
            'wap',
            'mobile'
        ); 

        if (preg_match("/(" . implode('|', $clientkeywords) . ")/i", strtolower($_SERVER['HTTP_USER_AGENT']))) {
            return true;
        } 
    } 

    if (isset ($_SERVER['HTTP_ACCEPT'])) { 
        
        if ((strpos($_SERVER['HTTP_ACCEPT'], 'vnd.wap.wml') !== false) && (strpos($_SERVER['HTTP_ACCEPT'], 'text/html') === false || (strpos($_SERVER['HTTP_ACCEPT'], 'vnd.wap.wml') < strpos($_SERVER['HTTP_ACCEPT'], 'text/html')))) {
            return true;
        } 
    } 
    return false;
} 
if (isMobile()==true) {
    return true;
}else{
    return false;
}

function JSON($array) {
    arrayRecursive($array, 'urlencode', true);
    $json = json_encode($array);
    return urldecode($json);
}

function arrayRecursive(&$array, $function, $apply_to_keys_also = false)  
{  
    static $recursive_counter = 0;  
    if (++$recursive_counter > 1000) {  
        die('possible deep recursion attack');  
    }  
    foreach ($array as $key => $value) {  
        if (is_array($value)) {  
            arrayRecursive($array[$key], $function, $apply_to_keys_also);  
        } else {  
            $array[$key] = $function($value);  
        }  
   
        if ($apply_to_keys_also && is_string($key)) {  
            $new_key = $function($key);  
            if ($new_key != $key) {  
                $array[$new_key] = $array[$key];  
                unset($array[$key]);  
            }  
        }  
    }  
    $recursive_counter--;  
}