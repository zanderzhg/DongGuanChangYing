<?php
namespace app\ad\controller;
use app\base\model\Record as Rec;
/*预约记录类*/
class Record extends Base
{
    /**
     *  查看"所有记录" 包括了访客预约和员工邀请
     *  
     *  @param check 查询关键词
     *  @param check_type 查询条件 UNDONE  查找未回复记录
     *  @param check_type 查询条件 DONE    查找已允许记录
     *  @param check_type 查询条件 NONE    查找已拒绝记录
     *  @param check_type 查询条件 VISNAME 根据访客姓名查找记录
     *  @param check_type 查询条件 VISPHO  根据访客手机查找记录
     *  @param check_type 查询条件 EMPNAME 根据员工姓名查找记录
     *  @param check_type 查询条件 EMPPHO  根据员工手机查找记录
     *  
     */
	public function allRecord()
	{
        // print_r($this->request->param());
        $check        = $this->request->param('check');
        $check_type   = $this->request->param('check_type');
        $map['id']    = [ '>', 0 ];
        $map['token'] = session('token');

        switch ($check_type) {
            case 'UNDONE':#查找未回复记录
                $map['status'] = '0'; 
                break;
            case 'DONE':#查找已允许记录
                $map['status'] = '1';
                break;
            case 'NONE':#查找已拒绝记录
                $map['status'] = '2';
                break;
            case 'VISNAME':#根据访客姓名查找记录
                $map['name'] = ['like', '%'.$check.'%'];
                break;
            case 'VISPHO':#根据访客手机查找记录
                $map['phone'] = ['like', '%'.$check.'%'];
                break;
            case 'EMPNAME':#根据员工姓名查找记录
                $map['ename'] = ['like', '%'.$check.'%'];
                break;
            case 'EMPPHO':#根据员工手机查找记录
                $map['ephone'] = ['like', '%'.$check.'%'];
                break;
            default:
                # code...
                break;
        }

		// 获取数据
        $allRecordres = Rec::where($map)
                            ->order('id desc')
                            ->paginate(20, false,[
                                'type'      => 'Bootstrap',
                                'var_page'  => 'page',
                                'query'     => ['check' => $check, 'check_type' => $check_type],
                            ]);
        // 把数据传递给模板文件
        $this->assign('allRecordres', $allRecordres);
        $this->assign('type', 'allRecord');
        $this->assign('check', $check);
        $this->assign('check_type', $check_type);
        // 渲染模板文件
    	return $this->fetch();
	}

    /**
     *  查看"访客预约记录" 包括了访客预约和员工邀请
     *  
     *  @param check 查询关键词
     *  @param check_type 查询条件 UNDONE  查找未回复记录
     *  @param check_type 查询条件 DONE    查找已允许记录
     *  @param check_type 查询条件 NONE    查找已拒绝记录
     *  @param check_type 查询条件 VISNAME 根据访客姓名查找记录
     *  @param check_type 查询条件 VISPHO  根据访客手机查找记录
     *  @param check_type 查询条件 EMPNAME 根据员工姓名查找记录
     *  @param check_type 查询条件 EMPPHO  根据员工手机查找记录
     *  
     */
	public function visitorRecord()
	{
        $check        = $this->request->param('check');
        $check_type   = $this->request->param('check_type');
        $map['id']    = [ '>', 0 ];
        $map['token'] = session('token');
        $map['visittype'] = 0;

        switch ($check_type) {
            case 'UNDONE':#查找未回复记录
                $map['status'] = '0'; 
                break;
            case 'DONE':#查找已允许记录
                $map['status'] = '1';
                break;
            case 'NONE':#查找已拒绝记录
                $map['status'] = '2';
                break;
            case 'VISNAME':#根据访客姓名查找记录
                $map['name'] = ['like', '%'.$check.'%'];
                break;
            case 'VISPHO':#根据访客手机查找记录
                $map['phone'] = ['like', '%'.$check.'%'];
                break;
            case 'EMPNAME':#根据员工姓名查找记录
                $map['ename'] = ['like', '%'.$check.'%'];
                break;
            case 'EMPPHO':#根据员工手机查找记录
                $map['ephone'] = ['like', '%'.$check.'%'];
                break;
            default:
                # code...
                break;
        }

		// 获取数据
        $allRecordres = Rec::where($map)
                            ->order('id desc')
                            ->paginate(20, false,[
                                'type'     => 'Bootstrap',
                                'var_page' => 'page',
                                'query'    => ['check' => $check,'check_type' => $check_type],
                            ]);
        // 把数据传递给模板文件
        $this->assign('allRecordres', $allRecordres);
        $this->assign('type', 'visitorRecord');
        $this->assign('check', $check);
        $this->assign('check_type', $check_type);
        // 渲染模板文件
    	return $this->fetch();
	}

