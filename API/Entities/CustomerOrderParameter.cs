using CustomerOrderManager.Business.Enums;

namespace CustomerOrderManager.Business.Entities
{
    public class CustomerOrderParameter
    {
        public int ID { get; set; }
        public ParameterType ParameterType { get; set; }
        public double Value { get; set; }
        public Guid CustomerOrderParameter_UID { get; set; }
    }
}
