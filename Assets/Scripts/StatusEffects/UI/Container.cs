using UnityEngine;
using UnityEngine.UI;

namespace UI {
	namespace Portrait {
		public class Container : MonoBehaviour {

			private bool _empty;
			private bool _full;
			private Image _image;

			private void Awake() {
				_image = transform.Find("Image").GetComponent<Image>();
			}

			public int GetAmountOfQuarter() {
				if (_image.fillAmount == 0.25f) return 1;
				else if (_image.fillAmount == 0.5f) return 2;
				else if (_image.fillAmount == 0.75f) return 3;
				else if (_image.fillAmount == 1f) return 4;
				else return 0;
			}

			public bool SetAmountOfQuarter(int Quarter) {
				switch (Quarter) {
					case 0:
						_image.fillAmount = 0;
						_empty = true;
						_full = false;
						break;
					case 1:
						_image.fillAmount = 0.25f;
						_empty = false;
						_full = false;
						break;
					case 2:
						_image.fillAmount = 0.5f;
						_empty = false;
						_full = false;
						break;
					case 3:
						_image.fillAmount = 0.75f;
						_empty = false;
						_full = false;
						break;
					case 4:
						_image.fillAmount = 1;
						_empty = false;
						_full = true;
						break;
				}

				return _empty;
			}

			public bool Full { get; private set; }
			public bool Empty { get; private set; }
		}
	}
}