using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entidades;

namespace PruebasAplicacion.Entidades
{
    [TestClass]
    public class OrderItemPruebas
    {
        /// <summary>
        /// Valida que un OrderItem correctamente configurado sea válido.
        /// </summary>
        [TestMethod]
        public void OrderItem_Valido_DeberiaSerValido()
        {
            // Arrange
            var item = new OrderItem
            {
                Id = 1,
                OrderId = 100,
                ProductName = "Laptop",
                Quantity = 2,
                UnitPrice = 750.50m
            };

            // Act
            var resultado = ValidarModelo(item);

            // Assert
            Assert.AreEqual(0, resultado.Count, "El ítem debería ser válido.");
            Assert.AreEqual(1501.00m, item.Subtotal, "El subtotal debe ser cantidad * precio unitario.");
        }


        /// <summary>
        /// Valida que el nombre del producto sea obligatorio.
        /// </summary>
        [TestMethod]
        public void OrderItem_SinNombreProducto_DeberiaSerInvalido()
        {
            // Arrange
            var item = new OrderItem
            {
                OrderId = 200,
                Quantity = 1,
                UnitPrice = 50.00m
            };

            // Act
            var resultado = ValidarModelo(item);

            // Assert
            Assert.IsTrue(resultado.Any(r => r.MemberNames.Contains("ProductName")), "El nombre del producto es obligatorio.");
        }

        /// <summary>
        /// Valida que el nombre del producto no supere los 100 caracteres.
        /// </summary>
        [TestMethod]
        public void OrderItem_NombreMuyLargo_DeberiaSerInvalido()
        {
            // Arrange
            var item = new OrderItem
            {
                OrderId = 201,
                ProductName = new string('A', 101),
                Quantity = 1,
                UnitPrice = 20.00m
            };

            // Act
            var resultado = ValidarModelo(item);

            // Assert
            Assert.IsTrue(resultado.Any(r => r.MemberNames.Contains("ProductName")), "El nombre del producto no puede superar los 100 caracteres.");
        }

        /// <summary>
        /// Valida que la cantidad mínima sea 1.
        /// </summary>
        [TestMethod]
        public void OrderItem_CantidadInvalida_DeberiaSerInvalido()
        {
            // Arrange
            var item = new OrderItem
            {
                OrderId = 300,
                ProductName = "Mouse",
                Quantity = 0, // inválido
                UnitPrice = 15.00m
            };

            // Act
            var resultado = ValidarModelo(item);

            // Assert
            Assert.IsTrue(resultado.Any(r => r.MemberNames.Contains("Quantity")), "La cantidad debe ser al menos 1.");
        }

        /// <summary>
        /// Valida que el precio unitario no pueda ser cero o negativo.
        /// </summary>
        [TestMethod]
        public void OrderItem_PrecioUnitarioInvalido_DeberiaSerInvalido()
        {
            // Arrange
            var item = new OrderItem
            {
                OrderId = 400,
                ProductName = "Monitor",
                Quantity = 1,
                UnitPrice = 0m // inválido
            };

            // Act
            var resultado = ValidarModelo(item);

            // Assert
            Assert.IsTrue(resultado.Any(r => r.MemberNames.Contains("UnitPrice")), "El precio unitario debe ser mayor que cero.");
        }

        /// <summary>
        /// Método auxiliar para validar modelos con anotaciones.
        /// </summary>
        private IList<ValidationResult> ValidarModelo(object modelo)
        {
            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(modelo, null, null);
            Validator.TryValidateObject(modelo, contexto, resultados, true);
            return resultados;
        }
    }
}
