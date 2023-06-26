USE [OPERATION_MNSDB]
GO
/****** Object:  StoredProcedure [dbo].[GET_OUT_PUT_LOT_LIST_SAMPLE]    Script Date: 2023-06-21 1:54:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GET_OUT_PUT_LOT_LIST_SAMPLE]
AS
BEGIN TRY
BEGIN
  	  -- view outout lot list : mes

    DECLARE @A_Year [int] 
	DECLARE @A_Month [int] 
	DECLARE @A_MucDoKhanCap [int]
	DECLARE @A_Model [nvarchar](50)
	DECLARE @A_Code [nvarchar](50)
	DECLARE @A_PhanLoai [nvarchar](50)
	DECLARE @A_ModelDonLinhKien [nvarchar](50)
	DECLARE @A_LotNo [nvarchar](50)
	DECLARE @A_QtyInput [int]
	DECLARE @A_QtyNG [int]
	DECLARE @A_OperationNow [nvarchar](50) 
	DECLARE @A_MucDichNhap [nvarchar](50)
	DECLARE @A_GhiChu [nvarchar](250)
	DECLARE @A_NguoiChiuTrachNhiem [nvarchar](50)
	DECLARE @A_InputDate [nvarchar](50)
	DECLARE @A_OutputDate [nvarchar](50)
	DECLARE @A_PlanInputDate [nvarchar](50)
	DECLARE @A_PlanOutputDate [nvarchar](50)
	DECLARE @A_Wall_Plan_Date [nvarchar](50)
	DECLARE @A_Wall_Actual_Date [nvarchar](50)
	DECLARE @A_Roof_Plan_Date [nvarchar](50)
	DECLARE @A_Roof_Actual_Date [nvarchar](50)
	DECLARE @A_Seed_Plan_Date [nvarchar](50)
	DECLARE @A_Seed_Actual_Date [nvarchar](50)
	DECLARE @A_PlatePR_Plan_Date [nvarchar](50)
	DECLARE @A_PlatePR_Actual_Date [nvarchar](50)
	DECLARE @A_Plate_Plan_Date [nvarchar](50)
	DECLARE @A_Plate_Actual_Date [nvarchar](50)
	DECLARE @A_PreProbe_Plan_Date [nvarchar](50)
	DECLARE @A_PreProbe_Actual_Date [nvarchar](50)
	DECLARE @A_PreDicing_Plan_Date [nvarchar](50)
	DECLARE @A_PreDicing_Actual_Date [nvarchar](50)
	DECLARE @A_AllProbe_Plan_Date [nvarchar](50)
	DECLARE @A_AllProbe_Actual_Date [nvarchar](50)
	DECLARE @A_BG_Plan_Date [nvarchar](50)
	DECLARE @A_BG_Actual_Date [nvarchar](50)
	DECLARE @A_Dicing_Plan_Date [nvarchar](50)
	DECLARE @A_Dicing_Actual_Date [nvarchar](50)
	DECLARE @A_ChipIns_Plan_Date [nvarchar](50)
	DECLARE @A_ChipIns_Actual_Date [nvarchar](50) 
	DECLARE @A_Packing_Plan_Date [nvarchar](50)
	DECLARE @A_Packing_Actual_Date [nvarchar](50) 
	DECLARE @A_OQC_Plan_Date [nvarchar](50)
	DECLARE @A_OQC_Actual_Date [nvarchar](50)
	DECLARE @A_Shipping_Plan_Date [nvarchar](50) 
	DECLARE @A_Shipping_Actual_Date [nvarchar](50)
	DECLARE @A_LeadTime [int]
	DECLARE @A_DeleteFlg nvarchar(5)
	DECLARE @A_WaferOuput [int]
	DECLARE @A_LeadTime_Plan [int]

   DECLARE OutPutLotListCusor CURSOR LOCAL STATIC FOR
   SELECT distinct
		A.MATERIAL_GROUP4 'Model', 
		A.MATERIAL_GROUP3 'LNLT', 
		A.MATERIAL_ID 'Material', 
		A.CASSETTE_ID 'Cassette ID', 
		B.DELETE_FLAG
FROM 
  (
    SELECT distinct 
      M.MATERIAL_GROUP3, 
      M.MATERIAL_GROUP4, 
      L.MATERIAL_ID,
	  L.CASSETTE_ID
    FROM 
      [10.70.21.215].[WHWLP].[dbo].WS_LOT_SUMMARY L (NOLOCK) 
      INNER JOIN [10.70.21.215].[WHWLP].[dbo].NM_LOTS A WITH (NOLOCK) ON A.LOT_ID = L.LOT_ID 
      INNER JOIN [10.70.21.215].[WHWLP].[dbo].NM_OPERATIONS O (NOLOCK) ON O.SITE_ID = L.SITE_ID 
      AND O.OPERATION_ID = L.OPERATION_ID 
      INNER JOIN [10.70.21.215].[WHWLP].[dbo].NM_MATERIALS M (NOLOCK) ON M.SITE_ID = L.SITE_ID 
      AND M.MATERIAL_ID = L.MATERIAL_ID 
      LEFT OUTER JOIN [10.70.21.215].[WHWLP].[dbo].WS_REEL_SHIPPING R WITH (NOLOCK) ON R.LOT_ID = L.LOT_ID 
      LEFT OUTER JOIN [10.70.21.215].[WHWLP].[dbo].WS_CUSTOMERS C WITH (NOLOCK) ON C.CUSTOMER_ID = R.CUSTOMER_ID 
      LEFT OUTER JOIN [10.70.21.215].[WHWLP].[dbo].NM_EQUIPMENT E (NOLOCK) ON E.SITE_ID = L.SITE_ID 
      AND E.EQUIPMENT_ID = L.EQUIPMENT_ID 
      LEFT OUTER JOIN [10.70.21.215].[WHWLP].[dbo].NB_CODE_DATA LT (NOLOCK) ON LT.SITE_ID = L.SITE_ID 
      AND LT.PK1 = L.LOT_TYPE 
      AND LT.TABLE_ID = 'LOT_TYPE' 
      LEFT OUTER JOIN [10.70.21.215].[WHWLP].[dbo].NB_CODE_DATA LC (NOLOCK) ON LC.SITE_ID = L.SITE_ID 
      AND LC.PK1 = L.LOT_CATEGORY 
      AND LC.TABLE_ID = 'LOT_CATEGORY' 
    WHERE 
      L.SITE_ID = 'WHWLP' 
      AND L.COMPLETE_FLAG = 'Y' 
      AND L.LOT_OPTION = '' 
      AND L.LOT_TYPE IN ('C')
	  AND (A.DELETE_FLAG <> 'Y' OR (A.DELETE_FLAG = 'Y' AND 
	  O.OPERATION_ID IN ('BS00000','OPD0100','OPD0200','OPD0300','OPD0400','OPD0500','OPD0600','OPD0700','OPD0800','OPD0900','OPD1000','OPD1100','OPD1200','OPD1300','OPD1400','OPD1500','OPD1600','OPD1700','OPD1800','OPD1900','OPD2000','OPD2100','OPD2200','OPD2300','OPD2400','OPD2500','OPD2600','OPD2700','OPD2800','OPD2900','OPD3000','OPD3100','OPD3200','OPD3300','OPD3400','OPD3500','OPD3600','OPD3700','OPD3800','OPD3900','OPD4000','OPD4100','OPD4200','OPD4300','OPD4400','OPD4500','OPD4600','OPD4700','OPD4800','OPD4900','OPD5000','OPD5100','OPD5200','OPD5300','OPD5400','OPD5500','FA00000','RM00000','IN30000','OP00000','OP30000','OP00500','OP30500','OP30700','OP31000','OP33000','OP34000','OP35000','OP36000','OP37000','OP37500','OP39000','OP38000','OP39500','OP40000','OP40500','OP41000','OP42000','OP43000','OP44000','OP45000','OP46000','OP46500','OP47000','OP47100','OP48000','OP48500','OP49000','OP50000','OP51000','OP52000','OP52500','OP53000','OP54000','OP55000','OP56000','OP56500','OP57000','OP58000','OP59000','OP59500','OP60000','OP61000','OP65000','OP62000','OP62500','OP63000','OP64000','OP69000','OP69500','OP70000','OP70500','OP71000','OP72000','OP72500','OP73000','OP74000','OPS0100','OPS0200','OP75000','OPT1500','OP76000','OPT1000','OP77000','OPT2000','OP78000','OPT3000','OP79000','OP01100','OP02000','OP04000','OP04500','OP05000','OP06000','OP06500','OP07000','OP06700','OP08000','OP10000','OP09000','OP11000')  
	  AND DATEDIFF(day,CAST(L.WORK_DATE AS DATE),SYSDATETIME()) <= 5))
  ) A 
inner join  (
select DISTINCT CASSETTE_ID,DELETE_FLAG
	from  [10.70.21.215].[WHWLP].[dbo].NM_LOTS L WITH (NOLOCK) 
	where L.LOT_TYPE IN ('C')) 	  -- AND L.DELETE_FLAG <> 'Y' AND L.SITE_ID = 'WHWLP'
	B ON A.CASSETTE_ID = B.CASSETTE_ID
ORDER BY A.CASSETTE_ID ,B.DELETE_FLAG DESC

  OPEN OutPutLotListCusor

  FETCH NEXT FROM OutPutLotListCusor
  INTO @A_Model,@A_PhanLoai,@A_ModelDonLinhKien,@A_LotNo,@A_DeleteFlg

  		   WHILE @@FETCH_STATUS = 0
			 BEGIN

			        SET @A_MucDichNhap = ''
					SET @A_NguoiChiuTrachNhiem = ''
					SET @A_PlanOutputDate = ''
					SET @A_Code = ''

					SELECT @A_MucDichNhap  = [MucDichInput],
					       @A_NguoiChiuTrachNhiem = [NguoiChiuTrachNhiem],
						   @A_PlanOutputDate = format(CAST([OutPutDatePlan] AS date),'yyyyMMdd'),
						   @A_PlanInputDate = format(CAST([InputDatePlan] as date),'yyyyMMdd'),
						   @A_Code = [Code]
					FROM [dbo].[TCARD_SAMPLE] WHERE [LotNo] = @A_LotNo

					SET @A_QtyInput = [dbo].[GET_INPUT_QTY_CASSETID](@A_LotNo)
					SET @A_OperationNow = [dbo].[GET_CURRENT_OPERATION](@A_LotNo)
					SET @A_InputDate = [dbo].[GET_INPUT_TIME_CASSETID](@A_LotNo) 
					SET @A_Wall_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP39000','')
					SET @A_Roof_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP51000','') 
					SET @A_Seed_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP52000','') 
					SET @A_PlatePR_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP59000','')
					SET @A_Plate_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP70000','') 
					SET @A_AllProbe_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP72500','') 
					SET @A_BG_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OPT3000','')
					SET @A_Dicing_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP02000','') 
					SET @A_ChipIns_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP05000','') 
					SET @A_Packing_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP06000','')
					set @A_LeadTime_Plan = isnull([dbo].[GET_WORKDAY](CAST(@A_PlanInputDate AS DATE),CAST(@A_PlanOutputDate AS DATE)),0)

					SET @A_WaferOuput = [dbo].[GET_OUTPUT_WAFER_CASSET_ID](@A_LotNo,@A_Code)

					IF @A_Code = 'P'
					   BEGIN
							SET @A_OQC_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OPS0100','')
							SET @A_Shipping_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OPS0300','')
					   END
					ELSE 
					  BEGIN
							SET @A_OQC_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP09000','')
							SET @A_Shipping_Actual_Date = [dbo].[GET_WORK_DATE_BY_OPERATION_CASSSET_ID](@A_LotNo,'OP90000','')
					  END
					
					SET @A_Year = CAST(SUBSTRING(@A_InputDate,1,4) AS int)
					SET @A_Month = CAST(SUBSTRING(@A_InputDate,5,2) AS int)

					IF @A_Shipping_Actual_Date IS NOT NULL 
					   set @A_LeadTime = [dbo].[GET_WORKDAY](CAST(@A_InputDate AS DATE),CAST(@A_Shipping_Actual_Date AS DATE)) --DATEDIFF(day,CAST(@A_InputDate AS DATE),CAST(@A_Shipping_Actual_Date AS DATE))
					ELSE 
					   SET @A_LeadTime = -1 

					  -- Tính thoi gian chay cong doan
					IF TRY_CAST(@A_PlanInputDate as date) IS NOT NULL
					 BEGIN
							IF @A_LeadTime_Plan >= 8 
							  BEGIN
							  IF @A_Code = 'H' OR @A_Code = 'R' OR @A_Code = 'Z'
							    BEGIN
										SET @A_Wall_Plan_Date =	   format(dateadd(day,1,cast(@A_PlanInputDate as date)),'yyyyMMdd')
										SET @A_Roof_Plan_Date =	   format(dateadd(day,2,cast(@A_PlanInputDate as date)),'yyyyMMdd')

										SET @A_Seed_Plan_Date =	   format(dateadd(day,3,cast(@A_PlanInputDate as date)),'yyyyMMdd')
										SET @A_PlatePR_Plan_Date = format(dateadd(day,3,cast(@A_PlanInputDate as date)),'yyyyMMdd')

										SET @A_Plate_Plan_Date =     format(dateadd(day,4,cast(@A_PlanInputDate as date)),'yyyyMMdd')
										SET @A_PreProbe_Plan_Date =  format(dateadd(day,4,cast(@A_PlanInputDate as date)),'yyyyMMdd')
										SET @A_PreDicing_Plan_Date = format(dateadd(day,4,cast(@A_PlanInputDate as date)),'yyyyMMdd')

										SET @A_AllProbe_Plan_Date =  format(dateadd(day,5,cast(@A_PlanInputDate as date)),'yyyyMMdd')

										SET @A_BG_Plan_Date =     format(dateadd(day,6,cast(@A_PlanInputDate as date)),'yyyyMMdd')
										SET @A_Dicing_Plan_Date = format(dateadd(day,6,cast(@A_PlanInputDate as date)),'yyyyMMdd')

										SET @A_ChipIns_Plan_Date = format(dateadd(day,7,cast(@A_PlanInputDate as date)),'yyyyMMdd')
										SET @A_Packing_Plan_Date = format(dateadd(day,7,cast(@A_PlanInputDate as date)),'yyyyMMdd')

										SET @A_OQC_Plan_Date = @A_PlanOutputDate
										SET @A_Shipping_Plan_Date = @A_PlanOutputDate
								END
					   END
							ELSE 
							    BEGIN
								 IF @A_Code = 'H' OR @A_Code = 'R' OR @A_Code = 'Z'
									BEGIN
											SET @A_Wall_Plan_Date =	   @A_PlanInputDate
											SET @A_Roof_Plan_Date =	   format(dateadd(day,1,cast(@A_PlanInputDate as date)),'yyyyMMdd')

											SET @A_Seed_Plan_Date =	   format(dateadd(day,2,cast(@A_PlanInputDate as date)),'yyyyMMdd')
											SET @A_PlatePR_Plan_Date = format(dateadd(day,2,cast(@A_PlanInputDate as date)),'yyyyMMdd')

											SET @A_Plate_Plan_Date =     format(dateadd(day,3,cast(@A_PlanInputDate as date)),'yyyyMMdd')
											SET @A_PreProbe_Plan_Date =  format(dateadd(day,3,cast(@A_PlanInputDate as date)),'yyyyMMdd')

											SET @A_AllProbe_Plan_Date =  format(dateadd(day,4,cast(@A_PlanInputDate as date)),'yyyyMMdd')
											SET @A_BG_Plan_Date =        format(dateadd(day,4,cast(@A_PlanInputDate as date)),'yyyyMMdd')
											SET @A_Dicing_Plan_Date =    format(dateadd(day,4,cast(@A_PlanInputDate as date)),'yyyyMMdd')

											SET @A_ChipIns_Plan_Date = format(dateadd(day,5,cast(@A_PlanInputDate as date)),'yyyyMMdd')
											SET @A_Packing_Plan_Date = format(dateadd(day,5,cast(@A_PlanInputDate as date)),'yyyyMMdd')

											SET @A_OQC_Plan_Date = @A_PlanOutputDate
											SET @A_Shipping_Plan_Date = @A_PlanOutputDate
									END
						 END

							 IF @A_Code = 'P'
							   BEGIN
									    SET @A_Wall_Plan_Date =	   format(dateadd(day,1,cast(@A_PlanInputDate as date)),'yyyyMMdd')
										SET @A_Roof_Plan_Date =	   format(dateadd(day,2,cast(@A_PlanInputDate as date)),'yyyyMMdd')

										SET @A_Seed_Plan_Date =	   format(dateadd(day,3,cast(@A_PlanInputDate as date)),'yyyyMMdd')
										SET @A_PlatePR_Plan_Date = format(dateadd(day,3,cast(@A_PlanInputDate as date)),'yyyyMMdd')

										SET @A_Plate_Plan_Date =     format(dateadd(day,4,cast(@A_PlanInputDate as date)),'yyyyMMdd')
										SET @A_PreProbe_Plan_Date =  format(dateadd(day,4,cast(@A_PlanInputDate as date)),'yyyyMMdd')

										SET @A_OQC_Plan_Date = @A_PlanOutputDate
										SET @A_Shipping_Plan_Date = @A_PlanOutputDate
							   END
							ELSE IF @A_Code = 'M'
							     BEGIN
										 	SET @A_BG_Plan_Date =        format(dateadd(day,1,cast(@A_PlanInputDate as date)),'yyyyMMdd')
											SET @A_Dicing_Plan_Date =    format(dateadd(day,1,cast(@A_PlanInputDate as date)),'yyyyMMdd')

											SET @A_ChipIns_Plan_Date = format(dateadd(day,2,cast(@A_PlanInputDate as date)),'yyyyMMdd')
											SET @A_Packing_Plan_Date = format(dateadd(day,3,cast(@A_PlanInputDate as date)),'yyyyMMdd')

											SET @A_OQC_Plan_Date = @A_PlanOutputDate
										    SET @A_Shipping_Plan_Date = @A_PlanOutputDate
								 END
					END

					 IF EXISTS(SELECT * FROM [dbo].[TINH_HINH_SAN_XUAT_SAMPLE] WHERE [LotNo] = @A_LotNo)
					    BEGIN
						     IF @A_QtyInput <= 0  -- k update khi da het cong doan
							    SELECT @A_QtyInput = [QtyInput]
								FROM [dbo].[TINH_HINH_SAN_XUAT_SAMPLE] WHERE [LotNo] = @A_LotNo

						     UPDATE [dbo].[TINH_HINH_SAN_XUAT_SAMPLE]
								SET [Year] = @A_Year
								   ,[Month] = @A_Month
								   ,[Model] = @A_Model
								   ,[Code] = @A_Code
								   ,[PhanLoai] = @A_PhanLoai
								   ,[ModelDonLinhKien] = @A_ModelDonLinhKien
								   ,[QtyInput] = @A_QtyInput
								   ,[OperationNow] = @A_OperationNow
								   ,[MucDichNhap] = @A_MucDichNhap
								   ,[NguoiChiuTrachNhiem] = @A_NguoiChiuTrachNhiem
								   ,[InputDate] = @A_InputDate
								   ,[OutputDate] = @A_Shipping_Actual_Date
								   ,PlanInputDateTcard = @A_PlanInputDate
								   ,[PlanOutputDate] = @A_PlanOutputDate
								   ,[Wall_Actual_Date] = @A_Wall_Actual_Date
								   ,[Roof_Actual_Date] = @A_Roof_Actual_Date
								   ,[Seed_Actual_Date] = @A_Seed_Actual_Date
								   ,[PlatePR_Actual_Date] = @A_PlatePR_Actual_Date
								   ,[Plate_Actual_Date] = @A_Plate_Actual_Date
								   ,[AllProbe_Actual_Date] = @A_AllProbe_Actual_Date
								   ,[BG_Actual_Date] = @A_BG_Actual_Date
								   ,[Dicing_Actual_Date] = @A_Dicing_Actual_Date
								   ,[ChipIns_Actual_Date] = @A_ChipIns_Actual_Date
								   ,[Packing_Actual_Date] = @A_Packing_Actual_Date
								   ,[OQC_Actual_Date] = @A_OQC_Actual_Date
								   ,[Shipping_Actual_Date] = @A_Shipping_Actual_Date
								   ,[LeadTime] = @A_LeadTime
								   ,[DateModified] = SYSDATETIME()
								   ,[UserModified] = 'sys-update'
								   ,[DeleteFlg] = @A_DeleteFlg
								   ,[OutPutWafer] = @A_WaferOuput 
								   ,[LeadTimePlan] = isnull(@A_LeadTime_Plan,0)
								   ,[Wall_Plan_Date] = @A_Wall_Plan_Date
								   ,[Roof_Plan_Date] = @A_Roof_Plan_Date
								   ,[Seed_Plan_Date] = @A_Seed_Plan_Date
								   ,[PlatePR_Plan_Date] = @A_PlatePR_Plan_Date
								   ,[Plate_Plan_Date] = @A_Plate_Plan_Date
								   ,[PreProbe_Plan_Date] = @A_PreProbe_Plan_Date
								   ,[PreDicing_Plan_Date] = @A_PreDicing_Plan_Date
								   ,[AllProbe_Plan_Date] = @A_AllProbe_Plan_Date
								   ,[BG_Plan_Date] = @A_BG_Plan_Date
								   ,[Dicing_Plan_Date] = @A_Dicing_Plan_Date
								   ,[ChipIns_Plan_Date] = @A_ChipIns_Plan_Date
								   ,[Packing_Plan_Date] = @A_Packing_Plan_Date
								   ,[OQC_Plan_Date] = @A_OQC_Plan_Date
								   ,[Shipping_Plan_Date] = @A_Shipping_Plan_Date
								   ,[PlanInputDate] = @A_PlanInputDate
							  WHERE [LotNo] = @A_LotNo
						END
					  ELSE
					     BEGIN
								INSERT INTO [dbo].[TINH_HINH_SAN_XUAT_SAMPLE]
												([Year]
												,[Month]
												,[MucDoKhanCap]
												,[Model]
												,[Code]
												,[PhanLoai]
												,[ModelDonLinhKien]
												,[LotNo]
												,[QtyInput]
												,[QtyNG]
												,[OperationNow]
												,[MucDichNhap]
												,[GhiChu]
												,[NguoiChiuTrachNhiem]
												,[InputDate]
												,[OutputDate]
												,[PlanInputDateTcard]
												,[PlanOutputDate]
												,[Wall_Plan_Date]
												,[Wall_Actual_Date]
												,[Roof_Plan_Date]
												,[Roof_Actual_Date]
												,[Seed_Plan_Date]
												,[Seed_Actual_Date]
												,[PlatePR_Plan_Date]
												,[PlatePR_Actual_Date]
												,[Plate_Plan_Date]
												,[Plate_Actual_Date]
												,[PreProbe_Plan_Date]
												,[PreProbe_Actual_Date]
												,[PreDicing_Plan_Date]
												,[PreDicing_Actual_Date]
												,[AllProbe_Plan_Date]
												,[AllProbe_Actual_Date]
												,[BG_Plan_Date]
												,[BG_Actual_Date]
												,[Dicing_Plan_Date]
												,[Dicing_Actual_Date]
												,[ChipIns_Plan_Date]
												,[ChipIns_Actual_Date]
												,[Packing_Plan_Date]
												,[Packing_Actual_Date]
												,[OQC_Plan_Date]
												,[OQC_Actual_Date]
												,[Shipping_Plan_Date]
												,[Shipping_Actual_Date]
												,[LeadTime]
												,[DateCreated]
												,[DateModified]
												,[UserCreated]
												,[UserModified]
												,[DeleteFlg]
												,[OutPutWafer]
												,[LeadTimePlan]
												,PlanInputDate)
									 VALUES
									       (@A_Year
									       ,@A_Month
									       ,0
									       ,@A_Model
									       ,@A_Code
									       ,@A_PhanLoai
									       ,@A_ModelDonLinhKien
									       ,@A_LotNo
									       ,@A_QtyInput
									       ,0
									       ,@A_OperationNow
									       ,@A_MucDichNhap
									       ,@A_GhiChu
									       ,@A_NguoiChiuTrachNhiem
									       ,@A_InputDate
									       ,@A_OutputDate
									       ,@A_PlanInputDate
									       ,@A_PlanOutputDate
									       ,@A_Wall_Plan_Date
									       ,@A_Wall_Actual_Date
									       ,@A_Roof_Plan_Date
									       ,@A_Roof_Actual_Date
									       ,@A_Seed_Plan_Date
									       ,@A_Seed_Actual_Date
									       ,@A_PlatePR_Plan_Date
									       ,@A_PlatePR_Actual_Date
									       ,@A_Plate_Plan_Date
									       ,@A_Plate_Actual_Date
									       ,@A_PreProbe_Plan_Date
									       ,@A_PreProbe_Actual_Date
									       ,@A_PreDicing_Plan_Date
									       ,@A_PreDicing_Actual_Date
									       ,@A_AllProbe_Plan_Date
									       ,@A_AllProbe_Actual_Date
									       ,@A_BG_Plan_Date
									       ,@A_BG_Actual_Date
									       ,@A_Dicing_Plan_Date
									       ,@A_Dicing_Actual_Date
									       ,@A_ChipIns_Plan_Date
									       ,@A_ChipIns_Actual_Date
									       ,@A_Packing_Plan_Date
									       ,@A_Packing_Actual_Date
									       ,@A_OQC_Plan_Date
									       ,@A_OQC_Actual_Date
									       ,@A_Shipping_Plan_Date
									       ,@A_Shipping_Actual_Date
									       ,@A_LeadTime
									       ,SYSDATETIME()
									       ,SYSDATETIME()
									       ,'sys'
									       ,''
										   ,@A_DeleteFlg
										   ,@A_WaferOuput
										   ,isnull(@A_LeadTime_Plan,0)
										   ,@A_PlanInputDate)

						 END
   

				   FETCH NEXT FROM OutPutLotListCusor
				   INTO @A_Model,@A_PhanLoai,@A_ModelDonLinhKien,@A_LotNo,@A_DeleteFlg
			 END

  CLOSE OutPutLotListCusor
  DEALLOCATE OutPutLotListCusor
   
   -- insert comment 
   INSERT INTO [dbo].[DELAY_COMMENT_SAMPLE]
           ([Wall]
           ,[Roof]
           ,[Seed]
           ,[PlatePR]
           ,[Plate]
           ,[PreProbe]
           ,[PreDicing]
           ,[AllProbe]
           ,[BG]
           ,[Dicing]
           ,[ChipIns]
           ,[Packing]
           ,[OQC]
           ,[DateCreated]
           ,[UserCreated]
           ,[MaTinhHinhSX]
           ,[Shipping])
     SELECT 
            ''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,SYSDATETIME()
           ,'SYS'
           ,Id
           ,''
	FROM  [TINH_HINH_SAN_XUAT_SAMPLE]
	WHERE Id not in (select [MaTinhHinhSX] from [dbo].[DELAY_COMMENT_SAMPLE])

END
END TRY
	BEGIN CATCH
END CATCH


