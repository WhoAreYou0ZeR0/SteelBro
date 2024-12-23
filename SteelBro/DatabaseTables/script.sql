USE [master]
GO
/****** Object:  Database [SteelBro]    Script Date: 20/12/2024 7:21:56 pm ******/
CREATE DATABASE [SteelBro]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SteelBro', FILENAME = N'C:\databases\SteelBro.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SteelBro_log', FILENAME = N'C:\databases\SteelBro_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [SteelBro] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SteelBro].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SteelBro] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SteelBro] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SteelBro] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SteelBro] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SteelBro] SET ARITHABORT OFF 
GO
ALTER DATABASE [SteelBro] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SteelBro] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SteelBro] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SteelBro] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SteelBro] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SteelBro] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SteelBro] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SteelBro] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SteelBro] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SteelBro] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SteelBro] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SteelBro] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SteelBro] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SteelBro] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SteelBro] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SteelBro] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SteelBro] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SteelBro] SET RECOVERY FULL 
GO
ALTER DATABASE [SteelBro] SET  MULTI_USER 
GO
ALTER DATABASE [SteelBro] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SteelBro] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SteelBro] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SteelBro] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SteelBro] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SteelBro] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'SteelBro', N'ON'
GO
ALTER DATABASE [SteelBro] SET QUERY_STORE = ON
GO
ALTER DATABASE [SteelBro] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SteelBro]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 20/12/2024 7:21:56 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[ClientId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](25) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[PhoneNumber] [bigint] NOT NULL,
	[Email] [nvarchar](40) NULL,
	[UserId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComponentOrder]    Script Date: 20/12/2024 7:21:56 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComponentOrder](
	[OrderId] [int] NOT NULL,
	[ComponentId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[ComponentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Components]    Script Date: 20/12/2024 7:21:56 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Components](
	[ComponentId] [int] IDENTITY(1,1) NOT NULL,
	[ComponentName] [nvarchar](70) NOT NULL,
	[Price] [money] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Specifications] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 20/12/2024 7:21:56 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[WorkerId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
	[OrderDate] [date] NOT NULL,
	[ExDate] [date] NULL,
	[Comment] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 20/12/2024 7:21:56 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[PostName] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 20/12/2024 7:21:56 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Services]    Script Date: 20/12/2024 7:21:56 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](50) NOT NULL,
	[Price] [money] NOT NULL,
	[Comment] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 20/12/2024 7:21:56 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 20/12/2024 7:21:56 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Salt] [nvarchar](255) NOT NULL,
	[RoleId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Workers]    Script Date: 20/12/2024 7:21:56 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Workers](
	[WorkerId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](25) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[PhoneNumber] [bigint] NOT NULL,
	[Email] [nvarchar](60) NULL,
	[Post] [int] NULL,
	[UserId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([ClientId], [FirstName], [LastName], [MiddleName], [PhoneNumber], [Email], [UserId]) VALUES (1, N'John', N'Doe', N'Edward', 1234567890, N'john.doe@example.com', 1)
INSERT [dbo].[Clients] ([ClientId], [FirstName], [LastName], [MiddleName], [PhoneNumber], [Email], [UserId]) VALUES (2, N'Сам', N'Тхинг', NULL, 89217822398, NULL, 6)
INSERT [dbo].[Clients] ([ClientId], [FirstName], [LastName], [MiddleName], [PhoneNumber], [Email], [UserId]) VALUES (3, N'Пиля', N'Миланова', N'Викторовна', 89217822398, N'Mila.ea@example.com', 7)
INSERT [dbo].[Clients] ([ClientId], [FirstName], [LastName], [MiddleName], [PhoneNumber], [Email], [UserId]) VALUES (4, N'Денис', N'Кашин', N'Александрович', 70217682396, N'kashin@example.com', 10)
INSERT [dbo].[Clients] ([ClientId], [FirstName], [LastName], [MiddleName], [PhoneNumber], [Email], [UserId]) VALUES (5, N'Anton', N'Prokofiev', N'Andreevich', 89522647980, N'anton2001snake@gmail.com', 11)
SET IDENTITY_INSERT [dbo].[Clients] OFF
GO
INSERT [dbo].[ComponentOrder] ([OrderId], [ComponentId], [Quantity]) VALUES (1, 7, 1)
INSERT [dbo].[ComponentOrder] ([OrderId], [ComponentId], [Quantity]) VALUES (2, 12, 1)
INSERT [dbo].[ComponentOrder] ([OrderId], [ComponentId], [Quantity]) VALUES (11, 1, 1)
INSERT [dbo].[ComponentOrder] ([OrderId], [ComponentId], [Quantity]) VALUES (12, 7, 1)
INSERT [dbo].[ComponentOrder] ([OrderId], [ComponentId], [Quantity]) VALUES (12, 12, 2)
INSERT [dbo].[ComponentOrder] ([OrderId], [ComponentId], [Quantity]) VALUES (13, 13, 1)
INSERT [dbo].[ComponentOrder] ([OrderId], [ComponentId], [Quantity]) VALUES (14, 12, 2)
INSERT [dbo].[ComponentOrder] ([OrderId], [ComponentId], [Quantity]) VALUES (14, 13, 1)
GO
SET IDENTITY_INSERT [dbo].[Components] ON 

INSERT [dbo].[Components] ([ComponentId], [ComponentName], [Price], [Quantity], [Specifications]) VALUES (1, N'Процессор', 14.0000, 2, N'Intel Core i5-12400F')
INSERT [dbo].[Components] ([ComponentId], [ComponentName], [Price], [Quantity], [Specifications]) VALUES (2, N'Кулер для процессора', 7399.0000, 20, N'DEEPCOOL AK620')
INSERT [dbo].[Components] ([ComponentId], [ComponentName], [Price], [Quantity], [Specifications]) VALUES (3, N'Процессор', 17999.0000, 5, N'AMD Ryzen 5 7500F')
INSERT [dbo].[Components] ([ComponentId], [ComponentName], [Price], [Quantity], [Specifications]) VALUES (4, N'Видеокарта', 42999.0000, 2, N'MSI GeForce RTX 4060 VENTUS 2X BLACK OC')
INSERT [dbo].[Components] ([ComponentId], [ComponentName], [Price], [Quantity], [Specifications]) VALUES (5, N'Видеокарта', 37999.0000, 14, N'Palit GeForce RTX 3060 Dual OC')
INSERT [dbo].[Components] ([ComponentId], [ComponentName], [Price], [Quantity], [Specifications]) VALUES (6, N'ОЗУ 16GB', 4699.0000, 15, N'Kingston FURY Beast Black DDR4 3200MHz')
INSERT [dbo].[Components] ([ComponentId], [ComponentName], [Price], [Quantity], [Specifications]) VALUES (7, N'ОЗУ 32GB', 10199.0000, 12, N'Kingston FURY Beast Black DDR4, 3200MHz')
INSERT [dbo].[Components] ([ComponentId], [ComponentName], [Price], [Quantity], [Specifications]) VALUES (11, N'212', 32.0000, 4, N'41')
INSERT [dbo].[Components] ([ComponentId], [ComponentName], [Price], [Quantity], [Specifications]) VALUES (12, N'dsd', 32.0000, 2, N'ds')
INSERT [dbo].[Components] ([ComponentId], [ComponentName], [Price], [Quantity], [Specifications]) VALUES (13, N'ав', 341.0000, 32, N'ва')
SET IDENTITY_INSERT [dbo].[Components] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (1, 1, 1, 6, 7, CAST(N'2024-12-01' AS Date), CAST(N'2024-12-05' AS Date), NULL)
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (2, 1, 1, 3, 8, CAST(N'2024-12-19' AS Date), NULL, NULL)
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (6, 3, 1, 1, 3, CAST(N'2024-12-19' AS Date), NULL, NULL)
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (7, 3, 1, 1, 2, CAST(N'2024-12-19' AS Date), NULL, NULL)
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (8, 2, 1, 1, 4, CAST(N'2024-12-19' AS Date), NULL, NULL)
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (9, 2, 1, 1, 1, CAST(N'2024-12-19' AS Date), NULL, N'КАКОЙ-ТО ЧЕЛОВЕК МНЕ ВИРУС ПОДСУНУЛ. У МЕНЯ ВЫМОГАЮТ ДЕНЬГИИИИ!!!!!')
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (10, 2, 1, 1, 2, CAST(N'2024-12-19' AS Date), NULL, N'<script>alert("Hello");</script>')
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (11, 1, 4, 1, 12, CAST(N'2024-12-20' AS Date), NULL, NULL)
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (12, 1, 2, 1, 7, CAST(N'2024-12-20' AS Date), NULL, NULL)
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (13, 1, 2, 1, 1, CAST(N'2024-12-20' AS Date), NULL, NULL)
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (14, 1, 4, 1, 2, CAST(N'2024-12-20' AS Date), NULL, NULL)
INSERT [dbo].[Orders] ([OrderId], [ClientId], [WorkerId], [StatusId], [ServiceId], [OrderDate], [ExDate], [Comment]) VALUES (15, 5, 1, 1, 9, CAST(N'2024-12-20' AS Date), NULL, N'Сломался экран, памагите, не могу найти провод')
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Post] ON 

INSERT [dbo].[Post] ([PostId], [PostName]) VALUES (1, N'Администратор')
INSERT [dbo].[Post] ([PostId], [PostName]) VALUES (2, N'Менеджер')
INSERT [dbo].[Post] ([PostId], [PostName]) VALUES (3, N'Техник')
INSERT [dbo].[Post] ([PostId], [PostName]) VALUES (4, N'Бухгалтер')
SET IDENTITY_INSERT [dbo].[Post] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (1, N'Administrator')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (5, N'Client')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (4, N'Financier')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (2, N'Manager')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (3, N'Technician')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Services] ON 

INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (1, N'Удаление вируса', 150.0000, NULL)
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (2, N'Настройка маршрутизатора', 600.0000, NULL)
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (3, N'Установка и настройка ОС', 350.0000, N'Windows, macOS, Linux')
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (4, N'Чистка от пыли', 990.0000, NULL)
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (5, N'Установка антивируса', 390.0000, NULL)
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (6, N'Замена системы охлаждения', 1170.0000, NULL)
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (7, N'Замена модуля оперативной памяти', 390.0000, NULL)
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (8, N'Замена системной (материнской) платы', 1430.0000, NULL)
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (9, N'Замена экрана', 990.0000, N'экрана, дисплея')
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (10, N'Замена SSD диска', 570.0000, NULL)
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (11, N'Замена жесткого диска', 570.0000, NULL)
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Price], [Comment]) VALUES (12, N'Замена процессора', 1270.0000, NULL)
SET IDENTITY_INSERT [dbo].[Services] OFF
GO
SET IDENTITY_INSERT [dbo].[Status] ON 

INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (1, N'Новый заказ')
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (2, N'Принят в работу')
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (3, N'Ожидает запчастей')
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (4, N'В ремонте')
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (5, N'Готов к выдаче ')
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (6, N'Выдан клиенту')
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (7, N'Отменен')
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (8, N'Возврат')
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Salt], [RoleId]) VALUES (1, N'client1', N'8agEwlzBXyj+qb0GgF5dWfXCrTUwF9LfBfMzPu24+9tDNpB7kb1JmEbudIenMtZdhw+/3O1dN/Lweb/flLSKkA==', N'IPclJEa1UidkibM+q5nDEsYaYd4H4tIE0Cl4FLHSJ9JSzdz0EPPfXoXKfBBsxUiwAlu1wZqdAGxY9Ea1+CWIO28OHO0Rwxik2tUu9wtMrukRkD92AeS0FuUVsqbI6h9BdIX4ChJHb4X8lUiTiAO9Upxgf4pP4wZ2SaZO1D+kF9Q=', 5)
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Salt], [RoleId]) VALUES (2, N'technician1', N'XqT9gRfuqosasDoLLltV2TNjuArmhT+QtGslroCBXf8qwMcNhOBINduqeEme47CArxcFlTqlTdmyxMHV4ZcSEw==', N'F/UbWRh13a/fajE+MAfjdUtBgeBydAkBW3rxf7rLsY7swE84/6yLXzhFF9odMzTMySP3vAY/78Onh6c0XlyjTeAZU0SWrkcTXhxyosviPRkuGuDJtDCQhqkzUaogbzvaLYjWweGCHOo2LATyZyQrLcr4DVQXtxRn4GhNI9OSX0o=', 3)
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Salt], [RoleId]) VALUES (3, N'manager1', N'ITSkqVYB9FL+2JbrnHrgCWkdihHwlyXpCKR3DkIz+rx5q7GE791FKvRWReBZMl6wEburkSda4r/dHUZkllR8bg==', N'JL2pkjO/gH56NO02PDGiNGc1bt/iSQMWM+fClNyPml4zoEuwmVDfYZc1UNfGJGmqP+TQa/RPF4jIB99WScnuNgdgdCW34sCWezYYMlr6atxqVKaaLXR4j3Nan3EbJSkgY6dit+PGFtvmzpCiGyq4o7oxtaFZGuLIUfDA/Un6Tiw=', 2)
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Salt], [RoleId]) VALUES (4, N'financier1', N'eYGSJWn/n1mg1G/Vv6DhYLGxxWE+mYb40AH3Ycpuu17PNAbAs9HSEwLk4raZfSif+xyNV2wexIUHn1mb/VZwZg==', N'oyedHm061ME9G7EM1HzbIxyacUqUx4addpHNpPAxcMhGi0fv9h4lgo/GjQ1gaVSaa0D3byHh0Ls/dUp6HmwYdiTjXhGPRENwRpM4IwG0T1fcJoeSwGJJL1A3plHzCuNDbHtcQM3PnD6d+YU2YIYTpYdiStXDpJVAsvPjkO/z+Nc=', 4)
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Salt], [RoleId]) VALUES (5, N'admin1', N'IwBd5204K/b7fvgtBocL/KjezSzjvMrMEp/TZ+w51RtoU93A1wUZOxlr+x/HPumWF5xV0qhMJXAjcg5iNgTjaQ==', N'68iSk9TFy3rpVlWqPuI6N/DOZRyuuG6iMeJWiqZrfjAPmOjU/F7Gb/CbYNIysvv4CFriucqFG8Adx+29UDV8hzx4jtd4KtEvQW9/a8ghPENQXleoMIXI7U08iZ+Ias4G8OezrMv4eKGWuQ6WDIErCvgk29CywNKO9YxSMv2Ni8E=', 1)
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Salt], [RoleId]) VALUES (6, N'some', N'LdUOX87idMU9pxnlWf4Z0Cqy9gUTEBslOyZjiFVEQb9w6FtRi7PfZeR4AF6piV1FavKdL43I2nLo5WgOwxv+Vg==', N'YFoCILteM9ovvF73UewO2iVl5kIBoBU547jw5xEQ5C1/1gp5oZH3rGvLp/QDOxKk5NcdIqmn9x5R3a1opwlDMiNET+0f3OYuACUM5XzAB4w8Uz0NnRj/W5wMruoSpXi+RJsPZsVsc77gng8XuBaFAGMtMRt+63fdy9Yn0au85ZQ=', 5)
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Salt], [RoleId]) VALUES (7, N'cat1', N'ANE2zgf5yJ7K04Qj2NzB1/Ti9IF4+EZTRH6iSwcp/tzoorbklg8UAaOjmYI/E7zhANNUKVqwkz1qnrIrVSVHGQ==', N'PF9iEK4zrf8tiukM/zWOoUW2eAVB6Z82HXqIC+IGFHsmjdH0ws4BgAWjcieRqZYjB1mAONlTe9nK+n/Li1N6ZPlWamqgXbhyTVpuzFFC9mtaX/zpkgvu0sXVBbwbI7nWr/0/d/hpa5xO6JnD+pmokv55qO2npfTMsdB93TJoXx0=', 5)
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Salt], [RoleId]) VALUES (8, N'helloCutie', N'ijDpZs1Ql6od+1X8bOLoLXhBKA8UCKol7bhTVlt9CpaUebvTja/IhSs/3nzmevCB7fOE7I6jfQ/EhRD66VK5mg==', N'Cc5V2XfKbe68pCIs2WEYRvoiDjdk7oM+ALzwHgCtd1tKfiXwIX/OHRCjtHqTn241EIJe2evsSxU94+wlh7EKGJAGSuJGwUeKXJKSm6Qcf7RbHMbvHEw6cpGsjGQEPUm5nZ1qDs6u6wabB/hcy5Okz0rwHMCD/TlCYXXXAe48JuA=', 5)
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Salt], [RoleId]) VALUES (10, N'Dencheese228', N'2hPk8Mlydc2MAgG8EwWPzgvEpyxDIoll6IN8dABKZGCcvkXkPwHc0p+4wzTSpKxpssRFjtmPn3XRjw75kW2+SA==', N'Ksej6K+OIscZGTdRjwzfKgrTlLFwuWaovWGERTRs8hU9p2cJCABMhj6vYPma6DS2i6Sef8r1aU8InSahJKo80HkUfvbUwObWEsV+MhtxJgkQPzxRuuOw+FMbosaC3bD/GJdeZePtr1gO6k7xNcjs5rtE+TGldJYwg2s1zbrMD3I=', 5)
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Salt], [RoleId]) VALUES (11, N'anton', N'JfF4Cl2oP5k7dnbFXrJUA43W/ebFXOCESx8ohhyIfERCoVUcMUDPuOsmPcRmH35M0FdY+YxD8J44r8viUKDvgg==', N'Qg+EGFhxGweKlDmwYIVnnY8tu8OP5ZSGpSEHd5kRHge9en1LGe8ljSLNxeKnDrEU1wJbb+Ip0CVAE1JFbBKqGvUABPh+q76X7Q37ZLODpYYohaB8XcCoxYTccMDcQfV3BqCV6VpjytXKNaQaL1g3eeSSOCdazk+25Y5piO2/iaM=', 5)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Workers] ON 

