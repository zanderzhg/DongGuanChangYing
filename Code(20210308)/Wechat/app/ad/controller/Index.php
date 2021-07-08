<?php
namespace app\ad\controller;

class Index extends Base
{
    public function index()
    {
    	$this->redirect('log/login');
        // return $this->fetch();
    }
}
