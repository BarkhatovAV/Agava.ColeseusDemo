// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.26
// 

using Colyseus.Schema;
using Action = System.Action;

public partial class Player : Schema {
	[Type(0, "string")]
	public string login = default(string);

	[Type(1, "boolean")]
	public bool isWhitePlayer = default(bool);

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

	protected event PropertyChangeHandler<bool> __isWhitePlayerChange;
	public Action OnIsWhitePlayerChange(PropertyChangeHandler<bool> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.isWhitePlayer));
		__isWhitePlayerChange += __handler;
		if (__immediate && this.isWhitePlayer != default(bool)) { __handler(this.isWhitePlayer, default(bool)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(isWhitePlayer));
			__isWhitePlayerChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(login): __loginChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(isWhitePlayer): __isWhitePlayerChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
			default: break;
		}
	}
}

