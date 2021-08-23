using UnityEngine;

namespace UI {
	namespace Portrait {
		public abstract class Bar : MonoBehaviour {

			private Container[] _container;
			private float _currentValue;

			private void Awake() {
				_container = new Container[transform.childCount];

				for (int i = 0; i < transform.childCount; i++) {
					_container[i] = transform.GetChild(i).GetComponent<Container>();
				}
			}

			public void InitializeQuarter(int numberOfQuarter) {
				float nbOfFilled = (numberOfQuarter / (float)4);
				int roundNbOfFilled = Mathf.RoundToInt(nbOfFilled);
				float dec = (nbOfFilled - roundNbOfFilled) * 4;

				for (int i = 0; i < nbOfFilled; i++) {
					_container[i].SetAmountOfQuarter(4);
				}

				if (roundNbOfFilled < transform.childCount) {
					_container[roundNbOfFilled].SetAmountOfQuarter(Mathf.RoundToInt(dec));
				}
			}

			public void UpdateQuarter(float value) {
				float diff = value - _currentValue;
				if (diff >= 0.245) {
					int quarter = Mathf.FloorToInt(diff / 0.245f);
					diff %= 0.245f;
					AddQuarter(quarter);
					_currentValue = value;
				} else if (diff <= -0.245) {
					RemoveQuarter(Mathf.CeilToInt(-diff));
					_currentValue = value;
				}
			}

			public void AddQuarter(int numberOfQuarter) {
				int counter = numberOfQuarter;
				for (int i = 0; i < _container.Length; i++) {
					if (!_container[i].Full) {
						int currentNbQuarter = (int)_container[i].GetAmountOfQuarter();

						int nbOfEmptyQuarter = 4 - currentNbQuarter;

						if (nbOfEmptyQuarter >= counter) {
							_container[i].SetAmountOfQuarter(currentNbQuarter + counter);
							counter = 0;
						} else {
							_container[i].SetAmountOfQuarter(4);
							counter -= nbOfEmptyQuarter;
						}

						if (counter <= 0) {
							break;
						}
					}
				}
			}

			public void RemoveQuarter(int numberOfQuarter) {
				int counter = Mathf.Abs(numberOfQuarter);
				for (int i = _container.Length; i-- > 0;) {
					if (!_container[i].Empty) {
						int currentNbQuarter = (int)_container[i].GetAmountOfQuarter();
						if (currentNbQuarter >= counter) {
							int difference = currentNbQuarter - counter;
							_container[i].SetAmountOfQuarter(difference);
							counter = 0;
						} else {
							_container[i].SetAmountOfQuarter(0);
							counter -= currentNbQuarter;
						}

						if (counter <= 0) {
							break;
						}
					}
				}
			}
		}
	}
}
