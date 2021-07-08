<?php
// echo "ceshi";die;
require_once('./response.php');
require_once('./check.php');
require_once('./db.php');

const KEY = 'tecsun';

$id    = isset($_GET['id']) ? $_GET['id'] : '';
$key   = isset($_GET['key']) ? $_GET['key'] : '';
$func  = isset($_GET['func']) ? $_GET['func'] : '';
$token = isset($_GET['token']) ? $_GET['token'] : '';
$pageInt = isset($_GET['pageInt']) ? (int)$_GET['pageInt'] : 1;
$pageSize = isset($_GET['pageSize']) ? (int)$_GET['pageSize'] : 200;

$pageInt = $pageInt == 0 ? 1 : $pageInt;

// 判断秘钥是否正确
if ($key != KEY)
{
	// 401请求授权失败
	$data = array(
		'key'    => $key,
		);
	//返回错误信息
	return Response::show(401,'Unauthorized',$data);
	exit;
}
if( $token == '')
{
	// 401请求授权失败
	$data = array(
		'token'    => $token,
		);
	//返回错误信息
	return Response::show(401,'Unauthorized',$data);
	exit;
} else {
	$connect = DB::getInstance()->connect();
	if(!$connect)
	{
		return Response::show(500,'Internal Server Error');
	}
	// 查询token是否存在
	$sql = " SELECT * FROM `wx_member` WHERE token = '$token' ";
	// echo $sql;
	$res = $connect->query($sql);
	// var_dump($res->num_rows);
	// exit;
	if ($res->num_rows == 0) {
		$data = array(
			'token'    => $token,
		);
		return Response::show(401,'Unauthorized',$data);
		exit;
	} 
}
if(!is_int($pageInt)){
	// 401请求授权失败
	$data = array(
		'pageInt'    => $pageInt,
		);
	//返回错误信息
	return Response::show(401,'Unauthorized',$data);
	exit;
}
if(!is_int($pageSize)){
	// 401请求授权失败
	$data = array(
		'pageSize'    => $pageSize,
		);
	//返回错误信息
	return Response::show(401,'Unauthorized',$data);
	exit;
}
if($pageSize < 0){
	// 401请求授权失败
	$data = array(
		'pageSize'   => $pageSize,
		);
	//返回错误信息
	return Response::show(401,'Unauthorized',$data);
	exit;	
}
$pageSize = $pageSize > 400 ? 400 : $pageSize;
switch ( $func ) {
	case 'getBooking':
		getBooking( $token, $connect );
		break;
	case 'changeBooking':
		changeBooking( $token, $connect, $id );
		break;
	case 'getUser':
		getUser( $token, $connect, $pageInt, $pageSize);
		break;
	case 'sentMessage':
		sentMessage( $token, $connect, $data );
		break;
	default:
		bad();
		break;
}
die;
function getBooking( $token, $connect )
{
	$sql = " SELECT a.id as id, b.idcard as strIdCertNo, b.name as strVisitorName, b.phone as strTel,  b.sex as strSex,  a.code as strQRCode,  b.ename as strBookName,  b.ephone as strBookTel,  b.start_time as strValidTimeStart,  b.end_time as strValidTimeEnd,  b.account as strReason,  b.visittype as iVisitorType, b.accompanying as iVisitNum, b.car_num as strLicensePlate, b.create_time as strApplyTime
			FROM `wx_recordlog` as a LEFT JOIN `wx_record` as b on a.id = b.id
			WHERE a.token = '$token' and a.isUsed = 0 LIMIT 10 ";
	// echo $sql;
	$res = $connect->query($sql);
	// var_dump($res);
	$result = array();
	
	if ($res) {
		while($row = mysqli_fetch_assoc($res)){
			// echo $row['strValidTimeStart'];
			$row['strVisitorCompany'] = '';
			$row['strValidTimeStart'] = date("Y-m-d H:i:s",$row['strValidTimeStart']);
			$row['strValidTimeEnd']   = date("Y-m-d H:i:s",$row['strValidTimeEnd']);
			$row['strApplyTime']	  = date("Y-m-d H:i:s",$row['strApplyTime']);
			$result[] = $row;
		}
		// print_r($result);
		return Response::show(200,'success',$result);
	} else {
		return Response::show(200,'null');
	}
}

// 修改预约信息状态
function changeBooking( $token, $connect, $id )
{
	if( ( $id == '' ) || ( !is_numeric($id) ) )
	{
		$data = array(
			'id'    => $id,
		);
		return Response::show(401,'Unauthorized',$data);
		exit;
	}
	
	$sql = " UPDATE `wx_recordlog` SET isUsed = 1 WHERE id = $id and token = '$token' ";
	// echo $sql;
	$res = $connect->query( $sql );
	// var_dump($res);
	if( mysqli_affected_rows( $connect) )
	{
		$data = array(
			'id'    => $id,
		);
		return Response::show(200,'success',$data);
		exit;
	} else {
		return Response::show(500,'Internal Server Error');
		exit;
	}
}

// 获取所有员工信息
/**
 * @param $strName		 	员工姓名 		name
 * @param $strPhone		 	员工手机号码 	phone
 * @param $strSex		 	员工性别 	    sex
 * @param $strIdCard	 	员工身份证号码 	idcard
 * @param $strIcCard	 	员工IC号码 		iccard
 * @param $strAddress	 	员工所在地址 	address
 * @param $strDepartment	员工所在部门 	department
 * @param $strCompany		员工所在公司 	company 
 * @param $strOfficePhone	员工办公电话 	office_phone 
 * @param $strExtPhone		员工分机电话 	ext_phone 
 * @param $strRoomNumber	员工房间号 		room_number 
 * @param $intStatus		员工删除状态 	delete_time 
 * @param $strToken		 	token token 
 */
function getUser( $token, $connect,$pageInt, $pageSize )
{
	
	// $sql = " SELECT a.id as id, b.idcard as strIdCertNo, b.name as strVisitorName, b.phone as strTel,  b.sex as strSex,  a.code as strQRCode,  b.ename as strBookName,  b.ephone as strBookTel,  b.start_time as strValidTimeStart,  b.end_time as strValidTimeEnd,  b.account as strReason,  b.visittype as iVisitorType, b.accompanying as iVisitNum, b.car_num as strLicensePlate
	// 		FROM `wx_recordlog` as a LEFT JOIN `wx_record` as b on a.id = b.id
	// 		WHERE a.token = '$token' and a.isUsed = 0 LIMIT 10 ";
	$pageInt = $pageSize * ($pageInt - 1);
	$sql = " SELECT id, name as strName,sex as intSex, phone as strPhone, idcard as strIdCard, iccard as strIcCard,  address as strAddress, department as strDepartment, company as strCompany, token as strToken, office_phone as strOfficePhone, ext_phone as strExtPhone, room_number as strRoomNumber, delete_time as intStatus
			FROM `wx_user` 
			WHERE token = '$token' and type = 1
			LIMIT $pageInt, $pageSize";
			// LIMIT (($pageInt) * $pageSize), $pageSize";
	// echo $sql;
	$res = $connect->query($sql);
	// var_dump($res);
	$result = array();
	
	if ($res) {
		while($row = mysqli_fetch_assoc($res))
		{
			$result[] = $row;
		}
		// print_r($result);
		return Response::show(200,'success',$result);
	} else {
		return Response::show(200,'null');
	}
}

function sentMessage()
{
	
}

function bad()
{
	$data = array(
		'func'    => $func,
		);
	//返回错误信息
	return Response::show(401,'Unauthorized',$data);
	exit;
}
