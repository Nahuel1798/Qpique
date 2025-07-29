-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 29-07-2025 a las 07:46:14
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `db_qpiqueweb`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetroleclaims`
--

CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetroles`
--

CREATE TABLE `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `aspnetroles`
--

INSERT INTO `aspnetroles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
('000a1ef4-6a7f-41c7-92b4-9608126fcce4', 'Empleado', 'EMPLEADO', NULL),
('5fc8489e-0c56-4fe1-afef-df9335de4fe2', 'Administrador', 'ADMINISTRADOR', NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetuserclaims`
--

CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetuserlogins`
--

CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetuserroles`
--

CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `aspnetuserroles`
--

INSERT INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
('04f75c34-8837-4a49-adc1-04b144d5c285', '5fc8489e-0c56-4fe1-afef-df9335de4fe2'),
('0788310c-2268-4e37-8e60-f18710c7af92', '000a1ef4-6a7f-41c7-92b4-9608126fcce4'),
('8dcbea42-b7b1-4807-8552-c549dcf2fc24', '000a1ef4-6a7f-41c7-92b4-9608126fcce4'),
('bcadfb3a-0ed5-4c96-a84e-7f7560e5e18d', '000a1ef4-6a7f-41c7-92b4-9608126fcce4');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetusers`
--

CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `Apellido` varchar(20) NOT NULL,
  `Avatar` longtext DEFAULT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `aspnetusers`
--

INSERT INTO `aspnetusers` (`Id`, `Nombre`, `Apellido`, `Avatar`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`) VALUES
('04f75c34-8837-4a49-adc1-04b144d5c285', 'Nahuel', 'Lucero', '/img/avatars/b675477a-6479-4d74-9b99-fc722809b048.jpg', 'nahuellucero25@gmail.com', 'NAHUELLUCERO25@GMAIL.COM', 'nahuellucero25@gmail.com', 'NAHUELLUCERO25@GMAIL.COM', 1, 'AQAAAAIAAYagAAAAEPKe+XrcqkLKw4cRNK3ccugHHY8LGkbBIZ/oLNI/bsOORpefJsG+xI3KIIf022JX9w==', '5GK4NZPDBLE4PVWIQJIMG6ASGOPK73FO', '038cc365-3ee9-48f3-b47e-8c455b3b7d45', NULL, 0, 0, NULL, 1, 0),
('0788310c-2268-4e37-8e60-f18710c7af92', 'Santino', 'Molina', '/img/avatars/32eeb85f-e3cd-457f-9b37-8fd2743bb533.jpg', 'Santino@gmail.com', 'SANTINO@GMAIL.COM', 'Santino@gmail.com', 'SANTINO@GMAIL.COM', 1, 'AQAAAAIAAYagAAAAEKyKzLNJCnlr0Apf7i5DCBDpuy3ScO6gW2kxZl4yhgVjzbsIaq/y8VlNlTYVI5F4EA==', 'DLC57O4WQZJEC2PQGAWPQ4QAV6MOUJFL', 'f45918ab-cd23-4d4a-9cbb-507b65fbfd8f', NULL, 0, 0, NULL, 1, 0),
('8dcbea42-b7b1-4807-8552-c549dcf2fc24', 'Nando', 'Aguero', NULL, 'Nando@gmail.com', 'NANDO@GMAIL.COM', 'Nando@gmail.com', 'NANDO@GMAIL.COM', 1, 'AQAAAAIAAYagAAAAEPCA8jgcZJQvmImjr/OMKcDMALutD1zW0S7+BiiNFHSBNyJl+xbKUzsAPv31FKflDw==', 'SNVFDKA4UG4FOIUKS2A47CDHG3VLAQ6I', '6dc12b58-8b40-4ca3-9fa4-e629278c310c', NULL, 0, 0, NULL, 1, 0),
('bcadfb3a-0ed5-4c96-a84e-7f7560e5e18d', 'Marcelo', 'Lucero', NULL, 'maluce314@gmail.com', 'MALUCE314@GMAIL.COM', 'maluce314@gmail.com', 'MALUCE314@GMAIL.COM', 1, 'AQAAAAIAAYagAAAAEMCZkiLBHVgoj21QAPpo4szKjcZJtXmFs5BOaNNW65cU5TzIxYXtqWJxy/wlgDDQDg==', 'OEHOIXSOMKQKZHP6SU2ITVUGWZ77KTC2', '0ac53020-c855-41a5-a153-88fdaff74dcf', NULL, 0, 0, NULL, 1, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetusertokens`
--

CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(128) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `Value` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `categorias`
--

CREATE TABLE `categorias` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(30) NOT NULL,
  `Estado` tinyint(4) NOT NULL,
  `ImagenUrl` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `categorias`
--

INSERT INTO `categorias` (`Id`, `Nombre`, `Estado`, `ImagenUrl`) VALUES
(1, 'Boya', 0, '/img/categorias/2e2abb23-dad7-48aa-ba4a-35514b912da0.jpg'),
(2, 'Reelss', 0, '/img/categorias/168ee885-02b3-45fe-b082-c74bc55eb60c.png'),
(3, 'Camping', 0, '/img/categorias/b9b78761-e551-4d1d-a7f6-4b53b9aff2a3.jpg'),
(4, 'Linternas', 0, '/img/categorias/7695ea56-1565-410b-8cef-883a5c5932c4.jpg'),
(5, 'Indumentaria', 0, '/img/categorias/fde4c6c5-5a79-4e68-9557-0773ccc03d99.png'),
(6, 'Accesorios de pesca', 0, '/img/categorias/15baa3be-982a-4270-8635-8887d47d1842.jpg'),
(7, 'Cañas', 0, '/img/categorias/3e75394c-22e8-4900-b53e-99d3c38d47a9.jpg'),
(8, 'Asador', 1, '/img/categorias/b939d375-83e4-4f7b-9c97-0a31e1d012f5.jpeg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `clientes`
--

CREATE TABLE `clientes` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(30) NOT NULL,
  `Apellido` varchar(30) NOT NULL,
  `Telefono` varchar(15) NOT NULL,
  `Email` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `clientes`
--

INSERT INTO `clientes` (`Id`, `Nombre`, `Apellido`, `Telefono`, `Email`) VALUES
(1, 'Marcelo', 'Lucero', '', ''),
(2, 'Nahuel', 'Lucero', '', ''),
(3, 'Dana', 'Muñoz', '', ''),
(4, 'Laura', 'Lucero', '', ''),
(5, 'Santino', 'Molina', '', ''),
(6, 'German', 'Gomez', '', ''),
(7, 'Franco', 'Ponce', '', '');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detallesventa`
--

CREATE TABLE `detallesventa` (
  `Id` int(11) NOT NULL,
  `VentaId` int(11) NOT NULL,
  `ProductoId` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL,
  `PrecioUnitario` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `detallesventa`
--

INSERT INTO `detallesventa` (`Id`, `VentaId`, `ProductoId`, `Cantidad`, `PrecioUnitario`) VALUES
(3, 2, 11, 1, 15000.00),
(4, 2, 9, 1, 43200.00),
(5, 1, 6, 1, 8000.00),
(6, 1, 7, 1, 40000.00);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `productos`
--

CREATE TABLE `productos` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `Descripcion` varchar(500) NOT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Stock` int(11) NOT NULL,
  `Estado` tinyint(4) NOT NULL,
  `CategoriaId` int(11) NOT NULL,
  `ImagenUrl` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `productos`
--

INSERT INTO `productos` (`Id`, `Nombre`, `Descripcion`, `Precio`, `Stock`, `Estado`, `CategoriaId`, `ImagenUrl`) VALUES
(1, 'Boya Luminosa', '15gr', 8000.00, 15, 1, 1, '/img/productos/40f9244e-a404-49d8-80a8-cf68ffdddb78.jpg'),
(2, 'carpa', '4 persona', 70000.00, 3, 1, 3, '/img/productos/1aa12ad9-a1cb-4cf1-b11a-3a8df8032616.jpg'),
(3, 'Linterna generica', 'Tactica', 30000.00, 3, 1, 4, '/img/productos/826d40c0-1e4a-43b0-83db-b39d907012e8.jpg'),
(4, 'Wader', 'Impermeable', 50000.00, 3, 1, 5, '/img/productos/75ed38a4-8e37-4edf-8543-eec048cfe299.jpg'),
(5, 'Caja de pesca', 'Casterr', 54000.00, 3, 1, 6, '/img/productos/4e6790fa-cd9c-4890-80f1-e5b02c965600.jpg'),
(6, 'Boya Luminosa', '15gr', 8000.00, 8, 0, 1, '/img/productos/a12c90d1-222f-42ff-b073-b2290c4a0c19.jpg'),
(7, 'Caja de pesca', 'Caster', 40000.00, 3, 0, 6, '/img/productos/e65f9a5e-d5b7-43b1-88a3-62a223a9ed45.jpg'),
(8, 'Wader', 'Impermeable', 50000.00, 4, 0, 5, '/img/productos/ccd537aa-8c44-4ace-b7a7-f659fadd86a0.jpg'),
(9, 'Caste Havoc 2005', 'Frontal', 43200.00, 2, 0, 2, '/img/productos/5085ed2e-fe7b-4115-b501-14b08f2c55b3.jpg'),
(10, 'Dakota Autoarmeble', '4 persona', 45000.00, 5, 0, 3, '/img/productos/43141cf4-5856-4f88-bbbc-9909d2f961cc.jpg'),
(11, 'Vincha Motoma', '450 Lumens', 15000.00, 9, 0, 4, '/img/productos/fbdc4330-6a0a-4f98-9bb8-cd7d00958d92.jpg'),
(12, 'Caña Caster', 'Telescopica Arrow 4m Carbono', 58000.00, 5, 0, 7, '/img/productos/513f0dae-02cf-45e3-8304-47ffd8ad2fa8.jpg'),
(13, 'Caña Spint Lancer', '4 Mts 3 tramos 200-300 gr', 80000.00, 4, 0, 7, '/img/productos/302559ea-4eaa-4821-8f21-550d56c19e50.jpg'),
(14, 'Kit X2 Boya', 'Bigotera Puntero 30cm', 28500.00, 13, 0, 1, '/img/productos/5d07fc4c-ee3c-4a7e-a446-4f92fd155aff.jpg'),
(15, 'Kit Caja de pesca', 'Accesorios completos', 42500.00, 4, 0, 6, '/img/productos/c4e67db4-fb14-40e4-86e6-c5a545fd8c08.jpg'),
(16, 'Remera Payo', 'Capucha + Filtro Proteccion Uv Secado Rapido', 45000.00, 8, 0, 5, '/img/productos/aa135769-be71-461e-bfc8-ffee003d8f16.jpg'),
(17, 'Lintena Tactica', 'Militar Led Lumify X9 Caza Camping Exterior Color Negro', 19990.00, 9, 0, 4, '/img/productos/d2208ebe-a628-4548-a1cd-6318000fa470.jpg'),
(18, 'Reels Shimano', 'Frontal FX 4000 FC derecho/izquierdo', 52300.00, 5, 0, 2, '/img/productos/aa96226b-ad94-476c-b478-fc7d09965736.jpg'),
(19, 'Anafe Dakota', 'Portatil Plegable Pesca Excursiones Gas Butano', 10500.00, 6, 0, 3, '/img/productos/2981361c-5357-4f85-b87e-9e2bcaa386d8.jpg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ventas`
--

CREATE TABLE `ventas` (
  `Id` int(11) NOT NULL,
  `Fecha` datetime NOT NULL,
  `Total` decimal(15,2) NOT NULL,
  `ClienteId` int(11) NOT NULL,
  `UsuarioId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ventas`
--

INSERT INTO `ventas` (`Id`, `Fecha`, `Total`, `ClienteId`, `UsuarioId`) VALUES
(1, '2025-07-28 18:22:23', 48000.00, 6, '04f75c34-8837-4a49-adc1-04b144d5c285'),
(2, '2025-07-29 00:33:30', 58200.00, 1, '04f75c34-8837-4a49-adc1-04b144d5c285');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`);

--
-- Indices de la tabla `aspnetroles`
--
ALTER TABLE `aspnetroles`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `RoleNameIndex` (`NormalizedName`);

--
-- Indices de la tabla `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetUserClaims_UserId` (`UserId`);

--
-- Indices de la tabla `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  ADD KEY `IX_AspNetUserLogins_UserId` (`UserId`);

--
-- Indices de la tabla `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD PRIMARY KEY (`UserId`,`RoleId`),
  ADD KEY `IX_AspNetUserRoles_RoleId` (`RoleId`);

--
-- Indices de la tabla `aspnetusers`
--
ALTER TABLE `aspnetusers`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  ADD KEY `EmailIndex` (`NormalizedEmail`);

--
-- Indices de la tabla `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD PRIMARY KEY (`UserId`,`LoginProvider`,`Name`);

--
-- Indices de la tabla `categorias`
--
ALTER TABLE `categorias`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `clientes`
--
ALTER TABLE `clientes`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `detallesventa`
--
ALTER TABLE `detallesventa`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_DetallesVenta_ProductoId` (`ProductoId`),
  ADD KEY `IX_DetallesVenta_VentaId` (`VentaId`);

--
-- Indices de la tabla `productos`
--
ALTER TABLE `productos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Productos_CategoriaId` (`CategoriaId`);

--
-- Indices de la tabla `ventas`
--
ALTER TABLE `ventas`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Ventas_ClienteId` (`ClienteId`),
  ADD KEY `IX_Ventas_UsuarioId` (`UsuarioId`);

--
-- Indices de la tabla `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `categorias`
--
ALTER TABLE `categorias`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `clientes`
--
ALTER TABLE `clientes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `detallesventa`
--
ALTER TABLE `detallesventa`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `productos`
--
ALTER TABLE `productos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT de la tabla `ventas`
--
ALTER TABLE `ventas`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD CONSTRAINT `aspnetuserroles_ibfk_1` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`),
  ADD CONSTRAINT `aspnetuserroles_ibfk_2` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`);

--
-- Filtros para la tabla `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `detallesventa`
--
ALTER TABLE `detallesventa`
  ADD CONSTRAINT `FK_DetallesVenta_Productos_ProductoId` FOREIGN KEY (`ProductoId`) REFERENCES `productos` (`Id`),
  ADD CONSTRAINT `FK_DetallesVenta_Ventas_VentaId` FOREIGN KEY (`VentaId`) REFERENCES `ventas` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `productos`
--
ALTER TABLE `productos`
  ADD CONSTRAINT `FK_Productos_Categorias_CategoriaId` FOREIGN KEY (`CategoriaId`) REFERENCES `categorias` (`Id`);

--
-- Filtros para la tabla `ventas`
--
ALTER TABLE `ventas`
  ADD CONSTRAINT `FK_Ventas_AspNetUsers_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `aspnetusers` (`Id`),
  ADD CONSTRAINT `FK_Ventas_Clientes_ClienteId` FOREIGN KEY (`ClienteId`) REFERENCES `clientes` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
