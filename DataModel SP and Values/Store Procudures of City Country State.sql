Create PROC [dbo].[PR_LOC_City_SelectAll]
AS 
Begin
SELECT
		[dbo].[City].[CityID],
		[dbo].[City].[StateID],
		[dbo].[Country].CountryID,
		[dbo].[Country].[CountryName],
		[dbo].[State].[StateName],
		[dbo].[State].[StateCode],
		[dbo].[City].[CreatedDate],
		[dbo].[City].[ModifiedDate],
		[dbo].[City].[CityName],
		[dbo].[City].[CityCode]
		
FROM [dbo].[City]
LEFT OUTER JOIN [dbo].[State]
ON [dbo].[State].[StateID] = [dbo].[City].[StateID]
LEFT OUTER JOIN [dbo].[Country]
ON [dbo].[Country].[CountryID] = [dbo].[State].[CountryID]
End


Create PROC [dbo].[PR_LOC_City_SelectByPK]
	@CityID	Int
AS
Begin 
SELECT
		[dbo].[City].[CityID],
		[dbo].[City].[StateID],
		[dbo].[Country].CountryID,
		[dbo].[Country].[CountryName],
		[dbo].[State].[StateName],
		[dbo].[State].[StateCode],
		[dbo].[City].[CreatedDate],
		[dbo].[City].[ModifiedDate],
		[dbo].[City].[CityName],
		[dbo].[City].[CityCode]
		
FROM [dbo].[City]
LEFT OUTER JOIN [dbo].[State]
ON [dbo].[State].[StateID] = [dbo].[City].[StateID]
LEFT OUTER JOIN [dbo].[Country]
ON [dbo].[Country].[CountryID] = [dbo].[State].[CountryID]
where [dbo].[City].[CityID] = @CityID
End

CREATE PROCEDURE PR_LOC_City_Insert
    @CityName NVARCHAR(100),
    @CityCode NVARCHAR(10),
    @StateID INT,
    @CountryID INT
AS
BEGIN
    INSERT INTO City (CityName, CityCode, StateID, CountryID, CreatedDate)
    VALUES (@CityName, @CityCode, @StateID, @CountryID, GETDATE());
END


CREATE PROCEDURE PR_LOC_City_Update
    @CityID INT,
    @CityName NVARCHAR(100),
    @CityCode NVARCHAR(10),
    @StateID INT,
    @CountryID INT
AS
BEGIN
    UPDATE City
    SET CityName = @CityName,
        CityCode = @CityCode,
        StateID = @StateID,
        CountryID = @CountryID,
        ModifiedDate = GETDATE()
    WHERE CityID = @CityID;
END

CREATE PROCEDURE PR_LOC_City_Delete
    @CityID INT
AS
BEGIN
    DELETE FROM City
    WHERE CityID = @CityID
END

CREATE PROCEDURE [dbo].[PR_LOC_Country_SelectComboBox]
AS
begin 
SELECT
    COUNTRYID,
    COUNTRYNAME
FROM COUNTRY
ORDER BY COUNTRYNAME
end

CREATE PROCEDURE [dbo].[PR_LOC_State_SelectComboBoxByCountryID]
@CountryID INT
AS 
Begin
SELECT
    [dbo].[State].[StateID],
    [dbo].[State].[StateName]	
FROM [dbo].[State]
WHERE [dbo].[State].[CountryID] = @CountryID
End

-----------------------------------------------------

Alter PROC [dbo].[PR_LOC_Country_SelectAll]
AS 
Begin
SELECT
		[dbo].[Country].CountryID,
		[dbo].[Country].[CountryName],
		[dbo].[Country].[CountryCode],
		[dbo].[Country].[CreatedDate],
		[dbo].[Country].[ModifiedDate],
		COUNT([dbo].[State].[StateID]) as StateCount
