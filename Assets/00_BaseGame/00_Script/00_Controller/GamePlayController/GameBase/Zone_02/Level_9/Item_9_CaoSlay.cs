using Cysharp.Threading.Tasks;

public class Item_9_CaoSlay : ItemBase, IPostPlacementAction
{
    public void HandlePostPlacementAction()
    {
        gameObject.SetActive(false);
        var Level9 = (Level_9)GamePlayController.Instance.levelController.currentLevel;
        Level9.HandleCaoSlayAnimation().Forget();
    }
}