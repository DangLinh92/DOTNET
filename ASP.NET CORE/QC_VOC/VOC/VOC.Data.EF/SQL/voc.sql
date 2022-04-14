USE [GLOBAL_VOC]
GO
CREATE TYPE [dbo].[VOC_MST_TYPE] AS TABLE(
	[Received_site] [nvarchar](50) NULL,
	[PlaceOfOrigin] [nvarchar](50) NULL,
	[ReceivedDept] [nvarchar](50) NULL,
	[ReceivedDate] [nvarchar](50) NULL,
	[SPLReceivedDate] [nvarchar](50) NULL,
	[SPLReceivedDateWeek] [nvarchar](50) NULL,
	[Customer] [nvarchar](150) NULL,
	[SETModelCustomer] [nvarchar](100) NULL,
	[ProcessCustomer] [nvarchar](250) NULL,
	[ModelFullname] [nvarchar](50) NULL,
	[DefectNameCus] [nvarchar](500) NULL,
	[DefectRate] [nvarchar](50) NULL,
	[PBA_FAE_Result] [nvarchar](50) NULL,
	[PartsClassification] [nvarchar](50) NULL,
	[PartsClassification2] [nvarchar](50) NULL,
	[ProdutionDateMarking] [nvarchar](500) NULL,
	[AnalysisResult] [nvarchar](500) NULL,
	[VOCCount] [nvarchar](50) NULL,
	[DefectCause] [nvarchar](500) NULL,
	[DefectClassification] [nvarchar](100) NULL,
	[CustomerResponse] [nvarchar](500) NULL,
	[Report_FinalApprover] [nvarchar](50) NULL,
	[Report_Sender] [nvarchar](50) NULL,
	[Rport_sentDate] [nvarchar](50) NULL,
	[VOCState] [nvarchar](max) NULL,
	[VOCFinishingDate] [nvarchar](50) NULL,
	[VOC_TAT] [nvarchar](50) NULL
)
GO

/****** Object:  StoredProcedure [dbo].[PKG_BUSINESS@PUT_VOC]    Script Date: 2022-04-14 5:13:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[PKG_BUSINESS@PUT_VOC](
@A_USER NVARCHAR(50),
@A_DATA		VOC_MST_TYPE READONLY,
@N_RETURN			int				OUTPUT,
@V_RETURN			NVARCHAR(4000)	OUTPUT
)
AS
BEGIN TRY
		BEGIN
		    SET NOCOUNT OFF;  

			INSERT INTO [dbo].[VOC_MST]
			SELECT 
				    [Received_site]
				    ,[PlaceOfOrigin]
				    ,[ReceivedDept]
				    ,[ReceivedDate]
				    ,[SPLReceivedDate]
				    ,[SPLReceivedDateWeek]
				    ,[Customer]
				    ,[SETModelCustomer]
				    ,[ProcessCustomer]
				    ,[ModelFullname]
				    ,[DefectNameCus]
				    ,[DefectRate]
				    ,[PBA_FAE_Result]
				    ,[PartsClassification]
				    ,[PartsClassification2]
				    ,[ProdutionDateMarking]
				    ,[AnalysisResult]
				    ,[VOCCount]
				    ,[DefectCause]
				    ,[DefectClassification]
				    ,[CustomerResponse]
				    ,[Report_FinalApprover]
				    ,[Report_Sender]
				    ,[Rport_sentDate]
				    ,[VOCState]
				    ,[VOCFinishingDate]
				    ,[VOC_TAT] , 
					 SYSDATETIME(),
					 SYSDATETIME(),
					  @A_USER,
					  @A_USER,
					  ''
			FROM @A_DATA
       END
	SET @N_RETURN = 0;
	SET @V_RETURN = 'MSG_COM_004';
END TRY
	BEGIN CATCH
  SET @N_RETURN = ERROR_NUMBER();
  SET @V_RETURN = ERROR_MESSAGE();
END CATCH