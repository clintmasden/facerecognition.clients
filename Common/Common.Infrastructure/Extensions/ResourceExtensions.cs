using System.IO;
using System.Linq;
using System.Reflection;

namespace Common.Infrastructure.Extensions
{
    public static class ResourceExtensions
    {
        public static string ReadResource(string @namespace, string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = name;

            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            if (!name.StartsWith(@namespace))
            {
                resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(name));
            }

            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}