<?php
namespace app\ad\controller;
use app\base\model\Member as Mem;

// 公司账号信息
class Member extends Base
{

	public function memberDetail()
	{
		$member = Mem::where([
						'token' => session('token')
					])->find();
		if( $this->request->isPost() )
		{
			//获取表单上传图片
			$file = request()->file('image');
			
			
			if($file){
				//移动到图片目录下
				$info = $file->move(ROOT_PATH . 'public' . DS . 'upload');

				if($info){
					 // 成功上传后 获取上传信息
			        // 文件类型
			        $extension = $info->getExtension();
			        // 文件路径
			        $saveName = $info->getSaveName();
			        // 获取图片宽高
			        $img_info = getimagesize("upload/".$saveName);

					//上传图片应为正方形
					if($img_info[0]==$img_info[1]){
						
						//上传图片类型应为jpg/png
				        if($extension == 'jpg' || $extension == 'png'){

				        }else{
				        	$this->error('文件类型错误');	
				        }

					}else{
						$this->error('上传图片应为正方形');	
					}
					$img_url = "/upload/".$saveName;

				}else{
					//上传失败
					echo $file->getError();
				}

			} else {
				$img_url = session('img_url');
			}

			$data = [
	        	// 'img_url'  => "/upload/".$saveName,
	        	'img_url'  => $img_url,
				'id'       => $member['id'],
				'username' => $this->request->post('username'),
				'password' => $this->request->post('password'),
				'nickname' => $this->request->post('nickname'),
				'address'  => $this->request->post('address'),
				
			];
			$res = Mem::update( $data );

			if( $res !== null )
			{
				session( 'img_url' , $data['img_url']  );
				session( 'nickname', $data['username'] );
				session( 'username', $data['nickname'] );
				session( 'address' , $data['address'] );
				// $this->redirect('memberDetail');
				$this->success( '修改成功', 'memberDetail' );
			} else {
				$this->error('修改失败');				
			}		

		}  else {

			$this->assign( 'list', $member );
			// print_r($member);die;
			$this->assign( 'img_url', session('img_url') );

			return $this->fetch();
		}
			
	}
}