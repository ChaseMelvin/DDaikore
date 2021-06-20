using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public interface ICondition<T>
    {
        public T Evaluate(ConditionContext context);
    }

    /// <summary>
    /// A node of a condition tree. Every class that inherits from Condition should also implement ICondition&lt;bool&gt; or ICondition&lt;double&gt;.
    /// </summary>
    public abstract class Condition
    {
        /// <summary>
        /// Get the tree as a list. Implementation detail: this is a postorder traversal (spits out children first, then the root) but there's currently no requirement that it be in that order.
        /// </summary>
        public List<Condition> FlattenTree()
        {
            var childTrees = Children.SelectMany(p => p.FlattenTree()).ToList();
            childTrees.Add(this);
            return childTrees;
        }

        /// <summary>
        /// Make a *partial* deep copy of the condition tree. Children, siblings, and cousins of the given node argument will NOT be cloned.
        /// </summary>
        public Condition Clone(IEnumerable<Condition> cloneTheseNodesAndTheirAncestors)
        {
            //Clone (or don't clone, depending) the children
            var childrenClones = Children.ConvertAll(p => p.Clone(cloneTheseNodesAndTheirAncestors));

            //If no returned child is an ACTUAL clone and THIS node isn't the one we're looking for, we don't need to clone at all. Assumes we don't override the default equals comparator for any Condition class.
            if (Children.SequenceEqual(childrenClones) && !cloneTheseNodesAndTheirAncestors.Contains(this)) return this;

            //Note: We must ensure every child only has structs and primitives. Otherwise, we'd have to define an overridable parameterless SemiDeepCopy method, too.
            var newMe = (Condition)MemberwiseClone();
            newMe.Children = childrenClones;
            return newMe;
        }

        public void AddChildren(params Condition[] newChildren)
        {
            foreach (var child in newChildren)
            {
                if (Children.Count == GetMaxChildren()) throw new InvalidOperationException("The condition has too many children.");
                if (GetAllowedChildType() != child.GetType()) throw new ArgumentException("The condition cannot have children of the specified type.");
                Children.Add(child);
            }
        }

        /// <summary>
        /// Get a list of the condition nodes in this tree that are based on an entity location. (You can then use the Clone method on the root node to get a new copy of the whole tree, then call this again so you can modify those cloned nodes.)
        /// </summary>
        public IEnumerable<ConditionLocationCheck> LocationConditions() => FlattenTree().OfType<ConditionLocationCheck>();

        public IEnumerable<Condition> GetChildren() => Children.AsEnumerable();
        /// <summary>
        /// Children that evaluate as doubles. Alias for Children.OfType&lt;ICondition&lt;double&gt;&gt;()
        /// </summary>
        public IEnumerable<ICondition<double>> DoubleChildren() => Children.OfType<ICondition<double>>();
        /// <summary>
        /// Children that evaluate as bools. Alias for Children.OfType&lt;ICondition&lt;bool&gt;&gt;()
        /// </summary>
        public IEnumerable<ICondition<bool>> BoolChildren() => Children.OfType<ICondition<bool>>();

        /// <summary>
        /// The one and only type of child allowed in this Condition node. This just helps us report better errors when we do something wrong.
        /// </summary>
        protected virtual Type GetAllowedChildType() => typeof(ICondition<double>);
        /// <summary>
        /// The number of children allowed in this Condition node. This just helps us report better errors when we do something wrong.
        /// </summary>
        protected virtual int GetMaxChildren() => int.MaxValue;
        /// <summary>
        /// The children of this node. This is protected so we can enforce the type and count limits with our own AddChild method.
        /// </summary>
        protected List<Condition> Children = new();
    }
}
