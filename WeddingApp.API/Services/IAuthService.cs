using System;
using System.Threading.Tasks;
using WeddingApp.API.Dtos;

namespace WeddingApp.API.Services
{
    public interface IAuthService
    {
        Task<UserForDetailedDto> Register(UserForRegisterDto userForRegisterDto);
        Task<Tuple<UserForListDto, string>> Login(UserForLoginDto userForLoginDto);
    }
}