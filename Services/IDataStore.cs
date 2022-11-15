using Firebase.Database;

namespace MauiApp13
{
	public interface IDataStore<T> where T : class
	{
		Task<FirebaseObject<T>> AddItemAsync(T item);
		Task<bool> UpdateItemAsync(T item, string id);
		Task<bool> DeleteItemAsync(string id);
		Task<T> GetItemAsync(string id);
		Task<IReadOnlyCollection<FirebaseObject<T>>> GetAllItemsAsync();
	}
}