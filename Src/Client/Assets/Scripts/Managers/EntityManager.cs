using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    interface IEntityNotify
    {
        void OnEntityRemove();
        void OnEntityChanged(Entity entity);
        void OnEntityEvent(EntityEvent @event);
    }
    class EntityManager:Singleton<EntityManager>
    {
        Dictionary<int,Entity> entities = new Dictionary<int,Entity>();

        Dictionary<int,IEntityNotify> notifiers = new Dictionary<int,IEntityNotify>();

        public void RegisterEntityChangeNotify(int entityId , IEntityNotify notify)
        {
            notifiers[entityId] = notify;
        }  
        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }
        public void RemoveEntity(NEntity entity)
        {
            entities.Remove(entity.Id);
            if (notifiers.ContainsKey(entity.Id))
            {
                notifiers[entity.Id].OnEntityRemove();
                entities.Remove(entity.Id);
            }
        }

        internal void OnEntitySync(NEntitySync date)
        {
            Entity entity = null;
            entities.TryGetValue(date.Id, out entity);
            if (entity != null)
            {
                if (date.Entity != null)
                {
                    entity.EntityData = date.Entity;
                }
                if (notifiers.ContainsKey(date.Id))
                {
                    notifiers[entity.entityId].OnEntityChanged(entity);
                    notifiers[entity.entityId].OnEntityEvent(date.Event);
                }
            }
        }
    }
}
