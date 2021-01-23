using System;
using System.Collections;
using System.Collections.Generic;
using Grigorov.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grigorov.Unity.SceneManagement {
	public class SceneWrapper {
		readonly List<Action<Scene>> _loadedActions  = new List<Action<Scene>>();
		readonly List<Action<float>> _loadingActions = new List<Action<float>>();

		string         _sceneName;
		AsyncOperation _asyncOperation;

		public SceneWrapper LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
			_sceneName = sceneName;
			_asyncOperation = SceneManager.LoadSceneAsync(_sceneName, loadSceneMode);
			return this;
		}

		public void AddLoadedAction(Action<Scene> action) {
			_loadedActions.AddIfNotExists(action);
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		public SceneWrapper AddLoadingAction(Action<float> action) {
			_loadingActions.AddIfNotExists(action);
			ShellCoroutine.Instance.StartCoroutine(LoadingCoroutine());
			return this;
		}

		IEnumerator LoadingCoroutine() {
			while ( _asyncOperation != null && !_asyncOperation.isDone ) {
				var progress = _asyncOperation.progress / 0.9f;
				_loadingActions.ForEach(action => action?.Invoke(progress));
				yield return null;
			}

			_loadingActions.Clear();
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) {
			if ( scene.name != _sceneName ) {
				return;
			}

			_loadedActions.ForEach(action => action?.Invoke(scene));
			SceneManager.sceneLoaded -= OnSceneLoaded;
			_loadedActions.Clear();
		}
	}
}