using System;
using System.Diagnostics;

public class EventField<T> : IEventField<T>
{
	public EventFieldHandlerCollection Handlers { get; set; }
	public Action OnBeforeChanged { get; set; }
	public Action OnAfterChanged { get; set; }

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private T InternalValue;

	public T Value
	{
		get => InternalValue;
		set
		{
			Handlers.InvokeBeforeChanged();
			OnBeforeChanged?.Invoke();

			InternalValue = value;

			Handlers.InvokeAfterChanged();
			OnAfterChanged?.Invoke();
		}
	}

	public EventField()
	{
		Handlers = new EventFieldHandlerCollection(this);
	}

	public EventField(T value)
		: this()
	{
		InternalValue = value;
	}
}