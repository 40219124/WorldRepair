
public interface IReadOnlyEventField<T> : IEventField
{
	T Value { get; }
}
