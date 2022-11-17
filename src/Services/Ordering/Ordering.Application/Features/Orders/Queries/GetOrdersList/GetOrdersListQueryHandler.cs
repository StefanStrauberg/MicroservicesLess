using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    internal class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderVm>>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        
        public GetOrdersListQueryHandler(IMapper mapper, IOrderRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }
        
        public async System.Threading.Tasks.Task<List<OrderVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _repository.GetOrdersByUserName(request.UserName);
            return _mapper.Map<List<OrderVm>>(orderList);
        }
    }
}
