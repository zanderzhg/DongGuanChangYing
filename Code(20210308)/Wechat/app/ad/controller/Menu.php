<?php
namespace app\ad\controller;
use app\base\model\Menu as Me;
use app\base\controller\Base as Bas;
use app\base\model\Member as Member;

class Menu extends Base
{
    /*自定义菜单管理*/
    public function menu()
    {
        
        if( Request()->isPost() )
        {
            $menu = trim( input('post.menu') );
            
            $url="https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=".trim(session('appid'))."&secret=".trim(session('appsecret'));
            $content=file_get_contents($url);
            $ret=json_decode($content,true);
            // var_dump($ret);die;
            // 创建
            $url="https://api.weixin.qq.com/cgi-bin/menu/create?access_token=".$ret['access_token'];

            $content=get_contents($url,$menu);
            // var_dump($content);die;
            $ret=json_decode($content,true);
            // var_dump($ret1);
            if($ret['errcode'] == 0){
                //创建成功
                // 保存菜单数据
                $menuRes = db( 'menu' )->insert( ['token'=>session('token'),'menu'=>$menu,'createtime'=>date('Y-m-d H:i:s')]);
                // 提示成功
                $this->success( '创建自定义菜单成功' );
            }else{
                //创建失败
                $this->error( '创建自定义菜单失败，请核实数据再试！');
            }
        } else {

            // $this->getMaterial();
            // $this->addMaterial();
            // $this->add_material(); 
            $menu = db( 'menu' )->where( ['token'=>session('token'),'mid'=>0])->order( 'position desc' )->select();
            $this->assign( 'menu', $menu);
            return $this->fetch();  
            
        }
    }

    // 添加一级自定义菜单
    public function menuAdd()
    {
        if($this->request->isPost())
        {

            $data['name'] = $this->request->post('menuName');
            $data['type'] = $this->request->post('menuType');
            $data['value'] = $this->request->post('menuValue');
            $data['position'] = $this->request->post('menuPosition');
            if($data['position'] == '1')
                    {
                        $data['position_name'] = '右';
                    } else if($data['position'] == '2'){
                        $data['position_name'] = '中';
                    }else{
                        $data['position_name'] = '左';
                    }
            $data['token'] = session('token');
            $data['create_time'] = time();
            // $res = db('menu')->insert($data);
            $res = Me::insert($data);

            if($res)
            {
                $this->redirect('menu');
            } else {
                $this->error('添加一级自定义菜单失败');
            }
        } else {

            $menu = Me::where([
                'token' => session('token'),
                'mid' =>0
                ])->order( 'id desc' )
                ->select();

            $mun = count($menu);
            if($mun >= 3)
            {
                return $this->error('超出数量啦');
            }

            #查找可编辑的菜单位置
            //所有的菜单位置
            $all_menu_data = [
                '1' => '右',
                '2' => '中',
                '3' => '左',
            ];

            //现在有的菜单位置
            $now_menu_data =[

            ];

            $menu11 = Me::where([
                'token' => session('token'),
                'mid' =>0
                ])->order( 'id desc' )
                ->select();

            foreach ($menu11 as $k) {
                $now_menu_data[$k['position']] = $k['position_name'];
            }

            //剩下可添加的菜单位置
            $diff_menu_data = array_diff($all_menu_data, $now_menu_data);
            // print_r($diff_menu_data);
            // die;
            
            // $diff_menu_data[$menu['position']]=$menu['position_name'];
            $data = [];
            foreach ($diff_menu_data as $k => $v) {
                $data[$k]['position']   = $k;
                $data[$k]['position_name'] = $v;
            }
            $this->assign('diff_menu_data',$data);
            // print_r($data);

            // die;

            return $this->fetch();
        }


    }

