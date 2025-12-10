using System; // Imports core system classes.
using System.ComponentModel; // Imports classes for data binding (BindingList).
using System.Windows.Forms; // Imports Windows Forms classes for UI components.
using System.Collections.Generic; // Imports generic collections classes.
using MySql.Data.MySqlClient; // Imports MySQL client library.

namespace Coursework_GUI // Defines the application namespace.
{
    public enum UserRole // Defines enumeration for user roles.
    {
        Teacher, // User role: Teacher.
        Admin, // User role: Administrator/Admin Staff.
        Student // User role: Student.
    }

    public abstract class Person // Defines the abstract base class for all persons.
    {
        public int Id { get; set; } // Property for person ID.
        public string Name { get; set; } // Property for person Name.
        public string Telephone { get; set; } // Property for person Telephone number.
        public string Email { get; set; } // Property for person Email address.
        public UserRole Role { get; protected set; } // Property for User Role (settable only in derived classes).
    }

    public class Teacher : Person // Defines Teacher class, inheriting from Person.
    {
        public decimal Salary { get; set; } // Teacher-specific property: Salary.
        public string Subject1 { get; set; } // Teacher-specific property: Subject 1.
        public string Subject2 { get; set; } // Teacher-specific property: Subject 2.

        public Teacher() // Constructor for Teacher.
        {
            Role = UserRole.Teacher; // Sets the role to Teacher.
        }
    }

    public class AdminStaff : Person // Defines AdminStaff class, inheriting from Person.
    {
        public decimal Salary { get; set; } // AdminStaff-specific property: Salary.
        public bool IsFullTime { get; set; } // AdminStaff-specific property: Full-time status.
        public int WorkingHours { get; set; } // AdminStaff-specific property: Working Hours.

        public AdminStaff() // Constructor for AdminStaff.
        {
            Role = UserRole.Admin; // Sets the role to Admin.
        }
    }

    public class Student : Person // Defines Student class, inheriting from Person.
    {
        public string Subject1 { get; set; } // Student-specific property: Subject 1.
        public string Subject2 { get; set; } // Student-specific property: Subject 2.
        public string Subject3 { get; set; } // Student-specific property: Subject 3.

        public Student() // Constructor for Student.
        {
            Role = UserRole.Student; // Sets the role to Student.
        }
    }

    public static class DataStore // Defines static class for database operations and data storage.
    {
        private static string connStr = // MySQL connection string initialization.
            "Server=127.0.0.1;Database=educationdb;User=root;Password=;";

        private static int _nextId = 1; // Private field to track the next available ID.

        public static BindingList<Person> Users { get; } = // Public static BindingList for UI data binding.
            new BindingList<Person>(); // Initializes the BindingList.

        private static MySqlConnection GetConn() // Helper method to create a new MySQL connection object.
        {
            return new MySqlConnection(connStr); // Returns the new connection.
        }

