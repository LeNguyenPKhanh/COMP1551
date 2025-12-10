using System; // Imports the fundamental System namespace for basic functionality
using System.Linq; // Imports LINQ for querying and filtering collections
using System.Windows.Forms; // Imports Windows Forms classes for creating GUI applications

namespace Coursework_GUI // Defines the namespace for organizing related classes
{
    public partial class UserListForm : Form // Declares UserListForm class that inherits from Form
    {
        private Person _editingPerson;   // Private field to store the currently selected or edited Person object

        public UserListForm() // Constructor method that initializes the UserListForm
        {
            InitializeComponent(); // Initializes all form controls defined in the designer

            // Configure the DataGridView settings for displaying user data
            gridUsers.ReadOnly = true; // Sets the grid to read-only mode so users cannot edit cells directly
            gridUsers.AllowUserToAddRows = false; // Prevents the empty new row from appearing at the bottom
            gridUsers.AllowUserToDeleteRows = false; // Disables row deletion through the grid interface
            gridUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Makes it so clicking any cell selects the entire row
            gridUsers.RowHeadersVisible = false; // Hides the leftmost gray column with row numbers

            // Configure ComboBox for filtering users by role
            if (cmbRoleFilter != null) // Checks if the ComboBox control exists before configuring it
            {
                cmbRoleFilter.DropDownStyle = ComboBoxStyle.DropDownList; // Makes the ComboBox non-editable by user typing
                cmbRoleFilter.Items.Clear(); // Removes any items that might have been added in the designer
                cmbRoleFilter.Items.Add("All"); // Adds option to show all users regardless of role
                cmbRoleFilter.Items.Add("Teacher"); // Adds Teacher role filter option
                cmbRoleFilter.Items.Add("Admin"); // Adds Admin role filter option
                cmbRoleFilter.Items.Add("Student"); // Adds Student role filter option
                cmbRoleFilter.SelectedIndex = 0; // Sets initial selection to the first item which is All
                cmbRoleFilter.SelectedIndexChanged += cmbRoleFilter_SelectedIndexChanged; // Subscribes to the selection change event to trigger filtering
            }

            // Initially hide the edit panel when the form first loads
            if (panelEdit != null) // Checks if the edit panel control exists
                panelEdit.Visible = false; // Sets the panel visibility to false to hide it

            // Assign event handlers to button controls
            if (btnEditSelected != null) // Checks if the Edit button exists
                btnEditSelected.Click += btnEditSelected_Click; // Subscribes the click event to the edit handler method

            if (btnDeleteSelected != null) // Checks if the Delete button exists
                btnDeleteSelected.Click += btnDeleteSelected_Click; // Subscribes the click event to the delete handler method

            if (btnSaveEdit != null) // Checks if the Save button in the edit panel exists
                btnSaveEdit.Click += btnSaveEdit_Click; // Subscribes the click event to the save changes handler method

            // Assign event handlers for Full-time and Part-time checkboxes to ensure mutual exclusion
            if (chkFullTimeEdit != null) // Checks if the Full-time checkbox exists
                chkFullTimeEdit.CheckedChanged += chkFullTimeEdit_CheckedChanged; // Subscribes to handle when Full-time is checked or unchecked
            if (chkPartTimeEdit != null) // Checks if the Part-time checkbox exists
                chkPartTimeEdit.CheckedChanged += chkPartTimeEdit_CheckedChanged; // Subscribes to handle when Part-time is checked or unchecked

            // NEW: Handle row selection in DataGridView to automatically show View Mode panel
            gridUsers.SelectionChanged += gridUsers_SelectionChanged; // Subscribes to the event that fires when user selects a different row

            this.Load += UserListForm_Load; // Subscribes to the form Load event to fetch and display data when form opens
        }

        // Section for loading and displaying data in the DataGridView

