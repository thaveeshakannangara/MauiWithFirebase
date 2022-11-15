using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;

namespace MauiApp13
{
	public class MainPageViewModel : BaseViewModel
	{
		private readonly IDataStore<Todo> _todoFirebaseService;
		private ObservableCollection<TodoModel> _todoCollection;
		private BranchView _branchPage;

		public string Name { get; set; }
		public string Priority { get; set; }
		public string PrimaryKey { get; set; }

		public ObservableCollection<TodoModel> TodoCollection
		{
			get => _todoCollection;
			set
			{
				if (_todoCollection == value) return;
				_todoCollection = value;
				OnPropertyChanged(nameof(TodoCollection));
			}
		}

		public ICommand ICommandAddRecordsTapped { get; set; }
		public ICommand ICommandGetRecordsTapped { get; set; }
		public ICommand ICommandGetOneRecordTapped { get; set; }
		public ICommand ICommandUpdateRecordTapped { get; set; }
		public ICommand ICommandDeleteRecordTapped { get; set; }
		public ICommand ICommandNavigateTapped { get; set; }

		public MainPageViewModel(IDataStore<Todo> todoFirebaseService, BranchView branchPage)
		{
			_todoCollection = new ObservableCollection<TodoModel>();
			_todoFirebaseService = todoFirebaseService;
			_branchPage = branchPage;

			ICommandAddRecordsTapped = new Command(async () => await AddRecords());
			ICommandGetRecordsTapped = new Command(async () => await GetRecords());
			ICommandGetOneRecordTapped = new Command(async () => await GetOneRecord());
			ICommandUpdateRecordTapped = new Command(async () => await UpdateRecord());
			ICommandDeleteRecordTapped = new Command(async () => await DeleteRecord());
			ICommandNavigateTapped = new Command(async () => await NavigateToNext());

			GetRecords();
		}

		private async Task NavigateToNext()
		{
			await Application.Current.MainPage.Navigation.PushAsync(_branchPage);
		}

		private async Task AddRecords()
		{
			try
			{
				var todo = new Todo
				{
					Name = Name,
					Priority = Convert.ToInt32(Priority),
					CreatedDate = DateTime.Now
				};

				var result = await _todoFirebaseService.AddItemAsync(todo);

				if (result is null || result.Object is null) return;

				TodoCollection.Add(new TodoModel()
				{
					Id = result.Key,
					Name = result.Object.Name,
					Priority = result.Object.Priority,
					CreatedDate = result.Object.CreatedDate
				});
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
			}
		}

		private async Task UpdateRecord()
		{
			try
			{
				string id = PrimaryKey;
				var todo = new Todo
				{
					Name = Name,
					Priority = Convert.ToInt32(Priority),
					CreatedDate = DateTime.Now
				};

				bool isUpdated = await _todoFirebaseService.UpdateItemAsync(todo, id);

				if (isUpdated)
				{
					var updatedItem = TodoCollection.FirstOrDefault(x => x.Id == id);
					int i = TodoCollection.IndexOf(updatedItem);

					updatedItem.Name = todo.Name;
					updatedItem.Priority = todo.Priority;
					TodoCollection[i] = updatedItem;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
			}
		}

		private async Task DeleteRecord()
		{
			try
			{
				string id = PrimaryKey;
				bool isDeleted = await _todoFirebaseService.DeleteItemAsync(id);
				if (isDeleted)
				{
					var removedItem = TodoCollection.FirstOrDefault(x => x.Id == id);
					TodoCollection.Remove(removedItem);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
			}
		}

		private async Task GetRecords()
		{
			try
			{
				var firebaseObjects = await _todoFirebaseService.GetAllItemsAsync();

				foreach (var item in firebaseObjects)
				{
					if (item is null || item.Object is null) return;

					TodoCollection.Add(new TodoModel()
					{
						Id = item.Key,
						Name = item.Object.Name,
						Priority = item.Object.Priority,
						CreatedDate = item.Object.CreatedDate
					});
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
			}
		}

		private async Task GetOneRecord()
		{
			try
			{
				var val = await _todoFirebaseService.GetItemAsync(PrimaryKey);
				Application.Current.MainPage.DisplayAlert("Alert", $"Id : {PrimaryKey},Name : {val.Name}, Priority : {val.Priority}", "Ok");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
			}
		}
	}
}