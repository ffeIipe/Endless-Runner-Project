using UnityEngine.SceneManagement;

namespace ScreenManagerFolder
{
    public class LevelSelectionScreen : BaseScreen
    {
        public void LoadSelectedScene(int level)
        {
            SceneManager.LoadScene(level);
        }
    }
}