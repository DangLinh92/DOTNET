USE [HRMSDB2]
GO

/****** Object:  UserDefinedTableType [dbo].[NHANVIEN_CALAMVIEC]    Script Date: 2022-03-05 11:44:39 AM ******/
CREATE TYPE [dbo].[NHANVIEN_CALAMVIEC] AS TABLE(
	[MaNV] [nvarchar](50) NULL,
	[Danhmuc_CaLviec] [nvarchar](50) NULL,
	[BatDau_TheoCa] [nvarchar](50) NULL,
	[KetThuc_TheoCa] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[DATA_RESULT_HRMS_BIOSTAR]    Script Date: 2022-03-05 11:44:58 AM ******/
CREATE TYPE [dbo].[DATA_RESULT_HRMS_BIOSTAR] AS TABLE(
	[Date_Check] [nvarchar](50) NULL,
	[userId] [varchar](64) NULL,
	[userName] [nvarchar](96) NULL,
	[Department] [nvarchar](255) NULL,
	[Shift_] [nvarchar](64) NULL,
	[Daily_Schedule] [nvarchar](64) NULL,
	[First_In_Time] [nvarchar](50) NULL,
	[Last_Out_Time] [nvarchar](50) NULL,
	[Result] [nvarchar](255) NULL,
	[First_In] [nvarchar](50) NULL,
	[Last_Out] [nvarchar](50) NULL,
	[HanhChinh] [nvarchar](50) NULL,
	[TangCa] [nvarchar](50) NULL,
	[BreakTime] [nvarchar](50) NULL,
	[WorkTime] [nvarchar](50) NULL
)
GO
CREATE TYPE [dbo].[DANG_KY_OT_NVIEN_TYPE] AS TABLE(
	[NgayOT] [nvarchar](50) NULL,
	[MaNV] [nvarchar](50) NULL,
	[DM_NgayLViec] [nvarchar](50) NULL,
	[Approve] [nvarchar](50) NULL
)
GO

CREATE PROC [dbo].[PKG_BUSINESS@PUT_EVENT_LOG](
@A_DATA		DATA_RESULT_HRMS_BIOSTAR READONLY,
@N_RETURN			int				OUTPUT,
@V_RETURN			NVARCHAR(4000)	OUTPUT
)
AS
BEGIN TRY
		BEGIN
		    SET NOCOUNT OFF;  

			MERGE [dbo].[CHAM_CONG_LOG] AS TARGET
			USING @A_DATA AS SOURCE
			ON (
			      TARGET.ID_NV = SOURCE.userId AND
				  TARGET.Ngay_ChamCong = SOURCE.Date_Check
			   )
			WHEN MATCHED
			    THEN UPDATE SET 
				     TARGET.FirstIn_Time =SOURCE.[First_In_Time],
					 TARGET.Last_Out_Time = SOURCE.[Last_Out_Time], 
					 TARGET.[FirstIn] =  SOURCE.[First_In],
					 TARGET.[LastOut]	=   SOURCE.[Last_Out],
					 TARGET.[DateModified] = FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),
					 TARGET.[Ten_NV]   =  SOURCE.[userName]	,
					  TARGET.[Department] = SOURCE.Department
			WHEN NOT MATCHED BY TARGET 
			    THEN INSERT ([MaNV],[Danhmuc_CaLviec],[BatDau_TheoCa],[KetThuc_TheoCa],[DateCreated],[DateModified],[UserCreated],[UserModified],[Status],Approved)
				VALUES(SOURCE.[MaNV],SOURCE.[Danhmuc_CaLviec], SOURCE.[BatDau_TheoCa],SOURCE.[KetThuc_TheoCa],FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),'sys','sys',SOURCE.[Status],'N');
       END
	SET @N_RETURN = 0;
	SET @V_RETURN = 'MSG_COM_004';
END TRY
	BEGIN CATCH
  SET @N_RETURN = ERROR_NUMBER();
  SET @V_RETURN = ERROR_MESSAGE();
END CATCH

GO
-----
CREATE PROC [dbo].[PKG_BUSINESS@PUT_NHANVIEN_CALAMVIEC](
@A_DATA		NHANVIEN_CALAMVIEC READONLY,
@N_RETURN			int				OUTPUT,
@V_RETURN			NVARCHAR(4000)	OUTPUT
)
AS
BEGIN TRY
		BEGIN
		    SET NOCOUNT OFF;  

			MERGE [dbo].[NHANVIEN_CALAMVIEC] AS TARGET
			USING @A_DATA AS SOURCE
			ON (
			      TARGET.[MaNV] = SOURCE.[MaNV] AND
				  TARGET.[Danhmuc_CaLviec] = SOURCE.[Danhmuc_CaLviec] AND
				  TARGET.[BatDau_TheoCa] =  SOURCE.[BatDau_TheoCa] AND
				  TARGET.[KetThuc_TheoCa] = SOURCE.[KetThuc_TheoCa]
				)
			WHEN MATCHED
			    THEN UPDATE SET 
					 TARGET.[DateModified] = FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),
					 TARGET.[Status]   =  SOURCE.[Status]
			WHEN NOT MATCHED BY TARGET 
			    THEN INSERT ([MaNV],[Danhmuc_CaLviec],[BatDau_TheoCa],[KetThuc_TheoCa],[DateCreated],[DateModified],[UserCreated],[UserModified],[Status])
				VALUES(SOURCE.[MaNV],SOURCE.[Danhmuc_CaLviec], SOURCE.[BatDau_TheoCa],SOURCE.[KetThuc_TheoCa],FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),'sys','sys',SOURCE.[Status]);
       END
	SET @N_RETURN = 0;
	SET @V_RETURN = 'MSG_COM_004';
END TRY
	BEGIN CATCH
  SET @N_RETURN = ERROR_NUMBER();
  SET @V_RETURN = ERROR_MESSAGE();
END CATCH
---
CREATE PROC [dbo].[PKG_BUSINESS@PUT_NHANVIEN_OVERTIME](
@A_DATA		[dbo].[DANG_KY_OT_NVIEN_TYPE] READONLY,
@N_RETURN			int				OUTPUT,
@V_RETURN			NVARCHAR(4000)	OUTPUT
)
AS
BEGIN TRY
		BEGIN
		    SET NOCOUNT OFF;  

			MERGE [dbo].[DANGKY_OT_NHANVIEN] AS TARGET
			USING @A_DATA AS SOURCE
			ON (
			      TARGET.[NgayOT] = SOURCE.[NgayOT] AND
				  TARGET.[MaNV] = SOURCE.[MaNV]
			   )
			WHEN MATCHED
			    THEN UPDATE SET 
					 TARGET.[DateModified] = FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),
					 TARGET.[DM_NgayLViec] = SOURCE.[DM_NgayLViec],
					 TARGET.[Approve]	   = SOURCE.[Approve]
			WHEN NOT MATCHED BY TARGET 
			    THEN INSERT ([NgayOT],[MaNV],[DM_NgayLViec],[DateCreated],[DateModified],[UserCreated],[UserModified],[Approve],[Passed])
				VALUES(SOURCE.[NgayOT],SOURCE.[MaNV],SOURCE.[DM_NgayLViec],FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),'sys','sys',SOURCE.[Approve],'N');
       END
	SET @N_RETURN = 0;
	SET @V_RETURN = 'MSG_COM_004';
END TRY
	BEGIN CATCH
  SET @N_RETURN = ERROR_NUMBER();
  SET @V_RETURN = ERROR_MESSAGE();
END CATCH