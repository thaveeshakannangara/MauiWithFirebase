using Firebase.Database;
using Firebase.Database.Query;

namespace MauiApp13
{
	public class FirebaseDataStore<T> : IDataStore<T> where T : class
	{
		private readonly ChildQuery _firebaseQuery;

		public FirebaseDataStore()
		{
			_firebaseQuery = new FirebaseClient(AppConstants.FirebaseUrl).Child(GetTableName(typeof(T)));
		}

		public async Task<FirebaseObject<T>> AddItemAsync(T item)
		{
			try
			{
				return await _firebaseQuery
							.PostAsync(item);
			}
			catch (Exception e)
			{
				//Need to Log and Handle the return
				Console.WriteLine(e.Message.ToString());
				return null;
			}
		}

		public async Task<bool> DeleteItemAsync(string id)
		{
			try
			{
				await _firebaseQuery
					.Child(id)
					.DeleteAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
				return false;
			}
			return true;
		}

		public async Task<bool> UpdateItemAsync(T item, string id)
		{
			try
			{
				await _firebaseQuery
					.Child(id)
					.PutAsync(item);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
				return false;
			}

			return true;
		}

		public async Task<IReadOnlyCollection<FirebaseObject<T>>> GetAllItemsAsync()
		{
			try
			{
				return await _firebaseQuery.OnceAsync<T>();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
				return null;
			}
		}

		public async Task<T> GetItemAsync(string id)
		{
			try
			{
				return await _firebaseQuery
					.Child(id)
					.OnceSingleAsync<T>();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
				return null;
			}
		}

		private string GetTableName(Type type)
		{
			return $"{type.Name.ToLower()}s";
		}
	}
}