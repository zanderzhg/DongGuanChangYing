<?php
namespace app\wechat\controller;

use app\base\model\Record;
use app\base\model\Recordlog;

class Text extends Base
{
    public function done()
    {
        $this->assign('message1', $this->request->param('message1'));
        $this->assign('message2', $this->request->param('message2'));
        return $this->fetch('don');
    }
    
    // 员工邀请时显示页面
    public function reservation($id)
    {
        #设置页面编码
        header("Content-Type:text/html;charset=utf-8");
        #查找数据
        $record    = Record::where('id',$id)->find();
        $recordlog = Recordlog::where('id',$id)->find();
        #把数据转换为数组
        $record    = $record ? $record->toArray() : $record;
        $recordlog = $recordlog ? $recordlog->toArray() : $recordlog;
        #传递数据到视图层
        $this->assign('record', $record);
        $this->assign('recordlog', $recordlog);
        // print_r($record);
        #渲染页面
        if($record['visittype']==3)
        {
            #属于会议邀请分类
            return $this->fetch('conference');
        } elseif($record['visittype']==1) {
            #属于员工邀请分类
            return $this->fetch('invite');
        } else {
            #属于访客预约分类
            return $this->fetch();
        }
    }

    public function conferencedetail($message='')
    {
        print_r($message);
        print_r($this->request->param());
    }

    public function err()
    {
        return $this->fetch();
    }

}
