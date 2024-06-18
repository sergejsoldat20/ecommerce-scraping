﻿namespace backend.Entities;

public class ProductHistory
{
	public Guid Id { get; set; }
	public Guid ProductId { get; set; }
	public virtual Product Product { get; set; }
	public double OldValue { get; set; }
	public double NewValue { get; set; }
	public DateTime Timestamp { get; set; }
	public Guid ChangeGroup { get; set; }
}
