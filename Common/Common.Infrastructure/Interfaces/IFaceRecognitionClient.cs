using System.Collections.Generic;
using System.Drawing;
using Common.Domain.Entities;

namespace Common.Infrastructure.Interfaces
{
    /// <summary>
    ///     Face recognition client
    /// </summary>
    public interface IFaceRecognitionClient
    {
        /// <summary>
        ///     Gets a bitmap with face locations drawn
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        Bitmap GetBitmapWithLocations(Bitmap bitmap);

        /// <summary>
        ///     Gets a bitmap with matched face locations drawn
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        Bitmap GetBitmapWithClassifiedMatchedLocations(Bitmap bitmap, List<User> users);
    }
}