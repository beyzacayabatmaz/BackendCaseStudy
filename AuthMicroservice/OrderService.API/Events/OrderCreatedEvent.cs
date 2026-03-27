namespace OrderService.API.Events;

// Bu sınıf, RabbitMQ kuyruğuna bırakacağımız mesajın şablonudur
public class OrderCreatedEvent
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}