        /// <summary>
        /// Load tất cả user từ MySQL vào BindingList Users. (Load all users from MySQL into BindingList Users.)
        /// Gọi hàm này trước khi mở form Main. (Call this function before opening the Main form.)
        /// </summary>
        public static void LoadFromDatabase() // Method to load data from database into Users list.
        {
            Users.Clear(); // Clears the existing list before loading.
            int maxId = 0; // Initializes max ID tracker.

            using (var conn = GetConn()) // Creates and uses MySQL connection (ensures disposal).
            {
                conn.Open(); // Opens the database connection.

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
                "; // SQL query string using LEFT JOINs.

                using (var cmd = new MySqlCommand(sql, conn)) // Creates MySQL command object.
                using (var r = cmd.ExecuteReader()) // Executes query and gets data reader.
                {
                    while (r.Read()) // Loops through each record returned.
                    {
                        int id = Convert.ToInt32(r["id"]); // Reads person ID.
                        string name = r["name"].ToString(); // Reads person Name.
                        string tel = r["telephone"].ToString(); // Reads person Telephone.
                        string email = r["email"].ToString(); // Reads person Email.
                        string roleStr = r["role"].ToString(); // Reads person Role string.

                        Person p; // Declares a Person object reference.

                        if (roleStr == "Teacher") // Checks if role is Teacher.
                        {
                            var teacher = new Teacher(); // Creates new Teacher object.
                            teacher.Id = id; // Maps common properties.
                            teacher.Name = name;
                            teacher.Telephone = tel;
                            teacher.Email = email;
                            // Maps Teacher-specific properties, handling DBNull.
                            teacher.Salary = r["t_sal"] == DBNull.Value ? 0 : Convert.ToDecimal(r["t_sal"]);
                            teacher.Subject1 = r["t_s1"].ToString();
                            teacher.Subject2 = r["t_s2"].ToString();
                            p = teacher; // Assigns Teacher to Person reference.
                        }
                        else if (roleStr == "Admin") // Checks if role is Admin.
                        {
                            var admin = new AdminStaff(); // Creates new AdminStaff object.
                            admin.Id = id; // Maps common properties.
                            admin.Name = name;
                            admin.Telephone = tel;
                            admin.Email = email;
                            // Maps AdminStaff-specific properties, handling DBNull.
                            admin.Salary = r["a_sal"] == DBNull.Value ? 0 : Convert.ToDecimal(r["a_sal"]);
                            // Converts database integer (0/1) to boolean.
                            admin.IsFullTime = (r["a_ft"] != DBNull.Value && Convert.ToInt32(r["a_ft"]) == 1);
                            admin.WorkingHours = r["working_hours"] == DBNull.Value ? 0 : Convert.ToInt32(r["working_hours"]);
                            p = admin; // Assigns AdminStaff to Person reference.
                        }
                        else // Assumes Student role.
                        {
                            var student = new Student(); // Creates new Student object.
                            student.Id = id; // Maps common properties.
                            student.Name = name;
                            student.Telephone = tel;
                            student.Email = email;
                            // Maps Student-specific properties.
                            student.Subject1 = r["s_s1"].ToString();
                            student.Subject2 = r["s_s2"].ToString();
                            student.Subject3 = r["s_s3"].ToString();
                            p = student; // Assigns Student to Person reference.
                        }

                        Users.Add(p); // Adds the newly created Person object to the BindingList.
                        if (id > maxId) maxId = id; // Updates max ID for tracking.
                    }
                }
            }

            _nextId = maxId + 1; // Sets the next available ID.
        }

