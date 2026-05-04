USE SistemaPalets;
GO

-- Check if there are any palets for proceso 27
SELECT COUNT(*) AS TotalPalets FROM Palets WHERE ProcesoId = 27;
GO

-- Check all palets
SELECT Id, NumeroPalet, ProcesoId, Estado, CantidadCajas FROM Palets;
GO

-- Check all procesos
SELECT Id, FechaProceso, Turno, Estado, AcopioId FROM Procesos WHERE Estado = 'ABIERTO';
GO
