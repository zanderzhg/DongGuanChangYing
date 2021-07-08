<?php
namespace app\ad\controller;
use app\base\model\User as Us;
use app\base\model\Department as Dep;
class User extends Base
{
    public function index()
    {
        return $this->fetch();
    }

    /**
     * 访客管理-显示访客信息
     * 可以根据查询条件筛选访客信息（访客姓名，手机号码，身份证号码）
     * 
     * @param check 检索值
     * @param check_type 检索条件
     * 
     */
    public function visitor()
    {
        #接收查询参数
        $check        = $this->request->param('check');
        $check_type   = $this->request->param('check_type');
        
        #拼接查询条件
        $map['type']  = 0; #人员类型为访客
        $map['token'] = session('token');

        switch ($check_type) {
            case 'ALL':
                break;
            case 'NAME':
                $map['name']   = ['like', '%'.$check.'%'];
                break;
            case 'PHONE':
                $map['phone']  = ['like', '%'.$check.'%'];
                break;
            case 'IDCARD':
                $map['idcard'] = ['like', '%'.$check.'%'];
                break;
            default:
                # code...
                break;
        }

        #分页查询数据
        $visitor = Us::where($map)
            ->order('id desc')
            ->paginate(20, false,[
                'type'      => 'Bootstrap',
                'var_page'  => 'page',
                'query'     => [
                    'check'      => $check, 
                    'check_type' => $check_type
                ],
            ]);

        #把数据传递给模板文件
        $this->assign('visitor', $visitor);
        $this->assign('check', $check);
        $this->assign('check_type', $check_type);
        // 渲染模板文件
        return $this->fetch();
    }


    /**
     *  员工管理-显示访客信息
     *  可以根据查询条件筛选员工信息（员工姓名，手机号码，所在部门）
     *  
     * @param check 检索值
     * @param check_type 检索条件 
     *  
     */
    public function employee()
    {
        #获取请求数据
        $check        = $this->request->param('check');
        $check_type   = $this->request->param('check_type');
        
        #拼接查询条件
        $map['type'] = 1;       #人员类型为员工
        $map['token'] = session('token');

        #查询员工绑定情况
        $countAll = count(Us::where($map)
                ->order('id desc')
                ->select());
        $countLink = count(Us::where($map)
                ->where('status', 1)
                ->order('id desc')
                ->select());
        $countUnlink = count(Us::where($map)
                ->where('status', 2)
                ->order('id desc')
                ->select());
        $count = [
            'countAll'    => $countAll,
            'countLink'   => $countLink,
            'countUnlink' => $countUnlink,
        ];

        switch ($check_type) {
            case 'ALL':
                break;
            case 'NAME':
                $map['name']  = ['like', '%'.$check.'%'];
                break;
            case 'PHONE':
                $map['phone'] = ['like', '%'.$check.'%'];
                break;
            case 'DEPARTMENT':
                $map['department'] = ['like', '%'.$check.'%'];
                break;
            default:
                # code...
                break;
        }

        $employee = Us::where($map)
            ->order('id desc')
            ->paginate(20, false,[
                'type'      => 'Bootstrap',
                'var_page'  => 'page',
                'query'     => [
                    'check'      => $check, 
                    'check_type' => $check_type
                ],
            ]);

        // 把数据传递给模板文件
        $this->assign('count', $count);
        $this->assign('check', $check);
        $this->assign('employee',$employee);
        $this->assign('check_type', $check_type);
        // 渲染模板文件
        return $this->fetch();
    }


    /**
     * 删除访客信息-包含批量删除和单独删除
     *  
     * @param id 需要删除数据的集合 可能是int，也可以能是data数组 
     *  
     */
    public function visitorDel()
    {
        // 这里缺少对传递的数据进行验证
        $id[] = input('post.id/a');

        if(!empty($id[0])){#批量删除
            for($i=0; $i<count($id[0]); $i++)
            {
                $data   = ["id" => $id[0][$i]];
                $dels[] = Us::destroy($data);
            }
            print_r($dels);
        } else {#单独删除
            $data   = ['id'=>input('id')];
            $dels[] = Us::destroy($data);
            
            if($dels){
                return $this->success('删除访客信息成功','visitor');
            } else {
                return $this->error( '删除访客信息失败,请稍后再试');
            }
        }
    }


