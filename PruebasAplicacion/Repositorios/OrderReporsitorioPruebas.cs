using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entidades;
using Infrasturura.Data;
using Infrasturura.Reporsitorios;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace PruebasAplicacion.Repositorios
{
    [TestClass]
    public class OrderReporsitorioPruebas
    {
        private AppDbContext _context = null!;
        private OrderRepository _repository = null!;

        /// <summary>
        /// Inicializa el contexto en memoria y el repositorio antes de cada prueba.
        /// </summary>
        [TestInitialize]
        public void Inicializar()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nueva BD para cada prueba
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();

            _repository = new OrderRepository(_context);
        }

        /// <summary>
        /// Verifica que se retorne error si se consulta un ID inválido (menor o igual a cero).
        /// </summary>
        [TestMethod]
        public async Task ConsultarPorId_DeberiaRetornarError_SiIdEsInvalido()
        {
            var resultado = await _repository.ConsultarPorId(0);

            Assert.IsTrue(resultado.IsFailed);
            Assert.AreEqual("El ID debe ser mayor que cero.", resultado.Errors[0].Message);
        }

        /// <summary>
        /// Verifica que se retorne error si no se encuentra una orden con el ID proporcionado.
        /// </summary>
        [TestMethod]
        public async Task ConsultarPorId_DeberiaRetornarError_SiOrdenNoExiste()
        {
            var resultado = await _repository.ConsultarPorId(999);

            Assert.IsTrue(resultado.IsFailed);
            Assert.AreEqual("No se encontró una orden con ID 999.", resultado.Errors[0].Message);
        }

        /// <summary>
        /// Verifica que se retorne correctamente una orden existente por su ID.
        /// </summary>
        [TestMethod]
        public async Task ConsultarPorId_DeberiaRetornarOrden_SiExiste()
        {
            var orden = new Order { Id = 1, CustomerId = 1, Customer = new() { Id=1}};
            _context.Orders.Add(orden);
            _context.SaveChanges();

            var resultado = await _repository.ConsultarPorId(1);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(orden.Id, resultado.Value.Id);
        }

        /// <summary>
        /// Verifica que no se permita crear una orden nula o sin ítems.
        /// </summary>
        [TestMethod]
        public async Task CrearOrder_DeberiaRetornarError_SiOrdenEsInvalida()
        {
            var resultado = await _repository.CrearOrder(null!);

            Assert.IsTrue(resultado.IsFailed);
            Assert.AreEqual("La orden debe contener al menos un ítem.", resultado.Errors[0].Message);
        }

        /// <summary>
        /// Verifica que se retorne error si se intenta crear una orden con un cliente inexistente.
        /// </summary>
        [TestMethod]
        public async Task CrearOrder_DeberiaRetornarError_SiClienteNoExiste()
        {
            var orden = new Order
            {
                CustomerId = 99,
                Items = new List<OrderItem> { new OrderItem() }
            };

            var resultado = await _repository.CrearOrder(orden);

            Assert.IsTrue(resultado.IsFailed);
            Assert.AreEqual("No se encontró un cliente con ID 99.", resultado.Errors[0].Message);
        }

        /// <summary>
        /// Verifica que se pueda crear una orden si todos los datos son válidos.
        /// </summary>
        [TestMethod]
        public async Task CrearOrder_DeberiaCrearOrden_SiDatosSonValidos()
        {
            var cliente = new Customer { Id = 1 };
            await _context.Customers.AddAsync(cliente);
            await _context.SaveChangesAsync();

            var orden = new Order
            {
                CustomerId = 1,
                Items = new List<OrderItem> { new OrderItem() }
            };

            var resultado = await _repository.CrearOrder(orden);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual("Orden creada exitosamente.", resultado.Value);
        }

        /// <summary>
        /// Verifica que se retorne error si se intenta actualizar una orden nula.
        /// </summary>
        [TestMethod]
        public async Task ActualizarOrder_DeberiaRetornarError_SiOrdenEsInvalida()
        {
            var resultado = await _repository.ActualizarOrder(null!);

            Assert.IsTrue(resultado.IsFailed);
        }

        /// <summary>
        /// Verifica que se retorne error si el cliente asociado a la orden no existe.
        /// </summary>
        [TestMethod]
        public async Task ActualizarOrder_DeberiaRetornarError_SiClienteNoExiste()
        {
            var orden = new Order { Id = 1, CustomerId = 999 };

            var resultado = await _repository.ActualizarOrder(orden);

            Assert.IsTrue(resultado.IsFailed);
            Assert.AreEqual("No se encontró un cliente con ID 999.", resultado.Errors[0].Message);
        }

        /// <summary>
        /// Verifica que se pueda actualizar una orden existente con datos válidos.
        /// </summary>
        [TestMethod]
        public async Task ActualizarOrder_DeberiaActualizarOrden_SiDatosSonValidos()
        {
            var cliente = new Customer { Id = 2 };
            await _context.Customers.AddAsync(cliente);
            await _context.SaveChangesAsync();

            var orden = new Order { Id = 1, CustomerId = 2 };
            _context.Orders.Add(orden);
            await _context.SaveChangesAsync();

            orden.CustomerId = 2;

            var resultado = await _repository.ActualizarOrder(orden);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual("Orden actualizada correctamente.", resultado.Value);
        }

        /// <summary>
        /// Verifica que se retorne error si se intenta eliminar una orden que no existe.
        /// </summary>
        [TestMethod]
        public async Task EliminarOrder_DeberiaRetornarError_SiOrdenNoExiste()
        {
            var resultado = await _repository.EliminarOrder(100);

            Assert.IsTrue(resultado.IsFailed);
            Assert.AreEqual("No se encontró la orden con ID 100.", resultado.Errors[0].Message);
        }

        /// <summary>
        /// Verifica que se pueda eliminar una orden existente correctamente.
        /// </summary>
        [TestMethod]
        public async Task EliminarOrder_DeberiaEliminarOrden_SiExiste()
        {
            var orden = new Order { Id = 1 };
            await _context.Orders.AddAsync(orden);
            await _context.SaveChangesAsync();

            var resultado = await _repository.EliminarOrder(1);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual("Orden eliminada exitosamente.", resultado.Value);
        }
    }
}
