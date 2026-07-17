using UnityEngine;
using UnityEngine.UI;

public class UIButtonSelect : MonoBehaviour
{
    public void ChoiceUIButton(int index)
    {
        UISelectStoryManager.Instance.ChangeSelectStoryElent(index);
        //半透明にして無効化する処理の追加
    }
}
