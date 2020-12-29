using System.Windows.Forms;
using Common.Domain.Configurations;
using Common.Infrastructure.Interfaces;
using FaceRecognition.SampleUi.Extensions;
using FaceRecognition.SampleUi.States;

namespace FaceRecognition.SampleUi.UserControls
{
    public partial class UserBrowserUserControl : UserControl
    {
        public UserBrowserUserControl(ApplicationState applicationState)
        {
            InitializeComponent();

            ApplicationState = applicationState;

            SetDataGridViewBindings();
        }

        private ApplicationState ApplicationState { get; }
        private ApplicationConfiguration Configuration => ApplicationState.ApplicationConfiguration;

        private void SetDataGridViewBindings()
        {
            UsersDataGridView.Enabled = true;
            UsersDataGridView.ColumnCount = 1;

            UsersDataGridView.Columns[0].Name = "User";
            UsersDataGridView.Columns[0].DataPropertyName = "Name";
            UsersDataGridView.Columns[0].Width = 350;
            UsersDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
            UsersDataGridView.Columns.Add(editButton);
            editButton.HeaderText = "Edit User";
            editButton.Text = "Edit";
            editButton.Name = "EditUserButton";
            editButton.UseColumnTextForButtonValue = true;

            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
            UsersDataGridView.Columns.Add(deleteButton);
            deleteButton.HeaderText = "Delete User";
            deleteButton.Text = "Delete";
            deleteButton.Name = "DeleteUserButton";
            deleteButton.UseColumnTextForButtonValue = true;

            UsersDataGridView.AutoGenerateColumns = false;

            SetDataGridViewBindingSource_LameWinForms();
        }

        private void SetDataGridViewBindingSource_LameWinForms()
        {
            UsersDataGridView.DataSource = new BindingSource
            {
                DataSource = Configuration.Users
            };
        }

        private void UsersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 1:
                    // Edit
                    UserControlExtensions.OpenUserControlWithXtraForm(new UserUserControl(ApplicationState, Configuration.Users[e.RowIndex]));
                    break;

                case 2:
                    //Delete
                    Configuration.Users.RemoveAt(e.RowIndex); // _configuration.Users.Remove(_configuration.Users[e.RowIndex]);
                    Configuration.Save();

                    ApplicationState.InitializeFaceRecognitionClient();
                    break;
            }

            SetDataGridViewBindingSource_LameWinForms();
            UsersDataGridView.Invalidate();
        }
    }
}