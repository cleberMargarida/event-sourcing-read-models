using BetAPI.Events;
using EventStore.Client;
using MassTransit;
using System.Text.Json;

namespace EventStoringAPI.Consumers;

public class EventStoringBetSettledConsumer(EventStoreClient eventStore) : IConsumer<BetSettledEvent>
{
    public async Task Consume(ConsumeContext<BetSettledEvent> context)
    {
        await eventStore.AppendToStreamAsync(context.Message.Bet.CustomerId.ToString(),
           expectedState: StreamState.Any,
           eventData: [new EventData(Uuid.NewUuid(), "BetSettledEvent", JsonSerializer.SerializeToUtf8Bytes(context.Message))],
           cancellationToken: context.CancellationToken);
    }
}