    // 查看一级自定义菜单详情
    public function menuDetail()
    {
        if($this->request->isPost())
        {
            $data['name'] = $this->request->post('menuName');
            $data['type'] = $this->request->post('menuType');
            
            if($data['type']=='URL')
            {
                $data['value'] = $this->request->post('menuValue');
            }else{
                $data['value'] = $this->request->post('menuValueKey');
            }

            $data['position'] = $this->request->post('menuPosition');
            
            if($data['position'] == '1'){
                $data['position_name'] = '右';
            } else if($data['position'] == '2'){
                $data['position_name'] = '中';
            }else{
                $data['position_name'] = '左';
            }
            // print_r($data);die;
            // $data['position_name'] = $this->request->post('menuPosition_name');
            $data['token'] = session('token');
            $data['id'] = $this->request->post('id');
            // $res = db('menu')->update($data);
            $res = Me::update($data);
            if($res)
            {
                return $this->redirect('menu');
            } else {
                return $this->error('修改一级自定义菜单失败');
            }

        } else {
            $id = input('id'); 
            $menu = db('menu')->where([
                'id'=>$id,
                'token'=>session('token')
            ])->find();
            $this->assign('menu', $menu);

            $lst = db( 'menu' )->where( [
                'token'=>session('token'),
                'mid'=>$id
            ])->order( 'position desc' )->select();
            $this->assign( 'lst', $lst);

            #查找可编辑的菜单位置

            $all_menu_data = [
                '1' => '右',
                '2' => '中',
                '3' => '左',
            ];

            $now_menu_data =[

            ];

            $menu11 = Me::where([
                'token' => session('token'),
                'mid' =>0
                ])->order( 'id desc' )
                ->select();

            foreach ($menu11 as $k) {
                $now_menu_data[$k['position']] = $k['position_name'];
            }

            //所有可供选择的位置-当前已使用的位置
            $diff_menu_data = array_diff($all_menu_data, $now_menu_data);
            $dat=[];
            foreach ($diff_menu_data as $k => $v) {
                $dat[$k]['position']    = $k;
                $dat[$k]['position_name'] = $v;
            }
            $this->assign('diff_menu_dat',$dat);
            // print_r($dat);
            // die;
            
            $diff_menu_data[$menu['position']]=$menu['position_name'];
            $data = [];
            foreach ($diff_menu_data as $k => $v) {
                $data[$k]['position']   = $k;
                $data[$k]['position_name'] = $v;
            }
            $this->assign('diff_menu_data',$data);
            // print_r($menu);
            // print_r($data);

            // die;
            // 所有一级菜单 - 目前查出所有的一级菜单 + 当前

            // 查出所有的一级菜单


            // 排除当前一级菜单

            // print_r($menu);
            return $this->fetch();
        }
        
    }

    // 查看二级自定义菜单详情
    public function menuDetail2()
    {
        if($this->request->isPost())
        {
            $data['name'] = $this->request->post('menuName');
            $data['type'] = $this->request->post('menuType');

            if($data['type']=='URL')
            {
                $data['value'] = $this->request->post('menuValue');
            }else{
                $data['value'] = $this->request->post('menuValueKey');
            }

            $data['position'] =$this->request->post('menuPosition');
                if($data['position'] == '1')
                    {
                        $data['position_name'] = '上5';
                    } else if($data['position'] == '2'){
                        $data['position_name'] = '上4';
                    }else if($data['position'] == '3'){
                        $data['position_name'] = '上3';
                    }else if($data['position'] == '4'){
                        $data['position_name'] = '上2';
                    }else{
                        $data['position_name'] = '上1';
                    }
            $data['token'] = session('token');
            $data['id'] = $this->request->post('id');
            $data['mid'] = $this->request->post('mid');

            // print_r($data);
            // die;
            // $res = db('menu')->update($data);
            $res = Me::update($data);
            if($res)
            {
                return $this->redirect('menu');
            } else {
                return $this->error('修改二级自定义菜单失败');
            }
        } else {

            $id = input('id'); 

            $menu = db('menu')->where([
                'id'=>$id,
                'token'=>session('token')
            ])->find();
            $this->assign('menu', $menu);

            $mid = $menu['mid'];

            // print_r($mid);
            // die;
            //所有可选择的位置数组
            $all_menu_data = [
                '1' => '上5',
                '2' => '上4',
                '3' => '上3',
                '4' => '上2',
                '5' => '上1',
            ];

            //现已使用的位置数组
            $now_menu_data =[

            ];

            //查出当前已使用的位置并放入现已使用的位置数组
            $menu11 = Me::where([
                'mid' => $mid,
                'token'=>session('token')
                ])->order( 'position desc' )
                ->select();
            foreach ($menu11 as $k) {
                $now_menu_data[$k['position']] = $k['position_name'];
            }
            // print_r($now_menu_data);

            // die;

            //所有可供选择的位置-当前已使用的位置
            $diff_menu_data = array_diff($all_menu_data, $now_menu_data);
            $dat=[];
            foreach ($diff_menu_data as $k => $v) {
                $dat[$k]['position']    = $k;
                $dat[$k]['position_name'] = $v;
            }
            $this->assign('diff_menu_dat',$dat);
            //  print_r($dat);

            // die;

            //最终要展示位置：所有可供选择的位置-当前已使用的位置+当前待编辑位置数组
            $diff_menu_data[$menu['position']]=$menu['position_name'];

            $data = [];
            foreach ($diff_menu_data as $k => $v) {
                $data[$k]['position']   = $k;
                $data[$k]['position_name'] = $v;
            }
            // $diff_menu_data=array_reverse($diff_menu_data); 
            $this->assign('diff_menu_data',$data);
            // print_r($data);

            // die;


            return $this->fetch();
        }
        
    }

