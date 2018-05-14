namespace FriendlyLocale.Parser.Nodes
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal abstract class YCollection<TModel> : YNode, IEnumerable<YNode>
    {
        private List<TModel> children;

        public YCollection(YNodeStyle style, params YNode[] content)
            : base(style)
        {
            this.Initialize(content);
        }
        
        internal List<TModel> Children => this.children ?? (this.children = new List<TModel>());

        protected abstract YNode FirstNode { get; }
        protected abstract YNode LastNode { get; }

        public abstract IEnumerator<YNode> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerable<YNode> Descendants()
        {
            foreach (var i in this)
            {
                yield return i;

                if (!(i is YCollection<TModel> container))
                {
                    continue;
                }

                foreach (var j in container.Descendants())
                {
                    yield return j;
                }
            }
        }

        protected internal abstract YNode GetPreviousNode(YNode node);
        protected internal abstract YNode GetNextNode(YNode node);
        public abstract void Add(params YNode[] content);
        public abstract void AddFirst(params object[] content);
        protected internal abstract void RemoveChild(YNode node);
        public abstract void RemoveNodes();

        public virtual void ReplaceNodes(params YNode[] content)
        {
            this.RemoveNodes();
            this.Add(content);
        }

        private void Initialize(params YNode[] content)
        {
            this.Add(content);
        }

        protected static IEnumerable<object> Flattern(IEnumerable<object> objects)
        {
            return objects.SelectMany(i =>
                i is YNode ? new[] {i} :
                i is IEnumerable<object> objs ? Flattern(objs) :
                i is IEnumerable enumerable ? Flattern(enumerable.Cast<object>()) : new[] {i});
        }
    }
}