    /**
     * 导出所有访客信息 
     *  
     */
    public function visitorExportAll()
    {
        header("Content-Type:text/html;charset=UTF-8");

        #拼接导出条件
        $map['type']   = 0;
        $map['token']  = session( 'token' );
        $map['status'] = ['>' , -1];

        $epl = Us::where($map)
                ->order('id desc')
                ->select();

        echo $this->excelExport($epl, 'visitor');
    }

    /**
     * 导出选定访客信息 
     *  
     *  @param id 需要删除数据的集合
     */
    public function visitorExport()
    {
        $id[] = input('post.id/a');
        for($i=0; $i<count($id[0]); $i++)
        {
            $epl[] = Us::where('id', $id[0][$i])->find();
        }

        echo $this->excelExport($epl, 'visitor');
    }

    /**
     * 增加员工信息
     * 
     * @param name 员工姓名
     * @param phone 员工手机号码
     * @param idcard 员工身份证号码
     * @param department 员工所在部门
     * 
     */
    public function employeeAdd()
    {
        header("Content-type:text/html;charset=UTF-8");
        
        if($this->request->isPost()){
            $token   = session( 'token' );
            $status  = 2;
            $company = session('company') == '' ? ' ' : session('company');
            $type    = 1;
            $create_time = time();
            $data = [
                'department' => $this->request->post('department'),
                'create_time'=> $create_time,
                'name'   => $this->request->post('name'),
                'code'   => $this->request->post('code'),
                'phone'  => $this->request->post('phone'),
                'idcard' => $this->request->post('idcard'),
                'type'   => $type,
                'status' => $status,
                // 'address'=> $this->request->post('address'),
                'company'=> $company,
                'token'  => session('token'),
            ];

            // 判断该公众号下，该手机号码是否已经存在
            $res = Us::where([
                    'phone' => $data['phone'],
                    'token' => $data['token']
                ])->find();

            if($res){#员工手机号码已经存在系统里面
                $this->error('该用户手机号码已经存在，请核实信息后再试！');
            }

            $db = Us::insert($data);

            if($db) {
                $this->success('添加员工信息成功', 'employee');
            } else {
                $this->error('添加员工信息失败，请稍后再试！');
            }

        } else {
            #查询该公司部门结构
            $department = Dep::where([
                    'token' => session('token')
                ])->select();

            $this->assign('department', $department);
            $this->assign('address', session('address'));

            return $this->fetch();
        }
    }

    /**
     * 编辑员工信息
     * 
     * @param id 员工id
     * @param name 员工姓名
     * @param phone 员工手机号码
     * @param idcard 员工身份证号码
     * @param department 员工所在部门
     * 
     */
    public function employeeDetail()
    {
        header("Content-type:text/html;charset=utf-8");

        if($this->request->isPost()){
            $data = [
                'id'         => $this->request->post('id'),
                'name'       => $this->request->post('name'),
                'code'       => $this->request->post('code'),
                'phone'      => $this->request->post('phone'),
                'idcard'     => $this->request->post('idcard'),
                'department' => $this->request->post('department'),
            ];

            // 判断该公众号下，该手机号码是否已经存在
            $res = Us::where([ 
                    'phone' => $data['phone'], 
                    'token' => session('token')
                ])->find();

            if($res){
                $res = $res->toArray();
                if($res['id'] != $data['id']){
                    return $this->error( '该用户手机号码已经存在，请核实信息后再试！');
                }
            }

            if($data['idcard']){
                // 判断该公众号下，该身份证号码是否已经存在
                $res = Us::where([ 
                        'idcard' => $data['idcard'], 
                        'token'  => session('token') 
                    ])->find();
                
                if($res){
                    $res = $res->toArray();
                    if($res['id'] != $data['id']){
                        return $this->error( '该用户身份证号码已经存在，请核实信息后再试！');
                    }
                }  
            }
            
            $db = Us::update($data);
            
            if($db) {
                $this->success('信息修改成功', 'employee');
            }else if($db!==false) {
                $this->success('信息无修改', 'employee');
            } else {
                return $this->error('修改员工信息失败，请稍后再试！');
            }

        } else {

            $employee   = Us::where('id', $this->request->param('id'))->find();
            $department = Dep::where(['token' => session('token')])->select();
           
            $this->assign('department', $department);
            $this->assign('employee',$employee);
           
            return $this->fetch();
        }      
    }