    // 添加二级菜单
    public function menuAdd2()
    {

        if($this->request->isPost())
        {
            $data['name'] = $this->request->post('menuName');
            $data['type'] = $this->request->post('menuType');
            $data['value'] =$this->request->post('menuValue');
            $data['position'] =$this->request->post('menuPosition');
            if($data['position'] == '1')
                    {
                        $data['position_name'] = '上5';
                    } else if($data['position'] == '2'){
                        $data['position_name'] = '上4';
                    }else if($data['position'] == '3'){
                        $data['position_name'] = '上3';
                    }else if($data['position'] == '4'){
                        $data['position_name'] = '上2';
                    }else{
                        $data['position_name'] = '上1';
                    }
            $data['token'] = session('token');
            $data['create_time'] = time();
            $data['mid'] = $this->request->post('mid');
            // $res = db('menu')->insert($data);
            $res = Me::insert($data);
            if($res)
            {
                $this->redirect('menu');
            } else {
                $this->error('添加二级自定义菜单失败');
            }
        } else {
            $id = input('id');
            $menu = db( 'menu' )->where([
                'token'=>session('token'),
                'mid'=>$id
            ])->order( 'id desc' )
            ->select();

            $mun = count($menu);
            if($mun >= 5)
            {
                return $this->error('超出数量啦');
            }
            $this->assign('mid',$id);

            

            // print_r($menu);
            // die;
            // $mid = $menu['mid'];
            //所有可选择的位置数组
            $all_menu_data = [
                '1' => '上5',
                '2' => '上4',
                '3' => '上3',
                '4' => '上2',
                '5' => '上1',
            ];

            //现已使用的位置数组
            $now_menu_data =[

            ];

            //查出当前已使用的位置并放入现已使用的位置数组
            $menu11 = Me::where([
                'mid' => $id,
                'token'=>session('token')
                ])->order( 'id desc' )
                ->select();
            foreach ($menu11 as $k) {
                $now_menu_data[$k['position']] = $k['position_name'];
            }
            // print_r($now_menu_data);
            // die;

            //所有可供添加的位置-当前已使用的位置
            $diff_menu_data = array_diff($all_menu_data, $now_menu_data);
            // print_r($diff_menu_data);
            // die;

            $data = [];
            foreach ($diff_menu_data as $k => $v) {
                $data[$k]['position']   = $k;
                $data[$k]['position_name'] = $v;
            }
            $this->assign('diff_menu_data',$data);
            //  print_r($data);

            // die;

            return $this->fetch();
        }
    }

