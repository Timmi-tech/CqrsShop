using Application.Interfaces;
using Application.Interfaces.Contracts;
using Domain.Common;
using MediatR;

namespace Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductHandler(IRepositoryManager repository, ILoggerManager logger) : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = (await _repository.Product
                            .GetProductsByIdsAsync(new[] { request.Id }, trackChanges: false))
                            .FirstOrDefault();

            if (product is null)
            {
                _logger.LogError($"Product with id: {request.Id} doesn't exist in the database.");
                return Result.Failure(Error.NotFound("Product", request.Id.ToString()));
            }

            _repository.Product.DeleteProduct(product);
            await _repository.SaveAsync();
            
            return Result.Success();
        }
    }
}