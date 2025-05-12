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
    public class OrderPruebas
    {
        /// <summary>
        /// Valida que una orden correctamente construida sea válida.
        /// </summary>
        [TestMethod]
        public void Orden_Valida_DeberiaSerValida()
        {
            // Arrange
            var orden = new Order
            {
                Id = 1,
                CustomerId = 100,
                CreatedAt = DateTime.UtcNow,
                DeliveryDate = DateTime.UtcNow.AddDays(3),
                Status = "Pendiente",
                TotalAmount = 150.75m,
                Notes = "Entregar en la mañana.",
                Items = new List<OrderItem> { new OrderItem() }
            };

            // Act
            var resultado = ValidarModelo(orden);

            // Assert
            Assert.AreEqual(0, resultado.Count, "La orden debería ser válida.");
        }

        /// <summary>
        /// Valida que el Status no supere los 50 caracteres.
        /// </summary>
        [TestMethod]
        public void Orden_StatusMuyLargo_DeberiaSerInvalida()
        {
            // Arrange
            var orden = new Order
            {
                CustomerId = 101,
                CreatedAt = DateTime.UtcNow,
                Status = new string('A', 51), // 51 caracteres
                TotalAmount = 200.00m,
                Items = new List<OrderItem> { new OrderItem() }
            };

            // Act
            var resultado = ValidarModelo(orden);

            // Assert
            Assert.IsTrue(resultado.Any(v => v.MemberNames.Contains("Status")), "El estado no debe superar los 50 caracteres.");
        }

        /// <summary>
        /// Valida que el TotalAmount no pueda ser cero o negativo.
        /// </summary>
        [TestMethod]
        public void Orden_TotalAmountInvalido_DeberiaSerInvalida()
        {
            // Arrange
            var orden = new Order
            {
                CustomerId = 102,
                CreatedAt = DateTime.UtcNow,
                Status = "Pendiente",
                TotalAmount = 0m,
                Items = new List<OrderItem> { new OrderItem() }
            };

            // Act
            var resultado = ValidarModelo(orden);

            // Assert
            Assert.IsTrue(resultado.Any(v => v.MemberNames.Contains("TotalAmount")), "El total debe ser mayor que cero.");
        }

        /// <summary>
        /// Valida que las notas no superen los 500 caracteres.
        /// </summary>
        [TestMethod]
        public void Orden_NotaMuyLarga_DeberiaSerInvalida()
        {
            // Arrange
            var orden = new Order
            {
                CustomerId = 103,
                CreatedAt = DateTime.UtcNow,
                Status = "Pendiente",
                TotalAmount = 90.00m,
                Notes = new string('N', 501),
                Items = new List<OrderItem> { new OrderItem() }
            };

            // Act
            var resultado = ValidarModelo(orden);

            // Assert
            Assert.IsTrue(resultado.Any(v => v.MemberNames.Contains("Notes")), "Las notas no deben superar los 500 caracteres.");
        }

        /// <summary>
        /// Valida que una orden sin ítems sea inválida.
        /// </summary>
        [TestMethod]
        public void Orden_SinItems_DeberiaSerInvalida()
        {
            // Arrange
            var orden = new Order
            {
                CustomerId = 104,
                CreatedAt = DateTime.UtcNow,
                Status = "Pendiente",
                TotalAmount = 120.00m,
                Items = new List<OrderItem>() // vacío
            };

            // Act
            var resultado = ValidarModelo(orden);

            // Assert
            Assert.IsTrue(resultado.Any(v => v.MemberNames.Contains("Items")), "La orden debe contener al menos un ítem.");
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