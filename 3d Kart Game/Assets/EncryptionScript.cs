using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using UnityEngine;

public class EncryptionScript {

	static readonly string PasswordHash = SystemInfo.deviceName;
	static readonly string SaltKey = SystemInfo.deviceModel;
	static readonly string VIKey = SystemInfo.deviceUniqueIdentifier;

	private string Encrypt(string plainText)
	{
		byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

		byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
		var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
		var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

		byte[] cipherTextBytes;

		using (var memoryStream = new MemoryStream())
		{
			using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
			{
				cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
				cryptoStream.FlushFinalBlock();
				cipherTextBytes = memoryStream.ToArray();
				cryptoStream.Close();
			}
			memoryStream.Close();
		}
		return Convert.ToBase64String(cipherTextBytes);
	}

	private string Decrypt(string encryptedText)
	{
		byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
		byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
		var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

		var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
		var memoryStream = new MemoryStream(cipherTextBytes);
		var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
		byte[] plainTextBytes = new byte[cipherTextBytes.Length];

		int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
		memoryStream.Close();
		cryptoStream.Close();
		return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
	}

	public void savePlayerPref(string playerPref, float value ) {
		if (PlayerPrefs.HasKey(playerPref)) {
			PlayerPrefs.SetString (playerPref, Encrypt(value.ToString()));
		}
	}


	public float getPlayerPref(string playerPref) {
		if (PlayerPrefs.HasKey (playerPref)) {
			return float.Parse(Decrypt (PlayerPrefs.GetString (playerPref)));
		} else {
			return 0f;
		}
	}

}