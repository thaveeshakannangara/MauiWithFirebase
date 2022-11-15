using Newtonsoft.Json;

namespace MauiApp13
{
	public class Todo
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "priority")]
		public int Priority { get; set; }

		[JsonProperty(PropertyName = "createdDate")]
		public DateTime CreatedDate { get; set; }
	}
}