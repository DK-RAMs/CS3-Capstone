using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.UILibrary
{
	public class LevelLoader : MonoBehaviour
	{
		public Animator transition;

		public float transitionTime = 1f;
		// Update is called once per frame

		private void Update()
		{

		}

		public void loadNextLevel()
		{
			StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
		}

		IEnumerator loadLevel(int levelIndex)
		{
			transition.SetTrigger("Start");
			yield return new WaitForSeconds(transitionTime);

			SceneManager.LoadScene(levelIndex);
		}
	}
}