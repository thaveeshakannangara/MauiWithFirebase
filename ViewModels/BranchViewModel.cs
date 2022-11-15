using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiApp13
{
	public class BranchViewModel : BaseViewModel
	{
		private readonly IDataStore<Branch> _branchFirebaseService;
		private ObservableCollection<BranchModel> _branchCollection;

		public string Name { get; set; }
		public string Description { get; set; }
		public string PrimaryKey { get; set; }

		public ObservableCollection<BranchModel> BranchCollection
		{
			get => _branchCollection;
			set
			{
				if (_branchCollection == value) return;
				_branchCollection = value;
				OnPropertyChanged(nameof(BranchCollection));
			}
		}

		public ICommand ICommandAddRecordsTapped { get; set; }
		public ICommand ICommandGetRecordsTapped { get; set; }
		public ICommand ICommandGetOneRecordTapped { get; set; }
		public ICommand ICommandUpdateRecordTapped { get; set; }
		public ICommand ICommandDeleteRecordTapped { get; set; }

		public BranchViewModel(IDataStore<Branch> branchFirebaseService)
		{
			_branchCollection = new ObservableCollection<BranchModel>();
			_branchFirebaseService = branchFirebaseService;

			ICommandAddRecordsTapped = new Command(async () => await AddRecords());
			ICommandGetRecordsTapped = new Command(async () => await GetRecords());
			ICommandGetOneRecordTapped = new Command(async () => await GetOneRecord());
			ICommandUpdateRecordTapped = new Command(async () => await UpdateRecord());
			ICommandDeleteRecordTapped = new Command(async () => await DeleteRecord());

			GetRecords();
		}

		private async Task AddRecords()
		{
			try
			{
				var branch = new Branch
				{
					Name = Name,
					Description = Description,
					CreatedDate = DateTime.Now
				};

				var result = await _branchFirebaseService.AddItemAsync(branch);

				if (result is null || result.Object is null) return;

				BranchCollection.Add(new BranchModel()
				{
					Id = result.Key,
					Name = result.Object.Name,
					Description = result.Object.Description,
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
				var branch = new Branch
				{
					Name = Name,
					Description = Description,
					CreatedDate = DateTime.Now
				};

				bool isUpdated = await _branchFirebaseService.UpdateItemAsync(branch, id);

				if (isUpdated)
				{
					var updatedItem = BranchCollection.FirstOrDefault(x => x.Id == id);
					int i = BranchCollection.IndexOf(updatedItem);

					updatedItem.Name = branch.Name;
					updatedItem.Description = branch.Description;
					BranchCollection[i] = updatedItem;
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
				bool isDeleted = await _branchFirebaseService.DeleteItemAsync(id);
				if (isDeleted)
				{
					var removedItem = BranchCollection.FirstOrDefault(x => x.Id == id);
					BranchCollection.Remove(removedItem);
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
				var firebaseObjects = await _branchFirebaseService.GetAllItemsAsync();

				foreach (var item in firebaseObjects)
				{
					if (item is null || item.Object is null) return;

					BranchCollection.Add(new BranchModel()
					{
						Id = item.Key,
						Name = item.Object.Name,
						Description = item.Object.Description,
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
				var val = await _branchFirebaseService.GetItemAsync(PrimaryKey);
				Application.Current.MainPage.DisplayAlert("Alert", $"Id : {PrimaryKey},Name : {val.Name}, Priority : {val.Description}", "Ok");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
			}
		}
	}
}