    // 删除自定义菜单
    public function menuDel()
    {
        $id[] = input('post.id/a');
        if(!empty($id[0])){
            for($i=0 ; $i<count($id[0]) ; $i++)
            {
                $data   = ["id" => $id[0][$i]];
                $dels[] = db('user')->delete( $data );
            }
            print_r($dels);
        } else {
            $data   = [ 'id'=>input('id')];
            $dels[] = db('menu')->delete( $data );
            if( $dels )
            {
                return $this->redirect( 'menu' );
                // $this->success('删除员工信息成功','employee');
            } else {
                return $this->error( '删除菜单失败,请稍后再试');
            }
        }
    }

    public function push()
    {
       header("Content-type: text/html; charset=utf-8"); 
	 $data['button'] = [];
        $menu = db('menu')->where([
            'token'=>session('token'),
            'mid'=>0
        ])->order('position desc')->select();
        foreach ($menu as $key => $value) 
        {
            $menuDate = db('menu')->where([
                'token'=>session('token'),
                'mid'=>$value['id']
            ])->order('position desc')->select();
            if($menuDate)
            {
                $dat = [];
                $dat['name'] = $value['name'];
                $dat['sub_button'] = [];
                $da =[];
                foreach ($menuDate as $k => $v) 
                {
                    $d = [];
                    $d['name'] = $v['name'];
                    $d['type'] = $v['type'] == "KEY" ? 'click' : 'view';
                    if($d['type'] == 'click')
                    {
                        $d['key'] = $v['value'];
                    } else {
                        $d['url'] = $v['value'];
                    }
                    $da[] = $d; 
                }
                $dat['sub_button'] = $da;
                $data['button'][] = $dat;
            } else {
                $dat = [];
                $dat['name'] = $value['name'];
                $dat['type'] = $value['type'] == "KEY" ? 'click' : 'view';
                if($dat['type'] == 'click')
                {
                    $dat['key'] = $value['value'];
                } else {
                    $dat['url'] = $value['value'];
                }
                $data['button'][] = $dat;
            }
        }
        // $menuJson = json_encode($data);
        $menuJson = JSON($data);
        // print_r($menuJson);
        //print_r($menuJson);die;
        //获取access_token
        // $url="https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=".trim(session('appid'))."&secret=".trim(session('appsecret'));
        // $content=file_get_contents($url);
        // $ret=json_decode($content,true);
        // print_r($ret);die;
        $member_arr = Member::where([
            'token' => session('token')
        ])->find();
        // $access_token = Bas::getAccessToken($member_arr);
        $access_token = Bas::getAccessToken();

        // 创建
        $url="https://api.weixin.qq.com/cgi-bin/menu/create?access_token=".$access_token;
        $content=Base::getContents($url,$menuJson);
        $ret=json_decode($content,true);
       //  print_r($ret);die;
        if($ret['errcode'] == 0){
            //创建成功
            // 提示成功
            $this->success( '创建自定义菜单成功' );
        }else{
            //创建失败
            $this->error( '创建自定义菜单失败，请核实数据再试！');
        }
    }


    public function getMaterial()
    {
        #请求数据 参数 地址
        $data = [
            'type'  => 'image',
            'offset'=> 0,
            'count' => 20,
        ];
        $data_json = JSON($data);
        

        $url="https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=".trim(session('appid'))."&secret=".trim(session('appsecret'));
        $content=file_get_contents($url);
        $ret=json_decode($content,true);

        $url="https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=".$ret['access_token'];
        
        #请求
        $content=get_contents($url,$data_json);
        $ret=json_decode($content,true);

        #处理结果

        print_r($ret);
    }

    public function addMaterial(){
        
        $data=[
            'articles'  => [
                "title" => '标题',
                "thumb_media_id" => '1',
                "show_cover_pic" => '1',
                "content" => '',
                "content_source_url" => 'https://www.baidu.com',

            ]
        ];


        $data_json = JSON($data);
        

        $url="https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=".trim(session('appid'))."&secret=".trim(session('appsecret'));
        $content=file_get_contents($url);
        $ret=json_decode($content,true);
        // print_r($ret);
        // $url="https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=".$ret['access_token'];

        $url="https://api.weixin.qq.com/cgi-bin/material/add_news?access_token=".$ret['access_token'];
        // #请求
        $content=Base::getContents($url,$data_json);
        $ret=json_decode($content,true);

        // #处理结果

        print_r($ret);
    }



}

