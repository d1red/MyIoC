using MyIoCContainer.Attributes;

namespace UnitTest.Entities
{
    [Export(typeof(ICustomerDAL))]
    public class CustomerDAL : ICustomerDAL
    {
    }
}
