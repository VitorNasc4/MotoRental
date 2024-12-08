using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Repositories;
using MotoRental.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MotoRental.Application.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserViewModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id);

            if (user == null)
            {
                return null;
            }

            return new UserViewModel(user.FullName, user.Email);
        }
    }
}