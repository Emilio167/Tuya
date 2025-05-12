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

namespace PruebasAplicacion.Controladores
{
    [TestClass]
    public class CustomerControllerPruebas
    {
        private Mock<ICustomerRepository> _mockRepo = null!;
        private CustomerController _controller = null!;

        /// <summary>
        /// Inicializa los mocks y el controlador antes de cada prueba.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _controller = new CustomerController(_mockRepo.Object);
        }

        /// <summary>
        /// Verifica que ConsultarTodos retorne OK con una lista de clientes.
        /// </summary>
        [TestMethod]
        public async Task ConsultarTodos_DeberiaRetornarOkConClientes()
        {
            var clientes = new List<Customer> { new() { Id = 1, Name = "Cliente 1" } };
            _mockRepo.Setup(r => r.ConsultarTodos(null, null, null))
                     .ReturnsAsync(clientes);

            var resultado = await _controller.ConsultarTodos(null, null, null);

            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));
        }

        /// <summary>
        /// Verifica que ConsultarTodos retorne error 500 si falla el repositorio.
        /// </summary>
        [TestMethod]
        public async Task ConsultarTodos_DeberiaRetornar500SiFalla()
        {
            _mockRepo.Setup(r => r.ConsultarTodos(null, null, null))
                     .ReturnsAsync(Result.Fail("Error"));

            var resultado = await _controller.ConsultarTodos(null, null, null);

            var objectResult = resultado as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        /// <summary>
        /// Verifica que ConsultarPorId retorne OK si el cliente existe.
        /// </summary>
        [TestMethod]
        public async Task ConsultarPorId_DeberiaRetornarOkSiExiste()
        {
            var cliente = new Customer { Id = 1 };
            _mockRepo.Setup(r => r.ConsultarPorId(1))
                     .ReturnsAsync(Result.Ok(cliente));

            var resultado = await _controller.ConsultarPorId(1);

            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));
        }

        /// <summary>
        /// Verifica que ConsultarPorId retorne NotFound si falla.
        /// </summary>
        [TestMethod]
        public async Task ConsultarPorId_DeberiaRetornarNotFoundSiNoExiste()
        {
            _mockRepo.Setup(r => r.ConsultarPorId(999))
                     .ReturnsAsync(Result.Fail("No encontrado"));

            var resultado = await _controller.ConsultarPorId(999);

            Assert.IsInstanceOfType(resultado, typeof(NotFoundObjectResult));
        }

        /// <summary>
        /// Verifica que GuardarCustomer retorne CreatedAtAction si se guarda correctamente.
        /// </summary>
        [TestMethod]
        public async Task GuardarCustomer_DeberiaRetornarCreatedSiEsExitoso()
        {
            var cliente = new Customer { Id = 1, Name = "Nuevo Cliente" };
            _mockRepo.Setup(r => r.GuardarCustomer(cliente))
                     .ReturnsAsync(Result.Ok());

            var resultado = await _controller.GuardarCustomer(cliente);

            Assert.IsInstanceOfType(resultado, typeof(CreatedAtActionResult));
        }

        /// <summary>
        /// Verifica que GuardarCustomer retorne BadRequest si falla el guardado.
        /// </summary>
        [TestMethod]
        public async Task GuardarCustomer_DeberiaRetornarBadRequestSiFalla()
        {
            var cliente = new Customer();
            _mockRepo.Setup(r => r.GuardarCustomer(cliente))
                     .ReturnsAsync(Result.Fail("Error"));

            var resultado = await _controller.GuardarCustomer(cliente);

            Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// Verifica que ActualizarCustomer retorne CreatedAtAction si se actualiza correctamente.
        /// </summary>
        [TestMethod]
        public async Task ActualizarCustomer_DeberiaRetornarCreatedSiEsExitoso()
        {
            var cliente = new Customer { Id = 1, Name = "Actualizado" };
            _mockRepo.Setup(r => r.ActualizarCustomer(cliente))
                     .ReturnsAsync(Result.Ok());

            var resultado = await _controller.ActualizarCustomer(cliente);

            Assert.IsInstanceOfType(resultado, typeof(CreatedAtActionResult));
        }

        /// <summary>
        /// Verifica que ActualizarCustomer retorne BadRequest si falla.
        /// </summary>
        [TestMethod]
        public async Task ActualizarCustomer_DeberiaRetornarBadRequestSiFalla()
        {
            var cliente = new Customer();
            _mockRepo.Setup(r => r.ActualizarCustomer(cliente))
                     .ReturnsAsync(Result.Fail("Error"));

            var resultado = await _controller.ActualizarCustomer(cliente);

            Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// Verifica que EliminarCustomerPorIdConsultarPorId retorne OK si se elimina correctamente.
        /// </summary>
        [TestMethod]
        public async Task EliminarCustomer_DeberiaRetornarOkSiSeElimina()
        {
            _mockRepo.Setup(r => r.EliminarCustomerPorId(1))
                     .ReturnsAsync(Result.Ok("Eliminado"));

            var resultado = await _controller.EliminarCustomerPorIdConsultarPorId(1);

            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));
        }

        /// <summary>
        /// Verifica que EliminarCustomerPorIdConsultarPorId retorne BadRequest si falla.
        /// </summary>
        [TestMethod]
        public async Task EliminarCustomer_DeberiaRetornarBadRequestSiFalla()
        {
            _mockRepo.Setup(r => r.EliminarCustomerPorId(1))
                     .ReturnsAsync(Result.Fail("No se pudo eliminar"));

            var resultado = await _controller.EliminarCustomerPorIdConsultarPorId(1);

            Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));
        }
    }
}