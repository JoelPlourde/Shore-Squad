namespace SaveSystem {
	public interface ISaveable {

		void Load(Save save);

		void Save(Save save);
	}
}
