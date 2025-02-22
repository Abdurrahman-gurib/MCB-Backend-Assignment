namespace MCBBackend.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int OrderID { get; set; }
        public string InvoiceRef { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }

        public virtual Order Order { get; set; }
    }
}
