using System; // Imports fundamental system types.
using System.Collections.Generic; // Imports support for generic collections like List<T>.
using MySql.Data.MySqlClient; // Imports the necessary library for MySQL database connectivity.

namespace Coursework_GUI // Defines the application's namespace.
{
    // This class is responsible for all database interaction using MySQL.
    public static class DataStoreMySQL
    {
        // Private static field for the database connection string.
        private static string connStr =
            "Server=localhost;Database=educationdb;Uid=root;Pwd=;";

        // Helper method to create a new MySqlConnection object.
        private static MySqlConnection GetConn()
        {
            return new MySqlConnection(connStr);
        }

        // ===================== GET ALL PERSONS =====================
        // Retrieves a list of all persons (Teachers, Admins, Students) from the database.
        public static List<Person> GetAllPersons()
        {
            var list = new List<Person>(); // Initialize the list to store the results.

            using (var conn = GetConn()) // Establish and manage the database connection.
            {
                conn.Open(); // Open the connection.

                // SQL query uses LEFT JOINs to fetch data from the main persons table
                // and the related child tables (teachers, adminstaff, students) in one go.
                string sql = @"
                    SELECT p.*,
                            t.salary       AS t_sal,
                            t.subject1     AS t_s1,
                            t.subject2     AS t_s2,
                            a.salary       AS a_sal,
                            a.is_full_time AS a_ft,
                            a.working_hours,
                            s.subject1     AS s_s1,
                            s.subject2     AS s_s2,
                            s.subject3     AS s_s3
                        FROM persons p
                        LEFT JOIN teachers    t ON p.id = t.person_id
                        LEFT JOIN adminstaff a ON p.id = a.person_id
                        LEFT JOIN students    s ON p.id = s.person_id
                        ORDER BY p.id;
                ";

                using (var cmd = new MySqlCommand(sql, conn)) // Create the command.
                using (var r = cmd.ExecuteReader()) // Execute the query and get the data reader.
                {
                    while (r.Read()) // Loop through each row returned by the query.
                    {
                        // Read common properties.
                        int id = Convert.ToInt32(r["id"]);
                        string name = r["name"].ToString();
                        string tel = r["telephone"].ToString();
                        string email = r["email"].ToString();
                        string roleStr = r["role"].ToString();

                        Person p; // Reference to the base class Person.

                        if (roleStr == "Teacher") // Check the role string to determine which object type to instantiate.
                        {
                            var teacher = new Teacher();
                            // Map common properties
                            teacher.Id = id;
                            teacher.Name = name;
                            teacher.Telephone = tel;
                            teacher.Email = email;
                            // Map Teacher-specific properties (handling DBNull for potentially empty values).
                            teacher.Salary = r["t_sal"] == DBNull.Value ? 0 : Convert.ToDecimal(r["t_sal"]);
                            teacher.Subject1 = r["t_s1"].ToString();
                            teacher.Subject2 = r["t_s2"].ToString();
                            p = teacher;
                        }
                        else if (roleStr == "Admin") // Role is Admin.
                        {
                            var admin = new AdminStaff();
                            // Map common properties
                            admin.Id = id;
                            admin.Name = name;
                            admin.Telephone = tel;
                            admin.Email = email;
                            // Map AdminStaff-specific properties (handling DBNull).
                            admin.Salary = r["a_sal"] == DBNull.Value ? 0 : Convert.ToDecimal(r["a_sal"]);
                            // Convert database int (1/0) to C# boolean.
                            admin.IsFullTime = (r["a_ft"] != DBNull.Value && Convert.ToInt32(r["a_ft"]) == 1);
                            admin.WorkingHours = r["working_hours"] == DBNull.Value ? 0 : Convert.ToInt32(r["working_hours"]);
                            p = admin;
                        }
                        else // Role must be Student.
                        {
                            var student = new Student();
                            // Map common properties
                            student.Id = id;
                            student.Name = name;
                            student.Telephone = tel;
                            student.Email = email;
                            // Map Student-specific properties.
                            student.Subject1 = r["s_s1"].ToString();
                            student.Subject2 = r["s_s2"].ToString();
                            student.Subject3 = r["s_s3"].ToString();
                            p = student;
                        }

                        list.Add(p); // Add the constructed object to the list.
                    }
                }
            }

            return list; // Return the full list of Person objects.
        }

