namespace Palmmedia.DbContext2Yuml.Core.Model
{
    public class Property
    {
        public Property(string name, string typeName)
        {
            this.Name = name;
            this.TypeName = typeName;
        }

        public string Name { get; private set; }

        public string TypeName { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
