namespace SaveSystem {
	/// <summary>
	/// Interface for objects that can be saved.
	/// </summary>
	public interface ISaveable {

		void Load(Save save);

		void Save(Save save);
	}
}
