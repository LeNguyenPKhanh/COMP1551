using System; // Imports the fundamental classes and base data types
using System.Windows.Forms; // Imports classes for creating Windows Forms GUI

namespace Coursework_GUI // Defines a namespace to organize related code
{
    public partial class UserForm : Form // Defines the 'UserForm' class, a 'partial' class inheriting from 'Form'
    {
        private readonly Person _editingPerson; // Private field to hold a reference to the Person object being edited (null in Add mode)
        private readonly bool _isEdit;           // Private field indicating if the form is in Edit mode (true) or Add mode (false)

        // Flag to prevent double-saving (event being triggered multiple times, e.g., double-clicking)
        private bool _isSaving = false; // Flag to prevent the save logic from executing multiple times concurrently

        // Form constructor for ADD new user
        public UserForm() // The default constructor used when adding a new user
        {
            InitializeComponent();  // Initializes the form's controls and layout (UI elements)

            // Populate the role ComboBox with enum values (Teacher, Admin, Student)
            cmbRole.DataSource = Enum.GetValues(typeof(UserRole)); // Fills the ComboBox with all values from the UserRole enumeration

            // Event handlers for Full-time/Part-time checkboxes
            chkFullTime.CheckedChanged += chkFullTime_CheckedChanged; // Attaches the custom handler to the Full-time checkbox change event
            chkPartTime.CheckedChanged += chkPartTime_CheckedChanged; // Attaches the custom handler to the Part-time checkbox change event

            // Update the UI according to the selected role
            UpdateRoleFields(); // Calls the method to show/hide fields based on the initial role selection (usually the first enum value)
        }

        // Constructor for EDIT existing user
        public UserForm(Person personToEdit) : this() // Constructor for editing, calls the default (Add) constructor first
        {
            // Assign the person being edited
            _editingPerson = personToEdit ?? throw new ArgumentNullException(nameof(personToEdit)); // Assigns the person object, throwing an exception if it's null
            _isEdit = true; // Set the form mode to "Edit"

            // Pre-fill the form fields with data from the existing user
            txtName.Text = _editingPerson.Name; // Populates the Name textbox
            txtPhone.Text = _editingPerson.Telephone; // Populates the Phone textbox
            txtEmail.Text = _editingPerson.Email; // Populates the Email textbox
            cmbRole.SelectedItem = _editingPerson.Role; // Sets the selected item in the role ComboBox

            // Disable role change when editing an existing user
            cmbRole.Enabled = false; // Prevents the user from changing the role of an existing person

            // Load role-specific values (e.g., Salary, Subject) into the form
            LoadRoleSpecificValuesFromPerson(); // Loads fields like Salary or Subjects based on the person's type
        }

        // Event triggered when role is changed in the ComboBox
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e) // Event handler for when the user selects a different role
        {
            UpdateRoleFields(); // Update the fields visibility based on selected role
        }

