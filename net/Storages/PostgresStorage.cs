using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace net.Storages
{
    public class PostgresStorage
    {
        private readonly string _pgConnStr = "Host=127.0.0.1;Port=5432;Username=postgres;Password=123456;Database=postgres;Pooling=true;";

        public async Task Init()
        {
            using var connection = new NpgsqlConnection(_pgConnStr);
            connection.Open();

            new NpgsqlCommand
            {
                Connection = connection,
                CommandText = "DROP TABLE IF EXISTS projects;"
            }.ExecuteNonQuery();
            
            new NpgsqlCommand
            {
                Connection = connection,
                CommandText = @"CREATE TABLE projects (
                    id SERIAL PRIMARY KEY,
                    name VARCHAR(100) NOT NULL,
                    version INTEGER NOT NULL,
                    tags TEXT [],
                    status BOOLEAN NOT NULL,
                    createdAt TIMESTAMP NOT NULL
                );"
            }.ExecuteNonQuery();

            foreach (var v in Data.List)
            {
                var insertQuery = "INSERT INTO projects (name, version, tags, status, createdAt) VALUES (@name, @version, @tags, @status, @createdAt)";

                using (var cmd = new NpgsqlCommand(insertQuery, connection))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar)
                    {
                        Value = v.name
                    });
                    
                    cmd.Parameters.Add(new NpgsqlParameter("version", NpgsqlDbType.Integer)
                    {
                        Value = v.version
                    });
                    
                    cmd.Parameters.Add(new NpgsqlParameter("tags", NpgsqlDbType.Array | NpgsqlDbType.Text)
                    {
                        Value = v.tags
                    });
                    
                    cmd.Parameters.Add(new NpgsqlParameter("status", NpgsqlDbType.Boolean)
                    {
                        Value = v.status
                    });
                    
                    cmd.Parameters.Add(new NpgsqlParameter("createdAt", NpgsqlDbType.Date)
                    {
                        Value = v.createdAt
                    });

                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public List<Dictionary<string, object>> Query(string condition, object[] args)
        {
            using var connection = new NpgsqlConnection(_pgConnStr);
            connection.Open();
            
            using var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = "SELECT id, name, version, tags, status, createdAt FROM projects WHERE " + condition
            };

            foreach (var arg in args)
            {
                command.Parameters.Add(new NpgsqlParameter { Value = arg });
            }

            using var reader = command.ExecuteReader();
            var results = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                var version = reader.GetInt32(2);
                var tags = (string[])reader.GetValue(3);
                var status = reader.GetBoolean(4);
                var createdAt = reader.GetDateTime(5);

                results.Add(new Dictionary<string, object>
                {
                    { "id", id },
                    { "name", name },
                    { "version", version },
                    { "tags", tags },
                    { "status", status },
                    { "createdAt", createdAt }
                });
            }

            return results;
        }
    }
}