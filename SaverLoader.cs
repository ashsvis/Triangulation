using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace Triangulation
{
    public static class SaverLoader
    {
        /// <summary>
        /// Метод загрузки всех вершин из файла
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="clientSize"></param>
        /// <returns></returns>
        public static Vertices LoadVerticesFromFile(string fileName, Size clientSize)
        {
            using (var fs = File.OpenRead(fileName))
            using (var zip = new GZipStream(fs, CompressionMode.Decompress))
            {
                var formatter = new BinaryFormatter();
                return Vertices.UnNormalize((Vertices)formatter.Deserialize(zip), clientSize);
            }
        }

        /// <summary>
        /// Метод записи всех вершин в файл
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="vertices"></param>
        /// <param name="clientSize"></param>
        public static void SaveVerticesToFile(string fileName, Vertices vertices, Size clientSize)
        {
            using (var fs = File.Create(fileName))
            using (var zip = new GZipStream(fs, CompressionMode.Compress))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(zip, vertices.NormalizeXY(clientSize));
            }
        }
    }
}
