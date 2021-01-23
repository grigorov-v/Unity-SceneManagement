using UnityEngine;

namespace Grigorov.Unity.SceneManagement {
	public class ShellCoroutine : MonoBehaviour {
		static ShellCoroutine _instance;

		public static ShellCoroutine Instance {
			get {
				if ( !_instance ) {
					_instance = FindObjectOfType<ShellCoroutine>();
					_instance = !_instance
						? new GameObject("[ShellCoroutine]").AddComponent<ShellCoroutine>()
						: _instance;
					DontDestroyOnLoad(_instance.gameObject);
				}

				return _instance;
			}
		}
	}
}