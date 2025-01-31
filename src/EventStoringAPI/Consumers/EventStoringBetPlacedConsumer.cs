using BetAPI.Events;
using EventStore.Client;
using MassTransit;
using System.Text.Json;

namespace EventStoringAPI.Consumers;

public class EventStoringBetPlacedConsumer(EventStoreClient eventStore) : IConsumer<BetPlacedEvent>
{
    public async Task Consume(ConsumeContext<BetPlacedEvent> context)
    {
        await eventStore.AppendToStreamAsync(context.Message.Bet.CustomerId.ToString(),
            expectedState: StreamState.Any,
            eventData: [new EventData(Uuid.NewUuid(), "BetPlacedEvent", JsonSerializer.SerializeToUtf8Bytes(context.Message))],
            cancellationToken: context.CancellationToken);
    }
}

