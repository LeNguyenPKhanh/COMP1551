namespace Coursework_GUI
{
    partial class UserListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelAddUser = new System.Windows.Forms.Panel();
            this.cmbRoleFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnEditSelected = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.gridUsers = new System.Windows.Forms.DataGridView();
            this.panelEdit = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRoleEdit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEmailEdit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPhoneEdit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNameEdit = new System.Windows.Forms.TextBox();
            this.lblWorkingHoursEdit = new System.Windows.Forms.Label();
            this.txtWorkingHoursEdit = new System.Windows.Forms.TextBox();
            this.chkPartTimeEdit = new System.Windows.Forms.CheckBox();
            this.chkFullTimeEdit = new System.Windows.Forms.CheckBox();
            this.btnSaveEdit = new System.Windows.Forms.Button();
            this.lblSubject3 = new System.Windows.Forms.Label();
            this.lblSubject2 = new System.Windows.Forms.Label();
            this.lblSubject1 = new System.Windows.Forms.Label();
            this.lblSalary = new System.Windows.Forms.Label();
            this.txtSubject3 = new System.Windows.Forms.TextBox();
            this.txtSubject2 = new System.Windows.Forms.TextBox();
            this.txtSubject1 = new System.Windows.Forms.TextBox();
            this.txtSalary = new System.Windows.Forms.TextBox();
            this.panelAddUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).BeginInit();
            this.panelEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelAddUser
            // 
            this.panelAddUser.BackColor = System.Drawing.Color.White;
            this.panelAddUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAddUser.Controls.Add(this.cmbRoleFilter);
            this.panelAddUser.Controls.Add(this.label1);
            this.panelAddUser.Controls.Add(this.btnDeleteSelected);
            this.panelAddUser.Controls.Add(this.btnEditSelected);
            this.panelAddUser.Controls.Add(this.label3);
            this.panelAddUser.Location = new System.Drawing.Point(14, 28);
            this.panelAddUser.Name = "panelAddUser";
            this.panelAddUser.Size = new System.Drawing.Size(1474, 120);
            this.panelAddUser.TabIndex = 0;
            this.panelAddUser.Paint += new System.Windows.Forms.PaintEventHandler(this.panelAddUser_Paint);
            // 
            // cmbRoleFilter
            // 
            this.cmbRoleFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRoleFilter.FormattingEnabled = true;
            this.cmbRoleFilter.Location = new System.Drawing.Point(340, 57);
            this.cmbRoleFilter.Name = "cmbRoleFilter";
            this.cmbRoleFilter.Size = new System.Drawing.Size(121, 24);
            this.cmbRoleFilter.TabIndex = 6;
            this.cmbRoleFilter.SelectedIndexChanged += new System.EventHandler(this.cmbRoleFilter_SelectedIndexChanged_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(353, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Filter by Role";
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSelected.Location = new System.Drawing.Point(629, 50);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(106, 37);
            this.btnDeleteSelected.TabIndex = 4;
            this.btnDeleteSelected.Text = "Delete";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            // 
            // btnEditSelected
            // 
            this.btnEditSelected.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditSelected.Location = new System.Drawing.Point(497, 50);
            this.btnEditSelected.Name = "btnEditSelected";
            this.btnEditSelected.Size = new System.Drawing.Size(106, 37);
            this.btnEditSelected.TabIndex = 3;
            this.btnEditSelected.Text = "Edit";
            this.btnEditSelected.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 31);
            this.label3.TabIndex = 0;
            this.label3.Text = "View All Users";
            // 
            // gridUsers
            // 
            this.gridUsers.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.gridUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUsers.Location = new System.Drawing.Point(14, 166);
            this.gridUsers.Name = "gridUsers";
            this.gridUsers.RowHeadersWidth = 51;
            this.gridUsers.RowTemplate.Height = 24;
            this.gridUsers.Size = new System.Drawing.Size(1474, 451);
            this.gridUsers.TabIndex = 1;
            // 
            // panelEdit
            // 
            this.panelEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.panelEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEdit.Controls.Add(this.label6);
            this.panelEdit.Controls.Add(this.txtRoleEdit);
            this.panelEdit.Controls.Add(this.label5);
            this.panelEdit.Controls.Add(this.txtEmailEdit);
            this.panelEdit.Controls.Add(this.label4);
            this.panelEdit.Controls.Add(this.txtPhoneEdit);
            this.panelEdit.Controls.Add(this.label2);
            this.panelEdit.Controls.Add(this.txtNameEdit);
            this.panelEdit.Controls.Add(this.lblWorkingHoursEdit);
            this.panelEdit.Controls.Add(this.txtWorkingHoursEdit);
            this.panelEdit.Controls.Add(this.chkPartTimeEdit);
            this.panelEdit.Controls.Add(this.chkFullTimeEdit);
            this.panelEdit.Controls.Add(this.btnSaveEdit);
            this.panelEdit.Controls.Add(this.lblSubject3);
            this.panelEdit.Controls.Add(this.lblSubject2);
            this.panelEdit.Controls.Add(this.lblSubject1);
            this.panelEdit.Controls.Add(this.lblSalary);
            this.panelEdit.Controls.Add(this.txtSubject3);
            this.panelEdit.Controls.Add(this.txtSubject2);
            this.panelEdit.Controls.Add(this.txtSubject1);
            this.panelEdit.Controls.Add(this.txtSalary);
            this.panelEdit.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.panelEdit.Location = new System.Drawing.Point(14, 623);
            this.panelEdit.Name = "panelEdit";
            this.panelEdit.Size = new System.Drawing.Size(1009, 199);
            this.panelEdit.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(281, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 20);
            this.label6.TabIndex = 51;
            this.label6.Text = "Role:";
            // 
            // txtRoleEdit
            // 
            this.txtRoleEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRoleEdit.Location = new System.Drawing.Point(285, 145);
            this.txtRoleEdit.Name = "txtRoleEdit";
            this.txtRoleEdit.ReadOnly = true;
            this.txtRoleEdit.Size = new System.Drawing.Size(200, 22);
            this.txtRoleEdit.TabIndex = 50;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(281, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 20);
            this.label5.TabIndex = 49;
            this.label5.Text = "Email:";
            // 
            // txtEmailEdit
            // 
            this.txtEmailEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailEdit.Location = new System.Drawing.Point(285, 59);
            this.txtEmailEdit.Name = "txtEmailEdit";
            this.txtEmailEdit.Size = new System.Drawing.Size(200, 22);
            this.txtEmailEdit.TabIndex = 48;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(29, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 20);
            this.label4.TabIndex = 47;
            this.label4.Text = "Phone:";
            // 
            // txtPhoneEdit
            // 
            this.txtPhoneEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhoneEdit.Location = new System.Drawing.Point(33, 147);
            this.txtPhoneEdit.Name = "txtPhoneEdit";
            this.txtPhoneEdit.Size = new System.Drawing.Size(200, 22);
            this.txtPhoneEdit.TabIndex = 46;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(29, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 45;
            this.label2.Text = "Name:";
            // 
            // txtNameEdit
            // 
            this.txtNameEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNameEdit.Location = new System.Drawing.Point(33, 59);
            this.txtNameEdit.Name = "txtNameEdit";
            this.txtNameEdit.Size = new System.Drawing.Size(200, 22);
            this.txtNameEdit.TabIndex = 44;
            // 
            // lblWorkingHoursEdit
            // 
            this.lblWorkingHoursEdit.AutoSize = true;
            this.lblWorkingHoursEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkingHoursEdit.ForeColor = System.Drawing.Color.Black;
            this.lblWorkingHoursEdit.Location = new System.Drawing.Point(777, 24);
            this.lblWorkingHoursEdit.Name = "lblWorkingHoursEdit";
            this.lblWorkingHoursEdit.Size = new System.Drawing.Size(107, 20);
            this.lblWorkingHoursEdit.TabIndex = 43;
            this.lblWorkingHoursEdit.Text = "Working Hours";
            // 
            // txtWorkingHoursEdit
            // 
            this.txtWorkingHoursEdit.Location = new System.Drawing.Point(781, 61);
            this.txtWorkingHoursEdit.Name = "txtWorkingHoursEdit";
            this.txtWorkingHoursEdit.Size = new System.Drawing.Size(200, 22);
            this.txtWorkingHoursEdit.TabIndex = 42;
            // 
            // chkPartTimeEdit
            // 
            this.chkPartTimeEdit.AutoSize = true;
            this.chkPartTimeEdit.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPartTimeEdit.Location = new System.Drawing.Point(529, 146);
            this.chkPartTimeEdit.Name = "chkPartTimeEdit";
            this.chkPartTimeEdit.Size = new System.Drawing.Size(85, 21);
            this.chkPartTimeEdit.TabIndex = 41;
            this.chkPartTimeEdit.Text = "Part Time";
            this.chkPartTimeEdit.UseVisualStyleBackColor = true;
            // 
            // chkFullTimeEdit
            // 
            this.chkFullTimeEdit.AutoSize = true;
            this.chkFullTimeEdit.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFullTimeEdit.Location = new System.Drawing.Point(528, 111);
            this.chkFullTimeEdit.Name = "chkFullTimeEdit";
            this.chkFullTimeEdit.Size = new System.Drawing.Size(81, 21);
            this.chkFullTimeEdit.TabIndex = 40;
            this.chkFullTimeEdit.Text = "Full Time";
            this.chkFullTimeEdit.UseVisualStyleBackColor = true;
            // 
            // btnSaveEdit
            // 
            this.btnSaveEdit.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveEdit.ForeColor = System.Drawing.Color.Black;
            this.btnSaveEdit.Location = new System.Drawing.Point(882, 126);
            this.btnSaveEdit.Name = "btnSaveEdit";
            this.btnSaveEdit.Size = new System.Drawing.Size(99, 43);
            this.btnSaveEdit.TabIndex = 39;
            this.btnSaveEdit.Text = "Save";
            this.btnSaveEdit.UseVisualStyleBackColor = true;
            // 
            // lblSubject3
            // 
            this.lblSubject3.AutoSize = true;
            this.lblSubject3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubject3.ForeColor = System.Drawing.Color.Black;
            this.lblSubject3.Location = new System.Drawing.Point(524, 24);
            this.lblSubject3.Name = "lblSubject3";
            this.lblSubject3.Size = new System.Drawing.Size(61, 20);
            this.lblSubject3.TabIndex = 38;
            this.lblSubject3.Text = "Subject:";
            // 
            // lblSubject2
            // 
            this.lblSubject2.AutoSize = true;
            this.lblSubject2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubject2.ForeColor = System.Drawing.Color.Black;
            this.lblSubject2.Location = new System.Drawing.Point(524, 108);
            this.lblSubject2.Name = "lblSubject2";
            this.lblSubject2.Size = new System.Drawing.Size(73, 20);
            this.lblSubject2.TabIndex = 37;
            this.lblSubject2.Text = "Subject 2:";
            // 
            // lblSubject1
            // 
            this.lblSubject1.AutoSize = true;
            this.lblSubject1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubject1.ForeColor = System.Drawing.Color.Black;
            this.lblSubject1.Location = new System.Drawing.Point(777, 23);
            this.lblSubject1.Name = "lblSubject1";
            this.lblSubject1.Size = new System.Drawing.Size(73, 20);
            this.lblSubject1.TabIndex = 36;
            this.lblSubject1.Text = "Subject 1:";
            // 
            // lblSalary
            // 
            this.lblSalary.AutoSize = true;
            this.lblSalary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalary.ForeColor = System.Drawing.Color.Black;
            this.lblSalary.Location = new System.Drawing.Point(525, 23);
            this.lblSalary.Name = "lblSalary";
            this.lblSalary.Size = new System.Drawing.Size(52, 20);
            this.lblSalary.TabIndex = 35;
            this.lblSalary.Text = "Salary:";
            // 
            // txtSubject3
            // 
            this.txtSubject3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubject3.Location = new System.Drawing.Point(528, 61);
            this.txtSubject3.Name = "txtSubject3";
            this.txtSubject3.Size = new System.Drawing.Size(200, 22);
            this.txtSubject3.TabIndex = 34;
            // 
            // txtSubject2
            // 
            this.txtSubject2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubject2.Location = new System.Drawing.Point(528, 145);
            this.txtSubject2.Name = "txtSubject2";
            this.txtSubject2.Size = new System.Drawing.Size(200, 22);
            this.txtSubject2.TabIndex = 33;
            // 
            // txtSubject1
            // 
            this.txtSubject1.Location = new System.Drawing.Point(781, 61);
            this.txtSubject1.Name = "txtSubject1";
            this.txtSubject1.Size = new System.Drawing.Size(200, 22);
            this.txtSubject1.TabIndex = 32;
            // 
            // txtSalary
            // 
            this.txtSalary.Location = new System.Drawing.Point(528, 61);
            this.txtSalary.Name = "txtSalary";
            this.txtSalary.Size = new System.Drawing.Size(200, 22);
            this.txtSalary.TabIndex = 31;
            // 
            // UserListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1500, 834);
            this.Controls.Add(this.panelEdit);
            this.Controls.Add(this.gridUsers);
            this.Controls.Add(this.panelAddUser);
            this.Name = "UserListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View All User";
            this.Load += new System.EventHandler(this.UserListForm_Load);
            this.panelAddUser.ResumeLayout(false);
            this.panelAddUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).EndInit();
            this.panelEdit.ResumeLayout(false);
            this.panelEdit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelAddUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView gridUsers;
        private System.Windows.Forms.Panel panelEdit;
        private System.Windows.Forms.Label lblSubject3;
        private System.Windows.Forms.Label lblSubject2;
        private System.Windows.Forms.Label lblSubject1;
        private System.Windows.Forms.Label lblSalary;
        private System.Windows.Forms.TextBox txtSubject3;
        private System.Windows.Forms.TextBox txtSubject2;
        private System.Windows.Forms.TextBox txtSubject1;
        private System.Windows.Forms.TextBox txtSalary;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Button btnEditSelected;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRoleFilter;
        private System.Windows.Forms.Button btnSaveEdit;
        private System.Windows.Forms.Label lblWorkingHoursEdit;
        private System.Windows.Forms.TextBox txtWorkingHoursEdit;
        private System.Windows.Forms.CheckBox chkPartTimeEdit;
        private System.Windows.Forms.CheckBox chkFullTimeEdit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRoleEdit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEmailEdit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPhoneEdit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNameEdit;
    }
}
