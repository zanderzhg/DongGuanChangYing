<?php
namespace app\ad\controller;

use app\base\model\Department as Dep;
// 部门管理，不考虑职务
class Department extends Base
{

	/*部门列表*/
	public function lst()
	{
		$department = Dep::where([
					'token' => session('token')
					])->order('id desc')
					->select();
		$this->assign( 'list', $department );
		return $this->fetch();
	}

	// 添加部门
	public function departmentAdd()
	{
		if( $this->request->isPost() )
		{
			$data = [
				'name'  => $this->request->post('department'),
				'token' => session('token'),
			];
			$department = Dep::where($data)->find();
			$data['create_time'] = date( 'Y-m-d H:i:s', time());
			if( $department )
			{
				return $this->error( '已经有了相同的部门名称，请核实后再添加');
			}
			$res = Dep::insert( $data );
			if( $res )
			{
				// $this->redirect( 'lst' );
				$this->success('添加部门成功', 'lst');
			} else {
				return $this->errot( '添加部门失败' );
			}
		} else {
			return $this->fetch();
		}
	}

	// 查看部门的详细信息
	public function departmentDetail()
	{
		$id = input( 'id' );
		$department = Dep::where( [ 'id'=> $id ] )->find();
		$this->assign( 'list', $department );
		return $this->fetch(); 
	}

	// 删除部门信息
	public function departmentDel()
	{
		$id[] = input( 'post.id/a' );
		if( $id[0] )
		{
			for( $i=0; $i<count($id[0]); $i++ )
			{
				$data = [ 'id' => $id[0][$i] ];
				$dels = Dep::destroy( $data );
			}
			print_r( $dels );
		} else {
			$id = input( 'id' );
			$res = Dep::destroy( $id );
			$this->success('删除部门信息成功','lst');
			// return $this->redirect( 'lst' );
		}
	}

	// 清空所有部门信息
	public function departmentDelAll()
	{
		if( session('id') != 0 )
		{
			$map['token'] = session( 'token' );
		}
		$map['id'] = [ '>', 1 ];
		$delid = Dep::where( $map )->column( 'id' );
		$dels = Dep::destroy( $delid );
		print_r( $dels );
	}


}
