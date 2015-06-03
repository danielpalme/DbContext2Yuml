using System;
using System.Collections.Generic;
using System.Text;
using Palmmedia.DbContext2Yuml.Core.Model;

namespace Palmmedia.DbContext2Yuml.Core
{
    /// <summary>
    /// Creates Yuml graphs from GIT History.
    /// </summary>
    public class YumlGraphBuilder : IYumlGraphBuilder
    {
        /// <summary>
        /// Creates the Yuml graph.
        /// </summary>
        /// <param name="pathToDll">The path to the DLL.</param>
        /// <returns>The Yuml graph.</returns>
        public string CreateYumlGraph(string pathToDll)
        {
            if (pathToDll == null)
            {
                throw new ArgumentNullException("pathToDll");
            }

            IEnumerable<Entity> entities = EntityExtractor.GetEntities(pathToDll);
            return CreateYumlGraph(entities);
        }

        /// <summary>
        /// Converts the commits into a Yuml graph.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>The Yuml graph.</returns>
        private static string CreateYumlGraph(IEnumerable<Entity> entities)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var entity in entities)
            {
                sb.AppendFormat("[{0}|{1}],\r\n", entity.Name, string.Join(";", entity.Properties));
            }

            foreach (var entity in entities)
            {
                foreach (var relation in entity.Relations)
                {
                    switch (relation.RelationType)
                    {
                        case RelationType.InheritsFrom:
                            sb.AppendFormat("[{0}]^-[{1}],\r\n", relation.Target.Name, relation.Source.Name);
                            break;
                        case RelationType.OneToOne:
                            sb.AppendFormat("[{0}]{1} 1 - {2} 1[{3}],\r\n", relation.Source.Name, relation.NameTargetToSource, relation.NameSourceToTarget, relation.Target.Name);
                            break;
                        case RelationType.OneToMany:
                            sb.AppendFormat("[{0}]{1} 1 - {2} *[{3}],\r\n", relation.Source.Name, relation.NameTargetToSource, relation.NameSourceToTarget, relation.Target.Name);
                            break;
                        case RelationType.ManyToMany:
                            sb.AppendFormat("[{0}]{1} *- {2} *[{3}],\r\n", relation.Source.Name, relation.NameTargetToSource, relation.NameSourceToTarget, relation.Target.Name);
                            break;
                        default:
                            break;
                    }
                }
            }

            return sb.ToString();
        }
    }
}
