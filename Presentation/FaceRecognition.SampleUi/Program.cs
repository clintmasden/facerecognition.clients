using System;
using System.Windows.Forms;
using FaceRecognition.SampleUi.Forms;
using FaceRecognition.SampleUi.Models;
using FaceRecognition.SampleUi.States;

namespace FaceRecognition.SampleUi
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var state = new ApplicationState { FaceClient = FaceClients.EmguComputerVision4 };
            state.InitializeFaceRecognitionClient();

            Application.Run(new MainForm(state));
        }
    }
}