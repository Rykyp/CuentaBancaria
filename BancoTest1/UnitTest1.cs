using Moq;
using Xunit;
using CuentaBancaria;

namespace BancoTest1
{
    public class UnitTest1
    {
        [Fact]
        public void TC01_RetirarEfectivo_CantidadNegativa_LanzaError()
        {
            var mockAuditoria = new Mock<IServicioAuditoria>();
            var cuenta = new CuentaBancaria.CuentaBancaria(500, mockAuditoria.Object);

            Assert.Throws<ArgumentException>(() => cuenta.RetirarEfectivo(-10));
        }

        [Fact]
        public void TC02_RetirarEfectivo_CantidadCero_LanzaArgumentException()
        {
            var mockAuditoria = new Mock<IServicioAuditoria>();
            var cuenta = new CuentaBancaria.CuentaBancaria(500, mockAuditoria.Object);

            Assert.Throws<ArgumentException>(() => cuenta.RetirarEfectivo(0));
        }

        [Fact]
        public void TC03_RetirarEfectivo_SinComision_ActualizaSaldo()
        {
            var mockAuditoria = new Mock<IServicioAuditoria>();
            var cuenta = new CuentaBancaria.CuentaBancaria(500, mockAuditoria.Object);

            cuenta.RetirarEfectivo(20);

            Assert.Equal(480, cuenta.Saldo);
        }

        [Fact]
        public void TC04_RetirarEfectivo_49Euros_SinComision()
        {
            var mockAuditoria = new Mock<IServicioAuditoria>();
            var cuenta = new CuentaBancaria.CuentaBancaria(500, mockAuditoria.Object);

            cuenta.RetirarEfectivo(49);

            Assert.Equal(451, cuenta.Saldo);
        }

        [Fact]
        public void TC05_RetirarEfectivo_50Euros_Comision1()
        {
            var mockAuditoria = new Mock<IServicioAuditoria>();
            var cuenta = new CuentaBancaria.CuentaBancaria(500, mockAuditoria.Object);

            cuenta.RetirarEfectivo(50);

            Assert.Equal(449, cuenta.Saldo);

            mockAuditoria.Verify(x => x.NotificarRetirada(50), Times.Once);
        }

        [Fact]
        public void TC06_RetirarEfectivo_200Euros_Comision1()
        {
            var mockAuditoria = new Mock<IServicioAuditoria>();
            var cuenta = new CuentaBancaria.CuentaBancaria(500, mockAuditoria.Object);

            cuenta.RetirarEfectivo(200);

            Assert.Equal(299, cuenta.Saldo);

            mockAuditoria.Verify(x => x.NotificarRetirada(200), Times.Once);
        }

        [Fact]
        public void TC07_RetirarEfectivo_Mayor200_Comision3()
        {
            var mockAuditoria = new Mock<IServicioAuditoria>();
            var cuenta = new CuentaBancaria.CuentaBancaria(500, mockAuditoria.Object);

            cuenta.RetirarEfectivo(201);

            Assert.Equal(296, cuenta.Saldo);

            mockAuditoria.Verify(x => x.NotificarRetirada(201), Times.Once);
        }

        [Fact]
        public void TC08_RetirarEfectivo_SaldoInsuficiente_LanzaError()
        {
            var mockAuditoria = new Mock<IServicioAuditoria>();
            var cuenta = new CuentaBancaria.CuentaBancaria(100, mockAuditoria.Object);

            Assert.Throws<InvalidOperationException>(() => cuenta.RetirarEfectivo(150));
        }

        [Fact]
        public void TC09_RetirarEfectivo_MasDe600_LanzaInvalidOperationException()
        {
            var mockAuditoria = new Mock<IServicioAuditoria>();
            var cuenta = new CuentaBancaria.CuentaBancaria(1000, mockAuditoria.Object);

            Assert.Throws<InvalidOperationException>(() => cuenta.RetirarEfectivo(700));
        }

        [Fact]
        public void TC10_RetirarEfectivo_600Euros_Comision3()
        {
            var mockAuditoria = new Mock<IServicioAuditoria>();
            var cuenta = new CuentaBancaria.CuentaBancaria(603, mockAuditoria.Object);

            cuenta.RetirarEfectivo(600);

            Assert.Equal(0, cuenta.Saldo);

            mockAuditoria.Verify(x => x.NotificarRetirada(600), Times.Once);
        }
    }
}