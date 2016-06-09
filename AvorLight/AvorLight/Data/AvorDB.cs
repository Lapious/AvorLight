using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using System.Reflection;

#if __ANDROID__
using Android.Content.Res;
#endif

namespace AvorLight.Data
{
	public sealed class AvorDB : IDataService
	{
		static object locker = new object();

		private const string DatabaseName = "AvorDB";

		private static string DatabasePath
		{
			get
			{
#if __IOS__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				var path = Path.Combine(libraryPath, DatabaseName);
#else
#if __ANDROID__
				string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
				var path = Path.Combine(documentsPath, DatabaseName);
#else
				// WinPhone
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, DatabaseName);;
#endif
#endif
				return path;
			}
		}

#if __ANDROID__

		public static void SetupDatabaseIfNeeded(AssetManager Assets)
		{
			// Check if your DB has already been extracted.
			// Uncomment for production? 
			//if (!File.Exists(DatabasePath))
			{
				using (BinaryReader br = new BinaryReader(Assets.Open(DatabaseName)))
				{
					using (BinaryWriter bw = new BinaryWriter(new FileStream(DatabasePath, FileMode.Create)))
					{
						byte[] buffer = new byte[2048];
						int len = 0;
						while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
						{
							bw.Write(buffer, 0, len);
						}
					}
				}

			}
		}
#endif

		private SQLiteConnection _connection;
		private SQLiteConnection Connection
		{
			get
			{
				if (_connection == null)
					_connection = new SQLiteConnection(DatabasePath);

				return _connection;
			}
		}

		public IEnumerable<ProductCategory> GetProductCategory()
		{
			lock (locker)
				return Connection.Table<ProductCategory>();
		}

		public IEnumerable<Product> GetProduct(int categoryId)
		{
			lock (locker)
				return Connection.Table<Product>()
									.Where(p => p.CategoryID == categoryId);
		}

		public void AddCartProduct(Product product)
		{
			lock(locker)
			{
				// Create table if it doesn't exist
				Connection.CreateTable<CartProduct>();

				Connection.Insert(product, typeof(CartProduct));
			}
		}

		public void DeleteCartProduct(Product Product)
		{
			lock (locker)
			{
				// Create table if it doesn't exist
				Connection.CreateTable<CartProduct>();

				Connection.Delete<CartProduct>(Product.ID);
			}
		}

		public IEnumerable<CartProduct> GetCartProduct()
		{
			lock (locker)
			{
				// Create table if it doesn't exist
				Connection.CreateTable<CartProduct>();

				return Connection.Table<CartProduct>();
			}
		}

		public void Checkout()
		{
			lock (locker)
			{
				// Create table if it doesn't exist
				Connection.CreateTable<CartProduct>();
				Connection.DeleteAll<CartProduct>();
			}
		}
	}
}
