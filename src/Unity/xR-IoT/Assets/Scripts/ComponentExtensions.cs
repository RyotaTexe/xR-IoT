using UnityEngine;
using Cysharp.Threading.Tasks;

namespace xRIoT.Util
{
    public static class ComponentExtensions
    {
        public static async UniTask<GameObject> ResourcesLoadAsync(this string path)
            => await Resources.LoadAsync(path) as GameObject;

        public static async UniTask<T> ResourcesLoadAsync<T>(this string path) where T : Object
            => await Resources.LoadAsync(path) as T;

        public static async UniTask<T> ResourcesLoadAsyncSafe<T>(this string path) where T : Object
        {
            var obj = await path.ResourcesLoadAsync<T>();
            if(obj == null) { Debug.LogWarning($"{path} is not exist."); }
            return obj;
        }
    }
}
