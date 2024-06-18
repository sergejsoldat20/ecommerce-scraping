using backend.Enums;
using Newtonsoft.Json;

namespace backend.Entities.DTO;

public class FilterDTO
{
	public double? priceFrom { get; set; } = 0;
	public double? priceTo { get; set; } = Int32.MaxValue;
	public string gender { get; set; } = string.Empty;
	public string shopName { get; set; } = string.Empty;
	public Guid brandId { get; set; } = Guid.Empty;
	public string sortType { get; set; } 
	public string searchString {get; set; } = string.Empty;

}

public class SortType
{
	public static string BY_NAME = "BY_NAME"; // 0
	public static string BY_PRICE_ASC = "BY_PRICE_ASC"; // 0
	public static string BY_PRICE_DESC = "BY_PRICE_DESC"; // 0
}
