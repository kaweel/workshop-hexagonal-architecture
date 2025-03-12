using Microsoft.AspNetCore.Mvc;
using API.Entity;
using Application.Port;
using AutoMapper;

namespace API.Controller
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServicePort _orderPort;
        private readonly IMapper _mapper;

        public OrderController(IOrderServicePort orderPort, IMapper mapper)
        {
            _orderPort = orderPort;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            return Ok(_mapper.Map<OrderResponse>(await _orderPort.GetOrderById(id)));
        }
    }
}
