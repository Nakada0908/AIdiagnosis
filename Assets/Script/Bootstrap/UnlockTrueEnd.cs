using UnityEngine;

public class UnlockTrueEnd : MonoBehaviour
{
    public static UnlockTrueEnd Instance;

    private int lookEndCnt = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddLookedEnd()
    {
        lookEndCnt++;
    }

    public int GetlookEndCnt()
    {
        return lookEndCnt;
    }
}
