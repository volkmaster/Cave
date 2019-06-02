using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WorldGenerator {
    public static class Serialization {
        public static string BASE_DIRECTORY = "GameSaves";

        public static void SaveChunk(Chunk chunk) {
            Save save = new Save(chunk);
            if (save.Blocks.Count == 0) {
                return;
            }

            string path = Path.Combine(ConstructDirectoryPath(chunk.World.Name), ConstructFileName(chunk.Position));
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, save);
            stream.Close();
        }

        public static bool LoadChunk(Chunk chunk) {
            string path = Path.Combine(ConstructDirectoryPath(chunk.World.Name), ConstructFileName(chunk.Position));
            if (!File.Exists(path)) {
                return false;
            }

            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Save save = (Save)formatter.Deserialize(stream);
            foreach (var block in save.Blocks) {
                chunk.Blocks[block.Key.X, block.Key.Y, block.Key.Z] = block.Value;
            }
            stream.Close();
            return true;
        }

        public static string ConstructDirectoryPath(string directory) {
            string path = Path.Combine(BASE_DIRECTORY, directory);
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string ConstructFileName(WorldPosition position) =>
            $"{position.X},{position.Y},{position.Z}.bin";
    }
}