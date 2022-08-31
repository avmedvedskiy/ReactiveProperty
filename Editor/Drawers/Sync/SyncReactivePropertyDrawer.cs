using UnityEditor;

namespace MVVM.Editor
{
    [CustomPropertyDrawer(typeof(SyncReactiveProperty<>))]
    public class SyncReactivePropertyDrawer : BaseSyncPropertyDrawer<IReactivePropertyValue>
    {
    }

    [CustomPropertyDrawer(typeof(SyncReactiveEvent))]
    public class SyncReactiveEventDrawer : BaseSyncPropertyDrawer<IReactiveEvent>
    {
    }

    [CustomPropertyDrawer(typeof(SyncReactiveList<>))]
    public class SyncReactiveListDrawer : BaseSyncPropertyDrawer<IReactiveList>
    {
    }
}