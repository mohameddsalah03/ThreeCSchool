using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ThreeCSchool.Core.Domain.Contracts.Persistence;
using ThreeCSchool.Core.Service.Abstraction.Services;
using ThreeCSchool.Core.Service.Abstraction.Services.Auth;
using ThreeCSchool.Core.Service.Abstraction.Services.Categories;
using ThreeCSchool.Core.Service.Services.Categories;

namespace ThreeCSchool.Core.Service.Services
{
    internal class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<ICategoryService> _categoryService;

        public ServiceManager(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IServiceProvider serviceProvider
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _serviceProvider = serviceProvider;

            _authService = new Lazy<IAuthService>(() => _serviceProvider.GetRequiredService<IAuthService>());
            _categoryService = new Lazy<ICategoryService>(() => new CategoryService(_unitOfWork, _mapper));
        }

        public IAuthService AuthService => _authService.Value;
        public ICategoryService CategoryService => _categoryService.Value;
    }
}