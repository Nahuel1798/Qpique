🐟 Narrativa del Proyecto
Sistema de Gestión para Casa de Venta de Pesca "Qpique"
________________________________________
🎯 Objetivo del Proyecto
Desarrollar una aplicación web integral utilizando ASP.NET Core para gestionar las operaciones comerciales de la casa de venta de pesca "Qpique". El sistema busca digitalizar y optimizar procesos como administración de productos, ventas, clientes, stock y reportes, brindando una herramienta moderna, segura y fácil de usar.
________________________________________
🧩 Alcance del Sistema
El sistema está diseñado para cubrir las necesidades operativas de un negocio que comercializa artículos de pesca deportiva y profesional. Incluye:
•	Gestión de Productos: Alta, baja y modificación de productos con categorías (cañas, reels, señuelos, etc.), precios y stock.
•	Control de Inventario: Actualización automática del stock al realizar ventas, alertas por bajo stock, generación de reportes.
•	Gestión de Clientes: Registro de clientes con historial de compras y datos de contacto.
•	Ventas y Facturación: Carga rápida de ventas, selección de productos, cálculo automático de totales y generación de comprobantes.
•	Reportes y Estadísticas: Informes de ventas por período, productos más vendidos, ganancias y stock, informe de comisión .
•	Usuarios y Roles: Control de acceso mediante autenticación, roles diferenciados y auditoría.
________________________________________
⚙️ Tecnologías Utilizadas
•	Backend: ASP.NET Core MVC
•	Frontend: Razor + Vue.js (para un módulo)
•	ORM: Entity Framework Core
•	Base de Datos: MySQL
•	Autenticación y Autorización: Identity con roles y JWT
•	Gestión de Archivos: Subida de avatares y comprobantes
•	UI: Bootstrap, DataTables, Vue.js + Axios
________________________________________
👤 Usuarios del Sistema
•	Administrador: Gestiona usuarios, productos, reportes y puede realizar ventas.
•	Vendedor: Accede solo al módulo de ventas y visualización de productos.
________________________________________
🧪 Casos de Uso Principales
•	Registrar nueva venta
El vendedor selecciona productos, ingresa cantidad, el sistema calcula totales y actualiza el stock.
•	Agregar nuevo producto al catálogo
El administrador registra nombre, descripción, categoría, proveedor, precio y stock.
•	Generar reporte de ventas del mes
Permite filtrar ventas por fecha, mostrar resultados en gráficos y exportar en PDF o Excel.
________________________________________
📈 Beneficios del Sistema
•	Mejora la eficiencia en la gestión de ventas e inventario
•	Reduce errores humanos
•	Facilita la toma de decisiones basadas en datos
•	Permite análisis de comportamiento de compra
•	Mejora la atención al cliente
________________________________________
🏁 Estado Actual y Futuras Mejoras
El sistema ya cuenta con las funcionalidades básicas completas y está listo para su implementación.
Futuras mejoras:
•	Integración con medios de pago online
•	Gestión de pedidos y envíos
•	Tienda virtual para ventas por internet
•	Aplicación móvil para vendedores
________________________________________
📊 Clases / Tablas Principales
•	Usuario
o	Tiene avatar
o	Relación un Rol
o	Login y autorización
•	Producto
o	Pertenece a una Categoría (FK)
o	Se relaciona con DetalleVenta
•	Venta
o	Relación 1:N con DetalleVenta
o	Pertenece a un Cliente
o	Subida de comprobante (PDF o imagen)
o	Auditoria de quien vendio
•	DetalleVenta
o	Muchos por cada Venta
o	Incluye cantidad y precio
•	Cliente
o	Tiene muchas Ventas
•	Categoría
o	Relación 1:N con Producto
________________________________________
🔐 Seguridad (Login, Roles y Archivos)
•	Autenticación con ASP.NET Core Identity
•	Roles: Administrador, Vendedor
•	Funcionalidades restringidas:
o	Solo el Administrador puede agregar o editar productos y ventas
o	El Vendedor solo puede registrar ventas y subir comprobantes
•	Subida de avatar en el perfil del usuario
•	Comprobantes de venta se cargan como archivos PDF o imagen
•	Autenticación por cookies y API protegida con JWT

