using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.ProductTypes.Commands
{
    public class CreateProductTypeCommand : IRequest<Either<BaseException, ProductType>>
    {
        public required string Title { get; init; }
    }

    public class CreateProductTypeCommandHandler(
    IRepository<ProductType> productTypeRepository,
    IProductTypeQueries productTypeQueries,
    ICurrentUserService currentUserService)
    : IRequestHandler<CreateProductTypeCommand, Either<BaseException, ProductType>>
    {
        public async Task<Either<BaseException, ProductType>> Handle(
            CreateProductTypeCommand request,
            CancellationToken cancellationToken)
        {
            var existingProductType = await productTypeQueries.GetByTitleAsync(request.Title, cancellationToken);

            return await existingProductType.MatchAsync(
                pt => new ProductTypeAlreadyExistsException(pt.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<BaseException, ProductType>> CreateEntity(
            CreateProductTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User not authenticated");

                var productType = await productTypeRepository.CreateAsync(
                    ProductType.New(
                        request.Title,
                        userId),
                    cancellationToken);

                return productType;
            }
            catch (Exception ex)
            {
                return new UnhandledProductTypeException(0, ex);
            }
        }
    }
}