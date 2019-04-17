using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using Business;

namespace DataLayer
{
    public class SqlConnector : ISqlConnector
    {
        #region Properties
        public static string DbStatus { get; set; }
        /// <summary>
        /// Property used to override the name of the application
        /// </summary>
        public static string ApplicationName { get; set; }
        /// <summary>
        /// Overrides the connection timeout
        /// </summary>
        public static int ConnectionTimeout { get; set; } = 10;

        /// <summary>
        /// Gets the connection string
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return BuildConnectionString();
            }
        }
        #endregion

        
        public static void EncryptConnectionString()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(Environment.CurrentDirectory + "\\VPNManager.exe");
            ConnectionStringsSection connectionStringsSection = ConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection;
            connectionStringsSection.SectionInformation.ProtectSection("RSAProtectedConfigurationProvider");
            
            if (connectionStringsSection != null)
            {
                if (!connectionStringsSection.SectionInformation.IsProtected)
                    connectionStringsSection.SectionInformation.ProtectSection(null);
                else
                    connectionStringsSection.SectionInformation.UnprotectSection();
            }
            connectionStringsSection.SectionInformation.ForceSave = true;
            configuration.Save(ConfigurationSaveMode.Full);
        }

        #region Methods
        /// <summary>
        /// Creates new connection string based on parameters values
        /// </summary>
        /// <returns></returns>
        private static string BuildConnectionString()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProxosSQLDatabase"].ToString();
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(connectionString);
            // If the application name is null set the default name
            sb.ApplicationName = ApplicationName ?? sb.ApplicationName;
            // Set connection timeout
            sb.ConnectTimeout = (ConnectionTimeout > 0) ? ConnectionTimeout : sb.ConnectTimeout;
            return sb.ToString();
        }

        /// <summary>
        /// Returns an opened connection to the caller
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetSqlConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            try
            {
                sqlConnection.Open();
                return sqlConnection;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Checks the SQL database connection status
        /// </summary>
        /// <returns></returns>
        public static bool CheckSqlConnection()
        {
            var isConnected = false;
            SqlConnection connection = null;
            try
            {
                connection = GetSqlConnection();
                isConnected = true;
            }
            catch
            {
                isConnected = false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return isConnected;
        }

        /// <summary>
        /// Gets VPN remote hosts list
        /// </summary>
        /// <returns></returns>
        public List<VpnRemoteHost> GetRemoteHosts()
        {
            List<VpnRemoteHost> vpnRemoteHosts = new List<VpnRemoteHost>();

            try
            {
                using (SqlConnection sqlConnection = GetSqlConnection())
                {
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = @"spGetVpnRemoteHosts";
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.Notification = null;

                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                vpnRemoteHosts.Add(new VpnRemoteHost
                                {
                                    Id = Convert.ToInt16(sqlDataReader[0]),
                                    HostName = sqlDataReader[1].ToString(),
                                    HostDescription = sqlDataReader[2].ToString(),
                                    NetworkDst = sqlDataReader[3].ToString(),
                                    NetworkMask = sqlDataReader[4].ToString(),
                                    Gateway = sqlDataReader[5].ToString(),
                                    Latency = sqlDataReader[6].ToString() == "" ? "n/a" : sqlDataReader[6].ToString(),
                                    CreatedDate = sqlDataReader[7] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(sqlDataReader[7])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
            return vpnRemoteHosts;
        }

        public static bool CheckUserCredentials(string username, string password)
        {
            bool result = false;

            // Open new sql connection
            using (SqlConnection sqlConnection = GetSqlConnection())
            {
                // Create new sql command
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    // Use stored procedure with number parameter
                    sqlCommand.CommandText = @"spCheckUserCredentials";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter userSqlParam = new SqlParameter("User", System.Data.SqlDbType.VarChar);
                    SqlParameter passwordSqlParam = new SqlParameter("Password", System.Data.SqlDbType.VarChar);
                    userSqlParam.Value = username;
                    sqlCommand.Parameters.Add(userSqlParam);
                    passwordSqlParam.Value = password;
                    sqlCommand.Parameters.Add(passwordSqlParam);
                    result = (bool)sqlCommand.ExecuteScalar();
                }
            }

            return result;
        }

        public bool UpdateVpnRemoteHost(VpnRemoteHost vpnRemoteHost)
        {
            bool result = false;

            // Open new sql connection
            using (SqlConnection sqlConnection = GetSqlConnection())
            {
                // Create new sql command
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    // Use stored procedure with number parameter
                    sqlCommand.CommandText = @"spUpdateVpnRemoteHost";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter id = new SqlParameter("id", System.Data.SqlDbType.Int);
                    SqlParameter hostName = new SqlParameter("hostName", System.Data.SqlDbType.VarChar);
                    SqlParameter hostDescription = new SqlParameter("hostDescription", System.Data.SqlDbType.VarChar);
                    SqlParameter networkDst = new SqlParameter("networkDst", System.Data.SqlDbType.VarChar);
                    SqlParameter networkMask = new SqlParameter("networkMask", System.Data.SqlDbType.VarChar);
                    SqlParameter gateway = new SqlParameter("gateway", System.Data.SqlDbType.VarChar);
                    SqlParameter queryStatus = new SqlParameter("queryStatus", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output };
                    id.Value = vpnRemoteHost.Id;
                    sqlCommand.Parameters.Add(id);

                    hostName.Value = vpnRemoteHost.HostName;
                    sqlCommand.Parameters.Add(hostName);

                    hostDescription.Value = vpnRemoteHost.HostDescription;
                    sqlCommand.Parameters.Add(hostDescription);

                    networkDst.Value = vpnRemoteHost.NetworkDst;
                    sqlCommand.Parameters.Add(networkDst);

                    networkMask.Value = vpnRemoteHost.NetworkMask;
                    sqlCommand.Parameters.Add(networkMask);

                    gateway.Value = vpnRemoteHost.Gateway;
                    sqlCommand.Parameters.Add(gateway);

                    sqlCommand.Parameters.Add(queryStatus);

                    sqlCommand.ExecuteNonQuery();

                    result = (bool)queryStatus.Value;
                }
            }

            return result;
        }
        #endregion
    }
}
