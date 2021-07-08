/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.

	常量定义

********************************************************************/
/*	@file
 *	@brief	常量定义
 */

#pragma once

//(日志/错误)信息级别
#define _FG_LOG_LEVEL_DEBUG		0		//调试信息(预防错误、有助于优化性能）
#define _FG_LOG_LEVEL_INFO		1		//性能数据
#define _FG_LOG_LEVEL_RUN		2		//基本信息(系统运行扼要状态跟踪)
#define _FG_LOG_LEVEL_ERROR		0x10	//程序错误(功能无法完成，但对系统运行无影响)
#define _FG_LOG_LEVEL_STOP		0x20	//程序错误高级(这种情况下可能是连带错误，系统已无运行必要)
#define _FG_LOG_LEVEL_MAX		255		//最大级别

//文件格式
#define _FG_FILE_BMP			6		//Windows BMP
#define _FG_FILE_JPEG			10		//JPEG File Interchange Format
#define _FG_FILE_TIF_JPEG		11		//Jpeg Tag Image File Format
#define _FG_FILE_JPEG_411		21		//JPEG  4:1:1
#define _FG_FILE_TIF_JPEG_411	22		//JPEG  4:1:1
#define _FG_FILE_JPEG_422		23		//JPEG  4:2:2
#define _FG_FILE_TIF_JPEG_422	24		//JPEG  4:2:2
#define _FG_FILE_PNG			75
#define _FG_FILE_EXIF_JPEG		98		//JPEG 4:2:2 compressed Exif file
#define _FG_FILE_EXIF_JPEG_411	101		//JPEG 4:1:1 compressed Exif file


//图像颜色通道
#define _FG_CHANNEL_MASTER		0x0000	//All channels 
#define _FG_CHANNEL_RED			0x0001	//Red channel only
#define _FG_CHANNEL_GREEN		0x0002	//Green channel only
#define _FG_CHANNEL_BLUE		0x0003	//Blue channel only

//图像旋转模式
#define _FG_ROTATE_RESIZE       0x0001
#define _FG_ROTATE_RESAMPLE		0x0002
#define _FG_ROTATE_BICUBIC		0x0004
#define _FG_ROTATE_OFFCENTER	0x0008

//图像尺寸模式
#define _FG_SIZE_NORMAL			0x0000
#define _FG_SIZE_RESAMPLE		0x0002
#define _FG_SIZE_BICUBIC		0x0004
#define _FG_SIZE_ADBICUBIC		0x0008
#define _FG_SIZE_STRETCH		0x0000
#define _FG_SIZE_KEEPASPECT		0x1000

//形状合并运算模式定义
#define _FG_RGN_AND             0
#define _FG_RGN_SET             1
#define _FG_RGN_ANDNOTBITMAP    2
#define _FG_RGN_ANDNOTRGN       3
#define _FG_RGN_OR              4
#define _FG_RGN_XOR             5
#define _FG_RGN_SETNOT          6

#define _FG_COLORSEP_RGB		0x00 /* Use 3 RGB color planes. */
#define _FG_COLORSEP_CMYK       0x01 /* Use 4 CMYK color planes. */
#define _FG_COLORSEP_HSV		0x02 /* Use 3 HSV color planes. */
#define _FG_COLORSEP_HLS		0x03 /* Use 3 HLS color planes. */
#define _FG_COLORSEP_CMY		0x04 /* Use 3 CMY color planes. */

//读卡器定义
#define IDCARD_READER_NONE			-1	//
#define IDCARD_READER_SYNTHSIS		1	//神思读卡器100
#define IDCARD_READER_SHENDUN		2	//一所神盾读卡器

//证件类型定义
#define IDCARD_TYPE_ID_Ix			0	//第一代居民身份证
#define IDCARD_TYPE_ID_II			1	//第二代居民身份证
#define IDCARD_TYPE_ID_I			2	//第一代居民身份证
#define IDCARD_TYPE_ID_II_MANAUL	3	//手工输入的二代证
#define IDCARD_TYPE_ID_MILITARY		4	//军官证
#define IDCARD_TYPE_ID_POLICE		5	//警察证
#define IDCARD_TYPE_ID_DRIVER		6	//驾驶证
#define IDCARD_TYPE_ID_OTHER		255	//其他证件

