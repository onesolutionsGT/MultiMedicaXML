using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class Pago
    {
        public Pago()
        {
            this.PagoContado = new Contado();
            this.PagoTransferencia = new Transferencia();
            this.PagoCheque = new List<Cheque>();
            this.PagoTarjeta = new List<Tarjeta>();
        }
        private DateTime docDate;
        private string cardCode;
        private string currency;
        private double rate;
        private string docEntry;
        private string comentario;
        private Contado pagoContado;
        private Transferencia pagoTransferencia;
        private List<Cheque> pagoCheque;
        private List<Tarjeta> pagoTarjeta;

        public DateTime DocDate { get => docDate; set => docDate = value; }
        public string CardCode { get => cardCode; set => cardCode = value; }
        public string Currency { get => currency; set => currency = value; }
        public double Rate { get => rate; set => rate = value; }
        public string DocEntry { get => docEntry; set => docEntry = value; }
        public string Comentario { get => comentario; set => comentario = value; }
        public Contado PagoContado { get => pagoContado; set => pagoContado = value; }
        public Transferencia PagoTransferencia { get => pagoTransferencia; set => pagoTransferencia = value; }
        public List<Cheque> PagoCheque { get => pagoCheque; set => pagoCheque = value; }
        public List<Tarjeta> PagoTarjeta { get => pagoTarjeta; set => pagoTarjeta = value; }
    }

    public class Contado
    {
        public Double CashSum;
    }

    public class Cheque
    {
        private string bankCode;
        private int checkNumber;
        private string accounttNum;
        private DateTime dueDate;
        private Double checkSum;

        public string BankCode { get => bankCode; set => bankCode = value; }
        public int CheckNumber { get => checkNumber; set => checkNumber = value; }
        public string AccounttNum { get => accounttNum; set => accounttNum = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public double CheckSum { get => checkSum; set => checkSum = value; }
    }

    public class Transferencia
    {
        private DateTime transferDate;
        private string transferReference;
        private string transferAccount;
        private Double transferSum;

        public DateTime TransferDate { get => transferDate; set => transferDate = value; }
        public string TransferReference { get => transferReference; set => transferReference = value; }
        public string TransferAccount { get => transferAccount; set => transferAccount = value; }
        public double TransferSum { get => transferSum; set => transferSum = value; }
    }

    public class Tarjeta
    {
        private int creditCard;
        private string creditCardNumber;
        private double creditSum;
        private string voucherNum;
        private DateTime cardValidUntil;

        public int CreditCard { get => creditCard; set => creditCard = value; }
        public string CreditCardNumber { get => creditCardNumber; set => creditCardNumber = value; }
        public double CreditSum { get => creditSum; set => creditSum = value; }
        public string VoucherNum { get => voucherNum; set => voucherNum = value; }
        public DateTime CardValidUntil { get => cardValidUntil; set => cardValidUntil = value; }
    }
}
