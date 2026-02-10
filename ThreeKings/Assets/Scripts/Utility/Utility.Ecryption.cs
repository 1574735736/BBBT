
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace LieyouFramework
{
	public static partial class Utility
	{
		/// <summary>
		/// 加密类
		/// </summary>
		public static class Ecryption
		{
			private static MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();

			/// <summary>
			/// 对字符串进行MD5
			/// </summary>
			/// <param name="str"></param>
			/// <returns></returns>
			public static string MD5HashString(string str)
			{
				byte[] data = Encoding.UTF8.GetBytes(str);
				byte[] hash_byte = _md5.ComputeHash(data);
				string result = System.BitConverter.ToString(hash_byte);
				result = result.Replace("-", "");
				return result.ToLower();
			}

			/// <summary>
			/// 对文件进行MD5
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
			public static string MD5HashFile(string path)
			{
				if (!System.IO.File.Exists(path))
				{
					return "";
				}
				try
				{
					using (FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						byte[] hash_byte = _md5.ComputeHash(get_file);
						get_file.Close();

						string result = System.BitConverter.ToString(hash_byte);
						result = result.Replace("-", "");
						return result.ToLower();
					}
				}
				catch (System.Exception)
				{
					Utility.Debug.Log("File is Error:" + path);
					return "";
				}
			}

			/// <summary>
			/// 对字节进行MD5
			/// </summary>
			/// <param name="bytes"></param>
			/// <returns></returns>
			public static string MD5HashBytes(byte[] bytes)
			{
				if (bytes == null)
				{
					return "";
				}

				byte[] hash_byte = _md5.ComputeHash(bytes);
				string result = System.BitConverter.ToString(hash_byte);
				result = result.Replace("-", "");
				return result.ToLower();
			}

			/// <summary>
			/// 对字符串进行Base64加密
			/// </summary>
			/// <param name="str"></param>
			/// <returns></returns>
			public static string Base64Encrypt(string str)
			{
				byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
				return System.Convert.ToBase64String(encbuff);
			}

			/// <summary>
			/// Base64解密
			/// </summary>
			/// <param name="str"></param>
			/// <returns></returns>
			public static string Base64Decrypt(string str)
			{
				byte[] decbuff = System.Convert.FromBase64String(str);
				return System.Text.Encoding.UTF8.GetString(decbuff);
			}

			/// <summary>
			/// GZip 压缩
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			public static string GzipEncrypt(string text)
			{
				byte[] buffer = Encoding.UTF8.GetBytes(text);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
					{
						gZipStream.Write(buffer, 0, buffer.Length);
					}
					return Convert.ToBase64String(memoryStream.ToArray());
				}
			}

			/// <summary>
			/// GZip 压缩
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			public static byte[] GzipEncryptByte(string text)
			{
				byte[] buffer = System.Text.Encoding.UTF8.GetBytes(text);
				using (var memoryStream = new MemoryStream())
				{
					using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
					{
						gzipStream.Write(buffer, 0, buffer.Length);
					}
					return memoryStream.ToArray();
				}
			}

			/// <summary>
			/// GZip 解压
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			public static string GzipDecrypt(string text)
			{
				try
				{
					byte[] gZipBuffer = Convert.FromBase64String(text);
					using (MemoryStream memoryStream = new MemoryStream(gZipBuffer))
					{
						using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
						{
							using (MemoryStream outStream = new MemoryStream())
							{
								byte[] buffer = new byte[4096];
								int read;
								while ((read = gZipStream.Read(buffer, 0, buffer.Length)) > 0)
								{
									outStream.Write(buffer, 0, read);
								}
								return Encoding.UTF8.GetString(outStream.ToArray());
							}
						}
					}
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}

			/// <summary>
			/// GZip 解压
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			public static string GzipDecrypt(byte[] gZipBuffer)
			{
				try
				{
					using (MemoryStream memoryStream = new MemoryStream(gZipBuffer))
					{
						using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
						{
							using (MemoryStream outStream = new MemoryStream())
							{
								byte[] buffer = new byte[4096];
								int read;
								while ((read = gZipStream.Read(buffer, 0, buffer.Length)) > 0)
								{
									outStream.Write(buffer, 0, read);
								}
								return Encoding.UTF8.GetString(outStream.ToArray());
							}
						}
					}
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}

			/// <summary>
			/// Brotli 压缩
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			//public static string CompressBrotliString(string text)
			//{
			//	byte[] buffer = Encoding.UTF8.GetBytes(text);
			//	using (MemoryStream memoryStream = new MemoryStream())
			//	{
			//		using (BrotliStream brotliStream = new BrotliStream(memoryStream, CompressionMode.Compress, true))
			//		{
			//			brotliStream.Write(buffer, 0, buffer.Length);
			//		}
			//		return Convert.ToBase64String(memoryStream.ToArray());
			//	}
			//}

			/// <summary>
			/// Brotli 解压
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			//public static string DecompressBrotliString(string text)
			//{
			//	try
			//	{
			//		byte[] brotliBuffer = Convert.FromBase64String(text);
			//		using (MemoryStream memoryStream = new MemoryStream(brotliBuffer))
			//		{
			//			using (BrotliStream brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
			//			{
			//				using (MemoryStream outStream = new MemoryStream())
			//				{
			//					byte[] buffer = new byte[4096];
			//					int read;
			//					while ((read = brotliStream.Read(buffer, 0, buffer.Length)) > 0)
			//					{
			//						outStream.Write(buffer, 0, read);
			//					}
			//					return Encoding.UTF8.GetString(outStream.ToArray());
			//				}
			//			}
			//		}
			//	}
			//	catch (Exception)
			//	{
			//		return string.Empty;
			//	}
			//}

			/// <summary>
			/// HMACSHA256 加密
			/// </summary>
			/// <param name="message"></param>
			/// <param name="secret"></param>
			/// <returns></returns>
			public static string Sha256Hmac(string message, string secret)
			{
				string hash = "";
				try
				{
					using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
					{
						byte[] bytes = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(message));
						hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
					}
				}
				catch (Exception e)
				{
					Console.WriteLine($"sha256Hmac exception: {e.Message}");
				}
				return hash;
			}

		}
	}
}
