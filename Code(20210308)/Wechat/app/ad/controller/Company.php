<?php
namespace app\ad\controller;

use app\base\model\Company as Com;
// 单位管理，不考虑职务
class Company extends Base
{

	/*单位列表*/
	public function lst()
	{
		$company = Com::where([
					'token' => session('token')
					])->order('id desc')
					->select();
		$this->assign( 'list', $company );
		return $this->fetch();
	}

	// 添加单位
	public function companyAdd()
	{
		if( $this->request->isPost() )
		{
			$data = [
				'name'  => $this->request->post('company'),
				'token' => session('token'),
			];
			$company = Com::where($data)->find();
			$data['create_time'] = date( 'Y-m-d H:i:s', time());
			if( $company )
			{
				return $this->error( '已经有了相同的单位名称，请核实后再添加');
			}
			$res = Com::insert( $data );
			if( $res )
			{
				// $this->redirect( 'lst' );
				$this->success('添加单位成功', 'lst');
			} else {
				return $this->errot( '添加单位失败' );
			}
		} else {
			return $this->fetch();
		}
	}

	// 查看单位的详细信息
	public function companyDetail()
	{
		$id = input( 'id' );
		$company = Com::where( [ 'id'=> $id ] )->find();
		$this->assign( 'list', $company );
		return $this->fetch(); 
	}

	// 删除单位信息
	public function companyDel()
	{
		$id[] = input( 'post.id/a' );
		if( $id[0] )
		{
			for( $i=0; $i<count($id[0]); $i++ )
			{
				$data = [ 'id' => $id[0][$i] ];
				$dels = Com::destroy( $data );
			}
			print_r( $dels );
		} else {
			$id = input( 'id' );
			$res = Com::destroy( $id );
			$this->success('删除单位信息成功','lst');
			// return $this->redirect( 'lst' );
		}
	}

	// 清空所有单位信息
	public function companyDelAll()
	{
		if( session('id') != 0 )
		{
			$map['token'] = session( 'token' );
		}
		$map['id'] = [ '>', 1 ];
		$delid = Com::where( $map )->column( 'id' );
		$dels = Com::destroy( $delid );
		print_r( $dels );
	}


}
