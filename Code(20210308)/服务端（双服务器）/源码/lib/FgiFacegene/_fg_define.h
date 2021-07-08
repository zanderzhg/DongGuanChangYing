/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.

	��������

********************************************************************/
/*	@file
 *	@brief	��������
 */

#pragma once

//(��־/����)��Ϣ����
#define _FG_LOG_LEVEL_DEBUG		0		//������Ϣ(Ԥ�������������Ż����ܣ�
#define _FG_LOG_LEVEL_INFO		1		//��������
#define _FG_LOG_LEVEL_RUN		2		//������Ϣ(ϵͳ���ж�Ҫ״̬����)
#define _FG_LOG_LEVEL_ERROR		0x10	//�������(�����޷���ɣ�����ϵͳ������Ӱ��)
#define _FG_LOG_LEVEL_STOP		0x20	//�������߼�(��������¿�������������ϵͳ�������б�Ҫ)
#define _FG_LOG_LEVEL_MAX		255		//��󼶱�

//�ļ���ʽ
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


//ͼ����ɫͨ��
#define _FG_CHANNEL_MASTER		0x0000	//All channels 
#define _FG_CHANNEL_RED			0x0001	//Red channel only
#define _FG_CHANNEL_GREEN		0x0002	//Green channel only
#define _FG_CHANNEL_BLUE		0x0003	//Blue channel only

//ͼ����תģʽ
#define _FG_ROTATE_RESIZE       0x0001
#define _FG_ROTATE_RESAMPLE		0x0002
#define _FG_ROTATE_BICUBIC		0x0004
#define _FG_ROTATE_OFFCENTER	0x0008

//ͼ��ߴ�ģʽ
#define _FG_SIZE_NORMAL			0x0000
#define _FG_SIZE_RESAMPLE		0x0002
#define _FG_SIZE_BICUBIC		0x0004
#define _FG_SIZE_ADBICUBIC		0x0008
#define _FG_SIZE_STRETCH		0x0000
#define _FG_SIZE_KEEPASPECT		0x1000

//��״�ϲ�����ģʽ����
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

//����������
#define IDCARD_READER_NONE			-1	//
#define IDCARD_READER_SYNTHSIS		1	//��˼������100
#define IDCARD_READER_SHENDUN		2	//һ����ܶ�����

//֤�����Ͷ���
#define IDCARD_TYPE_ID_Ix			0	//��һ���������֤
#define IDCARD_TYPE_ID_II			1	//�ڶ����������֤
#define IDCARD_TYPE_ID_I			2	//��һ���������֤
#define IDCARD_TYPE_ID_II_MANAUL	3	//�ֹ�����Ķ���֤
#define IDCARD_TYPE_ID_MILITARY		4	//����֤
#define IDCARD_TYPE_ID_POLICE		5	//����֤
#define IDCARD_TYPE_ID_DRIVER		6	//��ʻ֤
#define IDCARD_TYPE_ID_OTHER		255	//����֤��

//��¼��־����
#define RECORD_TYPE_UPLOADING		-2	//��¼�����ϴ���
#define RECORD_TYPE_ALL				-1	//����״̬
#define RECORD_TYPE_UNUPLOAD		0	//��¼δ�ϴ�
#define RECORD_TYPE_UPLOADED		1	//��¼���ϴ�

//ͼ�񴰿���ʾģʽ
#define _FG_ZOOM_MODE_PERCENT		1	//����ǰ�Ŵ������ʾ
#define _FG_ZOOM_MODE_FITWND		2	//�ʺϴ���
#define _FG_ZOOM_MODE_PERC100		3	//1��1��ʾ

//////////////////////////////////////////////////////////////////////

//�������Ͷ���

#define _FG_FACETYPE_FRONTAL		0	//��������
#define _FG_FACETYPE_LEFTPROFILE	1	//�����
#define _FG_FACETYPE_RIGHTPROFILE	2	//�Ҳ���
#define _FG_FACETYPE_OTHER			3	//����

