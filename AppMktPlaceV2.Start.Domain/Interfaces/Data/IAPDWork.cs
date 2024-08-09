namespace Test.Trade.Domain.Interfaces.Data
{
    public interface IAPDWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
