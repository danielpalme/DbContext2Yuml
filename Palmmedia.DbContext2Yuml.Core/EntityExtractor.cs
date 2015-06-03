using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Palmmedia.DbContext2Yuml.Core.Model;

namespace Palmmedia.DbContext2Yuml.Core
{
    /// <summary>
    /// Extracts entities from DLLs.
    /// </summary>
    internal static class EntityExtractor
    {
        /// <summary>
        /// Gets all entities from the given DLL.
        /// </summary>
        /// <param name="pathToDll">The path to the DLL.</param>
        /// <returns>The commits.</returns>
        public static IEnumerable<Entity> GetEntities(string pathToDll)
        {
            var assembly = Assembly.LoadFrom(pathToDll);

            var entities = assembly.GetTypes()
                .Where(t => t.GetProperties().Any(p => p.CustomAttributes.Any(a => a.AttributeType.Equals(typeof(KeyAttribute)))))
                .ToDictionary(t => t, t => new Entity(t.Name));

            foreach (var item in entities)
            {
                if (entities.ContainsKey(item.Key.BaseType))
                {
                    item.Value.Relations.Add(
                        new Relation(
                            null,
                            null,
                            item.Value,
                            entities[item.Key.BaseType],
                            RelationType.InheritsFrom));
                }

                foreach (var sourceProperty in item.Key.GetProperties())
                {
                    if (sourceProperty.Name.EndsWith("Id"))
                    {
                        continue;
                    }

                    if (sourceProperty.PropertyType.IsCollectionType())
                    {
                        // ManyToMany or OneToMany
                        Type targetType = sourceProperty.PropertyType.GenericTypeArguments[0];
                        Entity targetEntity = entities[targetType];

                        if (targetType.GetProperties().Any(p => p.PropertyType.IsCollectionType()
                            && p.PropertyType.GenericTypeArguments[0].Equals(item.Key)))
                        {
                            PropertyInfo targetProperty = targetType.GetProperties().First(p => p.PropertyType.IsCollectionType()
                            && p.PropertyType.GenericTypeArguments[0].Equals(item.Key));

                            if (targetEntity.Relations.Any(r => r.Source == targetEntity
                                && r.Target == item.Value && r.RelationType == RelationType.ManyToMany))
                            {
                                // Only one direction is added to the two related entities
                                continue;
                            }

                            // ManyToMany
                            item.Value.Relations.Add(
                                new Relation(
                                    sourceProperty.Name,
                                    targetProperty.Name,
                                    item.Value,
                                    targetEntity,
                                    RelationType.ManyToMany));
                        }
                        else if (targetType.GetProperties().Any(p => p.PropertyType.Equals(item.Key)))
                        {
                            PropertyInfo targetProperty = targetType.GetProperties().First(p => p.PropertyType.Equals(item.Key));

                            // OneToMany
                            targetEntity.Relations.Add(
                                    new Relation(
                                        sourceProperty.Name,
                                        targetProperty.Name,
                                        item.Value,
                                        targetEntity,
                                        RelationType.OneToMany));
                        }
                    }
                    else
                    {
                        if (entities.ContainsKey(sourceProperty.PropertyType))
                        {
                            // OneToOne or ManyToOne
                            Type targetType = sourceProperty.PropertyType;

                            PropertyInfo targetProperty = targetType.GetProperties().FirstOrDefault(p => p.PropertyType.Equals(item.Key));

                            if (targetProperty != null)
                            {
                                Entity targetEntity = entities[targetType];

                                if (targetEntity.Relations.Any(r => r.Source == targetEntity
                                    && r.Target == item.Value && r.RelationType == RelationType.OneToOne))
                                {
                                    // Only one direction is added to the two related entities
                                    continue;
                                }

                                // OneToOne
                                item.Value.Relations.Add(
                                    new Relation(
                                        sourceProperty.Name,
                                        targetProperty.Name,
                                        item.Value,
                                        targetEntity,
                                        RelationType.OneToOne));
                            }
                        }
                        else
                        {
                            // Property
                            item.Value.Properties.Add(new Property(sourceProperty.Name, sourceProperty.PropertyType.ToString()));
                        }
                    }
                }
            }

            return entities.Values;
        }
    }
}
