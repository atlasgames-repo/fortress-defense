using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserV2 : User
{
    public static async Task<bool> SyncSetCoin(int value)
    {
        await APIManager.instance.Request_Gem(new GemRequestModel(_amount: value.ToString()));
        return true;
    }
}
