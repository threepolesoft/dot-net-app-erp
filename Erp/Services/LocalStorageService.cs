using Microsoft.JSInterop;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Erp.Services
{
    public class LocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string EncryptionKey = "asdfsdfsadwedfsdfsdfasdf";

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<T> GetItem<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            // Serialize model to JSON string
            var jsonValue = JsonSerializer.Serialize(value);

            // Encrypt the JSON string
            var encryptedValue = Encrypt(jsonValue, EncryptionKey);

            // Store in localStorage
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, encryptedValue);
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            // Get encrypted string from localStorage
            var encryptedValue = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (encryptedValue == null)
            {
                return default; // Return default value if not found
            }

            // Decrypt the string
            var decryptedValue = Decrypt(encryptedValue, EncryptionKey);

            // Deserialize the decrypted JSON string back to the model
            return JsonSerializer.Deserialize<T>(decryptedValue);
        }

        public async Task SetItem<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        private string Encrypt(string plainText, string key)
        {
            using var aes = Aes.Create();
            var keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32));
            aes.Key = keyBytes;
            aes.IV = new byte[16]; // Zero IV (change for stronger security)
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(encryptedBytes);
        }

        private string Decrypt(string encryptedText, string key)
        {
            using var aes = Aes.Create();
            var keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32));
            aes.Key = keyBytes;
            aes.IV = new byte[16]; // Zero IV
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}