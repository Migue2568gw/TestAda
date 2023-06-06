-- =========================================================================================================
-- DESARROLLADO POR		    : [Miguel Gonzalez]
-- LENGUAJE PROGRAMACIÓN	: [T-SQL]
-- =========================================================================================================
/*
	|Prueba Ada|
*/
-- =========================================================================================================

USE [master]
GO
IF EXISTS (
	SELECT *
	FROM sysdatabases
	WHERE (name = 'AdaTestBd')
)
BEGIN
	DROP DATABASE AdaTestBd
END

EXEC ('CREATE DATABASE [AdaTestBd]')
GO
IF EXISTS (
	SELECT *
	FROM sysdatabases
	WHERE (name = 'AdaTestBd')
)
BEGIN
	 USE [AdaTestBd]
END
GO
-- Abre la transaccion
BEGIN TRAN TX

	-- Abre el Try
	BEGIN TRY

-- =========================================================================================================
------------------------------------------------------------
------------------  Se crean las Tablas ------------------------
------------------------------------------------------------

		IF OBJECT_ID('Usuarios', 'U') IS NULL
		CREATE TABLE Usuarios (
		    UsuarioId INT IDENTITY PRIMARY KEY,
		    Nombres VARCHAR(100),
		    Direccion VARCHAR(100),
		    Telefono VARCHAR(20),
		    Identificacion VARCHAR(20),
			UsuarioLog VARCHAR(20),
		    Contrasena VARCHAR(10),
			Rol INT --1: ADMIN 2:USER
		);
		
		IF OBJECT_ID('Productos', 'U') IS NULL
		CREATE TABLE Productos (
		    ProductoId INT IDENTITY PRIMARY KEY,
		    Nombre VARCHAR(100),
		    CantidadDisponible INT,
		    Descripcion VARCHAR(500)
		);
		
		IF OBJECT_ID('Transacciones', 'U') IS NULL
		CREATE TABLE Transacciones (
		    TransaccionId INT IDENTITY PRIMARY KEY,
		    UsuarioId INT,
		    ProductoId INT,
		    Cantidad INT,
		    FechaTransaccion DATETIME,
		    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId),
		    FOREIGN KEY (ProductoId) REFERENCES Productos(ProductoId)
		);
