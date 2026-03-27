using System;
using System.Collections.Generic;
using MediatR;
using ProductService.Core.Entities; 

namespace ProductService.Application.Features.Products.Queries
{
    public class GetAllProductsQuery : IRequest<List<Product>>
    {
    }
}