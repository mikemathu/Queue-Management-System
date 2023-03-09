using Npgsql;
using Queue_Management_System.Services;
using Queue_Management_System.Models;

namespace Queue_Management_System.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private const string _serviceProvidersTable = "appusers";
        private const string _servicePointTable = "servicepoints";
        private const string _role = "Service Provider";
        private IConfiguration _config;
        private NpgsqlConnection _connection;
        public AdminRepository(IConfiguration config)
        {
            _config = config;
        }
        private void OpenConnection()
        {
            string connectionString = _config.GetConnectionString("DefaultConnection");

            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
        }           
        public async Task<IEnumerable<ServiceProviderVM>> GetServiceProviders()
        {
             OpenConnection();
            List<ServiceProviderVM> serviceProviders = new List<ServiceProviderVM>();

            // Prep command object.
            string commandText = $"SELECT id, name FROM {_serviceProvidersTable} WHERE role = '{_role}'";
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, _connection))
            {
                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        serviceProviders.Add(new ServiceProviderVM
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"]
                        });
                    }
                    reader.Close();
                }
                _connection.Close();
            }
            if (serviceProviders.Count() == 0)
                return null;
            return serviceProviders;
        }
        public async Task<ServiceProviderVM> GetServiceProviderDetails(int id)
        {
            OpenConnection();
            ServiceProviderVM serviceProviderDetails = null;

            string commandText = $"SELECT * FROM {_serviceProvidersTable} WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, _connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        serviceProviderDetails = new ServiceProviderVM
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                            Password = (string)reader["password"],
                            Role = (string)reader["role"]
                        };
                    }
                    reader.Close();
                }
                _connection.Close();
            }
            if (serviceProviderDetails == null)
                return null;
            return serviceProviderDetails;
        }
        public async Task CreateServiceProvider(ServiceProviderVM serviceProvider)
        {
            OpenConnection();
            string commandText = $"INSERT INTO {_serviceProvidersTable} (name, password, role, servicepointid) " +
                                 $"VALUES (@name, @password, @role, @servicepointid)";
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, _connection))
            {
                command.Parameters.AddWithValue("@name", serviceProvider.Name);
                command.Parameters.AddWithValue("@password", serviceProvider.Password);
                command.Parameters.AddWithValue("@role", _role);
                command.Parameters.AddWithValue("@servicepointid", serviceProvider.ServicepointId);

                await command.ExecuteNonQueryAsync();
            }
            _connection.Close();
        }
        public async Task UpdateServiceProvider(ServiceProviderVM serviceProvider)
        {
            OpenConnection();
            string commandText = $"UPDATE {_serviceProvidersTable} " +
                                 $"SET password = @password, servicepointid = @servicepointid WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, _connection))
            {               
                command.Parameters.AddWithValue("@password", serviceProvider.Password);
                command.Parameters.AddWithValue("@servicepointid", serviceProvider.ServicepointId);
                command.Parameters.AddWithValue("@id", serviceProvider.Id);

                await command.ExecuteNonQueryAsync();
            }
            _connection.Close();
        }
        public async Task DeleteServiceProvider(int id)
        {
            OpenConnection();
            string commandText = $"DELETE FROM {_serviceProvidersTable} WHERE id=@id";
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, _connection))
            {
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync();
            }
            _connection.Close();
        }
        public async Task<IEnumerable<ServicePointVM>> GetServicePoints()
        {
            OpenConnection();
            List<ServicePointVM> servicePoints = new List<ServicePointVM>();

            string commandText = $"SELECT * FROM {_servicePointTable}";
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, _connection))
            {
                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        servicePoints.Add(new ServicePointVM
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                            ServiceProviderId = (int)reader["serviceproviderid"]
                        });
                    }
                    reader.Close();
                }
                _connection.Close();
            }
            if (servicePoints.Count() == 0)
                return null;
            return servicePoints;
        }
        public async Task<ServicePointVM> GetServicePointDetails(int id)
        {
            OpenConnection();

            ServicePointVM servicePointDetails = null;

            string commandText = $"SELECT * FROM {_servicePointTable} WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, _connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        servicePointDetails = new ServicePointVM
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                            ServiceProviderId = (int)reader["serviceproviderid"]
                        };
                    }
                    reader.Close();
                }
                _connection.Close();
            }
            if (servicePointDetails == null)
                return null;
            return servicePointDetails;
        }
        public async Task CreateServicePoint(ServicePointVM servicePoint)
        {
            OpenConnection();
            string commandText = $"INSERT INTO {_servicePointTable} (name, serviceproviderid) " +
                                 $"VALUES (@name, @serviceproviderid)";
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, _connection))
            {
                command.Parameters.AddWithValue("@name", servicePoint.Name);
                command.Parameters.AddWithValue("@serviceproviderid", servicePoint.ServiceProviderId);

                await command.ExecuteNonQueryAsync();
            }
            _connection.Close();
        }
        public async Task UpdateServicePoint(ServicePointVM servicePoint)
        {
            OpenConnection();
            string commandText = $"UPDATE {_servicePointTable} SET serviceproviderid = @serviceproviderid WHERE id = @id";

            using (var command = new NpgsqlCommand(commandText, _connection))
            {
                command.Parameters.AddWithValue("@serviceproviderid", servicePoint.ServiceProviderId);
                command.Parameters.AddWithValue("@id", servicePoint.Id);

                await command.ExecuteNonQueryAsync();
            }
            _connection.Close();
        }
        public async Task DeleteServicePoint(int id)
        {
            OpenConnection();
            string commandText = $"DELETE FROM {_servicePointTable} WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, _connection))
            {
                command.Parameters.AddWithValue("@id", id);
                await command.ExecuteNonQueryAsync();
            }
            _connection.Close();
        }       
    }
}