FROM [dbo].[Country]
Left outer join [dbo].[State] on [dbo].[State].[CountryID] = [dbo].[Country].[CountryID]
Group By
		[dbo].[Country].CountryID,
		[dbo].[Country].[CountryName],
		[dbo].[Country].[CountryCode],
		[dbo].[Country].[CreatedDate],
		[dbo].[Country].[ModifiedDate] 
End

Create PROC [dbo].[PR_LOC_Country_SelectByPK]
@CountryID	int
AS 
Begin
SELECT
		[dbo].[Country].CountryID,
		[dbo].[Country].[CountryName],
		[dbo].[Country].[CountryCode],
		[dbo].[Country].[CreatedDate],
		[dbo].[Country].[ModifiedDate]	
FROM [dbo].[Country]
where [dbo].[Country].CountryID = @CountryID
End

Alter PROCEDURE PR_LOC_Country_Insert
    @CountryName NVARCHAR(100),
    @CountryCode NVARCHAR(10)
AS
BEGIN
    INSERT INTO Country (CountryName, CountryCode, CreatedDate)
    VALUES (@CountryName, @CountryCode, GETDATE());
END

CREATE PROCEDURE PR_LOC_Country_Update
    @CountryID	int,
	@CountryName NVARCHAR(100),
    @CountryCode NVARCHAR(10)
AS
BEGIN
	Update Country
	set
		CountryName = @CountryName,
		CountryCode = @CountryCode,
		ModifiedDate = GETDATE()
	where CountryID = @CountryID
END

CREATE PROCEDURE PR_LOC_Country_Delete
    @CountryID INT
AS
BEGIN
    DELETE FROM Country
    WHERE CountryID = @CountryID
END

--------------------------------------------------------

ALter PROC [dbo].[PR_LOC_State_SelectAll]
AS 
Begin
SELECT
		[dbo].[State].StateID,
		[dbo].[State].[StateName],
		[dbo].[State].[StateCode],
		[dbo].[State].[CountryID],
		C.[CountryName],
		[dbo].[State].[CreatedDate],
		[dbo].[State].[ModifiedDate],
		COUNT(CI.[StateID]) as CityCount		
FROM [dbo].[State]
Left Outer join Country as C on C.CountryID = [dbo].[State].[CountryID]
Left Outer join City as CI on CI.StateID = [dbo].[State].[StateID]
GROUP BY	
        [dbo].[State].[StateID],
        [dbo].[State].[StateName],
        [dbo].[State].[StateCode],
        [dbo].[State].[CountryID],
        C.[CountryName],
        [dbo].[State].[CreatedDate],
        [dbo].[State].[ModifiedDate];
End

Create PROC [dbo].[PR_LOC_State_SelectPK]
@StateID	int
AS 
Begin
SELECT
		[dbo].[State].StateID,
		[dbo].[State].[StateName],
		[dbo].[State].[StateCode],
		[dbo].[State].[CountryID],
		C.[CountryName],
		[dbo].[State].[CreatedDate],
		[dbo].[State].[ModifiedDate]	
FROM [dbo].[State]
Left Outer join Country as C on C.CountryID = [dbo].[State].[CountryID]
where [dbo].[State].StateID = @StateID
End

CREATE PROCEDURE PR_LOC_State_Insert
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10),
	@CountryID	int
AS
BEGIN
    INSERT INTO State (StateName, StateCode,CountryID, CreatedDate)
    VALUES (@StateName, @StateCode,@CountryID, GETDATE());
END

CREATE PROCEDURE PR_LOC_State_Update
    @StateID	int,
	@StateName NVARCHAR(100),
    @StateCode NVARCHAR(10),
	@CountryID	int
AS
BEGIN
	Update State
	set
		StateName = @StateName,
		StateCode = @StateCode,
		CountryID = @CountryID,
		ModifiedDate = GETDATE()
	where StateID = @StateID
END

Create Proc PR_LOC_State_Delete
@StateID	int
As
Begin
	Delete State
	where StateID = @StateID
End