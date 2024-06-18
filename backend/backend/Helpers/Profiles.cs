using AutoMapper;
using backend.Entities;
using backend.Entities.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Helpers;

public class UserMapper : Profile
{
	public UserMapper()
	{
		CreateMap<RegisterRequest, Account>();
		CreateMap<Product, ProductResponse>();
	}
}
