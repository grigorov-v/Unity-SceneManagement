using Grigorov.Unity.SceneManagement.UI;

namespace Grigorov.Unity.SceneManagement {
	public static class SceneLoadingHelper {
		static readonly SceneWrapper _targetSceneHandler = new SceneWrapper();

		public static SceneWrapper StartLoadingScene(string sceneName, LoadingUI loadingUI) {
			loadingUI.Show();
			_targetSceneHandler.LoadSceneAsync(sceneName)
				.AddLoadingAction(loadingUI.UpdateBar)
				.AddLoadedAction(scene => loadingUI.Hide());

			return _targetSceneHandler;
		}
	}
}