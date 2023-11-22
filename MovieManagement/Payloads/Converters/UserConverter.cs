using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataUser;

namespace MovieManagement.Payloads.Converters
{
    public class UserConverter
    {
        public DataResponseUser EntityToDTO(User user)
        {
            return new DataResponseUser
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Point = user.Point,
                Username = user.Username
            };
        }
    }
}
