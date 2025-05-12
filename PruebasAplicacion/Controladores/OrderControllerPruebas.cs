using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entidades;
using Domain.Interfases;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tuya.Controllers;

namespace PruebasAplicacion.Controladores
{
    [TestClass]
    public class OrderControllerPruebas
    {
        private Mock<IOrderRepository> _mockRepo=null!;
        private OrderController _controller = null!;

        /// <summary>
        /// Inicializa los mocks y el controlador antes de cada prueba.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IOrderRepository>();
            _controller = new OrderController(_mockRepo.Object);
        }

        /// <summary>
        /// Verifica que ObtenerTodas retorne OK con la lista de órdenes.
        /// </summary>
        [TestMethod]
        public async Task ObtenerTodas_DeberiaRetornarOkConListaDeOrdenes()
        {
            var ordenes = new[] { new Order { Id = 1, Notes = "Orden 1" }, new Order { Id = 2, Notes = "Orden 2" } };
            _mockRepo.Setup(r => r.ConsultarTodos(null,null,null,null)).ReturnsAsync(ordenes);

            var resultado = await _controller.ConsultarTodos(null, null, null, null);

            var objectResult = resultado as OkObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        /// <summary>
        /// Verifica que ObtenerTodas retorne un error si falla.
        /// </summary>
        [TestMethod]
        public async Task ObtenerTodas_DeberiaRetornarErrorSiFalla()
        {
            _mockRepo.Setup(r => r.ConsultarTodos(null, null, null, null)).ReturnsAsync(Result.Fail("Error"));

            var resultado = await _controller.ConsultarTodos();

            var objectResult = resultado as ObjectResult;
            Assert.IsNotNull(objectResult);
        }

        /// <summary>
        /// Verifica que ObtenerPorId retorne OK si la orden existe.
        /// </summary>
        [TestMethod]
        public async Task ObtenerPorId_DeberiaRetornarOkSiExiste()
        {
            var orden = new Order { Id = 1, Notes = "Orden 1" };
            _mockRepo.Setup(r => r.ConsultarPorId(1)).ReturnsAsync(Result.Ok(orden));

            var resultado = await _controller.ObtenerPorId(1);

            var objectResult = resultado as OkObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        /// <summary>
        /// Verifica que ObtenerPorId retorne NotFound si no encuentra la orden.
        /// </summary>
        [TestMethod]
        public async Task ObtenerPorId_DeberiaRetornarNotFoundSiNoExiste()
        {
            _mockRepo.Setup(r => r.ConsultarPorId(999)).ReturnsAsync(Result.Fail("No encontrado"));

            var resultado = await _controller.ObtenerPorId(999);

            var objectResult = resultado as NotFoundObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        /// <summary>
        /// Verifica que CrearOrden retorne OK si se crea la orden correctamente.
        /// </summary>
        [TestMethod]
        public async Task CrearOrden_DeberiaRetornarOkSiEsExitoso()
        {
            var orden = new Order { Id = 1, Notes = "Nueva Orden" };
            _mockRepo.Setup(r => r.CrearOrder(orden)).ReturnsAsync(Result.Ok("Orden creada"));

            var resultado = await _controller.CrearOrden(orden);

            var objectResult = resultado as OkObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        /// <summary>
        /// Verifica que CrearOrden retorne BadRequest si falla al crear la orden.
        /// </summary>
        [TestMethod]
        public async Task CrearOrden_DeberiaRetornarBadRequestSiFalla()
        {
            var orden = new Order { Id = 1, Notes = "Nueva Orden" };
            _mockRepo.Setup(r => r.CrearOrder(orden)).ReturnsAsync(Result.Fail("Error al crear"));

            var resultado = await _controller.CrearOrden(orden);

            var objectResult = resultado as BadRequestObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        /// <summary>
        /// Verifica que ActualizarOrden retorne OK si se actualiza correctamente.
        /// </summary>
        [TestMethod]
        public async Task ActualizarOrden_DeberiaRetornarOkSiEsExitoso()
        {
            var orden = new Order { Id = 1, Notes = "Orden Actualizada" };
            _mockRepo.Setup(r => r.ActualizarOrder(orden)).ReturnsAsync(Result.Ok("Orden actualizada"));

            var resultado = await _controller.ActualizarOrden(1, orden);

            var objectResult = resultado as OkObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        /// <summary>
        /// Verifica que ActualizarOrden retorne BadRequest si falla.
        /// </summary>
        [TestMethod]
        public async Task ActualizarOrden_DeberiaRetornarBadRequestSiFalla()
        {
            var orden = new Order { Id = 1, Notes = "Orden Actualizada" };
            _mockRepo.Setup(r => r.ActualizarOrder(orden)).ReturnsAsync(Result.Fail("Error al actualizar"));

            var resultado = await _controller.ActualizarOrden(1, orden);

            var objectResult = resultado as BadRequestObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        /// <summary>
        /// Verifica que EliminarOrden retorne OK si se elimina correctamente.
        /// </summary>
        [TestMethod]
        public async Task EliminarOrden_DeberiaRetornarOkSiEsExitoso()
        {
            _mockRepo.Setup(r => r.EliminarOrder(1)).ReturnsAsync(Result.Ok("Orden eliminada"));

            var resultado = await _controller.EliminarOrden(1);

            var objectResult = resultado as OkObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        /// <summary>
        /// Verifica que EliminarOrden retorne NotFound si falla.
        /// </summary>
        [TestMethod]
        public async Task EliminarOrden_DeberiaRetornarNotFoundSiNoExiste()
        {
            _mockRepo.Setup(r => r.EliminarOrder(999)).ReturnsAsync(Result.Fail("No encontrada"));

            var resultado = await _controller.EliminarOrden(999);

            var objectResult = resultado as NotFoundObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }
    }
}