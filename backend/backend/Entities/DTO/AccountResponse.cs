﻿namespace backend.Entities.DTO;

public class AccountResponse
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string PhoneNumber { get; set; } = string.Empty;
	
}
