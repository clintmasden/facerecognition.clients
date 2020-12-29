using System.Linq;
using Common.Domain.Configurations;
using Common.Infrastructure.Interfaces;
using FaceRecognition.SampleUi.Models;

namespace FaceRecognition.SampleUi.States
{
    public class ApplicationState
    {
        private ApplicationConfiguration _applicationConfiguration { get; set; }

        internal ApplicationConfiguration ApplicationConfiguration
        {
            get
            {
                if (_applicationConfiguration != null)
                {
                    return _applicationConfiguration;
                }

                _applicationConfiguration = new ApplicationConfiguration();

                if (_applicationConfiguration.ConfigurationFileExists)
                {
                    _applicationConfiguration.Load();
                }
                else
                {
                    _applicationConfiguration.Save();
                }

                return _applicationConfiguration;
            }
        }

        internal FaceClients FaceClient { get; set; }

        internal IFaceRecognitionClient FaceRecognitionClient { get; set; }

        internal void InitializeFaceRecognitionClient()
        {
            switch (FaceClient)
            {
                case FaceClients.EmguComputerVision3:
                    var emgu3FaceClient = new EmguComputerVision3.FaceRecognitionClient(@"..\..\..\..\..\Resources\ComputerVision.Models\haarcascade_frontalface_default.xml");
                    if (ApplicationConfiguration.Users.Any()) emgu3FaceClient.TrainRecognizer(ApplicationConfiguration.Users);

                    FaceRecognitionClient = emgu3FaceClient;
                    break;

                case FaceClients.EmguComputerVision4:
                    var emgu4FaceClient = new EmguComputerVision4.FaceRecognitionClient(@"..\..\..\..\..\Resources\ComputerVision.Models\haarcascade_frontalface_default.xml");
                    if (ApplicationConfiguration.Users.Any()) emgu4FaceClient.TrainRecognizer(ApplicationConfiguration.Users);

                    FaceRecognitionClient = emgu4FaceClient;
                    break;

                case FaceClients.FaceDotNet:
                    var dotNetFaceClient = new FaceDotNet.FaceRecognitionClient(@"..\..\..\..\..\Resources\FaceDotNet.Models\");

                    FaceRecognitionClient = dotNetFaceClient;
                    break;

                case FaceClients.OpenComputerVisionSharp4:
                    var sharp4FaceClient = new OpenComputerVisionSharp4.FaceRecognitionClient(@"..\..\..\..\..\Resources\ComputerVision.Models\haarcascade_frontalface_default.xml");
                    if (ApplicationConfiguration.Users.Any()) sharp4FaceClient.TrainRecognizer(ApplicationConfiguration.Users);

                    FaceRecognitionClient = sharp4FaceClient;
                    break;
            }
        }
    }
}