using MyIoCContainer.Attributes;

namespace UnitTest.Entities
{
    [ImportConstructor]
    public class FirstCustomerBLL
    {
        public FirstCustomerBLL(ICustomerDAL dal, Logger logger)
        { }
    }

    public class SecondCustomerBLL
    {
        [Import]
        public ICustomerDAL CustomerDAL { get; set; }

        [Import]
        public Logger Logger { get; set; }
    }
}
