using backend.Enums;

namespace backend.Entities.DTO;

public class ProductResponse
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string ProductUrl { get; set; } = string.Empty;
	public double Price { get; set; }
	public double PriceWithDiscount { get; set; }
	public string ShopName { get; set; } = string.Empty;
	public string PhotoUrl { get; set; } = string.Empty;
	public Guid BrandId { get; set; }
	// public string BrandName { get; set; } = string.Empty;
	public Gender Gender { get; set; }
}
