namespace backend.Entities;

public class Brand
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public virtual List<Product> Products { get; set; } = new List<Product>();
}
