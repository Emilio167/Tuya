using Domain.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PruebasAplicacion.Entidades
{
    [TestClass]
    public class CustomerPruebas
    {
        /// <summary>
        /// Valida que un cliente con todos los datos correctos sea válido.
        /// </summary>
        [TestMethod]
        public void Cliente_Valido_DeberiaSerValido()
        {
            // Arrange
            var cliente = new Customer
            {
                Id = 1,
                Name = "Juan Pérez",
                Email = "juan.perez@correo.com",
                FechaRegistro = DateTime.UtcNow
            };

            // Act
            var resultado = ValidarModelo(cliente);

            // Assert
            Assert.AreEqual(0, resultado.Count, "El cliente debería ser válido.");
        }

        /// <summary>
        /// Valida que el nombre sea obligatorio.
        /// </summary>
        [TestMethod]
        public void Cliente_SinNombre_DeberiaSerInvalido()
        {
            // Arrange
            var cliente = new Customer
            {
                Id = 2,
                Name = "",
                Email = "cliente@correo.com"
            };

            // Act
            var resultado = ValidarModelo(cliente);

            // Assert
            Assert.IsTrue(resultado.Any(v => v.MemberNames.Contains("Name")), "El nombre es obligatorio.");
        }

        /// <summary>
        /// Valida que el correo electrónico sea obligatorio.
        /// </summary>
        [TestMethod]
        public void Cliente_SinCorreo_DeberiaSerInvalido()
        {
            // Arrange
            var cliente = new Customer
            {
                Id = 3,
                Name = "Ana García",
                Email = ""
            };

            // Act
            var resultado = ValidarModelo(cliente);

            // Assert
            Assert.IsTrue(resultado.Any(v => v.MemberNames.Contains("Email")), "El correo electrónico es obligatorio.");
        }

        /// <summary>
        /// Valida que un correo con formato inválido sea rechazado.
        /// </summary>
        [TestMethod]
        public void Cliente_CorreoInvalido_DeberiaSerInvalido()
        {
            // Arrange
            var cliente = new Customer
            {
                Id = 4,
                Name = "Carlos Ruiz",
                Email = "correo-sin-arroba"
            };

            // Act
            var resultado = ValidarModelo(cliente);

            // Assert
            Assert.IsTrue(resultado.Any(v => v.MemberNames.Contains("Email")), "El correo debe tener un formato válido.");
        }

        /// <summary>
        /// Valida que un nombre muy largo sea inválido.
        /// </summary>
        [TestMethod]
        public void Cliente_NombreMuyLargo_DeberiaSerInvalido()
        {
            // Arrange
            var cliente = new Customer
            {
                Id = 5,
                Name = new string('A', 101), // más de 100 caracteres
                Email = "cliente@correo.com"
            };

            // Act
            var resultado = ValidarModelo(cliente);

            // Assert
            Assert.IsTrue(resultado.Any(v => v.MemberNames.Contains("Name")), "El nombre no puede superar los 100 caracteres.");
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