<?php 
// 数据库连接文件
class Db
{
	static private $_instance;
	static private $_connectSource;
	private $_dbConfig = array(
		'host'=>'127.0.0.1',
		'user'=>'root',
		'password'=>'Xvfk@2019..',
		'database'=>'cy',
		);

	private function __construct()
	{

	}
	
	static public function getInstance()
	{
		if (!(self::$_instance instanceof self))
		{
			self::$_instance = new self();
		}

		return self::$_instance;
	}

	public function connect()
	{
		if(!self::$_connectSource)
		{
			self::$_connectSource = mysqli_connect($this->_dbConfig['host'],$this->_dbConfig['user'],$this->_dbConfig['password'],$this->_dbConfig['database']);
			if(!self::$_connectSource) 
			{
				die('mysql connect error'.mysql_error());
			}
			self::$_connectSource->query("set names UTF8");
		}
		
		return self::$_connectSource;
	}
}
?>
