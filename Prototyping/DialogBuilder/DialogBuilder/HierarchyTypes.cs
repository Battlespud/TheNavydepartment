using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogBuilder
{
    public enum HierarchyTypes : byte
    {
        /// <summary>
        /// Absolutely beginning of the tree
        /// </summary>
        Root,
        /// <summary>
        /// next main extension from the root
        /// </summary>
        Trunk,
        /// <summary>
        /// next extension from the trunk
        /// </summary>
        Limb,
        /// <summary>
        /// next extension from the limb
        /// </summary>
        Branch,
        /// <summary>
        /// next extension from the branch
        /// </summary>
        Twig,
        /// <summary>
        /// extension from the twig, bottom most level (for now)
        /// </summary>
        Leaf
    }
}