        private void UserListForm_Load(object sender, EventArgs e) // Event handler method that executes when the form loads
        {
            try // Begins a try block to handle potential database errors
            {
                var persons = DataStoreMySQL.GetAllPersons(); // Calls the MySQL data access method to retrieve all person records from the database
                DataStore.Users.Clear(); // Clears the existing in-memory user list to avoid duplicates
                foreach (var p in persons) // Loops through each person object returned from the database
                {
                    DataStore.Users.Add(p); // Adds each person to the static in-memory DataStore collection
                }
            }
            catch (Exception ex) // Catches any exceptions that occur during database operations
            {
                MessageBox.Show("Cannot load users from database:\n" + ex.Message, // Displays an error message with exception details
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Sets the dialog title, button, and icon
            }

            LoadUsers(); // Calls the method to populate the DataGridView with all users by default
        }

        private void LoadUsers(string roleFilter = null) // Method to populate the DataGridView with user data, accepts optional role filter parameter
        {
            var query = DataStore.Users.AsEnumerable(); // Converts the static user list to an enumerable collection for LINQ operations
                
            // Apply role filtering if a specific role is selected and it is not the All option
            if (!string.IsNullOrWhiteSpace(roleFilter) && // Checks if roleFilter parameter has a value
                !roleFilter.Equals("All", StringComparison.OrdinalIgnoreCase)) // Checks if the filter is not All using case-insensitive comparison
            {
                query = query.Where(u => // Filters the user collection
                    u.Role.ToString().Equals(roleFilter, StringComparison.OrdinalIgnoreCase)); // Keeps only users whose role matches the filter string
            }

            var data = query // Begins building the data structure for binding to the grid
                .Select(user => new // Projects each user into an anonymous type object
                {
                    user.Id, // Includes the user ID property
                    user.Name, // Includes the user name property
                    Phone = user.Telephone, // Maps Telephone property to Phone for better column naming
                    user.Email, // Includes the email property
                    Role = user.Role.ToString(), // Converts the UserRole enum to its string representation

                    // Determine Subject1 based on user type using pattern matching and ternary operators
                    Subject1 = user is Teacher t1 ? t1.Subject1 // If user is Teacher type, get Subject1 from Teacher
                             : user is Student s1 ? s1.Subject1 // Otherwise if user is Student type, get Subject1 from Student
                             : string.Empty, // Otherwise leave empty for Admin users

                    // Determine Subject2 using the same pattern matching approach
                    Subject2 = user is Teacher t2 ? t2.Subject2 // Get Subject2 if Teacher
                             : user is Student s2 ? s2.Subject2 // Get Subject2 if Student
                             : string.Empty, // Empty for Admin

                    Subject3 = user is Student s3 ? s3.Subject3 : string.Empty, // Only Students have Subject3, others get empty string

                    // Determine Salary field for Teacher and AdminStaff roles
                    Salary = user is Teacher t // Check if user is Teacher
                                 ? t.Salary.ToString() // Get Teacher salary as string
                            : user is AdminStaff a // Otherwise check if user is AdminStaff
                                 ? a.Salary.ToString() // Get AdminStaff salary as string
                            : string.Empty, // Students have no salary so leave empty

                    // Determine Full-time or Part-time status for AdminStaff only
                    FullTimeStatus = user is AdminStaff ad // Check if user is AdminStaff
                                         ? (ad.IsFullTime ? "Full-time" : "Part-time") // Convert boolean to readable string
                                         : string.Empty, // Empty for Teacher and Student

                    // Determine Working Hours for AdminStaff only
                    WorkingHours = user is AdminStaff ad2 // Check if user is AdminStaff
                                         ? ad2.WorkingHours.ToString() // Get working hours as string
                                         : string.Empty // Empty for Teacher and Student
                })
                .ToList(); // Converts the query results to a List collection

            gridUsers.DataSource = null; // Clears the current data source binding to force a refresh
            gridUsers.DataSource = data; // Binds the new data list to the DataGridView
        }

        // Section for filtering users by role

        private void cmbRoleFilter_SelectedIndexChanged(object sender, EventArgs e) // Event handler for when user changes the role filter ComboBox selection
        {
            var selected = cmbRoleFilter.SelectedItem?.ToString() ?? "All"; // Gets the selected role text or defaults to All if null
            LoadUsers(selected); // Calls LoadUsers method with the selected role to refresh the grid with filtered data
        }

        // NEW: Event handler for DataGridView row selection to display user info in View Mode
        private void gridUsers_SelectionChanged(object sender, EventArgs e) // Executes whenever the user selects a different row in the grid
        {
            if (gridUsers.SelectedRows.Count > 0) // Checks if at least one row is currently selected
            {
                var user = GetSelectedUser(); // Retrieves the Person object corresponding to the selected row
                if (user != null) // Verifies that a valid user object was found
                {
                    ShowUserInViewMode(user); // Displays the user information in the side panel in read-only View Mode
                }
            }
        }

        // Section for retrieving the selected user from the grid

        private Person GetSelectedUser() // Method to get the Person object of the currently selected row in the DataGridView
        {
            if (gridUsers.SelectedRows.Count == 0) // Checks if no rows are selected
                return null; // Returns null if nothing is selected

            var row = gridUsers.SelectedRows[0]; // Gets the first selected row from the collection

            if (row.Cells["Id"].Value == null) // Checks if the Id cell value is null
                return null; // Returns null if Id is missing

            int id; // Declares variable to store the parsed user ID
            try
            {
                id = Convert.ToInt32(row.Cells["Id"].Value); // Attempts to convert the Id cell value to an integer
            }
            catch // Catches any conversion exceptions
            {
                return null; // Returns null if conversion fails
            }

            var user = DataStore.Users.FirstOrDefault(u => u.Id == id); // Uses LINQ to find the user in the DataStore by matching ID
            return user; // Returns the found Person object or null if not found
        }

        // NEW: Method to display selected user information in View Mode with read-only fields
        private void ShowUserInViewMode(Person user) // Takes a Person object and displays its information in the panel without allowing edits
        {
            if (user == null) return; // Exits the method if the user parameter is null

            _editingPerson = user; // Stores the reference to the currently viewed user

            // Show the side panel to display user information
            if (panelEdit != null) // Checks if the panel control exists
                panelEdit.Visible = true; // Makes the panel visible to the user

            // Fill all the panel fields with the user's data
            FillEditPanelFromUser(user); // Calls method to populate text boxes and other controls with user information

            // Make all input fields read-only so user cannot modify them in View Mode
            SetEditPanelReadOnly(true); // Calls method to disable editing on all text boxes and controls

            // Hide the Save button because no editing is allowed in View Mode
            if (btnSaveEdit != null) // Checks if the Save button exists
                btnSaveEdit.Visible = false; // Hides the Save button from the panel
        }

        // Section for deleting a user

        private void btnDeleteSelected_Click(object sender, EventArgs e) // Event handler for Delete button click
        {
            var user = GetSelectedUser(); // Retrieves the Person object of the currently selected row
            if (user == null) // Checks if no valid user is selected
            {
                MessageBox.Show("Please select a user row first.", "Info", // Displays information message prompting user to select a row
                    MessageBoxButtons.OK, MessageBoxIcon.Information); // Sets dialog buttons and icon
                return; // Exits the method early since there is no user to delete
            }

            var confirm = MessageBox.Show( // Shows a confirmation dialog asking user to confirm deletion
                "Are you sure you want to delete this user?", // Message text asking for confirmation
                "Confirm delete", // Dialog title
                MessageBoxButtons.YesNo, // Shows Yes and No buttons
                MessageBoxIcon.Question); // Shows question mark icon

            if (confirm == DialogResult.Yes) // Checks if user clicked the Yes button to confirm deletion
            {
                try // Begins try block to handle database errors
                {
                    DataStoreMySQL.DeletePerson(user); // Calls the MySQL data access method to delete the user from the database
                }
                catch (Exception ex) // Catches any database errors during deletion
                {
                    MessageBox.Show("Cannot delete user from database:\n" + ex.Message, // Displays error message with exception details
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Shows error dialog
                    return; // Exits the method without proceeding since database deletion failed
                }

                DataStore.DeletePerson(user); // Removes the user from the in-memory DataStore collection

                string currentRole = cmbRoleFilter?.SelectedItem?.ToString() ?? "All"; // Gets the current role filter selection
                LoadUsers(currentRole); // Reloads the grid data to reflect the deletion with the current filter applied

                // If the deleted user was being displayed in the panel, hide the panel
                if (_editingPerson == user && panelEdit != null) // Checks if the deleted user is the one currently shown in the panel
                {
                    _editingPerson = null; // Clears the reference to the editing person
                    panelEdit.Visible = false; // Hides the side panel
                    ClearEditPanel(); // Clears all the text fields in the panel
                }

                MessageBox.Show("User deleted successfully.", "Info", // Shows success message to confirm deletion
                    MessageBoxButtons.OK, MessageBoxIcon.Information); // Uses information icon
            }
        }

        // Switch to Edit Mode
        private void btnEditSelected_Click(object sender, EventArgs e)
        {
            var user = GetSelectedUser();
            if (user == null)
            {
                MessageBox.Show("Please select a user row first.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _editingPerson = user;

            // Show the edit panel
            if (panelEdit != null)
                panelEdit.Visible = true;

            FillEditPanelFromUser(user);

            // Enable editing
            SetEditPanelReadOnly(false);

            // Show Save button in Edit Mode
            if (btnSaveEdit != null)
                btnSaveEdit.Visible = true;
        }

        // Set ReadOnly state for all input fields
        private void SetEditPanelReadOnly(bool readOnly)
        {
            if (txtNameEdit != null) txtNameEdit.ReadOnly = readOnly;
            if (txtPhoneEdit != null) txtPhoneEdit.ReadOnly = readOnly;
            if (txtEmailEdit != null) txtEmailEdit.ReadOnly = readOnly;

            if (txtSalary != null) txtSalary.ReadOnly = readOnly;
            if (txtSubject1 != null) txtSubject1.ReadOnly = readOnly;
            if (txtSubject2 != null) txtSubject2.ReadOnly = readOnly;
            if (txtSubject3 != null) txtSubject3.ReadOnly = readOnly;
            if (txtWorkingHoursEdit != null) txtWorkingHoursEdit.ReadOnly = readOnly;

            // Disable checkboxes in View Mode
            if (chkFullTimeEdit != null) chkFullTimeEdit.Enabled = !readOnly;
            if (chkPartTimeEdit != null) chkPartTimeEdit.Enabled = !readOnly;
        }

        private void FillEditPanelFromUser(Person user) // Method to populate all panel fields with data from a Person object
        {
            if (user == null) return; // Exits if user parameter is null

            // Display the user's role in the read-only role field
            if (txtRoleEdit != null) // Checks if role text box exists
                txtRoleEdit.Text = user.Role.ToString(); // Shows the role as a string

            // Fill common fields that apply to all user types
            if (txtNameEdit != null) // Checks if name text box exists
                txtNameEdit.Text = user.Name; // Sets the name field value

            if (txtPhoneEdit != null) // Checks if phone text box exists
                txtPhoneEdit.Text = user.Telephone; // Sets the phone field value

            if (txtEmailEdit != null) // Checks if email text box exists
                txtEmailEdit.Text = user.Email; // Sets the email field value

            // Clear all role-specific fields before populating to avoid showing old data
            txtSalary.Text = ""; // Clears the Salary text box
            txtSubject1.Text = ""; // Clears the Subject1 text box
            txtSubject2.Text = ""; // Clears the Subject2 text box
            txtSubject3.Text = ""; // Clears the Subject3 text box

            if (chkFullTimeEdit != null) chkFullTimeEdit.Checked = false; // Unchecks Full-time checkbox
            if (chkPartTimeEdit != null) chkPartTimeEdit.Checked = false; // Unchecks Part-time checkbox
            if (txtWorkingHoursEdit != null) txtWorkingHoursEdit.Text = ""; // Clears Working Hours text box

            ApplyRoleEditLayout(user.Role); // Calls method to show or hide fields based on the user's role

            // Fill Teacher-specific fields if user is a Teacher
            if (user is Teacher teacher) // Pattern matching to check if user is Teacher type
            {
                txtSalary.Text = teacher.Salary.ToString(); // Displays Teacher salary
                txtSubject1.Text = teacher.Subject1; // Displays first subject taught
                txtSubject2.Text = teacher.Subject2; // Displays second subject taught
            }
            // Fill AdminStaff-specific fields if user is AdminStaff
            else if (user is AdminStaff admin) // Pattern matching to check if user is AdminStaff type
            {
                txtSalary.Text = admin.Salary.ToString(); // Displays AdminStaff salary
                chkFullTimeEdit.Checked = admin.IsFullTime; // Checks Full-time box if admin is full-time
                chkPartTimeEdit.Checked = !admin.IsFullTime; // Checks Part-time box if admin is not full-time
                txtWorkingHoursEdit.Text = admin.WorkingHours.ToString(); // Displays working hours per week
            }
            // Fill Student-specific fields if user is a Student
            else if (user is Student student) // Pattern matching to check if user is Student type
            {
                txtSubject1.Text = student.Subject1; // Displays first subject studied
                txtSubject2.Text = student.Subject2; // Displays second subject studied
                txtSubject3.Text = student.Subject3; // Displays third subject studied
            }
        }

        private void ApplyRoleEditLayout(UserRole role) // Method to show or hide panel fields based on the user's role
        {
            if (role == UserRole.Teacher) // Checks if the role is Teacher
            {
                // Show fields relevant to Teacher role
                lblSalary.Visible = true; // Makes Salary label visible
                txtSalary.Visible = true; // Makes Salary text box visible

                lblSubject1.Visible = true; // Makes Subject1 label visible
                txtSubject1.Visible = true; // Makes Subject1 text box visible
                lblSubject2.Visible = true; // Makes Subject2 label visible
                txtSubject2.Visible = true; // Makes Subject2 text box visible

                // Hide Subject3 since Teachers only have two subjects
                lblSubject3.Visible = false; // Hides Subject3 label
                txtSubject3.Visible = false; // Hides Subject3 text box

                // Hide AdminStaff-specific fields for Teacher
                chkFullTimeEdit.Visible = false; // Hides Full-time checkbox
                chkPartTimeEdit.Visible = false; // Hides Part-time checkbox
                lblWorkingHoursEdit.Visible = false; // Hides Working Hours label
                txtWorkingHoursEdit.Visible = false; // Hides Working Hours text box
            }
            else if (role == UserRole.Student) // Checks if the role is Student
            {
                // Hide Salary field since Students do not have salary
                lblSalary.Visible = false; // Hides Salary label
                txtSalary.Visible = false; // Hides Salary text box

                // Show all three subject fields for Student
                lblSubject1.Visible = true; // Makes Subject1 label visible
                txtSubject1.Visible = true; // Makes Subject1 text box visible
                lblSubject2.Visible = true; // Makes Subject2 label visible
                txtSubject2.Visible = true; // Makes Subject2 text box visible
                lblSubject3.Visible = true; // Makes Subject3 label visible
                txtSubject3.Visible = true; // Makes Subject3 text box visible

                // Hide AdminStaff-specific fields for Student
                chkFullTimeEdit.Visible = false; // Hides Full-time checkbox
                chkPartTimeEdit.Visible = false; // Hides Part-time checkbox
                lblWorkingHoursEdit.Visible = false; // Hides Working Hours label
                txtWorkingHoursEdit.Visible = false; // Hides Working Hours text box
            }
            else if (role == UserRole.Admin) // Checks if the role is Admin
            {
                // Show Salary field for AdminStaff
                lblSalary.Visible = true; // Makes Salary label visible
                txtSalary.Visible = true; // Makes Salary text box visible

                // Hide all Subject fields since AdminStaff do not teach or study subjects
                lblSubject1.Visible = false; // Hides Subject1 label
                txtSubject1.Visible = false; // Hides Subject1 text box
                lblSubject2.Visible = false; // Hides Subject2 label
                txtSubject2.Visible = false; // Hides Subject2 text box
                lblSubject3.Visible = false; // Hides Subject3 label
                txtSubject3.Visible = false; // Hides Subject3 text box

                // Show AdminStaff-specific fields for employment status
                chkFullTimeEdit.Visible = true; // Makes Full-time checkbox visible
                chkPartTimeEdit.Visible = true; // Makes Part-time checkbox visible
                lblWorkingHoursEdit.Visible = true; // Makes Working Hours label visible
                txtWorkingHoursEdit.Visible = true; // Makes Working Hours text box visible
            }
        }

        private void ClearEditPanel() // Method to reset all fields in the edit panel to empty values
        {
            // Clear common fields that apply to all user types
            if (txtNameEdit != null) txtNameEdit.Text = ""; // Clears Name text box
            if (txtPhoneEdit != null) txtPhoneEdit.Text = ""; // Clears Phone text box
            if (txtEmailEdit != null) txtEmailEdit.Text = ""; // Clears Email text box

            // Clear the read-only role display field
            if (txtRoleEdit != null) txtRoleEdit.Text = ""; // Clears Role text box

            // Clear role-specific fields
            txtSalary.Text = ""; // Clears Salary text box
            txtSubject1.Text = ""; // Clears Subject1 text box
            txtSubject2.Text = ""; // Clears Subject2 text box
            txtSubject3.Text = ""; // Clears Subject3 text box

            // Reset checkboxes to unchecked state
            if (chkFullTimeEdit != null) chkFullTimeEdit.Checked = false; // Unchecks Full-time checkbox
            if (chkPartTimeEdit != null) chkPartTimeEdit.Checked = false; // Unchecks Part-time checkbox
            if (txtWorkingHoursEdit != null) txtWorkingHoursEdit.Text = ""; // Clears Working Hours text box
        }

        // Section for handling mutual exclusion between Full-time and Part-time checkboxes

        private void chkFullTimeEdit_CheckedChanged(object sender, EventArgs e) // Event handler for Full-time checkbox state change
        {
            if (chkFullTimeEdit.Checked && chkPartTimeEdit != null) // Checks if Full-time is now checked and Part-time checkbox exists
                chkPartTimeEdit.Checked = false; // Unchecks Part-time to ensure mutual exclusion
        }

        private void chkPartTimeEdit_CheckedChanged(object sender, EventArgs e) // Event handler for Part-time checkbox state change
        {
            if (chkPartTimeEdit.Checked && chkFullTimeEdit != null) // Checks if Part-time is now checked and Full-time checkbox exists
                chkFullTimeEdit.Checked = false; // Unchecks Full-time to ensure mutual exclusion
        }

        // Section for saving edited user information

        private void btnSaveEdit_Click(object sender, EventArgs e) // Event handler for Save button click
        {
            SaveEditingChanges(); // Calls the method that validates and saves changes
        }

        private void SaveEditingChanges() // Method that validates, saves, and persists changes made to user information
        {
            if (_editingPerson == null) // Checks if there is a user object currently being edited
            {
                MessageBox.Show("No user is being edited.", "Info", // Displays message indicating no user is loaded
                    MessageBoxButtons.OK, MessageBoxIcon.Information); // Shows information dialog
                return; // Exits the method since there is nothing to save
            }

            // Update common fields that apply to all user types if they have valid values
            if (txtNameEdit != null && !string.IsNullOrWhiteSpace(txtNameEdit.Text)) // Checks if Name field has a value
                _editingPerson.Name = txtNameEdit.Text.Trim(); // Updates Name property after removing leading and trailing spaces

            if (txtPhoneEdit != null && !string.IsNullOrWhiteSpace(txtPhoneEdit.Text)) // Checks if Phone field has a value
                _editingPerson.Telephone = txtPhoneEdit.Text.Trim(); // Updates Telephone property after trimming spaces

            if (txtEmailEdit != null && !string.IsNullOrWhiteSpace(txtEmailEdit.Text)) // Checks if Email field has a value
                _editingPerson.Email = txtEmailEdit.Text.Trim(); // Updates Email property after trimming spaces

            // Update fields specific to Teacher role
            if (_editingPerson is Teacher teacher) // Pattern matching to check if the editing person is a Teacher
            {
                if (decimal.TryParse(txtSalary.Text, out var sal)) // Attempts to parse salary text as decimal number
                    teacher.Salary = sal; // Updates Salary property if parsing was successful

                teacher.Subject1 = txtSubject1.Text.Trim(); // Updates first subject taught after trimming spaces
                teacher.Subject2 = txtSubject2.Text.Trim(); // Updates second subject taught after trimming spaces
            }
            // Update fields specific to AdminStaff role
            else if (_editingPerson is AdminStaff admin) // Pattern matching to check if the editing person is AdminStaff
            {
                if (decimal.TryParse(txtSalary.Text, out var sal)) // Attempts to parse salary text as decimal number
                    admin.Salary = sal; // Updates Salary property if parsing was successful

                // Validate that either Full-time or Part-time is selected
                if (!chkFullTimeEdit.Checked && !chkPartTimeEdit.Checked) // Checks if both checkboxes are unchecked
                {
                    MessageBox.Show("Please choose Full-time or Part-time.", // Displays validation error message
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Shows warning dialog
                    return; // Exits method without saving since validation failed
                }

                admin.IsFullTime = chkFullTimeEdit.Checked; // Updates Full-time status based on checkbox state

                if (int.TryParse(txtWorkingHoursEdit.Text, out var hours)) // Attempts to parse working hours text as integer
                    admin.WorkingHours = hours; // Updates Working Hours property if parsing was successful
            }
            // Update fields specific to Student role
            else if (_editingPerson is Student student) // Pattern matching to check if the editing person is Student
            {
                student.Subject1 = txtSubject1.Text.Trim(); // Updates first subject studied after trimming spaces
                student.Subject2 = txtSubject2.Text.Trim(); // Updates second subject studied after trimming spaces
                student.Subject3 = txtSubject3.Text.Trim(); // Updates third subject studied after trimming spaces
            }

            try // Begins try block to handle database errors
            {
                DataStoreMySQL.UpdatePerson(_editingPerson); // Calls MySQL data access method to persist changes to the database
            }
            catch (Exception ex) // Catches any database errors during update operation
            {
                MessageBox.Show("Cannot update user in database:\n" + ex.Message, // Displays error message with exception details
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Shows error dialog
                return; // Exits method without proceeding since database update failed
            }

            // After successful save, return panel to View Mode
            SetEditPanelReadOnly(true); // Makes all fields read-only again
            if (btnSaveEdit != null) // Checks if Save button exists
                btnSaveEdit.Visible = false; // Hides Save button since editing is complete

            string currentRole = cmbRoleFilter?.SelectedItem?.ToString() ?? "All"; // Gets the current role filter selection
            LoadUsers(currentRole); // Reloads the grid to display updated data with current filter applied

            MessageBox.Show("User updated successfully.", "Info", // Displays success confirmation message
                MessageBoxButtons.OK, MessageBoxIcon.Information); // Shows information dialog
        }

        private void panelAddUser_Paint(object sender, PaintEventArgs e) { } // Empty event handler for panel paint event, required by designer

        private void cmbRoleFilter_SelectedIndexChanged_1(object sender, EventArgs e) { } // Empty duplicate event handler, likely generated by designer
    }
}