
namespace MS.CommonUtils{
    using Profiler;

    /// <summary>
    /// 与ObjectPool的不同之处在于,当Request请求时，如果Pool为空，则会自动创建一个对象，并返回.
    /// </summary>
    public class AutoAllocatePool<T> : ObjectPool<T> where T: new()
    {
        public override T Request()
        {
            if(this.isEmpty){
                EditorPoolProfiler.TrackAllocate(this);
                return new T();
            }else{
                return base.Request();
            }
        }
    }
}
