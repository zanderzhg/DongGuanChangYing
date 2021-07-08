<?php
namespace app\base\model;

use think\Model;
use traits\model\SoftDelete;

class Rou extends Model
{
	use SoftDelete;
	protected $deleteTime = 'delete_time';
}