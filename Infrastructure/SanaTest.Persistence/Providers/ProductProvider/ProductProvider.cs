using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using SanaTest.Domain.Models;
using SanaTest.Domain.scripts;

namespace SanaTest.Persistence.Providers.ProductProvider
{
    public class ProductProvider : IProductProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductProvider> _logger;

        public ProductProvider(IConfiguration configuration, ILogger<ProductProvider> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<List<Product>> GetAllProductsAsync()
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
                return await GetAllProductsPostAsync();
            }
             _logger.LogInformation("Provider DB Sqlserver");
            return await GetAllProductsServerAsync();
        }

        private async Task<List<Product>> GetAllProductsServerAsync()
        {
             List<Product> products = new List<Product>();
            string connectionString = _configuration.GetConnectionString("SanaTestSqlServer");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))  
                {  
                    await conn.OpenAsync();  
                    await using (var cmd = new SqlCommand(Querys.PRODUCTQUERY, conn))
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Product productproduct = new Product
                            {
                                Id = reader.GetGuid(0),
                                Code = reader.GetString(1),
                                Description = reader.GetString(2),
                                Stock = reader.GetInt32(3),
                                Image = reader.GetString(4),
                                Value = reader.GetDecimal(5)
                            };

                            products.Add(productproduct);
                        }      
                    }
                    
                }  
            }catch(Exception ex)
            {
                _logger.LogError("An error occurred in the product query", ex);
                throw;
            }
            return products;
        }

        private async Task<List<Product>> GetAllProductsPostAsync()
        {
            List<Product> products = new List<Product>();
            string connectionString = _configuration.GetConnectionString("SanaTestPostgresql");
            try{
                await using var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand(Querys.PRODUCTQUERY, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Product productproduct = new Product
                        {
                            Id = reader.GetGuid(0),
                            Code = reader.GetString(1),
                            Description = reader.GetString(2),
                            Stock = reader.GetInt32(3),
                            Image = reader.GetString(4),
                            Value = reader.GetDecimal(5)
                        };

                        products.Add(productproduct);
                    }      
                }
            }catch(Exception ex)
            {
                _logger.LogError("An error occurred in the product query", ex);
                throw;
            }

            return products;
        }
    }
}