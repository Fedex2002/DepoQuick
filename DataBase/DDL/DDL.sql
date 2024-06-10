-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- DepoQuick.dbo.Persons definition

-- Drop table

-- DROP TABLE DepoQuick.dbo.Persons;

CREATE TABLE DepoQuick.dbo.Persons (
	Email nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Surname nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	IsAdmin bit NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Persons PRIMARY KEY (Email)
);


-- DepoQuick.dbo.Promotions definition

-- Drop table

-- DROP TABLE DepoQuick.dbo.Promotions;

CREATE TABLE DepoQuick.dbo.Promotions (
	Id int IDENTITY(1,1) NOT NULL,
	Label nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DateStart datetime2 NOT NULL,
	DateEnd datetime2 NOT NULL,
	Discount int NOT NULL,
	CONSTRAINT PK_Promotions PRIMARY KEY (Id)
);


-- DepoQuick.dbo.StorageUnits definition

-- Drop table

-- DROP TABLE DepoQuick.dbo.StorageUnits;

CREATE TABLE DepoQuick.dbo.StorageUnits (
	Id nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Area int NOT NULL,
	[Size] int NOT NULL,
	Climatization bit NOT NULL,
	CONSTRAINT PK_StorageUnits PRIMARY KEY (Id)
);


-- DepoQuick.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE DepoQuick.dbo.[__EFMigrationsHistory];

CREATE TABLE DepoQuick.dbo.[__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- DepoQuick.dbo.Bookings definition

-- Drop table

-- DROP TABLE DepoQuick.dbo.Bookings;

CREATE TABLE DepoQuick.dbo.Bookings (
	Id int IDENTITY(1,1) NOT NULL,
	Approved bit NOT NULL,
	PersonEmail nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DateStart datetime2 NOT NULL,
	DateEnd datetime2 NOT NULL,
	StorageUnitId nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	RejectedMessage nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Status nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Payment bit NOT NULL,
	CONSTRAINT PK_Bookings PRIMARY KEY (Id),
	CONSTRAINT FK_Bookings_StorageUnits_StorageUnitId FOREIGN KEY (StorageUnitId) REFERENCES DepoQuick.dbo.StorageUnits(Id)
);
 CREATE NONCLUSTERED INDEX IX_Bookings_StorageUnitId ON dbo.Bookings (  StorageUnitId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DepoQuick.dbo.DateRange definition

-- Drop table

-- DROP TABLE DepoQuick.dbo.DateRange;

CREATE TABLE DepoQuick.dbo.DateRange (
	Id int IDENTITY(1,1) NOT NULL,
	StartDate datetime2 NOT NULL,
	EndDate datetime2 NOT NULL,
	StorageUnitId nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_DateRange PRIMARY KEY (Id),
	CONSTRAINT FK_DateRange_StorageUnits_StorageUnitId FOREIGN KEY (StorageUnitId) REFERENCES DepoQuick.dbo.StorageUnits(Id)
);
 CREATE NONCLUSTERED INDEX IX_DateRange_StorageUnitId ON dbo.DateRange (  StorageUnitId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DepoQuick.dbo.StorageUnitPromotion definition

-- Drop table

-- DROP TABLE DepoQuick.dbo.StorageUnitPromotion;

CREATE TABLE DepoQuick.dbo.StorageUnitPromotion (
	StorageUnitId nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	PromotionId int NOT NULL,
	CONSTRAINT PK_StorageUnitPromotion PRIMARY KEY (StorageUnitId,PromotionId),
	CONSTRAINT FK_StorageUnitPromotion_Promotions_PromotionId FOREIGN KEY (PromotionId) REFERENCES DepoQuick.dbo.Promotions(Id) ON DELETE CASCADE,
	CONSTRAINT FK_StorageUnitPromotion_StorageUnits_StorageUnitId FOREIGN KEY (StorageUnitId) REFERENCES DepoQuick.dbo.StorageUnits(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_StorageUnitPromotion_PromotionId ON dbo.StorageUnitPromotion (  PromotionId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;