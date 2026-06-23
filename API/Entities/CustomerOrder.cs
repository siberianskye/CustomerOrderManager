namespace CustomerOrderManager.Business.Entities
{
    public class CustomerOrder
    {
        public int ID { get; set; }
        public required string CustomerName { get; set; }
        public string? Description { get; set;  }
        public double OrderValue { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerOrder_UID { get; set; }
    }
}
