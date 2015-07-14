using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DiamondJam.View
{
	/// <summary>
	/// This script loads a level on click of a Button.
	/// </summary>
	public class LoadLevel : MonoBehaviour {

		/// <summary>
		/// The name of the level to load.
		/// </summary>
		public string levelName;

		/// <summary>
		/// The button to register.
		/// </summary>
		private Button button;

		void Start () {
			button = GetComponent<Button>();
			button.onClick.AddListener(loadLevel);
		}

		/// <summary>
		/// Loads the level.
		/// </summary>
		private void loadLevel(){
			Application.LoadLevel(levelName);
		}
	}
}
