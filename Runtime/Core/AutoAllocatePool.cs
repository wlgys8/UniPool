
namespace MS.CommonUtils{
    using Profiler;

    /// <summary>
    /// 与ObjectPool的不同之处在于,当Request请求时，如果Pool为空，则会自动创建一个对象，并返回.
    /// </summary>
    public class AutoAllocatePool<T> : ObjectPool<T> where T: new()
    {

        /// <summary>
        /// 在Pool中预先生成一定数量的对象
        /// </summary>
        public void PreAllocate(int count){
            for(var i = 0; i < count; i ++){
                EditorPoolProfiler.TrackAllocate(this);
                Release(new T());
            }
        }

        
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
