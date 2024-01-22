// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.26
// 

using Colyseus.Schema;
using Action = System.Action;

public partial class HeyWhere : Schema {
	[Type(0, "string")]
	public string login = default(string);

	[Type(1, "boolean")]
	public bool isTurnReady = default(bool);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<string> __loginChange;
	public Action OnLoginChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.login));
		__loginChange += __handler;
		if (__immediate && this.login != default(string)) { __handler(this.login, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(login));
			__loginChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<bool> __isTurnReadyChange;
	public Action OnIsTurnReadyChange(PropertyChangeHandler<bool> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.isTurnReady));
		__isTurnReadyChange += __handler;
		if (__immediate && this.isTurnReady != default(bool)) { __handler(this.isTurnReady, default(bool)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(isTurnReady));
			__isTurnReadyChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(login): __loginChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(isTurnReady): __isTurnReadyChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
			default: break;
		}
	}
}