    /**
     *  查看"员工邀请记录" 包括了访客预约和员工邀请
     *  
     *  @param check 查询关键词
     *  @param check_type 查询条件 VISNAME 根据访客姓名查找记录
     *  @param check_type 查询条件 VISPHO  根据访客手机查找记录
     *  @param check_type 查询条件 EMPNAME 根据员工姓名查找记录
     *  @param check_type 查询条件 EMPPHO  根据员工手机查找记录
     *  
     */
	public function employeeRecord()
	{
        $check        = $this->request->param('check');
        $check_type   = $this->request->param('check_type');
        $map['id']    = [ '>', 0 ];
        $map['token'] = session('token');
        $map['visittype'] = 1;

         switch ($check_type) {
            case 'VISNAME':#根据访客姓名查找记录
                $map['name'] = ['like', '%'.$check.'%'];
                break;
            case 'VISPHO':#根据访客手机查找记录
                $map['phone'] = ['like', '%'.$check.'%'];
                break;
            case 'EMPNAME':#根据员工姓名查找记录
                $map['ename'] = ['like', '%'.$check.'%'];
                break;
            case 'EMPPHO':#根据员工手机查找记录
                $map['ephone'] = ['like', '%'.$check.'%'];
                break;
            default:
                # code...
                break;
        }


		// 获取数据
        $allRecordres = Rec::where($map)
                            ->order('id desc')
                            ->paginate(20, false,[
                                'type'     => 'Bootstrap',
                                'var_page' => 'page',
                                'query'    => ['check' => $check,'check_type' => $check_type],
                            ]);

        // 把数据传递给模板文件
        $this->assign('allRecordres', $allRecordres);
        $this->assign('type', 'employeeRecord');
        $this->assign('check', $check);
        $this->assign('check_type', $check_type);
        // 渲染模板文件
    	return $this->fetch();
	}

    /**
     *  删除预约记录 包含批量和单独
     *  @param id 需要删除记录的id
     *  
     */
    public function recordDel()
    {
        // 这里缺少对传递的数据进行验证
        $id[] = input('post.id/a');
        if(!empty($id[0]))
        {
            // 批量删除
            for($i=0; $i<count($id[0]); $i++)
            {
                $data   = ["id" => $id[0][$i]];
                $dels[] = Rec::destroy($data);
            }
            print_r($dels);
        } else {
            // 单独删除
            $data   = ['id' => input('id')];
            $dels[] = Rec::destroy($data);
            if( $dels )
            {
                $this->success('删除预约记录成功');
            } else {
                $this->error('删除预约记录失败');
            }
        }
    }

	/*导出选定记录*/
	public function recordExport()
	{
        header("Content-type:text/html;charset=utf-8");
        
        $id[] = input('post.id/a');
        // print_r($id);die;
        for($i=0 ; $i<count($id[0]); $i++)
        {
            $epl[] = Rec::where('id',$id[0][$i])->find();
        }
        foreach ($epl as $k => $v) {
            $epl[$k]['start_time'] = date('Y-m-d H-i', $v['start_time']);
            $epl[$k]['end_time']   = date('Y-m-d H-i', $v['end_time']);
            $epl[$k]['visittype']  = $v['visittype'] == 0 ? '访客预约' : '员工邀请';
        }
        echo $this->excelExport($epl, 'records');
    }

    /*导出所有预约记录*/
	public function recordExportAll()
	{
    
        
        $map['id'] = ['>' , 1];
        $map['token'] = session( 'token');
        $epls[] = Rec::where($map)->select();
        $epl = $epls[0];
        foreach ($epl as $k => $v) {
            $epl[$k]['start_time'] = date('Y-m-d H-i', $v['start_time']);
            $epl[$k]['end_time'] = date('Y-m-d H-i', $v['end_time']);
            $epl[$k]['visittype'] = $v['visittype'] == 0 ? '访客预约' : '员工邀请';
        }
        echo $this->excelExport($epl, 'records');
    }

    /**
     *  邀请记录的详细信息
     *  
     *  @param id 记录id
     */
    public function recordDetail()
    {
        $id     = $this->request->param('id');
        $record = Rec::where('id',$id)->find();
        $this->assign('list',$record);
        return $this->fetch();
    }
}
