using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using SanaTest.Domain.Models;
using SanaTest.Domain.scripts;

namespace SanaTest.Persistence.Providers.OrderProvider
{
    public class OrdersProvider : IOrdersProvider
    {
         private readonly IConfiguration _configuration;
        private readonly ILogger<OrdersProvider> _logger;

        public OrdersProvider(IConfiguration configuration, ILogger<OrdersProvider> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<Guid> AddOrderAsync(Order order, List<OrderProducts> orderProducts)
        {
            _logger.LogInformation("Start Get products");
            var dataProvider = Environment.GetEnvironmentVariable("TypeConnection");
            if(string.IsNullOrEmpty(dataProvider))
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }

            if(Convert.ToInt32(dataProvider) == 1)
            {
                 _logger.LogInformation("Provider DB Postgresql");
                 return await AddOrderPostAsync(order, orderProducts);
            }
             _logger.LogInformation("Provider DB Sqlserver");
            return await AddOrderServerAsync(order, orderProducts);
        }

        #region "SQLSERVER"
        private async Task<Guid> AddOrderServerAsync(Order order, List<OrderProducts> orderProducts)
        {
            string connectionString = _configuration.GetConnectionString("SanaTestSqlServer");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();
                    Guid idOrder = await AddOrder(conn, transaction, order);
                    await AddOrderProduct(conn, transaction, orderProducts, idOrder);

                    transaction.Commit();
                    return idOrder;
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        throw;
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception("An error occurs registering the order. Try again later", ex2);
                    }
                }
            }
        }

        private async Task AddOrderProduct(SqlConnection conn, SqlTransaction transaction, List<OrderProducts> orderProducts, Guid idOrder)
        {
            foreach (var item in orderProducts)
            {
                Guid idOrderProduct = Guid.NewGuid();
                using (SqlCommand cmd = new SqlCommand(Querys.ADDORDERPRODUCTQUERY, conn, transaction)) 
                {
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.UniqueIdentifier).Value = idOrderProduct;
                    cmd.Parameters.Add("@idorder", System.Data.SqlDbType.UniqueIdentifier).Value = idOrder;
                    cmd.Parameters.Add("@idproduct", System.Data.SqlDbType.UniqueIdentifier).Value = item.IdProduct;
                    cmd.Parameters.Add("@subvalue", System.Data.SqlDbType.Date).Value = item.Subvalue;
                    cmd.Parameters.Add("@quantity", System.Data.SqlDbType.Timestamp).Value = item.Quantity;
                    await cmd.ExecuteNonQueryAsync(); 
                }
            }
        }
        private async Task<Guid> AddOrder(SqlConnection conn, SqlTransaction transaction, Order order)
        {
            Guid idOrder = Guid.NewGuid();
            using (SqlCommand cmd = new SqlCommand(Querys.ADDORDERQUERY, conn, transaction)) 
            {
                cmd.Parameters.Add("@id", System.Data.SqlDbType.UniqueIdentifier).Value = idOrder;
                cmd.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = order.Date;
                cmd.Parameters.Add("@value", System.Data.SqlDbType.Decimal).Value = order.Value;
                cmd.Parameters.Add("@idcustomer", System.Data.SqlDbType.UniqueIdentifier).Value = order.IdCustomer;
                await cmd.ExecuteNonQueryAsync(); 
            }

            return idOrder;
        }

        #endregion

        #region "Postgresql" 
        private async Task<Guid> AddOrderPostAsync(Order order, List<OrderProducts> orderProducts)
        {
            string connectionString = _configuration.GetConnectionString("SanaTestPostgresql");
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                NpgsqlTransaction transaction = null;

                try
                {
                    conn.Open();
                    transaction = await conn.BeginTransactionAsync();
                    Guid idOrder = await AddOrderPostSql(conn, transaction, order);
                    await AddOrderProductPostsql(conn, transaction, orderProducts, idOrder);

                    transaction.Commit();
                    return idOrder;
                }catch(Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        throw;
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception("An error occurs registering the order. Try again later", ex2);
                    }
                }

            }
        }

        private async Task<Guid> AddOrderPostSql(NpgsqlConnection conn, NpgsqlTransaction transaction, Order order)
        {
            Guid idOrder = Guid.NewGuid();
            using (NpgsqlCommand cmd = new NpgsqlCommand(Querys.ADDORDERQUERY, conn, transaction)) 
            {
                cmd.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Uuid).Value = idOrder;
                cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = order.Date;
                cmd.Parameters.Add("@value", NpgsqlTypes.NpgsqlDbType.Numeric).Value = order.Value;
                cmd.Parameters.Add("@idcustomer", NpgsqlTypes.NpgsqlDbType.Uuid).Value = order.IdCustomer;
                await cmd.ExecuteNonQueryAsync(); 
            }
            return idOrder;

        }
        private async Task AddOrderProductPostsql(NpgsqlConnection conn, NpgsqlTransaction transaction, List<OrderProducts> orderProducts, Guid idOrder)
        {
            foreach (var item in orderProducts)
            {
                Guid idOrderProduct = Guid.NewGuid();
                using (NpgsqlCommand cmd = new NpgsqlCommand(Querys.ADDORDERPRODUCTQUERY, conn, transaction)) 
                {
                    cmd.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Uuid).Value = idOrderProduct;
                    cmd.Parameters.Add("@idorder", NpgsqlTypes.NpgsqlDbType.Uuid).Value = idOrder;
                    cmd.Parameters.Add("@idproduct", NpgsqlTypes.NpgsqlDbType.Uuid).Value = item.IdProduct;
                    cmd.Parameters.Add("@subvalue", NpgsqlTypes.NpgsqlDbType.Numeric).Value = item.Subvalue;
                    cmd.Parameters.Add("@quantity", NpgsqlTypes.NpgsqlDbType.Integer).Value = item.Quantity;
                    await cmd.ExecuteNonQueryAsync(); 
                }
            }
        }

        #endregion
    }
}