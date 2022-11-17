using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;
    
    public CheckoutOrderCommandHandler(IOrderRepository repository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        _emailService = emailService ?? throw new AggregateException(nameof(emailService));
        _logger = logger ?? throw new AggregateException(nameof(logger));
    }
    
    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(request);
        var newOrder = await _repository.AddAsync(orderEntity);

        await SendEmail(newOrder);

        return newOrder.Id;
    }
    
    private async Task SendEmail(Order newOrder)
    {
        var email = new Email()
            { To = "StefanStrauberg@gmail.com", Body = "Order was created.", Subject = "Order was created." };
        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Order {newOrder.Id} failed due to an error with mail service: {ex.Message}");
        }
    }
}