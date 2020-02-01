using System;


public interface IEventField : IEventWrapper
{
	EventFieldHandlerCollection Handlers { get; }
	Action OnBeforeChanged { get; set; }
	Action OnAfterChanged { get; set; }
}

public interface IEventField<T> : IReadOnlyEventField<T>
{
	new T Value { get; set; }
}
