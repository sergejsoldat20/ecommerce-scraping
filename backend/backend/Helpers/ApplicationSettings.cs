namespace backend.Helpers
{
	public class ApplicationSettings
	{
		public static readonly string SectionName = "Jwt";
		public string Key { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public int Minutes { get; set; }
	}
}
