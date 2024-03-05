using System.Security.Cryptography;
using System.Text;

namespace MedicineMarketPlace.BuildingBlocks.Extensions
{
    public static class EncryptionService
    {

        public static string EncryptUsingAES256(string plainText, int id)
        {
            var key = "0123456789123456";

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 256;
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                byte[] idBytes = BitConverter.GetBytes(id);

                byte[] combinedData = new byte[idBytes.Length + Encoding.UTF8.GetByteCount(plainText)];
                Buffer.BlockCopy(idBytes, 0, combinedData, 0, idBytes.Length);
                Encoding.UTF8.GetBytes(plainText, 0, plainText.Length, combinedData, idBytes.Length);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedData = encryptor.TransformFinalBlock(combinedData, 0, combinedData.Length);

                return Convert.ToBase64String(encryptedData);
            }
        }

        public static string DecryptUsingAES256(string encryptedText, int id)
        {
            var key = "0123456789123456";

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 256;
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                byte[] idBytes = BitConverter.GetBytes(id);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedData = Convert.FromBase64String(encryptedText);
                byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                byte[] originalIdBytes = new byte[sizeof(int)];
                Buffer.BlockCopy(decryptedData, 0, originalIdBytes, 0, sizeof(int));
                int originalId = BitConverter.ToInt32(originalIdBytes, 0);

                byte[] originalPlainTextBytes = new byte[decryptedData.Length - sizeof(int)];
                Buffer.BlockCopy(decryptedData, sizeof(int), originalPlainTextBytes, 0, decryptedData.Length - sizeof(int));

                if (originalId != id)
                {
                    throw new Exception("Decryption failed. Invalid identifier.");
                }

                return Encoding.UTF8.GetString(originalPlainTextBytes);
            }
        }
    }
}