#define _FG_CPD_EYESDISTANCE		1	//���۾���
#define _FG_CPD_FACEWIDTH			2	//����
#define _FG_CPD_HEADWIDTH			3	//ͷ��
#define _FG_CPD_EYESY				6	//�۾�Y
#define _FG_CPD_HEADTOP				7	//ͷ��Y
#define _FG_CPD_HEADHEIGHT			8	//ͷ��

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


//�����ؼ���궨��Ϣ
struct _FGFacePoint
{
	int faceType;						// �������� 0:���� 1:����� 2:�Ҳ���
	union
	{
		struct struct_frontal			// ��������
		{
			int xleft;					// ������ԭʼͼ���е�λ��
			int yleft;
			int xright;					// ������ԭʼͼ���е�λ��
			int yright;
			int faceLeft;				// �������Ե��X���꣬-1��ʾδ֪
			int faceRight;				// �����ұ�Ե��X���꣬-1��ʾδ֪
		} frontal;
		struct struct_profile			// ��������
		{
			int eyeX;					// �۾���X����
			int eyeY;					// �۾���Y����
			int noseTipX;				// �Ǽ��X����
			int noseTipY;				// �Ǽ��Y���꣬-1��ʾδ֪
			int headBackX;				// �����׵�X����
			int headBackY;				// �����׵�Y���꣬-1��ʾδ֪
		} profile;
	} facePos;
	int headLeft;						// ��ͷ���Ե��X���꣬-1��ʾδ֪
	int headRight;						// ��ͷ�ұ�Ե��X���꣬-1��ʾδ֪
	int headTop;						// ͷ����Y���꣬-1��ʾδ֪
	int chinPos;						// �°͵�Y���꣬-1��ʾδ֪
	double skewAngle;					// ƽ����ƫб�Ƕ� -181��ʾδ֪
};

//������ýṹ����
struct _FGCropProfileData
{
	char	szName[31];					//���÷�������
	int		nFaceType;					//���桢����
	double	fWidth;
	double	fHeight;
	int		unitSize;					//�ߴ絥λ
	bool	bKeepRatio;					//����ȱ�������
	double	fDPI;
	int		unitDPI;
	double	fEyesDisMin;				//���۾���
	double	fEyesDisMax;
	double	fFaceWidthMin;				//���������ڲࣩ
	double	fFaceWidthMax;
	double	fHeadWidthMin;				//ͷ��������ࣩ
	double	fHeadWidthMax;
	double	fEyesYMin;					//�۾�Yλ��
	double	fEyesYMax;
	double	fHeadHeightMin;				//ͷ�ߣ�ͷ�����°ͣ�
	double	fHeadHeightMax;
	double	fHeadTopYMin;				//ͷ��
	double	fHeadTopYMax;
};

// ͼ�����������ݽṹ
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
	float fo1, fo2;						// faceOffset����ƫ��
	float ed1, ed2;						// eyesDistance

	int bktype;							// 0: Monochromel; 1: Complex;
	float bu1, bu2;						// bgUniform
	float bx1, by1, bv1;				// bk a, b, l
	float bx2, by2, bv2;

	int	nFaceType;						//���桢���桢����
	int	bKeepRatio;						//����ȱ�������
	int	fWidth;							// width, height
	int	fHeight;
	int	fEyesDisMin;					//���۾���
	int	fEyesDisMax;
	int	fFaceWidthMin;					//���������ڲࣩ
	int	fFaceWidthMax;
	int	fHeadWidthMin;					//ͷ��������ࣩ
	int	fHeadWidthMax;
	int	fEyesYMin;						//�۾�Yλ��
	int	fEyesYMax;
	int	fHeadHeightMin;					//ͷ�ߣ�ͷ�����°ͣ�
	int	fHeadHeightMax;
	int	fHeadTopYMin;					//ͷ��
	int	fHeadTopYMax;

	// ���ز��ϸ�Χ(��ɫ)
	float dfx1, dfy1, dfv1;
	float dfx2, dfy2, dfv2;
	float dbx1, dby1, dbv1;
	float dbx2, dby2, dbv2;

	float lowcs1, lowcs2;				//����
	float ffroll1, ffroll2;				//����ת
	float ffyaw1, ffyaw2;				//����ת
	float fhpitch1, fhpitch2;			//ͷ����
	float gf1, gf2;						//�۾��ܿ�
	float gc1, gc2;						//�۾�Ƭ��ɫ��ǳ
	float gflash1, gflash2;				//��Ƭ����
	float ec1, ec2;						//�۾����ڵ�
	float ered1, ered2;					//����
	float fexp1, fexp2;					//������Ȼ
	float mc1, mc2;						//��ͱպ�
	float noise1, noise2;				//ͼ������
	float psss1, psss2;					//PS:ĥƤ
	float psos1, psos2;					//PS:������
};