//记录标志定义
#define RECORD_TYPE_UPLOADING		-2	//记录正在上传中
#define RECORD_TYPE_ALL				-1	//所有状态
#define RECORD_TYPE_UNUPLOAD		0	//记录未上传
#define RECORD_TYPE_UPLOADED		1	//记录已上传

//图像窗口显示模式
#define _FG_ZOOM_MODE_PERCENT		1	//按当前放大比例显示
#define _FG_ZOOM_MODE_FITWND		2	//适合窗口
#define _FG_ZOOM_MODE_PERC100		3	//1：1显示

//////////////////////////////////////////////////////////////////////

//数据类型定义

#define _FG_FACETYPE_FRONTAL		0	//正面人脸
#define _FG_FACETYPE_LEFTPROFILE	1	//左侧面
#define _FG_FACETYPE_RIGHTPROFILE	2	//右侧面
#define _FG_FACETYPE_OTHER			3	//其它

#define _FG_CPD_EYESDISTANCE		1	//两眼距离
#define _FG_CPD_FACEWIDTH			2	//脸宽
#define _FG_CPD_HEADWIDTH			3	//头宽
#define _FG_CPD_EYESY				6	//眼睛Y
#define _FG_CPD_HEADTOP				7	//头顶Y
#define _FG_CPD_HEADHEIGHT			8	//头高

#define _FG_FACEPOINT_IDS_NON			-1
#define _FG_FACEPOINT_IDS_EYE			1
#define _FG_FACEPOINT_IDS_FIRSTEYE		2
#define _FG_FACEPOINT_IDS_SECONDEYE		3
#define _FG_FACEPOINT_IDS_FACELEFT		6
#define _FG_FACEPOINT_IDS_FACERIGHT		7
#define _FG_FACEPOINT_IDS_HEADLEFT		8
#define _FG_FACEPOINT_IDS_HEADRIGHT		9
#define _FG_FACEPOINT_IDS_NOSETIP		10
#define _FG_FACEPOINT_IDS_BACKOFHEAD	11
#define _FG_FACEPOINT_IDS_CHIN			16
#define _FG_FACEPOINT_IDS_HEADTOP		17
#define _FG_FACEPOINT_IDS_END			255


//人脸关键点标定信息
struct _FGFacePoint
{
	int faceType;						// 人脸类型 0:正面 1:左侧面 2:右侧面
	union
	{
		struct struct_frontal			// 正面人脸
		{
			int xleft;					// 左眼在原始图像中的位置
			int yleft;
			int xright;					// 右眼在原始图像中的位置
			int yright;
			int faceLeft;				// 人脸左边缘的X坐标，-1表示未知
			int faceRight;				// 人脸右边缘的X坐标，-1表示未知
		} frontal;
		struct struct_profile			// 侧面人脸
		{
			int eyeX;					// 眼睛的X坐标
			int eyeY;					// 眼睛的Y坐标
			int noseTipX;				// 鼻尖的X坐标
			int noseTipY;				// 鼻尖的Y坐标，-1表示未知
			int headBackX;				// 后脑勺的X坐标
			int headBackY;				// 后脑勺的Y坐标，-1表示未知
		} profile;
	} facePos;
	int headLeft;						// 人头左边缘的X坐标，-1表示未知
	int headRight;						// 人头右边缘的X坐标，-1表示未知
	int headTop;						// 头顶的Y坐标，-1表示未知
	int chinPos;						// 下巴的Y坐标，-1表示未知
	double skewAngle;					// 平面内偏斜角度 -181表示未知
};

//人像剪裁结构定义
struct _FGCropProfileData
{
	char	szName[31];					//剪裁方案名称
	int		nFaceType;					//正面、侧面
	double	fWidth;
	double	fHeight;
	int		unitSize;					//尺寸单位
	bool	bKeepRatio;					//允许等比例缩放
	double	fDPI;
	int		unitDPI;
	double	fEyesDisMin;				//两眼距离
	double	fEyesDisMax;
	double	fFaceWidthMin;				//脸宽（耳朵内侧）
	double	fFaceWidthMax;
	double	fHeadWidthMin;				//头宽（耳朵外侧）
	double	fHeadWidthMax;
	double	fEyesYMin;					//眼睛Y位置
	double	fEyesYMax;
	double	fHeadHeightMin;				//头高（头顶到下巴）
	double	fHeadHeightMax;
	double	fHeadTopYMin;				//头顶
	double	fHeadTopYMax;
};

