<?php
namespace app\ad\controller;
use app\base\model\Config;
class Refuse extends Base
{
	/*拒绝理由*/
	public function refuseReason()
	{
		
		// 不是系统管理员，查询该用户下的拒绝理由
		$refuse = Config::where([
						'name'  => 'REFUSE_REASONS',
						'token' => session('token'), 
						'title' => '拒绝理由'
					])->order( 'token desc' )
					->select();
		// 判断该用户是否已经设置拒绝理由
		// print_r($refuse);die;
		$this->assign( 'list', $refuse);
		return $this->fetch();
	}

	// 添加拒绝理由
	public function refuseAdd()
	{
		if( $this->request->isPost() )
		{
			// 获取表单数据
			$data = [
				'value' => $this->request->post('refuse'),
				'name'  => 'REFUSE_REASONS',
				'token' => session('token'),
				'title' => '拒绝理由',
			];
			// 查询该设置是否已经存在
			$refuse = Config::where($data)->find();



			// 获取表单数据
			// $data = [
			// 	'content' => $this->request->post('reason'),
			// 	'status' => '2',
			// 	'time' => '2018',
			// 	'evid' => '3116'
			// ];
			// // 查询该设置是否已经存在
			// $refuse = db('refuse')->where($data)->find();

			if($refuse)
			{
				// 已经有了相同的配置
				$this->error('已经有了相同的配置，请核实后再添加');
			}
			// 添加来访事由到该用户
			$res = Config::insert($data);
			if( $res )
			{
				// 添加来访事由成功
				return $this->redirect( 'refuseReason' );
			} else {
				// 添加来访事由失败
				$this->error( '添加拒绝理由失败' );
			}
		} else {
			return $this->fetch();
		}
	}

	// 查看拒绝理由详细信息
	public function refuseDetail()
	{
		// // 获取要查询数据的id
		// $id      = $this->request->param('id');		
		// // 根据id查询数据
		// $refuse = Config::where([
		// 			'id'	=> $id,
		// 			'token' => session('token'),
		// 		])->find();
		// $this->assign( 'list', $refuse );
		// return $this->fetch();

		// 获取要查询数据的id
		$id      = $this->request->param('id');		
		// 根据id查询数据
		$refuse = Config::where([
					'id'	=> $id,
					'token' => session('token'),
				])->find();
		$this->assign( 'list', $refuse );
		return $this->fetch();
	}

	// 删除拒绝理由
	public function refuseDel()
	{
		// 获取批量查询的id
		$id[] = input('post.id/a');

		// 根据id是否为空判断是否是批量删除操作
		if( $id[0] )
		{
			// 这是通过批量Ajax删除
			for( $i=0 ; $i<count($id[0]) ; $i++ )
			{
				// 把二维数组转化为一维数组
				$data = [ 'id' => $id[0][$i] ];
				// 批量删除数据
				$dels =Config::destroy($data);
			}
			// 把删除数据的id输出 这个有待考虑
			print_r( $dels );
		} else {
			// 这是通过单个a标签进行删除
			$id  = $this->request->param('id');
			// print_r($id);die;
			$res = Config::destroy($id);
			return $this->redirect('refuseReason');
		}
	}

	// 清空所有来访事由
	public function refuseDelAll()
	{

		// 判断是否是系统管理员
		if( session('id') != 0 )
		{
			// 不是系统管理员
			$map['token'] = session('token');
		}
		$map['id']   = ['>' ,1];
		$map['name'] = 'REFUSE_REASONS'; 
		$delid = Config::where($map)->column('id');
		$dels  = Config::destroy($delid);
		print_r( $dels );
	}
}