//ͼ��������ֵĩ
struct _FGDetectResult
{
	// �ļ�����
	int		fileFormat;					// �ļ���ʽ
	int		fileSize;					// �ļ���С

	// ͼ������
	int		imgWidth, imgHeight;		// ͼ���ȡ��߶ȣ����أ�
	int		imgBits;					// ͼ��ɫ��λ��
	int		imgDpi;						// ���DPI������/Ӣ�磩 
	int		imgCorrupt;					// ͼ��������
	int		imgGrayRange;				// �Ҷȶ�̬��Χ [0,255]
	float	imgAverageGrayscale;		// ƽ�����ȣ��Ҷȣ�[0,255]
	float	imgLowContrast;				// �������[0,1]
	float	imgNoise;					// ͼ������

	// ��Ƭ����
	int		faceCount;					// ��Ƭ������
	int		headWidth, headHeight;		// ͷ��ͷ��
	int		faceWidth;					// ����
	int		headTop;					// ͷ��
	int     eyeTop;                     // �۾������ϱ���
	float	faceOffsetX, faceOffsetY;	// ����ƫ������ˮƽ��ֱ���ٷֱȣ�
	                                    // (-) ƫ��ƫ�� (+) ƫ�ң�ƫ��
	float	faceFrontal;				// ����
	float	faceRoll;					// ����ת�Ƕ�
	float	faceYaw;					// ����ת�Ƕ�
	float	headPitch;					// ͷ�����Ƕ�
	float	faceExposure;				// �����ع�
	float	faceUniform;				// �������߾�����
	float	faceColorL, faceColorA, faceColorB;	// ����ƽ��ɫ��
	float	faceHotspots;				// �����߹�
	float	faceBlur;					// ����ģ����
	int		eyesDistance;				// ���۾���
	float	eyesOpen;					// �۾�����
	float	eyesGlasses;				// �Ƿ���۾�
	float	eyesFrontal;				// �۾�����ǰ��
	float	eyesColorL, eyesColorA, eyesColorB;	// �۾�ɫ��
	float	glassFrame;					// �۾��ܿ�
	float	glassCover;					// �۾�Ƭ��ɫ��ǳ
	float	glassFlash;					// �۾�����
	float	eyeCovered;					// �۾����ڵ�
	float	eyesRed;					// ����
	float	faceExpression;				// ������Ȼ
	float	mouthClose;					// ��ͱպ�

	// ��������
	float	bgPureColor;				// ������ɫ(0,1)
	float	bgUniform;					// ����������(0,1)
	float	bgColorL, bgColorA, bgColorB; // ��ɫ��������ɫ

	// PSЧ�����
	float	skinSmoothed;				// ĥƤЧ��(0,1)
	float	overSharpened;				// ������(0,1)

	// �ۺ�Ʒ��ֵ
	float	detectQuality;				// �ۺ�Ʒ��ֵ
};

//ͼ��Ʒ�����۽ṹ����
struct _FGDetectAssess
{
	bool	pass;						// �ۺ�Ʒ���Ƿ�ϸ�

	bool	fileCorrupt;				// �ļ�����
	bool    fileFormat;                 // �ļ���ʽ����
	bool    fileSize;                   // �ļ���С����
	bool    imgWidHei;                  // ͼ��߿���
	bool    imgBits;                    // ɫ��λ������
	bool    imgDPI;                     // DPI����
	bool	imgBorders;					// ��Ƭ�б߿�
	bool	grayrange;					// �Ҷȷ�Χ
	bool	averagegray;				// ƽ���Ҷ�
	bool	imgNoise;					// ͼ������
	bool	hasface;					// �������ǵ��������գ�
	bool	cropresolution;				// ���÷ֱ��ʵ�
	bool	eyedistance;				// ���۾��벻��Ҫ��
	bool    redeye;						// ����
	bool	eyesopen;					// �۾�����
	bool	bkuniform;					// ����������
	int     backgroundcolorl;			// ����ƫ��(-2) ƫ��(-1) ����(0) ƫ��(1) ����ƫ��(2)     
	int     backgroundcolora;			// ����ƫ��(-2) ƫ��(-1) ����(0) ƫ��(1) ����ƫ��(2)   
	int     backgroundcolorb;			// ����ƫ��(-2) ƫ��(-1) ����(0) ƫ��(1) ����ƫ��(2)
	bool	bklightover;				// ��������
	bool	bklightunder;				// ��������
	bool    bkcolor;					// ������ɫ����Ҫ��
	int     facecolorl;					// ����ƫ��(-2) ƫ��(-1) ����(0) ƫ��(1) ����ƫ��(2)    
	int     facecolora;					// ����ƫ��(-2) ƫ��(-1) ����(0) ƫ��(1) ����ƫ��(2)
	int     facecolorb;					// ����ƫ��(-2) ƫ��(-1) ����(0) ƫ��(1) ����ƫ��(2)
	bool	lowbrightness;				// ���壨������ƫ��
	bool	highbrightness;				// ���壨������ƫ��
	bool	skincolorun;				// ��ɫ������
	bool	faceblur;					// ����ģ����ͼ��ģ����
	bool	underexposure;				// �عⲻ��
	bool	overexposure;				// �ع����
	bool	hotspots;					// �����߹�
	bool	lightuniform;				// �����ع�����ԣ����ղ����ȣ�

	bool	wearsglasses;				// �Ƿ���۾�
	bool    glassframe;					// �۾����/ɫ��
	bool    glasscover;					// �۾�Ƭ��ɫ̫��
	bool    glassflash;					// �۾�����
	bool    glasscovereye;				// �۾����ڵ��۾�
	bool    haircovereye;				// ͷ����ס�۾�
	bool	eyecovered;					// �۾����ڵ�
	int		faceroll;					// ����ת -1������ʱ��ת��1����˳ʱ��ת��0ûת
	int		faceyaw;					// ����ת -1���������ת��1�������Ҳ�ת��0ûת
	int		headpitch;					// ͷ���� -1��ͷ���¸�, 1��ͷ���ϰ���0:������
	bool    expression;					// ���鲻��Ȼ
	bool	facefrontal;				// ��������̬������
	bool	eyesfrontal;				// ���ۣ��۾���ǰ����Ŀ��δ�Ӿ�ͷ��
	bool	colourcast;					// ����ƫɫ
	bool	lowcs;						// ���巢��
	bool	colorWearUn;				// �����뱳��ɫ�ӽ�
	bool	mouthclose;					// ��ͱպ�

	bool    headhigh;					// ͷ����ƫ��
	bool    headlow;					// ͷ����ƫ��
	bool    headleft;					// ͷ����ƫ��
	bool    headright;					// ͷ����ƫ��
	bool    largehead;					// ͷ��/����ƫ��
	bool    smallhead;					// ͷ��/����ƫС
	bool    eyeYCood;                   // �۾�Y����
	bool    headTop;                    // ͷ������

	int		headHeightpass;				//(-1,0,1) ƫС��ͨ����ƫ��
	int		faceheadWidthpass;			//(-3,-2,-1,1,2,3) ƫС��ͨ����ƫ�󣨸�ֵΪ������ֵΪͷ��

	// PSЧ�����
	bool	skinSmoothed;				// ĥƤЧ��
	bool	overSharpened;				// ������
};

//ͼ��Ʒ�����۲���
struct _FGDetectAssessProfile
{
	bool	fileCorrupt;				// �ļ�������
	bool    fileFormat;                 // �ļ���ʽ����
	bool    fileSize;                   // �ļ���С����
	bool    imgWidHei;                  // ͼ��߿���
	bool    imgBits;                    // ɫ��λ������
	bool    imgDPI;                     // DPI����
	bool	imgBorders;					// ��Ƭ�б߿�
	bool	grayrange;					// �Ҷȷ�Χ
	bool	averagegray;				// ƽ���Ҷ�
	bool	hasface;					// �������ǵ��������գ�
	bool	cropresolution;				// ���÷ֱ��ʵ�
	bool	eyedistance;				// ���۾��벻��Ҫ��
	bool    redeye;						// ����
	bool	eyesopen;					// �۾�����
	bool	bkuniform;					// ����������
	bool	bklightover;				// ��������
	bool	bklightunder;				// ��������
	bool    bkcolor;					// ������ɫ����Ҫ��
	bool	lowbrightness;				// ���壨������ƫ��
	bool	highbrightness;				// ���壨������ƫ��
	bool	skincolorun;				// ��ɫ������
	bool	faceblur;					// ����ģ����ͼ��ģ����
	bool	underexposure;				// �عⲻ��
	bool	overexposure;				// �ع����
	bool	hotspots;					// �����߹�
	bool	lightuniform;				// �����ع�����ԣ����ղ����ȣ�

	bool	wearsglasses;				// �Ƿ���۾�
	bool    glassframe;					// �۾����/ɫ��
	bool    glasscover;					// �۾�Ƭ��ɫ̫��
	bool    glassflash;					// �۾�����
	bool    glasscovereye;				// �۾����ڵ��۾�
	bool    haircovereye;				// ͷ����ס�۾�
	bool    expression;					// ���鲻��Ȼ
	bool	facefrontal;				// ��������̬������
	bool	eyesfrontal;				// ���ۣ��۾���ǰ����Ŀ��δ�Ӿ�ͷ��
	bool	colourcast;					// ����ƫɫ
	bool	lowcs;						// ���巢��
	bool	colorWearUn;				// �����뱳��ɫ�ӽ�
	bool	mouthclose;					// ��ͱպ�

	bool    headhigh;					// ͷ����ƫ��
	bool    headlow;					// ͷ����ƫ��
	bool    headleft;					// ͷ����ƫ��
	bool    headright;					// ͷ����ƫ��
	bool    largehead;					// ͷ��/����ƫ��
	bool    smallhead;					// ͷ��/����ƫС
	bool    eyeYCood;                   // �۾�Y����
	bool    headTop;                    // ͷ������
	bool    imgNoise;					// ͼ������

	bool	skinSmoothed;				// ĥƤЧ��
	bool	overSharpened;				// ������
};

struct _FGQualityLevel
{
	int		level;

	int		imgWidth, imgHeight;		// ͼ���ȡ��߶ȣ����أ�
	int		imgSize;					// ͼ���С
	int		imgBits;					// ͼ��ɫ��λ��
	int		imgDpi;						// ���DPI������/Ӣ�磩 
	int		imgGrayRange;				// �Ҷȶ�̬��Χ

	int		eyesDistance;				// ���۾���
	float	eyesOpen;					// �۾�����
	float	eyesGlasses;				// �Ƿ���۾�

	float	faceBlur;					// ����ģ����
	float	faceFrontal;				// ����
	float	faceRoll;					// ����ת�Ƕ�
	float	faceYaw;					// ����ת�Ƕ�
	float	headPitch;					// ͷ�����Ƕ�
	float	hotSpots;					// �����߹�

	float	skinSmoothed;				// ĥƤЧ��(0,1)
	float	overSharpened;				// ������(0,1)
};

struct _FGQualityLevelAssess
{
	int		level;
	bool	isPass;

