#region ATRIBUTTES
using Microsoft.Data.SqlClient;
using System.Data;
using Test.Trade.Application.Helper.Static.Settings;
#endregion

namespace Test.Trade.Domain.Connector
{
    public class APConnector
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public APConnector()
        {
            Connection = new SqlConnection(RumtimeSettings.ConnectionString);
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
