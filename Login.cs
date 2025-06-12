using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JahresprojektNeu.Classes;
using Microsoft.Data.SqlClient;

namespace JahresprojektNeu
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            SQLManagement.Autoconnect(); // Connect to database on startup
        }
        

        // Login button click event handler
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtLoginUsername.Text;
            string password = txtLoginPassword.Text;

            // Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Authentication
            if (SQLManagement.AuthenticateUser(username, password))
            {
                MessageBox.Show("Login successful!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

               // TODO: Open the main application form
                Main mainForm = new Main();
                this.Hide();
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Register button click event handler
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtRegisterUsername.Text;
            string password = txtRegisterPassword.Text;
            string confirmPassword = txtRegisterConfirmPassword.Text;

            // Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Register new user
            if (SQLManagement.RegisterUser(username, password))
            {
                MessageBox.Show("Registration successful! You can now log in.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControl.SelectedIndex = 0; // Switch to login tab
                txtRegisterUsername.Clear();
                txtRegisterPassword.Clear();
                txtRegisterConfirmPassword.Clear();
            }
            else
            {
                MessageBox.Show("Registration failed. Username may already exist.", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}