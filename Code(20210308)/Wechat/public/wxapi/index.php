<?php 
/**
接口文件说明
接口功能一：访客机轮训查询数据，每次返回若干条，默认条数是10条

接口功能二：根据id修改记录的状态（是否是已经下发）

接口功能三：查询所有员工信息


*/
require_once('./response.php');
require_once('./check.php');
require_once('./db.php');

const KEY = 'tecsun';
date_default_timezone_set('PRC'); 
// 获取请求参数
$id = isset($_GET['id']) ? $_GET['id'] : '';
$key = isset($_GET['key']) ? $_GET['key'] : '';
$func = isset($_GET['func']) ? $_GET['func'] : '';
$token = isset($_GET['token']) ? $_GET['token'] : '';
$areaTag = isset($_GET['areaTag']) ? $_GET['areaTag'] : '';
$pageInt = isset($_GET['pageInt']) ? (int)$_GET['pageInt'] : 1;
$pageSize = isset($_GET['pageSize']) ? (int)$_GET['pageSize'] : 200;
$employee_data = isset($_POST['data']) ? $_POST['data'] : '';

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
// 判断公众号是否是注册公众号
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
	// var_dump($res);
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
// var_dump((int)$pageInt);
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
		getBooking( $token, $connect,$areaTag );
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
	case 'pushEmployee':
		pushEmployee( $token, $connect, $employee_data );
		break;
	default:
		bad();
		break;
}
die;

// 获取预约信息
/**
 * @param $strIdCertNo          访客身份证 idcard
 * @param $strVisitorCompany    访客单位   没有
 * @param $strVisitorName       访客姓名   name
 * @param $strTel               访客手机   phone
 * @param $strSex               访客性别   sex
 * @param $strQRCode            二维码号   .code
 * @param $strBookName          员工姓名   ename
 * @param $strBookTel           员工手机   ephone
 * @param $strValidTimeStart    来访时间   start_time
 * @param $strValidTimeEnd      截止时间   end_time
 * @param $strApplyTime         生成时间   create_time
 * @param $strReason            来访事由   account
 * @param $iVisitorType         客户类型   visittype
 * @param $iVisitNum            来访人数   accompanying
 * @param $strLicensePlate      车牌号码   car_num
 */
function getBooking( $token, $connect,$areaTag )
{
	$sql = " SELECT a.id as id, b.idcard as strIdCertNo, b.name as strVisitorName, b.phone as strTel,  b.sex as strSex,  a.code as strQRCode,  b.ename as strBookName,  b.ephone as strBookTel,  b.start_time as strValidTimeStart,  b.end_time as strValidTimeEnd,  b.reasons as strReason,  b.visittype as iVisitorType, b.number as iVisitNum, b.car_num as strLicensePlate, b.create_time as strApplyTime, b.company as strVisitorCompany,b.quyu,b.area
			FROM `wx_recordlog` as a LEFT JOIN `wx_record` as b on a.id = b.id
			WHERE a.token = '$token' and a.isUsed = 0  and b.areaTag='$areaTag' order by id desc LIMIT 30 ";
	// echo $sql;
	$res = $connect->query($sql);
	// var_dump($res);
	$result = array();
	
	if ($res) {
		while($row = mysqli_fetch_assoc($res)){
			// $row['strVisitorCompany'] = '';
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

/**
 * 访客易推送员工信息
 * @param $strName		 	员工姓名 		name
 * @param $strPhone		 	员工手机号码   	phone
 * @param $strSex		 	员工性别 	    sex
 * @param $strIdCard	 	员工身份证号码 	idcard
 * @param $strIcCard	 	员工IC号码 		iccard
 * @param $strAddress	 	员工所在地址  	address
 * @param $strDepartment	员工所在部门  	department
 * @param $strCompany		员工所在公司  	company 
 * @param $strOfficePhone	员工办公电话  	office_phone 
 * @param $strExtPhone		员工分机电话  	ext_phone 
 * @param $strRoomNumber	员工房间号 		room_number 
 * @param $intStatus		员工删除状态  	delete_time 
 * @param $strToken		 	token     		token 
 */
function pushEmployee( $token, $connect, $employee_data )
{
	if($employee_data == '') {#员工数据为空
		$data = array(
			'data'    => $employee_data,
		);
		return Response::show(401,'Unauthorized',$data);
		exit;
	} else {#员工数据不为空，需要判断员工数据是否是json数据
		$employee_data = json_decode($employee_data, true);

			// return Response::show(401,'Unauthorized', $employee_data);
		
		if($employee_data == ''){#员工数据为非json数据
			return Response::show(401,'Unauthorized', '请数据json格式的data数据');
			exit;
		}
	}

	foreach ($employee_data as $v) {#循环处理数据
		#查询该手机号码时候已经存在在后台
		$sql = " SELECT *
			FROM `wx_user` 
			WHERE token = '$token' and type = 1 and phone = '{$v['strTel']}' and delete_time is null;";
		$res = $connect->query($sql);

		if($res->num_rows != 0) {#已经存在员工信息，需要更新
			$row = mysqli_fetch_assoc($res);

			if($v['iStatus'] == 0){#记录状态标记为非删除
				$sql = "UPDATE wx_user
					SET name = '{$v['strName']}', 
						phone = '{$v['strTel']}', 
						department = '{$v['strDept']}', 
						room_number = '{$v['strRoom']}', 
						office_phone = '{$v['strOfficePhone']}', 
						ext_phone = '{$v['strExtOfficePhone']}', 
						idcard = '{$v['strIdCertNo']}', 
						iccard = '{$v['strCardNo']}'
					WHERE id = {$row['id']};";
			} else {#记录状态标记为删除
				// $sql = "DELETE wx_user
			 			// WHERE id = {$row['id']};";
			 	$sql = "UPDATE wx_user
					SET name = '{$v['strName']}', 
						phone = '{$v['strTel']}', 
						department = '{$v['strDept']}', 
						room_number = '{$v['strRoom']}', 
						office_phone = '{$v['strOfficePhone']}', 
						ext_phone = '{$v['strExtOfficePhone']}', 
						idcard = '{$v['strIdCertNo']}', 
						iccard = '{$v['strCardNo']}',
						delete_time = '111'
					WHERE id = {$row['id']};";
			}

		} else {#目前还没有该员工信息，需要添加
			$sql = "INSERT INTO wx_user(name, phone, department, room_number, office_phone, ext_phone, idcard, iccard, type, token, status)
					VALUES('{$v['strName']}', '{$v['strTel']}', '{$v['strDept']}', '{$v['strRoom']}', '{$v['strOfficePhone']}', '{$v['strExtOfficePhone']}', '{$v['strIdCertNo']}', '{$v['strCardNo']}', 1, '{$token}', 2);";
		}

		$res = $connect->query($sql);
	}

	return Response::show(200,'success',$employee_data);
	exit;
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

?>
