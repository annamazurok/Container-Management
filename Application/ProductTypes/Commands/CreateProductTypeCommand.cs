using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
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
    IProductTypeQueries productTypeQueries)
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
                var productType = await productTypeRepository.CreateAsync(
                    ProductType.New(
                        request.Title,
                        1), // TODO: Replace with actual userId from ICurrentUserService
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