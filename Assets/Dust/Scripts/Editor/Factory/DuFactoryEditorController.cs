using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [InitializeOnLoad]
    public static class DuFactoryEditorController
    {
        private static HashSet<DuFactory> m_QueueUpdateFactory;

        static DuFactoryEditorController()
        {
            m_QueueUpdateFactory = new HashSet<DuFactory>();

            PrefabUtility.prefabInstanceUpdated += OnPrefabInstanceUpdated;
            EditorApplication.update += EditorUpdate;
        }

        static void OnPrefabInstanceUpdated(GameObject instance)
        {
            // Notice:
            // Yes. DuFactoryInstance has [DisallowMultipleComponent] flag, but if user ?SOMEHOW? added few components
            // to a single prefab, then require analyze of all this components!
            var duFactoryInstances = instance.GetComponents<DuFactoryInstance>();

            foreach (var duFactoryInstance in duFactoryInstances)
            {
                if (Dust.IsNull(duFactoryInstance) || Dust.IsNull(duFactoryInstance.parentFactory))
                    continue;

                var duFactory = duFactoryInstance.parentFactory;

                if (m_QueueUpdateFactory.Contains(duFactory))
                    continue;

                if (duFactory.autoRebuildOnPrefabUpdated == false)
                {
#if DUST_DEBUG_FACTORY_BUILDER
                    Dust.Debug.CheckpointWarning("Factory.Controller", "OnPrefabInstanceUpdated",
                        string.Format("Skip factory rebuilding [{0} (#{1})]", duFactory.gameObject.name, duFactory.transform.GetInstanceID()));
#endif
                    continue;
                }

                m_QueueUpdateFactory.Add(duFactory);

#if DUST_DEBUG_FACTORY_BUILDER
                Dust.Debug.CheckpointWarning("Factory.Controller", "OnPrefabInstanceUpdated",
                    string.Format("Require rebuild factory [{0} (#{1})]", duFactory.gameObject.name, duFactory.transform.GetInstanceID()));
#endif
            }
        }

        static void EditorUpdate()
        {
            if (m_QueueUpdateFactory.Count == 0)
                return;

            foreach (var duFactory in m_QueueUpdateFactory)
            {
#if DUST_DEBUG_FACTORY_BUILDER
                Dust.Debug.CheckpointWarning("Factory.Controller", "UpdateParentFactory",
                    string.Format("Rebuild [{0} (#{1})]", duFactory.gameObject.name, duFactory.transform.GetInstanceID()));
#endif

                duFactory.RebuildInstances();
            }

            m_QueueUpdateFactory.Clear();
        }
    }
}
