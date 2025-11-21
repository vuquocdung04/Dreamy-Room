
public class Item_8_Shiba : ItemBase, IPostPlacementAction
{
    public void HandlePostPlacementAction()
    {
        gameObject.SetActive(false);
        var level8 = (Level_8)GamePlayController.Instance.levelController.currentLevel;
        level8.level8Phase2.HandleShibaAnimation();
    }
}