        /// <summary>
        /// Shows and hides fields based on the selected role
        /// </summary>
        private void UpdateRoleFields() // Method to dynamically adjust the visible fields based on the chosen role
        {
            // Ensure a role is selected
            if (cmbRole.SelectedItem == null) // Checks if an item is selected in the ComboBox
                return; // Exits if no item is selected

            var role = (UserRole)cmbRole.SelectedItem; // Casts the selected item to the UserRole enumeration

            // If in Add mode, reset the fields
            if (!_isEdit) // Checks if the form is in Add mode
            {
                txtSalary.Text = ""; // Clears the Salary field
                txtSubject1.Text = ""; // Clears Subject 1 field
                txtSubject2.Text = ""; // Clears Subject 2 field
                txtSubject3.Text = ""; // Clears Subject 3 field
                chkFullTime.Checked = false; // Unchecks Full-time
                chkPartTime.Checked = false; // Unchecks Part-time
                txtWorkingHours.Text = ""; // Clears Working Hours field
            }

            // Show/hide fields based on the selected role
            if (role == UserRole.Teacher) // Logic for Teacher role
            {
                // Show Salary and Subjects for Teacher
                lblSalary.Visible = true; // Shows Salary label
                txtSalary.Visible = true; // Shows Salary textbox

                lblSubject1.Visible = true; // Shows Subject 1 label
                txtSubject1.Visible = true; // Shows Subject 1 textbox
                lblSubject2.Visible = true; // Shows Subject 2 label
                txtSubject2.Visible = true; // Shows Subject 2 textbox

                // Hide Subject3 for Teacher
                lblSubject3.Visible = false; // Hides Subject 3 label
                txtSubject3.Visible = false; // Hides Subject 3 textbox

                // Hide Full-time/Part-time and Working Hours for Teacher
                chkFullTime.Visible = false; // Hides Full-time checkbox
                chkPartTime.Visible = false; // Hides Part-time checkbox
                lblWorkingHours.Visible = false; // Hides Working Hours label
                txtWorkingHours.Visible = false; // Hides Working Hours textbox
            }
            else if (role == UserRole.Student) // Logic for Student role
            {
                // Hide Salary for Student
                lblSalary.Visible = false; // Hides Salary label
                txtSalary.Visible = false; // Hides Salary textbox

                // Show Subjects for Student
                lblSubject1.Visible = true; // Shows Subject 1 label
                txtSubject1.Visible = true; // Shows Subject 1 textbox
                lblSubject2.Visible = true; // Shows Subject 2 label
                txtSubject2.Visible = true; // Shows Subject 2 textbox
                lblSubject3.Visible = true; // Shows Subject 3 label
                txtSubject3.Visible = true; // Shows Subject 3 textbox

                // Hide Full-time/Part-time and Working Hours for Student
                chkFullTime.Visible = false; // Hides Full-time checkbox
                chkPartTime.Visible = false; // Hides Part-time checkbox
                lblWorkingHours.Visible = false; // Hides Working Hours label
                txtWorkingHours.Visible = false; // Hides Working Hours textbox
            }
            else if (role == UserRole.Admin) // Logic for AdminStaff role
            {
                // Show Salary, Full-time/Part-time, and Working Hours for Admin
                lblSalary.Visible = true; // Shows Salary label
                txtSalary.Visible = true; // Shows Salary textbox

                chkFullTime.Visible = true; // Shows Full-time checkbox
                chkPartTime.Visible = true; // Shows Part-time checkbox
                lblWorkingHours.Visible = true; // Shows Working Hours label
                txtWorkingHours.Visible = true; // Shows Working Hours textbox

                // Hide Subjects for Admin
                lblSubject1.Visible = false; // Hides Subject 1 label
                txtSubject1.Visible = false; // Hides Subject 1 textbox
                lblSubject2.Visible = false; // Hides Subject 2 label
                txtSubject2.Visible = false; // Hides Subject 2 textbox
                lblSubject3.Visible = false; // Hides Subject 3 label
                txtSubject3.Visible = false; // Hides Subject 3 textbox
            }
        }

        /// <summary>
        /// Fills the role-specific fields with the current user's data
        /// </summary>
        private void LoadRoleSpecificValuesFromPerson() // Method to load data into the form when editing a user
        {
            if (_editingPerson == null) return; // If no person is being edited, exit

            // Load the values based on the role
            if (_editingPerson is Teacher teacher) // Uses pattern matching to check if the person is a Teacher
            {
                txtSalary.Text = teacher.Salary.ToString(); // Loads Teacher's salary
                txtSubject1.Text = teacher.Subject1; // Loads Teacher's Subject 1
                txtSubject2.Text = teacher.Subject2; // Loads Teacher's Subject 2
            }
            else if (_editingPerson is AdminStaff admin) // Checks if the person is an AdminStaff
            {
                txtSalary.Text = admin.Salary.ToString(); // Loads AdminStaff's salary
                chkFullTime.Checked = admin.IsFullTime; // Sets Full-time checkbox based on IsFullTime property
                chkPartTime.Checked = !admin.IsFullTime; // Sets Part-time checkbox as the opposite of IsFullTime
                txtWorkingHours.Text = admin.WorkingHours.ToString(); // Loads AdminStaff's working hours
            }
            else if (_editingPerson is Student student) // Checks if the person is a Student
            {
                txtSubject1.Text = student.Subject1; // Loads Student's Subject 1
                txtSubject2.Text = student.Subject2; // Loads Student's Subject 2
                txtSubject3.Text = student.Subject3; // Loads Student's Subject 3
            }

            UpdateRoleFields(); // Update UI to ensure correct fields are visible after loading data
        }

