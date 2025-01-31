using CustomerAPI.Events;
using EventStore.Client;
using MassTransit;
using System.Text.Json;

namespace EventStoringAPI.Consumers;

public class EventStoringCustomerCreatedConsumer(EventStoreClient eventStore) : IConsumer<CustomerCreatedEvent>
{
    public async Task Consume(ConsumeContext<CustomerCreatedEvent> context)
    {
        await eventStore.AppendToStreamAsync(context.Message.Customer.Id.ToString(),
           expectedState: StreamState.Any,
           eventData: [new EventData(Uuid.NewUuid(), "CustomerCreatedEvent", JsonSerializer.SerializeToUtf8Bytes(context.Message))],
           cancellationToken: context.CancellationToken);
    }
}