    /**
     * 编辑访客信息
     * 
     * @param id 员工id
     */
    public function visitorDetail()
    {
        header("Content-type:text/html;charset=utf-8");
       
        $visitors = Us::where('id', $this->request->param('id'))->find();
        $this->assign('visitors',$visitors);
        return $this->fetch();    
    }

    /**
     * 删除员工信息
     * @param id
     *  
     */
    public function employeeDel()
    {
        // 这里缺少对传递的数据进行验证
        $id[] = input('post.id/a');

        if(!empty($id[0])){
            for($i=0 ; $i<count($id[0]) ; $i++)
            {
                $data   = ["id" => $id[0][$i]];
                $dels[] = Us::destroy($data);
            }
            print_r($dels);
        } else {
            $data   = ['id'=>input('id')];
            $dels[] = Us::destroy($data);
            
            if( $dels ){
                $this->success('删除员工信息成功','employee');
            } else {
                return $this->error('删除员工信息失败,请稍后再试');
            }

        }       
    }

    /**
     * 导出所有员工信息 
     *  
     */
    public function employeeExportAll()
    {
        #拼接查询条件
        $map['type']   = 1;
        $map['token']  = session('token');
        $map['status'] = ['>' , -1];

        $epl = Us::where($map)->order('id desc')->select();
        echo $this->excelExport($epl, 'employee');
    }

    public function employeeout()
    {
        #拼接查询条件
        $map['type']   = 10000;
        $map['token']  = session('token');
        $map['status'] = ['>' , -1];

        $epl = Us::where($map)->order('id desc')->select();
        echo $this->excelExport($epl, 'employee');

    }


    /**
     * 导出所有员工信息 
     *  
     * @param id array数组
     */
    public function employeeExport()
    {
        // 导出文件名
        $fileName = "employeeAll".date("Ymd-His",time()).".xls";

        // 获取要导出的数据
        $id[] = input('post.id/a');
        for($i=0 ; $i<count($id[0]) ; $i++)
        {
            $epl[] = Us::where('id',$id[0][$i])->find();
        }
        
        echo $this->excelExport($epl, 'employee');

    }

    /*接收批量录入员工信息数据*/
    public function employeeImport()
    {
        header( "Content-Type:text/html;charset=utf-8" );

        // print_r($_FILES);die;
        // 获取表单上传文件
        $file = request()->file('excelData');
        // var_dump($_FILES['excelData']["name"]);die;
        $filetype = pathinfo($_FILES['excelData']["name"], PATHINFO_EXTENSION);
        // print_r($filetype);
        // die;
        // 移动到框架应用目录/public/uploads/目录下
        $info = $file->move(ROOT_PATH . 'public' . DS . 'upload');
        // var_dump($info);die;
        // echo "ddd";
        // 数据为空返回错误
        if(empty($info))
        {
            // 返回错误信息
            return $this->error('导入数据失败，请稍后再试！');
        } else {
            // 获取文件名
            $fileName = $info->getSaveName();
            // 获取带参数文件名
            $excelPath = ROOT_PATH . 'public' . DS . 'upload' . DS .$fileName; 
            // 获取文件扩展名
            $extension = strtolower(pathinfo($fileName, PATHINFO_EXTENSION));
            // 判断文件上传是否成功
            if($info)
            {
                // 信息上传成功，导入访客信息
                $this->employeeImports($excelPath, $extension);
            }
        }
    }

