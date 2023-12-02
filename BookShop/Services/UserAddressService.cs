using BookShop.Dtos.UserAddressDto;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class UserAddressService : IUserAddressService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserAddressRepository _userAddressRepository;

    public UserAddressService(IUnitOfWork unitOfWork, IUserAddressRepository userAddressRepository)
    {
        _unitOfWork = unitOfWork;
        _userAddressRepository = userAddressRepository;
    }
    
    public async Task AddAsync(AddUserAddressDto addUserAddressDto)
    {
        var userAddress = new UserAddress
        {
            ApplicationUserId = addUserAddressDto.ApplicationUserId,
            AddressType = addUserAddressDto.AddressType,
            StreetAddress = addUserAddressDto.StreetAddress,
            City = addUserAddressDto.City,
            State = addUserAddressDto.State,
            PostalCode = addUserAddressDto.PostalCode,
            Country = addUserAddressDto.Country,
            PhoneNumber = addUserAddressDto.PhoneNumber
        };
        await _unitOfWork.AddAsync(userAddress);
        await _unitOfWork.SaveAsync();
    }

    public async Task EditAsync(EditUserAddressDto editUserAddressDto)
    {
        var userAddress = await _userAddressRepository.GetByIdAsync(editUserAddressDto.Id);
        if (userAddress == null) throw new Exception("User address not found");
        userAddress.ApplicationUserId = editUserAddressDto.ApplicationUserId;
        userAddress.AddressType = editUserAddressDto.AddressType;
        userAddress.StreetAddress = editUserAddressDto.StreetAddress;
        userAddress.City = editUserAddressDto.City;
        userAddress.State = editUserAddressDto.State;
        userAddress.PostalCode = editUserAddressDto.PostalCode;
        userAddress.Country = editUserAddressDto.Country;
        userAddress.PhoneNumber = editUserAddressDto.PhoneNumber;
        await _unitOfWork.SaveAsync();
    }
}