// 图像检测特征数据结构
struct _FGDetectStdData
{
	int structSize;
	char name[48];
	char version[16];
	char date[24];
	char reserved[8];					// reserved

	int ijpg;							// file format
	int sz1, sz2;						// file size
	int bits1, bits2;					// image bits
	int dpi;							// DPI
	int igr1, igr2;						// imgGrayRange
	float iag1, iag2;					// imgAverageGrayscale
	float ff1, ff2;						// faceFrontal
	float fe1, fe2;						// faceExposure
	float fu1, fu2;						// faceUniform
	float fx1, fy1, fv1;				// fg a, b, l
	float fx2, fy2, fv2;
	float fh1, fh2;						// faceHotspots
	float fb1, fb2;						// faceBlur
	float eo1, eo2;						// eyesOpen
	float eg1, eg2;						// eyesGlasses
	float ef1, ef2;						// eyesFrontal
	float fo1, fo2;						// faceOffset中心偏移
	float ed1, ed2;						// eyesDistance

	int bktype;							// 0: Monochromel; 1: Complex;
	float bu1, bu2;						// bgUniform
	float bx1, by1, bv1;				// bk a, b, l
	float bx2, by2, bv2;

	int	nFaceType;						//正面、侧面、其它
	int	bKeepRatio;						//允许等比例缩放
	int	fWidth;							// width, height
	int	fHeight;
	int	fEyesDisMin;					//两眼距离
	int	fEyesDisMax;
	int	fFaceWidthMin;					//脸宽（耳朵内侧）
	int	fFaceWidthMax;
	int	fHeadWidthMin;					//头宽（耳朵外侧）
	int	fHeadWidthMax;
	int	fEyesYMin;						//眼睛Y位置
	int	fEyesYMax;
	int	fHeadHeightMin;					//头高（头顶到下巴）
	int	fHeadHeightMax;
	int	fHeadTopYMin;					//头顶
	int	fHeadTopYMax;

	// 严重不合格范围(颜色)
	float dfx1, dfy1, dfv1;
	float dfx2, dfy2, dfv2;
	float dbx1, dby1, dbv1;
	float dbx2, dby2, dbv2;

	float lowcs1, lowcs2;				//灰蒙
	float ffroll1, ffroll2;				//脸旋转
	float ffyaw1, ffyaw2;				//脸侧转
	float fhpitch1, fhpitch2;			//头俯昂
	float gf1, gf2;						//眼镜架框
	float gc1, gc2;						//眼镜片颜色深浅
	float gflash1, gflash2;				//镜片反光
	float ec1, ec2;						//眼睛被遮挡
	float ered1, ered2;					//红眼
	float fexp1, fexp2;					//表情自然
	float mc1, mc2;						//嘴巴闭合
	float noise1, noise2;				//图像噪声
	float psss1, psss2;					//PS:磨皮
	float psos1, psos2;					//PS:过度锐化
};

//图像检测结果数值末
struct _FGDetectResult
{
	// 文件属性
	int		fileFormat;					// 文件格式
	int		fileSize;					// 文件大小

	// 图像属性
	int		imgWidth, imgHeight;		// 图像宽度、高度（像素）
	int		imgBits;					// 图像色彩位数
	int		imgDpi;						// 输出DPI（像素/英寸） 
	int		imgCorrupt;					// 图像损坏类型
	int		imgGrayRange;				// 灰度动态范围 [0,255]
	float	imgAverageGrayscale;		// 平均亮度（灰度）[0,255]
	float	imgLowContrast;				// 整体灰蒙[0,1]
	float	imgNoise;					// 图像噪声

