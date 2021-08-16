using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScenesMgr : BaseManager<ScenesMgr>
{
    // Start is called before the first frame update
    public void LoadScene(string name, UnityAction fun)
    {
        //场景同步加载
        SceneManager.LoadScene(name);
        //加载完成之后才会执行fun();
        fun();
    }
    public void LoadSceneAsyn(string name, UnityAction fun)
    {
        MonoManager.GetInstance().StartCoroutine(ReallyLoadSceneAsyn(name, fun));

    }
    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //可以得到场景加载的一个进度
        while (!ao.isDone)
        {
            //事件中心 向外分发 进度情况 外面想用就用
            EventCenter.GetInstance().EventTrigger("进度条更新", ao.progress);
            //这里面更新进度条
            yield return ao.progress;
        }
        yield return ao;
        //加载完成过后，才会执行这个方法fun()
        fun();
    }
}