INSERT [dbo].[Workers] ([WorkerId], [FirstName], [LastName], [MiddleName], [PhoneNumber], [Email], [Post], [UserId]) VALUES (1, N'Anna', N'Taylor', N'Marie', 4445556666, N'anna.taylor@example.com', 3, 2)
INSERT [dbo].[Workers] ([WorkerId], [FirstName], [LastName], [MiddleName], [PhoneNumber], [Email], [Post], [UserId]) VALUES (2, N'Mark', N'Wilson', NULL, 7778889999, N'mark.wilson@example.com', 3, 3)
INSERT [dbo].[Workers] ([WorkerId], [FirstName], [LastName], [MiddleName], [PhoneNumber], [Email], [Post], [UserId]) VALUES (3, N'Emily', N'Davis', N'Rose', 2223334444, N'emily.davis@example.com', 4, 4)
INSERT [dbo].[Workers] ([WorkerId], [FirstName], [LastName], [MiddleName], [PhoneNumber], [Email], [Post], [UserId]) VALUES (4, N'Henry', N'Pico', NULL, 5723234544, N'henry.pico@example.com', 1, 5)
SET IDENTITY_INSERT [dbo].[Workers] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Roles__8A2B616053F8DC3D]    Script Date: 20/12/2024 7:21:56 pm ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E4F39C9F2D]    Script Date: 20/12/2024 7:21:56 pm ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[ComponentOrder]  WITH CHECK ADD FOREIGN KEY([ComponentId])
REFERENCES [dbo].[Components] ([ComponentId])
GO
ALTER TABLE [dbo].[ComponentOrder]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([ClientId])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([ServiceId])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([WorkerId])
REFERENCES [dbo].[Workers] ([WorkerId])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Workers]  WITH CHECK ADD FOREIGN KEY([Post])
REFERENCES [dbo].[Post] ([PostId])
GO
ALTER TABLE [dbo].[Workers]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
USE [master]
GO
ALTER DATABASE [SteelBro] SET  READ_WRITE 
GO
