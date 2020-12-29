using Common.Domain.Configurations;
using Common.Infrastructure.Interfaces;
using FaceRecognition.SampleUi.UserControls;
using System.Windows.Forms;
using FaceRecognition.SampleUi.States;

namespace FaceRecognition.SampleUi.Forms
{
    public partial class MainForm : Form
    {
        public MainForm(ApplicationState applicationState)
        {
            InitializeComponent();

            var userControl = new FaceRecognitionUserControl(applicationState);

            this.Controls.Add(userControl);
            userControl.Dock = DockStyle.Fill;
        }
    }
}