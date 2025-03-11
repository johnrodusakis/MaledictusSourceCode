using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SomniaGames.Persistence
{
    public class JsonFilePersistence : IPersistence<GameData>
    {
        private readonly string _filePath;
        private readonly bool _useEncryption;
        private readonly byte[] _encryptionKey;

        public JsonFilePersistence(string fileName, bool useEncryption = false, byte[] encryptionKey = null)
        {
            _filePath = Path.Combine(Application.persistentDataPath, fileName);
            _useEncryption = useEncryption;
            // For demonstration, a hardcoded key is used. In production, inject or securely retrieve this.
            _encryptionKey = encryptionKey ?? Encoding.UTF8.GetBytes("16CharSecretKey!");
        }

        public async Task SaveAsync(GameData data)
        {
            var json = JsonUtility.ToJson(data, true);
            var fileBytes = Encoding.UTF8.GetBytes(json);

            if(_useEncryption)
                fileBytes = Encrypt(fileBytes, _encryptionKey);

            await File.WriteAllBytesAsync(_filePath, fileBytes);
        }

        public async Task<GameData> LoadAsync()
        {
            if (!File.Exists(_filePath))
                return new GameData();

            byte[] fileBytes = await File.ReadAllBytesAsync(_filePath);

            if (_useEncryption)
                fileBytes = Decrypt(fileBytes, _encryptionKey);

            string json = Encoding.UTF8.GetString(fileBytes);
            var data = JsonUtility.FromJson<GameData>(json);
            return data; 
        }

        private byte[] Encrypt(byte[] data, byte[] key)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();

            using MemoryStream ms = new();

            // Write IV at the start of the stream.
            ms.Write(aes.IV, 0, aes.IV.Length);

            using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        private byte[] Decrypt(byte[] data, byte[] key)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;

            // Read IV from the data stream.
            var iv = new byte[aes.BlockSize / 8];
            System.Array.Copy(data, iv, iv.Length);
            aes.IV = iv;

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

            // Decrypt starting after the IV.
            cs.Write(data, iv.Length, data.Length - iv.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }
    }
}