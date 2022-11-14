USE [master]
GO
/****** Object:  Database [HRMSDB2]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE DATABASE [HRMSDB2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HRMSDB2', FILENAME = N'D:\DATA_SQL\MSSQL15.MSSQLSERVER\MSSQL\DATA\HRMSDB2.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HRMSDB2_log', FILENAME = N'D:\DATA_SQL\MSSQL15.MSSQLSERVER\MSSQL\DATA\HRMSDB2_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [HRMSDB2] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HRMSDB2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HRMSDB2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HRMSDB2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HRMSDB2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HRMSDB2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HRMSDB2] SET ARITHABORT OFF 
GO
ALTER DATABASE [HRMSDB2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HRMSDB2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HRMSDB2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HRMSDB2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HRMSDB2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HRMSDB2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HRMSDB2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HRMSDB2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HRMSDB2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HRMSDB2] SET  ENABLE_BROKER 
GO
ALTER DATABASE [HRMSDB2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HRMSDB2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HRMSDB2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HRMSDB2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HRMSDB2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HRMSDB2] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [HRMSDB2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HRMSDB2] SET RECOVERY FULL 
GO
ALTER DATABASE [HRMSDB2] SET  MULTI_USER 
GO
ALTER DATABASE [HRMSDB2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HRMSDB2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HRMSDB2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HRMSDB2] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [HRMSDB2] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [HRMSDB2] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [HRMSDB2] SET QUERY_STORE = OFF
GO
USE [HRMSDB2]
GO
/****** Object:  User [SMT2]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE USER [SMT2] FOR LOGIN [SMT2] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  UserDefinedTableType [dbo].[DANG_KY_OT_NVIEN_TYPE]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE TYPE [dbo].[DANG_KY_OT_NVIEN_TYPE] AS TABLE(
	[NgayOT] [nvarchar](50) NULL,
	[MaNV] [nvarchar](50) NULL,
	[DM_NgayLViec] [nvarchar](50) NULL,
	[Approve] [nvarchar](50) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[DATA_RESULT_HRMS_BIOSTAR]    Script Date: 2022-05-16 4:05:40 PM ******/
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
/****** Object:  UserDefinedTableType [dbo].[NHANVIEN_CALAMVIEC]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE TYPE [dbo].[NHANVIEN_CALAMVIEC] AS TABLE(
	[MaNV] [nvarchar](50) NULL,
	[Danhmuc_CaLviec] [nvarchar](50) NULL,
	[BatDau_TheoCa] [nvarchar](50) NULL,
	[KetThuc_TheoCa] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL
)
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APP_ROLE]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APP_ROLE](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[NormalizedName] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Description] [nvarchar](250) NULL,
 CONSTRAINT [PK_APP_ROLE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APP_ROLE_CLAIM]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APP_ROLE_CLAIM](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_APP_ROLE_CLAIM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APP_USER]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APP_USER](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[NormalizedUserName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[NormalizedEmail] [nvarchar](max) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[FullName] [nvarchar](250) NULL,
	[BirthDay] [datetime2](7) NULL,
	[Avatar] [nvarchar](max) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[Status] [int] NOT NULL,
	[ShowPass] [nvarchar](max) NULL,
 CONSTRAINT [PK_APP_USER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APP_USER_CLAIM]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APP_USER_CLAIM](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_APP_USER_CLAIM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APP_USER_LOGIN]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APP_USER_LOGIN](
	[UserId] [uniqueidentifier] NOT NULL,
	[LoginProvider] [nvarchar](max) NULL,
	[ProviderKey] [nvarchar](max) NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
 CONSTRAINT [PK_APP_USER_LOGIN] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APP_USER_ROLE]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APP_USER_ROLE](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_APP_USER_ROLE] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APP_USER_TOKEN]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APP_USER_TOKEN](
	[UserId] [uniqueidentifier] NOT NULL,
	[LoginProvider] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_APP_USER_TOKEN] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ATTENDANCE_OVERTIME]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ATTENDANCE_OVERTIME](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CaLviec] [int] NOT NULL,
	[MaAttendence] [bigint] NOT NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[Value] [float] NULL,
 CONSTRAINT [PK_ATTENDANCE_OVERTIME] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ATTENDANCE_RECORD]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ATTENDANCE_RECORD](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MaNV] [nvarchar](50) NULL,
	[Time_Check] [nvarchar](50) NULL,
	[Working_Status] [nvarchar](20) NULL,
	[EL_LC] [float] NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[IsLock] [nvarchar](20) NULL,
 CONSTRAINT [PK_ATTENDANCE_RECORD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BOPHAN]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BOPHAN](
	[Id] [nvarchar](50) NOT NULL,
	[TenBoPhan] [nvarchar](500) NULL,
 CONSTRAINT [PK_BOPHAN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CA_LVIEC]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CA_LVIEC](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Danhmuc_CaLviec] [nvarchar](50) NULL,
	[DM_NgayLViec] [nvarchar](50) NULL,
	[TenCa] [nvarchar](100) NULL,
	[Time_BatDau] [nvarchar](50) NULL,
	[Time_BatDau2] [nvarchar](50) NULL,
	[Time_KetThuc] [nvarchar](50) NULL,
	[Time_KetThuc2] [nvarchar](50) NULL,
	[HeSo_OT] [real] NOT NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_CA_LVIEC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CHAM_CONG_LOG]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CHAM_CONG_LOG](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Ngay_ChamCong] [nvarchar](50) NULL,
	[ID_NV] [nvarchar](50) NULL,
	[Ten_NV] [nvarchar](96) NULL,
	[Last_Out_Time] [nvarchar](50) NULL,
	[FirstIn] [varchar](50) NULL,
	[LastOut] [varchar](50) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[FirstIn_Time] [nvarchar](50) NULL,
	[Department] [nvarchar](50) NULL,
	[FirstIn_Time_Update] [nvarchar](50) NULL,
	[Last_Out_Time_Update] [nvarchar](50) NULL,
 CONSTRAINT [PK_CHAM_CONG_LOG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CHUNG_CHI]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CHUNG_CHI](
	[Id] [nvarchar](50) NOT NULL,
	[TenChungChi] [nvarchar](250) NULL,
	[LoaiChungChi] [int] NULL,
	[DateCreated] [nvarchar](max) NULL,
	[DateModified] [nvarchar](max) NULL,
	[UserCreated] [nvarchar](max) NULL,
	[UserModified] [nvarchar](max) NULL,
 CONSTRAINT [PK_CHUNG_CHI] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DANGKY_CHAMCONG_CHITIET]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DANGKY_CHAMCONG_CHITIET](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenChiTiet] [nvarchar](150) NULL,
	[PhanLoaiDM] [int] NULL,
	[KyHieuChamCong] [nvarchar](20) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_DANGKY_CHAMCONG_CHITIET] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DANGKY_CHAMCONG_DACBIET]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DANGKY_CHAMCONG_DACBIET](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaNV] [nvarchar](50) NULL,
	[MaChamCong_ChiTiet] [int] NULL,
	[NgayBatDau] [nvarchar](50) NULL,
	[NgayKetThuc] [nvarchar](50) NULL,
	[NoiDung] [nvarchar](300) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[Approve] [nvarchar](50) NULL,
 CONSTRAINT [PK_DANGKY_CHAMCONG_DACBIET] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DANGKY_OT_NHANVIEN]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DANGKY_OT_NHANVIEN](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NgayOT] [nvarchar](50) NULL,
	[MaNV] [nvarchar](50) NULL,
	[DM_NgayLViec] [nvarchar](50) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[Approve] [nvarchar](50) NULL,
	[Passed] [nvarchar](2) NULL,
 CONSTRAINT [PK_DANGKY_OT_NHANVIEN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DC_CHAM_CONG]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DC_CHAM_CONG](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaNV] [nvarchar](50) NULL,
	[NgayCanDieuChinh_From] [nvarchar](50) NULL,
	[NgayCanDieuChinh_To] [nvarchar](50) NULL,
	[NoiDungDC] [nvarchar](300) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[DM_DieuChinhCong] [int] NULL,
	[GiaTriBoXung] [float] NULL,
	[TrangThaiChiTra] [nvarchar](20) NULL,
 CONSTRAINT [PK_DC_CHAM_CONG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DM_CA_LVIEC]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DM_CA_LVIEC](
	[Id] [nvarchar](50) NOT NULL,
	[TenCaLamViec] [nvarchar](100) NULL,
	[MaTruSo] [nvarchar](50) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_DM_CA_LVIEC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DM_DANGKY_CHAMCONG]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DM_DANGKY_CHAMCONG](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TieuDe] [nvarchar](250) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_DM_DANGKY_CHAMCONG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DM_DIEUCHINH_CHAMCONG]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DM_DIEUCHINH_CHAMCONG](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TieuDe] [nvarchar](250) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_DM_DIEUCHINH_CHAMCONG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DM_NGAY_LAMVIEC]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DM_NGAY_LAMVIEC](
	[Id] [nvarchar](50) NOT NULL,
	[Ten_NgayLV] [nvarchar](100) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_DM_NGAY_LAMVIEC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FUNCTION]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FUNCTION](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[URL] [nvarchar](250) NOT NULL,
	[ParentId] [nvarchar](128) NULL,
	[IconCss] [nvarchar](max) NULL,
	[SortOrder] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_FUNCTION] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_BHXH]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_BHXH](
	[Id] [nvarchar](50) NOT NULL,
	[MaNV] [nvarchar](50) NULL,
	[NgayThamGia] [nvarchar](50) NULL,
	[NgayKetThuc] [nvarchar](50) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_HR_BHXH] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_BO_PHAN_DETAIL]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_BO_PHAN_DETAIL](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenBoPhanChiTiet] [nvarchar](500) NULL,
	[MaBoPhan] [nvarchar](50) NULL,
	[DateCreated] [nvarchar](max) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_HR_BO_PHAN_DETAIL] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_CHEDOBH]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_CHEDOBH](
	[Id] [nvarchar](50) NOT NULL,
	[TenCheDo] [nvarchar](250) NULL,
 CONSTRAINT [PK_HR_CHEDOBH] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_CHUCDANH]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_CHUCDANH](
	[Id] [nvarchar](50) NOT NULL,
	[TenChucDanh] [nvarchar](50) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_HR_CHUCDANH] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_CHUNGCHI_NHANVIEN]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_CHUNGCHI_NHANVIEN](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaNV] [nvarchar](50) NULL,
	[MaChungChi] [nvarchar](50) NULL,
	[NoiCap] [nvarchar](max) NULL,
	[NgayCap] [nvarchar](50) NULL,
	[NgayHetHan] [nvarchar](50) NULL,
	[ChuyenMon] [nvarchar](250) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_HR_CHUNGCHI_NHANVIEN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_HOPDONG]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_HOPDONG](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaHD] [nvarchar](50) NULL,
	[MaNV] [nvarchar](50) NULL,
	[TenHD] [nvarchar](500) NULL,
	[LoaiHD] [int] NULL,
	[NgayTao] [nvarchar](50) NULL,
	[NgayKy] [nvarchar](50) NULL,
	[NgayHieuLuc] [nvarchar](50) NULL,
	[NgayHetHieuLuc] [nvarchar](50) NULL,
	[ChucDanh] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[IsDelete] [nvarchar](10) NULL,
	[DayNumberNoti] [int] NOT NULL,
 CONSTRAINT [PK_HR_HOPDONG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_KEKHAIBAOHIEM]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_KEKHAIBAOHIEM](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaNV] [nvarchar](50) NULL,
	[CheDoBH] [nvarchar](50) NULL,
	[NgayBatDau] [nvarchar](50) NULL,
	[NgayKetThuc] [nvarchar](50) NULL,
	[NgayThanhToan] [nvarchar](50) NULL,
	[SoTienThanhToan] [float] NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_HR_KEKHAIBAOHIEM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_LOAIHOPDONG]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_LOAIHOPDONG](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenLoaiHD] [nvarchar](500) NULL,
	[ShortName] [nvarchar](50) NULL,
 CONSTRAINT [PK_HR_LOAIHOPDONG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_NHANVIEN]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_NHANVIEN](
	[Id] [nvarchar](50) NOT NULL,
	[TenNV] [nvarchar](250) NULL,
	[MaChucDanh] [nvarchar](50) NULL,
	[MaBoPhan] [nvarchar](50) NULL,
	[MaBoPhanChiTiet] [int] NULL,
	[GioiTinh] [nvarchar](20) NULL,
	[NgaySinh] [nvarchar](50) NULL,
	[NoiSinh] [nvarchar](250) NULL,
	[TinhTrangHonNhan] [nvarchar](50) NULL,
	[DanToc] [nvarchar](50) NULL,
	[TonGiao] [nvarchar](50) NULL,
	[DiaChiThuongTru] [nvarchar](250) NULL,
	[SoDienThoai] [nvarchar](50) NULL,
	[SoDienThoaiNguoiThan] [nvarchar](50) NULL,
	[QuanHeNguoiThan] [nvarchar](100) NULL,
	[CMTND] [nvarchar](50) NULL,
	[NgayCapCMTND] [nvarchar](50) NULL,
	[NoiCapCMTND] [nvarchar](250) NULL,
	[SoTaiKhoanNH] [nvarchar](50) NULL,
	[TenNganHang] [nvarchar](50) NULL,
	[TruongDaoTao] [nvarchar](500) NULL,
	[NgayVao] [nvarchar](50) NULL,
	[NguyenQuan] [nvarchar](250) NULL,
	[DChiHienTai] [nvarchar](250) NULL,
	[KyLuatLD] [nvarchar](500) NULL,
	[MaBHXH] [nvarchar](50) NULL,
	[MaSoThue] [nvarchar](50) NULL,
	[SoNguoiGiamTru] [int] NOT NULL,
	[Email] [nvarchar](250) NULL,
	[Note] [nvarchar](250) NULL,
	[NgayNghiViec] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[Image] [nvarchar](500) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[IsDelete] [nvarchar](10) NULL,
 CONSTRAINT [PK_HR_NHANVIEN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_PHEP_NAM]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_PHEP_NAM](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaNhanVien] [nvarchar](50) NULL,
	[SoPhepNam] [real] NOT NULL,
	[SoPhepConLai] [real] NOT NULL,
	[Year] [int] NOT NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_HR_PHEP_NAM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_QUATRINHLAMVIEC]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_QUATRINHLAMVIEC](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaNV] [nvarchar](50) NOT NULL,
	[TieuDe] [nvarchar](500) NULL,
	[Note] [nvarchar](max) NULL,
	[ThơiGianBatDau] [nvarchar](50) NULL,
	[ThoiGianKetThuc] [nvarchar](50) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_HR_QUATRINHLAMVIEC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HR_TINHTRANGHOSO]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HR_TINHTRANGHOSO](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaNV] [nvarchar](50) NULL,
	[SoYeuLyLich] [bit] NOT NULL,
	[CMTND] [bit] NOT NULL,
	[SoHoKhau] [bit] NOT NULL,
	[GiayKhaiSinh] [bit] NOT NULL,
	[BangTotNghiep] [bit] NOT NULL,
	[XacNhanDanSu] [bit] NOT NULL,
	[AnhThe] [bit] NOT NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
 CONSTRAINT [PK_HR_TINHTRANGHOSO] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KY_HIEU_CHAM_CONG]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KY_HIEU_CHAM_CONG](
	[Id] [nvarchar](20) NOT NULL,
	[GiaiThich] [nvarchar](300) NULL,
	[Heso] [float] NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_KY_HIEU_CHAM_CONG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LANGUAGE]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LANGUAGE](
	[Id] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[Resources] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_LANGUAGE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LOAICHUNGCHI]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LOAICHUNGCHI](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenLoaiChungChi] [nvarchar](250) NULL,
 CONSTRAINT [PK_LOAICHUNGCHI] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NGAY_DAC_BIET]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NGAY_DAC_BIET](
	[Id] [nvarchar](10) NOT NULL,
	[TenNgayDacBiet] [nvarchar](150) NULL,
	[KyHieuChamCong] [nvarchar](20) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_NGAY_DAC_BIET] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NGAY_LE_NAM]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NGAY_LE_NAM](
	[Id] [nvarchar](10) NOT NULL,
	[TenNgayLe] [nvarchar](150) NULL,
	[KyHieuChamCong] [nvarchar](20) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[IslastHoliday] [nvarchar](10) NULL,
 CONSTRAINT [PK_NGAY_LE_NAM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NGAY_NGHI_BU_LE_NAM]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NGAY_NGHI_BU_LE_NAM](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NoiDungNghi] [nvarchar](250) NULL,
	[KyHieuChamCong] [nvarchar](20) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[NgayNghiBu] [nvarchar](10) NULL,
 CONSTRAINT [PK_NGAY_NGHI_BU_LE_NAM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NHANVIEN_CALAMVIEC]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHANVIEN_CALAMVIEC](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaNV] [nvarchar](50) NULL,
	[Danhmuc_CaLviec] [nvarchar](50) NULL,
	[BatDau_TheoCa] [nvarchar](50) NULL,
	[KetThuc_TheoCa] [nvarchar](50) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[Approved] [nvarchar](5) NULL,
 CONSTRAINT [PK_NHANVIEN_CALAMVIEC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PERMISSION]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PERMISSION](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[FunctionId] [nvarchar](128) NOT NULL,
	[CanCreate] [bit] NOT NULL,
	[CanRead] [bit] NOT NULL,
	[CanUpdate] [bit] NOT NULL,
	[CanDelete] [bit] NOT NULL,
 CONSTRAINT [PK_PERMISSION] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SETTING_TIME_CA_LVIEC]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SETTING_TIME_CA_LVIEC](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CaLamViec] [nvarchar](50) NULL,
	[NgayBatDau] [nvarchar](50) NULL,
	[NgayKetThuc] [nvarchar](50) NULL,
	[NgayBatDauDangKy] [nvarchar](50) NULL,
	[NgayKetThucDangKy] [nvarchar](50) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
 CONSTRAINT [PK_SETTING_TIME_CA_LVIEC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SETTING_TIME_DIMUON_VESOM]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SETTING_TIME_DIMUON_VESOM](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Danhmuc_CaLviec] [nvarchar](50) NULL,
	[Time_LateCome] [nvarchar](50) NULL,
	[Time_EarlyLeave] [nvarchar](50) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_SETTING_TIME_DIMUON_VESOM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TRU_SO_LVIEC]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRU_SO_LVIEC](
	[Id] [nvarchar](50) NOT NULL,
	[TenTruSo] [nvarchar](250) NULL,
	[DiaChi] [nvarchar](250) NULL,
	[DateCreated] [nvarchar](50) NULL,
	[DateModified] [nvarchar](50) NULL,
	[UserCreated] [nvarchar](50) NULL,
	[UserModified] [nvarchar](50) NULL,
 CONSTRAINT [PK_TRU_SO_LVIEC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_ATTENDANCE_OVERTIME_CaLviec]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_ATTENDANCE_OVERTIME_CaLviec] ON [dbo].[ATTENDANCE_OVERTIME]
(
	[CaLviec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ATTENDANCE_OVERTIME_MaAttendence]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_ATTENDANCE_OVERTIME_MaAttendence] ON [dbo].[ATTENDANCE_OVERTIME]
(
	[MaAttendence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ATTENDANCE_RECORD_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_ATTENDANCE_RECORD_MaNV] ON [dbo].[ATTENDANCE_RECORD]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ATTENDANCE_RECORD_Working_Status]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_ATTENDANCE_RECORD_Working_Status] ON [dbo].[ATTENDANCE_RECORD]
(
	[Working_Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_CA_LVIEC_Danhmuc_CaLviec]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_CA_LVIEC_Danhmuc_CaLviec] ON [dbo].[CA_LVIEC]
(
	[Danhmuc_CaLviec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_CA_LVIEC_DM_NgayLViec]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_CA_LVIEC_DM_NgayLViec] ON [dbo].[CA_LVIEC]
(
	[DM_NgayLViec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CHUNG_CHI_LoaiChungChi]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_CHUNG_CHI_LoaiChungChi] ON [dbo].[CHUNG_CHI]
(
	[LoaiChungChi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DANGKY_CHAMCONG_CHITIET_KyHieuChamCong]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_DANGKY_CHAMCONG_CHITIET_KyHieuChamCong] ON [dbo].[DANGKY_CHAMCONG_CHITIET]
(
	[KyHieuChamCong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DANGKY_CHAMCONG_CHITIET_PhanLoaiDM]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_DANGKY_CHAMCONG_CHITIET_PhanLoaiDM] ON [dbo].[DANGKY_CHAMCONG_CHITIET]
(
	[PhanLoaiDM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DANGKY_CHAMCONG_DACBIET_MaChamCong_ChiTiet]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_DANGKY_CHAMCONG_DACBIET_MaChamCong_ChiTiet] ON [dbo].[DANGKY_CHAMCONG_DACBIET]
(
	[MaChamCong_ChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DANGKY_CHAMCONG_DACBIET_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_DANGKY_CHAMCONG_DACBIET_MaNV] ON [dbo].[DANGKY_CHAMCONG_DACBIET]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DANGKY_OT_NHANVIEN_DM_NgayLViec]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_DANGKY_OT_NHANVIEN_DM_NgayLViec] ON [dbo].[DANGKY_OT_NHANVIEN]
(
	[DM_NgayLViec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DANGKY_OT_NHANVIEN_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_DANGKY_OT_NHANVIEN_MaNV] ON [dbo].[DANGKY_OT_NHANVIEN]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DC_CHAM_CONG_DM_DieuChinhCong]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_DC_CHAM_CONG_DM_DieuChinhCong] ON [dbo].[DC_CHAM_CONG]
(
	[DM_DieuChinhCong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DC_CHAM_CONG_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_DC_CHAM_CONG_MaNV] ON [dbo].[DC_CHAM_CONG]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DM_CA_LVIEC_MaTruSo]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_DM_CA_LVIEC_MaTruSo] ON [dbo].[DM_CA_LVIEC]
(
	[MaTruSo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_BHXH_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_BHXH_MaNV] ON [dbo].[HR_BHXH]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_CHUNGCHI_NHANVIEN_MaChungChi]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_CHUNGCHI_NHANVIEN_MaChungChi] ON [dbo].[HR_CHUNGCHI_NHANVIEN]
(
	[MaChungChi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_CHUNGCHI_NHANVIEN_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_CHUNGCHI_NHANVIEN_MaNV] ON [dbo].[HR_CHUNGCHI_NHANVIEN]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_HR_HOPDONG_LoaiHD]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_HOPDONG_LoaiHD] ON [dbo].[HR_HOPDONG]
(
	[LoaiHD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_HOPDONG_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_HOPDONG_MaNV] ON [dbo].[HR_HOPDONG]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_KEKHAIBAOHIEM_CheDoBH]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_KEKHAIBAOHIEM_CheDoBH] ON [dbo].[HR_KEKHAIBAOHIEM]
(
	[CheDoBH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_KEKHAIBAOHIEM_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_KEKHAIBAOHIEM_MaNV] ON [dbo].[HR_KEKHAIBAOHIEM]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_NHANVIEN_MaBoPhan]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_NHANVIEN_MaBoPhan] ON [dbo].[HR_NHANVIEN]
(
	[MaBoPhan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_HR_NHANVIEN_MaBoPhanChiTiet]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_NHANVIEN_MaBoPhanChiTiet] ON [dbo].[HR_NHANVIEN]
(
	[MaBoPhanChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_NHANVIEN_MaChucDanh]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_NHANVIEN_MaChucDanh] ON [dbo].[HR_NHANVIEN]
(
	[MaChucDanh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_PHEP_NAM_MaNhanVien]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_PHEP_NAM_MaNhanVien] ON [dbo].[HR_PHEP_NAM]
(
	[MaNhanVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_QUATRINHLAMVIEC_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_QUATRINHLAMVIEC_MaNV] ON [dbo].[HR_QUATRINHLAMVIEC]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_HR_TINHTRANGHOSO_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_HR_TINHTRANGHOSO_MaNV] ON [dbo].[HR_TINHTRANGHOSO]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_NGAY_DAC_BIET_KyHieuChamCong]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_NGAY_DAC_BIET_KyHieuChamCong] ON [dbo].[NGAY_DAC_BIET]
(
	[KyHieuChamCong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_NGAY_LE_NAM_KyHieuChamCong]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_NGAY_LE_NAM_KyHieuChamCong] ON [dbo].[NGAY_LE_NAM]
(
	[KyHieuChamCong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_NGAY_NGHI_BU_LE_NAM_KyHieuChamCong]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_NGAY_NGHI_BU_LE_NAM_KyHieuChamCong] ON [dbo].[NGAY_NGHI_BU_LE_NAM]
(
	[KyHieuChamCong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_NHANVIEN_CALAMVIEC_Danhmuc_CaLviec]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_NHANVIEN_CALAMVIEC_Danhmuc_CaLviec] ON [dbo].[NHANVIEN_CALAMVIEC]
(
	[Danhmuc_CaLviec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_NHANVIEN_CALAMVIEC_MaNV]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_NHANVIEN_CALAMVIEC_MaNV] ON [dbo].[NHANVIEN_CALAMVIEC]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_PERMISSION_FunctionId]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_PERMISSION_FunctionId] ON [dbo].[PERMISSION]
(
	[FunctionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PERMISSION_RoleId]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_PERMISSION_RoleId] ON [dbo].[PERMISSION]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SETTING_TIME_CA_LVIEC_CaLamViec]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_SETTING_TIME_CA_LVIEC_CaLamViec] ON [dbo].[SETTING_TIME_CA_LVIEC]
(
	[CaLamViec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SETTING_TIME_DIMUON_VESOM_Danhmuc_CaLviec]    Script Date: 2022-05-16 4:05:40 PM ******/