        // ===================== ADD PERSON =====================
        // Inserts a new Person object into the database.
        public static void AddPerson(Person p)
        {
            using (var conn = GetConn()) // Establish connection.
            {
                conn.Open(); // Open connection.

                // 1. Thêm vào persons (Insert into the parent table 'persons')
                string sqlPerson = @"
                    INSERT INTO persons (name, telephone, email, role)
                    VALUES (@name, @tel, @mail, @role);
                    SELECT LAST_INSERT_ID();
                "; // SQL to insert main data and retrieve the new ID.

                int newId;
                using (var cmd = new MySqlCommand(sqlPerson, conn))
                {
                    // Add parameters for common person fields.
                    cmd.Parameters.AddWithValue("@name", p.Name);
                    cmd.Parameters.AddWithValue("@tel", p.Telephone);
                    cmd.Parameters.AddWithValue("@mail", p.Email);
                    cmd.Parameters.AddWithValue("@role", p.Role.ToString());
                    // Execute and get the ID of the newly inserted row.
                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                p.Id = newId; // Assign the new database ID back to the object.

                // 2. Thêm vào bảng con theo loại user (Insert into the specific child table based on user type)
                if (p is Teacher t) // If the object is a Teacher.
                {
                    string sql = @"INSERT INTO teachers (person_id, salary, subject1, subject2)
                                     VALUES (@id, @sal, @s1, @s2)";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", p.Id); // Use the new ID as the foreign key.
                        cmd.Parameters.AddWithValue("@sal", t.Salary);
                        cmd.Parameters.AddWithValue("@s1", t.Subject1);
                        cmd.Parameters.AddWithValue("@s2", t.Subject2);
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (p is AdminStaff a) // If the object is AdminStaff.
                {
                    string sql = @"INSERT INTO adminstaff (person_id, salary, is_full_time, working_hours)
                                     VALUES (@id, @sal, @ft, @h)";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", p.Id);
                        cmd.Parameters.AddWithValue("@sal", a.Salary);
                        cmd.Parameters.AddWithValue("@ft", a.IsFullTime ? 1 : 0); // Convert boolean to int (1 or 0).
                        cmd.Parameters.AddWithValue("@h", a.WorkingHours);
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (p is Student s) // If the object is a Student.
                {
                    string sql = @"INSERT INTO students (person_id, subject1, subject2, subject3)
                                     VALUES (@id, @s1, @s2, @s3)";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", p.Id);
                        cmd.Parameters.AddWithValue("@s1", s.Subject1);
                        cmd.Parameters.AddWithValue("@s2", s.Subject2);
                        cmd.Parameters.AddWithValue("@s3", s.Subject3);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // ===================== DELETE PERSON =====================
        // Deletes a Person and their corresponding child record from the database.
        public static void DeletePerson(Person p)
        {
            using (var conn = GetConn()) // Establish connection.
            {
                conn.Open(); // Open connection.

                // Xác định bảng con theo Role (Determine the child table based on Role)
                string childTable = null;
                if (p.Role == UserRole.Teacher) // Check role.
                {
                    childTable = "teachers";
                }
                else if (p.Role == UserRole.Admin) // Check role.
                {
                    childTable = "adminstaff";
                }
                else if (p.Role == UserRole.Student) // Check role.
                {
                    childTable = "students";
                }

                // 1. Xóa ở bảng con (Delete from the child table first)
                if (childTable != null)
                {
                    // Use string concatenation for table name (not possible with parameters).
                    string sqlChild = "DELETE FROM " + childTable + " WHERE person_id=@id";
                    using (var cmd = new MySqlCommand(sqlChild, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", p.Id); // Delete using the person_id foreign key.
                        cmd.ExecuteNonQuery();
                    }
                }

                // 2. Xóa ở bảng persons (Delete from the main 'persons' table)
                string sqlPerson = "DELETE FROM persons WHERE id=@id"; // SQL to delete from the parent table.
                using (var cmd = new MySqlCommand(sqlPerson, conn))
                {
                    cmd.Parameters.AddWithValue("@id", p.Id); // Delete using the primary key id.
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ===================== UPDATE PERSON (OPTIONAL) =====================
        // Updates an existing Person object's details in the database.
        public static void UpdatePerson(Person p)
        {
            using (var conn = GetConn()) // Establish connection.
            {
                conn.Open(); // Open connection.

                // Update chung trong persons (Update common fields in the 'persons' table)
                string sqlPerson = @"
                    UPDATE persons
                    SET name = @name,
                        telephone = @tel,
                        email = @mail
                    WHERE id = @id;
                "; // SQL to update parent table.

                using (var cmd = new MySqlCommand(sqlPerson, conn))
                {
                    // Update common parameters.
                    cmd.Parameters.AddWithValue("@name", p.Name);
                    cmd.Parameters.AddWithValue("@tel", p.Telephone);
                    cmd.Parameters.AddWithValue("@mail", p.Email);
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    cmd.ExecuteNonQuery();
                }

                // Update bảng con (Update the child table)
                if (p is Teacher t) // If object is Teacher.
                {
                    string sql = @"
                        UPDATE teachers
                        SET salary = @sal,
                            subject1 = @s1,
                            subject2 = @s2
                        WHERE person_id = @id;
                    "; // SQL to update teachers table.
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@sal", t.Salary);
                        cmd.Parameters.AddWithValue("@s1", t.Subject1);
                        cmd.Parameters.AddWithValue("@s2", t.Subject2);
                        cmd.Parameters.AddWithValue("@id", t.Id); // Use person_id (which is p.Id).
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (p is AdminStaff a) // If object is AdminStaff.
                {
                    string sql = @"
                        UPDATE adminstaff
                        SET salary = @sal,
                            is_full_time = @ft,
                            working_hours = @h
                        WHERE person_id = @id;
                    "; // SQL to update adminstaff table.
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@sal", a.Salary);
                        cmd.Parameters.AddWithValue("@ft", a.IsFullTime ? 1 : 0);
                        cmd.Parameters.AddWithValue("@h", a.WorkingHours);
                        cmd.Parameters.AddWithValue("@id", a.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (p is Student s) // If object is Student.
                {
                    string sql = @"
                        UPDATE students
                        SET subject1 = @s1,
                            subject2 = @s2,
                            subject3 = @s3
                        WHERE person_id = @id;
                    "; // SQL to update students table.
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@s1", s.Subject1);
                        cmd.Parameters.AddWithValue("@s2", s.Subject2);
                        cmd.Parameters.AddWithValue("@s3", s.Subject3);
                        cmd.Parameters.AddWithValue("@id", s.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}