        public static void AddPerson(Person p) // Method to add a new Person to the database and list.
        {
            using (var conn = GetConn()) // Creates and uses MySQL connection.
            {
                conn.Open(); // Opens the connection.

                // Thêm vào persons (Insert into persons)
                string sqlPerson = @"
                    INSERT INTO persons (name, telephone, email, role)
                    VALUES (@name, @tel, @mail, @role);
                    SELECT LAST_INSERT_ID();
                "; // SQL for inserting into the 'persons' table.

                int newId;
                using (var cmd = new MySqlCommand(sqlPerson, conn)) // Creates command for persons table.
                {
                    cmd.Parameters.AddWithValue("@name", p.Name); // Adds parameter for Name.
                    cmd.Parameters.AddWithValue("@tel", p.Telephone); // Adds parameter for Telephone.
                    cmd.Parameters.AddWithValue("@mail", p.Email); // Adds parameter for Email.
                    cmd.Parameters.AddWithValue("@role", p.Role.ToString()); // Adds parameter for Role.
                    newId = Convert.ToInt32(cmd.ExecuteScalar()); // Executes insert and gets the new ID.
                }

                p.Id = newId; // Assigns the new database ID to the Person object.
                if (newId >= _nextId) _nextId = newId + 1; // Updates the next ID tracker.

                // Thêm vào bảng con theo role (Insert into child table based on role)
                if (p is Teacher t) // Checks if the object is a Teacher.
                {
                    string sql = @"INSERT INTO teachers (person_id, salary, subject1, subject2)
                                     VALUES (@id, @sal, @s1, @s2)"; // SQL for teachers table.
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", p.Id); // Adds FK (person_id).
                        cmd.Parameters.AddWithValue("@sal", t.Salary); // Adds Salary.
                        cmd.Parameters.AddWithValue("@s1", t.Subject1); // Adds Subject 1.
                        cmd.Parameters.AddWithValue("@s2", t.Subject2); // Adds Subject 2.
                        cmd.ExecuteNonQuery(); // Executes insert.
                    }
                }
                else if (p is AdminStaff a) // Checks if the object is AdminStaff.
                {
                    string sql = @"INSERT INTO adminstaff (person_id, salary, is_full_time, working_hours)
                                     VALUES (@id, @sal, @ft, @h)"; // SQL for adminstaff table.
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", p.Id); // Adds (person_id).
                        cmd.Parameters.AddWithValue("@sal", a.Salary); // Adds Salary.
                        cmd.Parameters.AddWithValue("@ft", a.IsFullTime ? 1 : 0); // Converts bool to int (1/0).
                        cmd.Parameters.AddWithValue("@h", a.WorkingHours); // Adds Working Hours.
                        cmd.ExecuteNonQuery(); // Executes insert.
                    }
                }
                else if (p is Student s) // Checks if the object is a Student.
                {
                    string sql = @"INSERT INTO students (person_id, subject1, subject2, subject3)
                                     VALUES (@id, @s1, @s2, @s3)"; // SQL for students table.
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", p.Id); // Adds FK (person_id).
                        cmd.Parameters.AddWithValue("@s1", s.Subject1); // Adds Subject 1.
                        cmd.Parameters.AddWithValue("@s2", s.Subject2); // Adds Subject 2.
                        cmd.Parameters.AddWithValue("@s3", s.Subject3); // Adds Subject 3.
                        cmd.ExecuteNonQuery(); // Executes insert.
                    }
                }
            }

            // Thêm vào danh sách để UI cập nhật (Add to the list for UI update)
            Users.Add(p); // Adds the object to the BindingList (triggers UI update).
        }

        public static void DeletePerson(Person p) // Method to delete a Person from the database and list.
        {
            using (var conn = GetConn()) // Creates and uses MySQL connection.
            {
                conn.Open(); // Opens the connection.

                string childTable = null; // Variable to hold the name of the specific child table.
                if (p.Role == UserRole.Teacher) // Checks role and assigns child table name.
                {
                    childTable = "teachers";
                }
                else if (p.Role == UserRole.Admin) // Checks role and assigns child table name.
                {
                    childTable = "adminstaff";
                }
                else if (p.Role == UserRole.Student) // Checks role and assigns child table name.
                {
                    childTable = "students";
                }

                if (childTable != null) // If a specific child table exists.
                {
                    string sqlChild = "DELETE FROM " + childTable + " WHERE person_id=@id"; // SQL to delete from the child table.
                    using (var cmd = new MySqlCommand(sqlChild, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", p.Id); // Specifies the ID to delete.
                        cmd.ExecuteNonQuery(); // Executes the delete from the child table.
                    }
                }

                string sqlPerson = "DELETE FROM persons WHERE id=@id"; // SQL to delete from the main persons table.
                using (var cmd = new MySqlCommand(sqlPerson, conn))
                {
                    cmd.Parameters.AddWithValue("@id", p.Id); // Specifies the ID to delete.
                    cmd.ExecuteNonQuery(); // Executes the delete from the main table.
                }
            }

            Users.Remove(p); // Removes the object from the local BindingList (triggers UI update).
        }
    }

    internal static class Program // Defines the application entry point class.
    {
        [STAThread] // Attribute required for Windows Forms applications.
        static void Main() // The entry method of the application.
        {
            Application.EnableVisualStyles(); // Enables visual styles for controls.
            Application.SetCompatibleTextRenderingDefault(false); // Sets text rendering compatibility.

            try // Start of error handling block.
            {
                DataStore.LoadFromDatabase(); // Attempts to load initial data.
            }
            catch (Exception ex) // Catches any exception during database loading.
            {
                // Shows a message box indicating a database connection error.
                MessageBox.Show("Cannot load data from MySQL:\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.Run(new Main()); // Starts the application and displays the Main form.
        }
    }
}