CREATE NONCLUSTERED INDEX [IX_SETTING_TIME_DIMUON_VESOM_Danhmuc_CaLviec] ON [dbo].[SETTING_TIME_DIMUON_VESOM]
(
	[Danhmuc_CaLviec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HR_HOPDONG] ADD  DEFAULT ((30)) FOR [DayNumberNoti]
GO
ALTER TABLE [dbo].[ATTENDANCE_OVERTIME]  WITH CHECK ADD  CONSTRAINT [FK_ATTENDANCE_OVERTIME_ATTENDANCE_RECORD_MaAttendence] FOREIGN KEY([MaAttendence])
REFERENCES [dbo].[ATTENDANCE_RECORD] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ATTENDANCE_OVERTIME] CHECK CONSTRAINT [FK_ATTENDANCE_OVERTIME_ATTENDANCE_RECORD_MaAttendence]
GO
ALTER TABLE [dbo].[ATTENDANCE_OVERTIME]  WITH CHECK ADD  CONSTRAINT [FK_ATTENDANCE_OVERTIME_CA_LVIEC_CaLviec] FOREIGN KEY([CaLviec])
REFERENCES [dbo].[CA_LVIEC] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ATTENDANCE_OVERTIME] CHECK CONSTRAINT [FK_ATTENDANCE_OVERTIME_CA_LVIEC_CaLviec]
GO
ALTER TABLE [dbo].[ATTENDANCE_RECORD]  WITH CHECK ADD  CONSTRAINT [FK_ATTENDANCE_RECORD_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ATTENDANCE_RECORD] CHECK CONSTRAINT [FK_ATTENDANCE_RECORD_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[ATTENDANCE_RECORD]  WITH CHECK ADD  CONSTRAINT [FK_ATTENDANCE_RECORD_KY_HIEU_CHAM_CONG_Working_Status] FOREIGN KEY([Working_Status])
REFERENCES [dbo].[KY_HIEU_CHAM_CONG] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ATTENDANCE_RECORD] CHECK CONSTRAINT [FK_ATTENDANCE_RECORD_KY_HIEU_CHAM_CONG_Working_Status]
GO
ALTER TABLE [dbo].[CA_LVIEC]  WITH CHECK ADD  CONSTRAINT [FK_CA_LVIEC_DM_CA_LVIEC_Danhmuc_CaLviec] FOREIGN KEY([Danhmuc_CaLviec])
REFERENCES [dbo].[DM_CA_LVIEC] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[CA_LVIEC] CHECK CONSTRAINT [FK_CA_LVIEC_DM_CA_LVIEC_Danhmuc_CaLviec]
GO
ALTER TABLE [dbo].[CA_LVIEC]  WITH CHECK ADD  CONSTRAINT [FK_CA_LVIEC_DM_NGAY_LAMVIEC_DM_NgayLViec] FOREIGN KEY([DM_NgayLViec])
REFERENCES [dbo].[DM_NGAY_LAMVIEC] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[CA_LVIEC] CHECK CONSTRAINT [FK_CA_LVIEC_DM_NGAY_LAMVIEC_DM_NgayLViec]
GO
ALTER TABLE [dbo].[CHUNG_CHI]  WITH CHECK ADD  CONSTRAINT [FK_CHUNG_CHI_LOAICHUNGCHI_LoaiChungChi] FOREIGN KEY([LoaiChungChi])
REFERENCES [dbo].[LOAICHUNGCHI] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[CHUNG_CHI] CHECK CONSTRAINT [FK_CHUNG_CHI_LOAICHUNGCHI_LoaiChungChi]
GO
ALTER TABLE [dbo].[DANGKY_CHAMCONG_CHITIET]  WITH CHECK ADD  CONSTRAINT [FK_DANGKY_CHAMCONG_CHITIET_DM_DANGKY_CHAMCONG_PhanLoaiDM] FOREIGN KEY([PhanLoaiDM])
REFERENCES [dbo].[DM_DANGKY_CHAMCONG] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DANGKY_CHAMCONG_CHITIET] CHECK CONSTRAINT [FK_DANGKY_CHAMCONG_CHITIET_DM_DANGKY_CHAMCONG_PhanLoaiDM]
GO
ALTER TABLE [dbo].[DANGKY_CHAMCONG_CHITIET]  WITH CHECK ADD  CONSTRAINT [FK_DANGKY_CHAMCONG_CHITIET_KY_HIEU_CHAM_CONG_KyHieuChamCong] FOREIGN KEY([KyHieuChamCong])
REFERENCES [dbo].[KY_HIEU_CHAM_CONG] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[DANGKY_CHAMCONG_CHITIET] CHECK CONSTRAINT [FK_DANGKY_CHAMCONG_CHITIET_KY_HIEU_CHAM_CONG_KyHieuChamCong]
GO
ALTER TABLE [dbo].[DANGKY_CHAMCONG_DACBIET]  WITH CHECK ADD  CONSTRAINT [FK_DANGKY_CHAMCONG_DACBIET_DANGKY_CHAMCONG_CHITIET_MaChamCong_ChiTiet] FOREIGN KEY([MaChamCong_ChiTiet])
REFERENCES [dbo].[DANGKY_CHAMCONG_CHITIET] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[DANGKY_CHAMCONG_DACBIET] CHECK CONSTRAINT [FK_DANGKY_CHAMCONG_DACBIET_DANGKY_CHAMCONG_CHITIET_MaChamCong_ChiTiet]
GO
ALTER TABLE [dbo].[DANGKY_CHAMCONG_DACBIET]  WITH CHECK ADD  CONSTRAINT [FK_DANGKY_CHAMCONG_DACBIET_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DANGKY_CHAMCONG_DACBIET] CHECK CONSTRAINT [FK_DANGKY_CHAMCONG_DACBIET_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[DANGKY_OT_NHANVIEN]  WITH CHECK ADD  CONSTRAINT [FK_DANGKY_OT_NHANVIEN_DM_NGAY_LAMVIEC_DM_NgayLViec] FOREIGN KEY([DM_NgayLViec])
REFERENCES [dbo].[DM_NGAY_LAMVIEC] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[DANGKY_OT_NHANVIEN] CHECK CONSTRAINT [FK_DANGKY_OT_NHANVIEN_DM_NGAY_LAMVIEC_DM_NgayLViec]
GO
ALTER TABLE [dbo].[DANGKY_OT_NHANVIEN]  WITH CHECK ADD  CONSTRAINT [FK_DANGKY_OT_NHANVIEN_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DANGKY_OT_NHANVIEN] CHECK CONSTRAINT [FK_DANGKY_OT_NHANVIEN_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[DC_CHAM_CONG]  WITH CHECK ADD  CONSTRAINT [FK_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONG_DM_DieuChinhCong] FOREIGN KEY([DM_DieuChinhCong])
REFERENCES [dbo].[DM_DIEUCHINH_CHAMCONG] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[DC_CHAM_CONG] CHECK CONSTRAINT [FK_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONG_DM_DieuChinhCong]
GO
ALTER TABLE [dbo].[DC_CHAM_CONG]  WITH CHECK ADD  CONSTRAINT [FK_DC_CHAM_CONG_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DC_CHAM_CONG] CHECK CONSTRAINT [FK_DC_CHAM_CONG_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[DM_CA_LVIEC]  WITH CHECK ADD  CONSTRAINT [FK_DM_CA_LVIEC_TRU_SO_LVIEC_MaTruSo] FOREIGN KEY([MaTruSo])
REFERENCES [dbo].[TRU_SO_LVIEC] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[DM_CA_LVIEC] CHECK CONSTRAINT [FK_DM_CA_LVIEC_TRU_SO_LVIEC_MaTruSo]
GO
ALTER TABLE [dbo].[HR_BHXH]  WITH CHECK ADD  CONSTRAINT [FK_HR_BHXH_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HR_BHXH] CHECK CONSTRAINT [FK_HR_BHXH_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[HR_CHUNGCHI_NHANVIEN]  WITH CHECK ADD  CONSTRAINT [FK_HR_CHUNGCHI_NHANVIEN_CHUNG_CHI_MaChungChi] FOREIGN KEY([MaChungChi])
REFERENCES [dbo].[CHUNG_CHI] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[HR_CHUNGCHI_NHANVIEN] CHECK CONSTRAINT [FK_HR_CHUNGCHI_NHANVIEN_CHUNG_CHI_MaChungChi]
GO
ALTER TABLE [dbo].[HR_CHUNGCHI_NHANVIEN]  WITH CHECK ADD  CONSTRAINT [FK_HR_CHUNGCHI_NHANVIEN_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HR_CHUNGCHI_NHANVIEN] CHECK CONSTRAINT [FK_HR_CHUNGCHI_NHANVIEN_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[HR_HOPDONG]  WITH CHECK ADD  CONSTRAINT [FK_HR_HOPDONG_HR_LOAIHOPDONG_LoaiHD] FOREIGN KEY([LoaiHD])
REFERENCES [dbo].[HR_LOAIHOPDONG] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[HR_HOPDONG] CHECK CONSTRAINT [FK_HR_HOPDONG_HR_LOAIHOPDONG_LoaiHD]
GO
ALTER TABLE [dbo].[HR_HOPDONG]  WITH CHECK ADD  CONSTRAINT [FK_HR_HOPDONG_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HR_HOPDONG] CHECK CONSTRAINT [FK_HR_HOPDONG_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[HR_KEKHAIBAOHIEM]  WITH CHECK ADD  CONSTRAINT [FK_HR_KEKHAIBAOHIEM_HR_CHEDOBH_CheDoBH] FOREIGN KEY([CheDoBH])
REFERENCES [dbo].[HR_CHEDOBH] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[HR_KEKHAIBAOHIEM] CHECK CONSTRAINT [FK_HR_KEKHAIBAOHIEM_HR_CHEDOBH_CheDoBH]
GO
ALTER TABLE [dbo].[HR_KEKHAIBAOHIEM]  WITH CHECK ADD  CONSTRAINT [FK_HR_KEKHAIBAOHIEM_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HR_KEKHAIBAOHIEM] CHECK CONSTRAINT [FK_HR_KEKHAIBAOHIEM_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[HR_NHANVIEN]  WITH CHECK ADD  CONSTRAINT [FK_HR_NHANVIEN_BOPHAN_MaBoPhan] FOREIGN KEY([MaBoPhan])
REFERENCES [dbo].[BOPHAN] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[HR_NHANVIEN] CHECK CONSTRAINT [FK_HR_NHANVIEN_BOPHAN_MaBoPhan]
GO
ALTER TABLE [dbo].[HR_NHANVIEN]  WITH CHECK ADD  CONSTRAINT [FK_HR_NHANVIEN_HR_BO_PHAN_DETAIL_MaBoPhanChiTiet] FOREIGN KEY([MaBoPhanChiTiet])
REFERENCES [dbo].[HR_BO_PHAN_DETAIL] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[HR_NHANVIEN] CHECK CONSTRAINT [FK_HR_NHANVIEN_HR_BO_PHAN_DETAIL_MaBoPhanChiTiet]
GO
ALTER TABLE [dbo].[HR_NHANVIEN]  WITH CHECK ADD  CONSTRAINT [FK_HR_NHANVIEN_HR_CHUCDANH_MaChucDanh] FOREIGN KEY([MaChucDanh])
REFERENCES [dbo].[HR_CHUCDANH] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[HR_NHANVIEN] CHECK CONSTRAINT [FK_HR_NHANVIEN_HR_CHUCDANH_MaChucDanh]
GO
ALTER TABLE [dbo].[HR_PHEP_NAM]  WITH CHECK ADD  CONSTRAINT [FK_HR_PHEP_NAM_HR_NHANVIEN_MaNhanVien] FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HR_PHEP_NAM] CHECK CONSTRAINT [FK_HR_PHEP_NAM_HR_NHANVIEN_MaNhanVien]
GO
ALTER TABLE [dbo].[HR_QUATRINHLAMVIEC]  WITH CHECK ADD  CONSTRAINT [FK_HR_QUATRINHLAMVIEC_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HR_QUATRINHLAMVIEC] CHECK CONSTRAINT [FK_HR_QUATRINHLAMVIEC_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[HR_TINHTRANGHOSO]  WITH CHECK ADD  CONSTRAINT [FK_HR_TINHTRANGHOSO_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HR_TINHTRANGHOSO] CHECK CONSTRAINT [FK_HR_TINHTRANGHOSO_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[NGAY_DAC_BIET]  WITH CHECK ADD  CONSTRAINT [FK_NGAY_DAC_BIET_KY_HIEU_CHAM_CONG_KyHieuChamCong] FOREIGN KEY([KyHieuChamCong])
REFERENCES [dbo].[KY_HIEU_CHAM_CONG] ([Id])
GO
ALTER TABLE [dbo].[NGAY_DAC_BIET] CHECK CONSTRAINT [FK_NGAY_DAC_BIET_KY_HIEU_CHAM_CONG_KyHieuChamCong]
GO
ALTER TABLE [dbo].[NGAY_LE_NAM]  WITH CHECK ADD  CONSTRAINT [FK_NGAY_LE_NAM_KY_HIEU_CHAM_CONG_KyHieuChamCong] FOREIGN KEY([KyHieuChamCong])
REFERENCES [dbo].[KY_HIEU_CHAM_CONG] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[NGAY_LE_NAM] CHECK CONSTRAINT [FK_NGAY_LE_NAM_KY_HIEU_CHAM_CONG_KyHieuChamCong]
GO
ALTER TABLE [dbo].[NGAY_NGHI_BU_LE_NAM]  WITH CHECK ADD  CONSTRAINT [FK_NGAY_NGHI_BU_LE_NAM_KY_HIEU_CHAM_CONG_KyHieuChamCong] FOREIGN KEY([KyHieuChamCong])
REFERENCES [dbo].[KY_HIEU_CHAM_CONG] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[NGAY_NGHI_BU_LE_NAM] CHECK CONSTRAINT [FK_NGAY_NGHI_BU_LE_NAM_KY_HIEU_CHAM_CONG_KyHieuChamCong]
GO
ALTER TABLE [dbo].[NHANVIEN_CALAMVIEC]  WITH CHECK ADD  CONSTRAINT [FK_NHANVIEN_CALAMVIEC_DM_CA_LVIEC_Danhmuc_CaLviec] FOREIGN KEY([Danhmuc_CaLviec])
REFERENCES [dbo].[DM_CA_LVIEC] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[NHANVIEN_CALAMVIEC] CHECK CONSTRAINT [FK_NHANVIEN_CALAMVIEC_DM_CA_LVIEC_Danhmuc_CaLviec]
GO
ALTER TABLE [dbo].[NHANVIEN_CALAMVIEC]  WITH CHECK ADD  CONSTRAINT [FK_NHANVIEN_CALAMVIEC_HR_NHANVIEN_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[HR_NHANVIEN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NHANVIEN_CALAMVIEC] CHECK CONSTRAINT [FK_NHANVIEN_CALAMVIEC_HR_NHANVIEN_MaNV]
GO
ALTER TABLE [dbo].[PERMISSION]  WITH CHECK ADD  CONSTRAINT [FK_PERMISSION_APP_ROLE_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[APP_ROLE] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PERMISSION] CHECK CONSTRAINT [FK_PERMISSION_APP_ROLE_RoleId]
GO
ALTER TABLE [dbo].[PERMISSION]  WITH CHECK ADD  CONSTRAINT [FK_PERMISSION_FUNCTION_FunctionId] FOREIGN KEY([FunctionId])
REFERENCES [dbo].[FUNCTION] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PERMISSION] CHECK CONSTRAINT [FK_PERMISSION_FUNCTION_FunctionId]
GO
ALTER TABLE [dbo].[SETTING_TIME_CA_LVIEC]  WITH CHECK ADD  CONSTRAINT [FK_SETTING_TIME_CA_LVIEC_DM_CA_LVIEC_CaLamViec] FOREIGN KEY([CaLamViec])
REFERENCES [dbo].[DM_CA_LVIEC] ([Id])
GO
ALTER TABLE [dbo].[SETTING_TIME_CA_LVIEC] CHECK CONSTRAINT [FK_SETTING_TIME_CA_LVIEC_DM_CA_LVIEC_CaLamViec]
GO
ALTER TABLE [dbo].[SETTING_TIME_DIMUON_VESOM]  WITH CHECK ADD  CONSTRAINT [FK_SETTING_TIME_DIMUON_VESOM_DM_CA_LVIEC_Danhmuc_CaLviec] FOREIGN KEY([Danhmuc_CaLviec])
REFERENCES [dbo].[DM_CA_LVIEC] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[SETTING_TIME_DIMUON_VESOM] CHECK CONSTRAINT [FK_SETTING_TIME_DIMUON_VESOM_DM_CA_LVIEC_Danhmuc_CaLviec]
GO
/****** Object:  StoredProcedure [dbo].[PKG_BUSINESS@GET_INFO_NHANVIEN_CHAMCONG]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [dbo].[PKG_BUSINESS@GET_INFO_NHANVIEN_CHAMCONG](
@A_DATE_TIME NVARCHAR(50),
@N_RETURN			int				OUTPUT,
@V_RETURN			NVARCHAR(4000)	OUTPUT
)
AS
BEGIN TRY
		BEGIN
		    SET NOCOUNT OFF;  

			DECLARE @BEGIN_TIME NVARCHAR(50)
			DECLARE @END_TIME NVARCHAR(50)

			SELECT @BEGIN_TIME = FORMAT(DATEADD(mm, DATEDIFF(mm, 0, CAST(@A_DATE_TIME AS DATE)), 0),'yyyy-MM-dd')
			SELECT @END_TIME = 	 FORMAT(DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, CAST(@A_DATE_TIME AS DATE)) + 1, 0)),'yyyy-MM-dd')

		   -- thong tin nhan vien
			SELECT distinct nv.Id as MaNV, nv.TenNV,bp.Id as MaBoPhan,bp.TenBoPhan,bpct.Id as MaBoPhanChiTiet,bpct.TenBoPhanChiTiet, nv.NgayVao
			FROM HR_NHANVIEN nv
			INNER JOIN BOPHAN bp on nv.MaBoPhan = bp.Id
			LEFT JOIN HR_BO_PHAN_DETAIL bpct on nv.MaBoPhanChiTiet = bpct.Id
			WHERE nv.Status = 'Active'

			-- thong tin nhan vien, hop dong
			SELECT distinct nv.Id as MaNV, nv.TenNV,bp.Id as MaBoPhan,bp.TenBoPhan,bpct.Id as MaBoPhanChiTiet,bpct.TenBoPhanChiTiet, nv.NgayVao,hd.LoaiHD,ldh.[ShortName],ldh.[TenLoaiHD],hd.NgayHieuLuc,hd.NgayHetHieuLuc
			FROM HR_NHANVIEN nv
			INNER JOIN BOPHAN bp on nv.MaBoPhan = bp.Id
			LEFT JOIN HR_BO_PHAN_DETAIL bpct on nv.MaBoPhanChiTiet = bpct.Id
			LEFT JOIN HR_HOPDONG hd on nv.Id = hd.MaNV
			LEFT JOIN HR_LOAIHOPDONG ldh on hd.LoaiHD = ldh.Id
			WHERE nv.Status = 'Active' and hd.NgayHieuLuc <= @END_TIME and hd.NgayHetHieuLuc >= @BEGIN_TIME

			-- thong tin nhan vien, ca lam viec
			SELECT distinct nv.Id AS MaNV,nv_calviec.BatDau_TheoCa,nv_calviec.KetThuc_TheoCa,dm_calviec.Id as MaCaLaviec,dm_calviec.TenCaLamViec,caLviec.DM_NgayLViec,ngayLv.Ten_NgayLV,caLviec.Time_BatDau,caLviec.Time_BatDau2,caLviec.Time_KetThuc,caLviec.Time_KetThuc2,caLviec.HeSo_OT
			FROM HR_NHANVIEN nv
			INNER JOIN NHANVIEN_CALAMVIEC nv_calviec on nv.Id = nv_calviec.MaNV
			INNER JOIN DM_CA_LVIEC dm_calviec on nv_calviec.Danhmuc_CaLviec = dm_calviec.Id
			INNER JOIN SETTING_TIME_CA_LVIEC st_calviec on dm_calviec.Id = st_calviec.CaLamViec
			INNER JOIN CA_LVIEC caLviec on dm_calviec.Id = caLviec.Danhmuc_CaLviec
			INNER JOIN DM_NGAY_LAMVIEC ngayLv on ngayLv.Id = caLviec.DM_NgayLViec
			WHERE nv.Status = 'Active' and nv_calviec.Approved = 'Y' AND nv_calviec.BatDau_TheoCa <= @END_TIME and nv_calviec.KetThuc_TheoCa >= @BEGIN_TIME

		    -- cham cong dac biet
			SELECT nv.Id as MaNV,dkyChamChiTiet.TenChiTiet,dkyChamDB.NgayBatDau,dkyChamDB.NgayKetThuc,dkyChamChiTiet.KyHieuChamCong
			FROM HR_NHANVIEN nv
			LEFT JOIN DANGKY_CHAMCONG_DACBIET dkyChamDB	on nv.Id = dkyChamDB.MaNV
			LEFT JOIN DANGKY_CHAMCONG_CHITIET dkyChamChiTiet on dkyChamChiTiet.Id = dkyChamDB.MaChamCong_ChiTiet
			LEFT JOIN DM_DANGKY_CHAMCONG dm_DkyChamCong on dkyChamChiTiet.PhanLoaiDM = dm_DkyChamCong.Id
			LEFT JOIN KY_HIEU_CHAM_CONG kyhieuCC on dkyChamChiTiet.KyHieuChamCong = kyhieuCC.Id
			where nv.Status = 'Active' and dkyChamDB.Approve = 'Y' and dkyChamDB.NgayBatDau <= @END_TIME and dkyChamDB.NgayKetThuc >= @BEGIN_TIME

			-- Danh sach dang ky OT
			SELECT nv.Id as MaNV,ot.NgayOT,ot.DM_NgayLViec,ngaylv.Ten_NgayLV 
			FROM HR_NHANVIEN nv
			LEFT JOIN DANGKY_OT_NHANVIEN ot on ot.MaNV = nv.Id
			LEFT JOIN DM_NGAY_LAMVIEC ngaylv on ot.DM_NgayLViec = ngaylv.Id
			where nv.Status  = 'Active' and ot.Approve = 'Y' and ot.NgayOT between @BEGIN_TIME and @END_TIME

       END
	SET @N_RETURN = 0;
	SET @V_RETURN = 'MSG_COM_004';
END TRY
	BEGIN CATCH
  SET @N_RETURN = ERROR_NUMBER();
  SET @V_RETURN = ERROR_MESSAGE();
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[PKG_BUSINESS@PUT_EVENT_LOG]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
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
			    THEN INSERT ([Ngay_ChamCong],[ID_NV],[Ten_NV],[Last_Out_Time],[FirstIn],[LastOut],[DateCreated],[DateModified],[UserCreated],[UserModified],[FirstIn_Time],[Department],[FirstIn_Time_Update],[Last_Out_Time_Update])
				VALUES(SOURCE.Date_Check,SOURCE.userId, SOURCE.[userName],SOURCE.[Last_Out_Time],SOURCE.[First_In],SOURCE.[Last_Out],FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),FORMAT(GETDATE(),'yyyy-MM-dd HH:mm:ss'),'sys','sys',SOURCE.[First_In_Time],SOURCE.Department,'','');
       END
	SET @N_RETURN = 0;
	SET @V_RETURN = 'MSG_COM_004';
END TRY
	BEGIN CATCH
  SET @N_RETURN = ERROR_NUMBER();
  SET @V_RETURN = ERROR_MESSAGE();
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[PKG_BUSINESS@PUT_NHANVIEN_CALAMVIEC]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
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
/****** Object:  StoredProcedure [dbo].[PKG_BUSINESS@PUT_NHANVIEN_OVERTIME]    Script Date: 2022-05-16 4:05:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROC [dbo].[PKG_BUSINESS@PUT_NHANVIEN_OVERTIME](
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
GO
USE [master]
GO
ALTER DATABASE [HRMSDB2] SET  READ_WRITE 
GO
