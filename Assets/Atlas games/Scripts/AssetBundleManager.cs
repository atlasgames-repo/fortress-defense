using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System;
public class AssetBundleManager : MonoBehaviour
{
    public AssetBundleGenerator[] bundles;
    //public ScriptableObject asd;

    async void Awake()
    {
        await LoadDownloadedAssets();
    }
    async Task LoadDownloadedAssets()
    {

        await Task.Delay(1);
        List<Task> tasks = new List<Task>(bundles.Length);
        for (int i = 0; i < bundles.Length; i++)
        {
            tasks.Add(LoadFromFile(i));
        }
        await Task.WhenAll(tasks);
    }
    public async Task LoadFromFile(int i)
    {
        var updates = await APIManager.instance.check_for_updates(type: bundles[i].name);
        if (File.Exists(APIManager.instance.GetFilePath(updates.list[0])))
        {
            AssetBundleCreateRequest bundle = AssetBundle.LoadFromFileAsync(APIManager.instance.GetFilePath(updates.list[0]));

            while (!bundle.isDone)
            {
                await Task.Yield();
            }
            foreach (var item in bundle.assetBundle.LoadAllAssets<GameObject>())
            {
                GameObject obj;
                if (bundles[i].is_root_world)
                {
                    obj = Instantiate(item);
                    obj.name = item.name;
                }
                else
                {
                    obj = Instantiate(item, bundles[i].root, false);
                    obj.name = item.name;
                }

                if (bundles[i].has_depends)
                {
                    bundles[i].depends_type.SendMessage("assetBundleDepends", obj);
                }
            }
            bundle.assetBundle.Unload(false);
        }
        else
        {
            Application.Quit(); // TODO: Show an proper error and than exit the game
        }
    }

}
