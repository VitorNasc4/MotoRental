using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Entities;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MediatR;
using MotoRental.Core.Exceptions;

namespace MotoRental.Application.Commands.UpdateUserToAdmin
{
    public class UpdateUserToAdminCommandHandler : IRequestHandler<UpdateUserToAdminCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserToAdminCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Unit> Handle(UpdateUserToAdminCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            
            if (user is null)
            {
                throw new UserIdNotFoundException(request.UserId);
            }

            user.SetAdmin(request.IsAdmin);
            
            await _userRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}