	// 照片属性
	int		faceCount;					// 照片人脸数
	int		headWidth, headHeight;		// 头宽、头高
	int		faceWidth;					// 脸宽
	int		headTop;					// 头顶
	int     eyeTop;                     // 眼睛距离上边沿
	float	faceOffsetX, faceOffsetY;	// 居中偏移量，水平垂直（百分比）
	                                    // (-) 偏左，偏上 (+) 偏右，偏下
	float	faceFrontal;				// 正脸
	float	faceRoll;					// 脸旋转角度
	float	faceYaw;					// 脸侧转角度
	float	headPitch;					// 头俯昂角度
	float	faceExposure;				// 脸部曝光
	float	faceUniform;				// 脸部光线均匀性
	float	faceColorL, faceColorA, faceColorB;	// 脸部平均色彩
	float	faceHotspots;				// 脸部高光
	float	faceBlur;					// 脸部模糊度
	int		eyesDistance;				// 两眼距离
	float	eyesOpen;					// 眼睛开闭
	float	eyesGlasses;				// 是否戴眼镜
	float	eyesFrontal;				// 眼睛正视前方
	float	eyesColorL, eyesColorA, eyesColorB;	// 眼睛色彩
	float	glassFrame;					// 眼镜架框
	float	glassCover;					// 眼镜片颜色深浅
	float	glassFlash;					// 眼镜反光
	float	eyeCovered;					// 眼睛被遮挡
	float	eyesRed;					// 红眼
	float	faceExpression;				// 表情自然
	float	mouthClose;					// 嘴巴闭合

	// 背景属性
	float	bgPureColor;				// 背景纯色(0,1)
	float	bgUniform;					// 背景均匀性(0,1)
	float	bgColorL, bgColorA, bgColorB; // 纯色背景的颜色

	// PS效果检测
	float	skinSmoothed;				// 磨皮效果(0,1)
	float	overSharpened;				// 过度锐化(0,1)

	// 综合品质值
	float	detectQuality;				// 综合品质值
};

//图像品质评价结构定义
struct _FGDetectAssess
{
	bool	pass;						// 综合品质是否合格

	bool	fileCorrupt;				// 文件已损坏
	bool    fileFormat;                 // 文件格式不符
	bool    fileSize;                   // 文件大小不符
	bool    imgWidHei;                  // 图像高宽不符
	bool    imgBits;                    // 色彩位数不符
	bool    imgDPI;                     // DPI不符
	bool	imgBorders;					// 相片有边框
	bool	grayrange;					// 灰度范围
	bool	averagegray;				// 平均灰度
	bool	imgNoise;					// 图像噪声
	bool	hasface;					// 脸数（非单人正脸照）
	bool	cropresolution;				// 剪裁分辨率低
	bool	eyedistance;				// 两眼距离不合要求
	bool    redeye;						// 红眼
	bool	eyesopen;					// 眼睛睁开
	bool	bkuniform;					// 背景不均匀
	int     backgroundcolorl;			// 严重偏暗(-2) 偏暗(-1) 正常(0) 偏亮(1) 严重偏亮(2)     
	int     backgroundcolora;			// 严重偏绿(-2) 偏绿(-1) 正常(0) 偏红(1) 严重偏红(2)   
	int     backgroundcolorb;			// 严重偏蓝(-2) 偏蓝(-1) 正常(0) 偏黄(1) 严重偏黄(2)
	bool	bklightover;				// 背景过亮
	bool	bklightunder;				// 背景过暗
	bool    bkcolor;					// 背景颜色不合要求
	int     facecolorl;					// 严重偏暗(-2) 偏暗(-1) 正常(0) 偏亮(1) 严重偏亮(2)    
	int     facecolora;					// 严重偏绿(-2) 偏绿(-1) 正常(0) 偏红(1) 严重偏红(2)
	int     facecolorb;					// 严重偏蓝(-2) 偏蓝(-1) 正常(0) 偏黄(1) 严重偏黄(2)
	bool	lowbrightness;				// 整体（脸部）偏暗
	bool	highbrightness;				// 整体（脸部）偏亮
	bool	skincolorun;				// 肤色不正常
	bool	faceblur;					// 脸部模糊（图像模糊）
	bool	underexposure;				// 曝光不足
	bool	overexposure;				// 曝光过度
	bool	hotspots;					// 脸部高光
	bool	lightuniform;				// 脸部曝光均匀性（光照不均匀）

	bool	wearsglasses;				// 是否戴眼镜
	bool    glassframe;					// 眼镜框宽/色深
	bool    glasscover;					// 眼镜片颜色太深
	bool    glassflash;					// 眼镜反光
	bool    glasscovereye;				// 眼镜框遮挡眼睛
	bool    haircovereye;				// 头发遮住眼睛
	bool	eyecovered;					// 眼睛被遮挡
	int		faceroll;					// 脸旋转 -1：脸逆时针转，1：脸顺时针转，0没转
	int		faceyaw;					// 脸侧转 -1：脸向左侧转，1：脸向右侧转，0没转
	int		headpitch;					// 头俯昂 -1：头向下俯, 1：头向上昂，0:不俯昂
	bool    expression;					// 表情不自然
	bool	facefrontal;				// 正脸（姿态不正）
	bool	eyesfrontal;				// 正眼：眼睛朝前看（目光未视镜头）
	bool	colourcast;					// 整体偏色
	bool	lowcs;						// 整体发灰
	bool	colorWearUn;				// 服饰与背景色接近
	bool	mouthclose;					// 嘴巴闭合