	bool	imgWidth, imgHeight;		// ͼ���ȡ��߶ȣ����أ�
	bool	imgSize;
	bool	imgBits;					// ͼ��ɫ��λ��
	bool	imgDpi;						// ���DPI������/Ӣ�磩 
	bool	imgGrayRange;				// �Ҷȶ�̬��Χ

	bool	eyesDistance;				// ���۾���
	bool	eyesOpen;					// �۾�����
	bool	eyesGlasses;				// �Ƿ���۾�

	bool	hasFace;
	bool	faceBlur;					// ����ģ����
	bool	faceFrontal;				// ����
	bool	faceRoll;					// ����ת�Ƕ�
	bool	faceYaw;					// ����ת�Ƕ�
	bool	headPitch;					// ͷ�����Ƕ�
	bool	hotSpots;					// �����߹�

	bool	skinSmoothed;				// ĥƤЧ��(0,1)
	bool	overSharpened;				// ������(0,1)
};

//Ʒ�ʼ���ѡ������
struct _FGDetectOption
{
	bool	detEyes;					// �Ƿ����۾�����������
	bool	detGlasses;					// �Ƿ����۾�
	bool	detExpress;					// �Ƿ�����������
	bool	detMouth;					// �Ƿ�����ͱպ�
	bool	detPS;						// �Ƿ���PSЧ��

	float	skewAngle;					// �Զ�����->�Զ���ת����,���ٶ�֮���Զ���ת��ȱʡΪ2��
};

//���ù����չ����
struct _FGCropRectExt
{
	int		marginL;
	int		marginT;
	int		marginR;
	int		marginB;
	int		imageWi;
	int		imageHi;
};

//�������ٶ�λ�ṹ
struct _FGFaceLocation
{
	int		id;							// ����ID(�ݲ�֧��)
	int		x;							// ����ͼ���е����Ͻ����� x
	int		y;							// ����ͼ���е����Ͻ����� y
	int		width;						// ���Ŀ�ȣ�-1��ʾδ֪
	int		height;						// ���ĸ߶ȣ�-1��ʾδ֪
	float	confidence;					// �����Ŷȣ�-1��ʾδ֪
	int		xFirstEye;					// ��һֻ�۾��� x ����
	int		yFirstEye;					// ��һֻ�۾��� y ����
	float	firstConfidence;			// confidence for first eye��-1��ʾδ֪
	int		xSecondEye;					// �ڶ�ֻ�۾��� x ����
	int		ySecondEye;					// �ڶ�ֻ�۾��� y ����
	float	secondConfidence;			// confidence for second eye��-1��ʾδ֪
};

//ͼ��ɼ��豸
#define _FG_VDCS_VIDEOSIZE		1		// �豸��Ƶ�ߴ�ı䣨��С���򱻸ı䣩
#define _FG_VDCS_DEVICELOST		2		// �豸��ʧ���ϵ��رգ�
#define _FG_VDCS_PROPPARAM		3		// �豸���Բ����ı�

//FG���
typedef void* _FGHANDLE;


/// ʶ���㷨ʵ������ƥ��ṹ
struct MATCH_RESULT
{
	unsigned int pid;		///< ��ԱId
	unsigned int tid;		///< ��ƬId
	float score;			//
};
typedef MATCH_RESULT* LPMATCH_RESULT;


/// ��������������ƥ��ṹ
struct SERVER_MATCH_RESULT
{
	unsigned int pid;		///< ��ԱId
	unsigned int tid;		///< ��ƬId
	float score;			///< ���Ʒ�ֵ
	char customId[41];		///< ��Ƭ�Զ�����
};


/// �۾�����
struct EYESLOCATION
{
	int xFirstEye;			///< ��һֻ�۾��� x ����
	int yFirstEye;			///< ��һֻ�۾��� y ����
	int xSecondEye;			///< �ڶ�ֻ�۾��� x ����
	int ySecondEye;			///< �ڶ�ֻ�۾��� y ����
	float confidence;		///< �����Ŷ�
};
