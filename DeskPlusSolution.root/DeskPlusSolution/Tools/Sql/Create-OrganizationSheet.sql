-- Run once in the database used by SQLConnSolution. Existing organization data is retained.
IF OBJECT_ID(N'dbo.DP_ACC_OrganizationSheet', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DP_ACC_OrganizationSheet
    (
        CompanyID int NOT NULL CONSTRAINT PK_DP_ACC_OrganizationSheet PRIMARY KEY,
        LayoutJson nvarchar(max) NOT NULL,
        Revision int NOT NULL,
        ModifyUserID int NOT NULL,
        ModifyDate datetime2 NOT NULL
    );
END;
GO