	bool    headhigh;					// 头整体偏上
	bool    headlow;					// 头整体偏下
	bool    headleft;					// 头整体偏左
	bool    headright;					// 头整体偏右
	bool    largehead;					// 头（/脸）偏大
	bool    smallhead;					// 头（/脸）偏小
	bool    eyeYCood;                   // 眼睛Y坐标
	bool    headTop;                    // 头顶距离

	int		headHeightpass;				//(-1,0,1) 偏小，通过，偏大
	int		faceheadWidthpass;			//(-3,-2,-1,1,2,3) 偏小，通过，偏大（负值为脸宽，正值为头宽）

	// PS效果检测
	bool	skinSmoothed;				// 磨皮效果
	bool	overSharpened;				// 过度锐化
};

//图像品质评价策略
struct _FGDetectAssessProfile
{
	bool	fileCorrupt;				// 文件数据损坏
	bool    fileFormat;                 // 文件格式不符
	bool    fileSize;                   // 文件大小不符
	bool    imgWidHei;                  // 图像高宽不符
	bool    imgBits;                    // 色彩位数不符
	bool    imgDPI;                     // DPI不符
	bool	imgBorders;					// 相片有边框
	bool	grayrange;					// 灰度范围
	bool	averagegray;				// 平均灰度
	bool	hasface;					// 脸数（非单人正脸照）
	bool	cropresolution;				// 剪裁分辨率低
	bool	eyedistance;				// 两眼距离不合要求
	bool    redeye;						// 红眼
	bool	eyesopen;					// 眼睛睁开
	bool	bkuniform;					// 背景不均匀
	bool	bklightover;				// 背景过亮
	bool	bklightunder;				// 背景过暗
	bool    bkcolor;					// 背景颜色不合要求
	bool	lowbrightness;				// 整体（脸部）偏暗
	bool	highbrightness;				// 整体（脸部）偏亮
	bool	skincolorun;				// 肤色不正常
	bool	faceblur;					// 脸部模糊（图像模糊）
	bool	underexposure;				// 曝光不足
	bool	overexposure;				// 曝光过度
	bool	hotspots;					// 脸部高光
	bool	lightuniform;				// 脸部曝光均匀性（光照不均匀）

	bool	wearsglasses;				// 是否戴眼镜
	bool    glassframe;					// 眼镜框宽/色深
	bool    glasscover;					// 眼镜片颜色太深
	bool    glassflash;					// 眼镜反光
	bool    glasscovereye;				// 眼镜框遮挡眼睛
	bool    haircovereye;				// 头发遮住眼睛
	bool    expression;					// 表情不自然
	bool	facefrontal;				// 正脸（姿态不正）
	bool	eyesfrontal;				// 正眼：眼睛朝前看（目光未视镜头）
	bool	colourcast;					// 整体偏色
	bool	lowcs;						// 整体发灰
	bool	colorWearUn;				// 服饰与背景色接近
	bool	mouthclose;					// 嘴巴闭合

	bool    headhigh;					// 头整体偏上
	bool    headlow;					// 头整体偏下
	bool    headleft;					// 头整体偏左
	bool    headright;					// 头整体偏右
	bool    largehead;					// 头（/脸）偏大
	bool    smallhead;					// 头（/脸）偏小
	bool    eyeYCood;                   // 眼睛Y坐标
	bool    headTop;                    // 头顶距离
	bool    imgNoise;					// 图像噪声

	bool	skinSmoothed;				// 磨皮效果
	bool	overSharpened;				// 过度锐化
};

struct _FGQualityLevel
{
	int		level;

	int		imgWidth, imgHeight;		// 图像宽度、高度（像素）
	int		imgSize;					// 图像大小
	int		imgBits;					// 图像色彩位数
	int		imgDpi;						// 输出DPI（像素/英寸） 
	int		imgGrayRange;				// 灰度动态范围