------------------------------------------------------------
------------------  Se crean los SP ------------------------
------------------------------------------------------------

		IF EXISTS (
			SELECT 1
			FROM SYS.OBJECTS
			WHERE TYPE = 'P' AND NAME = 'SP_ActualizarProducto'
		)
		BEGIN
			DROP PROCEDURE SP_ActualizarProducto
		END
		EXEC ('
		CREATE PROCEDURE SP_ActualizarProducto
			@ProductoId INT,
		    @Cantidad INT,
		    @UsuarioId INT
		AS
		BEGIN
		    IF EXISTS (SELECT 1 FROM Productos WHERE ProductoId = @ProductoId)
		    BEGIN
		        UPDATE Productos
		        SET CantidadDisponible = CantidadDisponible - @Cantidad
		        WHERE ProductoId = @ProductoId
		        
		        INSERT INTO Transacciones (UsuarioId, ProductoId, Cantidad, FechaTransaccion)
		        VALUES (@UsuarioId, @ProductoId, @Cantidad, GETDATE())
		        
		    END
		    ELSE
		    BEGIN
		        THROW 50001, ''El producto no existe'', 1
		    END
		END
		')

		IF EXISTS (
		SELECT 1
		FROM SYS.OBJECTS
		WHERE TYPE = 'P' AND NAME = 'SP_ConsultarUsuarios'
		)
		BEGIN
			DROP PROCEDURE SP_ConsultarUsuarios
		END
		EXEC ('
		CREATE PROCEDURE SP_ConsultarUsuarios
		AS
		BEGIN
		        SELECT * FROM Usuarios    
		END
		')


		IF EXISTS (
		SELECT 1
		FROM SYS.OBJECTS
		WHERE TYPE = 'P' AND NAME = 'SP_CrearUsuarios'
		)
		BEGIN
			DROP PROCEDURE SP_CrearUsuarios
		END
		EXEC ('
		CREATE PROCEDURE SP_CrearUsuarios
		    @Nombres VARCHAR(100),
		    @Direccion VARCHAR(100),
		    @Telefono VARCHAR(20),
		    @Identificacion VARCHAR(20),
		    @UsuarioLog VARCHAR(20),
		    @Contrasena VARCHAR(10),
		    @Rol INT
		AS
		BEGIN 
		    IF EXISTS (SELECT 1 FROM Usuarios WHERE UsuarioLog = @UsuarioLog)
		    BEGIN
		        THROW 50001, ''Ya existe un usuario con el nombre de Usuario.'', 1;
		    END
		    ELSE
		    BEGIN
		        INSERT INTO Usuarios
		        VALUES (@Nombres, @Direccion, @Telefono, @Identificacion, @UsuarioLog, @Contrasena, @Rol);  
		    END
		END
		')

		IF EXISTS (
		SELECT 1
		FROM SYS.OBJECTS
		WHERE TYPE = 'P' AND NAME = 'SP_GetTrasaccion'
		)
		BEGIN
			DROP PROCEDURE SP_GetTrasaccion
		END
		EXEC ('
		CREATE PROCEDURE SP_GetTrasaccion
		AS
		BEGIN
			SELECT Tr.TransaccionId, Tr.UsuarioId, Us.Nombres AS NombreUsuario,
			Tr.ProductoId, Pr.Nombre AS NombreProducto, Tr.Cantidad, Tr.FechaTransaccion
			FROM Transacciones AS Tr
			INNER JOIN Usuarios AS Us ON Us.UsuarioId = Tr.UsuarioId
			INNER JOIN Productos AS Pr ON Pr.ProductoId = Tr.ProductoId
		END
		')

		IF EXISTS (
		SELECT 1
		FROM SYS.OBJECTS
		WHERE TYPE = 'P' AND NAME = 'SP_GetProducto'
		)
		BEGIN
			DROP PROCEDURE SP_GetProducto
		END
		EXEC ('
		CREATE PROCEDURE SP_GetProducto
		AS
		BEGIN
			SELECT *
			FROM Productos
		END
		')

		IF EXISTS (
		SELECT 1
		FROM SYS.OBJECTS
		WHERE TYPE = 'P' AND NAME = 'SP_CRUDPROD'
		)
		BEGIN
			DROP PROCEDURE SP_CRUDPROD
		END
		EXEC ('
		CREATE PROCEDURE SP_CRUDPROD
		    @ProductoId INT = NULL,
		    @Nombre VARCHAR(100),
		    @CantidadDisponible INT,
		    @Descripcion VARCHAR(500),
		    @OPC VARCHAR(8)
		AS
		BEGIN
		    IF @OPC = ''CREA''
		    BEGIN
		        INSERT INTO Productos (Nombre, CantidadDisponible, Descripcion)
		        VALUES (@Nombre, @CantidadDisponible, @Descripcion)
		    END
		    ELSE IF @OPC = ''MODI''
		    BEGIN
		        UPDATE Productos
		        SET Nombre = @Nombre,
		            CantidadDisponible = @CantidadDisponible,
		            Descripcion = @Descripcion
		        WHERE ProductoId = @ProductoId
		    END
		END
		')
------------------------------------------------------------
------------------  Se crean el usuario admin --------------
------------------------------------------------------------

		INSERT INTO Usuarios
		VALUES('Administrador','calle 16d 40','3143694426','1000513690','admin','abc123', 1)

-- =========================================================================================================
--ROLLBACK TRAN TX
COMMIT TRAN TX;		
		SELECT  GETDATE() as Dia_Ejecucion, 'Se ejecutó correctamente.'

	END TRY
	BEGIN CATCH
-- Deshace la transacción
ROLLBACK TRAN TX
-- Obtiene el codigo de error y el mensaje de error
		SELECT  'Hubo error en la ejecución, se aborta la operación.' AS Mensaje ,
				ERROR_MESSAGE() AS Detlle_Mensaje ,
				ERROR_NUMBER() AS Numero ,
				ERROR_LINE() AS Linea ,
				ERROR_SEVERITY() AS Severidad ,
				ERROR_STATE() AS Estado ,
				ERROR_PROCEDURE() AS Proceso      
	END CATCH ;