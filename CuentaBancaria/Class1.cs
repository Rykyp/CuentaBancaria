namespace CuentaBancaria;

public class CuentaBancaria
{
    public decimal Saldo { get; private set; }
    private IServicioAuditoria auditoria;

    public CuentaBancaria(decimal saldoInicial, IServicioAuditoria auditoria)
    {
        Saldo = saldoInicial;
 
        this.auditoria = auditoria;
    }

    public void RetirarEfectivo(decimal cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException();

        if (cantidad > 600)
            throw new InvalidOperationException();

        decimal comision = 0;

        if (cantidad < 50)
            comision = 0;
        else if (cantidad <= 200)
            comision = 1;
        else
            comision = 3;

        if (Saldo < cantidad + comision)
            throw new InvalidOperationException();

        Saldo -= (cantidad + comision);

        if (comision > 0)
        {
            auditoria.NotificarRetirada(cantidad);
        }
    }
}