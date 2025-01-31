using BetAPI.Events;
using EventStore.Client;
using MassTransit;
using System.Text.Json;

namespace EventStoringAPI.Consumers;

public class EventStoringBetResultedConsumer(EventStoreClient eventStore) : IConsumer<BetResultedEvent>
{
    public async Task Consume(ConsumeContext<BetResultedEvent> context)
    {
        await eventStore.AppendToStreamAsync(context.Message.Bet.CustomerId.ToString(),
           expectedState: StreamState.Any,
           eventData: [new EventData(Uuid.NewUuid(), "BetResultedEvent", JsonSerializer.SerializeToUtf8Bytes(context.Message))],
           cancellationToken: context.CancellationToken);
    }
}
