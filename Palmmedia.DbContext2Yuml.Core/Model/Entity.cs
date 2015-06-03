using System.Collections.ObjectModel;

namespace Palmmedia.DbContext2Yuml.Core.Model
{
    public class Entity
    {
        public Entity(string name)
        {
            this.Name = name;
            this.Properties = new Collection<Property>();
            this.Relations = new Collection<Relation>();
        }

        public string Name { get; private set; }

        public Collection<Property> Properties { get; private set; }

        public Collection<Relation> Relations { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
