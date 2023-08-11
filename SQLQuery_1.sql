USE VmpDb
GO

INSERT INTO [dbo].[Tasks]
    ([Name], EndDate, UserId, IsCompleted, [Description])
VALUES
    ('Add waybills for CA1234AB', CAST('2023-08-20' AS DATETIME2), 'd22b0574-0303-435d-b4b1-8c5e47e6f622', 0, 'Add all the waybills for vehicle CA1234AB for July 2023'),
    ('Check insurance B5555KM', CAST('2023-08-13' AS DATETIME2), 'd22b0574-0303-435d-b4b1-8c5e47e6f622', 0, 'Check if the insurance of B5555KM is valid'),
    ('Report July B5555KM', CAST('2023-08-11' AS DATETIME2), 'd22b0574-0303-435d-b4b1-8c5e47e6f622', 0, 'Make a report for July for B5555KM')

GO

INSERT INTO [dbo].[Owners]
    ([Name], Info, IsInactive)
VALUES
    ('Ivan Ivanov', null, 0),
    ('Georgi Georgiev', 'tel 0878258978, Varna, Bulgaria', 0),
    ('Milen Dimtrov', 'email: milen@abv.bg, Sofia, Bulgaria', 0)

GO

INSERT INTO [dbo].[CostCenters]
    ([Name], IsClosed)
VALUES
    ('Varna Employes', 0),
    ('Sofia Employes', 0),
    ('Invalid entries', 0)

GO


INSERT INTO [dbo].[Vehicles]
    ([Number], VIN, Model, Make, Mileage,FuelQuantity, FuelCapacity, OwnerId, FuelCostRate, IsDeleted, ModelImgUrl)
VALUES
    ('CA1234AB', '1G1JC5444R7252369', 'Chevrolet ', 'Lacheti', 187240, 10.20, 62.000, 3, 0.07, 0, null),
    ('B5555KM', 'WDB1680311J123456', 'Mercedes ', 'A140', 135480, 36.80, 42.000, 2, 0.06, 0, null)

GO

INSERT INTO [dbo].[DateChecks]
    ([Name],EndDate, VehicleNumber, IsCompleted, UserId)
VALUES
    ('Annual Technical Inspection', CAST('2023-08-31' AS DATETIME2), 'CA1234AB', 0, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    ('Annual Technical Inspection', CAST('2023-09-10' AS DATETIME2), 'B5555KM', 0, 'd22b0574-0303-435d-b4b1-8c5e47e6f622')

GO

INSERT INTO [dbo].[MileageChecks]
    ([Name], ExpectedMileage, VehicleNumber, IsCompleted, UserId)
VALUES
    ('Oil change', 187000, 'CA1234AB', 0, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    ('Oil change', 135000, 'B5555KM', 0, 'd22b0574-0303-435d-b4b1-8c5e47e6f622')

GO

INSERT INTO [dbo].[Waybills]
    ([Date], VehicleNumber, MileageStart, MileageEnd
    , MileageTraveled, RouteTraveled, FuelQuantityStart
    , FuelQuantityEnd, FuelConsumed, FuelLoaded, Info
    , [DateCreated], CostCenterId, UserId)
VALUES
    (CAST('2023-07-20' AS DATETIME2), 'CA1234AB', 186100, 186200, 100, 'Sofia - Blagoevgrad', 0.00, 53.000, 7.00, 60.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-21' AS DATETIME2), 'CA1234AB', 186200, 186350, 150, 'Blagoevgrad - Sofia -  Pernik', 53.00, 42.50, 10.50, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-22' AS DATETIME2), 'CA1234AB', 186350, 186520, 170, 'Pernik - Sofia - Pleven', 42.50, 30.6, 11.90, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-23' AS DATETIME2), 'CA1234AB', 186520, 186600, 80, 'Pleven - Vratsa', 30.60, 25, 5.60, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-24' AS DATETIME2), 'CA1234AB', 186600, 186690, 90, 'Vratsa - Sofia', 25.00, 18.70, 6.30, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-25' AS DATETIME2), 'CA1234AB', 186690, 186810, 120, 'Sofia - Blagoevgrad', 18.70, 10.30, 8.40, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-26' AS DATETIME2), 'CA1234AB', 186810, 186910, 100, 'Blagoevgrad - Sofia', 10.30, 23.30, 7.00, 20.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-27' AS DATETIME2), 'CA1234AB', 186910, 187010, 100, 'Sofia - Pernik - Sofia', 23.30, 16.30, 7.00, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-28' AS DATETIME2), 'CA1234AB', 187010, 187020, 10, 'Sofia in town', 16.30, 15.60, 0.70, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-29' AS DATETIME2), 'CA1234AB', 187020, 187050, 30, 'Sofia in town', 15.60, 13.50, 2.10, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-30' AS DATETIME2), 'CA1234AB', 187050, 187140, 90, 'Sofia - Pernik - Sofia', 13.50, 7.20, 6.30, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-31' AS DATETIME2), 'CA1234AB', 187140, 187240, 100, 'Sofia - Pernik - Sofia', 7.20, 10.20, 7.00, 10.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 2, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-20' AS DATETIME2), 'B5555KM', 134560, 134680, 120, 'Varna - Dobrich - Varna', 0.00, 32.80, 7.20, 40.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-21' AS DATETIME2), 'B5555KM', 134680, 134810, 130, 'Varna - Dobrich - Varna', 32.80, 25.00, 7.80, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-22' AS DATETIME2), 'B5555KM', 134810, 134940, 130, 'Varna - Dobrich - Varna', 25.00, 17.20, 7.80, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-23' AS DATETIME2), 'B5555KM', 134940, 135050, 110, 'Varna - Dobrich - Varna', 17.20, 10.60, 6.60, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-24' AS DATETIME2), 'B5555KM', 135050, 135120, 70, 'Varna - Balchik- Varna', 10.60, 6.40, 4.20, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-25' AS DATETIME2), 'B5555KM', 135120, 135170, 50, 'Varna in town', 6.40, 3.40, 3.00, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-26' AS DATETIME2), 'B5555KM', 135170, 135220, 50, 'Varna in town', 3.40, 20.40, 3.00, 20.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-27' AS DATETIME2), 'B5555KM', 135220, 135260, 40, 'Varna in town', 20.40, 18.00, 2.40, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-28' AS DATETIME2), 'B5555KM', 135260, 135280, 20, 'Varna in town', 18.00, 36.80, 1.20, 20.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-29' AS DATETIME2), 'B5555KM', 135280, 135310, 30, 'Varna in town', 36.80, 35.00, 1.80, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-30' AS DATETIME2), 'B5555KM', 135310, 135390, 80, 'Varna - Balchik- Varna', 35.00, 30.20, 4.80, 0.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622'),
    (CAST('2023-07-31' AS DATETIME2), 'B5555KM', 135390, 135480, 90, 'Varna - Balchik- Varna', 30.20, 36.80, 5.40, 12.00, 'Delivary', CAST('2023-08-01' AS DATETIME2), 1, 'd22b0574-0303-435d-b4b1-8c5e47e6f622')

GO
