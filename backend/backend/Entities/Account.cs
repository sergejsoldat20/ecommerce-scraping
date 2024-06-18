﻿namespace backend.Entities;

public class Account
{
	public Guid Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public string PhoneNumber { get; set; }
	public bool IsEmailConfirmed { get; set; }
	public string Role { get; set; }
}
