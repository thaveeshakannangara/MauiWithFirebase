using Newtonsoft.Json;

namespace MauiApp13
{
	public class Branch
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "createdDate")]
		public DateTime CreatedDate { get; set; }
	}
}