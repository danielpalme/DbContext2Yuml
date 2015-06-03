namespace Palmmedia.DbContext2Yuml.Core.Model
{
    public class Relation
    {
        public Relation(string nameSourceToTarget, string nameTargetToSource, Entity source, Entity target, RelationType relationType)
        {
            this.NameSourceToTarget = nameSourceToTarget;
            this.NameTargetToSource = nameTargetToSource;
            this.Source = source;
            this.Target = target;
            this.RelationType = relationType;
        }

        public string NameSourceToTarget { get; private set; }

        public string NameTargetToSource { get; private set; }

        public Entity Source { get; private set; }

        public Entity Target { get; private set; }

        internal RelationType RelationType { get; private set; }

        public override string ToString()
        {
            if (this.RelationType == RelationType.InheritsFrom)
            {
                return string.Format("{0} -> {1}", this.Source.Name, this.Target.Name);
            }
            else
            {
                return string.Format("{0} <-> {1}", this.NameSourceToTarget, this.NameTargetToSource);
            }
        }
    }
}
