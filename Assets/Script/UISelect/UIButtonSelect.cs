using UnityEngine;

public class UIButtonSelect : MonoBehaviour
{
    public void ChoiceUIButton(int index)
    {
        UISelectStoryManager.Instance.ChangeSelectStoryElent(index);
        //半透明にする処理の追加
    }
}
