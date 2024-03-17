using System.Text;
using System.Security.Cryptography;
namespace _2FA.Extensions
{
	public static class Encryption
	{
		private static readonly string Key = "CCCCCCCCC[P;YCeT{8oE|CCCCCCCCCC";
		private static readonly TripleDESCryptoServiceProvider DES = new() { Key = MD5.HashData(Encoding.ASCII.GetBytes(Key)), Mode = CipherMode.ECB };
		private static readonly ICryptoTransform cryptoEncrypt = DES.CreateEncryptor();
		private static readonly ICryptoTransform cryptoDecrypt = DES.CreateDecryptor();
		public static string Encrypt(this string plaintext)
		{
			byte[] Buffer = Encoding.ASCII.GetBytes(plaintext);
			return Convert.ToBase64String(cryptoEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
		}
		public static string Decrypt(this string base64Text)
		{
			byte[] Buffer = Convert.FromBase64String(base64Text);
			return Encoding.ASCII.GetString(cryptoDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
		}
		/// <summary>
		/// Compute SHA256 hash of a given string
		/// </summary>
		/// <param name="inputString"></param>
		/// <returns></returns>
		public static string ComputeSHA256Hash(this string inputString) => SHA256.HashData(Encoding.Unicode.GetBytes(inputString)).Select(b => string.Format("{0:x2}", b)).Aggregate((s1, s2) => s1 + s2);
	}
}