    /*批量录入员工信息·整理数据信息*/
    public function employeeImports($fileName ,$exts = 'xls')
    {
        // 导入PHPExcel
        \think\Loader::import('Excel.PHPExcel');
        // 创建一个处理对象实例
        $objExcel = new \PHPExcel();  
        // 根据后缀名导入对应的类库文件
        if($exts == 'xls')
        {
            \think\Loader::import('Excel.PHPExcel.Reader.Excel5');
            $PHPReader = new \PHPExcel_Reader_Excel5();
        } else if($exts == 'xlsx') {
            \think\Loader::import('Excel.PHPExcel.Reader.Excel2007');
            $PHPReader = new \PHPExcel_Reader_Excel2007();
        }
        // 载入文件
        $PHPExcel = $PHPReader->load($fileName);
        // print_r($PHPExcel);
        // 获取表中的第一个工作表
        $currentSheet = $PHPExcel->getSheet(0);
        // 获取总列数
        $allColum = $currentSheet->getHighestColumn();
        // 获取总行数
        $allRow   = $currentSheet->getHighestRow();
        // print_r($allColum);
        // echo "<br>";
        // print_r($allRow);
        // 循获取表中的数据，$currentRow表示当前行， 从哪行开始读取数据 ，索引值从0开始
        for( $currentRow=2; $currentRow<=$allRow; $currentRow++ )
        {
	
            // 判断是否到了数据最后一行，以行首元素是否为空为标准，如果为空，则跳出循环
           if( ( trim( $currentSheet->getCell( 'A'.$currentRow )->getValue() ) ) == '' ) break;
            // 从那列开始，A表示第一列
            for( $currentColumn='A'; $currentColumn<=$allColum; $currentColumn++ )
            {
                // 数据坐标
                $address = $currentColumn.$currentRow;
                // 读取到的数据，保存到数组$data中
                $data[$currentRow][$currentColumn] = trim($currentSheet->getCell($address)->getValue());
            }
                // // 获得部门数组
                $departmentArr[$currentRow] = trim($currentSheet->getCell('D'.$currentRow)->getValue());
                // // 获得职务数值
                // $dutiesArr[$currentRow] = trim($currentSheet->getCell('F'.$currentRow)->getValue());
        }
                // print_r($data);

        // 数组去重
        $departmentArr = array_flip(array_flip($departmentArr));
        // 删除缓存文件
        // unlink($fileName);
        // $departmentArr = array();
        $dutiesArr     = array();
        $this->save_import_employee( $data, $departmentArr, $dutiesArr );
        // print_r($data);
    }

    /*保存员工导入数据*/
    public function save_import_employee( $data, $departmentArr=array(), $dutiesArr=array() )
    {
        // 获取当前公司id
        $token   = session('token');
        $address = session('address');
        $company = session('company');
        $status  = 2; // 2为还没有绑定（解绑）
        $type    = 1; // 1为员工
        $create_time = time();

        // 目前没有做部门和职位的处理

        // 处理没有部门的记录
            $departmentData = array();
            // 获得数据库当前部门数据集合
            $hasDepartment = Dep::where( ['token'=>session('token')] )->column('name');
            // 获得当前公司没有的部门数据集合
            // print_r($hasDepartment);die;
            $departmentAdd = array_diff($departmentArr, $hasDepartment);
            // print_r($departmentAdd);die;
            // 如果差集不为空，将差集首先插入部门表
            if(!empty($departmentAdd))
            {
                foreach ($departmentAdd as $k => $v)
                {
                    $departmentAddArr[] = ['name'=> $v, 'token'=>session('token'), 'create_time'=>time() ];
                }
                $res = Dep::insertAll($departmentAddArr);
            }

            $addData = array();
            $error = '';
            foreach ($data as $k => $v) {
                // 这里缺少对数据的验证
                // 
                $addData[] = array(
                    'name'   => $data[$k]['A'],
                    'phone'  => $data[$k]['B'],
                    'address'=> $data[$k]['C'],
                    'idcard' => $data[$k]['E'],                 
                    'token'  => $token,
                    'type'   => $type,
                    'status' => $status,
                    'company'=> $company,
                    'department'  => $data[$k]['D'],
                    'code'  => $data[$k]['F'],
                    'create_time' => $create_time,
                    );
                // $addData['address'] = $addData['address'] == '' ? $addData['address'] : $address;
            }
        
        // $arr = db('user')->where('token', $token)->column('idcard','phone');

        // 这里缺少对重复数据的验证操作
        // 插入数据
        // 
            
            $res = array_values($addData);
            // 批量插入数据
            $result = Us::insertAll($res);
            // 判断插入数据是否成功
            if( $result )
            {
                // 插入数据成功
                return $this->redirect('employee');
                // return $this->success('访客信息导入成功');
            } else {
                // 插入数据失败
                return $this->error('员工信息格式不对，导入失败');
            }
    }
}
