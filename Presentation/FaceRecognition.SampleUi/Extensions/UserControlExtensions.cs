using System.Windows.Forms;

namespace FaceRecognition.SampleUi.Extensions
{
    internal class UserControlExtensions
    {
        internal static void OpenUserControlWithXtraForm(UserControl userControl, bool isDialog = true)
        {
            var form = new Form
            {
                Name = $"{userControl.Name}Form",
                Text = "",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowOnly,
                StartPosition = FormStartPosition.CenterScreen,
                ShowIcon = false
            };

            form.Controls.Add(userControl);
            userControl.Dock = DockStyle.Fill;

            if (isDialog)
            {
                form.ShowDialog();
            }
            else
            {
                form.Show();
            }
        }
    }
}