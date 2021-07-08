<?php
namespace app\ad\controller;
use app\base\model\Config;
class Address extends Base
{
	/*来访事由*/
	public function visitingAddress()
	{
		
		// 不是系统管理员，查询该用户下的来访事由
		$address = Config::where([
						'name'  => 'ADDRESS',
						'token' => session('token')
					])->order( 'token desc' )
					->select();
		// 判断该用户是否已经设置来访事由

		$this->assign( 'list', $address );
		return $this->fetch();
	}

	// 添加来访事由
	public function addressAdd()
	{
		if( $this->request->isPost() )
		{
			// 获取表单数据
			$data = [
				'value' => $this->request->post('reason'),
				'name'  => 'ADDRESS',
				'token' => session('token'),
				'title' => '来访事由',
			];
			// 查询该设置是否已经存在
			$address = Config::where($data)->find();
			if($address)
			{
				// 已经有了相同的配置
				$this->error('已经有了相同的配置，请核实后再添加');
			}
			// 添加来访事由到该用户
			$res = Config::insert($data);
			if( $res )
			{
				// 添加来访事由成功
				return $this->redirect( 'visitingAddress' );
			} else {
				// 添加来访事由失败
				$this->error( '添加来访事由失败' );
			}
		} else {
			return $this->fetch();
		}
	}

	// 查看来访事由详细信息
	public function addressDetail()
	{
		// 获取要查询数据的id
		$id      = $this->request->param('id');		
		// 根据id查询数据
		$address = Config::where([
					'id'	=> $id,
					'token' => session('token'),
				])->find();
		$this->assign( 'list', $address );
		return $this->fetch();
	}

	// 删除来访事由
	public function addressDel()
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
				$dels = Config::destroy($data);
			}
			// 把删除数据的id输出 这个有待考虑
			print_r( $dels );
		} else {
			// 这是通过单个a标签进行删除
			$id  = $this->request->param('id');
			$res = Config::destroy($id);
			return $this->redirect('visitingReason');
		}
	}

	// 清空所有来访事由
	public function addressDelAll()
	{
		// 判断是否是系统管理员
		if( session('id') != 0 )
		{
			// 不是系统管理员
			$map['token'] = session('token');
		}
		$map['id']   = ['>' ,1];
		$map['name'] = 'ADDRESS'; 
		$delid = Config::where($map)->column('id');
		$dels  = Config::destroy($delid);
		print_r( $dels );
	}
}
