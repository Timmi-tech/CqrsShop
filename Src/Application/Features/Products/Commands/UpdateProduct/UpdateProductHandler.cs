using Application.Interfaces;
using Application.Interfaces.Contracts;
using Domain.Common;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductHandler(IRepositoryManager repository, ILoggerManager logger) : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = (await _repository.Product
                            .GetProductsByIdsAsync(new[] { request.Id }, request.TrackChanges))
                            .FirstOrDefault();

            if (product is null)
            {
                _logger.LogError($"Product with id: {request.Id} doesn't exist in the database.");
                return Result.Failure(Error.NotFound("Product", request.Id.ToString()));
            }

            product.Update(request.Name, request.Description, request.Price, request.Category);

            await _repository.SaveAsync();
            return Result.Success();
        }

    }
}