        // ===== CHECKBOX FULL-TIME / PART-TIME =====

        private void chkFullTime_CheckedChanged(object sender, EventArgs e) // Event handler for the Full-time checkbox
        {
            // If Full-time is checked, uncheck Part-time
            if (chkFullTime.Checked) // If Full-time is now checked
                chkPartTime.Checked = false; // Uncheck the Part-time box
        }

        private void chkPartTime_CheckedChanged(object sender, EventArgs e) // Event handler for the Part-time checkbox
        {
            // If Part-time is checked, uncheck Full-time
            if (chkPartTime.Checked) // If Part-time is now checked
                chkFullTime.Checked = false; // Uncheck the Full-time box
        }

        // ===== SAVE BUTTON =====

        private void btnSave_Click(object sender, EventArgs e) // Event handler for the Save button click
        {
            // Prevent double-save (double-click or event triggered multiple times)
            if (_isSaving) // Checks if the saving process is already running
                return; // Exits the method if saving is in progress
            _isSaving = true; // Sets the flag to indicate saving has started

            // Validate common fields (Name, Phone, Email)
            if (string.IsNullOrWhiteSpace(txtName.Text) || // Checks if Name field is empty or whitespace
                string.IsNullOrWhiteSpace(txtPhone.Text) || // Checks if Phone field is empty or whitespace
                string.IsNullOrWhiteSpace(txtEmail.Text)) // Checks if Email field is empty or whitespace
            {
                MessageBox.Show("Please enter Name, Phone and Email.", // Displays a warning message
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _isSaving = false; // Resets the flag
                return; // Exits the method
            }

            // Validate email format
            if (!txtEmail.Text.Contains("@")) // Simple check to see if the email contains an "@" symbol
            {
                var confirm = MessageBox.Show( // Asks the user for confirmation to continue despite invalid email
                    "Email does not seem to be valid. Do you want to continue?",
                    "Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) // If the user chooses No
                {
                    _isSaving = false; // Resets the flag
                    return; // Exits the method
                }
            }

            // Save either new or edited user
            if (_isEdit) // Checks if the form is in Edit mode
            {
                SaveEdit(); // Calls method to update the existing user
            }
            else // Otherwise, the form is in Add mode
            {
                SaveNew(); // Calls method to create and save a new user
            }

            if (_isSaving) // If the SaveEdit/SaveNew method completed without resetting _isSaving (meaning no validation errors occurred)
            {
                DialogResult = DialogResult.OK; // Sets the form's result to OK
                Close(); // Closes the form
            }
        }

        // Update the current user (edit mode)
        private void SaveEdit() // Method to update an existing Person object
        {
            try
            {
                _editingPerson.Name = txtName.Text.Trim(); // Updates Name, removing leading/trailing whitespace
                _editingPerson.Telephone = txtPhone.Text.Trim(); // Updates Telephone
                _editingPerson.Email = txtEmail.Text.Trim(); // Updates Email

                // Update based on role (using pattern matching to cast and update properties)
                if (_editingPerson is Teacher teacher) // If the person is a Teacher
                {
                    if (decimal.TryParse(txtSalary.Text, out var sal)) // Attempts to convert Salary text to a decimal
                        teacher.Salary = sal; // Updates the salary if parsing succeeded
                    teacher.Subject1 = txtSubject1.Text.Trim(); // Updates Subject 1
                    teacher.Subject2 = txtSubject2.Text.Trim(); // Updates Subject 2
                }
                else if (_editingPerson is AdminStaff admin) // If the person is AdminStaff
                {
                    if (decimal.TryParse(txtSalary.Text, out var sal)) // Attempts to convert Salary text to a decimal
                        admin.Salary = sal; // Updates the salary

                    if (!chkFullTime.Checked && !chkPartTime.Checked) // Validation: Checks if neither F/T nor P/T is selected
                    {
                        MessageBox.Show("Please choose Full-time or Part-time.", // Displays a warning
                            "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _isSaving = false; // Saving failed, reset flag
                        return; // Exits
                    }

                    admin.IsFullTime = chkFullTime.Checked; // Sets IsFullTime based on the Full-time checkbox status

                    if (int.TryParse(txtWorkingHours.Text, out var hours)) // Attempts to convert Working Hours to an integer
                        admin.WorkingHours = hours; // Updates Working Hours
                }
                else if (_editingPerson is Student student) // If the person is a Student
                {
                    student.Subject1 = txtSubject1.Text.Trim(); // Updates Subject 1
                    student.Subject2 = txtSubject2.Text.Trim(); // Updates Subject 2
                    student.Subject3 = txtSubject3.Text.Trim(); // Updates Subject 3
                }
            }
            catch (Exception ex) // Catches any unexpected errors during the update process
            {
                MessageBox.Show("Error while updating user:\n" + ex.Message, // Displays the error message
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isSaving = false; // Saving failed, reset flag
            }
        }

        // Create and save a new user
        private void SaveNew() // Method to create a new Person object and save it to the data store
        {
            try
            {
                var role = (UserRole)cmbRole.SelectedItem; // Gets the selected role
                Person p; // Declares a Person variable to hold the new object

                switch (role) // Uses a switch statement to create the correct subclass based on role
                {
                    case UserRole.Teacher: // If Teacher is selected
                        var teacher = new Teacher(); // Creates a new Teacher object
                        if (decimal.TryParse(txtSalary.Text, out var tSal)) // Parses Salary
                            teacher.Salary = tSal;
                        teacher.Subject1 = txtSubject1.Text.Trim();
                        teacher.Subject2 = txtSubject2.Text.Trim();
                        p = teacher; // Assigns the Teacher object to the Person variable
                        break;

                    case UserRole.Admin: // If Admin is selected
                        var admin = new AdminStaff(); // Creates a new AdminStaff object
                        if (decimal.TryParse(txtSalary.Text, out var aSal)) // Parses Salary
                            admin.Salary = aSal;

                        if (!chkFullTime.Checked && !chkPartTime.Checked) // Validation: Checks F/T or P/T selection
                        {
                            MessageBox.Show("Please choose Full-time or Part-time.",
                                "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            _isSaving = false; // Saving failed, reset flag
                            return; // Exits
                        }

                        admin.IsFullTime = chkFullTime.Checked; // Sets F/T status

                        if (int.TryParse(txtWorkingHours.Text, out var hours)) // Parses Working Hours
                            admin.WorkingHours = hours;

                        p = admin; // Assigns the AdminStaff object to the Person variable
                        break;

                    default: // Student (or any other unhandled role)
                        var student = new Student(); // Creates a new Student object
                        student.Subject1 = txtSubject1.Text.Trim();
                        student.Subject2 = txtSubject2.Text.Trim();
                        student.Subject3 = txtSubject3.Text.Trim();
                        p = student; // Assigns the Student object to the Person variable
                        break;
                }

                p.Name = txtName.Text.Trim(); // Populates common properties
                p.Telephone = txtPhone.Text.Trim();
                p.Email = txtEmail.Text.Trim();

                // Add the new person to the data store
                DataStore.AddPerson(p); // Calls the static method to save the new user
            }
            catch (Exception ex) // Catches any unexpected errors during creation or saving
            {
                MessageBox.Show("Cannot save new user to database:\n" + ex.Message, // Displays the error message
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isSaving = false; // Saving failed, reset flag
            }
        }

        // Cancel the operation and close the form
        private void btnCancel_Click(object sender, EventArgs e) // Event handler for the Cancel button
        {
            DialogResult = DialogResult.Cancel; // Sets the form's result to Cancel
            Close(); // Closes the form
        }

        // Empty event handlers for unused events (can be removed if not needed)
        private void txtName_TextChanged(object sender, EventArgs e) { } // Empty text changed handler
        private void txtPhone_TextChanged(object sender, EventArgs e) { } // Empty text changed handler
        private void txtEmail_TextChanged(object sender, EventArgs e) { } // Empty text changed handler
        private void lblSubject3_Click(object sender, EventArgs e) { } // Empty label click handler
        private void txtSubject3_TextChanged(object sender, EventArgs e) { } // Empty text changed handler
        private void txtWorkingHours_TextChanged(object sender, EventArgs e) { } // Empty text changed handler
        private void chkFullTime_CheckedChanged_1(object sender, EventArgs e) { } // Empty checkbox handler
        private void chkPartTime_CheckedChanged_1(object sender, EventArgs e) { } // Empty checkbox handler
        private void txtSubject2_TextChanged(object sender, EventArgs e) { } // Empty text changed handler
    }
}