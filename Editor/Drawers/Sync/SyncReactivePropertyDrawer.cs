using UnityEditor;

namespace MVVM.Editor
{
    [CustomPropertyDrawer(typeof(SyncReactiveProperty<>))]
    public class SyncReactivePropertyDrawer : BaseSyncPropertyDrawer
    {
    }
    
    [CustomPropertyDrawer(typeof(SyncReactiveEvent))]
    public class SyncReactiveEventDrawer : BaseSyncPropertyDrawer
    {
    }
}