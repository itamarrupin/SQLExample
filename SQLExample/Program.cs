using System.Data.SqlClient;
using System.Collections;

namespace SQLExample
{
    internal class Program
    {
        const string SQL_SELECT = @"SELECT * FROM dbo.users WHERE active = 1 ORDER BY [grade] ASC";
        const string SQL_UPDATE = @"UPDATE users SET grade = @grade WHERE id = @id;";

        const string CONNECTION_STRING = @"Server=LAB-14-00\SQLEXPRESS;" +
                                  @"Database=Rupin;" +
                                  @"Integrated Security=True;" +
                                  @"TrustServerCertificate=False;";

        [Obsolete]
        static void Main(string[] args)
        {

            Console.WriteLine("==================== USERS (BEFORE FACTOR) =======================");
            List<User> users = GetUsers();
            PrintUsers(users);
            users = AddFactor(users, 10);
            Console.WriteLine("==================== USERS (AFTER FACTOR - IN MEMORY) =======================");
            PrintUsers(users);
            Console.WriteLine("==================== UPDATE USERS =======================");
            UpdateGrade(users);
            Console.WriteLine("==================== RELOAD USERS =======================");
            users = GetUsers();
            PrintUsers(users);

            Console.WriteLine("press any key");
            Console.ReadLine();
        }

        private static void UpdateGrade(List<User> users)
        {
            // CRUD: (U) - update list of users
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                try
                {
                    connection.Open();
                    foreach (User user in users)
                    {
                        SqlCommand cmd = connection.CreateCommand();
                        cmd.CommandText = SQL_UPDATE;
                        cmd.Parameters.AddWithValue("@id", user.id);
                        cmd.Parameters.AddWithValue("@grade", user.grade);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine($"Failed to update user with id: {user.id} ");
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to update users, see error: {ex.Message}");
                }
            }
        }
        private static void PrintUsers(List<User> users)
        {
            foreach (User user in users)
            {
                Console.WriteLine(user.ToString());
            }
        }
        private static List<User> GetUsers()
        {
            // CRUD: (R) - read list of users
            List<User> users = new List<User>();
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = SQL_SELECT;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User()
                            {
                                id = reader.GetInt32(0),
                                name = reader.GetString(1),
                                email = reader.GetString(2),
                                phone = reader.GetString(3),
                                password = reader.GetString(4),
                                active = reader.GetBoolean(5),
                                grade = reader.GetInt32(6)
                            };
                            users.Add(user);
                        }

                        connection.Close();

                    }
                    Console.WriteLine("Connection closed and disposed successfull");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to connect,see error {ex.Message} ");
                }




                foreach (var user in users)
                {
                }
            }

            return users;
        }
        private static List<User> AddFactor(List<User> users, int factor)
        {
            foreach (var user in users)
            {
                if (user.grade + factor <= 100)
                    user.grade += factor;
            }
            return users;
        }
    }
}
