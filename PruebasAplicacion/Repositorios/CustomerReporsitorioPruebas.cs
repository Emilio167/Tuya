using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infrasturura.Reporsitorios;
using Infrasturura.Data;
using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace PruebasAplicacion.Repositorios
{
    [TestClass]
    public class CustomerRepositorioPruebas
    {
        private AppDbContext _context=null!;
        private CustomerReporsitorio _repositorio = null!;

        /// <summary>
        /// Inicializa el contexto en memoria antes de cada prueba.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CustomerDbTest")
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repositorio = new CustomerReporsitorio(_context);
        }

        /// <summary>
        /// Prueba que se puedan consultar todos los clientes sin filtros.
        /// </summary>
        [TestMethod]
        public async Task ConsultarTodos_SinFiltros_DeberiaRetornarTodos()
        {
            // Arrange
            _context.Customers.Add(new Customer { Name = "Juan", Email = "juan@mail.com" });
            _context.Customers.Add(new Customer { Name = "Maria", Email = "maria@mail.com" });
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _repositorio.ConsultarTodos(null, null, null);

            // Assert
            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(2, resultado.Value.Count());
        }

        /// <summary>
        /// Prueba que se filtre por ID correctamente.
        /// </summary>
        [TestMethod]
        public async Task ConsultarTodos_ConId_DeberiaFiltrarPorId()
        {
            // Arrange
            var customer = new Customer { Name = "Carlos", Email = "carlos@mail.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _repositorio.ConsultarTodos(customer.Id, null, null);

            // Assert
            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual("Carlos", resultado.Value.First().Name);
        }

        /// <summary>
        /// Prueba que se pueda consultar un cliente por su ID.
        /// </summary>
        [TestMethod]
        public async Task ConsultarPorId_ClienteExistente_DeberiaRetornarCliente()
        {
            // Arrange
            var customer = new Customer { Name = "Laura", Email = "laura@mail.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _repositorio.ConsultarPorId(customer.Id);

            // Assert
            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual("Laura", resultado.Value.Name);
        }

        /// <summary>
        /// Prueba que se retorne error si se consulta un cliente inexistente.
        /// </summary>
        [TestMethod]
        public async Task ConsultarPorId_ClienteNoExistente_DeberiaRetornarError()
        {
            var resultado = await _repositorio.ConsultarPorId(999);
            Assert.IsTrue(resultado.IsFailed);
        }

        /// <summary>
        /// Prueba que se pueda guardar un nuevo cliente correctamente.
        /// </summary>
        [TestMethod]
        public async Task GuardarCustomer_ClienteValido_DeberiaGuardarCorrectamente()
        {
            var customer = new Customer { Name = "Pedro", Email = "pedro@mail.com" };

            var resultado = await _repositorio.GuardarCustomer(customer);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(1, _context.Customers.Count());
        }

        /// <summary>
        /// Prueba que falle si el cliente es nulo al guardar.
        /// </summary>
        [TestMethod]
        public async Task GuardarCustomer_ClienteNulo_DeberiaFallar()
        {
            var resultado = await _repositorio.GuardarCustomer(null);
            Assert.IsTrue(resultado.IsFailed);
        }

        /// <summary>
        /// Prueba que se actualice correctamente un cliente existente.
        /// </summary>
        [TestMethod]
        public async Task ActualizarCustomer_ClienteExistente_DeberiaActualizarCorrectamente()
        {
            var customer = new Customer { Name = "Andrés", Email = "andres@mail.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            customer.Name = "Andrés Actualizado";
            var resultado = await _repositorio.ActualizarCustomer(customer);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual("Andrés Actualizado", _context.Customers.First().Name);
        }

        /// <summary>
        /// Prueba que se elimine correctamente un cliente existente.
        /// </summary>
        [TestMethod]
        public async Task EliminarCustomerPorId_ClienteExistente_DeberiaEliminarCorrectamente()
        {
            var customer = new Customer { Name = "Eliminar", Email = "eliminar@mail.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var resultado = await _repositorio.EliminarCustomerPorId(customer.Id);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(0, _context.Customers.Count());
        }

        /// <summary>
        /// Prueba que falle al intentar eliminar un cliente con ID inválido.
        /// </summary>
        [TestMethod]
        public async Task EliminarCustomerPorId_IdInvalido_DeberiaFallar()
        {
            var resultado = await _repositorio.EliminarCustomerPorId(0);
            Assert.IsTrue(resultado.IsFailed);
        }
    }
}