	int		eyesDistance;				// 两眼距离
	float	eyesOpen;					// 眼睛开闭
	float	eyesGlasses;				// 是否戴眼镜

	float	faceBlur;					// 脸部模糊度
	float	faceFrontal;				// 正脸
	float	faceRoll;					// 脸旋转角度
	float	faceYaw;					// 脸侧转角度
	float	headPitch;					// 头俯昂角度
	float	hotSpots;					// 脸部高光

	float	skinSmoothed;				// 磨皮效果(0,1)
	float	overSharpened;				// 过度锐化(0,1)
};

struct _FGQualityLevelAssess
{
	int		level;
	bool	isPass;

	bool	imgWidth, imgHeight;		// 图像宽度、高度（像素）
	bool	imgSize;
	bool	imgBits;					// 图像色彩位数
	bool	imgDpi;						// 输出DPI（像素/英寸） 
	bool	imgGrayRange;				// 灰度动态范围

	bool	eyesDistance;				// 两眼距离
	bool	eyesOpen;					// 眼睛开闭
	bool	eyesGlasses;				// 是否戴眼镜

	bool	hasFace;
	bool	faceBlur;					// 脸部模糊度
	bool	faceFrontal;				// 正脸
	bool	faceRoll;					// 脸旋转角度
	bool	faceYaw;					// 脸侧转角度
	bool	headPitch;					// 头俯昂角度
	bool	hotSpots;					// 脸部高光

	bool	skinSmoothed;				// 磨皮效果(0,1)
	bool	overSharpened;				// 过度锐化(0,1)
};

//品质检测可选项设置
struct _FGDetectOption
{
	bool	detEyes;					// 是否检测眼睛睁闭与正向
	bool	detGlasses;					// 是否检测眼镜
	bool	detExpress;					// 是否检测脸部表情
	bool	detMouth;					// 是否检测嘴巴闭合
	bool	detPS;						// 是否检测PS效果

	float	skewAngle;					// 自动调整->自动旋转幅度,多少度之内自动旋转，缺省为2度
};

//剪裁规格扩展数据
struct _FGCropRectExt
{
	int		marginL;
	int		marginT;
	int		marginR;
	int		marginB;
	int		imageWi;
	int		imageHi;
};

//人脸跟踪定位结构
struct _FGFaceLocation
{
	int		id;							// 跟踪ID(暂不支持)
	int		x;							// 脸在图像中的右上角坐标 x
	int		y;							// 脸在图像中的右上角坐标 y
	int		width;						// 脸的宽度，-1表示未知
	int		height;						// 脸的高度，-1表示未知
	float	confidence;					// 脸的信度，-1表示未知
	int		xFirstEye;					// 第一只眼睛的 x 坐标
	int		yFirstEye;					// 第一只眼睛的 y 坐标
	float	firstConfidence;			// confidence for first eye，-1表示未知
	int		xSecondEye;					// 第二只眼睛的 x 坐标
	int		ySecondEye;					// 第二只眼睛的 y 坐标
	float	secondConfidence;			// confidence for second eye，-1表示未知
};

//图像采集设备
#define _FG_VDCS_VIDEOSIZE		1		// 设备视频尺寸改变（大小或方向被改变）
#define _FG_VDCS_DEVICELOST		2		// 设备丢失（断电或关闭）
#define _FG_VDCS_PROPPARAM		3		// 设备属性参数改变

//FG句柄
typedef void* _FGHANDLE;


/// 识别算法实现搜索匹配结构
struct MATCH_RESULT
{
	unsigned int pid;		///< 人员Id
	unsigned int tid;		///< 照片Id
	float score;			//
};
typedef MATCH_RESULT* LPMATCH_RESULT;


/// 服务器返回搜索匹配结构
struct SERVER_MATCH_RESULT
{
	unsigned int pid;		///< 人员Id
	unsigned int tid;		///< 照片Id
	float score;			///< 相似分值
	char customId[41];		///< 照片自定义编号
};


/// 眼睛坐标
struct EYESLOCATION
{
	int xFirstEye;			///< 第一只眼睛的 x 坐标
	int yFirstEye;			///< 第一只眼睛的 y 坐标
	int xSecondEye;			///< 第二只眼睛的 x 坐标
	int ySecondEye;			///< 第二只眼睛的 y 坐标
	float confidence;		///< 脸的信度
};
